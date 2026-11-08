using System.Configuration;
using MySqlConnector;

namespace Stingray.Data
{
    public static class Conexao
    {
        public static MySqlConnection Abrir()
        {
            string cs = ConfigurationManager
                .ConnectionStrings["StingrayDb"].ConnectionString;

            var conn = new MySqlConnection(cs);
            conn.Open();
            return conn;
        }
    }
}