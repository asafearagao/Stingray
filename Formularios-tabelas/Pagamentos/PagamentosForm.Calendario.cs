using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stingray {
    public partial class PagamentosForm {
        // Configura o popup utilizado para selecionar o período dos pagamentos
        private void ConfigurarPopupPeriodo() {
            popupPeriodo = new Panel();
            popupPeriodo.Name = "popupPeriodoRuntime";
            popupPeriodo.Size = new Size(250, 194);
            popupPeriodo.BackColor = Color.FromArgb(226, 232, 240);
            popupPeriodo.BorderStyle = BorderStyle.None;
            popupPeriodo.Visible = false;
            popupPeriodo.Padding = new Padding(1);

            Panel popupContent = new Panel();
            popupContent.Name = "popupPeriodoContent";
            popupContent.Dock = DockStyle.Fill;
            popupContent.BackColor = Color.White;
            popupPeriodo.Controls.Add(popupContent);

            Label titulo = new Label();
            titulo.Text = "Selecionar período";
            titulo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            titulo.ForeColor = Color.FromArgb(17, 24, 39);
            titulo.BackColor = Color.Transparent;
            titulo.Location = new Point(10, 8);
            titulo.Size = new Size(190, 20);
            popupContent.Controls.Add(titulo);

            Label labelInicial = new Label();
            labelInicial.Text = "Data inicial";
            labelInicial.Font = new Font("Segoe UI", 7.5F);
            labelInicial.ForeColor = Color.FromArgb(100, 116, 139);
            labelInicial.Location = new Point(10, 38);
            labelInicial.Size = new Size(220, 15);
            popupContent.Controls.Add(labelInicial);

            campoDataInicial = CriarCampoData(new Point(10, 55), true);
            popupContent.Controls.Add(campoDataInicial);

            Label labelFinal = new Label();
            labelFinal.Text = "Data final";
            labelFinal.Font = new Font("Segoe UI", 7.5F);
            labelFinal.ForeColor = Color.FromArgb(100, 116, 139);
            labelFinal.Location = new Point(10, 87);
            labelFinal.Size = new Size(220, 15);
            popupContent.Controls.Add(labelFinal);

            campoDataFinal = CriarCampoData(new Point(10, 104), false);
            popupContent.Controls.Add(campoDataFinal);

            Button aplicar = new Button();
            aplicar.Text = "Aplicar período";
            aplicar.Location = new Point(10, 143);
            aplicar.Size = new Size(220, 31);
            aplicar.FlatStyle = FlatStyle.Flat;
            aplicar.FlatAppearance.BorderSize = 0;
            aplicar.BackColor = VisualPadrao.Azul;
            aplicar.ForeColor = Color.White;
            aplicar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            aplicar.Cursor = Cursors.Hand;
            aplicar.Click += delegate { AplicarPeriodo(); };
            popupContent.Controls.Add(aplicar);

            paymentPagePanel.Controls.Add(popupPeriodo);
            popupPeriodo.BringToFront();

            CriarCalendarioFlutuante();
            paymentPagePanel.Controls.Add(calendarioFlutuante);
            calendarioFlutuante.BringToFront();

            filtroCliqueExterno = new FiltroFecharPopup(this, popupPeriodo, btnCalendario, calendarioFlutuante);
            Application.AddMessageFilter(filtroCliqueExterno);
        }

        private void CriarCalendarioFlutuante() {
            calendarioFlutuante = new Panel();
            calendarioFlutuante.Name = "calendarioDataPopup";
            calendarioFlutuante.Size = new Size(224, 196);
            calendarioFlutuante.BackColor = Color.White;
            calendarioFlutuante.BorderStyle = BorderStyle.None;
            calendarioFlutuante.Padding = new Padding(0);
            calendarioFlutuante.Visible = false;
            calendarioFlutuante.Paint += delegate(object sender, PaintEventArgs e) {
                using (Pen pen = new Pen(Color.FromArgb(203, 213, 225), 1F))
                    e.Graphics.DrawRectangle(pen, 0, 0,
                        Math.Max(0, calendarioFlutuante.ClientSize.Width - 1),
                        Math.Max(0, calendarioFlutuante.ClientSize.Height - 1));
            };

            Button anterior = CriarBotaoNavegacaoCalendario("‹", -1);
            anterior.Location = new Point(4, 3);
            calendarioFlutuante.Controls.Add(anterior);

            Button proximo = CriarBotaoNavegacaoCalendario("›", 1);
            proximo.Location = new Point(196, 3);
            calendarioFlutuante.Controls.Add(proximo);

            lblMesCalendario = new Label();
            lblMesCalendario.Name = "lblMesCalendario";
            lblMesCalendario.Location = new Point(28, 4);
            lblMesCalendario.Size = new Size(168, 21);
            lblMesCalendario.TextAlign = ContentAlignment.MiddleCenter;
            lblMesCalendario.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular);
            lblMesCalendario.ForeColor = Color.FromArgb(55, 65, 81);
            lblMesCalendario.BackColor = Color.White;
            calendarioFlutuante.Controls.Add(lblMesCalendario);

            string[] diasSemana = { "dom", "seg", "ter", "qua", "qui", "sex", "sáb" };
            for (int coluna = 0; coluna < 7; coluna++) {
                Label diaSemana = new Label();
                diaSemana.Name = "lblDiaSemana" + coluna;
                diaSemana.Text = diasSemana[coluna];
                diaSemana.Location = new Point(5 + coluna * 30, 27);
                diaSemana.Size = new Size(29, 18);
                diaSemana.TextAlign = ContentAlignment.MiddleCenter;
                diaSemana.Font = new Font("Segoe UI", 7.2F, FontStyle.Regular);
                diaSemana.ForeColor = Color.FromArgb(71, 85, 105);
                diaSemana.BackColor = Color.White;
                calendarioFlutuante.Controls.Add(diaSemana);
            }

            botoesDiasCalendario.Clear();
            for (int indice = 0; indice < 42; indice++) {
                Button dia = new CalendarDayButton();
                dia.Name = "btnDiaCalendario" + indice;
                dia.Size = new Size(27, 20);
                dia.Location = new Point(6 + (indice % 7) * 30, 45 + (indice / 7) * 20);
                dia.Margin = new Padding(0);
                dia.Padding = new Padding(0);
                dia.TabStop = false;
                dia.FlatStyle = FlatStyle.Flat;
                dia.FlatAppearance.BorderSize = 0;
                dia.FlatAppearance.MouseOverBackColor = VisualPadrao.Azul;
                dia.FlatAppearance.MouseDownBackColor = Color.FromArgb(29, 78, 216);
                dia.UseVisualStyleBackColor = false;
                dia.BackColor = Color.White;
                dia.Font = fonteDiaCalendario;
                dia.ForeColor = VisualPadrao.TextoPrincipal;
                dia.Cursor = Cursors.Hand;
                dia.Click += delegate {
                    if (dia.Tag is DateTime)
                        SelecionarDataCalendario((DateTime)dia.Tag);
                };
                dia.MouseEnter += delegate { EstilizarDiaCalendario(dia, true); };
                dia.MouseMove += delegate { EstilizarDiaCalendario(dia, true); };
                dia.MouseLeave += delegate { EstilizarDiaCalendario(dia, false); };
                botoesDiasCalendario.Add(dia);
                calendarioFlutuante.Controls.Add(dia);
            }

            lblHojeCalendario = new Label();
            lblHojeCalendario.Name = "lblHojeCalendario";
            // Reserva uma faixa para o rodapé do calendário
            lblHojeCalendario.Location = new Point(5, 171);
            lblHojeCalendario.Size = new Size(214, 20);
            lblHojeCalendario.Text = "Hoje: " + DateTime.Today.ToString("dd/MM/yyyy");
            lblHojeCalendario.TextAlign = ContentAlignment.MiddleCenter;
            lblHojeCalendario.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular);
            lblHojeCalendario.ForeColor = Color.FromArgb(71, 85, 105);
            lblHojeCalendario.BackColor = Color.White;
            lblHojeCalendario.Cursor = Cursors.Hand;
            lblHojeCalendario.TabStop = false;
            lblHojeCalendario.MouseEnter += delegate {
                lblHojeCalendario.ForeColor = VisualPadrao.Azul;
                lblHojeCalendario.BackColor = VisualPadrao.AzulClaro;
            };
            lblHojeCalendario.MouseLeave += delegate {
                lblHojeCalendario.ForeColor = Color.FromArgb(71, 85, 105);
                lblHojeCalendario.BackColor = Color.White;
            };
            lblHojeCalendario.Click += delegate {
                if (campoDataAtivo != null)
                    SelecionarDataCalendario(DateTime.Today);
            };
            calendarioFlutuante.Controls.Add(lblHojeCalendario);

            mesCalendarioVisualizado = DateTime.Today;
            AtualizarDiasCalendario();
        }

        private Button CriarBotaoNavegacaoCalendario(string simbolo, int meses) {
            Button botao = new Button();
            botao.Text = simbolo;
            botao.Size = new Size(22, 22);
            botao.FlatStyle = FlatStyle.Flat;
            botao.FlatAppearance.BorderSize = 0;
            botao.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            botao.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            botao.UseVisualStyleBackColor = false;
            botao.BackColor = Color.White;
            botao.ForeColor = Color.FromArgb(71, 85, 105);
            botao.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            botao.Padding = new Padding(0);
            botao.Margin = new Padding(0);
            botao.TabStop = false;
            botao.Cursor = Cursors.Hand;
            botao.Click += delegate {
                mesCalendarioVisualizado = mesCalendarioVisualizado.AddMonths(meses);
                AtualizarDiasCalendario();
            };
            return botao;
        }

        private void AtualizarDiasCalendario() {
            if (lblMesCalendario != null)
                lblMesCalendario.Text = mesCalendarioVisualizado.ToString("MMMM 'de' yyyy", new CultureInfo("pt-BR"));
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
            if (calendarioFlutuante != null)
                calendarioFlutuante.Invalidate(true);
        }

        private void EstilizarDiaCalendario(Button botao, bool hover) {
            if (botao == null || !(botao.Tag is DateTime)) return;
            DateTime data = ((DateTime)botao.Tag).Date;
            bool hoje = data == DateTime.Today;
            DateTime? selecionada = campoDataAtivo == campoDataInicial
                ? dataInicialSelecionada : (campoDataAtivo == campoDataFinal ? dataFinalSelecionada : null);
            bool escolhida = selecionada.HasValue && selecionada.Value.Date == data;

            botao.FlatAppearance.BorderSize = 0;
            botao.Font = hoje ? fonteDiaCalendarioHoje : fonteDiaCalendario;
            if (hover) {
                botao.BackColor = VisualPadrao.Azul;
                botao.ForeColor = Color.White;
                botao.Invalidate();
                return;
            }

            if (escolhida) {
                botao.BackColor = VisualPadrao.AzulClaro;
                botao.ForeColor = VisualPadrao.Azul;
                botao.FlatAppearance.BorderSize = 1;
                botao.FlatAppearance.BorderColor = Color.FromArgb(147, 197, 253);

            } else {
                botao.BackColor = Color.White;
                // O dia atual e o dia sob o mouse têm estilos distintos
                botao.ForeColor = data.Month != mesCalendarioVisualizado.Month || data.Year != mesCalendarioVisualizado.Year
                    ? Color.FromArgb(148, 163, 184)
                    : VisualPadrao.TextoPrincipal;
            }
            botao.Invalidate();
        }

        private void SelecionarDataCalendario(DateTime data) {
            data = data.Date;
            if (campoDataAtivo == campoDataInicial) {
                dataInicialSelecionada = data;
                if (txtDataInicial != null) {
                    txtDataInicial.Text = data.ToString("dd/MM/yyyy");
                    txtDataInicial.ForeColor = VisualPadrao.TextoPrincipal;
                }

            } else if (campoDataAtivo == campoDataFinal) {
                dataFinalSelecionada = data;

                if (txtDataFinal != null) {
                    txtDataFinal.Text = data.ToString("dd/MM/yyyy");
                    txtDataFinal.ForeColor = VisualPadrao.TextoPrincipal;
                }
            }

            calendarioFlutuante.Visible = false;
            campoDataAtivo = null;
        }

        private Panel CriarCampoData(Point location, bool dataInicial) {
            Panel campo = new Panel();
            campo.Name = dataInicial
                ? "campoDataInicial"
                : "campoDataFinal";
            campo.Location = location;
            campo.Size = new Size(220, 28);
            campo.BackColor = Color.White;
            campo.BorderStyle = BorderStyle.None;
            campo.Paint += CampoFiltro_Paint;

            TextBox texto = new TextBox();
            texto.Name = dataInicial
                ? "txtDataInicial"
                : "txtDataFinal";
            texto.ReadOnly = true;
            texto.BorderStyle = BorderStyle.None;
            texto.BackColor = Color.White;
            texto.ForeColor = VisualPadrao.TextoPrincipal;
            texto.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular);
            texto.TextAlign = HorizontalAlignment.Left;
            texto.Location = new Point(8, 6);
            texto.Size = new Size(166, 16);
            texto.TabStop = false;
            DateTime? dataSelecionada = dataInicial
                ? dataInicialSelecionada
                : dataFinalSelecionada;
            texto.Text = dataSelecionada.HasValue
                ? dataSelecionada.Value.ToString("dd/MM/yyyy")
                : "Selecione uma data";
            texto.ForeColor = dataSelecionada.HasValue
                ? VisualPadrao.TextoPrincipal
                : VisualPadrao.TextoPlaceholder;

            Button botaoCalendario = new Button();
            botaoCalendario.Name = dataInicial
                ? "btnCalendarioDataInicial"
                : "btnCalendarioDataFinal";
            botaoCalendario.Location = new Point(188, 1);
            botaoCalendario.Size = new Size(28, 26);
            ConfigurarBotaoIconeCalendario(botaoCalendario, TipoIconeCalendario.DataInicial);
            botaoCalendario.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            botaoCalendario.AccessibleName = dataInicial
                ? "Abrir calendário da data inicial"
                : "Abrir calendário da data final";
            botaoCalendario.Click += delegate { AlternarCalendarioData(campo, dataInicial); };

            campo.Controls.Add(texto);
            campo.Controls.Add(botaoCalendario);

            if (dataInicial) {
                txtDataInicial = texto;
                btnCalendarioInicial = botaoCalendario;

            } else {
                txtDataFinal = texto;
                btnCalendarioFinal = botaoCalendario;
            }

            return campo;
        }

        private void AlternarCalendarioData(Panel campo, bool dataInicial) {
            if (calendarioFlutuante == null || campo == null)
                return;
            if (calendarioFlutuante.Visible && campoDataAtivo == campo) {
                calendarioFlutuante.Visible = false;
                campoDataAtivo = null;
                return;
            }

            campoDataAtivo = campo;
            DateTime data = (dataInicial ? dataInicialSelecionada : dataFinalSelecionada) ?? DateTime.Today;
            mesCalendarioVisualizado = new DateTime(data.Year, data.Month, 1);
            AtualizarDiasCalendario();

            Point abaixoNaTela = campo.PointToScreen(new Point(0, campo.Height + 3));
            Point posicao = paymentPagePanel.PointToClient(abaixoNaTela);
            int margem = 4;
            if (posicao.X + calendarioFlutuante.Width > paymentPagePanel.ClientSize.Width - margem)
                posicao.X = paymentPagePanel.ClientSize.Width - calendarioFlutuante.Width - margem;
            if (posicao.Y + calendarioFlutuante.Height > paymentPagePanel.ClientSize.Height - margem) {
                Point acimaNaTela = campo.PointToScreen(new Point(0, -calendarioFlutuante.Height - 3));
                posicao = paymentPagePanel.PointToClient(acimaNaTela);
            }
            posicao.X = Math.Max(margem, posicao.X);
            posicao.Y = Math.Max(margem, posicao.Y);
            calendarioFlutuante.Location = posicao;
            calendarioFlutuante.Visible = true;
            calendarioFlutuante.BringToFront();
        }

        private void AlternarPopupPeriodo() {
            if (popupPeriodo == null)
                return;
            if (popupPeriodo.Visible) {
                FecharPopupPeriodo();
                return;
            }

            Rectangle bounds = periodoField.RectangleToScreen(periodoField.ClientRectangle);
            Point posicao = paymentPagePanel.PointToClient(new Point(bounds.Left, bounds.Bottom + 4));
            int x = Math.Max(8, Math.Min(posicao.X, paymentPagePanel.ClientSize.Width - popupPeriodo.Width - 8));
            int y = Math.Max(8, Math.Min(posicao.Y, paymentPagePanel.ClientSize.Height - popupPeriodo.Height - 8));
            popupPeriodo.Location = new Point(x, y);

            // Restaura o último intervalo aplicado, se houver
            if (dataInicialAplicada.HasValue && dataFinalAplicada.HasValue) {
                dataInicialSelecionada = dataInicialAplicada.Value;
                dataFinalSelecionada = dataFinalAplicada.Value;
            }
            if (txtDataInicial != null) {
                txtDataInicial.Text = dataInicialSelecionada.HasValue ? dataInicialSelecionada.Value.ToString("dd/MM/yyyy") : "Selecione uma data";
                txtDataInicial.ForeColor = dataInicialSelecionada.HasValue ? VisualPadrao.TextoPrincipal : VisualPadrao.TextoPlaceholder;
            }
            if (txtDataFinal != null) {
                txtDataFinal.Text = dataFinalSelecionada.HasValue ? dataFinalSelecionada.Value.ToString("dd/MM/yyyy") : "Selecione uma data";
                txtDataFinal.ForeColor = dataFinalSelecionada.HasValue ? VisualPadrao.TextoPrincipal : VisualPadrao.TextoPlaceholder;
            }

            popupPeriodo.Visible = true;
            popupPeriodo.BringToFront();
        }

        private void AplicarPeriodo() {
            if (!dataInicialSelecionada.HasValue || !dataFinalSelecionada.HasValue) {
                MessageBox.Show(this,
                    "Selecione a data inicial e a data final antes de aplicar o período.",
                    "Período incompleto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                popupPeriodo.Visible = true;
                popupPeriodo.BringToFront();
                return;
            }

            DateTime inicial = dataInicialSelecionada.Value.Date;
            DateTime final = dataFinalSelecionada.Value.Date;
            if (final < inicial) {
                MessageBox.Show(this,
                    "A data final não pode ser anterior à data inicial. Corrija as datas e tente novamente.",
                    "Erro ao aplicar período", MessageBoxButtons.OK, MessageBoxIcon.Error);
                popupPeriodo.Visible = true;
                popupPeriodo.BringToFront();
                return;
            }

            dataInicialAplicada = inicial;
            dataFinalAplicada = final;
            txtPeriodo.Text = inicial.ToString("dd/MM/yyyy") + " - " + final.ToString("dd/MM/yyyy");
            txtPeriodo.ForeColor = VisualPadrao.TextoPrincipal;
            FecharPopupPeriodo();
        }

        public void FecharSeletores() {
            FecharPopupPeriodo();
        }

        private void FecharPopupPeriodo() {
            if (calendarioFlutuante != null)
                calendarioFlutuante.Visible = false;
            campoDataAtivo = null;
            if (popupPeriodo != null)
                popupPeriodo.Visible = false;
        }

    }
}