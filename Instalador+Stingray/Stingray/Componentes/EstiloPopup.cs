using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stingray {
    // Linguagem visual comum aos diálogos de cadastro, edição e consulta
    internal static class EstiloPopup {
        private static readonly Color Fundo = VisualPadrao.FundoPagina;
        private static readonly Color Texto = VisualPadrao.TextoPrincipal;

        // Aplica o padrão visual aos controles da janela
        public static void Aplicar(Form formulario) {
            if (formulario == null || formulario.IsDisposed)
                return;

            formulario.StartPosition = FormStartPosition.CenterParent;
            formulario.FormBorderStyle = FormBorderStyle.FixedDialog;
            formulario.MinimizeBox = false;
            formulario.MaximizeBox = false;
            formulario.ShowInTaskbar = false;
            formulario.BackColor = Fundo;
            formulario.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            // Filete superior discreto que identifica os diálogos do Stingray
            formulario.Paint += delegate (object sender, PaintEventArgs e) {
                if (formulario.IsDisposed || formulario.ClientSize.Width <= 0)
                    return;

                using (Brush pincel = new SolidBrush(VisualPadrao.Azul))
                    e.Graphics.FillRectangle(pincel, 0, 0, formulario.ClientSize.Width, 2);
            };

            AplicarControles(formulario);
        }

        // Permite reutilizar a aparência em telas especiais, como o login,

        // sem alterar o enquadramento ou o comportamento da janela
        internal static void AplicarControles(Control pai) {
            if (pai == null || pai.IsDisposed)
                return;

            foreach (Control controle in pai.Controls) {
                if (controle is TextBoxBase campoTexto) {
                    campoTexto.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
                    campoTexto.ForeColor = Texto;
                    campoTexto.BackColor = Color.White;
                    campoTexto.BorderStyle = BorderStyle.FixedSingle;
                    ConfigurarRealceDeFoco(campoTexto);
                }
                else if (controle is ComboBox combo) {
                    combo.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
                    combo.ForeColor = Texto;
                    combo.BackColor = Color.White;
                    combo.FlatStyle = FlatStyle.Flat;
                    ConfigurarRealceDeFoco(combo);
                }
                else if (controle is CheckedListBox listaMarcacao) {
                    listaMarcacao.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
                    listaMarcacao.ForeColor = Texto;
                    listaMarcacao.BackColor = Color.White;
                    listaMarcacao.BorderStyle = BorderStyle.FixedSingle;
                    listaMarcacao.CheckOnClick = true;
                    listaMarcacao.IntegralHeight = false;
                    listaMarcacao.ItemHeight = 26;
                }
                else if (controle is ListBox lista) {
                    lista.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
                    lista.ForeColor = Texto;
                    lista.BackColor = Color.White;
                    lista.BorderStyle = BorderStyle.FixedSingle;
                    lista.IntegralHeight = false;
                    lista.ItemHeight = 28;
                }
                else if (controle is DateTimePicker data) {
                    data.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
                }
                else if (controle is NumericUpDown numero) {
                    numero.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
                    numero.ForeColor = Texto;
                    numero.BackColor = Color.White;
                    numero.BorderStyle = BorderStyle.FixedSingle;
                    ConfigurarRealceDeFoco(numero);
                }
                else if (controle is RoundedButton botaoArredondado) {
                    ConfigurarBotao(botaoArredondado,
                        EhAcaoPrincipal(botaoArredondado.Text), EhAcaoDestrutiva(botaoArredondado.Text));
                }
                else if (controle is Button seta &&
                         string.Equals(Convert.ToString(seta.Tag), "SetaComboDialogo", StringComparison.Ordinal)) {
                    seta.FlatStyle = FlatStyle.Flat;
                    seta.UseVisualStyleBackColor = false;
                    seta.Cursor = Cursors.Hand;
                    seta.BackColor = Color.White;
                    seta.FlatAppearance.BorderSize = 0;
                    seta.FlatAppearance.MouseOverBackColor = VisualPadrao.AzulClaro;
                    seta.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
                }
                else if (controle is Button botao) {
                    ConfigurarBotaoPadrao(botao,
                        EhAcaoPrincipal(botao.Text), EhAcaoDestrutiva(botao.Text));
                }

                if (controle.HasChildren)
                    AplicarControles(controle);
            }
        }

        // Destaca o campo que está recebendo a digitação
        private static void ConfigurarRealceDeFoco(Control controle) {
            if (controle == null)
                return;

            Color corAnterior = Color.Empty;
            controle.Enter += delegate {
                bool somenteLeitura =
                    (controle is TextBoxBase texto && texto.ReadOnly) ||
                    (controle is NumericUpDown numero && numero.ReadOnly);
                if (!controle.Enabled || controle.IsDisposed || somenteLeitura)
                    return;

                // Guarda a cor atual no momento do foco para respeitar estados
                // dinâmicos da própria tela, como campos desabilitados por regra
                corAnterior = controle.BackColor;
                controle.BackColor = VisualPadrao.FundoCampoFoco;
            };

            controle.Leave += delegate {
                if (controle.IsDisposed || corAnterior.IsEmpty)
                    return;

                controle.BackColor = corAnterior;
                corAnterior = Color.Empty;
            };
        }

        private static bool EhAcaoPrincipal(string texto) {
            if (string.IsNullOrWhiteSpace(texto))
                return false;

            return texto.StartsWith("Salvar", StringComparison.CurrentCultureIgnoreCase) ||
                   texto.StartsWith("Adicionar", StringComparison.CurrentCultureIgnoreCase) ||
                   texto.StartsWith("Criar", StringComparison.CurrentCultureIgnoreCase) ||
                   texto.StartsWith("Confirmar", StringComparison.CurrentCultureIgnoreCase) ||
                   texto.StartsWith("Entrar", StringComparison.CurrentCultureIgnoreCase) ||
                   texto.StartsWith("Acessar", StringComparison.CurrentCultureIgnoreCase) ||
                   texto.StartsWith("Continuar", StringComparison.CurrentCultureIgnoreCase);
        }

        private static bool EhAcaoDestrutiva(string texto) {
            if (string.IsNullOrWhiteSpace(texto))
                return false;

            return texto.StartsWith("Excluir", StringComparison.CurrentCultureIgnoreCase) ||
                   texto.StartsWith("Remover", StringComparison.CurrentCultureIgnoreCase) ||
                   texto.StartsWith("Apagar", StringComparison.CurrentCultureIgnoreCase) ||
                   texto.StartsWith("Desativar", StringComparison.CurrentCultureIgnoreCase);
        }

        // Aplica o estilo de acordo com a função do botão
        private static void ConfigurarBotao(RoundedButton botao, bool principal, bool destrutivo) {
            botao.CornerRadius = 7;
            botao.FlatStyle = FlatStyle.Flat;
            botao.UseVisualStyleBackColor = false;
            botao.Cursor = Cursors.Hand;
            botao.Font = new Font("Segoe UI", 9F,
                principal ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Point);

            if (destrutivo) {
                botao.BackColor = VisualPadrao.FundoExcluir;
                botao.ForeColor = VisualPadrao.IconeExcluir;
                botao.FlatAppearance.BorderSize = 1;
                botao.FlatAppearance.BorderColor = VisualPadrao.BordaExcluir;
                botao.FlatAppearance.MouseOverBackColor = VisualPadrao.FundoExcluirHover;
                botao.FlatAppearance.MouseDownBackColor = VisualPadrao.FundoExcluirPressionada;
            }
            else if (principal) {
                botao.BackColor = VisualPadrao.Azul;
                botao.ForeColor = Color.White;
                botao.FlatAppearance.BorderSize = 0;
                botao.FlatAppearance.BorderColor = VisualPadrao.Azul;
                botao.FlatAppearance.MouseOverBackColor = VisualPadrao.AzulHover;
                botao.FlatAppearance.MouseDownBackColor = VisualPadrao.AzulPressionado;
            }
            else {
                botao.BackColor = Color.White;
                botao.ForeColor = Texto;
                botao.FlatAppearance.BorderSize = 1;
                botao.FlatAppearance.BorderColor = VisualPadrao.BordaCampo;
                botao.FlatAppearance.MouseOverBackColor = VisualPadrao.AzulClaro;
                botao.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            }
        }

        private static void ConfigurarBotaoPadrao(Button botao, bool principal, bool destrutivo) {
            botao.FlatStyle = FlatStyle.Flat;
            botao.UseVisualStyleBackColor = false;
            botao.Cursor = Cursors.Hand;
            botao.Font = new Font("Segoe UI", 9F,
                principal ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Point);

            if (destrutivo) {
                botao.BackColor = VisualPadrao.FundoExcluir;
                botao.ForeColor = VisualPadrao.IconeExcluir;
                botao.FlatAppearance.BorderSize = 1;
                botao.FlatAppearance.BorderColor = VisualPadrao.BordaExcluir;
                botao.FlatAppearance.MouseOverBackColor = VisualPadrao.FundoExcluirHover;
                botao.FlatAppearance.MouseDownBackColor = VisualPadrao.FundoExcluirPressionada;
            }
            else if (principal) {
                botao.BackColor = VisualPadrao.Azul;
                botao.ForeColor = Color.White;
                botao.FlatAppearance.BorderSize = 0;
                botao.FlatAppearance.BorderColor = VisualPadrao.Azul;
                botao.FlatAppearance.MouseOverBackColor = VisualPadrao.AzulHover;
                botao.FlatAppearance.MouseDownBackColor = VisualPadrao.AzulPressionado;
            }
            else {
                botao.BackColor = Color.White;
                botao.ForeColor = Texto;
                botao.FlatAppearance.BorderSize = 1;
                botao.FlatAppearance.BorderColor = VisualPadrao.BordaCampo;
                botao.FlatAppearance.MouseOverBackColor = VisualPadrao.AzulClaro;
                botao.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
            }
        }
    }
}
