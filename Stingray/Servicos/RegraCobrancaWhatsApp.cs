using System;
using System.Globalization;
using System.Linq;
using Stingray.DadosTela;

namespace Stingray.Servicos
{

    // Define quando uma cobrança pode ser enviada ou repetida
    public static class RegraCobrancaWhatsApp
    {

        // Define quais situações permitem uma nova tentativa de envio
        public static bool PermiteNovaTentativa(string situacaoExistente)
        {
            return string.IsNullOrWhiteSpace(situacaoExistente) ||
                string.Equals(situacaoExistente, "Falha antes do envio", StringComparison.OrdinalIgnoreCase);
        }

        // Libera o envio somente por ação do usuário ou durante o encerramento do aplicativo
        public static bool PodeProcessarFila(bool envioAutorizado, bool encerrandoAplicativo)
        {
            return envioAutorizado || encerrandoAplicativo;
        }

        // Confere se o vencimento já ultrapassou o prazo configurado
        public static bool EstaElegivel(DateTime vencimento, DateTime hoje, int diasAposVencimento)
        {
            if (vencimento == DateTime.MinValue || hoje == DateTime.MinValue)
                return false;
            if (diasAposVencimento < 0 || diasAposVencimento > 365)
                throw new ArgumentOutOfRangeException(nameof(diasAposVencimento));

            return hoje.Date >= vencimento.Date.AddDays(diasAposVencimento);
        }
    }

    // Define quando uma cobrança pode ser enviada ou repetida
    public static class CobrancaWhatsAppMontador
    {

        // Monta a mensagem e identifica o destinatário adequado ao aluno
        public static bool TentarMontar(PagamentoLinha pagamento, DateTime hoje,
            out string telefone, out string mensagem, out string motivo)
        {
            telefone = string.Empty;
            mensagem = string.Empty;
            motivo = string.Empty;

            if (pagamento == null)
            {
                motivo = "Cobrança não informada.";
                return false;
            }

            if (pagamento.DataNascimentoAluno == DateTime.MinValue || pagamento.DataNascimentoAluno.Date > hoje.Date)
            {
                motivo = "A data de nascimento do aluno não está válida.";
                return false;
            }

            bool maiorDeIdade = CalcularIdade(pagamento.DataNascimentoAluno, hoje) >= 18;
            string responsavel = (pagamento.NomeResponsavel ?? string.Empty).Trim();
            if (!maiorDeIdade && responsavel.Length == 0)
            {
                motivo = "O cadastro do menor não contém o nome do responsável.";
                return false;
            }

            string telefoneCadastrado = maiorDeIdade || string.IsNullOrWhiteSpace(pagamento.TelefoneResponsavel)
                ? pagamento.TelefoneContato
                : pagamento.TelefoneResponsavel;
            telefone = NormalizarTelefone(telefoneCadastrado);
            if (telefone.Length == 0)
            {
                motivo = "Não há um telefone válido para esta cobrança.";
                return false;
            }

            CultureInfo cultura = CultureInfo.GetCultureInfo("pt-BR");
            if (maiorDeIdade)
            {
                mensagem = string.Format(cultura,
                    "Olá, {0}!\n\nIdentificamos uma mensalidade em aberto referente à competência {1}.\n\nValor total a pagar: {2:C2}\n\nCaso já tenha realizado o pagamento, por favor, desconsidere esta mensagem.\n\nObrigado!",
                    pagamento.NomeAluno, pagamento.MesReferencia.ToString("MM/yyyy"), pagamento.Valor);
            }
            else
            {
                mensagem = string.Format(cultura,
                    "Olá, {0}!\n\nIdentificamos uma mensalidade em aberto referente ao(à) aluno(a) {1}.\n\nCompetência: {2}\nValor total a pagar: {3:C2}\n\nCaso já tenha realizado o pagamento, por favor, desconsidere esta mensagem.\n\nObrigado!",
                    responsavel, pagamento.NomeAluno, pagamento.MesReferencia.ToString("MM/yyyy"), pagamento.Valor);
            }

            return true;
        }

        // Calcula a idade na data usada pela regra de cobrança
        private static int CalcularIdade(DateTime nascimento, DateTime hoje)
        {
            int idade = hoje.Year - nascimento.Year;
            if (nascimento.Date > hoje.Date.AddYears(-idade))
                idade--;
            return idade;
        }

        // Remove a formatação e prepara o número para o WhatsApp
        private static string NormalizarTelefone(string telefone)
        {
            string digitos = new string((telefone ?? string.Empty).Where(char.IsDigit).ToArray());
            if (digitos.Length == 10 || digitos.Length == 11)
                digitos = "55" + digitos;
            return digitos.Length >= 12 && digitos.Length <= 13 ? digitos : string.Empty;
        }
    }
}
