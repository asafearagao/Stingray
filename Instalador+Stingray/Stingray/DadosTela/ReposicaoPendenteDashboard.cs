using System;

namespace Stingray.DadosTela
{

    // Representa uma reposição que ainda precisa ser realizada
    public class ReposicaoPendenteDashboard
    {
        public int IdAluno { get; set; }
        public int IdMatricula { get; set; }
        public int IdOcorrenciaOriginal { get; set; }
        public int IdTurmaOrigem { get; set; }
        public int IdTurmaDestino { get; set; }
        public string Nome { get; set; }
        public string ModalidadeOriginal { get; set; }
        public DateTime DataOriginal { get; set; }
        public DateTime DataDestino { get; set; }
        public string Destino { get; set; }
    }
}
