using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Stingray.DadosTela;
using Stingray.Servicos;

namespace Stingray
{
    [System.ComponentModel.DesignerCategory("Form")]

    // Permite consultar e atualizar os participantes de uma turma
    public partial class TurmaParticipantesForm : Form
    {
        private readonly int idTurma;
        private readonly int capacidade;
        private readonly List<ParticipanteTurma> participantes;
        private readonly HashSet<int> selecionados;
        private readonly TurmaServico turmaServico = new TurmaServico();

        public TurmaParticipantesForm()
        {
            InitializeComponent();
            EstiloPopup.Aplicar(this);
        }

        // Construtor que recebe os dados da turma e a lista de participantes
        public TurmaParticipantesForm(int idTurma, string modalidade, int capacidade,
            IList<ParticipanteTurma> participantes) : this()
        {
            this.idTurma = idTurma;
            this.capacidade = capacidade;
            // Se a lista de participantes for nula, cria uma lista vazia
            this.participantes = participantes == null
                ? new List<ParticipanteTurma>()
                : participantes.ToList();
            // Cria um conjunto de IDs de participantes selecionados
            selecionados = new HashSet<int>(this.participantes
                .Where(item => item.Selecionado)
                .Select(item => item.IdMatricula));

            // Define o título e a descrição do formulário, dependendo da modalidade
            lblModalidade.Text = modalidade ?? "Turma";
            lblEstadoVazio.Visible = this.participantes.Count == 0;
            txtPesquisa.TextChanged += delegate { AtualizarLista(); };
            lstParticipantes.ItemCheck += LstParticipantes_ItemCheck;
            btnSalvar.Click += BtnSalvar_Click;
            btnCancelar.Click += delegate { DialogResult = DialogResult.Cancel; Close(); };

            AtualizarLista();
            Shown += delegate { txtPesquisa.Focus(); };
        }

        // Atualiza a lista de participantes com base na pesquisa
        private void AtualizarLista()
        {
            string termo = txtPesquisa.Text.Trim();
            lstParticipantes.BeginUpdate();
            try
            {
                // Limpa a lista e adiciona os participantes filtrados
                lstParticipantes.Items.Clear();
                foreach (ParticipanteTurma participante in participantes)
                {
                    if (termo.Length > 0 && participante.Nome.IndexOf(
                        termo, StringComparison.CurrentCultureIgnoreCase) < 0)
                        continue;

                    int indice = lstParticipantes.Items.Add(participante);
                    lstParticipantes.SetItemChecked(indice, selecionados.Contains(participante.IdMatricula));
                }
            }
            finally
            {
                lstParticipantes.EndUpdate();
            }

            AtualizarResumo();
            lblEstadoVazio.Visible = participantes.Count == 0;
        }


        // Atualiza a lista de participantes quando o usuário marca ou desmarca um item
        private void LstParticipantes_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ParticipanteTurma participante = lstParticipantes.Items[e.Index] as ParticipanteTurma;
            if (participante == null)
                return;

            if (e.NewValue == CheckState.Checked)
                selecionados.Add(participante.IdMatricula);
            else
                selecionados.Remove(participante.IdMatricula);

            if (!IsDisposed && IsHandleCreated)
                BeginInvoke(new Action(AtualizarResumo));
        }

        private void AtualizarResumo()
        {
            lblResumo.Text = selecionados.Count + " de " + participantes.Count +
                " alunos com matrícula ativa selecionados";
        }

        // Salva as alterações dos participantes da turma
        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            // Validar loteamento de participantes antes de salvar
            if (selecionados.Count > capacidade)
            {
                MessageBox.Show(this,
                    "A quantidade selecionada ultrapassa a capacidade de " + capacidade + " alunos.",
                    "Capacidade da turma", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Salva os participantes selecionados no banco de dados
                turmaServico.SalvarParticipantes(idTurma, selecionados.ToList(), capacidade);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception erro)
            {
                MessageBox.Show(this,
                    "Não foi possível salvar os participantes no banco de dados.\n\n" + erro.Message,
                    "Falha ao salvar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
