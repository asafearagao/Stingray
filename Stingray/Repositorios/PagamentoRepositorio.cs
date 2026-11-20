using System;
using System.Collections.Generic;
using MySqlConnector;
using Stingray.BancoDeDados;
using Stingray.DadosTela;
using Stingray.Servicos;

namespace Stingray.Repositorios
{

    // Gera, consulta, recebe e remove mensalidades no banco de dados
    public class PagamentoRepositorio : RepositorioMySqlBase
    {

        // Cria as mensalidades da competência sem repetir cobranças existentes
        public int GerarMensalidades(DateTime dataGeracao)
        {
            DateTime referencia = new DateTime(dataGeracao.Year, dataGeracao.Month, 1);
            const string sql = @"
                INSERT IGNORE INTO mensalidade
                    (mes_referencia, data_pagamento, data_geracao, data_vencimento, valor, situacao, id_matricula)
                SELECT @referencia, NULL, @geradaEm,
                       DATE_ADD(@referencia, INTERVAL (m.dia_vencimento - 1) DAY),
                       SUM(t.valor_mensal), 'Pendente', m.id_matricula
                FROM matricula m
                INNER JOIN (
                    SELECT DISTINCT id_matricula, id_turma
                    FROM turma_matricula
                ) tm ON tm.id_matricula = m.id_matricula
                INNER JOIN turma t ON t.id_turma = tm.id_turma
                WHERE m.situacao = 'Ativo'
                  AND m.id_matricula = (
                      SELECT MAX(m2.id_matricula)
                      FROM matricula m2
                      WHERE m2.id_aluno = m.id_aluno
                  )
                GROUP BY m.id_matricula, m.dia_vencimento
                HAVING COUNT(DISTINCT tm.id_turma) > 0;";

            return ExecutarComando(sql, comando =>
            {
                // Substitui palavra chave por variável
                comando.Parameters.Add("@referencia", MySqlDbType.Date).Value = referencia;
                comando.Parameters.Add("@geradaEm", MySqlDbType.DateTime).Value = dataGeracao;
            });
        }

        // Busca as mensalidades e os dados usados nos filtros da tela
        public List<PagamentoLinha> ListarTodos(DateTime hoje)
        {
            const string sql = @"
                SELECT me.id_mensalidade, me.mes_referencia, me.data_geracao,
                       me.data_vencimento, me.valor,
                       CASE WHEN me.situacao = 'Pendente' AND me.data_vencimento < @hoje
                            THEN 'Atrasado' ELSE me.situacao END AS situacao_exibida,
                       p.nome AS nome_aluno, a.id_aluno,
                       p.data_nascimento AS data_nascimento_aluno,
                       COALESCE((SELECT pessoa_resp.nome
                                 FROM aluno_responsavel ar_resp
                                 INNER JOIN pessoa pessoa_resp ON pessoa_resp.id_pessoa = ar_resp.id_responsavel
                                 WHERE ar_resp.id_aluno = a.id_aluno
                                   AND EXISTS (SELECT 1 FROM telefone tel_resp_nome
                                               WHERE tel_resp_nome.id_pessoa = ar_resp.id_responsavel)
                                 ORDER BY pessoa_resp.nome, ar_resp.id_responsavel
                                 LIMIT 1),
                                CASE WHEN LOCATE('Responsável:', COALESCE(a.observacao, '')) > 0
                                     THEN TRIM(SUBSTRING_INDEX(a.observacao, 'Responsável:', -1))
                                     ELSE '' END) AS nome_responsavel,
                       COALESCE((SELECT tel_aluno.numero_telefone
                                 FROM telefone tel_aluno
                                 WHERE tel_aluno.id_pessoa = a.id_aluno
                                 ORDER BY tel_aluno.id_telefone
                                 LIMIT 1), '') AS telefone_contato,
                       COALESCE((SELECT tel_resp.numero_telefone
                                 FROM aluno_responsavel ar_resp_tel
                                 INNER JOIN pessoa pessoa_resp_tel ON pessoa_resp_tel.id_pessoa = ar_resp_tel.id_responsavel
                                 INNER JOIN telefone tel_resp ON tel_resp.id_pessoa = ar_resp_tel.id_responsavel
                                 WHERE ar_resp_tel.id_aluno = a.id_aluno
                                 ORDER BY pessoa_resp_tel.nome, ar_resp_tel.id_responsavel, tel_resp.id_telefone
                                 LIMIT 1), '') AS telefone_responsavel,
                       COALESCE(GROUP_CONCAT(DISTINCT t.modalidade ORDER BY t.modalidade SEPARATOR ', '), 'Sem turma') AS turmas
                FROM mensalidade me
                INNER JOIN matricula m ON m.id_matricula = me.id_matricula
                INNER JOIN aluno a ON a.id_aluno = m.id_aluno
                INNER JOIN pessoa p ON p.id_pessoa = a.id_aluno
                LEFT JOIN turma_matricula tm ON tm.id_matricula = m.id_matricula
                LEFT JOIN turma t ON t.id_turma = tm.id_turma
                GROUP BY me.id_mensalidade, me.mes_referencia, me.data_geracao,
                         me.data_vencimento, me.valor, me.situacao, p.nome,
                         p.data_nascimento, a.observacao, a.id_aluno
                ORDER BY me.data_geracao DESC, p.nome;";

            return ConsultarLista(sql, comando =>
            {
                // Substitui palavra chave por variável
                comando.Parameters.Add("@hoje", MySqlDbType.Date).Value = hoje.Date;

            }, leitor => new PagamentoLinha
            {
                // Variável = função.tipo(leitor, "nome da coluna", valor padrão)
                IdMensalidade = LeitorMySql.Inteiro(leitor, "id_mensalidade"),
                MesReferencia = LeitorMySql.Data(leitor, "mes_referencia"),
                DataGeracao = LeitorMySql.Data(leitor, "data_geracao"),
                DataVencimento = LeitorMySql.Data(leitor, "data_vencimento"),
                Valor = LeitorMySql.Decimal(leitor, "valor"),
                Situacao = LeitorMySql.Texto(leitor, "situacao_exibida"),
                NomeAluno = LeitorMySql.Texto(leitor, "nome_aluno"),
                IdAluno = LeitorMySql.Inteiro(leitor, "id_aluno"),
                DataNascimentoAluno = LeitorMySql.Data(leitor, "data_nascimento_aluno"),
                NomeResponsavel = LeitorMySql.Texto(leitor, "nome_responsavel"),
                TelefoneContato = LeitorMySql.Texto(leitor, "telefone_contato"),
                TelefoneResponsavel = LeitorMySql.Texto(leitor, "telefone_responsavel"),
                Turmas = LeitorMySql.Texto(leitor, "turmas", "Sem turma")
            });
        }

        // Remove mensalidades recebidas que ultrapassaram o prazo de retenção
        public int ExcluirPagamentosPagosAntigos(DateTime hoje)
        {
            DateTime limite = RetencaoPagamentoRegra.ObterDataLimite(hoje);
            return ExecutarTransacao<int>((conexao, transacao) =>
            {
                const string excluirEnvios = @"
                    DELETE we FROM whatsapp_envio we
                    INNER JOIN mensalidade me ON me.id_mensalidade = we.id_mensalidade
                    WHERE me.situacao = 'Pago' AND COALESCE(me.data_pagamento, me.data_vencimento) < @limite;";
                using (MySqlCommand comando = new MySqlCommand(excluirEnvios, conexao, transacao))
                {
                    comando.Parameters.Add("@limite", MySqlDbType.Date).Value = limite;
                    comando.ExecuteNonQuery();
                }

                const string excluirMensalidades = @"
                    DELETE FROM mensalidade
                    WHERE situacao = 'Pago' AND COALESCE(data_pagamento, data_vencimento) < @limite;";
                using (MySqlCommand comando = new MySqlCommand(excluirMensalidades, conexao, transacao))
                {
                    comando.Parameters.Add("@limite", MySqlDbType.Date).Value = limite;
                    return comando.ExecuteNonQuery();
                }
            });
        }

        // Confere o estado atual da mensalidade no banco
        public bool EstaPago(int idMensalidade)
        {
            object situacao = ConsultarValor(@"
                SELECT situacao FROM mensalidade WHERE id_mensalidade = @idMensalidade LIMIT 1;",
                comando => comando.Parameters.Add("@idMensalidade", MySqlDbType.Int32).Value = idMensalidade);
            return string.Equals(Convert.ToString(situacao), "Pago", StringComparison.OrdinalIgnoreCase);
        }

        // Registra a data e a situação do recebimento
        public void RegistrarPagamento(int idMensalidade, DateTime dataPagamento)
        {
            if (idMensalidade <= 0)
                throw new ArgumentException("Selecione uma mensalidade válida.", nameof(idMensalidade));
            if (dataPagamento == DateTime.MinValue || dataPagamento.Date > DateTime.Today)
                throw new ArgumentException("Informe uma data de pagamento válida.", nameof(dataPagamento));

            const string sql = @"
                UPDATE mensalidade
                SET situacao = 'Pago', data_pagamento = @dataPagamento
                WHERE id_mensalidade = @idMensalidade
                  AND situacao IN ('Pendente', 'Atrasado');";

            int alteradas = ExecutarComando(sql, comando =>
            {
                comando.Parameters.Add("@dataPagamento", MySqlDbType.Date).Value = dataPagamento.Date;
                comando.Parameters.Add("@idMensalidade", MySqlDbType.Int32).Value = idMensalidade;
            });

            if (alteradas == 0)
                throw new InvalidOperationException("A cobrança não foi encontrada ou já está registrada como paga.");
        }

    }
}
