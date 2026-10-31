using System;
using System.Collections.Generic;

namespace Stingray.Formularios.Turmas.Modelos
{
    // Armazena os dados de alunos vinculados a uma turma
    internal sealed class RegistroAlunosTurma
    {
        public List<string> Alunos { get; private set; }
        public HashSet<string> Presentes { get; private set; }

        // Inicializa o registro utilizado para armazenar os alunos vinculados à turma
        public RegistroAlunosTurma()
        {
            Alunos = new List<string>();
            Presentes = new HashSet<string>(StringComparer.CurrentCultureIgnoreCase);
        }
    }
}