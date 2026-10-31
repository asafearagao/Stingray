using System.Drawing;
using System.Windows.Forms;

namespace Stingray {
    // Estilos compartilhados de tabelas
    internal static class VisualPadrao {
        internal static readonly Color FundoPagina = Color.FromArgb(250, 251, 252);
        internal static readonly Color FundoCabecalhoGrade = Color.FromArgb(247, 249, 252);
        internal static readonly Color TextoPrincipal = Color.FromArgb(55, 65, 81);
        internal static readonly Color BordaCartao = Color.FromArgb(226, 232, 240);
        internal static readonly Color BordaCampo = Color.FromArgb(203, 213, 225);
        internal static readonly Color LinhaGrade = Color.FromArgb(235, 239, 245);
        internal static readonly Color Azul = Color.FromArgb(25, 86, 196);
        internal static readonly Color AzulClaro = Color.FromArgb(239, 246, 255);
        internal static readonly Color AzulHover = Color.FromArgb(22, 73, 168);
        internal static readonly Color AzulPressionado = Color.FromArgb(18, 61, 140);
        internal static readonly Color TextoSecundario = Color.FromArgb(100, 116, 139);
        internal static readonly Color TextoPlaceholder = Color.FromArgb(148, 163, 184);
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

        // Seleciona as cores da ação conforme o tipo e o estado visual
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

                } else if (hover) {
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

            } else if (hover) {
                fundo = FundoAcaoHover;
                borda = BordaAcaoHover;
                icone = IconeAcaoHover;
            }
        }

        // Calcula as áreas dos dois botões de ação usados nas tabelas
        internal static Rectangle[] ObterRetangulosDuasAcoes(Rectangle celula) {
            const int tamanho = 26;
            const int espaco = 7;
            int largura = tamanho * 2 + espaco;
            int x = celula.Left + System.Math.Max(2, (celula.Width - largura) / 2);
            int y = celula.Top + System.Math.Max(2, (celula.Height - tamanho) / 2);

            return new Rectangle[] {
                new Rectangle(x, y, tamanho, tamanho),
                new Rectangle(x + tamanho + espaco, y, tamanho, tamanho)
            };
        }

        // Calcula a área de um botão centralizado dentro da célula
        internal static Rectangle ObterRetanguloAcaoCentralizada(Rectangle celula, int tamanho) {
            return new Rectangle(
                celula.Left + (celula.Width - tamanho) / 2,
                celula.Top + (celula.Height - tamanho) / 2,
                tamanho,
                tamanho);
        }

        // Desenha o fundo, a borda e o ícone de um botão de ação
        internal static void DesenharBotaoAcao(Graphics graphics, Rectangle area, bool excluir,
            bool visualizar, bool hover, bool pressionado, string icone) {
            Color fundo;
            Color borda;
            Color corIcone;
            ObterCoresAcao(excluir, visualizar, hover, pressionado, out fundo, out borda, out corIcone);

            using (Brush pincel = new SolidBrush(fundo))
            using (Pen caneta = new Pen(borda)) {
                graphics.FillRectangle(pincel, area);
                graphics.DrawRectangle(caneta, area);
            }

            using (Font fonte = new Font("Segoe MDL2 Assets", 10F)) {
                TextRenderer.DrawText(
                    graphics,
                    icone,
                    fonte,
                    area,
                    corIcone,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }
        }

        internal static void DesenharBordaCartao(Graphics graphics, Rectangle area) {
            if (graphics == null || area.Width <= 1 || area.Height <= 1)
                return;

            area.Width -= 1;
            area.Height -= 1;

            using (Pen borda = new Pen(BordaCartao, 1F))
                graphics.DrawRectangle(borda, area);
        }

        internal static void ConfigurarGrade(DataGridView tabela) {
            if (tabela == null)
                return;

            tabela.BackgroundColor = Color.White;
            tabela.BorderStyle = BorderStyle.None;
            tabela.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            tabela.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            tabela.EnableHeadersVisualStyles = false;
            tabela.ColumnHeadersHeight = 38;
            tabela.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            tabela.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tabela.ColumnHeadersDefaultCellStyle.BackColor = FundoCabecalhoGrade;
            tabela.ColumnHeadersDefaultCellStyle.ForeColor = TextoPrincipal;
            tabela.ColumnHeadersDefaultCellStyle.SelectionBackColor = FundoCabecalhoGrade;
            tabela.ColumnHeadersDefaultCellStyle.SelectionForeColor = TextoPrincipal;
            tabela.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            tabela.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            tabela.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            tabela.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tabela.DefaultCellStyle.BackColor = Color.White;
            tabela.DefaultCellStyle.ForeColor = TextoPrincipal;
            tabela.DefaultCellStyle.SelectionBackColor = Color.White;
            tabela.DefaultCellStyle.SelectionForeColor = TextoPrincipal;
            tabela.DefaultCellStyle.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            tabela.DefaultCellStyle.Padding = new Padding(12, 0, 8, 0);

            tabela.RowsDefaultCellStyle.BackColor = Color.White;
            tabela.RowsDefaultCellStyle.ForeColor = TextoPrincipal;
            tabela.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            tabela.AlternatingRowsDefaultCellStyle.ForeColor = TextoPrincipal;
            tabela.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.White;
            tabela.AlternatingRowsDefaultCellStyle.SelectionForeColor = TextoPrincipal;
            tabela.GridColor = LinhaGrade;
            tabela.RowTemplate.Height = 38;
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

            if (!tabela.IsDisposed)
                tabela.ClearSelection();
        }
    }
}