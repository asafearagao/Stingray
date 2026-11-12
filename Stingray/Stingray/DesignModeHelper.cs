using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Stingray {
    // Detecta a pré-visualização do Windows Forms Designer
    internal static class DesignModeHelper {
        public static bool IsDesignMode(Control control = null) {
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
