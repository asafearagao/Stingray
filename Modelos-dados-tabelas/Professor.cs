using System;

namespace Stingray.Modelos
{
    public class Professor
    {
        // Propriedades que representam os dados do professor
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cref { get; set; }
        public string Telefone { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}