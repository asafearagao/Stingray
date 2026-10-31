using System;
using System.Windows.Forms;

namespace Stingray.Formularios.Alunos.Componentes
{
    internal sealed class AlunosGradeInteracao
    {
        // Componentes responsáveis pelas interações da grade
        private readonly AlunosGradeMouse mouse;
        private readonly AlunosGradeCliqueAcao clique;

        // Inicializa os componentes responsáveis pelas interações da grade
        public AlunosGradeInteracao(
            DataGridView grade,
            Action<AcaoAlunoNaGrade, Stingray.Modelos.Aluno> executarAcao)
        {
            mouse = new AlunosGradeMouse(grade);                         // Controla o comportamento do mouse
            clique = new AlunosGradeCliqueAcao(grade, executarAcao);     // Identifica e executa ações dos botões
        }

        // Registra os eventos de mouse e de clique das ações da grade
        public void Configurar()
        {
            mouse.Configurar();     // Registra os eventos relacionados ao mouse
            clique.Configurar();    // Registra o evento de clique nos botões de ação
        }

        // Verifica se o mouse está sobre o botão informado em determinada linha
        public bool MouseEstaSobre(int indiceLinha, int indiceBotao)
        {
            return mouse.MouseEstaSobre(indiceLinha, indiceBotao);
        }

        // Verifica se o botão informado está pressionado em determinada linha
        public bool BotaoEstaPressionado(int indiceLinha, int indiceBotao)
        {
            return mouse.BotaoEstaPressionado(indiceLinha, indiceBotao);
        }
    }
}