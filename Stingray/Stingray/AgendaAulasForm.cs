using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Stingray
{
    // ---------- Dados ----------
    public class Aula
    {
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public string Atividade { get; set; }
        public string Professor { get; set; }
    }

    public static class AulasRepositorio
    {
        public static List<Aula> Aulas = new List<Aula>();

        static AulasRepositorio()
        {
            DateTime hoje = DateTime.Today;
            Aulas.Add(new Aula { Data = hoje, Hora = new TimeSpan(8, 0, 0), Atividade = "Hidroginástica", Professor = "Lucas Almeida" });
            Aulas.Add(new Aula { Data = hoje, Hora = new TimeSpan(9, 0, 0), Atividade = "Natação Iniciante", Professor = "Ana Paula Costa" });
            Aulas.Add(new Aula { Data = hoje, Hora = new TimeSpan(10, 0, 0), Atividade = "Natação Intermediário", Professor = "Gabriel Silva" });
            Aulas.Add(new Aula { Data = hoje, Hora = new TimeSpan(14, 0, 0), Atividade = "Natação Avançado", Professor = "Felipe Rocha" });
            Aulas.Add(new Aula { Data = hoje.AddDays(1), Hora = new TimeSpan(9, 0, 0), Atividade = "Natação Iniciante", Professor = "Ana Paula Costa" });
            Aulas.Add(new Aula { Data = hoje.AddDays(1), Hora = new TimeSpan(15, 30, 0), Atividade = "Hidroginástica", Professor = "Lucas Almeida" });
        }

        public static List<Aula> DoDia(DateTime dia)
        {
            return Aulas.Where(a => a.Data.Date == dia.Date).OrderBy(a => a.Hora).ToList();
        }
    }

    // ---------- Tela principal de horários ----------
    public class AgendaAulasForm : Form
    {
        private DateTime diaConsultado = DateTime.Today;
        private List<Aula> aulasExibidas = new List<Aula>();
        private Label lblDia;
        private Label lblResumo;
        private DataGridView grade;

        private readonly Color azul = Color.FromArgb(37, 56, 190);
        private readonly Color azulEscuro = Color.FromArgb(26, 35, 126);

        public AgendaAulasForm()
        {
            Text = "Horários das aulas";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(660, 500);
            MinimumSize = new Size(600, 420);
            BackColor = Color.White;
            Font = new Font("Segoe UI", 10F);

            // Grade (adicionada primeiro porque é Dock Fill)
            grade = new DataGridView();
            grade.Dock = DockStyle.Fill;
            grade.ReadOnly = true;
            grade.AllowUserToAddRows = false;
            grade.AllowUserToDeleteRows = false;
            grade.AllowUserToResizeRows = false;
            grade.MultiSelect = false;
            grade.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grade.RowHeadersVisible = false;
            grade.BackgroundColor = Color.White;
            grade.BorderStyle = BorderStyle.None;
            grade.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grade.GridColor = Color.FromArgb(230, 230, 230);
            grade.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grade.EnableHeadersVisualStyles = false;
            grade.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grade.ColumnHeadersHeight = 36;
            grade.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            grade.ColumnHeadersDefaultCellStyle.ForeColor = azulEscuro;
            grade.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grade.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White;
            grade.ColumnHeadersDefaultCellStyle.SelectionForeColor = azulEscuro;
            grade.DefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 231, 255);
            grade.DefaultCellStyle.SelectionForeColor = Color.Black;
            grade.RowTemplate.Height = 38;

            grade.Columns.Add("colHora", "Horário");
            grade.Columns.Add("colAtividade", "Atividade");
            grade.Columns.Add("colProfessor", "Professor");
            grade.Columns[0].FillWeight = 20;
            grade.Columns[1].FillWeight = 42;
            grade.Columns[2].FillWeight = 38;
            grade.Columns[0].DefaultCellStyle.ForeColor = azul;

            grade.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) EditarAula(); };

            // Topo
            Panel topo = new Panel();
            topo.Dock = DockStyle.Top;
            topo.Height = 76;
            topo.BackColor = Color.White;

            Button btnAnterior = CriarBotao("◀", 16, 22, 40, 32, Color.White, azulEscuro, true);
            Button btnProximo = CriarBotao("▶", 60, 22, 40, 32, Color.White, azulEscuro, true);
            Button btnHoje = CriarBotao("Hoje", 104, 22, 64, 32, Color.White, azulEscuro, true);
            btnAnterior.Click += (s, e) => { diaConsultado = diaConsultado.AddDays(-1); AtualizarTela(); };
            btnProximo.Click += (s, e) => { diaConsultado = diaConsultado.AddDays(1); AtualizarTela(); };
            btnHoje.Click += (s, e) => { diaConsultado = DateTime.Today; AtualizarTela(); };

            lblDia = new Label();
            lblDia.AutoSize = true;
            lblDia.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblDia.ForeColor = azulEscuro;
            lblDia.Location = new Point(184, 14);

            lblResumo = new Label();
            lblResumo.AutoSize = true;
            lblResumo.ForeColor = Color.Gray;
            lblResumo.Location = new Point(186, 44);

            topo.Controls.Add(btnAnterior);
            topo.Controls.Add(btnProximo);
            topo.Controls.Add(btnHoje);
            topo.Controls.Add(lblDia);
            topo.Controls.Add(lblResumo);

            // Rodapé
            Panel rodape = new Panel();
            rodape.Dock = DockStyle.Bottom;
            rodape.Height = 64;
            rodape.BackColor = Color.White;

            Button btnNova = CriarBotao("+ Nova aula", 16, 14, 130, 36, azul, Color.White, false);
            Button btnEditar = CriarBotao("Editar", 154, 14, 100, 36, Color.White, azul, true);
            Button btnExcluir = CriarBotao("Excluir", 262, 14, 100, 36, Color.White, Color.Firebrick, true);
            btnNova.Click += (s, e) => NovaAula();
            btnEditar.Click += (s, e) => EditarAula();
            btnExcluir.Click += (s, e) => ExcluirAula();

            rodape.Controls.Add(btnNova);
            rodape.Controls.Add(btnEditar);
            rodape.Controls.Add(btnExcluir);

            Controls.Add(grade);
            Controls.Add(topo);
            Controls.Add(rodape);

            AtualizarTela();
        }

        private Button CriarBotao(string texto, int x, int y, int largura, int altura, Color fundo, Color cor, bool comBorda)
        {
            Button b = new Button();
            b.Text = texto;
            b.Location = new Point(x, y);
            b.Size = new Size(largura, altura);
            b.BackColor = fundo;
            b.ForeColor = cor;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = comBorda ? 1 : 0;
            b.FlatAppearance.BorderColor = cor;
            b.Cursor = Cursors.Hand;
            return b;
        }

        private void AtualizarTela()
        {
            string texto = diaConsultado.ToString("dddd, dd 'de' MMMM 'de' yyyy", new CultureInfo("pt-BR"));
            texto = char.ToUpper(texto[0]) + texto.Substring(1);
            if (diaConsultado.Date == DateTime.Today) texto += "  (hoje)";
            lblDia.Text = texto;

            aulasExibidas = AulasRepositorio.DoDia(diaConsultado);

            grade.Rows.Clear();
            foreach (Aula a in aulasExibidas)
            {
                grade.Rows.Add(a.Hora.ToString(@"hh\:mm"), a.Atividade, "Prof. " + a.Professor);
            }

            if (aulasExibidas.Count == 0) lblResumo.Text = "Nenhuma aula neste dia";
            else if (aulasExibidas.Count == 1) lblResumo.Text = "1 aula";
            else lblResumo.Text = aulasExibidas.Count + " aulas";
        }

        private Aula AulaSelecionada()
        {
            if (grade.CurrentRow == null) return null;
            int i = grade.CurrentRow.Index;
            if (i < 0 || i >= aulasExibidas.Count) return null;
            return aulasExibidas[i];
        }

        private void NovaAula()
        {
            Aula nova = new Aula { Data = diaConsultado.Date, Hora = new TimeSpan(8, 0, 0) };
            using (AulaEditorForm f = new AulaEditorForm(nova, "Nova aula"))
            {
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    AulasRepositorio.Aulas.Add(nova);
                    diaConsultado = nova.Data.Date;
                    AtualizarTela();
                }
            }
        }

        private void EditarAula()
        {
            Aula aula = AulaSelecionada();
            if (aula == null)
            {
                MessageBox.Show("Selecione uma aula na lista para editar.", "Editar aula");
                return;
            }
            using (AulaEditorForm f = new AulaEditorForm(aula, "Editar aula"))
            {
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    AtualizarTela();
                }
            }
        }

        private void ExcluirAula()
        {
            Aula aula = AulaSelecionada();
            if (aula == null)
            {
                MessageBox.Show("Selecione uma aula na lista para excluir.", "Excluir aula");
                return;
            }
            DialogResult r = MessageBox.Show(
                "Excluir a aula \"" + aula.Atividade + "\" das " + aula.Hora.ToString(@"hh\:mm") + "?",
                "Excluir aula", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                AulasRepositorio.Aulas.Remove(aula);
                AtualizarTela();
            }
        }
    }

    // ---------- Janelinha para criar/editar uma aula ----------
    public class AulaEditorForm : Form
    {
        private Aula aula;
        private DateTimePicker dtpData;
        private DateTimePicker dtpHora;
        private TextBox txtAtividade;
        private TextBox txtProfessor;

        public AulaEditorForm(Aula aula, string titulo)
        {
            this.aula = aula;

            Text = titulo;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(340, 330);
            BackColor = Color.White;
            Font = new Font("Segoe UI", 10F);

            AdicionarRotulo("Data", 14);
            dtpData = new DateTimePicker();
            dtpData.Format = DateTimePickerFormat.Custom;
            dtpData.CustomFormat = "dd/MM/yyyy";
            dtpData.Location = new Point(16, 38);
            dtpData.Width = 308;
            dtpData.Value = aula.Data.Date;
            Controls.Add(dtpData);

            AdicionarRotulo("Horário", 76);
            dtpHora = new DateTimePicker();
            dtpHora.Format = DateTimePickerFormat.Custom;
            dtpHora.CustomFormat = "HH:mm";
            dtpHora.ShowUpDown = true;
            dtpHora.Location = new Point(16, 100);
            dtpHora.Width = 308;
            dtpHora.Value = DateTime.Today.Add(aula.Hora);
            Controls.Add(dtpHora);

            AdicionarRotulo("Atividade", 138);
            txtAtividade = new TextBox();
            txtAtividade.Location = new Point(16, 162);
            txtAtividade.Width = 308;
            txtAtividade.Text = aula.Atividade;
            Controls.Add(txtAtividade);

            AdicionarRotulo("Professor", 200);
            txtProfessor = new TextBox();
            txtProfessor.Location = new Point(16, 224);
            txtProfessor.Width = 308;
            txtProfessor.Text = aula.Professor;
            Controls.Add(txtProfessor);

            Button btnSalvar = new Button();
            btnSalvar.Text = "Salvar";
            btnSalvar.Location = new Point(16, 274);
            btnSalvar.Size = new Size(148, 36);
            btnSalvar.BackColor = Color.FromArgb(37, 56, 190);
            btnSalvar.ForeColor = Color.White;
            btnSalvar.FlatStyle = FlatStyle.Flat;
            btnSalvar.FlatAppearance.BorderSize = 0;
            btnSalvar.Click += Salvar_Click;
            Controls.Add(btnSalvar);

            Button btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Location = new Point(176, 274);
            btnCancelar.Size = new Size(148, 36);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.DialogResult = DialogResult.Cancel;
            Controls.Add(btnCancelar);

            AcceptButton = btnSalvar;
            CancelButton = btnCancelar;
        }

        private void AdicionarRotulo(string texto, int y)
        {
            Label l = new Label();
            l.Text = texto;
            l.AutoSize = true;
            l.Location = new Point(16, y);
            Controls.Add(l);
        }

        private void Salvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAtividade.Text) || string.IsNullOrWhiteSpace(txtProfessor.Text))
            {
                MessageBox.Show("Preencha a atividade e o professor.", "Campos obrigatórios");
                return;
            }

            aula.Data = dtpData.Value.Date;
            aula.Hora = new TimeSpan(dtpHora.Value.Hour, dtpHora.Value.Minute, 0);
            aula.Atividade = txtAtividade.Text.Trim();
            aula.Professor = txtProfessor.Text.Trim();

            DialogResult = DialogResult.OK;
        }
    }
}