using System;
using System.Windows.Forms;

namespace Stingray {
    internal static class Program {
        [STAThread]
        // Inicia o aplicativo e garante o encerramento do serviço do WhatsApp
        private static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try {
                Application.Run(new DashboardForm());
            
            } finally {
                // Garante o encerramento da tarefa do WebDriver de WhatsApp
                WhatsAppService.EncerrarAsync().GetAwaiter().GetResult();
            }
        }
    }
}