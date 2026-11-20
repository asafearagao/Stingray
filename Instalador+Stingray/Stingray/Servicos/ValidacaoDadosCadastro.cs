using System;
using Stingray.Entidades;

namespace Stingray.Servicos
{
    // Validações compartilhadas entre os tipos de entidades de pessoas
    internal static class ValidacaoDadosCadastro
    {
        private const int TamanhoNome = 50;
        private const int TamanhoRg = 14;
        private const int TamanhoEmail = 100;
        private const int TamanhoNumeroEndereco = 5;
        private const int TamanhoComplemento = 50;

        public static void ValidarPessoa(Pessoa pessoa)
        {
            if (pessoa == null)
                throw new ArgumentException("Os dados pessoais estão incompletos.");

            // Valida o tamanho do nome da pessoa
            if (string.IsNullOrWhiteSpace(pessoa.Nome) || pessoa.Nome.Trim().Length > TamanhoNome)
                throw new ArgumentException("Informe o nome completo (até 50 caracteres).");

            // Valida a data de nascimento da pessoa
            if (pessoa.DataNascimento == DateTime.MinValue || pessoa.DataNascimento.Date > DateTime.Today)
                throw new ArgumentException("Informe uma data de nascimento válida.");

            // Confere os dois dígitos verificadores do CPF
            if (!CpfValidador.EhValido(pessoa.Cpf))
                throw new ArgumentException("O CPF informado é inválido. Confira os números digitados.");

            // Valida o tamanho do RG da pessoa
            if (string.IsNullOrWhiteSpace(pessoa.Rg) || pessoa.Rg.Trim().Length > TamanhoRg)
                throw new ArgumentException("Informe o RG (até 14 caracteres).");

            // Valida o tamanho do e-mail da pessoa
            if (!string.IsNullOrWhiteSpace(pessoa.Email) && pessoa.Email.Trim().Length > TamanhoEmail)
                throw new ArgumentException("O e-mail deve ter até 100 caracteres.");
        }

        public static void ValidarEndereco(Endereco endereco)
        {
            // Valida se o endereço não é nulo
            if (endereco == null)
                throw new ArgumentException("Informe o endereço.");

            // Valida o tamanho do logradouro do endereço
            if (string.IsNullOrWhiteSpace(endereco.Numero) || endereco.Numero.Trim().Length > TamanhoNumeroEndereco)
                throw new ArgumentException("Informe o número do endereço (até 5 caracteres).");

            // Valida o tamanho do CEP do endereço
            if (SomenteDigitos(endereco.Cep).Length != 8)
                throw new ArgumentException("O CEP deve conter oito números.");

            // Valida o tamanho do complemento do endereço
            if (!string.IsNullOrWhiteSpace(endereco.Complemento) &&
                endereco.Complemento.Trim().Length > TamanhoComplemento)
            {
                throw new ArgumentException("O complemento deve ter até 50 caracteres.");
            }
        }

        public static void ValidarTelefone(Telefone telefone)
        {
            // Opcional (Null), mas se informado deve ser válido
            string numero = SomenteDigitos(telefone == null ? null : telefone.NumeroTelefone);

            // Valida se o telefone tem 10 ou 11 dígitos (DDD + número)
            if (numero.Length > 0 && numero.Length != 10 && numero.Length != 11)
                throw new ArgumentException("O telefone deve conter DDD e número completo.");
        }

        // Retira os caracteres de máscara, como parênteses e hífen
        public static string SomenteDigitos(string valor)
        {
            // Se o valor for nulo ou vazio, retorna vazio
            if (string.IsNullOrEmpty(valor))
                return string.Empty;

            char[] digitos = new char[valor.Length];
            int quantidade = 0;

            // Percorre cada caractere da string e adiciona somente os dígitos ao array
            foreach (char caractere in valor)
            {
                if (char.IsDigit(caractere))
                    digitos[quantidade++] = caractere;
            }

            return new string(digitos, 0, quantidade);
        }
    }
}
