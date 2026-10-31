
using System;
using System.Windows.Forms;
using Stingray.Modelos;

namespace Stingray.Formularios.Alunos.Componentes
{
    internal sealed class AplicadorFiltroAlunos
    {
        // Aplica os filtros e atualiza a quantidade de alunos exibidos
        public void Aplicar(
            DataGridView grade,
            Label resumo,
            string textoBusca,
            string turma,
            string professor,
            string status)
        {
            if (grade == null || grade.IsDisposed)
                return;

            int quantidadeVisivel = 0;

            // Percorre as linhas e verifica quais correspondem aos filtros
            foreach (DataGridViewRow linha in grade.Rows)
            {
                bool visivel = LinhaCorrespondeAosFiltros(
                    linha,
                    textoBusca,
                    turma,
                    professor,
                    status);

                OcultarCelulaAtualQuandoNecessario(grade, linha, visivel);
                linha.Visible = visivel;

                if (visivel)
                    quantidadeVisivel++;
            }

            AtualizarResumo(resumo, quantidadeVisivel, grade.Rows.Count);
        }

        // Verifica se uma linha atende simultaneamente a todos os filtros
        private static bool LinhaCorrespondeAosFiltros(
            DataGridViewRow linha,
            string textoBusca,
            string turma,
            string professor,
            string status)
        {
            Aluno aluno = linha.Tag as Aluno;
            string nome = Convert.ToString(linha.Cells["colunaAluno"].Value) ?? string.Empty;

            return CorrespondeAoTexto(nome, aluno, textoBusca) &&
                CorrespondeAoCampo(linha, "colunaTurma", turma) &&
                CorrespondeAoCampo(linha, "colunaProfessor", professor) &&
                CorrespondeAoCampo(linha, "colunaStatus", status);
        }

        // Verifica se o nome ou o CPF do aluno corresponde ao texto pesquisado
        private static bool CorrespondeAoTexto(string nome, Aluno aluno, string textoBusca)
        {
            if (string.IsNullOrEmpty(textoBusca))
                return true;

            bool nomeCorresponde = nome.IndexOf(textoBusca, StringComparison.CurrentCultureIgnoreCase) >= 0;
            bool cpfCorresponde = aluno != null &&
                (aluno.Cpf ?? string.Empty).IndexOf(textoBusca, StringComparison.CurrentCultureIgnoreCase) >= 0;

            return nomeCorresponde || cpfCorresponde;
        }

        // Compara o conteúdo de uma célula com o valor selecionado no filtro
        private static bool CorrespondeAoCampo(DataGridViewRow linha, string nomeColuna, string valorFiltro)
        {
            if (string.IsNullOrEmpty(valorFiltro))
                return true;

            string valorCelula = Convert.ToString(linha.Cells[nomeColuna].Value);

            return string.Equals(valorCelula, valorFiltro, StringComparison.CurrentCultureIgnoreCase);
        }

        // Oculta célula atual quando necessario
        private static void OcultarCelulaAtualQuandoNecessario(
            DataGridView grade,
            DataGridViewRow linha,
            bool linhaVisivel)
        {
            if (!linhaVisivel && grade.CurrentCell != null && grade.CurrentCell.RowIndex == linha.Index)
                grade.CurrentCell = null;
        }

        // Atualiza a quantidade de alunos visíveis e o total
        private static void AtualizarResumo(Label resumo, int quantidadeVisivel, int quantidadeTotal)
        {
            if (resumo == null)
                return;

            resumo.Text = "Mostrando " + quantidadeVisivel + " de " + quantidadeTotal + " alunos";
        }
    }
}