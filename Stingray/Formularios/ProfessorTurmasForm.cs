using System.Collections.Generic;
using System.Windows.Forms;

namespace Stingray
{
    [System.ComponentModel.DesignerCategory("Form")]

    // Apresenta as turmas vinculadas a um professor
    public partial class ProfessorTurmasForm : Form
    {
        public ProfessorTurmasForm()
        {
            InitializeComponent();
            EstiloPopup.Aplicar(this);
        }

        // Construtor que recebe o nome do professor e a lista de turmas
        public ProfessorTurmasForm(string nomeProfessor, IEnumerable<string> turmas) : this()
        {
            lblNomeProfessor.Text = nomeProfessor ?? string.Empty;

            // Adiciona as turmas à lista, ignorando valores nulos ou em branco
            if (turmas != null)
            {
                foreach (string turma in turmas)
                {
                    if (!string.IsNullOrWhiteSpace(turma))
                        lstTurmas.Items.Add(turma);
                }
            }

            // Se não houver turmas, exibe um aviso
            if (lstTurmas.Items.Count == 0)
                lstTurmas.Items.Add("Nenhuma turma vinculada.");
        }
    }
}
