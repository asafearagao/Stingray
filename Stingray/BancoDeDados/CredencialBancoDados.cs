using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Stingray.BancoDeDados
{

    // Gerencia a leitura e a gravação segura da senha do banco no Windows
    internal static class CredencialBancoDados
    {
        private const string AlvoCredencial = "AquaFit.Stingray.MySql";
        private const uint TipoGenerica = 1;
        private const uint PersistenciaLocal = 2;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]

        // Gerencia a leitura e a gravação segura da senha do banco no Windows
        private struct CredencialNativa
        {
            public uint Flags;
            public uint Type;
            [MarshalAs(UnmanagedType.LPWStr)] public string TargetName;
            [MarshalAs(UnmanagedType.LPWStr)] public string Comment;
            public long LastWritten;
            public uint CredentialBlobSize;
            public IntPtr CredentialBlob;
            public uint Persist;
            public uint AttributeCount;
            public IntPtr Attributes;
            [MarshalAs(UnmanagedType.LPWStr)] public string TargetAlias;
            [MarshalAs(UnmanagedType.LPWStr)] public string UserName;
        }

        [DllImport("Advapi32.dll", EntryPoint = "CredReadW", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]

        private static extern bool CredRead(string target, uint type, uint flags, out IntPtr credential);

        [DllImport("Advapi32.dll", EntryPoint = "CredWriteW", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]

        private static extern bool CredWrite(ref CredencialNativa credential, uint flags);

        [DllImport("Advapi32.dll", EntryPoint = "CredFree", SetLastError = false)]

        private static extern void CredFree(IntPtr credential);

        // Recupera a senha armazenada no Gerenciador de Credenciais do Windows
        public static string ObterSenha()
        {
            string senha = LerGerenciadorCredenciais();
            if (!string.IsNullOrWhiteSpace(senha))
                return senha;

            return MigrarCredencialAntiga();
        }

        // Lê a credencial protegida sem expor a senha em arquivos de configuração
        private static string LerGerenciadorCredenciais()
        {
            IntPtr ponteiro;
            if (!CredRead(AlvoCredencial, TipoGenerica, 0, out ponteiro))
            {
                int codigo = Marshal.GetLastWin32Error();
                if (codigo == 1168)
                    return null;
                throw new Win32Exception(codigo, "Não foi possível consultar a credencial do MySQL no Gerenciador de Credenciais do Windows.");
            }

            try
            {
                CredencialNativa credencial = (CredencialNativa)Marshal.PtrToStructure(
                    ponteiro, typeof(CredencialNativa));
                if (!string.Equals(credencial.UserName, "stingray_app", StringComparison.Ordinal))
                    throw new InvalidOperationException(
                        "A credencial AquaFit.Stingray.MySql pertence a um usuário diferente de stingray_app. Execute o instalador na mesma conta do Windows utilizada pelo Stingray.");

                if (credencial.CredentialBlob == IntPtr.Zero || credencial.CredentialBlobSize == 0)
                    return null;

                int tamanhoCredencial = checked((int)credencial.CredentialBlobSize);
                byte[] bytes = new byte[tamanhoCredencial];
                try
                {
                    Marshal.Copy(credencial.CredentialBlob, bytes, 0, bytes.Length);
                    return Encoding.Unicode.GetString(bytes).TrimEnd('\0');
                }
                finally
                {
                    Array.Clear(bytes, 0, bytes.Length);
                }
            }
            finally
            {
                CredFree(ponteiro);
            }
        }

        // Grava a senha no armazenamento seguro do Windows
        public static void SalvarSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
                throw new ArgumentException("A senha do usuário de banco de dados não pode ficar vazia.", nameof(senha));

            byte[] bytes = Encoding.Unicode.GetBytes(senha);
            GCHandle fixado = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                CredencialNativa credencial = new CredencialNativa
                {
                    Type = TipoGenerica,
                    TargetName = AlvoCredencial,
                    UserName = "stingray_app",
                    CredentialBlobSize = (uint)bytes.Length,
                    CredentialBlob = fixado.AddrOfPinnedObject(),
                    Persist = PersistenciaLocal
                };

                if (!CredWrite(ref credencial, 0))
                    throw new Win32Exception(Marshal.GetLastWin32Error(),
                        "Não foi possível salvar a credencial do MySQL no Gerenciador de Credenciais do Windows.");
            }
            finally
            {
                fixado.Free();
                Array.Clear(bytes, 0, bytes.Length);
            }
        }

        // Migra uma credencial antiga para o identificador atualmente utilizado
        private static string MigrarCredencialAntiga()
        {
            string caminho = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Stingray", "Seguranca", "mysql.cred");

            if (!File.Exists(caminho))
                throw new InvalidOperationException(
                    "A credencial do MySQL não foi encontrada no Gerenciador de Credenciais do Windows. Execute o instalador atualizado do Stingray.");

            byte[] protegidos = null;
            byte[] originais = null;
            try
            {
                string conteudo = File.ReadAllText(caminho).Trim();
                if (string.IsNullOrWhiteSpace(conteudo) || conteudo.Length % 2 != 0)
                    throw new InvalidDataException("A credencial antiga possui um formato inválido.");

                protegidos = new byte[conteudo.Length / 2];
                for (int i = 0; i < protegidos.Length; i++)
                    protegidos[i] = Convert.ToByte(conteudo.Substring(i * 2, 2), 16);

                originais = ProtectedData.Unprotect(protegidos, null, DataProtectionScope.CurrentUser);
                string senha = Encoding.Unicode.GetString(originais).TrimEnd('\0');
                SalvarSenha(senha);

                File.Delete(caminho);
                string pasta = Path.GetDirectoryName(caminho);
                string credencialAdmin = Path.Combine(pasta, "mysql.admin.cred");
                if (File.Exists(credencialAdmin))
                    File.Delete(credencialAdmin);
                if (Directory.Exists(pasta) && Directory.GetFiles(pasta).Length == 0 && Directory.GetDirectories(pasta).Length == 0)
                    Directory.Delete(pasta);
                return senha;
            }
            catch (Exception erro)
            {
                throw new InvalidOperationException(
                    "Não foi possível migrar a credencial antiga para o Gerenciador de Credenciais do Windows. Execute o instalador atualizado ou verifique se está usando a mesma conta do Windows.", erro);
            }
            finally
            {
                if (protegidos != null) Array.Clear(protegidos, 0, protegidos.Length);
                if (originais != null) Array.Clear(originais, 0, originais.Length);
            }
        }
    }
}
