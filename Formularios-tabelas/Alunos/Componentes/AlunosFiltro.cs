
using System;
using System.Windows.Forms;
using Stingray;

namespace Stingray.Formularios.Alunos.Componentes
{
    internal sealed class AlunosFiltro
    {

        private readonly CampoBuscaAluno campoBusca;       // Gerencia o campo de pesquisa por nome
        private readonly OpcaoFiltroAluno filtroTurma;     // Gerencia o filtro por turma
        private readonly OpcaoFiltroAluno filtroProfessor; // Gerencia o filtro por professor
        private readonly OpcaoFiltroAluno filtroStatus;    // Gerencia o filtro por status
        private readonly BotoesFiltroAluno botoes;         // Gerencia os botões de pesquisa e limpeza
        private readonly AplicadorFiltroAlunos aplicador;  // Responsável por aplicar a filtragem na tabela

        public event EventHandler Alterado; // Notifica quando algum filtro é alterado

        public AlunosFiltro( // Construtor
            TextBox campoBusca,
            ComboBox listaTurmas,
            ComboBox listaProfessores,
            ComboBox listaStatus,
            Button setaTurma,
            Button setaProfessor,
            Button setaStatus,
            RoundedButton botaoPesquisar,
            RoundedButton botaoLimpar)
        {
            this.campoBusca = new CampoBuscaAluno(campoBusca); // Gerenciador do campo de pesquisa

            filtroTurma = new OpcaoFiltroAluno(listaTurmas, setaTurma); // Gerenciador do filtro de turma
            filtroProfessor = new OpcaoFiltroAluno(listaProfessores, setaProfessor); // Gerenciador do filtro de professor
            filtroStatus = new OpcaoFiltroAluno(listaStatus, setaStatus); // Gerenciador do filtro de status

            botoes = new BotoesFiltroAluno(botaoPesquisar, botaoLimpar); // Cria o gerenciador dos botões de filtragem

            aplicador = new AplicadorFiltroAlunos();

            RegistrarEventos();
        }

        // Configura os controles de filtro e registra seus eventos
        public void Configurar(bool emModoDesign)
        {
            campoBusca.Configurar(emModoDesign);       // Configura o campo de pesquisa
            filtroTurma.Configurar(emModoDesign);      // Configura o filtro de turma
            filtroProfessor.Configurar(emModoDesign);  // Configura o filtro de professor
            filtroStatus.Configurar(emModoDesign);     // Configura o filtro de status
            botoes.Configurar(emModoDesign);           // Configura os botões de pesquisa e limpeza
        }

        public void Aplicar(DataGridView grade, Label resumo)  // Aplicação dos filtros
        {
            aplicador.Aplicar(
                grade,
                resumo,
                campoBusca.Texto,
                filtroTurma.ValorSelecionado,
                filtroProfessor.ValorSelecionado,
                filtroStatus.ValorSelecionado);
        }

        private void RegistrarEventos() // Associa os eventos dos componentes aos métodos correspondentes
        {
            campoBusca.Alterado += AoAlterarOpcao;         // Monitora alterações no campo de pesquisa
            filtroTurma.Alterado += AoAlterarOpcao;        // Monitora alterações no filtro de turma
            filtroProfessor.Alterado += AoAlterarOpcao;    // Monitora alterações no filtro de professor
            filtroStatus.Alterado += AoAlterarOpcao;       // Monitora alterações no filtro de status

            botoes.PesquisarSolicitado += AoAlterarOpcao;  // Encaminha a solicitação de pesquisa
            botoes.LimparSolicitado += AoLimparFiltros;    // Encaminha a solicitação de limpeza dos filtros
        }

        // Executado quando algum componente solicita uma atualização
        private void AoAlterarOpcao(object remetente, EventArgs argumentos)
        {
            Alterado?.Invoke(this, EventArgs.Empty); // Notifica os componentes interessados sobre a alteração
        }

        // Trata a limpeza de filtros
        private void AoLimparFiltros(object remetente, EventArgs argumentos)
        {
            campoBusca.Limpar();                        // Limpa o conteúdo do campo de pesquisa

            filtroTurma.SelecionarPrimeiraOpcao();      // Restaura a primeira opção do filtro de turma
            filtroProfessor.SelecionarPrimeiraOpcao();  // Restaura a primeira opção do filtro de professor
            filtroStatus.SelecionarPrimeiraOpcao();     // Restaura a primeira opção do filtro de status

            Alterado?.Invoke(this, EventArgs.Empty);    // Notifica que os filtros foram limpos
        }
    }
}