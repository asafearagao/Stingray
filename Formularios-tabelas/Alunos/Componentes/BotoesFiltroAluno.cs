using System;
using Stingray;

namespace Stingray.Formularios.Alunos.Componentes
{
    internal sealed class BotoesFiltroAluno
    {
        private readonly BotaoPesquisarAlunos botaoPesquisar;
        private readonly BotaoLimparFiltrosAlunos botaoLimpar;

        public event EventHandler PesquisarSolicitado;
        public event EventHandler LimparSolicitado;

        // Inicializa os botões de pesquisa e limpeza dos filtros
        public BotoesFiltroAluno(RoundedButton botaoPesquisar, RoundedButton botaoLimpar)
        {
            this.botaoPesquisar = new BotaoPesquisarAlunos(botaoPesquisar);
            this.botaoLimpar = new BotaoLimparFiltrosAlunos(botaoLimpar);

            this.botaoPesquisar.Clicado += AoSolicitarPesquisa;
            this.botaoLimpar.Clicado += AoSolicitarLimpeza;
        }

        // Configura os botões de pesquisa e limpeza conforme o modo de execução
        public void Configurar(bool emModoDesign)
        {
            botaoPesquisar.Configurar(emModoDesign);
            botaoLimpar.Configurar(emModoDesign);
        }

        private void AoSolicitarPesquisa(object remetente, EventArgs argumentos)
        {
            PesquisarSolicitado?.Invoke(this, EventArgs.Empty);
        }

        private void AoSolicitarLimpeza(object remetente, EventArgs argumentos)
        {
            LimparSolicitado?.Invoke(this, EventArgs.Empty);
        }
    }
}