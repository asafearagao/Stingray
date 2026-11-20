namespace Stingray.Entidades
{

    // Representa uma conta de acesso ao sistema
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public string SenhaHash { get; set; }
    }
}
