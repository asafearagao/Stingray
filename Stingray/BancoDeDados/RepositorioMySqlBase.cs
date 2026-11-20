using System;
using System.Collections.Generic;
using MySqlConnector;

namespace Stingray.BancoDeDados
{

    // Reúne operações comuns de comandos, consultas e transações
    public abstract class RepositorioMySqlBase
    {
        protected static List<T> ConsultarLista<T>(
            string sql,
            Action<MySqlCommand> configurar,
            Func<MySqlDataReader, T> mapear)
        {
            if (mapear == null)
                throw new ArgumentNullException(nameof(mapear));

            List<T> itens = new List<T>();

            using (MySqlConnection conexao = Conexao.Abrir())
            using (MySqlCommand comando = new MySqlCommand(sql, conexao))
            {
                if (configurar != null)
                    configurar(comando);

                using (MySqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                        itens.Add(mapear(leitor));
                }
            }

            return itens;
        }

        protected static T ConsultarUnico<T>(
            string sql,
            Action<MySqlCommand> configurar,
            Func<MySqlDataReader, T> mapear,
            T valorQuandoNaoEncontrado = default(T))
        {
            if (mapear == null)
                throw new ArgumentNullException(nameof(mapear));

            using (MySqlConnection conexao = Conexao.Abrir())
            using (MySqlCommand comando = new MySqlCommand(sql, conexao))
            {
                if (configurar != null)
                    configurar(comando);

                using (MySqlDataReader leitor = comando.ExecuteReader())
                {
                    return leitor.Read() ? mapear(leitor) : valorQuandoNaoEncontrado;
                }
            }
        }

        // Executa um comando parametrizado no banco de dados
        protected static int ExecutarComando(string sql, Action<MySqlCommand> configurar = null)
        {
            using (MySqlConnection conexao = Conexao.Abrir())
            using (MySqlCommand comando = new MySqlCommand(sql, conexao))
            {
                if (configurar != null)
                    configurar(comando);

                return comando.ExecuteNonQuery();
            }
        }

        // Obtém um único valor retornado por uma consulta
        protected static object ConsultarValor(string sql, Action<MySqlCommand> configurar = null)
        {
            using (MySqlConnection conexao = Conexao.Abrir())
            using (MySqlCommand comando = new MySqlCommand(sql, conexao))
            {
                if (configurar != null)
                    configurar(comando);

                return comando.ExecuteScalar();
            }
        }

        protected static T ExecutarTransacao<T>(
            Func<MySqlConnection, MySqlTransaction, T> executar)
        {
            if (executar == null)
                throw new ArgumentNullException(nameof(executar));

            using (MySqlConnection conexao = Conexao.Abrir())
            using (MySqlTransaction transacao = conexao.BeginTransaction())
            {
                try
                {
                    T resultado = executar(conexao, transacao);
                    transacao.Commit();
                    return resultado;
                }
                catch
                {
                    try
                    {
                        transacao.Rollback();
                    }
                    catch { }

                    throw;
                }
            }
        }

        // Executa as operações relacionadas dentro de uma única transação
        protected static void ExecutarTransacao(Action<MySqlConnection, MySqlTransaction> executar)
        {
            if (executar == null)
                throw new ArgumentNullException(nameof(executar));

            ExecutarTransacao<int>(delegate (MySqlConnection conexao, MySqlTransaction transacao)
            {
                executar(conexao, transacao);
                return 0;
            });
        }

        // Converte valores vazios em NULL para colunas opcionais
        protected static object ValorOuNulo(string valor)
        {
            return string.IsNullOrWhiteSpace(valor)
                ? (object)DBNull.Value
                : valor.Trim();
        }

        // Mantém apenas os dígitos de documentos e telefones
        protected static string SomenteDigitos(string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return string.Empty;

            char[] digitos = new char[valor.Length];
            int quantidade = 0;

            foreach (char caractere in valor)
            {
                if (char.IsDigit(caractere))
                    digitos[quantidade++] = caractere;
            }

            return new string(digitos, 0, quantidade);
        }
    }
}
