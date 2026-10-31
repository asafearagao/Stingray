
using System;
using System.Drawing;
using System.Windows.Forms;
using Stingray.Modelos;

namespace Stingray.Formularios.Alunos.Componentes
{
    internal sealed class AlunosGradeCliqueAcao
    {

        private readonly DataGridView grade; // Armazena a tabela onde os botões de ação estão desenhados
        private readonly Action<AcaoAlunoNaGrade, Aluno> executarAcao; // Armazena o método que executar a ação selecionada

        public AlunosGradeCliqueAcao( // Inicializa o gerenciador de cliques dos botões de ação
            DataGridView grade,
            Action<AcaoAlunoNaGrade, Aluno> executarAcao) 
        {
            this.grade = grade;                // Armazena a referência da tabela
            this.executarAcao = executarAcao;  // Armazena a ação que será executada após o clique
        }

        // Registra o evento responsável por identificar os cliques nas ações da grade
        public void Configurar()
        {
            grade.CellMouseClick += AoClicarNaCelula; // Associa o evento de clique nas células
        }

        // Trata o clique na célula e identifica o botão de ação selecionado
        private void AoClicarNaCelula(object remetente, DataGridViewCellMouseEventArgs argumentos)
        {
            if (!EhCliqueNaColunaDeAcoes(argumentos)) // Verifica se o clique ocorreu na coluna de ações
                return;

            // Identifica qual botão recebeu o clique
            int indiceBotao = ObterIndiceDoBotao(argumentos.ColumnIndex, argumentos.RowIndex);

            Aluno aluno = grade.Rows[argumentos.RowIndex].Tag as Aluno; // Obtém o aluno associado à linha clicada

            if (indiceBotao < 0 || aluno == null) // Verifica se o botão e o aluno são válidos
                return;

            executarAcao(ObterAcao(indiceBotao), aluno);
        }

        // Verifica se clique na coluna de ações
        private bool EhCliqueNaColunaDeAcoes(DataGridViewCellMouseEventArgs argumentos)
        {
            return argumentos.RowIndex >= 0 && // Verifica se o clique ocorreu em uma linha de dados
                argumentos.ColumnIndex >= 0 && // Verifica se o índice da coluna é válido
                argumentos.ColumnIndex < grade.Columns.Count && // Verifica se a coluna existe na tabela
                grade.Columns[argumentos.ColumnIndex].Name == "colunaAcoes"; // Confirma se a coluna corresponde às ações
        }

        // Obtém o índice do botão localizado sob a posição atual do mouse
        private int ObterIndiceDoBotao(int indiceColuna, int indiceLinha)
        {
            // Obtém a posição e as dimensões da célula na tela
            Rectangle celula = grade.GetCellDisplayRectangle(indiceColuna, indiceLinha, false);

            // Converte a posição do mouse para as coordenadas da tabela
            Point posicaoDoMouse = grade.PointToClient(Cursor.Position);

            Point posicaoNaCelula = new Point( // Calcula a posição do mouse em relação ao interior da célula
                posicaoDoMouse.X - celula.Left,
                posicaoDoMouse.Y - celula.Top);

            // Identifica o botão utilizando e sua posição
            return GeometriaBotoesAcaoAluno.ObterIndiceDoBotao(posicaoNaCelula, celula);
        }

        private static AcaoAlunoNaGrade ObterAcao(int indiceBotao) // Converte o índice do botão na ação correspondente
        {
            switch (indiceBotao)
            {
                case 0: return AcaoAlunoNaGrade.AbrirFicha;
                case 1: return AcaoAlunoNaGrade.Editar;
                case 2: return AcaoAlunoNaGrade.Excluir;

                // Informa que o índice recebido não corresponde a uma ação válida
                default: throw new ArgumentOutOfRangeException(nameof(indiceBotao));
            }
        }
    }
}