using System;
using System.Security.Cryptography;

namespace Stingray.BancoDeDados
{

    // Gera e confere os hashes usados na autenticação de usuários
    public sealed class ResultadoHashSenha
    {
        public string Hash { get; private set; }
        public string Salt { get; private set; }

        internal ResultadoHashSenha(string hash, string salt)
        {
            Hash = hash;
            Salt = salt;
        }
    }

    // Gera e confere os hashes usados na autenticação de usuários
    public static class SenhaHasher
    {
        private const int TamanhoSalt = 16;
        private const int TamanhoHash = 32;
        private const int Iteracoes = 600000;

        // Gera um salt e calcula o hash da senha
        public static ResultadoHashSenha GerarHash(string senha)
        {
            if (string.IsNullOrEmpty(senha))
                throw new ArgumentException("A senha não pode estar vazia.", nameof(senha));

            byte[] salt = new byte[TamanhoSalt];
            byte[] hash;

            using (var gerador = RandomNumberGenerator.Create())
            {
                gerador.GetBytes(salt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                senha,
                salt,
                Iteracoes,
                HashAlgorithmName.SHA256))
            {
                hash = pbkdf2.GetBytes(TamanhoHash);
            }

            try
            {
                return new ResultadoHashSenha(
                    Convert.ToBase64String(hash),
                    Convert.ToBase64String(salt));
            }
            finally
            {
                Array.Clear(hash, 0, hash.Length);
                Array.Clear(salt, 0, salt.Length);
            }
        }

        // Confere a senha informada com o hash armazenado
        public static bool VerificarSenha(
            string senhaDigitada,
            string hashArmazenado,
            string saltArmazenado)
        {
            if (string.IsNullOrEmpty(senhaDigitada) ||
                string.IsNullOrWhiteSpace(hashArmazenado) ||
                string.IsNullOrWhiteSpace(saltArmazenado))
            {
                return false;
            }

            byte[] salt = Convert.FromBase64String(saltArmazenado);
            byte[] hashOriginal = Convert.FromBase64String(hashArmazenado);
            byte[] hashCalculado = null;

            try
            {
                using (var pbkdf2 = new Rfc2898DeriveBytes(
                    senhaDigitada,
                    salt,
                    Iteracoes,
                    HashAlgorithmName.SHA256))
                {
                    hashCalculado = pbkdf2.GetBytes(TamanhoHash);
                }

                if (hashOriginal.Length != hashCalculado.Length)
                    return false;

                int diferenca = 0;
                for (int i = 0; i < hashOriginal.Length; i++)
                {
                    diferenca |= hashOriginal[i] ^ hashCalculado[i];
                }

                return diferenca == 0;
            }
            finally
            {
                Array.Clear(salt, 0, salt.Length);
                Array.Clear(hashOriginal, 0, hashOriginal.Length);

                if (hashCalculado != null)
                    Array.Clear(hashCalculado, 0, hashCalculado.Length);
            }
        }
    }
}
