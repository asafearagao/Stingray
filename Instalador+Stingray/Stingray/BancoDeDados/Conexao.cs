using System;
using System.Collections.Generic;
using System.Configuration;
using MySqlConnector;

namespace Stingray.BancoDeDados
{

    // Centraliza a abertura e a verificação das conexões com o MySQL
    public static class Conexao
    {

        // Monta a configuração de conexão com os parâmetros definidos para a instalação
        private static MySqlConnectionStringBuilder CriarConfiguracao(uint porta)
        {
            return new MySqlConnectionStringBuilder
            {
                // Configurações de conexão com o banco de dados MySQL
                Server = "127.0.0.1",
                Port = porta,
                Database = "teste_projeto",
                UserID = "stingray_app",
                Password = CredencialBancoDados.ObterSenha(),
                ConnectionTimeout = 10,
                DefaultCommandTimeout = 30,
                Pooling = true,

                // Desabilita a criptografia SSL
                SslMode = MySqlSslMode.Disabled,

                // Permite a recuperação da chave pública do servidor, necessária para autenticação
                AllowPublicKeyRetrieval = true
            };
        }

        // Abre uma conexão com o banco de dados MySQL usando as configurações definidas
        public static MySqlConnection Abrir()
        {
            Exception ultimoErro = null;
            foreach (uint porta in ObterPortas())
            {
                MySqlConnection conexao = new MySqlConnection(CriarConfiguracao(porta).ConnectionString);
                try
                {
                    conexao.Open();
                    return conexao;
                }
                catch (Exception erro)
                {
                    ultimoErro = erro;
                    conexao.Dispose();

                    // Falhas de autenticação ou banco inexistente não serão resolvidas
                    // tentando outra porta; preservamos a causa original para o diagnóstico
                    MySqlException erroMySql = erro as MySqlException;
                    if (erroMySql != null && (erroMySql.Number == 1045 || erroMySql.Number == 1049))
                        break;
                }
            }

            throw new InvalidOperationException(
                "Não foi possível conectar ao MySQL nas portas configuradas (" +
                string.Join(", ", ObterPortas()) + ").", ultimoErro);
        }

        // Identifica as portas configuradas para as instâncias do MySQL
        private static uint[] ObterPortas()
        {
            string configuracao = ConfigurationManager.AppSettings["MySqlPorts"];
            List<uint> portas = new List<uint>();

            if (!string.IsNullOrWhiteSpace(configuracao))
            {
                foreach (string item in configuracao.Split(','))
                {
                    uint porta;
                    if (uint.TryParse(item.Trim(), out porta) && porta > 0 && !portas.Contains(porta))
                        portas.Add(porta);
                }
            }

            if (portas.Count == 0)
            {
                portas.Add(3307);
            }

            return portas.ToArray();
        }

        // Testa a conexão e converte falhas comuns em orientações acionáveis
        public static bool Testar(out string mensagem)
        {
            try
            {
                using (var conexao = Abrir())
                using (var comando = new MySqlCommand("SELECT DATABASE(), CURRENT_USER()", conexao))
                using (var leitor = comando.ExecuteReader())
                {
                    if (!leitor.Read() || !string.Equals(leitor.GetString(0), "teste_projeto", StringComparison.OrdinalIgnoreCase))
                        throw new InvalidOperationException("A conexão foi aberta, mas não selecionou o banco teste_projeto.");
                }

                mensagem = "Conexão com teste_projeto estabelecida para o usuário stingray_app.";
                return true;
            }
            catch (Exception erro)
            {
                MySqlException erroMySql = erro as MySqlException;
                if (erroMySql == null)
                    erroMySql = erro.InnerException as MySqlException;

                if (erroMySql != null && erroMySql.Number == 1045)
                {
                    mensagem = "O MySQL recusou a autenticação do usuário stingray_app. " +
                        "Execute o instalador atualizado na mesma conta do Windows utilizada para abrir o Stingray. Detalhe: " + erroMySql.Message;
                }
                else if (erroMySql != null && erroMySql.Number == 1049)
                {
                    mensagem = "O banco teste_projeto não existe nesta instância MySQL. Execute o instalador para criar o banco e o esquema. Detalhe: " + erroMySql.Message;
                }
                else if (erroMySql != null)
                {
                    mensagem = "O MySQL respondeu, mas ocorreu um erro durante a conexão. Código " +
                        erroMySql.Number + ": " + erroMySql.Message;
                }
                else
                {
                    mensagem = "Não foi possível abrir a conexão com o banco de dados. " + erro.Message;
                }
                return false;
            }
        }

    }
}
