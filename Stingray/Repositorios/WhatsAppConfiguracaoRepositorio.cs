using System;
using MySqlConnector;
using Stingray.BancoDeDados;
using Stingray.DadosTela;

namespace Stingray.Repositorios
{

    // Persiste as preferências e o estado da integração com o WhatsApp
    public class WhatsAppConfiguracaoRepositorio : RepositorioMySqlBase
    {
        private static readonly object SincronizacaoEstrutura = new object();
        private static bool estruturaGarantida;

        // Cria a estrutura de configuração quando ainda não existe
        private void GarantirEstrutura()
        {
            if (estruturaGarantida)
                return;

            lock (SincronizacaoEstrutura)
            {
                if (estruturaGarantida)
                    return;

                object existe = ConsultarValor(@"
                    SELECT COUNT(*) FROM information_schema.COLUMNS
                    WHERE TABLE_SCHEMA = DATABASE()
                      AND TABLE_NAME = 'configuracao_whatsapp'
                      AND COLUMN_NAME = 'dias_apos_vencimento';");
                if (Convert.ToInt32(existe) == 0)
                    ExecutarComando("ALTER TABLE configuracao_whatsapp ADD COLUMN dias_apos_vencimento INT NOT NULL DEFAULT 4 AFTER ultimo_estado_conectado;");

                estruturaGarantida = true;
            }
        }

        // Recupera o estado salvo da integração
        public EstadoWhatsApp ObterEstado()
        {
            GarantirEstrutura();
            const string sql = @"
                SELECT integracao_ativada, ultimo_estado_conectado, atualizado_em, dias_apos_vencimento
                FROM configuracao_whatsapp
                WHERE id_configuracao = 1;";

            return ConsultarUnico(sql, null, leitor => new EstadoWhatsApp
            {
                IntegracaoAtivada = Convert.ToInt32(leitor["integracao_ativada"]) == 1,
                UltimoEstadoConectado = Convert.ToInt32(leitor["ultimo_estado_conectado"]) == 1,
                AtualizadoEm = Convert.ToDateTime(leitor["atualizado_em"]),
                DiasAposVencimento = Convert.ToInt32(leitor["dias_apos_vencimento"])

            }, new EstadoWhatsApp());
        }

        // Persiste a ativação e o estado de conexão
        public void SalvarEstado(bool ativado, bool conectado)
        {
            GarantirEstrutura();
            const string sql = @"
                INSERT INTO configuracao_whatsapp
                    (id_configuracao, integracao_ativada, ultimo_estado_conectado)
                VALUES (1, @ativado, @conectado)
                ON DUPLICATE KEY UPDATE
                    integracao_ativada = VALUES(integracao_ativada),
                    ultimo_estado_conectado = VALUES(ultimo_estado_conectado),
                    atualizado_em = CURRENT_TIMESTAMP;";

            ExecutarComando(sql, comando =>
            {
                // Substitui palavra chave por variável
                comando.Parameters.AddWithValue("@ativado", ativado ? 1 : 0);
                comando.Parameters.AddWithValue("@conectado", conectado ? 1 : 0);
            });
        }

        // Grava o prazo mínimo para iniciar uma cobrança
        public void SalvarDiasAposVencimento(int dias)
        {
            if (dias < 1 || dias > 30)
                throw new ArgumentOutOfRangeException(nameof(dias), "O prazo deve ficar entre 1 e 30 dias.");

            GarantirEstrutura();
            ExecutarComando(@"
                UPDATE configuracao_whatsapp
                SET dias_apos_vencimento = @dias
                WHERE id_configuracao = 1;",
                comando => comando.Parameters.Add("@dias", MySqlDbType.Int32).Value = dias);
        }
    }
}
