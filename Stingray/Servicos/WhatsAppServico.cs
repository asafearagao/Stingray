using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;                  // Biblioteca para requisições HTTP
using System.IO;
using System.Web.Script.Serialization;
using System.Threading;                 // Cancelamento de tarefas assíncronas
using System.Threading.Tasks;           // Tarefas assíncronas
using System.Diagnostics;
using Stingray.Repositorios;
using Stingray.DadosTela;
using OpenQA.Selenium;                  // Automação de navegadores
using OpenQA.Selenium.Chrome;           // Automação do Google Chrome
using OpenQA.Selenium.Edge;             // Automação do Microsoft Edge
using OpenQA.Selenium.Support.UI;       // Espera de elementos

namespace Stingray.Servicos {

    // Coordena a sessão, a preparação e o envio autorizado de cobranças pelo WhatsApp
    public static class WhatsAppServico {

        // Guarda os dados de uma cobrança enquanto ela percorre a fila
        private sealed class Mensagem {
            public Guid Id;
            public int IdMensalidade;
            public string Telefone;
            public string Texto;
            public string Estado;
            public bool EnvioAutorizado;
        }

        private static readonly BlockingCollection<Action> Comandos = new BlockingCollection<Action>();
        private static readonly Queue<Mensagem> Fila = new Queue<Mensagem>();
        private static readonly ConcurrentDictionary<Guid, Mensagem> Mensagens = new ConcurrentDictionary<Guid, Mensagem>();
        private static readonly HttpClient Http = new HttpClient();
        private static readonly object Sincronizacao = new object();

        private static IWebDriver navegador;
        private static Thread threadServico;
        private static volatile bool executando;
        private static volatile bool conexaoInternet;
        private static volatile bool conexaoWhatsApp;
        private static volatile bool sessaoConfirmadaAgora;
        private static volatile bool whatsappAtivado;
        private static string diretorioPerfilTemporario;
        private static readonly WhatsAppConfiguracaoRepositorio repositorioConfiguracao = new WhatsAppConfiguracaoRepositorio();
        private static readonly WhatsAppEnvioRepositorio repositorioEnvios = new WhatsAppEnvioRepositorio();
        private static bool filaCarregada;
        private static DateTime proximaVerificacaoCobrancas = DateTime.MinValue;
        private static TaskCompletionSource<bool> encerramentoPendente;
        private static volatile string ultimoErro;

        public static string UltimoErro {
            get { return ultimoErro; }
        }

        // Recupera a fila depois que o banco de dados estiver disponível
        public static void Inicializar() {
            try {
                CarregarConfiguracoes();
                if (whatsappAtivado)
                    IniciarServico();
            } catch (Exception erro) {
                ultimoErro = "Não foi possível inicializar a integração do WhatsApp: " + erro.Message;
            }
        }

        static WhatsAppServico() {
            Http.Timeout = TimeSpan.FromSeconds(5);
            LimparPerfisAbandonados();
            CarregarConfiguracoes();
        }

        // Representa o formato de configuração usado nas versões anteriores
        private sealed class ConfiguracoesPersistidasAntigas {
            public bool WhatsAppAtivado { get; set; }
            public bool WhatsAppConectado { get; set; }
        }

        // Carrega as preferências persistidas da integração
        private static void CarregarConfiguracoes() {
            try {
                EstadoWhatsApp estado = repositorioConfiguracao.ObterEstado();
                whatsappAtivado = estado.IntegracaoAtivada;
                MigrarConfiguracaoAntigaSeExistir(estado);

                // A sessão do navegador é temporária e não fica salva no computador
                // Por isso, uma conexão anterior precisa ser autenticada novamente
                // A conexão atual só é considerada ativa depois de uma verificação nesta execução
                // O último estado confirmado permanece salvo no MySQL como histórico
                conexaoWhatsApp = false;
                LimparPerfisPersistentesAntigos();

            } catch (Exception erro) {
                whatsappAtivado = false;
                conexaoWhatsApp = false;
                ultimoErro = "Não foi possível carregar o estado do WhatsApp no banco de dados: " + erro.Message;
            }
        }

        // Importa as preferências do formato antigo quando necessário
        private static void MigrarConfiguracaoAntigaSeExistir(EstadoWhatsApp estadoBanco) {
            string caminho = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Stingray", "configuracoes.json");
            if (!File.Exists(caminho))
                return;

            string json = File.ReadAllText(caminho);
            ConfiguracoesPersistidasAntigas antiga = new JavaScriptSerializer()
                .Deserialize<ConfiguracoesPersistidasAntigas>(json);
            if (antiga != null && !estadoBanco.IntegracaoAtivada && !estadoBanco.UltimoEstadoConectado) {
                whatsappAtivado = antiga.WhatsAppAtivado;
                repositorioConfiguracao.SalvarEstado(whatsappAtivado, false);
            }

            // Remove o arquivo antigo somente depois que o estado foi transferido para o banco
            File.Delete(caminho);
            string pasta = Path.GetDirectoryName(caminho);
            if (Directory.Exists(pasta) && Directory.GetFiles(pasta).Length == 0 && Directory.GetDirectories(pasta).Length == 0)
                Directory.Delete(pasta);
        }

        // Remove perfis temporários deixados por execuções anteriores
        private static void LimparPerfisPersistentesAntigos() {
            string pasta = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Stingray");
            string[] perfis = {
                Path.Combine(pasta, "WhatsAppChrome"),
                Path.Combine(pasta, "WhatsAppEdge")
            };
            foreach (string perfil in perfis) {
                if (Directory.Exists(perfil)) {
                    try { Directory.Delete(perfil, true); } catch { }
                }
            }
        }

        // Persiste as preferências atuais do WhatsApp
        private static void SalvarConfiguracoes() {
            repositorioConfiguracao.SalvarEstado(whatsappAtivado, conexaoWhatsApp);
        }

        public static bool ConexaoInternet {
            get { return conexaoInternet; }
        }

        public static bool ConexaoWhatsApp {
            get { return conexaoWhatsApp; }
        }

        // Indica se o estado do WhatsApp foi confirmado agora, durante uma verificação online
        public static bool SessaoConfirmadaAgora {
            get { return sessaoConfirmadaAgora; }
        }

        public static bool EstaAtivado() {
            return whatsappAtivado;
        }

        public static int ObterDiasAposVencimento() {
            return repositorioConfiguracao.ObterEstado().DiasAposVencimento;
        }

        // Atualiza o prazo usado pelas cobranças automáticas e manuais
        public static void SalvarDiasAposVencimento(int dias) {
            repositorioConfiguracao.SalvarDiasAposVencimento(dias);
            proximaVerificacaoCobrancas = DateTime.MinValue;
        }

        // Liga ou desliga a integração do WhatsApp Web
        public static void DefinirAtivado(bool ativado) {
            bool conectado = ativado && conexaoWhatsApp;
            repositorioConfiguracao.SalvarEstado(ativado, conectado);
            whatsappAtivado = ativado;
            conexaoWhatsApp = conectado;
            if (!ativado)
                sessaoConfirmadaAgora = false;
            else
                IniciarServico();
        }

        private static void DefinirEstadoConexao(bool conectado) {
            if (conexaoWhatsApp == conectado)
                return;
            repositorioConfiguracao.SalvarEstado(whatsappAtivado, conectado);
            conexaoWhatsApp = conectado;
        }

        // Inicia o processamento da fila de cobranças
        private static void IniciarServico() {
            lock (Sincronizacao) {
                if (threadServico != null && threadServico.IsAlive)
                    return;

                executando = true;
                threadServico = new Thread(ProcessarComandos);
                threadServico.IsBackground = true;
                threadServico.Name = "WhatsAppServico";
                threadServico.Start();
            }
        }

        // Confere a conectividade antes de preparar o navegador
        private static bool VerificarInternetInterno() {
            // Não depender de um único domínio: sites podem bloquear acessos por scripts
            string[] enderecos = new string[] {
                "https://web.whatsapp.com/",
                "https://www.google.com/generate_204",
                "https://www.microsoft.com/"
            };

            foreach (string endereco in enderecos) {
                try {
                    using (HttpResponseMessage resposta = Http.GetAsync(endereco).GetAwaiter().GetResult()) {
                        if (resposta.IsSuccessStatusCode)
                            return true;
                    }

                } catch {
                    // Tenta o próximo endereço antes de declarar ausência de internet
                }
            }

            return false;
        }

        // Só verifica acesso à internet; não abre o navegador
        public static async Task<bool> VerificarInternetAsync() {
            conexaoInternet = await Task.Run(() => VerificarInternetInterno());
            if (!conexaoInternet) {
                sessaoConfirmadaAgora = false;
                ultimoErro = "Não foi possível acessar os endereços de teste de internet. " +
                    "Verifique a conexão, o proxy ou o firewall.";

            } else {
                ultimoErro = null;
            }

            return conexaoInternet;
        }

        // Abre o WhatsApp Web para leitura manual do QR Code

        // O retorno informa se foi possível abrir o navegador
        public static Task<bool> PrepararWhatsAppAsync() {
            IniciarServico();
            TaskCompletionSource<bool> resultado = new TaskCompletionSource<bool>();

            Comandos.Add(delegate {
                try {
                    ultimoErro = null;

                    if (navegador == null) {
                        navegador = CriarNavegador();
                    }

                    navegador.Navigate().GoToUrl("https://web.whatsapp.com/");
                    conexaoInternet = VerificarInternetInterno();
                    bool? estadoDetectado = DetectarEstadoAutenticacao(navegador);
                    if (conexaoInternet && estadoDetectado.HasValue) {
                        DefinirEstadoConexao(estadoDetectado.Value);
                        sessaoConfirmadaAgora = true;

                    } else {
                        sessaoConfirmadaAgora = false;
                    }

                    resultado.TrySetResult(true);
                } catch (Exception ex) {
                    sessaoConfirmadaAgora = false;
                    // Uma falha ao iniciar o navegador não prova que a conta saiu do WhatsApp
                    ultimoErro = ex.ToString();

                    // Se a inicialização falhou parcialmente, permiti uma nova tentativa no próximo clique
                    try {
                        if (navegador != null)
                            navegador.Quit();
                    } catch { }

                    try {
                        if (navegador != null) navegador.Dispose();

                    } catch { }
                    navegador = null;

                    resultado.TrySetResult(false);
                }
            });

            return resultado.Task;
        }

        private static string CriarPerfilTemporario(string navegadorNome) {
            string pastaProcesso = Path.Combine(Path.GetTempPath(), "StingrayWhatsAppRuntime",
                "pid-" + Process.GetCurrentProcess().Id.ToString());
            string perfil = Path.Combine(pastaProcesso, navegadorNome + "-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(perfil);
            diretorioPerfilTemporario = pastaProcesso;
            return perfil;
        }

        private static void LimparPerfisAbandonados() {
            string raiz = Path.Combine(Path.GetTempPath(), "StingrayWhatsAppRuntime");
            if (!Directory.Exists(raiz))
                return;

            foreach (string pasta in Directory.GetDirectories(raiz, "pid-*")) {
                string nome = Path.GetFileName(pasta);
                int processoId;
                if (!nome.StartsWith("pid-", StringComparison.OrdinalIgnoreCase) ||
                    !int.TryParse(nome.Substring(4), out processoId))
                    continue;

                try {
                    using (Process processo = Process.GetProcessById(processoId)) {
                        if (!processo.HasExited)
                            continue;
                    }
                } catch {
                    try { Directory.Delete(pasta, true); } catch { }
                }
            }
        }

        private static void LimparPerfilTemporario() {
            string pasta = diretorioPerfilTemporario;
            diretorioPerfilTemporario = null;
            if (String.IsNullOrWhiteSpace(pasta))
                return;
            try {
                if (Directory.Exists(pasta))
                    Directory.Delete(pasta, true);
                string raiz = Directory.GetParent(pasta) == null ? null : Directory.GetParent(pasta).FullName;
                if (raiz != null && Directory.Exists(raiz) && Directory.GetDirectories(raiz).Length == 0)
                    Directory.Delete(raiz, false);
            } catch {
                // O perfil temporário será removido na próxima inicialização se o processo encerrar inesperadamente
            }
        }

        // Usa Selenium Manager para localizar/preparar o driver automaticamente

        // Tenta Chrome primeiro; se não estiver instalado ou não iniciar, tenta o Edge
        private static IWebDriver CriarNavegador() {
            Exception erroChrome;

            try {
                string perfilChrome = CriarPerfilTemporario("Chrome");
                ChromeOptions chrome = new ChromeOptions();
                chrome.AddArgument("--user-data-dir=" + perfilChrome);
                chrome.AddArgument("--no-first-run");
                chrome.AddArgument("--no-default-browser-check");
                chrome.AddArgument("--start-maximized");
                return new ChromeDriver(chrome);

            } catch (Exception ex) {
                erroChrome = ex;
            }

            try {
                string perfilEdge = CriarPerfilTemporario("Edge");
                EdgeOptions edge = new EdgeOptions();
                edge.AddArgument("--user-data-dir=" + perfilEdge);
                edge.AddArgument("--no-first-run");
                edge.AddArgument("--no-default-browser-check");
                edge.AddArgument("--start-maximized");
                return new EdgeDriver(edge);

            } catch (Exception erroEdge) {
                LimparPerfilTemporario();
                throw new InvalidOperationException(
                    "Não foi possível iniciar o Google Chrome nem o Microsoft Edge. " +
                    "Confirme que pelo menos um desses navegadores está instalado e " +
                    "que há internet para o Selenium Manager obter o driver, se necessário.\r\n\r\n" +
                    "Erro do Chrome: " + erroChrome.Message + "\r\n\r\n" +
                    "Erro do Edge: " + erroEdge.Message,
                    new AggregateException(erroChrome, erroEdge));
            }
        }

        // Confere a autenticação e o estado atual do WhatsApp Web
        public static Task<bool> VerificarWhatsAppAsync() {
            IniciarServico();
            TaskCompletionSource<bool> resultado = new TaskCompletionSource<bool>();

            Comandos.Add(delegate {
                try {
                    ultimoErro = null;
                    conexaoInternet = VerificarInternetInterno();

                    if (!conexaoInternet) {
                        sessaoConfirmadaAgora = false;
                        ultimoErro = "A internet está indisponível. O último estado do WhatsApp foi preservado.";
                        resultado.TrySetResult(false);
                        return;
                    }

                    if (navegador == null) {
                        sessaoConfirmadaAgora = false;
                        ultimoErro = "O navegador do WhatsApp ainda não foi aberto; não é possível confirmar a sessão neste momento.";
                        resultado.TrySetResult(false);
                        return;
                    }

                    bool? estadoDetectado = DetectarEstadoAutenticacao(navegador);

                    if (estadoDetectado.HasValue) {
                        DefinirEstadoConexao(estadoDetectado.Value);
                        sessaoConfirmadaAgora = true;
                        resultado.TrySetResult(estadoDetectado.Value);

                    } else {
                        // Estado indefinido: por exemplo, WhatsApp Web está reconectando
                        sessaoConfirmadaAgora = false;
                        ultimoErro = "A sessão do WhatsApp ainda está carregando ou reconectando. Tente verificar novamente.";
                        resultado.TrySetResult(false);
                    }

                } catch (Exception ex) {
                    sessaoConfirmadaAgora = false;
                    // Erro de rede não significa logout, logo conexaoWhatsApp não é alterada
                    ultimoErro = ex.ToString();
                    resultado.TrySetResult(false);
                }
            });

            return resultado.Task;
        }

        // Registra a cobrança e libera o envio somente após o clique explícito no WhatsApp
        public static Task<Guid?> EnviarCobrancaAsync(int idMensalidade, string telefone, string mensagem) {
            if (idMensalidade <= 0) {
                ultimoErro = "A mensalidade selecionada não é válida.";
                return Task.FromResult<Guid?>(null);
            }

            if (!whatsappAtivado) {
                ultimoErro = "A integração do WhatsApp está desativada.";
                return Task.FromResult<Guid?>(null);
            }

            if (String.IsNullOrWhiteSpace(telefone)) {
                ultimoErro = "Informe o telefone com código do país, DDD e número.";
                return Task.FromResult<Guid?>(null);
            }

            if (String.IsNullOrWhiteSpace(mensagem)) {
                ultimoErro = "A mensagem não pode ficar vazia.";
                return Task.FromResult<Guid?>(null);
            }

            string telefoneNormalizado = new string(telefone.Where(Char.IsDigit).ToArray());
            if (telefoneNormalizado.Length < 10 || telefoneNormalizado.Length > 15) {
                ultimoErro = "O telefone parece inválido. Use país + DDD + número.";
                return Task.FromResult<Guid?>(null);
            }

            try {
                repositorioEnvios.Inicializar();
                Guid id;
                string situacaoExistente;
                bool registrado = repositorioEnvios.TentarRegistrar(idMensalidade,
                    telefoneNormalizado, mensagem, out id, out situacaoExistente);

                // Uma cobrança automática já preparada pode ser autorizada pelo clique sem duplicar o registro
                if (!registrado && !string.Equals(situacaoExistente, "Pendente", StringComparison.OrdinalIgnoreCase)) {
                    ultimoErro = "Esta mensalidade já possui um registro de cobrança no WhatsApp " +
                        "com situação '" + situacaoExistente + "'. Para evitar duplicidade, ela não foi adicionada novamente.";
                    return Task.FromResult<Guid?>(null);
                }

                if (!registrado)
                    repositorioEnvios.AtualizarPendente(id, telefoneNormalizado, mensagem);

                // A fila e o estado em memória são alterados apenas pela thread do serviço
                IniciarServico();
                Guid idAutorizado = id;
                int mensalidadeAutorizada = idMensalidade;
                string telefoneAutorizado = telefoneNormalizado;
                string mensagemAutorizada = mensagem;
                Comandos.Add(delegate {
                    Mensagem item;
                    if (!Mensagens.TryGetValue(idAutorizado, out item)) {
                        item = new Mensagem {
                            Id = idAutorizado,
                            IdMensalidade = mensalidadeAutorizada,
                            Telefone = telefoneAutorizado,
                            Texto = mensagemAutorizada,
                            Estado = "Pendente"
                        };
                        Mensagens[idAutorizado] = item;
                    } else {
                        item.IdMensalidade = mensalidadeAutorizada;
                        item.Telefone = telefoneAutorizado;
                        item.Texto = mensagemAutorizada;
                        item.Estado = "Pendente";
                    }

                    item.EnvioAutorizado = true;
                    AdicionarFilaSeNaoExiste(item);
                });
                ultimoErro = null;
                return Task.FromResult<Guid?>(idAutorizado);

            } catch (Exception ex) {
                ultimoErro = ex.Message;
                return Task.FromResult<Guid?>(null);
            }
        }

        private static void AdicionarFilaSeNaoExiste(Mensagem item) {
            if (item == null || Fila.Any(pendente => pendente.Id == item.Id))
                return;
            Fila.Enqueue(item);
        }

        // Recupera mensagens pendentes registradas no banco
        private static void CarregarFilaPersistida() {
            repositorioEnvios.Inicializar();
            repositorioEnvios.RecuperarEnviosInterrompidos();
            foreach (WhatsAppEnvio registro in repositorioEnvios.ListarPendentes()) {
                Mensagem item;
                if (!Mensagens.TryGetValue(registro.IdEnvio, out item)) {
                    item = new Mensagem {
                        Id = registro.IdEnvio,
                        IdMensalidade = registro.IdMensalidade,
                        Telefone = registro.Telefone,
                        Texto = registro.Mensagem,
                        Estado = "Pendente"
                    };
                    Mensagens[registro.IdEnvio] = item;
                } else {
                    item.IdMensalidade = registro.IdMensalidade;
                    item.Telefone = registro.Telefone;
                    item.Texto = registro.Mensagem;
                    item.Estado = "Pendente";
                }
                AdicionarFilaSeNaoExiste(item);
            }
            filaCarregada = true;
        }

        // Localiza mensalidades elegíveis e prepara cobranças sem liberar o envio automático
        private static void EnfileirarCobrancasVencidas() {
            EstadoWhatsApp configuracao = repositorioConfiguracao.ObterEstado();
            int dias = configuracao.DiasAposVencimento;
            if (dias < 1 || dias > 30)
                dias = 4;

            List<PagamentoLinha> pagamentos = new PagamentoServico().ListarTodos(DateTime.Today);
            foreach (PagamentoLinha pagamento in pagamentos) {
                if (pagamento.Situacao == "Pago" ||
                    !RegraCobrancaWhatsApp.EstaElegivel(pagamento.DataVencimento, DateTime.Today, dias))
                    continue;

                string telefone;
                string mensagem;
                string motivo;
                if (!CobrancaWhatsAppMontador.TentarMontar(pagamento, DateTime.Today,
                    out telefone, out mensagem, out motivo))
                    continue;

                Guid id;
                string situacaoExistente;
                if (!repositorioEnvios.TentarRegistrar(pagamento.IdMensalidade,
                    telefone, mensagem, out id, out situacaoExistente))
                    continue;

                Mensagem item = new Mensagem {
                    Id = id,
                    IdMensalidade = pagamento.IdMensalidade,
                    Telefone = telefone,
                    Texto = mensagem,
                    Estado = "Pendente",
                    EnvioAutorizado = false
                };
                Mensagens[id] = item;
                AdicionarFilaSeNaoExiste(item);
            }
        }

        // Retorna a situação persistida de uma cobrança
        public static string ConsultarEstado(Guid id) {
            Mensagem item;
            return Mensagens.TryGetValue(id, out item)
                ? item.Estado
                : "Mensagem não encontrada";
        }

        // Executa os comandos recebidos pelo serviço de WhatsApp
        private static void ProcessarComandos() {
            DateTime proximaVerificacao = DateTime.MinValue;
            DateTime proximaTentativaCarga = DateTime.MinValue;

            while (executando) {
                try {
                    Action comando;
                    if (Comandos.TryTake(out comando, 500))
                        comando();

                    if (!executando)
                        break;

                    if (!filaCarregada && DateTime.UtcNow >= proximaTentativaCarga) {
                        try {
                            CarregarFilaPersistida();
                        } catch (Exception erroCarga) {
                            ultimoErro = "Não foi possível recuperar a fila de cobranças: " + erroCarga.Message;
                            proximaTentativaCarga = DateTime.UtcNow.AddSeconds(30);
                        }
                    }

                    // A preparação das cobranças ocorre em segundo plano, mas não autoriza o envio
                    if (whatsappAtivado && DateTime.UtcNow >= proximaVerificacaoCobrancas) {
                        proximaVerificacaoCobrancas = DateTime.UtcNow.AddMinutes(5);
                        try {
                            EnfileirarCobrancasVencidas();
                        } catch (Exception erroCobranca) {
                            ultimoErro = "Não foi possível verificar as cobranças vencidas: " + erroCobranca.Message;
                        }
                    }

                    if (DateTime.UtcNow >= proximaVerificacao) {
                        conexaoInternet = VerificarInternetInterno();
                        proximaVerificacao = DateTime.UtcNow.AddSeconds(10);
                    }

                    if (!conexaoInternet || navegador == null) {
                        sessaoConfirmadaAgora = false;
                        continue;
                    }

                    bool? estadoDetectado = DetectarEstadoAutenticacao(navegador);
                    if (estadoDetectado.HasValue) {
                        DefinirEstadoConexao(estadoDetectado.Value);
                        sessaoConfirmadaAgora = true;
                    } else {
                        sessaoConfirmadaAgora = false;
                    }

                    if (whatsappAtivado && conexaoInternet && sessaoConfirmadaAgora && conexaoWhatsApp) {
                        // O processamento normal atende somente cobranças liberadas pelo clique no WhatsApp
                        Mensagem proximaMensagem = ObterProximaMensagemParaEnvio(false);
                        if (proximaMensagem != null)
                            EnviarProximaMensagem(proximaMensagem);
                    }
                }
                catch (Exception ex) {
                    sessaoConfirmadaAgora = false;
                    ultimoErro = ex.ToString();
                }
            }

            TaskCompletionSource<bool> finalizacao;
            lock (Sincronizacao) {
                if (threadServico == Thread.CurrentThread)
                    threadServico = null;
                finalizacao = encerramentoPendente;
                encerramentoPendente = null;
            }
            if (finalizacao != null)
                finalizacao.TrySetResult(true);
        }

        // Identifica se o WhatsApp Web aguarda leitura do QR ou já está conectado
        private static bool? DetectarEstadoAutenticacao(IWebDriver driver) {
            if (driver == null)
                return null;

            try {
                // Se a lista de conversas existe, a sessão está autenticada
                bool listaConversas = driver.FindElements(By.Id("pane-side"))
                    .Any(elemento => elemento.Displayed);
                if (!listaConversas) {
                    listaConversas = driver.FindElements(
                        By.CssSelector("[data-testid='chat-list']"))
                        .Any(elemento => elemento.Displayed);
                }

                if (listaConversas) {
                    // O aviso temporário de reconexão à internet não é tratado como logout
                    return true;
                }

                // A tela de login do WhatsApp Web normalmente apresenta o QR Code
                // num canvas e/ou no elemento data-testid=qrcode
                bool qrCodeVisivel = driver.FindElements(
                    By.CssSelector("[data-testid='qrcode'], canvas"))
                    .Any(elemento => elemento.Displayed);
                if (qrCodeVisivel)
                    return false;

                try {
                    string textoPagina = driver.FindElement(By.TagName("body")).Text ?? String.Empty;
                    string texto = textoPagina.ToLowerInvariant();
                    if (texto.Contains("escaneie o código qr") ||
                        texto.Contains("escaneie este código qr") ||
                        texto.Contains("scan the qr code") ||
                        texto.Contains("use whatsapp on your phone"))
                        return false;

                } catch (WebDriverException) {

                }

                return null;

            } catch (WebDriverException) {
                // Se o driver falhar ao consultar a página, não assumir que houve logout
                return null;
            }
        }

        // Localiza a primeira cobrança autorizada, mantendo as demais aguardando confirmação
        private static Mensagem ObterProximaMensagemParaEnvio(bool encerrandoAplicativo) {
            return Fila.FirstOrDefault(item =>
                RegraCobrancaWhatsApp.PodeProcessarFila(item.EnvioAutorizado, encerrandoAplicativo));
        }

        // Remove uma cobrança específica sem descartar outras que ainda aguardam autorização
        private static void RemoverDaFila(Guid id) {
            if (Fila.Count == 0)
                return;

            Queue<Mensagem> restantes = new Queue<Mensagem>();
            while (Fila.Count > 0) {
                Mensagem atual = Fila.Dequeue();
                if (atual.Id != id)
                    restantes.Enqueue(atual);
            }

            while (restantes.Count > 0)
                Fila.Enqueue(restantes.Dequeue());
        }

        // Envia uma cobrança autorizada e atualiza seu estado persistente
        private static void EnviarProximaMensagem(Mensagem item) {
            if (item == null)
                return;

            item.Estado = "Em envio";

            if (new PagamentoRepositorio().EstaPago(item.IdMensalidade)) {
                repositorioEnvios.MarcarCancelado(item.Id, "A mensalidade foi registrada como paga antes do envio.");
                item.Estado = "Cancelada: mensalidade paga";
                RemoverDaFila(item.Id);
                return;
            }

            if (!repositorioEnvios.MarcarEmEnvio(item.Id)) {
                item.Estado = "Envio ignorado: registro não está pendente";
                RemoverDaFila(item.Id);
                return;
            }

            bool teclaEnvioAcionada = false;
            try {
                string url = "https://web.whatsapp.com/send?phone=" + item.Telefone;
                navegador.Navigate().GoToUrl(url);

                WebDriverWait espera = new WebDriverWait(navegador, TimeSpan.FromSeconds(30));
                IWebElement campo = espera.Until(driver =>
                    driver.FindElements(By.CssSelector("footer [contenteditable='true']"))
                        .FirstOrDefault(elemento => elemento.Displayed && elemento.Enabled));

                string[] linhas = item.Texto.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
                campo.Click();
                for (int i = 0; i < linhas.Length; i++) {
                    if (i > 0)
                        campo.SendKeys(Keys.Shift + Keys.Enter);
                    campo.SendKeys(linhas[i]);
                }

                teclaEnvioAcionada = true;
                campo.SendKeys(Keys.Enter);
                item.Estado = "Envio acionado (entrega não confirmada)";
                repositorioEnvios.MarcarEnviado(item.Id);

            } catch (Exception ex) {
                item.Estado = teclaEnvioAcionada
                    ? "Não confirmado: " + ex.Message
                    : "Falha antes do envio: " + ex.Message;
                try {
                    if (teclaEnvioAcionada)
                        repositorioEnvios.MarcarNaoConfirmado(item.Id, ex.ToString());
                    else
                        repositorioEnvios.MarcarFalhaAntesDoEnvio(item.Id, ex.ToString());
                } catch (Exception erroRegistro) {
                    ultimoErro = "Falha ao atualizar o registro de envio: " + erroRegistro.Message;
                }

            } finally {
                RemoverDaFila(item.Id);
            }
        }

        // Prepara e envia a fila pendente no encerramento, desde que a sessão já esteja autenticada
        private static void ProcessarFilaNoEncerramento() {
            if (!whatsappAtivado)
                return;

            try {
                if (!filaCarregada)
                    CarregarFilaPersistida();

                // Inclui cobranças vencidas geradas durante a sessão antes de fechar o aplicativo
                EnfileirarCobrancasVencidas();

                if (navegador == null)
                    return;

                conexaoInternet = VerificarInternetInterno();
                if (!conexaoInternet) {
                    sessaoConfirmadaAgora = false;
                    return;
                }

                bool? estadoDetectado = DetectarEstadoAutenticacao(navegador);
                if (!estadoDetectado.HasValue) {
                    sessaoConfirmadaAgora = false;
                    return;
                }

                DefinirEstadoConexao(estadoDetectado.Value);
                sessaoConfirmadaAgora = true;
                if (!estadoDetectado.Value)
                    return;

                // O fechamento é a autorização para processar todas as cobranças pendentes elegíveis
                Mensagem proximaMensagem;
                while ((proximaMensagem = ObterProximaMensagemParaEnvio(true)) != null)
                    EnviarProximaMensagem(proximaMensagem);

            } catch (Exception erro) {
                ultimoErro = "Não foi possível processar as cobranças pendentes ao encerrar o aplicativo: " + erro.Message;
            }
        }

        // Finaliza o navegador e encerra o processamento com segurança
        public static Task EncerrarAsync() {
            lock (Sincronizacao) {
                if (threadServico == null || !threadServico.IsAlive)
                    return Task.FromResult(true);

                if (encerramentoPendente != null)
                    return encerramentoPendente.Task;

                encerramentoPendente = new TaskCompletionSource<bool>(
                    TaskCreationOptions.RunContinuationsAsynchronously);
                Task tarefaFinalizacao = encerramentoPendente.Task;

                Comandos.Add(delegate {
                    try {
                        ProcessarFilaNoEncerramento();
                    } finally {
                        try {
                            if (navegador != null) {
                                navegador.Quit();
                                navegador.Dispose();
                                navegador = null;
                            }
                        } catch {
                            // O navegador pode já ter sido fechado manualmente
                        } finally {
                            sessaoConfirmadaAgora = false;
                            conexaoWhatsApp = false;
                            try { SalvarConfiguracoes(); } catch (Exception erro) { ultimoErro = erro.Message; }
                            LimparPerfilTemporario();
                            executando = false;
                        }
                    }
                });

                return tarefaFinalizacao;
            }
        }
    }
}
