using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stingray.Interface
{
    internal static class PlaceholderTexto
    {
        private const int MensagemDefinirPlaceholder = 0x1501;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr identificadorJanela, int mensagem, IntPtr parametro, string texto);

        // Aplica o placeholder quando o identificador da janela está disponível
        public static void Aplicar(TextBox campo, string texto)
        {
            if (campo.IsHandleCreated)
            {
                DefinirPlaceholder(campo, texto);
                return;
            }

            campo.HandleCreated += (remetente, argumentos) => DefinirPlaceholder(campo, texto);
        }

        // Define o texto de placeholder no controle por meio de uma mensagem do Windows
        private static void DefinirPlaceholder(TextBox campo, string texto)
        {
            SendMessage(campo.Handle, MensagemDefinirPlaceholder, (IntPtr)1, texto);
        }
    }
}