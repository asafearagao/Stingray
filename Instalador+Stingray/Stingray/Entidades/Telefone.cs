namespace Stingray.Entidades
{

    // Representa um telefone associado a uma pessoa
    public class Telefone
    {
        public int IdTelefone { get; set; }
        public string NumeroTelefone { get; set; }
        public int IdPessoa { get; set; }
    }
}
