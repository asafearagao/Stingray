using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;

namespace Stingray {
    // Teste para integração com o WhatsApp Web
    public static class WhatsAppService {
        private sealed class Mensagem {
            public Guid Id;
            public string Telefone;
            public string Texto;
            public string Estado;
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
        private static bool ultimoEstadoConectado;
        private static readonly string arquivoConfiguracoes = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Stingray", "configuracoes.json");
        private static TaskCompletionSource<bool> encerramentoPendente;
        private static volatile string ultimoErro;

        public static string UltimoErro {
            get { return ultimoErro; }
        }

        static WhatsAppService() {
            Http.Timeout = TimeSpan.FromSeconds(5);
            CarregarConfiguracoes();
        }

        private sealed class ConfiguracoesPersistidas {
            public bool WhatsAppAtivado {
                get;
                set;
            }
            public bool WhatsAppConectado {
                get;
                set;
            }
        }

        private static void CarregarConfiguracoes() {
            try {
                if (!File.Exists(arquivoConfiguracoes))
                    return;
                var json = File.ReadAllText(arquivoConfiguracoes);
                var config = new JavaScriptSerializer().Deserialize<ConfiguracoesPersistidas>(json);
                if (config == null)
                    return;
                whatsappAtivado = config.WhatsAppAtivado;
                ultimoEstadoConectado = config.WhatsAppConectado;
                conexaoWhatsApp = ultimoEstadoConectado;

            } catch {
                // Se o JSON estiver corrompido ou ausente, iniciar com valores seguros
                whatsappAtivado = false;
                ultimoEstadoConectado = false;
                conexaoWhatsApp = false;
            }
        }

        private static void SalvarConfiguracoes() {
            try {
                string pasta = Path.GetDirectoryName(arquivoConfiguracoes);
                Directory.CreateDirectory(pasta);
                var config = new ConfiguracoesPersistidas {
                    WhatsAppAtivado = whatsappAtivado,
                    WhatsAppConectado = conexaoWhatsApp
                };
                string json = new JavaScriptSerializer().Serialize(config);
                File.WriteAllText(arquivoConfiguracoes, json);
                ultimoEstadoConectado = conexaoWhatsApp;

            } catch {
                // A falha ao salvar preferências não deve derrubar o app
            }
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

        // Liga ou desliga a integração do WhatsApp Web
        public static void DefinirAtivado(bool ativado) {
            whatsappAtivado = ativado;
            // Desativar a integração significa sair da conta do WhatsApp
            if (!ativado)
                sessaoConfirmadaAgora = false;
            SalvarConfiguracoes();
        }

        private static void DefinirEstadoConexao(bool conectado) {
            conexaoWhatsApp = conectado;
            SalvarConfiguracoes();
        }

        private static void IniciarServico() {
            lock (Sincronizacao) {
                if (threadServico != null && threadServico.IsAlive)
                    return;

                executando = true;
                threadServico = new Thread(ProcessarComandos);
                threadServico.IsBackground = true;
                threadServico.Name = "WhatsAppService";
                threadServico.Start();
            }
        }

        private static bool VerificarInternetInterno() {
            // Não depender de um único domínio: redes corporativas podem bloquear acessos por scripts
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

        // Usa Selenium Manager para localizar/preparar o driver automaticamente
        // Tenta Chrome primeiro; se não estiver instalado ou não iniciar, tenta o Edge
        private static IWebDriver CriarNavegador() {
            Exception erroChrome;

            try {
                string perfilChrome = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Stingray", "WhatsAppChrome");
                Directory.CreateDirectory(perfilChrome);

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
                string perfilEdge = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Stingray", "WhatsAppEdge");
                Directory.CreateDirectory(perfilEdge);

                EdgeOptions edge = new EdgeOptions();
                edge.AddArgument("--user-data-dir=" + perfilEdge);
                edge.AddArgument("--no-first-run");
                edge.AddArgument("--no-default-browser-check");
                edge.AddArgument("--start-maximized");

                return new EdgeDriver(edge);

            } catch (Exception erroEdge) {
                throw new InvalidOperationException(
                    "Não foi possível iniciar o Google Chrome nem o Microsoft Edge. " +
                    "Confirme que pelo menos um desses navegadores está instalado e " +
                    "que há internet para o Selenium Manager obter o driver, se necessário.\r\n\r\n" +
                    "Erro do Chrome: " + erroChrome.Message + "\r\n\r\n" +
                    "Erro do Edge: " + erroEdge.Message,
                    new AggregateException(erroChrome, erroEdge));
            }
        }

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
                        // ou terminou de carregar. Mantemos conexaoWhatsApp inalterada
                        sessaoConfirmadaAgora = false;
                        ultimoErro = "A sessão do WhatsApp ainda está carregando ou reconectando. Tente verificar novamente.";
                        resultado.TrySetResult(false);
                    }

                } catch (Exception ex) {
                    sessaoConfirmadaAgora = false;
                    // Erro de rede/DOM não significa logout, logo conexaoWhatsApp não é alterada
                    ultimoErro = ex.ToString();
                    resultado.TrySetResult(false);
                }
            });

            return resultado.Task;
        }

        public static async Task<Guid?> EnviarMensagemAsync(string telefone, string mensagem) {
            if (String.IsNullOrWhiteSpace(telefone)) {
                ultimoErro = "Informe o telefone com código do país, DDD e número.";
                return null;
            }

            if (String.IsNullOrWhiteSpace(mensagem)) {
                ultimoErro = "A mensagem não pode ficar vazia.";
                return null;
            }

            string telefoneNormalizado = new string(telefone.Where(Char.IsDigit).ToArray());
            if (telefoneNormalizado.Length < 10 || telefoneNormalizado.Length > 15) {
                ultimoErro = "O telefone parece inválido. Use país + DDD + número.";
                return null;
            }

            bool autenticado = await VerificarWhatsAppAsync();
            if (!autenticado) {
                if (!conexaoInternet)
                    ultimoErro = "Não há conexão com a internet. A mensagem não foi enviada nem adicionada à fila.";
                else if (!sessaoConfirmadaAgora && conexaoWhatsApp)
                    ultimoErro = "O último estado do WhatsApp era conectado, mas a sessão ainda não pôde ser confirmada. Tente novamente em instantes.";
                else
                    ultimoErro = "O WhatsApp Web não está autenticado. Abra o navegador e leia o QR Code pelo celular. A mensagem não foi enviada nem adicionada à fila.";
                return null;
            }

            try {
                Guid id = EnfileirarMensagem(telefone, mensagem);
                ultimoErro = null;
                return id;

            } catch (Exception ex) {
                ultimoErro = ex.Message;
                return null;
            }
        }

        // Armazena uma mensagem na fila em memória, sem tentar enviá-la nesta chamada
        // O processador só a enviará depois, quando a integração estiver ativada,
        // houver internet e o WhatsApp Web estiver autenticado
        public static Guid EnfileirarMensagem(string telefone, string mensagem) {
            if (String.IsNullOrWhiteSpace(telefone))
                throw new ArgumentException("Informe o telefone com código do país e DDD.", "telefone");
            if (String.IsNullOrWhiteSpace(mensagem))
                throw new ArgumentException("A mensagem não pode ficar vazia.", "mensagem");

            telefone = new string(telefone.Where(Char.IsDigit).ToArray());
            if (telefone.Length < 10 || telefone.Length > 15)
                throw new ArgumentException("O telefone parece inválido. Use país + DDD + número.", "telefone");

            Mensagem item = new Mensagem();
            item.Id = Guid.NewGuid();
            item.Telefone = telefone;
            item.Texto = mensagem;
            item.Estado = "Pendente";
            Mensagens[item.Id] = item;

            IniciarServico();
            Comandos.Add(delegate { Fila.Enqueue(item); });
            return item.Id;
        }

        public static string ConsultarEstado(Guid id) {
            Mensagem item;
            return Mensagens.TryGetValue(id, out item)
                ? item.Estado
                : "Mensagem não encontrada";
        }

        private static void ProcessarComandos() {
            DateTime proximaVerificacao = DateTime.MinValue;

            while (executando) {
                try {
                    Action comando;
                    if (Comandos.TryTake(out comando, 500))
                        comando();

                    if (!executando)
                        break;

                    if (DateTime.UtcNow >= proximaVerificacao) {
                        conexaoInternet = VerificarInternetInterno();
                        proximaVerificacao = DateTime.UtcNow.AddSeconds(10);
                    }

                    if (!conexaoInternet || navegador == null) {
                        // Mantém conexaoWhatsApp como último estado conhecido; só suspende
                        // a confirmação atual e os envios até a conexão voltar
                        sessaoConfirmadaAgora = false;
                        continue;
                    }

                    bool? estadoDetectado = DetectarEstadoAutenticacao(navegador);
                    if (estadoDetectado.HasValue) {
                        DefinirEstadoConexao(estadoDetectado.Value);
                        sessaoConfirmadaAgora = true;
                    } else
       {
                        // Não confundir carregamento/reconexão temporária com logout
                        sessaoConfirmadaAgora = false;
                    }

                    if (whatsappAtivado && conexaoInternet && sessaoConfirmadaAgora &&
                        conexaoWhatsApp && Fila.Count > 0)
                        EnviarProximaMensagem();
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
                    // Um aviso temporário de reconexão à internet não é tratado como logout
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
                    // DOM ainda não pronto: a autenticação permanece indefinida
                }

                return null;

            } catch (WebDriverException) {
                // Se o driver falhar ao consultar a página, não assumir que houve logout
                return null;
            }
        }

        private static void EnviarProximaMensagem() {
            Mensagem item = Fila.Peek();
            item.Estado = "Enviando";

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

                campo.SendKeys(Keys.Enter);
                item.Estado = "Envio acionado (entrega não confirmada)";

            } catch (Exception ex) {
                item.Estado = "Falha: " + ex.Message;

            } finally {
                Fila.Dequeue();
            }
        }

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
                        if (navegador != null) {
                            navegador.Quit();
                            navegador.Dispose();
                            navegador = null;
                        }
                    } catch {
                        // O navegador pode já ter sido fechado manualmente
                    }
                    finally {
                        // Fechar a aplicação não equivale a sair da conta do WhatsApp
                        // O último estado conhecido permanece salvo no configuracoes.json
                        sessaoConfirmadaAgora = false;
                        executando = false;
                    }
                });

                return tarefaFinalizacao;
            }
        }
    }
}
