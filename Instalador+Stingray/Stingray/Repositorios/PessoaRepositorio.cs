using System;
using MySqlConnector; // Conexão com o MySQL
using Stingray.BancoDeDados;
using Stingray.Entidades;

namespace Stingray.Repositorios
{
    // Dados compartilhados entre todos os tipos de pessoas
    internal sealed class PessoaRepositorio : RepositorioMySqlBase
    {

        public int InserirEndereco(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            Endereco endereco)
        {
            // Insere os dados do endereço
            const string sql = @"
                INSERT INTO endereco (numero, cep, complemento)
                VALUES (@numero, @cep, @complemento);";

            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                comando.Parameters.AddWithValue("@numero", endereco.Numero.Trim());
                comando.Parameters.AddWithValue("@cep", FormatarCep(endereco.Cep));
                comando.Parameters.AddWithValue("@complemento", ValorOuNulo(endereco.Complemento));

                comando.ExecuteNonQuery(); // Executa o comando

                // Retorna o ID do endereço inserido
                return Convert.ToInt32(comando.LastInsertedId);
            }
        }

        public int InserirPessoa(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            Pessoa pessoa,
            int idEndereco)
        {
            // Insere os dados da pessoa
            const string sql = @"
                INSERT INTO pessoa
                    (nome, data_nascimento, cpf, rg, email, id_endereco)
                VALUES
                    (@nome, @nascimento, @cpf, @rg, @email, @idEndereco);";

            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                // Adiciona os parâmetros da pessoa ao comando SQL
                AdicionarDadosPessoais(comando, pessoa);
                comando.Parameters.AddWithValue("@idEndereco", idEndereco);

                comando.ExecuteNonQuery(); // Executa o comando SQL

                // Retorna o ID da pessoa inserida
                return Convert.ToInt32(comando.LastInsertedId);
            }
        }

        public void AtualizarEndereco(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            Endereco endereco,
            int idEndereco)
        {
            // Atualiza os campos do endereço
            const string sql = @"
                UPDATE endereco
                SET numero = @numero,
                    cep = @cep,
                    complemento = @complemento
                WHERE id_endereco = @idEndereco;";

            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                comando.Parameters.AddWithValue("@numero", endereco.Numero.Trim());
                comando.Parameters.AddWithValue("@cep", FormatarCep(endereco.Cep));
                comando.Parameters.AddWithValue("@complemento", ValorOuNulo(endereco.Complemento));
                comando.Parameters.AddWithValue("@idEndereco", idEndereco);

                comando.ExecuteNonQuery(); // Executa o comando SQL
            }
        }

        public void AtualizarPessoa(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            Pessoa pessoa,
            int idPessoa)
        {
            // Atualiza os campos da pessoa
            const string sql = @"
                UPDATE pessoa
                SET nome = @nome, data_nascimento = @nascimento, cpf = @cpf, rg = @rg, email = @email
                WHERE id_pessoa = @idPessoa;";

            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                AdicionarDadosPessoais(comando, pessoa);
                comando.Parameters.AddWithValue("@idPessoa", idPessoa);

                comando.ExecuteNonQuery(); // Executa o comando SQL
            }
        }

        public int ObterIdEndereco(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            int idPessoa)
        {
            const string sql = @"
                SELECT id_endereco
                FROM pessoa
                WHERE id_pessoa = @idPessoa
                FOR UPDATE;";

            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                comando.Parameters.AddWithValue("@idPessoa", idPessoa);
                object resultado = comando.ExecuteScalar();

                if (resultado == null || resultado == DBNull.Value)
                    throw new InvalidOperationException("A pessoa não foi encontrada no banco de dados.");

                return Convert.ToInt32(resultado);
            }
        }

        public void SincronizarTelefone(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            int idPessoa,
            Telefone telefone)
        {
            string numero = SomenteDigitos(telefone == null ? null : telefone.NumeroTelefone);
            int? idTelefone = ObterIdTelefone(conexao, transacao, idPessoa);

            if (numero.Length == 0)
            {
                ExcluirTelefones(conexao, transacao, idPessoa);
                return;
            }

            if (idTelefone.HasValue)
                AtualizarTelefone(conexao, transacao, idTelefone.Value, numero);
            else
                InserirTelefone(conexao, transacao, idPessoa, numero);

            ExcluirTelefonesAdicionais(conexao, transacao, idPessoa, idTelefone);
        }

        public void ExcluirTelefones(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            int idPessoa)
        {
            const string sql = "DELETE FROM telefone WHERE id_pessoa = @idPessoa;";
            ExecutarComId(sql, "@idPessoa", idPessoa, conexao, transacao);
        }

        public void ExcluirPessoa(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            int idPessoa)
        {
            // Remove a pessoa depois de remover todos os registros que a referenciam
            const string sql = "DELETE FROM pessoa WHERE id_pessoa = @idPessoa;";

            ExecutarComId(sql, "@idPessoa", idPessoa, conexao, transacao);
        }

        public void ExcluirEndereco(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            int idEndereco)
        {
            // Remove o endereço depois de remover o registro que o referencia
            const string sql = "DELETE FROM endereco WHERE id_endereco = @idEndereco;";

            ExecutarComId(sql, "@idEndereco", idEndereco, conexao, transacao);
        }

        private static void AdicionarDadosPessoais(MySqlCommand comando, Pessoa pessoa)
        {
            // Substitui as palavras-chave pelos valores dos parâmetros da pessoa
            comando.Parameters.AddWithValue("@nome", pessoa.Nome.Trim());
            comando.Parameters.AddWithValue("@nascimento", pessoa.DataNascimento.Date);
            comando.Parameters.AddWithValue("@cpf", SomenteDigitos(pessoa.Cpf));
            comando.Parameters.AddWithValue("@rg", pessoa.Rg.Trim());
            comando.Parameters.AddWithValue("@email", ValorOuNulo(pessoa.Email));
        }

        private static int? ObterIdTelefone(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            int idPessoa)
        {
            // Retorna o ID do telefone da pessoa, se existir
            const string sql = @"
                SELECT id_telefone
                FROM telefone
                WHERE id_pessoa = @idPessoa
                ORDER BY id_telefone
                LIMIT 1;";

            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                // Substitui a palavra-chave pelo valor do parâmetro do ID da pessoa
                comando.Parameters.AddWithValue("@idPessoa", idPessoa);

                // Executa o comando SQL e obtém o resultado
                object resultado = comando.ExecuteScalar();

                // Retorna o ID do telefone ou nulo se não houver telefone
                return resultado == null || resultado == DBNull.Value
                    ? (int?)null
                    : Convert.ToInt32(resultado);
            }
        }

        private static void InserirTelefone(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            int idPessoa,
            string numero)
        {
            // Insere um novo telefone para a pessoa
            const string sql = @"
                INSERT INTO telefone (numero_telefone, id_pessoa)
                VALUES (@numero, @idPessoa);";

            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                // Substitui as palavras-chave pelos valores dos parâmetros do telefone
                comando.Parameters.AddWithValue("@numero", numero);
                comando.Parameters.AddWithValue("@idPessoa", idPessoa);

                comando.ExecuteNonQuery(); // Executa o comando SQL
            }
        }

        private static void AtualizarTelefone(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            int idTelefone,
            string numero)
        {
            // Atualiza o número do telefone existente
            const string sql = @"
                UPDATE telefone
                SET numero_telefone = @numero
                WHERE id_telefone = @idTelefone;";

            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                comando.Parameters.AddWithValue("@numero", numero);
                comando.Parameters.AddWithValue("@idTelefone", idTelefone);

                comando.ExecuteNonQuery(); // Executa o comando SQL
            }
        }

        private static void ExcluirTelefonesAdicionais(
            MySqlConnection conexao,
            MySqlTransaction transacao,
            int idPessoa,
            int? idTelefonePrincipal)
        {
            const string sql = @"
                DELETE FROM telefone
                WHERE id_pessoa = @idPessoa
                  AND id_telefone <> @idTelefone;";

            if (!idTelefonePrincipal.HasValue)
                return;

            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                comando.Parameters.AddWithValue("@idPessoa", idPessoa);
                comando.Parameters.AddWithValue("@idTelefone", idTelefonePrincipal.Value);
                comando.ExecuteNonQuery();
            }
        }

        // Comando genérico que recebe ID e parametro
        private static void ExecutarComId(
            string sql,
            string parametro,
            int id,
            MySqlConnection conexao,
            MySqlTransaction transacao)
        {
            using (MySqlCommand comando = new MySqlCommand(sql, conexao, transacao))
            {
                comando.Parameters.AddWithValue(parametro, id);
                comando.ExecuteNonQuery(); // Executa o comando SQL
            }
        }

        private static string FormatarCep(string cep)
        {
            // Remove todos os caracteres não numéricos do CEP e formata como "XXXXX-XXX"
            string numeros = SomenteDigitos(cep);
            return numeros.Length == 8
                ? numeros.Substring(0, 5) + "-" + numeros.Substring(5, 3)
                : cep.Trim();
        }
    }
}
