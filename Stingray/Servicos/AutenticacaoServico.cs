using System;
using Stingray.BancoDeDados;
using Stingray.Entidades;
using Stingray.Repositorios;

namespace Stingray.Servicos
{

    // Valida o acesso e coordena a alteração das credenciais
    public class AutenticacaoServico
    {
        private readonly UsuarioRepositorio repositorio;

        public AutenticacaoServico() // Construtor
        {
            repositorio = new UsuarioRepositorio();
        }

        // Testa a conexão com o banco de dados
        public bool TestarConexao(out string mensagem)
        {
            return Conexao.Testar(out mensagem);
        }

        // Verifica é o primeiro acesso, quando não existe usuário cadastrado
        public bool ExisteUsuarioCadastrado()
        {
            return repositorio.ExisteAlgumUsuario();
        }

        // Autentifica o usuário e o hash da senha
        public Usuario Autenticar(string nomeUsuario, string senha)
        {
            if (string.IsNullOrWhiteSpace(nomeUsuario) || string.IsNullOrEmpty(senha))
                return null;

            Usuario usuario = repositorio.BuscarPorNome(nomeUsuario.Trim());
            if (usuario == null || !SenhaConfere(usuario.SenhaHash, senha))
                return null;

            return usuario;
        }

        public bool ValidarAcesso(string nomeUsuario, string senha)
        {
            return Autenticar(nomeUsuario, senha) != null;
        }

        public Usuario CriarPrimeiroUsuario(string nomeUsuario, string senha)
        {
            string nome = ValidarNomeUsuario(nomeUsuario);
            ValidarSenhaNova(senha);

            ResultadoHashSenha credenciais = SenhaHasher.GerarHash(senha);

            // O hash da senha é composto pelo hash e pelo salt, separados por "|"
            var usuario = new Usuario
            {
                NomeUsuario = nome,
                SenhaHash = credenciais.Hash + "|" + credenciais.Salt
            };

            repositorio.CadastrarPrimeiroUsuario(usuario);
            return repositorio.BuscarPorNome(nome);
        }

        // Troca o nome e a senha da conta autenticada e confere a senha atual
        public Usuario AlterarCredenciais(
            Usuario usuarioAtual,
            string senhaAtual,
            string novoNomeUsuario,
            string novaSenha)
        {
            if (usuarioAtual == null || usuarioAtual.IdUsuario <= 0)
                throw new ArgumentException("Não foi possível identificar a conta atual.", nameof(usuarioAtual));

            Usuario contaBanco = repositorio.BuscarPorId(usuarioAtual.IdUsuario);
            if (contaBanco == null || !SenhaConfere(contaBanco.SenhaHash, senhaAtual))
                throw new InvalidOperationException("A senha atual não confere.");

            string novoNome = ValidarNomeUsuario(novoNomeUsuario);
            ValidarSenhaNova(novaSenha);

            if (repositorio.NomeJaUtilizado(novoNome, usuarioAtual.IdUsuario))
                throw new InvalidOperationException("Esse nome de usuário já está em uso.");

            ResultadoHashSenha credenciais = SenhaHasher.GerarHash(novaSenha);
            contaBanco.NomeUsuario = novoNome;
            contaBanco.SenhaHash = credenciais.Hash + "|" + credenciais.Salt;

            repositorio.AtualizarCredenciais(contaBanco);

            // Atualiza os dados de usuário no banco de dados
            usuarioAtual.NomeUsuario = contaBanco.NomeUsuario;
            usuarioAtual.SenhaHash = contaBanco.SenhaHash;

            return usuarioAtual;
        }

        private static string ValidarNomeUsuario(string nomeUsuario)
        {
            string nome = (nomeUsuario ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Informe o nome de usuário.", nameof(nomeUsuario));

            if (nome.Length > 50)
                throw new ArgumentException("O nome de usuário deve ter até 50 caracteres.", nameof(nomeUsuario));

            return nome;
        }

        private static void ValidarSenhaNova(string senha)
        {
            if (string.IsNullOrEmpty(senha))
                throw new ArgumentException("Informe uma senha.", nameof(senha));
        }

        private static bool SenhaConfere(string senhaArmazenada, string senhaDigitada)
        {
            // Se a senha armazenada ou digitada estiver vazia, não confere
            if (string.IsNullOrWhiteSpace(senhaArmazenada) || string.IsNullOrEmpty(senhaDigitada))
                return false;

            // A senha armazenada é composta pelo hash e pelo salt, separados por "|"
            string[] partes = senhaArmazenada.Split('|');
            if (partes.Length != 2)
                return false;

            try
            {
                return SenhaHasher.VerificarSenha(senhaDigitada, partes[0], partes[1]);
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
