using System;
using System.Collections.Generic;
using Stingray.DadosTela;
using Stingray.Repositorios;

namespace Stingray.Servicos
{
    //
    // Falta coisas aqui, porque o pagamento deve ser gerado e não só exibido o valor
    // Sabe para criar um para cada mês
    //
    public class PagamentoServico
    {
        private readonly PagamentoRepositorio repositorio = new PagamentoRepositorio();
        private static readonly object SincronizacaoRetencao = new object();
        private static DateTime ultimaLimpeza = DateTime.MinValue;

        // Coordena a geração das cobranças para a competência informada
        public int GerarMensalidades(DateTime dataLocal)
        {
            if (dataLocal == DateTime.MinValue)
                throw new ArgumentException("Informe uma data de geração válida.", nameof(dataLocal));

            GarantirLimpezaHistorico(dataLocal);
            return repositorio.GerarMensalidades(dataLocal);
        }

        // Obtém as mensalidades e executa a manutenção do histórico vencido
        public List<PagamentoLinha> ListarTodos(DateTime hojeLocal)
        {
            GarantirLimpezaHistorico(hojeLocal);
            return repositorio.ListarTodos(hojeLocal);
        }

        // Executa a limpeza de registros antigos dentro das regras de retenção
        private void GarantirLimpezaHistorico(DateTime hojeLocal)
        {
            lock (SincronizacaoRetencao)
            {
                if (ultimaLimpeza.Date == hojeLocal.Date)
                    return;

                repositorio.ExcluirPagamentosPagosAntigos(hojeLocal);
                ultimaLimpeza = hojeLocal.Date;
            }
        }

        // Valida e registra o recebimento da mensalidade
        public void RegistrarPagamento(int idMensalidade, DateTime dataPagamento)
        {
            repositorio.RegistrarPagamento(idMensalidade, dataPagamento);
        }
    }
}
