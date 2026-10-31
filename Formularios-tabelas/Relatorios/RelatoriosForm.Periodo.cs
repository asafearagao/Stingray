using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stingray
{
    public partial class RelatoriosForm
    {
        private PainelComBorda popupPeriodoRelatorio;
        private DateTimePicker dataInicialRelatorio;
        private DateTimePicker dataFinalRelatorio;

        // Configura o popup utilizado para selecionar o período dos relatórios
        private void ConfigurarPopupPeriodoRelatorio(Control container)
        {
            popupPeriodoRelatorio = CriarPainelPopupPeriodo();
            popupPeriodoRelatorio.Controls.Add(CriarTituloPopupPeriodo());
            popupPeriodoRelatorio.Controls.Add(CriarSubtituloPopupPeriodo());
            popupPeriodoRelatorio.Controls.Add(CriarDivisorPopupPeriodo());
            popupPeriodoRelatorio.Controls.Add(CriarRotuloData("Data inicial", new Point(14, 61)));
            popupPeriodoRelatorio.Controls.Add(CriarRotuloData("Data final", new Point(158, 61)));

            dataInicialRelatorio = CriarSeletorData(
                "dataInicialRelatorio",
                new Point(14, 78),
                new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1));
            dataFinalRelatorio = CriarSeletorData(
                "dataFinalRelatorio",
                new Point(158, 78),
                DateTime.Today);

            popupPeriodoRelatorio.Controls.Add(dataInicialRelatorio);
            popupPeriodoRelatorio.Controls.Add(dataFinalRelatorio);
            popupPeriodoRelatorio.Controls.Add(CriarBotaoCancelarPeriodo());
            popupPeriodoRelatorio.Controls.Add(CriarBotaoAplicarPeriodo());

            container.Controls.Add(popupPeriodoRelatorio);
            popupPeriodoRelatorio.BringToFront();
        }

        // Cria o painel que reúne os controles de seleção do período
        private PainelComBorda CriarPainelPopupPeriodo()
        {
            return new PainelComBorda
            {
                Name = "popupPeriodoRelatorio",
                Size = new Size(304, 162),
                BackColor = Color.White,
                CorBorda = VisualPadrao.BordaCartao,
                RaioCanto = 9,
                Padding = new Padding(1),
                Visible = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
        }

        private Label CriarTituloPopupPeriodo()
        {
            return CriarRotuloPopup(
                "Selecionar período",
                new Font("Segoe UI Semibold", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
                VisualPadrao.TextoPrincipal,
                new Point(14, 9),
                new Size(260, 21));
        }

        private Label CriarSubtituloPopupPeriodo()
        {
            return CriarRotuloPopup(
                "Escolha as datas para consultar os relatórios.",
                new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point),
                VisualPadrao.TextoSecundario,
                new Point(14, 30),
                new Size(274, 17));
        }

        private Label CriarRotuloData(string texto, Point localizacao)
        {
            return CriarRotuloPopup(
                texto,
                new Font("Segoe UI Semibold", 7.5F, FontStyle.Regular, GraphicsUnit.Point),
                VisualPadrao.TextoSecundario,
                localizacao,
                new Size(128, 15));
        }

        private Label CriarRotuloPopup(string texto, Font fonte, Color cor, Point localizacao, Size tamanho)
        {
            return new Label
            {
                Text = texto,
                Font = fonte,
                ForeColor = cor,
                Location = localizacao,
                Size = tamanho
            };
        }

        private Panel CriarDivisorPopupPeriodo()
        {
            return new Panel
            {
                BackColor = Color.FromArgb(241, 245, 249),
                Location = new Point(13, 52),
                Size = new Size(276, 1)
            };
        }

        private DateTimePicker CriarSeletorData(string nome, Point localizacao, DateTime valor)
        {
            return new DateTimePicker
            {
                Name = nome,
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point),
                Location = localizacao,
                Size = new Size(128, 25),
                CalendarForeColor = VisualPadrao.TextoPrincipal,
                CalendarTitleBackColor = VisualPadrao.Azul,
                CalendarTitleForeColor = Color.White,
                Value = valor
            };
        }

        private Button CriarBotaoCancelarPeriodo()
        {
            Button botao = new Button
            {
                Name = "btnCancelarPeriodoRelatorio",
                Text = "Cancelar",
                Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = VisualPadrao.TextoPrincipal,
                UseVisualStyleBackColor = false,
                Size = new Size(82, 28),
                Location = new Point(112, 119),
                Cursor = Cursors.Hand
            };
            botao.FlatAppearance.BorderColor = VisualPadrao.BordaCartao;
            botao.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 250, 252);
            botao.Click += CancelarPeriodoRelatorio_Click;
            return botao;
        }

        private RoundedButton CriarBotaoAplicarPeriodo()
        {
            RoundedButton botao = new RoundedButton
            {
                Name = "btnAplicarPeriodoRelatorio",
                Text = "Aplicar período",
                CornerRadius = 5,
                Font = new Font("Segoe UI Semibold", 8F, FontStyle.Regular, GraphicsUnit.Point),
                BackColor = VisualPadrao.Azul,
                ForeColor = Color.White,
                Size = new Size(116, 28),
                Location = new Point(200, 119),
                Cursor = Cursors.Hand
            };
            botao.Click += AplicarPeriodoRelatorio_Click;
            return botao;
        }

        // Abertura do popup ao clicar no campo de período
        private void PeriodoRelatorio_Click(object sender, EventArgs e)
        {
            AlternarPopupPeriodoRelatorio(panelPeriodoRelatorio);
        }

        private void CancelarPeriodoRelatorio_Click(object sender, EventArgs e)
        {
            popupPeriodoRelatorio.Visible = false;
        }

        private void AplicarPeriodoRelatorio_Click(object sender, EventArgs e)
        {
            if (dataFinalRelatorio.Value.Date < dataInicialRelatorio.Value.Date)
            {
                MessageBox.Show(this,
                    "A data final não pode ser anterior à data inicial.",
                    "Período inválido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                popupPeriodoRelatorio.Visible = true;
                popupPeriodoRelatorio.BringToFront();
                return;
            }

            txtPeriodoRelatorio.Text = dataInicialRelatorio.Value.ToString("dd/MM/yyyy") +
                " - " + dataFinalRelatorio.Value.ToString("dd/MM/yyyy");
            txtPeriodoRelatorio.ForeColor = VisualPadrao.TextoPrincipal;
            popupPeriodoRelatorio.Visible = false;
        }

        private void AlternarPopupPeriodoRelatorio(Control campoPeriodo)
        {
            if (popupPeriodoRelatorio == null || campoPeriodo == null)
                return;

            if (popupPeriodoRelatorio.Visible)
            {
                popupPeriodoRelatorio.Visible = false;
                return;
            }

            Rectangle campoNaTela = campoPeriodo.RectangleToScreen(campoPeriodo.ClientRectangle);
            Point abaixo = popupPeriodoRelatorio.Parent.PointToClient(
                new Point(campoNaTela.Left, campoNaTela.Bottom + 5));
            int margem = 8;
            int x = Math.Max(margem, Math.Min(abaixo.X,
                popupPeriodoRelatorio.Parent.ClientSize.Width - popupPeriodoRelatorio.Width - margem));
            int y = abaixo.Y;

            if (y + popupPeriodoRelatorio.Height > popupPeriodoRelatorio.Parent.ClientSize.Height - margem)
            {
                Point acima = popupPeriodoRelatorio.Parent.PointToClient(
                    new Point(campoNaTela.Left, campoNaTela.Top - popupPeriodoRelatorio.Height - 5));
                y = acima.Y;
            }

            y = Math.Max(margem, Math.Min(y,
                popupPeriodoRelatorio.Parent.ClientSize.Height - popupPeriodoRelatorio.Height - margem));

            popupPeriodoRelatorio.Location = new Point(x, y);
            popupPeriodoRelatorio.Visible = true;
            popupPeriodoRelatorio.BringToFront();
        }
    }
}