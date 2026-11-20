using System;

namespace Stingray.Entidades
{

    // Reúne os dados pessoais compartilhados pelos cadastros
    public class Pessoa
    {
        public int IdPessoa { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public string Email { get; set; }
        public int IdEndereco { get; set; }
    }
}
