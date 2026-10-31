using System.Drawing;
using System.Windows.Forms;

namespace Stingray
{
    public partial class PresencaForm
    {
        private void ConfigurarAparencia() {
            BackColor = VisualPadrao.FundoPagina;
            pnlPresenca.BackColor = VisualPadrao.FundoPagina;

            pnlFiltros.BackColor = VisualPadrao.BordaCartao;
            pnlFiltros.Padding = new Padding(1);
            pnlTabela.BackColor = VisualPadrao.BordaCartao;
            pnlTabela.Padding = new Padding(1);
            pnlTotal.BackColor = VisualPadrao.BordaCartao;
            pnlTotal.Padding = new Padding(1);

            tlpFiltros.BackColor = Color.White;
            tlpFiltros.Padding = new Padding(12, 4, 12, 4);
            tlpFiltros.ColumnCount = 4;
            tlpFiltros.ColumnStyles.Clear();
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31F));
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31F));
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 19F));
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 19F));
            tlpFiltros.RowCount = 2;
            tlpFiltros.RowStyles.Clear();
            tlpFiltros.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F));
            tlpFiltros.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

            tlpFiltros.SetColumn(lblDataAula, 0);
            tlpFiltros.SetRow(lblDataAula, 0);
            tlpFiltros.SetColumn(lblTurma, 1);
            tlpFiltros.SetRow(lblTurma, 0);
            lblDataAula.Dock = DockStyle.Fill;
            lblTurma.Dock = DockStyle.Fill;
            lblDataAula.Margin = new Padding(0);
            lblTurma.Margin = new Padding(6, 0, 0, 0);

            superficieDataAula = PrepararCampo(pnlDataAula, txtDataAula, btnCalendario);
            superficieTurma = PrepararCampo(pnlTurma, cmbTurma);

            txtDataAula.BackColor = Color.White;
            txtDataAula.BorderStyle = BorderStyle.None;
            txtDataAula.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            txtDataAula.ForeColor = VisualPadrao.TextoPrincipal;
            txtDataAula.ReadOnly = true;
            txtDataAula.Cursor = Cursors.Hand;
            txtDataAula.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtDataAula.Click += delegate { AlternarCalendario(); };

            btnCalendario.BackColor = Color.White;
            btnCalendario.FlatStyle = FlatStyle.Flat;
            btnCalendario.FlatAppearance.BorderSize = 0;
            btnCalendario.FlatAppearance.MouseOverBackColor = VisualPadrao.AzulClaro;
            btnCalendario.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            btnCalendario.ForeColor = VisualPadrao.TextoPrincipal;
            btnCalendario.Cursor = Cursors.Hand;
            btnCalendario.UseVisualStyleBackColor = false;
            btnCalendario.Text = string.Empty;
            btnCalendario.AccessibleName = "Abrir calendário da data da aula";
            btnCalendario.Paint += CalendarioButton_Paint;
            btnCalendario.MouseEnter += delegate { btnCalendario.Invalidate(); };
            btnCalendario.MouseLeave += delegate { btnCalendario.Invalidate(); };
            btnCalendario.Click += delegate { AlternarCalendario(); };

            cmbTurma.BackColor = Color.White;
            cmbTurma.ForeColor = VisualPadrao.TextoPrincipal;
            cmbTurma.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            cmbTurma.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTurma.FlatStyle = FlatStyle.Flat;
            cmbTurma.DrawMode = DrawMode.OwnerDrawFixed;
            cmbTurma.ItemHeight = 18;
            cmbTurma.AutoSize = false;
            cmbTurma.DrawItem += ComboTurma_DrawItem;
            cmbTurma.DropDownClosed += delegate { cmbTurma.Invalidate(); superficieTurma.Invalidate(); };
            if (cmbTurma.Items.Count > 0)
                cmbTurma.SelectedIndex = 0;

            CriarBotaoSetaTurma();

            tlpTotal.BackColor = Color.White;
            foreach (Label label in new Label[] {
                lblPresentes, lblNumPresentes, lblAusentes, lblNumAusentes,
                lblJustificados, lblNumJustificados, lblTotalAlunos, lblNumTotalAlunos})
                {
                label.ForeColor = VisualPadrao.TextoPrincipal;
                label.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            }

            ConfigurarTabelaPresenca(gdvPresenca);
            superficieDataAula.SizeChanged += delegate { PosicionarCamposFiltro(); };
            superficieTurma.SizeChanged += delegate { PosicionarCamposFiltro(); };
            PosicionarCamposFiltro();
        }

        private void ConfigurarBotoesChamada() {
            btnSalvarChamada.Name = "btnSalvarChamada";
            btnSalvarChamada.Text = "Salvar chamada";
            btnSalvarChamada.CornerRadius = 5;
            btnSalvarChamada.BackColor = VisualPadrao.Azul;
            btnSalvarChamada.ForeColor = Color.White;
            btnSalvarChamada.FlatAppearance.BorderSize = 0;
            btnSalvarChamada.FlatAppearance.BorderColor = VisualPadrao.Azul;
            btnSalvarChamada.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 73, 168);
            btnSalvarChamada.FlatAppearance.MouseDownBackColor = Color.FromArgb(18, 61, 140);
            btnSalvarChamada.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            btnSalvarChamada.Size = new Size(132, 30);
            btnSalvarChamada.Dock = DockStyle.Fill;
            btnSalvarChamada.Margin = new Padding(6, 0, 0, 0);
            btnSalvarChamada.TextAlign = ContentAlignment.MiddleCenter;
            btnSalvarChamada.Cursor = Cursors.Hand;
            btnSalvarChamada.Click += BtnSalvarChamada_Click;

            btnLimparChamada.Name = "btnLimparChamada";
            btnLimparChamada.Text = "Limpar chamada";
            btnLimparChamada.CornerRadius = 5;
            btnLimparChamada.BackColor = Color.White;
            btnLimparChamada.ForeColor = VisualPadrao.TextoPrincipal;
            btnLimparChamada.FlatAppearance.BorderSize = 0;
            btnLimparChamada.FlatAppearance.BorderColor = VisualPadrao.BordaCampo;
            btnLimparChamada.FlatAppearance.MouseOverBackColor = Color.FromArgb(254, 242, 242);
            btnLimparChamada.FlatAppearance.MouseDownBackColor = Color.FromArgb(254, 226, 226);
            btnLimparChamada.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            btnLimparChamada.Size = new Size(126, 30);
            btnLimparChamada.Dock = DockStyle.Fill;
            btnLimparChamada.Margin = new Padding(6, 0, 0, 0);
            btnLimparChamada.TextAlign = ContentAlignment.MiddleCenter;
            btnLimparChamada.Cursor = Cursors.Hand;
            btnLimparChamada.Click += BtnLimparChamada_Click;
            btnLimparChamada.MouseEnter += delegate {
                btnLimparChamada.ForeColor = Color.FromArgb(185, 28, 28);
                btnLimparChamada.FlatAppearance.BorderColor = Color.FromArgb(248, 113, 113);
            };
            btnLimparChamada.MouseLeave += delegate {
                btnLimparChamada.ForeColor = VisualPadrao.TextoPrincipal;
                btnLimparChamada.FlatAppearance.BorderColor = VisualPadrao.BordaCampo;
            };
        }
    }
}