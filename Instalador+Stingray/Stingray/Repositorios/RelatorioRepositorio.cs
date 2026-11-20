
using MySqlConnector;
using Stingray.BancoDeDados;
using System;
using System.Collections.Generic;

namespace Stingray.Repositorios
{
    // Consulta os dados utilizados pelo painel de relatórios.
    // Todas as operações deste repositório são somente de leitura.
    public class RelatorioRepositorio : RepositorioMySqlBase
    {
        // Soma os pagamentos efetivamente recebidos no período.
        public decimal ObterReceitaRecebida(DateTime inicio, DateTime fim)
        {
            const string sql = @"
                SELECT COALESCE(SUM(valor), 0)
                FROM mensalidade
                WHERE situacao = 'Pago'
                  AND data_pagamento >= @inicio
                  AND data_pagamento < @fim;";

            return ConverterDecimal(ConsultarValor(sql, comando =>
            {
                comando.Parameters.Add("@inicio", MySqlDbType.Date).Value = inicio.Date;
                comando.Parameters.Add("@fim", MySqlDbType.Date).Value = fim.Date.AddDays(1);
            }));
        }

        // Soma todas as mensalidades ainda não pagas, inclusive as vencidas.
        public decimal ObterReceitaPendente()
        {
            const string sql = @"
                SELECT COALESCE(SUM(valor), 0)
                FROM mensalidade
                WHERE situacao = 'Pendente';";

            return ConverterDecimal(ConsultarValor(sql));
        }

        // Conta alunos distintos que possuem matrícula ativa.
        public int ObterQuantidadeAlunosAtivos()
        {
            const string sql = @"
                SELECT COUNT(DISTINCT id_aluno)
                FROM matricula
                WHERE situacao = 'Ativo';";

            return Convert.ToInt32(ConsultarValor(sql));
        }

        // Obtém os totais de presença, falta e justificativa
        // em aulas concluídas no período.
        public ResumoFrequenciaRelatorio ObterResumoFrequencia(
            DateTime inicio, DateTime fim)
        {
            const string sql = @"
                SELECT
                    COALESCE(SUM(CASE WHEN f.situacao = 'Presente' THEN 1 ELSE 0 END), 0) AS presentes,
                    COALESCE(SUM(CASE WHEN f.situacao = 'Ausente' THEN 1 ELSE 0 END), 0) AS ausentes,
                    COALESCE(SUM(CASE WHEN f.situacao = 'Justificado' THEN 1 ELSE 0 END), 0) AS justificados
                FROM frequencia f
                INNER JOIN ocorrencia_aula o
                    ON o.id_ocorrencia = f.id_ocorrencia
                WHERE o.situacao = 'Concluida'
                  AND o.data_aula >= @inicio
                  AND o.data_aula < @fim
                  AND f.situacao IN ('Presente', 'Ausente', 'Justificado')
                  AND NOT EXISTS (
                      SELECT 1
                      FROM reposicao r
                      WHERE r.id_ocorrencia = f.id_ocorrencia
                        AND r.situacao = 'Concluida'
                  );";

            return ConsultarUnico(sql, comando =>
            {
                comando.Parameters.Add("@inicio", MySqlDbType.Date).Value = inicio.Date;
                comando.Parameters.Add("@fim", MySqlDbType.Date).Value = fim.Date.AddDays(1);
            }, leitor =>
            {
                int presentes = Convert.ToInt32(leitor["presentes"]);
                int ausentes = Convert.ToInt32(leitor["ausentes"]);
                int justificados = Convert.ToInt32(leitor["justificados"]);

                int totalConsiderado = presentes + ausentes;

                decimal percentual = totalConsiderado == 0
                    ? 0
                    : (decimal)presentes / totalConsiderado * 100;

                return new ResumoFrequenciaRelatorio
                {
                    Presentes = presentes,
                    Ausentes = ausentes,
                    Justificados = justificados,
                    PercentualPresenca = percentual
                };
            }, new ResumoFrequenciaRelatorio());
        }

        // Retorna a receita recebida agrupada por mês.
        // Inclui meses sem recebimentos com valor zero.
        public List<ReceitaMensalRelatorio> ObterEvolucaoReceita(
            DateTime referencia, int quantidadeMeses)
        {
            if (quantidadeMeses <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantidadeMeses));

            DateTime primeiroMes = new DateTime(
                referencia.Year, referencia.Month, 1)
                .AddMonths(-(quantidadeMeses - 1));

            DateTime proximoMes = new DateTime(
                referencia.Year, referencia.Month, 1)
                .AddMonths(1);

            const string sql = @"
                SELECT
                    YEAR(data_pagamento) AS ano,
                    MONTH(data_pagamento) AS mes,
                    COALESCE(SUM(valor), 0) AS total
                FROM mensalidade
                WHERE situacao = 'Pago'
                  AND data_pagamento >= @inicio
                  AND data_pagamento < @fim
                GROUP BY YEAR(data_pagamento), MONTH(data_pagamento)
                ORDER BY ano, mes;";

            List<ReceitaMensalRelatorio> resultados = ConsultarLista(
                sql,
                comando =>
                {
                    comando.Parameters.Add("@inicio", MySqlDbType.Date).Value = primeiroMes;
                    comando.Parameters.Add("@fim", MySqlDbType.Date).Value = proximoMes;
                },
                leitor => new ReceitaMensalRelatorio
                {
                    Ano = Convert.ToInt32(leitor["ano"]),
                    Mes = Convert.ToInt32(leitor["mes"]),
                    Valor = ConverterDecimal(leitor["total"])
                });

            List<ReceitaMensalRelatorio> serie =
                new List<ReceitaMensalRelatorio>();

            for (int i = 0; i < quantidadeMeses; i++)
            {
                DateTime mesAtual = primeiroMes.AddMonths(i);

                ReceitaMensalRelatorio encontrado = resultados.Find(item =>
                    item.Ano == mesAtual.Year &&
                    item.Mes == mesAtual.Month);

                serie.Add(new ReceitaMensalRelatorio
                {
                    Ano = mesAtual.Year,
                    Mes = mesAtual.Month,
                    NomeMes = mesAtual.ToString("MMM"),
                    Valor = encontrado == null ? 0 : encontrado.Valor
                });
            }

            return serie;
        }

        private static decimal ConverterDecimal(object valor)
        {
            if (valor == null || valor == DBNull.Value)
                return 0;

            return Convert.ToDecimal(valor);
        }
    }

    // Resultado dos indicadores de frequência.
    public class ResumoFrequenciaRelatorio
    {
        public int Presentes { get; set; }
        public int Ausentes { get; set; }
        public int Justificados { get; set; }
        public decimal PercentualPresenca { get; set; }
    }

    // Resultado mensal utilizado no gráfico financeiro.
    public class ReceitaMensalRelatorio
    {
        public int Ano { get; set; }
        public int Mes { get; set; }
        public string NomeMes { get; set; }
        public decimal Valor { get; set; }
    }
}
