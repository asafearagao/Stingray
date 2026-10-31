
using System;
using System.Windows.Forms;

namespace Stingray.Formularios.Alunos.Componentes
{
    internal static class AlunosGrade
    {
        // Configura a aparência e as interações da tabela
        public static void Configurar(
            DataGridView grade,
            Action<AcaoAlunoNaGrade, Stingray.Modelos.Aluno> executarAcao)
        {
            if (grade == null)         // Verifica se a tabela foi informada
                throw new ArgumentNullException(nameof(grade));

            if (executarAcao == null)  // Verifica se a ação de interação foi informada
                throw new ArgumentNullException(nameof(executarAcao));

            // Cria o gerenciador das interações com a tabela
            AlunosGradeInteracao interacao = new AlunosGradeInteracao(grade, executarAcao);
            // Cria o gerenciador da aparência da tabela
            AlunosGradeAparencia aparencia = new AlunosGradeAparencia(grade, interacao);

            aparencia.Configurar();  // Configura os elementos visuais da tabela
            interacao.Configurar();  // Configura as interações disponíveis
        }
    }

    internal enum AcaoAlunoNaGrade
    {
        AbrirFicha = 0,
        Editar = 1,
        Excluir = 2
    }
}