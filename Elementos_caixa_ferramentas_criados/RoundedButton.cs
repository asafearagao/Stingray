using System;
using System.ComponentModel;
using System.Windows.Forms;

using System.Drawing; // Bibliotecas necessárias para desenhar o botão
using System.Drawing.Drawing2D;

namespace Stingray
{
    [DefaultEvent("Click")]  // Define como se interage com o elemeneto: clicando
    [DefaultValue(false)]    // Define o valor padrão como falso

    [ToolboxItem(true)]      // Adiciona o componente na caixa de ferramentas do Visual Studio
    [Category("Appearance")] // Nome da aba das propriedades na janela de propriedades do Visual Studio

    // Implementa um botão personalizado com cantos arredondados e desenho próprio

    public class RoundedButton : Button
    {
        private int cornerRadius = 9;             // "Quantidade de arredondamento" dos cantos
        [DefaultValue(9)]                         // Valor padrão do arredondamento dos cantos

        private bool pointerInside;               // Mouse em cima que altera a cor de fundo
        private bool pointerPressed;              // Clique que altera a cor de fundo
        private bool showLeftIndicator;           // Exibe o texto colado no lado esquerda 
        private const float BorderThickness = 1f; // Espessura da borda

        public RoundedButton()               // Construtor padrão do botão
        {
            // Ativa as ferramentas de desenho utilizadas
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            FlatStyle = FlatStyle.Flat;       // Sem 3D, sem sombra

            FlatAppearance.BorderSize = 0;    // Desativa a borda padrão

            UseVisualStyleBackColor = false;  // Permite definir a cor de fundo manualmente
            BackColor = Color.White;          // Define a cor de fundo do botão como branco 
        }

        public bool ShowLeftIndicator            // Função do indicador lateral (azul) do botão selecionado
        {
            get { return showLeftIndicator; }    // Retorna se o indicador lateral está habilitado

            set
            {
                if (showLeftIndicator == value)  // Evita redezenhar o indicador se não foi alterado
                    return;

                showLeftIndicator = value;       // Atualiza o estado do indicador lateral

                Invalidate();                    // Redesenha a indicador
            }
        }

        public int CornerRadius
        {
            get { return cornerRadius; }   // Retorna o raio atual das bordas arredondadas

            set
            {
                if (cornerRadius == value) // Evita redesenho se o raio não tiver mudado
                    return;

                cornerRadius = value;      // Atualiza o raio das bordas
                Invalidate();              // Solicita a atualização visual do botão
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e) // Sobrescreve o desenho padrão do botão
        {
            // Obtém a cor de fundo do componente atrás dele (pai)
            // Caso não exista, utiliza a cor padrão do Windows
            Color background = Parent != null
                ? Parent.BackColor
                : SystemColors.Control;

            // Cria um "pincel" com a cor de fundo que pinta o botão
            using (SolidBrush brush = new SolidBrush(background))
                e.Graphics.FillRectangle(brush, ClientRectangle);
        }

        protected override void OnPaint(PaintEventArgs e) // Pinta a borda, o fundo e o texto
        {
            /* Interrompe o desenho caso o botão fique pequeno demais
               (Um tamanho mínimo) */
            if (ClientSize.Width < 2 || ClientSize.Height < 2)
                return;

            Graphics graphics = e.Graphics; // Define o objeto responsável por desenhar

            // Armazena as configurações gráficas padrões para restaurá-las após o desenho do botão
            SmoothingMode oldSmoothing = graphics.SmoothingMode;
            PixelOffsetMode oldOffset = graphics.PixelOffsetMode;
            CompositingQuality oldCompositing = graphics.CompositingQuality;

            graphics.SmoothingMode = SmoothingMode.AntiAlias;             // Ativa um antiserrilhamento da borda
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;       // Melhora a precisão da "pintura"
            graphics.CompositingQuality = CompositingQuality.HighQuality; // Qualidade do processamento gráfico

            try
            {
                Color background = Parent != null // Obtém a cor de fundo do componente que está atrás do botão
                    ? Parent.BackColor
                    : SystemColors.Control;

                using (SolidBrush backgroundBrush = new SolidBrush(background)) // "Limpa" a área pintando um retângulo
                    graphics.FillRectangle(backgroundBrush, ClientRectangle);

                /* Define os limites do contorno
                   O deslocamento de 0.5 evita de pintar a borda */
                RectangleF bounds = new RectangleF(
                    0.5f,
                    0.5f,
                    Math.Max(1f, ClientSize.Width - 1f),
                    Math.Max(1f, ClientSize.Height - 1f));

                // Cria o formato arredondado do pincel com sua cor de fundo atual
                using (GraphicsPath path = CreateRoundedPath(bounds, cornerRadius))
                using (SolidBrush fill = new SolidBrush(GetDisplayedBackColor()))
                {
                    graphics.FillPath(fill, path); // "Pincel" percorre o caminho descrito
                }

                if (showLeftIndicator) // Verifica se o indicador lateral está habilitado
                {
                    GraphicsState indicatorState = graphics.Save(); // Salva o estado gráfico para retorna-lo depois

                    try
                    {
                        using (GraphicsPath buttonShape = CreateRoundedPath(bounds, cornerRadius)) // Cria o caminho para o pincel da borda
                        {
                            graphics.SetClip(buttonShape, CombineMode.Intersect); // Restringe o desenho à área interna do botão

                            RectangleF faixa = new RectangleF( // Define a posição e as dimensões do indicador lateral
                                bounds.Left,
                                bounds.Top + 2.5f,
                                3.5f,
                                Math.Max(1f, bounds.Height - 5f));

                            using (GraphicsPath indicatorPath = CreateRoundedPath(faixa, 2)) // Arredondada as pontas do indicador lateral

                            using (SolidBrush indicatorBrush = new SolidBrush(Color.FromArgb(25, 86, 196))) // Define a cor azul do indicador lateral
                            {
                                graphics.FillPath(indicatorBrush, indicatorPath); // Desenha o indicador lateral dentro dos limites definidos
                            }
                        }

                    } finally {

                        graphics.Restore(indicatorState); // Restaura as configurações gráficas padrões, removendo as temporárias
                    }
                }

                Color borderColor = Enabled      // Define a cor da borda conforme o estado do botão
                    ? FlatAppearance.BorderColor
                    : SystemColors.ControlDark;

                if (borderColor == Color.Empty)  // Utiliza uma cor padrão (cinza clara), caso o ternário falhe
                    borderColor = Color.FromArgb(229, 231, 235);

                using (GraphicsPath path = CreateRoundedPath(bounds, cornerRadius)) // Cria o formato arredondado do pincel da borda
                using (Pen border = new Pen(borderColor, BorderThickness))
                {
                    border.Alignment = PenAlignment.Center; // Centraliza o pincel sobre o contorno do caminho

                    border.LineJoin = LineJoin.Round;       // Suaviza as junções entre os segmentos da borda

                    graphics.DrawPath(border, path);        // "Pincel" percorre caminho
                }

                DrawButtonText(graphics);                   // Desenha o texto do botão

            } finally {

                // Restaura as configurações gráficas originais após finalizar o desenho
                graphics.SmoothingMode = oldSmoothing;
                graphics.PixelOffsetMode = oldOffset;
                graphics.CompositingQuality = oldCompositing;
            }
        }

        private Color GetDisplayedBackColor() // Determina a cor de fundo do botão pelo seu estado
        {
            if (!Enabled)                     // Cor padrão do sistema para um botão desabilitado
                return SystemColors.Control;

            if (pointerPressed)               // Verifica se o botão está pressionado
                return FlatAppearance.MouseDownBackColor;


            if (pointerInside)                // Verifica se o cursor está sobre o botão
                return FlatAppearance.MouseOverBackColor;

            return BackColor;                 // Caso nenhuma condição seja atendida, utiliza a cor padrão do botão
        }

        private void DrawButtonText(Graphics graphics)  // Responsável por desenhar o texto
        {
            Rectangle area = ClientRectangle;           // Obtém a área disponível para o texto

            area.X += Padding.Left + 2;                 // Espaçamento adicional à esquerda para o texto

            area.Y += Padding.Top;                      // Espaçamento adicional superior para o texto

            // Desconta os espaçamentos horizontais e a margem adicional da largura disponível
            area.Width -= Padding.Horizontal + 4;

            area.Height -= Padding.Vertical; // Desconta os espaçamentos verticais da altura disponível

            // Define as formas de renderização do texto
            TextFormatFlags flags = TextFormatFlags.SingleLine |
                                    TextFormatFlags.VerticalCenter |
                                    TextFormatFlags.EndEllipsis |
                                    TextFormatFlags.NoPrefix;

            switch (TextAlign)                     // Verifica o tipo de alinhamento de texto
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    flags |= TextFormatFlags.Left; // Mantém o texto alinhado à esquerda
                    break;

                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    flags |= TextFormatFlags.Right; // Mantém o texto alinhado à direita
                    break;

                default:
                    flags |= TextFormatFlags.HorizontalCenter; // Centraliza apenas os alinhamentos centrais
                    break;
            }

            Color textColor = Enabled // Cor do texto conforme o estado do botão
                ? ForeColor
                : SystemColors.GrayText;

            // Chama a função responsável por desenhar o texto do botão
            TextRenderer.DrawText(graphics, Text, Font, area, textColor, flags);
        }

        protected override void OnMouseEnter(EventArgs e) // Execução quando o mouse entra na área do botão
        {
            pointerInside = true;   // Registra que o cursor está dentro do botão

            base.OnMouseEnter(e);   // Executa o comportamento padrão do evento MouseEnter
            Invalidate();           // Solicita o redesenho para atualizar a cor de fundo
        }

        protected override void OnMouseLeave(EventArgs e) // Execução quando o mouse sai da área do botão
        {
            pointerInside = false;  // Registra que o mouse não está mais dentro do botão
            pointerPressed = false; // Garante que o estado de pressionamento seja desativado

            base.OnMouseLeave(e);   // Executa o comportamento padrão do evento MouseLeave

            Invalidate();           // Solicita o redesenho do botão
        }

        protected override void OnMouseDown(MouseEventArgs e) // Executado quando um botão do mouse é pressionado
        {
            if (e.Button == MouseButtons.Left)                // Registra o estado de precionado
                pointerPressed = true;

            base.OnMouseDown(e);                              // Executa o comportamento padrão do evento MouseDown

            Invalidate();                                     // Solicita o redesenho do botão
        }


        protected override void OnMouseUp(MouseEventArgs e)  // Executado quando o botão do mouse é solto
        {
            pointerPressed = false;                          // Desativa o estado de pressionamento

            base.OnMouseUp(e);                               // Executa o comportamento padrão do evento MouseUp

            Invalidate();                                    // Solicita o redesenho do botão
        }

        // Cria e retorna o caminho dos cantos arredondados
        private static GraphicsPath CreateRoundedPath(RectangleF rect, int radius)
        {
            GraphicsPath path = new GraphicsPath(); // Cria um novo caminho gráfico que armazenará o formato do botão

            /* Calcula o diâmetro dos cantos arredondados
               Impede que o diâmetro ultrapasse as dimensões do retângulo */
            float diameter = Math.Min(
                Math.Max(0, radius) * 2f,
                Math.Min(rect.Width, rect.Height));


            if (diameter < 1f)           // Verifica se o arredondamento é muito pequeno para ser aplicado
            {
                path.AddRectangle(rect); // O caminho gráfico será um retângulo

                return path;             // Retorna o caminho sem arredondamento
            }

            path.AddArc(rect.Left, rect.Top, diameter, diameter, 180, 90);                          // Desenha o arco do canto superior esquerdo
            path.AddArc(rect.Right - diameter, rect.Top, diameter, diameter, 270, 90);              // Desenha o arco do canto superior direito
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);  // Desenha o arco do canto inferior direito
            path.AddArc(rect.Left, rect.Bottom - diameter, diameter, diameter, 90, 90);             // Desenha o arco do canto inferior esquerdo

            path.CloseFigure();                                                                     // Conecta o último ponto ao primeiro

            return path; // Retorna o caminho gráfico completo com os quatro cantos arredondados
        }
    }
}