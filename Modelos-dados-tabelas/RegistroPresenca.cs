using System;

namespace Stingray.Modelos
{
    public class RegistroPresenca
    {
        // Propriedades que identificam o aluno, a turma e a situação da presença
        public int Id { get; set; }
        public int TurmaId { get; set; }
        public int AlunoId { get; set; }
        public DateTime DataAula { get; set; }
        public string Situacao { get; set; }
        public string Observacao { get; set; }
    }
}