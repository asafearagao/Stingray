using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Stingray
{
    public partial class PresencaForm
    {
        // Configura os controles e os eventos do calendário de presença
        private void ConfigurarCalendario() {
            calendarioFlutuante = new Panel();
            calendarioFlutuante.Name = "calendarioDataAula";
            calendarioFlutuante.Size = new Size(250, 240);
            calendarioFlutuante.BackColor = VisualPadrao.BordaCartao;
            calendarioFlutuante.Padding = new Padding(1);
            calendarioFlutuante.Visible = false;
            calendarioFlutuante.TabStop = false;

            Panel conteudo = new Panel();
            conteudo.Dock = DockStyle.Fill;
            conteudo.BackColor = Color.White;
            calendarioFlutuante.Controls.Add(conteudo);

            Button anterior = CriarBotaoMes("‹");
            anterior.Location = new Point(6, 6);
            anterior.Size = new Size(28, 25);
            anterior.Click += delegate {
                mesCalendarioVisualizado = mesCalendarioVisualizado.AddMonths(-1);
                AtualizarCalendario();
            };
            conteudo.Controls.Add(anterior);

            Button proximo = CriarBotaoMes("›");
            proximo.Location = new Point(215, 6);
            proximo.Size = new Size(28, 25);
            proximo.Click += delegate {
                mesCalendarioVisualizado = mesCalendarioVisualizado.AddMonths(1);
                AtualizarCalendario();
            };
            conteudo.Controls.Add(proximo);

            lblMesCalendario = new Label();
            lblMesCalendario.Name = "lblMesCalendario";
            lblMesCalendario.Location = new Point(34, 7);
            lblMesCalendario.Size = new Size(181, 23);
            lblMesCalendario.TextAlign = ContentAlignment.MiddleCenter;
            lblMesCalendario.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblMesCalendario.ForeColor = VisualPadrao.TextoPrincipal;
            lblMesCalendario.BackColor = Color.White;
            conteudo.Controls.Add(lblMesCalendario);

            string[] diasSemana = { "Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb" };
            for (int coluna = 0; coluna < diasSemana.Length; coluna++) {
                Label diaSemana = new Label();
                diaSemana.Text = diasSemana[coluna];
                diaSemana.TextAlign = ContentAlignment.MiddleCenter;
                diaSemana.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
                diaSemana.ForeColor = VisualPadrao.TextoSecundario;
                diaSemana.BackColor = Color.White;
                diaSemana.SetBounds(8 + coluna * 33, 34, 31, 17);
                conteudo.Controls.Add(diaSemana);
            }

            for (int indice = 0; indice < 42; indice++) {
                Button dia = new Button();
                dia.Name = "btnDiaAula" + indice;
                dia.Tag = DateTime.MinValue;
                dia.SetBounds(8 + (indice % 7) * 33, 52 + (indice / 7) * 23, 31, 22);
                dia.FlatStyle = FlatStyle.Flat;
                dia.FlatAppearance.BorderSize = 0;
                dia.FlatAppearance.MouseOverBackColor = VisualPadrao.AzulClaro;
                dia.BackColor = Color.White;
                dia.ForeColor = VisualPadrao.TextoPrincipal;
                dia.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point);
                dia.Margin = new Padding(0);
                dia.Padding = new Padding(0);
                dia.TabStop = false;
                dia.Cursor = Cursors.Hand;
                dia.Click += DiaCalendario_Click;
                dia.MouseEnter += delegate(object sender, EventArgs e) {
                    Button b = sender as Button;
                    if (b != null)
                        EstilizarDiaCalendario(b, true);
                };
                dia.MouseLeave += delegate(object sender, EventArgs e) {
                    Button b = sender as Button;
                    if (b != null)
                        EstilizarDiaCalendario(b, false);
                };
                botoesDiasCalendario.Add(dia);
                conteudo.Controls.Add(dia);
            }

            lblHojeCalendario = new Label();
            lblHojeCalendario.Name = "lblHojeCalendario";
            lblHojeCalendario.Text = "Hoje: " + DateTime.Today.ToString("dd/MM/yyyy");
            lblHojeCalendario.TextAlign = ContentAlignment.MiddleCenter;
            lblHojeCalendario.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblHojeCalendario.ForeColor = Color.FromArgb(71, 85, 105);
            lblHojeCalendario.BackColor = Color.White;
            lblHojeCalendario.Cursor = Cursors.Hand;
            lblHojeCalendario.SetBounds(8, 195, 232, 22);
            lblHojeCalendario.MouseEnter += delegate { lblHojeCalendario.ForeColor = VisualPadrao.Azul; lblHojeCalendario.BackColor = VisualPadrao.AzulClaro; };
            lblHojeCalendario.MouseLeave += delegate { lblHojeCalendario.ForeColor = Color.FromArgb(71, 85, 105); lblHojeCalendario.BackColor = Color.White; };
            lblHojeCalendario.Click += delegate { SelecionarData(DateTime.Today); };
            conteudo.Controls.Add(lblHojeCalendario);

            pnlPresenca.Controls.Add(calendarioFlutuante);
            calendarioFlutuante.BringToFront();
            AtualizarCalendario();
        }

        // Cria o botão utilizado para navegar entre os meses do calendário
        private Button CriarBotaoMes(string textoBotao) {
            Button botao = new Button();
            botao.Text = textoBotao;
            botao.FlatStyle = FlatStyle.Flat;
            botao.FlatAppearance.BorderSize = 0;
            botao.FlatAppearance.MouseOverBackColor = VisualPadrao.AzulClaro;
            botao.BackColor = Color.White;
            botao.ForeColor = VisualPadrao.TextoSecundario;
            botao.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
            botao.Cursor = Cursors.Hand;
            botao.TabStop = false;
            return botao;
        }

        // Alterna a exibição do calendário
        private void AlternarCalendario() {
            if (calendarioFlutuante == null)
                return;

            if (calendarioFlutuante.Visible) {
                FecharCalendario();
                return;
            }

            mesCalendarioVisualizado = new DateTime(dataSelecionada.Year, dataSelecionada.Month, 1);
            AtualizarCalendario();

            Point abaixoNaTela = superficieDataAula.PointToScreen(new Point(0, superficieDataAula.Height + 3));
            Point posicao = pnlPresenca.PointToClient(abaixoNaTela);
            int margem = 4;
            if (posicao.X + calendarioFlutuante.Width > pnlPresenca.ClientSize.Width - margem)
                posicao.X = pnlPresenca.ClientSize.Width - calendarioFlutuante.Width - margem;
            if (posicao.Y + calendarioFlutuante.Height > pnlPresenca.ClientSize.Height - margem) {
                Point acimaNaTela = superficieDataAula.PointToScreen(new Point(0, -calendarioFlutuante.Height - 3));
                posicao = pnlPresenca.PointToClient(acimaNaTela);
            }
            posicao.X = Math.Max(margem, posicao.X);
            posicao.Y = Math.Max(margem, posicao.Y);
            calendarioFlutuante.Location = posicao;
            calendarioFlutuante.Visible = true;
            calendarioFlutuante.BringToFront();

            if (filtroCliqueCalendario == null)
                filtroCliqueCalendario = new FiltroCliqueCalendario(this);
            Application.AddMessageFilter(filtroCliqueCalendario);
        }

        private void FecharCalendario() {
            if (calendarioFlutuante != null)
                calendarioFlutuante.Visible = false;
            if (filtroCliqueCalendario != null)
                Application.RemoveMessageFilter(filtroCliqueCalendario);
        }

        public void FecharSeletores() {
            FecharCalendario();
        }

        private void AtualizarCalendario() {
            if (lblMesCalendario != null) {
                string mes = mesCalendarioVisualizado.ToString("MMMM 'de' yyyy", new CultureInfo("pt-BR"));
                lblMesCalendario.Text = char.ToUpper(mes[0], new CultureInfo("pt-BR")) + mes.Substring(1);
            }
            if (lblHojeCalendario != null)
                lblHojeCalendario.Text = "Hoje: " + DateTime.Today.ToString("dd/MM/yyyy");

            DateTime primeiroDia = new DateTime(mesCalendarioVisualizado.Year, mesCalendarioVisualizado.Month, 1);
            DateTime inicioGrade = primeiroDia.AddDays(-(int)primeiroDia.DayOfWeek);
            for (int i = 0; i < botoesDiasCalendario.Count; i++) {
                Button dia = botoesDiasCalendario[i];
                dia.Tag = inicioGrade.AddDays(i).Date;
                dia.Text = ((DateTime)dia.Tag).Day.ToString(CultureInfo.InvariantCulture);
                EstilizarDiaCalendario(dia, false);
            }
        }

        private void EstilizarDiaCalendario(Button botao, bool hover) {
            if (botao == null || !(botao.Tag is DateTime))
                return;

            DateTime data = ((DateTime)botao.Tag).Date;
            bool mesAtual = data.Month == mesCalendarioVisualizado.Month && data.Year == mesCalendarioVisualizado.Year;
            bool selecionada = data.Date == dataSelecionada.Date;
            bool hoje = data.Date == DateTime.Today;

            botao.BackColor = selecionada ? VisualPadrao.Azul : (hover ? VisualPadrao.AzulClaro : Color.White);
            botao.ForeColor = selecionada
                ? Color.White
                : (mesAtual ? VisualPadrao.TextoPrincipal : Color.FromArgb(180, 188, 199));
            botao.Font = (selecionada || hoje)
                ? new Font("Segoe UI Semibold", 7.8F, FontStyle.Regular, GraphicsUnit.Point)
                : new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point);
            botao.FlatAppearance.MouseOverBackColor = VisualPadrao.AzulClaro;
            botao.FlatAppearance.BorderColor = selecionada ? VisualPadrao.Azul : Color.White;
        }

        // Trata o clique em um dia do calendário
        private void DiaCalendario_Click(object sender, EventArgs e) {
            Button botao = sender as Button;
            if (botao != null && botao.Tag is DateTime)
                SelecionarData((DateTime)botao.Tag);
        }

        // Atualiza a data selecionada
        private void SelecionarData(DateTime data) {
            dataSelecionada = data.Date;
            txtDataAula.Text = dataSelecionada.ToString("dd/MM/yyyy");
            txtDataAula.ForeColor = VisualPadrao.TextoPrincipal;
            AtualizarCalendario();
            FecharCalendario();
        }

    }
}