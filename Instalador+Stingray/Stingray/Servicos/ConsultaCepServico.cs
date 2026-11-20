using System;
using System.Collections.Generic;
using System.Net.Http;                 // Biblioteca para requisições HTTP
using System.Text.RegularExpressions;
using System.Threading;                // Cancelamento de tarefas assíncronas
using System.Threading.Tasks;          // Tarefas assíncronas
using System.Web.Script.Serialization;
using Stingray.DadosTela;

namespace Stingray.Servicos
{
    // Consulta o endereço de um CEP no  ViaCEP
    // A consulta é feita pela Internet e não grava os dados no banco
    public class ConsultaCepServico
    {
        // Cria um cliente HTTP para acessar o ViaCEP
        private static readonly HttpClient ClienteHttp = CriarClienteHttp();

        private static HttpClient CriarClienteHttp()
        {
            return new HttpClient
            {
                BaseAddress = new Uri("https://viacep.com.br/ws/"),
                Timeout = TimeSpan.FromSeconds(12)
            };
        }

        // Consulta um CEP e retorna os dados encontrados
public async Task<ResultadoConsultaCep> ConsultarPorCepAsync(
            string cep, CancellationToken cancelamento = default(CancellationToken))
        {
            // Remove tudo que não for dígito do CEP
            string cepNumerico = Regex.Replace(cep ?? string.Empty, @"\D", string.Empty);

            if (cepNumerico.Length != 8)  // O CEP deve ter exatamente oito dígitos
                throw new ArgumentException("Informe um CEP com oito dígitos.", nameof(cep));

            // Consulta o ViaCEP e retorna um JSON
            using (HttpResponseMessage respostaHttp = await ClienteHttp
                .GetAsync(cepNumerico + "/json/", cancelamento)
                .ConfigureAwait(false))
            {
                // Se o código HTTP não for 200, lança uma erro/exceção
                if (!respostaHttp.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(
                        "O ViaCEP respondeu com o código HTTP " +
                        (int)respostaHttp.StatusCode + ".");
                }

                // Lê o conteúdo o JSON e coloca as informações em um dicionário
                string conteudo = await respostaHttp.Content.ReadAsStringAsync()
                    .ConfigureAwait(false);

                var serializador = new JavaScriptSerializer();
                Dictionary<string, object> dados = serializador
                    .Deserialize<Dictionary<string, object>>(conteudo);

                if (dados == null || CepNaoEncontrado(dados))
                    return null;

                // Retorna os dados achados, já que podem ser vazios, mesmo com CEP válido
                string cepRetornado = ObterTexto(dados, "cep");

                // Se estiver Null, foi que não encontrou o endereço
                if (string.IsNullOrWhiteSpace(cepRetornado))
                    return null;

                return new ResultadoConsultaCep
                {
                    Cep = cepRetornado,
                    Logradouro = ObterTexto(dados, "logradouro"),
                    Bairro = ObterTexto(dados, "bairro"),
                    Cidade = ObterTexto(dados, "localidade")
                };
            }
        }

        private static bool CepNaoEncontrado(Dictionary<string, object> dados)
        {
            object valorErro;

            // Se o dicionário tiver as chaves "erro" e "true", significa CEP inválido
            return dados.TryGetValue("erro", out valorErro) &&
                valorErro != null &&
                string.Equals(valorErro.ToString(), "true", StringComparison.OrdinalIgnoreCase);
        }

        // Obtém o valor de um campo do dicionário
        private static string ObterTexto(Dictionary<string, object> dados, string campo)
        {
            object valor;

            // Se o campo for Null, devolve string vazia
            if (!dados.TryGetValue(campo, out valor) || valor == null)
                return string.Empty;

            return valor.ToString();
        }
    }
}
