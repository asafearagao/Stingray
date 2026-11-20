using System;

namespace Stingray.DadosTela
{

    // Guarda o estado de ativação e conexão do WhatsApp
    public class EstadoWhatsApp
    {
        public bool IntegracaoAtivada { get; set; }
        public bool UltimoEstadoConectado { get; set; }
        public DateTime AtualizadoEm { get; set; }
        public int DiasAposVencimento { get; set; } = 4;
    }
}
