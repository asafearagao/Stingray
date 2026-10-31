using System;
using System.Collections.Generic;
using Stingray.Modelos;
using Stingray.Formularios.Alunos.Modelos;

/* Este arquivo é temporário, mas exibe um exemplo simples de como criar uma lista de alunos
 * será a base para a criação do arquivo correto que irá carregar os dados reais do banco de dados
 * Lembre-sem de alterar as chamadas deste arquivo para o arquivo correto quando ele estiver pronto
 * Terão que susbtituir pelo do arquivo final
 */

namespace Stingray.Formularios.Alunos.DadosDemonstracao
{
    // Fornece registros de alunos utilizados para demonstração da tela
    internal static class AlunoDadosDemonstracao
    {
        // Cria a lista de alunos utilizada como dados de demonstração
        public static IList<AlunoListaItem> CriarAlunos()
        {
            return new List<AlunoListaItem>
            {
                CriarMariaSilva(),
                CriarJoaoPedro(),
                CriarAnaClara(),
                CriarPedroHenrique(),
                CriarLauraSouza(),
                CriarGabrielLima(),
                CriarBeatrizOliveira(),
                CriarMiguelFerreira(),
                CriarSofiaMartins()
            };
        }

        // Cria o registro de demonstração de Maria Silva
        private static AlunoListaItem CriarMariaSilva()
        {
            Aluno aluno = new Aluno
            {
                Nome = "Maria Silva",
                DataNascimento = new DateTime(1985, 3, 12),
                Telefone = "(19) 99876-5432",
                Numero = "120",
                Cep = "13025-000",
                Cidade = "Campinas",
                Responsavel = string.Empty,
                Cpf = "123.456.789-09",
                Rg = "12.345.678-9",
                Email = "maria.silva@email.com",
                DataCadastro = new DateTime(2024, 5, 20)
            };

            return CriarItem(aluno, "Hidroginástica", "Lucas Almeida", "Mensal", "Ativa");
        }

        // Cria o registro de demonstração de João Pedro
        private static AlunoListaItem CriarJoaoPedro()
        {
            Aluno aluno = new Aluno
            {
                Nome = "João Pedro",
                DataNascimento = new DateTime(2016, 7, 4),
                Telefone = "(19) 98765-4321",
                Numero = "450",
                Cep = "13073-001",
                Cidade = "Campinas",
                Responsavel = "Carla Pereira",
                Cpf = "234.567.890-10",
                Rg = "23.456.789-0",
                Email = "carla.pereira@email.com",
                DataCadastro = new DateTime(2024, 5, 18)
            };

            return CriarItem(aluno, "Natação Iniciante", "Ana Paula Costa", "Mensal", "Ativa");
        }

        // Cria o registro de demonstração de Ana Clara
        private static AlunoListaItem CriarAnaClara()
        {
            Aluno aluno = new Aluno
            {
                Nome = "Ana Clara",
                DataNascimento = new DateTime(2012, 11, 25),
                Telefone = "(19) 97654-3210",
                Numero = "85",
                Cep = "13015-001",
                Cidade = "Campinas",
                Responsavel = "Roberto Santos",
                Cpf = "345.678.901-21",
                Rg = "34.567.890-1",
                Email = "roberto.santos@email.com",
                DataCadastro = new DateTime(2024, 5, 22)
            };

            return CriarItem(aluno, "Natação Intermediário", "Gabriel Silva", "Trimestral", "Ativa");
        }

        // Cria o registro de demonstração de Pedro Henrique
        private static AlunoListaItem CriarPedroHenrique()
        {
            Aluno aluno = new Aluno
            {
                Nome = "Pedro Henrique",
                DataNascimento = new DateTime(2009, 1, 30),
                Telefone = "(19) 96543-2109",
                Numero = "301",
                Cep = "13076-000",
                Cidade = "Campinas",
                Responsavel = "Fernanda Alves",
                Cpf = "456.789.012-32",
                Rg = "45.678.901-2",
                Email = "fernanda.alves@email.com",
                DataCadastro = new DateTime(2024, 5, 21)
            };

            return CriarItem(aluno, "Natação Avançado", "Felipe Rocha", "Mensal", "Ativa");
        }

        // Cria o registro de demonstração de Laura Souza
        private static AlunoListaItem CriarLauraSouza()
        {
            Aluno aluno = new Aluno
            {
                Nome = "Laura Souza",
                DataNascimento = new DateTime(1992, 9, 8),
                Telefone = "(19) 95432-1098",
                Numero = "77",
                Cep = "13015-080",
                Cidade = "Campinas",
                Responsavel = string.Empty,
                Cpf = "567.890.123-43",
                Rg = "56.789.012-3",
                Email = "laura.souza@email.com",
                DataCadastro = new DateTime(2024, 5, 17)
            };

            return CriarItem(aluno, "Natação Adulto", "Mariana Oliveira", "Mensal", "Ativa");
        }

        // Cria o registro de demonstração de Gabriel Lima
        private static AlunoListaItem CriarGabrielLima()
        {
            Aluno aluno = new Aluno
            {
                Nome = "Gabriel Lima",
                DataNascimento = new DateTime(1978, 6, 15),
                Telefone = "(19) 94321-0987",
                Numero = "1500",
                Cep = "13035-000",
                Cidade = "Campinas",
                Responsavel = string.Empty,
                Cpf = "678.901.234-54",
                Rg = "67.890.123-4",
                Email = "gabriel.lima@email.com",
                DataCadastro = new DateTime(2024, 5, 19)
            };

            return CriarItem(aluno, "Natação Master", "Juliana Lima", "Trimestral", "Pausada");
        }

        // Cria o registro de demonstração de Beatriz Oliveira
        private static AlunoListaItem CriarBeatrizOliveira()
        {
            Aluno aluno = new Aluno
            {
                Nome = "Beatriz Oliveira",
                DataNascimento = new DateTime(2005, 12, 2),
                Telefone = "(19) 93210-9876",
                Numero = "640",
                Cep = "13070-010",
                Cidade = "Campinas",
                Responsavel = "Marcos Oliveira",
                Cpf = "789.012.345-65",
                Rg = "78.901.234-5",
                Email = "marcos.oliveira@email.com",
                DataCadastro = new DateTime(2024, 5, 16)
            };

            return CriarItem(aluno, "Natação Competição", "Rafael Santos", "Mensal", "Ativa");
        }

        // Cria o registro de demonstração de Miguel Ferreira
        private static AlunoListaItem CriarMiguelFerreira()
        {
            Aluno aluno = new Aluno
            {
                Nome = "Miguel Ferreira",
                DataNascimento = new DateTime(2017, 4, 19),
                Telefone = "(19) 92109-8765",
                Numero = "233",
                Cep = "13070-210",
                Cidade = "Campinas",
                Responsavel = "Patrícia Ferreira",
                Cpf = "890.123.456-76",
                Rg = "89.012.345-6",
                Email = "patricia.ferreira@email.com",
                DataCadastro = new DateTime(2024, 5, 15)
            };

            return CriarItem(aluno, "Natação Iniciante", "Ana Paula Costa", "Mensal", "Ativa");
        }

        // Cria o registro de demonstração de Sofia Martins
        private static AlunoListaItem CriarSofiaMartins()
        {
            Aluno aluno = new Aluno
            {
                Nome = "Sofia Martins",
                DataNascimento = new DateTime(1999, 8, 27),
                Telefone = "(19) 91098-7654",
                Numero = "18",
                Cep = "13092-120",
                Cidade = "Campinas",
                Responsavel = string.Empty,
                Cpf = "901.234.567-87",
                Rg = "90.123.456-7",
                Email = "sofia.martins@email.com",
                DataCadastro = new DateTime(2024, 5, 14)
            };

            return CriarItem(aluno, "Hidroginástica", "Lucas Almeida", "Mensal", "Ativa");
        }

        // Agrupa os dados do aluno com as informações da turma, do professor, do plano e do status
        private static AlunoListaItem CriarItem(
            Aluno aluno,
            string turma,
            string professor,
            string plano,
            string status)
        {
            return new AlunoListaItem(aluno, turma, professor, plano, status);
        }
    }
}