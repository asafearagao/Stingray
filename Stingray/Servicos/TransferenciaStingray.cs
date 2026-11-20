using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using Stingray.BancoDeDados;

namespace Stingray.Servicos
{
    // Prepara os arquivos necessários para levar o Stingray a outro computador
    public static class TransferenciaStingray
    {
        private const string NomeInstalador = "AquaFit.msi";
        private const string NomeBanco = "teste_projeto.sql";

        // Localiza a cópia do pacote MSI guardada pelo instalador do Windows
        public static string ObterCaminhoInstalador()
        {
            string pastaLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string caminho = Path.Combine(pastaLocal, "Stingray", "Instalador", NomeInstalador);
            if (!File.Exists(caminho))
                throw new FileNotFoundException(
                    "O pacote MSI do AquaFit não foi encontrado. Reinstale o programa usando o instalador oficial do Windows.",
                    caminho);

            return caminho;
        }

        // Gera um nome de arquivo livre sem substituir arquivos existentes
        public static string CriarCaminhoLivre(string pasta, string nome, string extensao)
        {
            string caminho = Path.Combine(pasta, nome + extensao);
            int numero = 1;

            while (File.Exists(caminho))
            {
                caminho = Path.Combine(pasta, nome + "_" + numero + extensao);
                numero++;
            }

            return caminho;
        }

        // Copia somente o instalador para a pasta escolhida
        public static void CopiarSomenteInstalador(string caminhoDestino)
        {
            if (string.IsNullOrWhiteSpace(caminhoDestino))
                throw new ArgumentException("Escolha uma pasta de destino.", nameof(caminhoDestino));

            string origem = ObterCaminhoInstalador();
            string pasta = Path.GetDirectoryName(caminhoDestino);
            Directory.CreateDirectory(pasta);
            File.Copy(origem, caminhoDestino, false);
        }

        // Cria um pacote com o instalador e uma cópia lógica do banco de dados
        public static void CriarPacoteCompleto(string caminhoDestino)
        {
            if (string.IsNullOrWhiteSpace(caminhoDestino))
                throw new ArgumentException("Escolha uma pasta de destino.", nameof(caminhoDestino));

            string instalador = ObterCaminhoInstalador();
            string pastaDestino = Path.GetDirectoryName(caminhoDestino);
            string pastaTemporaria = Path.Combine(
                Path.GetTempPath(), "Stingray-Transferencia-" + Guid.NewGuid().ToString("N"));
            string pastaDados = Path.Combine(pastaTemporaria, "dados");
            string pastaSeguranca = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Stingray", "Transferencia");
            string arquivoOpcoes = Path.Combine(pastaSeguranca, Guid.NewGuid().ToString("N") + ".ini");
            string arquivoBanco = Path.Combine(pastaDados, NomeBanco);
            string arquivoZipTemporario = Path.Combine(
                pastaDestino, ".Stingray-" + Guid.NewGuid().ToString("N") + ".zip.tmp");

            try
            {
                Directory.CreateDirectory(pastaDestino);
                Directory.CreateDirectory(pastaDados);
                Directory.CreateDirectory(pastaSeguranca);

                File.Copy(instalador, Path.Combine(pastaTemporaria, NomeInstalador), false);
                CriarArquivoOpcoes(arquivoOpcoes);
                GerarBackupBanco(arquivoOpcoes, arquivoBanco);

                string resumo =
                    "TRANSFERÊNCIA DO AQUAFIT" + Environment.NewLine + Environment.NewLine +
                    "O pacote contém o instalador e uma cópia do banco de dados teste_projeto." + Environment.NewLine +
                    "No computador de destino, extraia todos os arquivos e execute Instalar-AquaFit-Com-Dados.cmd." + Environment.NewLine +
                    "Esse comando abre o Windows Installer e envia o backup para restauração durante a instalação." + Environment.NewLine + Environment.NewLine +
                    "Arquivo do banco: dados\\" + NomeBanco + Environment.NewLine +
                    "SHA256: " + CalcularHash(arquivoBanco) + Environment.NewLine + Environment.NewLine +
                    "Este pacote contém dados dos usuários. Guarde-o em local seguro e não o compartilhe publicamente.";

                File.WriteAllText(
                    Path.Combine(pastaTemporaria, "LEIA-ME.txt"),
                    resumo,
                    new UTF8Encoding(true));
                File.WriteAllText(
                    Path.Combine(pastaTemporaria, "manifesto.sha256"),
                    "Banco=teste_projeto" + Environment.NewLine +
                    "Arquivo=dados\\" + NomeBanco + Environment.NewLine +
                    "SHA256=" + CalcularHash(arquivoBanco) + Environment.NewLine,
                    Encoding.ASCII);

                string instaladorComDados =
                    "@echo off\r\n" +
                    "setlocal\r\n" +
                    "set \"PASTA=%~dp0\"\r\n" +
                    "set \"MSI=%PASTA%" + NomeInstalador + "\"\r\n" +
                    "set \"BANCO=%PASTA%dados\\" + NomeBanco + "\"\r\n" +
                    "if not exist \"%MSI%\" (echo O arquivo AquaFit.msi nao foi encontrado.& pause & exit /b 1)\r\n" +
                    "if not exist \"%BANCO%\" (echo O backup do banco nao foi encontrado.& pause & exit /b 1)\r\n" +
                    "msiexec.exe /i \"%MSI%\" RESTOREFILE=\"%BANCO%\" /L*v \"%TEMP%\\AquaFit-Transferencia.log\"\r\n" +
                    "set \"RESULTADO=%ERRORLEVEL%\"\r\n" +
                    "if \"%RESULTADO%\"==\"0\" (echo Instalacao e restauracao concluidas.) else (echo A instalacao terminou com o codigo %RESULTADO%. Consulte %TEMP%\\AquaFit-Transferencia.log.)\r\n" +
                    "pause\r\n";
                File.WriteAllText(
                    Path.Combine(pastaTemporaria, "Instalar-AquaFit-Com-Dados.cmd"),
                    instaladorComDados,
                    Encoding.ASCII);

                ZipFile.CreateFromDirectory(
                    pastaTemporaria,
                    arquivoZipTemporario,
                    CompressionLevel.Optimal,
                    false);

                // A troca de nome falha se o destino já existir, sem substituir o arquivo.
                File.Move(arquivoZipTemporario, caminhoDestino);
            }
            finally
            {
                if (File.Exists(arquivoOpcoes))
                    File.Delete(arquivoOpcoes);

                if (File.Exists(arquivoZipTemporario))
                    File.Delete(arquivoZipTemporario);

                if (Directory.Exists(pastaTemporaria))
                    Directory.Delete(pastaTemporaria, true);
            }
        }

        // Cria um arquivo temporário com os dados de acesso usados pelo MySQL
        private static void CriarArquivoOpcoes(string caminho)
        {
            string senha = CredencialBancoDados.ObterSenha();
            string senhaProtegida = senha.Replace("\\", "\\\\").Replace("\"", "\\\"");
            string conteudo =
                "[client]" + Environment.NewLine +
                "host=127.0.0.1" + Environment.NewLine +
                "port=3307" + Environment.NewLine +
                "protocol=TCP" + Environment.NewLine +
                "user=stingray_app" + Environment.NewLine +
                "password=\"" + senhaProtegida + "\"" + Environment.NewLine +
                "default-character-set=utf8mb4" + Environment.NewLine;

            File.WriteAllText(caminho, conteudo, Encoding.ASCII);
        }

        // Exporta o banco sem interromper o serviço do MySQL
        private static void GerarBackupBanco(string arquivoOpcoes, string arquivoBanco)
        {
            string pastaProgramas = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string executavel = Path.Combine(pastaProgramas, "Stingray", "MySQL", "bin", "mysqldump.exe");

            if (!File.Exists(executavel))
                throw new FileNotFoundException(
                    "O utilitário de backup do MySQL não foi encontrado. Verifique a instalação do banco de dados.",
                    executavel);

            string argumentos =
                "--defaults-extra-file=\"" + arquivoOpcoes + "\" " +
                "--no-login-paths --single-transaction --skip-lock-tables " +
                "--no-tablespaces --set-gtid-purged=OFF --hex-blob " +
                "--skip-triggers --databases teste_projeto " +
                "--result-file=\"" + arquivoBanco + "\"";

            using (Process processo = new Process())
            {
                processo.StartInfo = new ProcessStartInfo
                {
                    FileName = executavel,
                    Arguments = argumentos,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                if (!processo.Start())
                    throw new InvalidOperationException("Não foi possível iniciar o backup do banco de dados.");

                var saida = processo.StandardOutput.ReadToEndAsync();
                var erro = processo.StandardError.ReadToEndAsync();

                if (!processo.WaitForExit(15 * 60 * 1000))
                {
                    try { processo.Kill(); } catch { }
                    throw new TimeoutException("O backup do banco ultrapassou o limite de 15 minutos.");
                }

                processo.WaitForExit();
                string detalhes = (erro.GetAwaiter().GetResult() + " " + saida.GetAwaiter().GetResult()).Trim();
                if (processo.ExitCode != 0)
                    throw new InvalidOperationException("Não foi possível gerar o backup do banco. " + detalhes);
            }

            if (!File.Exists(arquivoBanco) || new FileInfo(arquivoBanco).Length < 100)
                throw new InvalidDataException("O arquivo de backup foi gerado vazio ou incompleto.");
        }

        // Calcula uma identificação do arquivo para conferir sua integridade
        private static string CalcularHash(string caminho)
        {
            using (SHA256 sha = SHA256.Create())
            using (FileStream arquivo = File.OpenRead(caminho))
            {
                byte[] hash = sha.ComputeHash(arquivo);
                StringBuilder texto = new StringBuilder(hash.Length * 2);
                foreach (byte valor in hash)
                    texto.Append(valor.ToString("x2"));
                return texto.ToString();
            }
        }
    }
}
