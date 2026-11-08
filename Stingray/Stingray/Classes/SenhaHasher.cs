using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Stingray.Classes
{
    public static class SenhaHasher
    {
        private const int TamanhoSalt = 16;
        private const int TamanhoHash = 32;
        private const int Iteracoes = 600000;

        public static (string Hash, string Salt) GerarHash(string senha)
        {
            byte[] salt = new byte[TamanhoSalt];

            using (var gerador = RandomNumberGenerator.Create())
            {
                gerador.GetBytes(salt);
            }

            byte[] hash;

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                senha,
                salt,
                Iteracoes,
                HashAlgorithmName.SHA256))
            {
                hash = pbkdf2.GetBytes(TamanhoHash);
            }

            return (
                Convert.ToBase64String(hash),
                Convert.ToBase64String(salt)
            );
        }

        public static bool VerificarSenha(
            string senhaDigitada,
            string hashArmazenado,
            string saltArmazenado)
        {
            byte[] salt = Convert.FromBase64String(saltArmazenado);
            byte[] hashOriginal = Convert.FromBase64String(hashArmazenado);

            byte[] hashCalculado;

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
    }
}
