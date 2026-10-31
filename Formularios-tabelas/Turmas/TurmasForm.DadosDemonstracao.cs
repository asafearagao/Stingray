using Stingray.Formularios.Turmas.Modelos;

namespace Stingray
{
    public partial class TurmasForm
    {
        // Registros de demonstração/testes
        private void PreencherTurmasDeExemplo()
        {
            AdicionarTurmaDeExemplo(
                "Sexta-feira", "Natação A", "08:00 - 09:00", "Mariana Costa",
                new string[]
                {
                    "Ana Clara", "Bruno Lima", "Caio Mendes", "Daniela Souza", "Eduardo Costa", "Fernanda Alves",
                    "Gabriel Rocha", "Helena Martins", "Igor Santos", "Julia Ferreira", "Lucas Oliveira", "Marina Ribeiro"
                },
                9);

            AdicionarTurmaDeExemplo(
                "Sábado", "Hidroginástica", "09:30 - 10:30", "Camila Ferreira",
                new string[]
                {
                    "Adriana Lima", "Beatriz Souza", "Cecília Ramos", "Denise Costa", "Elaine Martins", "Fabiana Rocha",
                    "Gisele Alves", "Helena Melo", "Isabel Nunes", "Joana Prado", "Karen Dias", "Larissa Reis",
                    "Márcia Silva", "Neusa Costa", "Olga Ribeiro", "Paula Santos", "Quitéria Luz", "Renata Lima",
                    "Sônia Mendes", "Tereza Alves"
                },
                15);

            AdicionarTurmaDeExemplo(
                "Domingo", "Natação 8 a 14 anos", "18:00 - 19:00", "Rafael Mendes",
                new string[] { "Arthur Lima", "Bia Nunes", "Carlos Prado", "Davi Santos", "Enzo Rocha", "Felipe Melo", "Giovana Reis", "Heitor Dias" },
                6);

            AdicionarTurmaDeExemplo(
                "Segunda-feira", "Natação 5 a 7 anos", "14:00 - 15:00", "Lucas Almeida",
                new string[]
                {
                    "Alice Costa", "Bernardo Luz", "Clara Alves", "Daniel Silva", "Ester Prado", "Felipe Nunes",
                    "Giulia Rocha", "Hugo Reis", "Isabela Dias", "João Melo", "Kauã Santos", "Laura Lima",
                    "Miguel Costa", "Nina Martins", "Otávio Souza"
                },
                12);

            AplicarFiltrosTabela();
        }

        // Adiciona uma turma com os dados informados
        private void AdicionarTurmaDeExemplo(string dia, string modalidade, string horario, string professor, string[] alunos, int presentes)
        {
            int indice = dgvTurmas.Rows.Add(dia, modalidade, horario, professor, presentes.ToString());
            RegistroAlunosTurma registro = new RegistroAlunosTurma();
            registro.Alunos.AddRange(alunos);

            for (int indiceAluno = 0; indiceAluno < System.Math.Min(presentes, alunos.Length); indiceAluno++)
                registro.Presentes.Add(alunos[indiceAluno]);

            dgvTurmas.Rows[indice].Tag = registro;
        }
    }
}