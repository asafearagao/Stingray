using System;
using System.Drawing;
using System.Windows.Forms;
using Stingray.Interface;

namespace Stingray.Formularios.Alunos.Componentes
{
    internal sealed class OpcaoFiltroAluno
    {
        private readonly ComboBox lista;
        private readonly BotaoSetaFiltroAluno botaoSeta;

        public event EventHandler Alterado;

        // Inicializa a lista de opções
        public OpcaoFiltroAluno(ComboBox lista, Button botaoSeta)
        {
            if (lista == null)
                throw new ArgumentNullException(nameof(lista));

            this.lista = lista;
            this.botaoSeta = new BotaoSetaFiltroAluno(botaoSeta, lista);
        }

        public string ValorSelecionado
        {
            get
            {
                if (lista.SelectedIndex <= 0)
                    return string.Empty;

                return Convert.ToString(lista.SelectedItem);
            }
        }

        // Configura a lista de opções
        public void Configurar(bool emModoDesign)
        {
            ConfigurarLista();
            botaoSeta.Configurar();

            if (!emModoDesign)
                lista.SelectedIndexChanged += AoAlterarSelecao;
        }

        // Seleciona primeira opção (padrão) da lista
        public void SelecionarPrimeiraOpcao()
        {
            if (lista.Items.Count > 0)
                lista.SelectedIndex = 0;
        }

        private void ConfigurarLista()
        {
            lista.DropDownStyle = ComboBoxStyle.DropDownList;
            lista.FlatStyle = FlatStyle.Flat;
            lista.DrawMode = DrawMode.OwnerDrawFixed;
            lista.ItemHeight = 18;
            lista.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            lista.ForeColor = VisualPadrao.TextoPrincipal;
            lista.BackColor = Color.White;
            lista.Margin = new Padding(0);
            lista.DrawItem += AoDesenharItem;
        }

        private void AoDesenharItem(object remetente, DrawItemEventArgs argumentos)
        {
            DesenhoOpcaoFiltroAluno.DesenharItem(lista, argumentos);
        }

        private void AoAlterarSelecao(object remetente, EventArgs argumentos)
        {
            Alterado?.Invoke(this, EventArgs.Empty);
        }
    }
}