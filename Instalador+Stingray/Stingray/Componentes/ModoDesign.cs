using System;
using System.ComponentModel;  // Controles do Windows Forms
using System.Diagnostics;
using System.Windows.Forms;   // Controles do Windows Forms

namespace Stingray {
    // Detecta a pré-visualização do Windows Forms Designer
    internal static class ModoDesign {

        public static bool EstaEmModoDesign(Control control = null) {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return true;

            if (control != null && control.Site != null && control.Site.DesignMode)
                return true;

            try {
                string processo = Process.GetCurrentProcess().ProcessName;
                return processo.Equals("devenv", StringComparison.OrdinalIgnoreCase)
                    || processo.StartsWith("DesignToolsServer", StringComparison.OrdinalIgnoreCase)
                    || processo.StartsWith("XDesProc", StringComparison.OrdinalIgnoreCase)
                    || processo.Equals("Blend", StringComparison.OrdinalIgnoreCase);

            } catch {
                return false;
            }
        }
    }
}
