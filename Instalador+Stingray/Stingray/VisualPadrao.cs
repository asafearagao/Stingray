using System.Drawing;
using System.Windows.Forms;

namespace Stingray {
    // Mantém a identidade azul da AquaFit e evita variações entre telas
    internal static class VisualPadrao {
        // Cores de fundo
        internal static readonly Color FundoPagina = Color.FromArgb(250, 251, 252);
        internal static readonly Color FundoSuperficie = Color.White;
        internal static readonly Color FundoCabecalhoGrade = Color.FromArgb(247, 249, 252);
        internal static readonly Color FundoLinhaAlternada = Color.White;
        internal static readonly Color FundoSelecaoGrade = Color.White;
        internal static readonly Color FundoCampoFoco = Color.FromArgb(248, 251, 255);
        internal static readonly Color FundoBotaoDesabilitado = Color.FromArgb(241, 245, 249);

        // Cores para textos
        internal static readonly Color TextoPrincipal = Color.FromArgb(55, 65, 81);
        internal static readonly Color TextoSecundario = Color.FromArgb(100, 116, 139);
        internal static readonly Color TextoPlaceholder = Color.FromArgb(148, 163, 184);
        internal static readonly Color TextoDesabilitado = Color.FromArgb(148, 163, 184);

        // Cores para elementos
        internal static readonly Color BordaCartao = Color.FromArgb(226, 232, 240);
        internal static readonly Color BordaCampo = Color.FromArgb(203, 213, 225);
        internal static readonly Color BordaFoco = Color.FromArgb(147, 197, 253);
        internal static readonly Color LinhaGrade = Color.FromArgb(235, 239, 245);

        // Azul nos elementos de ação e navegação
        internal static readonly Color Azul = Color.FromArgb(25, 86, 196);
        internal static readonly Color AzulClaro = Color.FromArgb(239, 246, 255);
        internal static readonly Color AzulHover = Color.FromArgb(22, 73, 168);
        internal static readonly Color AzulPressionado = Color.FromArgb(18, 61, 140);

        internal static readonly Color FundoAcao = Color.FromArgb(247, 250, 255);
        internal static readonly Color BordaAcao = Color.FromArgb(219, 234, 254);
        internal static readonly Color IconeAcao = Color.FromArgb(25, 86, 196);
        internal static readonly Color FundoAcaoHover = Color.FromArgb(219, 234, 254);
        internal static readonly Color BordaAcaoHover = Color.FromArgb(147, 197, 253);
        internal static readonly Color IconeAcaoHover = Color.FromArgb(29, 78, 216);
        internal static readonly Color FundoAcaoPressionada = Color.FromArgb(191, 219, 254);
        internal static readonly Color BordaAcaoPressionada = Color.FromArgb(96, 165, 250);

        internal static readonly Color FundoExcluir = Color.FromArgb(255, 248, 248);
        internal static readonly Color BordaExcluir = Color.FromArgb(254, 226, 226);
        internal static readonly Color IconeExcluir = Color.FromArgb(220, 38, 38);
        internal static readonly Color FundoExcluirHover = Color.FromArgb(254, 226, 226);
        internal static readonly Color BordaExcluirHover = Color.FromArgb(252, 165, 165);
        internal static readonly Color IconeExcluirHover = Color.FromArgb(185, 28, 28);
        internal static readonly Color FundoExcluirPressionada = Color.FromArgb(254, 202, 202);
        internal static readonly Color BordaExcluirPressionada = Color.FromArgb(248, 113, 113);

        internal static void ObterCoresAcao(bool excluir, bool visualizar, bool hover, bool pressionado,
            out Color fundo, out Color borda, out Color icone) {
            if (excluir) {
                fundo = FundoExcluir;
                borda = BordaExcluir;
                icone = IconeExcluir;

                if (pressionado) {
                    fundo = FundoExcluirPressionada;
                    borda = BordaExcluirPressionada;
                    icone = IconeExcluirHover;
                }
                else if (hover) {
                    fundo = FundoExcluirHover;
                    borda = BordaExcluirHover;
                    icone = IconeExcluirHover;
                }

                return;
            }

            fundo = FundoAcao;
            borda = BordaAcao;
            icone = visualizar ? TextoPrincipal : IconeAcao;

            if (pressionado) {
                fundo = FundoAcaoPressionada;
                borda = BordaAcaoPressionada;
                icone = IconeAcaoHover;
            }
            else if (hover) {
                fundo = FundoAcaoHover;
                borda = BordaAcaoHover;
                icone = IconeAcaoHover;
            }
        }

        // Desenha a borda cinza de um cartão
        internal static void DesenharBordaCartao(Graphics graphics, Rectangle area) {
            if (graphics == null || area.Width <= 1 || area.Height <= 1)
                return;

            area.X += 1;
            area.Y += 1;
            area.Width -= 3;
            area.Height -= 3;

            using (Pen borda = new Pen(BordaCartao, 1F))
                graphics.DrawRectangle(borda, area);
        }

        // Configurações das colunas e células de cada tela
        internal static void ConfigurarGrade(DataGridView tabela) {
            if (tabela == null || tabela.IsDisposed)
                return;

            tabela.BackgroundColor = FundoSuperficie;
            tabela.BorderStyle = BorderStyle.None;
            tabela.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            tabela.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            tabela.EnableHeadersVisualStyles = false;
            tabela.ColumnHeadersHeight = 40;
            tabela.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            tabela.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tabela.ColumnHeadersDefaultCellStyle.BackColor = FundoCabecalhoGrade;
            tabela.ColumnHeadersDefaultCellStyle.ForeColor = TextoPrincipal;
            tabela.ColumnHeadersDefaultCellStyle.SelectionBackColor = FundoCabecalhoGrade;
            tabela.ColumnHeadersDefaultCellStyle.SelectionForeColor = TextoPrincipal;
            tabela.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            tabela.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            tabela.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            tabela.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tabela.DefaultCellStyle.BackColor = FundoSuperficie;
            tabela.DefaultCellStyle.ForeColor = TextoPrincipal;

            tabela.DefaultCellStyle.SelectionBackColor = FundoSelecaoGrade;
            tabela.DefaultCellStyle.SelectionForeColor = TextoPrincipal;
            tabela.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            tabela.DefaultCellStyle.Padding = new Padding(12, 0, 8, 0);

            tabela.RowsDefaultCellStyle.BackColor = FundoSuperficie;
            tabela.RowsDefaultCellStyle.ForeColor = TextoPrincipal;
            tabela.RowsDefaultCellStyle.SelectionBackColor = FundoSelecaoGrade;
            tabela.RowsDefaultCellStyle.SelectionForeColor = TextoPrincipal;

            tabela.AlternatingRowsDefaultCellStyle.BackColor = FundoLinhaAlternada;
            tabela.AlternatingRowsDefaultCellStyle.ForeColor = TextoPrincipal;

            tabela.AlternatingRowsDefaultCellStyle.SelectionBackColor = FundoLinhaAlternada;
            tabela.AlternatingRowsDefaultCellStyle.SelectionForeColor = TextoPrincipal;

            tabela.GridColor = LinhaGrade;
            tabela.RowTemplate.Height = 42;
            tabela.RowHeadersVisible = false;
            tabela.AllowUserToAddRows = false;
            tabela.AllowUserToDeleteRows = false;
            tabela.AllowUserToResizeRows = false;
            tabela.AllowUserToResizeColumns = false;
            tabela.AllowUserToOrderColumns = false;
            tabela.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tabela.MultiSelect = false;
            tabela.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            foreach (DataGridViewColumn coluna in tabela.Columns)
                coluna.Resizable = DataGridViewTriState.False;

            tabela.ClearSelection();
        }

        internal static void ConfigurarComboPopup(ComboBox combo)
        {
            if (combo == null || combo.IsDisposed)
                return;

            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.FlatStyle = FlatStyle.Standard;
            combo.BackColor = FundoSuperficie;
            combo.ForeColor = TextoPrincipal;
            combo.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            combo.Cursor = Cursors.Hand;
            combo.IntegralHeight = false;
            combo.Margin = new Padding(0);
        }
    }
}
