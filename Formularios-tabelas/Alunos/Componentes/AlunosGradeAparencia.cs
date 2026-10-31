
using System.Drawing;
using System.Windows.Forms;
using Stingray.Interface;

namespace Stingray.Formularios.Alunos.Componentes
{
    internal sealed class AlunosGradeAparencia
    {
        private readonly DataGridView grade; // Armazena a tabela de alunos que será aparência alterada
        private readonly AlunosGradeInteracao interacao;

        // Inicializa o componente de aparência com a grade e suas interações
        public AlunosGradeAparencia(DataGridView grade, AlunosGradeInteracao interacao)
        {
            this.grade = grade;          // Armazena a referência da tabela
            this.interacao = interacao;  // Armazena a referência do gerenciador de interações
        }

        // Configura a aparência da grade e registra os eventos de desenho
        public void Configurar()
        {
            ConfigurarDimensoes();        // Define as dimensões da tabela e de suas colunas
            ConfigurarAlinhamento();      // Define o alinhamento e os espaçamentos do conteúdo
            RegistrarEventosDeDesenho();
        }

        // Define as dimensões das colunas da grade
        private void ConfigurarDimensoes()
        {
            VisualPadrao.ConfigurarGrade(grade); // Aplica as configurações visuais padrão da tabela

            grade.ColumnHeadersHeight = 38;  // Define a altura do cabeçalho
            grade.RowTemplate.Height = 38;   // Define a altura das linhas

            grade.Columns["colunaAcoes"].Width = 100; // Define a largura da coluna de ações
            grade.Columns["colunaAcoes"].MinimumWidth = 100; // Define largura mínima das coluna

            // Desativa o redimensionamento automático da coluna
            grade.Columns["colunaAcoes"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        }

        // Define o alinhamento do conteúdo das colunas
        private void ConfigurarAlinhamento()
        {
            // Centraliza os textos do cabeçalho
            grade.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grade.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0); // Define o espaçamento interno do cabeçalho

            grade.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // Centraliza o conteúdo das células
            grade.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0); // Define o espaçamento interno das células

            // Centraliza os elementos da coluna de ações
            grade.Columns["colunaAcoes"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grade.Columns["colunaAcoes"].DefaultCellStyle.Padding = new Padding(0);
        }

        // Registra os eventos de desenho
        private void RegistrarEventosDeDesenho()
        {
            grade.CellFormatting += AoFormatarCelula;  // Associa a formatação das células
            grade.CellPainting += AoDesenharCelula;    // Associa o desenho das células
        }

        // Formata o conteúdo das células de acordo com o tipo de dado exibido
        private void AoFormatarCelula(object remetente, DataGridViewCellFormattingEventArgs argumentos)
        {
            // Verifica se a célula pertence à coluna de status
            if (!CelulaEhDeStatus(argumentos.RowIndex, argumentos.ColumnIndex))
                return;

            argumentos.CellStyle.BackColor = Color.White; // Define o fundo da célula
            argumentos.CellStyle.ForeColor = Color.FromArgb(55, 65, 81); // Define a cor do texto

            argumentos.CellStyle.SelectionBackColor = Color.White; // Mantém o fundo branco durante a seleção
            argumentos.CellStyle.SelectionForeColor = Color.FromArgb(55, 65, 81); // Mantém a cor do texto durante a seleção

            argumentos.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // Centraliza o conteúdo da célula

            // Define a fonte utilizada na coluna de status
            argumentos.CellStyle.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
        }

        // Desenha os elementos personalizados das células da grade
        private void AoDesenharCelula(object remetente, DataGridViewCellPaintingEventArgs argumentos)
        {
            // Verifica se o evento corresponde a uma célula de dados
            if (argumentos.RowIndex < 0 || argumentos.ColumnIndex < 0)
                return;

            string nomeColuna = grade.Columns[argumentos.ColumnIndex].Name; // Obtém o nome da coluna que será desenhada

            if (nomeColuna == "colunaStatus") // Verifica se a célula pertence à coluna de status
            {
                DesenhoStatusAluno.Desenhar(argumentos); // Executa o desenho personalizado da coluna status
                return;
            }

            if (nomeColuna == "colunaAcoes") // Verifica se a célula pertence à coluna de ações
                DesenhoBotoesAcaoAluno.Desenhar(argumentos, interacao);
        }

        // Verifica se a célula pertence à coluna de status
        private bool CelulaEhDeStatus(int indiceLinha, int indiceColuna)
        {
            return indiceLinha >= 0 && indiceColuna >= 0 && // Verifica se os índices correspondem a uma célula válida
                grade.Columns[indiceColuna].Name == "colunaStatus"; // Confirma se a coluna corresponde ao status
        }
    }
}