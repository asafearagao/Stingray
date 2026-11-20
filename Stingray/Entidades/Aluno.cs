namespace Stingray.Entidades
{

    // Representa os dados principais de um aluno
    public class Aluno
    {
        public int IdAluno { get; set; }
        public string Observacao { get; set; }

        public Pessoa Pessoa { get; set; }
        public Endereco Endereco { get; set; }
        public Telefone Telefone { get; set; }

        public int IdMatricula { get; set; }
        public System.DateTime DataMatricula { get; set; }
        public byte DiaVencimento { get; set; }
        public string Situacao { get; set; }
        public bool InativoAutomatico { get; set; }

        public string Modalidades { get; set; }
        public string Professores { get; set; }
        public string PrimeiraTurma { get; set; }
        public int QuantidadeTurmas { get; set; }
        public decimal ValorPlano { get; set; }
    }
}
