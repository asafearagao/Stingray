using System;
using System.Windows.Forms;

namespace Stingray {
    public partial class PagamentosForm {
        // Libera os recursos do formulário quando ele é fechado
        protected override void OnFormClosed(FormClosedEventArgs e) {
            if (filtroCliqueExterno != null) {
                Application.RemoveMessageFilter(filtroCliqueExterno);
                filtroCliqueExterno = null;
            }
            base.OnFormClosed(e);
        }

        private sealed class FiltroFecharPopup : IMessageFilter {
            private const int WM_LBUTTONDOWN = 0x0201;
            private const int WM_NCLBUTTONDOWN = 0x00A1;
            private readonly Form form;
            private readonly Control popup;
            private readonly Control acionador;
            private readonly Control popupSecundario;

            // Inicializa o detector de cliques externos aos popups
            public FiltroFecharPopup(Form form, Control popup, Control acionador, Control popupSecundario) {
                this.form = form;
                this.popup = popup;
                this.acionador = acionador;
                this.popupSecundario = popupSecundario;
            }

            public bool PreFilterMessage(ref Message m) {
                if ((m.Msg != WM_LBUTTONDOWN && m.Msg != WM_NCLBUTTONDOWN) || popup == null || !popup.Visible)
                    return false;

                Control alvo = Control.FromHandle(m.HWnd);
                if (alvo != null && alvo.FindForm() != form)
                    return false;
                if (alvo == null) {
                    if (!form.Bounds.Contains(Cursor.Position)) {
                        if (popupSecundario != null) popupSecundario.Visible = false;
                        popup.Visible = false;
                    }

                    return false;
                }

                if (EhFilhoDe(alvo, popupSecundario))
                    return false;
                if (EhFilhoDe(alvo, popup) || EhFilhoDe(alvo, acionador)) {
                    if (popupSecundario != null && popupSecundario.Visible && !EhFilhoDe(alvo, popupSecundario))
                        popupSecundario.Visible = false;
                    return false;
                }

                if (popupSecundario != null)
                    popupSecundario.Visible = false;
                popup.Visible = false;
                return false;
            }

            private static bool EhFilhoDe(Control controle, Control pai) {
                if (controle == null || pai == null)
                    return false;
                Control atual = controle;
                while (atual != null) {
                    if (atual == pai)
                        return true;
                    atual = atual.Parent;
                }
                return false;
            }
        }
    }
}