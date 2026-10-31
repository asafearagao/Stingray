using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stingray
{
    public partial class TurmasForm
    {
        // Configura a aparência da tabela de turmas
        private void ConfigurarAparencia()
        {
            BackColor = VisualPadrao.FundoPagina;
            pnlTurmas.BackColor = VisualPadrao.FundoPagina;

            ConfigurarCabecalho();
            ConfigurarBotoes();
            ConfigurarFiltros();
            ConfigurarTabela();
            PreencherTurmasDeExemplo();
        }

        private void ConfigurarCabecalho()
        {
            pnlCabecalhoTurmas.BackColor = VisualPadrao.FundoPagina;
            pnlCabecalhoTurmas.Padding = new Padding(0);

            lblTituloTurmas.AutoSize = true;
            lblTituloTurmas.Location = new Point(13, 17);
            lblTituloTurmas.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblTituloTurmas.ForeColor = Color.FromArgb(17, 24, 39);
            lblTituloTurmas.Text = "Turmas";
        }

        private void ConfigurarBotoes()
        {
            ConfigurarBotaoLimpar();
            ConfigurarBotaoAdicionar();
        }

        // Configura a aparência e os eventos do botão de limpeza dos filtros
        private void ConfigurarBotaoLimpar()
        {
            btnLimparFiltros.Name = "btnLimparFiltros";
            btnLimparFiltros.Text = "Limpar";
            btnLimparFiltros.CornerRadius = 5;
            btnLimparFiltros.BackColor = Color.White;
            btnLimparFiltros.ForeColor = VisualPadrao.TextoPrincipal;
            btnLimparFiltros.FlatAppearance.BorderSize = 0;
            btnLimparFiltros.FlatAppearance.BorderColor = VisualPadrao.BordaCampo;
            btnLimparFiltros.FlatAppearance.MouseOverBackColor = Color.FromArgb(254, 242, 242);
            btnLimparFiltros.FlatAppearance.MouseDownBackColor = Color.FromArgb(254, 226, 226);
            btnLimparFiltros.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            btnLimparFiltros.Dock = DockStyle.Fill;
            btnLimparFiltros.Margin = new Padding(8, 0, 0, 0);
            btnLimparFiltros.Cursor = Cursors.Hand;
            btnLimparFiltros.TextAlign = ContentAlignment.MiddleCenter;
            btnLimparFiltros.Click += AoClicarEmLimparFiltros;
            btnLimparFiltros.MouseEnter += AoPassarMouseSobreLimparFiltros;
            btnLimparFiltros.MouseLeave += AoRetirarMouseDeLimparFiltros;
            btnLimparFiltros.MouseDown += AoPressionarLimparFiltros;
            btnLimparFiltros.MouseUp += AoSoltarLimparFiltros;
        }

        private void AoPassarMouseSobreLimparFiltros(object remetente, EventArgs argumentos)
        {
            btnLimparFiltros.ForeColor = Color.FromArgb(185, 28, 28);
            btnLimparFiltros.FlatAppearance.BorderColor = Color.FromArgb(248, 113, 113);
            btnLimparFiltros.BackColor = Color.FromArgb(254, 242, 242);
        }

        private void AoRetirarMouseDeLimparFiltros(object remetente, EventArgs argumentos)
        {
            btnLimparFiltros.ForeColor = VisualPadrao.TextoPrincipal;
            btnLimparFiltros.FlatAppearance.BorderColor = VisualPadrao.BordaCampo;
            btnLimparFiltros.BackColor = Color.White;
        }

        private void AoPressionarLimparFiltros(object remetente, MouseEventArgs argumentos)
        {
            btnLimparFiltros.ForeColor = Color.FromArgb(153, 27, 27);
            btnLimparFiltros.BackColor = Color.FromArgb(254, 226, 226);
        }

        private void AoSoltarLimparFiltros(object remetente, MouseEventArgs argumentos)
        {
            Point posicaoMouse = btnLimparFiltros.PointToClient(Cursor.Position);
            if (!btnLimparFiltros.ClientRectangle.Contains(posicaoMouse))
                return;

            btnLimparFiltros.ForeColor = Color.FromArgb(185, 28, 28);
            btnLimparFiltros.BackColor = Color.FromArgb(254, 242, 242);
        }

        private void ConfigurarBotaoAdicionar()
        {
            if (btnAdicionar == null)
                return;

            btnAdicionar.Text = "+  Nova turma";
            btnAdicionar.CornerRadius = 5;
            btnAdicionar.BackColor = VisualPadrao.Azul;
            btnAdicionar.ForeColor = Color.White;
            btnAdicionar.FlatStyle = FlatStyle.Flat;
            btnAdicionar.FlatAppearance.BorderSize = 0;
            btnAdicionar.FlatAppearance.BorderColor = VisualPadrao.Azul;
            btnAdicionar.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 73, 168);
            btnAdicionar.FlatAppearance.MouseDownBackColor = Color.FromArgb(18, 61, 140);
            btnAdicionar.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            btnAdicionar.Dock = DockStyle.Fill;
            btnAdicionar.Margin = new Padding(8, 0, 0, 0);
            btnAdicionar.Cursor = Cursors.Hand;
            btnAdicionar.UseVisualStyleBackColor = false;
            btnAdicionar.TextAlign = ContentAlignment.MiddleCenter;
            btnAdicionar.Click -= AoClicarEmNovaTurma;
            btnAdicionar.Click += AoClicarEmNovaTurma;
            btnAdicionar.BringToFront();
        }

        // Configura os campos e as opções para filtrar as turmas
        private void ConfigurarFiltros()
        {
            tblFiltrosTurmas.BackColor = Color.White;
            tblFiltrosTurmas.Padding = new Padding(12, 5, 12, 5);
            tblFiltrosTurmas.Margin = new Padding(0);

            pnlModalidade.Dock = DockStyle.Fill;
            pnlDiaSemana.Dock = DockStyle.Fill;

            PrepararCampoTexto(pnlModalidade, pnlModalidadeSuperficie, txtModalidade);
            PrepararCampoCombo(pnlDiaSemana, pnlDiaSemanaSuperficie, cmbDiaSemana);
            txtModalidade.ReadOnly = false;

            ConfigurarRotulosDosFiltros();
            PreencherDiasDaSemana(cmbDiaSemana);
            cmbDiaSemana.SelectedIndex = 0;
            cmbDiaSemana.ForeColor = VisualPadrao.TextoSecundario;
            ConfigurarPlaceholderTexto(txtModalidade, "Digite a modalidade...");

            txtModalidade.TextChanged += AoAlterarTextoDoFiltroDeModalidade;
            cmbDiaSemana.SelectedIndexChanged += AoAlterarDiaDoFiltro;
        }

        // Configura os rótulos que identificam os campos de filtro
        private void ConfigurarRotulosDosFiltros()
        {
            foreach (Label rotulo in new Label[] { lblModalidade, lblDiaSemana })
            {
                rotulo.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
                rotulo.ForeColor = VisualPadrao.TextoPrincipal;
                rotulo.TextAlign = ContentAlignment.MiddleLeft;
                rotulo.Margin = new Padding(0);
            }

            lblModalidade.Margin = new Padding(0);
            lblDiaSemana.Margin = new Padding(0);
        }

        private void ConfigurarTabela()
        {
            VisualPadrao.ConfigurarGrade(dgvTurmas);
            ConfigurarEventosTabela();
            ConfigurarColunasDaTabela();
        }

        private void ConfigurarColunasDaTabela()
        {
            colDiaSemana.FillWeight = 110F;
            colModalidade.FillWeight = 160F;
            colHorario.FillWeight = 110F;
            colProfessor.FillWeight = 160F;
            colAlunos.FillWeight = 75F;
            colAlunos.ToolTipText = "Clique na lupa para gerenciar os alunos presentes";

            colDiaSemana.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colModalidade.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colHorario.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colProfessor.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colAlunos.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void PrepararPainelDoCampo(Panel painel)
        {
            painel.BackColor = VisualPadrao.BordaCampo;
            painel.Padding = new Padding(1);
            painel.Margin = new Padding(0, 1, 6, 1);
        }

        private void PrepararCampoTexto(Panel painel, Panel superficie, TextBox campo)
        {
            PrepararPainelDoCampo(painel);
            if (campo.Parent != superficie)
                superficie.Controls.Add(campo);

            campo.Enter += delegate { painel.BackColor = VisualPadrao.Azul; };
            campo.Leave += delegate { painel.BackColor = VisualPadrao.BordaCampo; };
            campo.BackColor = Color.White;
            campo.BorderStyle = BorderStyle.None;
            campo.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            campo.ForeColor = VisualPadrao.TextoSecundario;
            campo.Dock = DockStyle.None;
            campo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            campo.Height = 18;
            campo.Margin = new Padding(0);
            superficie.SizeChanged += delegate { PosicionarCampo(superficie, campo); };
            PosicionarCampo(superficie, campo);
        }

        private void PrepararCampoCombo(Panel painel, Panel superficie, ComboBox combo)
        {
            PrepararPainelDoCampo(painel);
            if (combo.Parent != superficie)
                superficie.Controls.Add(combo);

            combo.Enter += delegate { painel.BackColor = VisualPadrao.Azul; };
            combo.Leave += delegate { painel.BackColor = VisualPadrao.BordaCampo; };
            combo.BackColor = Color.White;
            combo.FlatStyle = FlatStyle.Flat;
            combo.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            combo.ForeColor = VisualPadrao.TextoPrincipal;
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.Margin = new Padding(0);
            combo.Dock = DockStyle.Fill;
            combo.BringToFront();

            ConfigurarSetaDoFiltroDeDia(superficie, combo);
        }

        // Configura o botão que abre as opções de dia da semana
        private void ConfigurarSetaDoFiltroDeDia(Panel superficie, ComboBox combo)
        {
            Button seta = btnSetaDiaSemana;
            seta.Name = "btnSetaDiaSemana";
            seta.TabStop = false;
            seta.Text = string.Empty;
            seta.Cursor = Cursors.Hand;
            seta.FlatStyle = FlatStyle.Flat;
            seta.BackColor = Color.White;
            seta.UseVisualStyleBackColor = false;
            seta.Padding = new Padding(0);
            seta.Margin = new Padding(0);
            seta.FlatAppearance.BorderSize = 0;
            seta.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            seta.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            seta.Paint += AoDesenharSetaDoFiltroDeDia;
            seta.MouseEnter += delegate { seta.Invalidate(); };
            seta.MouseLeave += delegate { seta.Invalidate(); };
            seta.Click += delegate { combo.Focus(); combo.DroppedDown = true; };

            if (seta.Parent != superficie)
                superficie.Controls.Add(seta);
            seta.BringToFront();

            Action posicionarSeta = delegate
            {
                int larguraSeta = Math.Min(28, Math.Max(22, superficie.ClientSize.Width / 6));
                seta.SetBounds(
                    Math.Max(0, superficie.ClientSize.Width - larguraSeta),
                    0,
                    larguraSeta,
                    superficie.ClientSize.Height);
                combo.SetBounds(0, 0, superficie.ClientSize.Width, superficie.ClientSize.Height);
                seta.BringToFront();
            };

            superficie.SizeChanged += delegate { posicionarSeta(); };
            posicionarSeta();
        }

        // Desenha a seta do campo de seleção do dia da semana
        private void AoDesenharSetaDoFiltroDeDia(object remetente, PaintEventArgs argumentos)
        {
            Button seta = (Button)remetente;
            Rectangle area = seta.ClientRectangle;
            if (area.Width < 8 || area.Height < 8)
                return;

            bool mouseSobre = area.Contains(seta.PointToClient(Cursor.Position));
            Color cor = mouseSobre
                ? Color.FromArgb(37, 99, 235)
                : Color.FromArgb(100, 116, 139);

            argumentos.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (Pen separador = new Pen(Color.FromArgb(226, 232, 240)))
                argumentos.Graphics.DrawLine(separador, 0, 1, 0, area.Height - 2);

            float centroX = area.Width / 2F;
            float centroY = area.Height / 2F;
            using (Pen caneta = new Pen(cor, 1.6F))
            {
                caneta.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                caneta.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                argumentos.Graphics.DrawLines(caneta, new PointF[]
                {
                    new PointF(centroX - 3.5F, centroY - 1.5F),
                    new PointF(centroX, centroY + 2F),
                    new PointF(centroX + 3.5F, centroY - 1.5F)
                });
            }
        }

        // Posiciona o controle de filtro
        private void PosicionarCampo(Panel superficie, Control campo)
        {
            if (superficie == null || campo == null)
                return;

            campo.Location = new Point(6, Math.Max(1, (superficie.ClientSize.Height - campo.Height) / 2));
            campo.Width = Math.Max(30, superficie.ClientSize.Width - campo.Left - 6);
        }
    }
}