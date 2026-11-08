using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stingray.Formularios
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Preencha o usuário e a senha!");
                return;
            }

            if (txtUsuario.Text == "Admin" && txtSenha.Text == "123")
            {
                DashboardForm dashboard = new DashboardForm();

                dashboard.Show();

                this.Hide();

                dashboard.FormClosed += (s, args) => this.Close();
            }
            else
            {
                MessageBox.Show("Usuário ou senha incorretos!", "Erro de autenticação", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool senhaVisivel = false;
        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            senhaVisivel = !senhaVisivel;

            if (senhaVisivel)
            {
                txtSenha.PasswordChar = '\0';
            }
            else
            {
                txtSenha.PasswordChar = '●';
            }
        }
    }
}
