using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stingray {
    public partial class ConfiguracoesForm {
        // Baixa a logo do WhatsApp
        private async Task CarregarLogoWhatsAppAsync(PictureBox pictureBox) {
            if (carregandoLogo || IsDisposed || pictureBox == null || pictureBox.IsDisposed)
                return;

            carregandoLogo = true;
            ultimaTentativaLogo = DateTime.Now;

            string[] urlsLogo = new string[] {
                "https://thumb.wikimedia.org/wikipedia/commons/thumb/5/5e/WhatsApp_icon.png/250px-WhatsApp_icon.png?utm_campaign=index&utm_content=thumbnail&utm_source=commons.wikimedia.org",
                "https://upload.wikimedia.org/wikipedia/commons/5/5e/WhatsApp_icon.png"
            };
            string pastaCache = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Stingray", "Assets");
            string caminhoLogo = Path.Combine(pastaCache, "whatsapp-icon-v2.png");

            try {
                if (!File.Exists(caminhoLogo)) {
                    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                    Directory.CreateDirectory(pastaCache);
                    byte[] bytes = null;
                    Exception erroDownload = null;

                    using (HttpClient cliente = new HttpClient()) {
                        cliente.Timeout = TimeSpan.FromSeconds(8);
                        cliente.DefaultRequestHeaders.UserAgent.ParseAdd("StingrayApp/1.0");

                        foreach (string urlLogo in urlsLogo) {
                            try {
                                bytes = await cliente.GetByteArrayAsync(urlLogo);
                                if (bytes != null && bytes.Length > 1000)
                                    break;
                                bytes = null;

                            } catch (Exception ex) {
                                erroDownload = ex;
                            }
                        }
                    }

                    if (bytes == null)
                        throw new InvalidOperationException(
                            "Não foi possível baixar a imagem do logotipo do WhatsApp.", erroDownload);

                    Directory.CreateDirectory(pastaCache);
                    File.WriteAllBytes(caminhoLogo, bytes);
                }

                if (IsDisposed || pictureBox.IsDisposed)
                    return;

                using (Image imagem = Image.FromFile(caminhoLogo))
                    pictureBox.Image = new Bitmap(imagem);

            } catch {
                // Se o cache estiver corrompido permiti novo download depois
                try {
                    if (File.Exists(caminhoLogo))
                        File.Delete(caminhoLogo);
                } catch { }

                // Sem internet no primeiro uso o ícone não será exibido
                if (!IsDisposed && !pictureBox.IsDisposed)
                    pictureBox.Image = null;

            } finally {
                carregandoLogo = false;
            }
        }
    }
}