using System;
using System.Collections.Generic;

namespace Stingray.Models
{
    public class Pessoa
    {
        public int IdPessoa { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public string Email { get; set; }
        public int IdEndereco { get; set; }

        public Endereco Endereco { get; set; }
        public List<Telefone> Telefones { get; set; } = new List<Telefone>();
    }
}