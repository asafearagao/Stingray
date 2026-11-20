using System;
using System.Collections.Generic;
using MySqlConnector;
using Stingray.BancoDeDados;
using Stingray.DadosTela;
using Stingray.Servicos;

namespace Stingray.Repositorios
{

    // Controla no banco os estados e a recuperação das cobranças enviadas
    public class WhatsAppEnvioRepositorio : RepositorioMySqlBase
    {

        // Confere se o esquema já foi provisionado pelo instalador
        public void Inicializar()
        {
            try
            {
                ConsultarValor("SELECT 1 FROM whatsapp_envio LIMIT 0;");
            }
            catch (MySqlException erro)
            {
                if (erro.Number != 1146)
                    throw;

                throw new InvalidOperationException(
                    "A tabela de controle do WhatsApp não existe no banco de dados. " +
                    "Execute o arquivo BancoDeDados\\Migracoes\\20261119_ControleWhatsApp.sql " +
                    "com uma conta administrativa do MySQL. O usuário stingray_app não precisa de permissão CREATE.",
                    erro);
            }
        }

        // Marca como não confirmados os envios interrompidos durante a execução
        public void RecuperarEnviosInterrompidos()
        {
            const string sql = @"
                UPDATE whatsapp_envio
                SET situacao = 'Não confirmado',
                    detalhe_erro = 'O sistema foi encerrado durante o envio. Confira o WhatsApp antes de qualquer nova tentativa.'
                WHERE situacao = 'Em envio';";
            ExecutarComando(sql);
        }

        // Registra a cobrança somente se ainda não houver envio equivalente
        public bool TentarRegistrar(int idMensalidade, string telefone, string mensagem,
            out Guid idEnvio, out string situacaoExistente)
        {
            Guid novoId = Guid.NewGuid();
            idEnvio = novoId;
            string idTexto = novoId.ToString("D");
            situacaoExistente = string.Empty;
            const string inserir = @"
                INSERT IGNORE INTO whatsapp_envio
                    (id_envio, id_mensalidade, telefone, mensagem, situacao)
                VALUES (@id, @mensalidade, @telefone, @mensagem, 'Pendente');";

            int inseridos = ExecutarComando(inserir, comando =>
            {
                comando.Parameters.Add("@id", MySqlDbType.VarChar).Value = idTexto;
                comando.Parameters.Add("@mensalidade", MySqlDbType.Int32).Value = idMensalidade;
                comando.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = telefone;
                comando.Parameters.Add("@mensagem", MySqlDbType.Text).Value = mensagem;
            });

            if (inseridos > 0)
                return true;

            WhatsAppEnvio existente = ConsultarUnico(@"
                SELECT id_envio, situacao FROM whatsapp_envio WHERE id_mensalidade = @mensalidade LIMIT 1;",
                comando => comando.Parameters.Add("@mensalidade", MySqlDbType.Int32).Value = idMensalidade,
                leitor => new WhatsAppEnvio
                {
                    IdEnvio = Guid.Parse(Convert.ToString(leitor["id_envio"])),
                    Situacao = Convert.ToString(leitor["situacao"])
                }, null);

            if (existente == null)
            {
                situacaoExistente = "Não foi possível verificar o registro anterior.";
                return false;
            }

            idEnvio = existente.IdEnvio;
            idTexto = existente.IdEnvio.ToString("D");
            situacaoExistente = existente.Situacao;
            if (!RegraCobrancaWhatsApp.PermiteNovaTentativa(existente.Situacao))
                return false;

            const string reativar = @"
                UPDATE whatsapp_envio
                SET telefone = @telefone, mensagem = @mensagem,
                    situacao = 'Pendente', detalhe_erro = NULL
                WHERE id_envio = @id AND situacao = 'Falha antes do envio';";
            int alterados = ExecutarComando(reativar, comando =>
            {
                comando.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = telefone;
                comando.Parameters.Add("@mensagem", MySqlDbType.Text).Value = mensagem;
                comando.Parameters.Add("@id", MySqlDbType.VarChar).Value = idTexto;
            });

            situacaoExistente = alterados > 0 ? "Pendente" : existente.Situacao;
            return alterados > 0;
        }

        // Recupera as mensagens que podem continuar na fila
        public List<WhatsAppEnvio> ListarPendentes()
        {
            return ConsultarLista(@"
                SELECT id_envio, id_mensalidade, telefone, mensagem, situacao
                FROM whatsapp_envio
                WHERE situacao = 'Pendente'
                ORDER BY criado_em, id_envio;", null, leitor => new WhatsAppEnvio
            {
                IdEnvio = Guid.Parse(Convert.ToString(leitor["id_envio"])),
                IdMensalidade = Convert.ToInt32(leitor["id_mensalidade"]),
                Telefone = Convert.ToString(leitor["telefone"]),
                Mensagem = Convert.ToString(leitor["mensagem"]),
                Situacao = Convert.ToString(leitor["situacao"])
            });
        }

        // Adquire o controle do envio antes de acionar o navegador
        public bool MarcarEmEnvio(Guid id)
        {
            return ExecutarComando(@"
                UPDATE whatsapp_envio SET situacao = 'Em envio', detalhe_erro = NULL
                WHERE id_envio = @id AND situacao = 'Pendente';",
                comando => comando.Parameters.Add("@id", MySqlDbType.VarChar).Value = id.ToString("D")) > 0;
        }

        // Registra que o envio foi concluído pelo fluxo do navegador
        public void MarcarEnviado(Guid id)
        {
            ExecutarComando(@"
                UPDATE whatsapp_envio SET situacao = 'Enviada', detalhe_erro = NULL
                WHERE id_envio = @id AND situacao = 'Em envio';",
                comando => comando.Parameters.Add("@id", MySqlDbType.VarChar).Value = id.ToString("D"));
        }

        // Bloqueia a repetição quando o resultado do envio é incerto
        public void MarcarNaoConfirmado(Guid id, string detalhe)
        {
            ExecutarComando(@"
                UPDATE whatsapp_envio SET situacao = 'Não confirmado', detalhe_erro = @detalhe
                WHERE id_envio = @id AND situacao = 'Em envio';",
                comando =>
                {
                    comando.Parameters.Add("@id", MySqlDbType.VarChar).Value = id.ToString("D");
                    comando.Parameters.Add("@detalhe", MySqlDbType.Text).Value = detalhe ?? string.Empty;
                });
        }

        // Registra o cancelamento de uma cobrança ainda não enviada
        public void MarcarCancelado(Guid id, string detalhe)
        {
            ExecutarComando(@"
                UPDATE whatsapp_envio SET situacao = 'Cancelada', detalhe_erro = @detalhe
                WHERE id_envio = @id AND situacao = 'Pendente';",
                comando =>
                {
                    comando.Parameters.Add("@id", MySqlDbType.VarChar).Value = id.ToString("D");
                    comando.Parameters.Add("@detalhe", MySqlDbType.Text).Value = detalhe ?? string.Empty;
                });
        }

        // Libera uma nova tentativa quando a falha ocorreu antes do envio
        public void MarcarFalhaAntesDoEnvio(Guid id, string detalhe)
        {
            ExecutarComando(@"
                UPDATE whatsapp_envio SET situacao = 'Falha antes do envio', detalhe_erro = @detalhe
                WHERE id_envio = @id AND situacao = 'Em envio';",
                comando =>
                {
                    comando.Parameters.Add("@id", MySqlDbType.VarChar).Value = id.ToString("D");
                    comando.Parameters.Add("@detalhe", MySqlDbType.Text).Value = detalhe ?? string.Empty;
                });
        }
    }
}
