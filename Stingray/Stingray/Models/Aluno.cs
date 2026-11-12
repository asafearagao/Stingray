using System.Collections.Generic;

namespace Stingray.Models
{
    public class Aluno : Pessoa
    {
        public string Observacao { get; set; }
        public List<Responsavel> Responsaveis { get; set; } = new List<Responsavel>();
    }
}
