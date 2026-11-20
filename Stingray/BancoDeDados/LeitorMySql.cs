using System;
using System.Data;

namespace Stingray.BancoDeDados
{

    // Converte valores retornados pelo MySQL para os tipos usados pelo sistema
    internal static class LeitorMySql
    {

        public static string Texto(IDataRecord registro, string coluna, string padrao = null)
        {
            int indice = registro.GetOrdinal(coluna);
            return registro.IsDBNull(indice) ? padrao : Convert.ToString(registro.GetValue(indice));
        }

        public static int Inteiro(IDataRecord registro, string coluna, int padrao = 0)
        {
            int indice = registro.GetOrdinal(coluna);
            return registro.IsDBNull(indice) ? padrao : Convert.ToInt32(registro.GetValue(indice));
        }

        public static byte Byte(IDataRecord registro, string coluna, byte padrao = 0)
        {
            int indice = registro.GetOrdinal(coluna);
            return registro.IsDBNull(indice) ? padrao : Convert.ToByte(registro.GetValue(indice));
        }

        public static DateTime Data(IDataRecord registro, string coluna, DateTime padrao = default(DateTime))
        {
            int indice = registro.GetOrdinal(coluna);
            return registro.IsDBNull(indice) ? padrao : Convert.ToDateTime(registro.GetValue(indice));
        }

        public static decimal Decimal(IDataRecord registro, string coluna, decimal padrao = 0M)
        {
            int indice = registro.GetOrdinal(coluna);
            return registro.IsDBNull(indice) ? padrao : Convert.ToDecimal(registro.GetValue(indice));
        }

        public static TimeSpan Hora(IDataRecord registro, string coluna)
        {
            int indice = registro.GetOrdinal(coluna);
            return registro.IsDBNull(indice) ? TimeSpan.Zero : (TimeSpan)registro.GetValue(indice);
        }


    }
}
