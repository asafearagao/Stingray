using System;
using MySqlConnector; // Conexão com o MySQL
using Stingray.BancoDeDados;
using Stingray.Entidades;

namespace Stingray.Repositorios
{

    // Persiste contas de acesso e controla o primeiro cadastro
    public class UsuarioRepositorio : RepositorioMySqlBase
    {

        public bool ExisteAlgumUsuario()
        {
            const string sql = "SELECT EXISTS(SELECT 1 FROM usuario);";
            return Convert.ToInt32(ConsultarValor(sql)) == 1;
        }

        public Usuario BuscarPorNome(string nomeUsuario)
        {
            // Localizar a conta pelo nome de usuário
            const string sql = @"
                SELECT id_usuario, nome_usuario, senha_hash
                FROM usuario
                WHERE nome_usuario = @nome
                LIMIT 1;";

            return ConsultarUnico(sql,
                comando => comando.Parameters.AddWithValue("@nome", nomeUsuario),
                MapearUsuario);
        }

        public Usuario BuscarPorId(int idUsuario)
        {
            // Localizar a conta pelo identificador da sessão
            const string sql = @"
                SELECT id_usuario, nome_usuario, senha_hash
                FROM usuario
                WHERE id_usuario = @id
                LIMIT 1;";

            return ConsultarUnico(sql,
                comando => comando.Parameters.AddWithValue("@id", idUsuario),
                MapearUsuario);
        }

        public void CadastrarPrimeiroUsuario(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            using (MySqlConnection conexao = Conexao.Abrir())
            {
                ObterBloqueioPrimeiroAcesso(conexao);

                try
                {
                    if (ExisteUsuarioNaConexao(conexao))
                    {
                        throw new InvalidOperationException(
                            "Já existe uma conta cadastrada. Entre no sistema para alterar seus dados.");
                    }

                    InserirUsuario(conexao, usuario);

                } finally {
                    LiberarBloqueioPrimeiroAcesso(conexao);
                }
            }
        }

        public bool NomeJaUtilizado(string nomeUsuario, int idIgnorado)
        {
            // Verifica se o nome de usuário já está em uso por outra conta
            const string sql = @"
                SELECT EXISTS(
                    SELECT 1
                    FROM usuario
                    WHERE nome_usuario = @nome
                      AND id_usuario <> @id
                );";

            return Convert.ToInt32(ConsultarValor(sql, comando =>
            {
                comando.Parameters.AddWithValue("@nome", nomeUsuario);
                comando.Parameters.AddWithValue("@id", idIgnorado);
            })) == 1;
        }

        public void AtualizarCredenciais(Usuario usuario)
        {
            if (usuario == null) // Valida se o objeto de usuário é nulo
                throw new ArgumentNullException(nameof(usuario));

            // Valida se o ID do usuário é válido
            const string sql = @"
                UPDATE usuario
                SET nome_usuario = @nome,
                    senha_hash = @senhaHash
                WHERE id_usuario = @id;";

            int registros = ExecutarComando(sql, comando =>
            {
                comando.Parameters.AddWithValue("@nome", usuario.NomeUsuario);
                comando.Parameters.AddWithValue("@senhaHash", usuario.SenhaHash);
                comando.Parameters.AddWithValue("@id", usuario.IdUsuario);
            });

            // O MySQL pode informar zero quando os valores já eram iguais
            if (registros == 0 && BuscarPorId(usuario.IdUsuario) == null)
                throw new InvalidOperationException("A conta não foi encontrada. Faça login novamente.");
        }

        private static void ObterBloqueioPrimeiroAcesso(MySqlConnection conexao)
        {
            using (MySqlCommand comando = new MySqlCommand(
                "SELECT GET_LOCK('stingray_primeiro_acesso', 10);", conexao))
            {
                if (Convert.ToInt32(comando.ExecuteScalar()) != 1)
                {
                    throw new InvalidOperationException(
                        "Não foi possível iniciar o cadastro da primeira conta. Tente novamente.");
                }
            }
        }

        private static bool ExisteUsuarioNaConexao(MySqlConnection conexao)
        {
            using (MySqlCommand comando = new MySqlCommand(
                "SELECT EXISTS(SELECT 1 FROM usuario);", conexao))
            {
                return Convert.ToInt32(comando.ExecuteScalar()) == 1;
            }
        }

        private static void InserirUsuario(MySqlConnection conexao, Usuario usuario)
        {
            // Insere o usuário e hash da senha no banco de dados
            const string sql = @"
                INSERT INTO usuario (nome_usuario, senha_hash)
                VALUES (@nome, @senhaHash);";

            using (MySqlCommand comando = new MySqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("@nome", usuario.NomeUsuario);
                comando.Parameters.AddWithValue("@senhaHash", usuario.SenhaHash);
                comando.ExecuteNonQuery();
            }
        }

        private static void LiberarBloqueioPrimeiroAcesso(MySqlConnection conexao)
        {
            try
            {
                // Libera o acesso
                using (MySqlCommand comando = new MySqlCommand(
                    "SELECT RELEASE_LOCK('stingray_primeiro_acesso');", conexao))
                {
                    comando.ExecuteScalar();
                }
            }
            catch { }
        }

        private static Usuario MapearUsuario(MySqlDataReader leitor)
        {
            return new Usuario
            {
                IdUsuario = LeitorMySql.Inteiro(leitor, "id_usuario"),
                NomeUsuario = LeitorMySql.Texto(leitor, "nome_usuario"),
                SenhaHash = LeitorMySql.Texto(leitor, "senha_hash")
            };
        }
    }
}
