
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stingray
{
    // Implementa um controle de alternância visual baseado em CheckBox
    public sealed class ToggleSwitch : CheckBox  // Construtor padrão, herdado a classe CheckBox
    {
        public ToggleSwitch()                   // Inicializa as configurações padrão do interruptor
        {
            AutoSize = false;                   // Permite definir manualmente as dimensões
            Size = new Size(46, 24);            // Define a largura e a altura do interruptor
            Cursor = Cursors.Hand;              // Altera o cursor para indicar que o elemento é clicável
            Text = string.Empty;                // Remove qualquer texto associado ao componente

            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true); // Ativa as ferramentas para o desenho
        }

        protected override void OnPaint(PaintEventArgs e)                   // Desenha o interruptor
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;             // Suaviza as bordas das formas

            e.Graphics.Clear(Parent == null                                 // A cor de fundo a mesma do elemento "de atrás" (pai)
                ? BackColor
                : Parent.BackColor); 

            Rectangle trilho = new Rectangle(1, 2, Width - 3, Height - 5);  // Define a posição e as dimensões do trilho

            int diametro = trilho.Height - 6;                               // Define o diâmetro do indicador circular

            using (GraphicsPath path = new GraphicsPath())                  // Cria o caminho gráfico do trilho
            {
                int d = trilho.Height;                                      // Define o diâmetro da borda arredondada

                path.AddArc(trilho.X, trilho.Y, d, d, 90, 180);             // Desenha a extremidade esquerda
                path.AddArc(trilho.Right - d, trilho.Y, d, d, 270, 180);    // Desenha a extremidade direita
                path.CloseFigure();                                         // Conecta as pontas e fecha

                using (SolidBrush fundo = new SolidBrush(Checked
                    ? Color.FromArgb(37, 99, 235)       // Azul quando o interruptor está ativado
                    : Color.FromArgb(203, 213, 225)))   // Cinza quando o interruptor está desativado
                {
                    e.Graphics.FillPath(fundo, path);   // Preenche o trilho com a cor do estado
                }
            }
                     
            int x = Checked                    // Posição do circulo indicador
                ? Width - diametro - 5         // Posiciona o indicador à direita quando ativado
                : 5;                           // Posiciona o indicador à esquerda quando desativado

            Rectangle bola = new Rectangle(x, (Height - diametro) / 2, diametro, diametro); // Define a posição e as dimensões do circulo

            using (SolidBrush branco = new SolidBrush(Color.White))
                e.Graphics.FillEllipse(branco, bola); // Desenha o circulo branco
        }
    }
}