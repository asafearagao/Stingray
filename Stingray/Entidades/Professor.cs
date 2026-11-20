using System.Collections.Generic;

namespace Stingray.Entidades
{

    // Representa os dados de um professor
    public class Professor
    {
        public int IdProfessor { get; set; }
        public string Cref { get; set; }
        public Pessoa Pessoa { get; set; }
        public Endereco Endereco { get; set; }
        public Telefone Telefone { get; set; }
        public List<string> Turmas { get; set; }

        // Retorna o nome do professor para exibição, mesmo que a pessoa não esteja carregada
        public string NomeExibicao
        {
            get { return Pessoa == null ? string.Empty : Pessoa.Nome ?? string.Empty; }
        }
    }
}
