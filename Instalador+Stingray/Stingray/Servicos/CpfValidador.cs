using System;

namespace Stingray.Servicos
{

    // Valida o CPF por meio dos dígitos verificadores
    internal static class CpfValidador
    {

        public static bool EhValido(string cpf)
        {
            string digitos = SomenteDigitos(cpf);
            if (digitos.Length != 11)
                return false;

            // CPFs com todos os números iguais não são válidos
            bool todosIguais = true;
            for (int i = 1; i < digitos.Length; i++)
            {
                if (digitos[i] != digitos[0])
                {
                    todosIguais = false;
                    break;
                }
            }

            if (todosIguais)
                return false;

            int primeiroDigito = CalcularDigito(digitos, 9, 10);
            if (digitos[9] - '0' != primeiroDigito)
                return false;

            int segundoDigito = CalcularDigito(digitos, 10, 11);
            return digitos[10] - '0' == segundoDigito;
        }

        private static int CalcularDigito(string cpf, int quantidade, int pesoInicial)
        {
            int soma = 0;
            for (int i = 0; i < quantidade; i++)
                soma += (cpf[i] - '0') * (pesoInicial - i);

            int resto = soma % 11;
            return resto < 2 ? 0 : 11 - resto;
        }

        private static string SomenteDigitos(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return string.Empty;

            char[] resultado = new char[valor.Length];
            int quantidade = 0;
            foreach (char caractere in valor)
            {
                if (caractere >= '0' && caractere <= '9')
                    resultado[quantidade++] = caractere;
            }

            return new string(resultado, 0, quantidade);
        }
    }
}
