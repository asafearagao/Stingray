using System;

namespace Stingray.DadosTela
{

    // Representa o registro persistente de uma cobrança pelo WhatsApp
    public class WhatsAppEnvio
    {
        public Guid IdEnvio { get; set; }
        public int IdMensalidade { get; set; }
        public string Telefone { get; set; }
        public string Mensagem { get; set; }
        public string Situacao { get; set; }
    }
}
