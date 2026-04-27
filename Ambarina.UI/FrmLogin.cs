using Ambarina.BLL;
using Ambarina.DAL;
using Ambarina.DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ambarina.UI
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
            this.ActiveControl = null; // Isso faz o foco começar no formulário, permitindo ver os placeholders

            // GARANTE que o sistema abre sem segredo e com a cor clarinha
            txtLoginSenha.PasswordChar = '\0'; // Caractere nulo (texto limpo)
            txtLoginSenha.ForeColor = corPlaceholder;
            picLoginNaoVerSenha.Image = Properties.Resources.fluent_eye_off_32_regular;
        }

        private void lbLoginExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lbLoginExit_MouseEnter(object sender, EventArgs e)
        {
            lbLoginExit.ForeColor = ColorTranslator.FromHtml("#8A4B2E");
        }

        private void lbLoginExit_MouseLeave(object sender, EventArgs e)
        {
            lbLoginExit.ForeColor = ColorTranslator.FromHtml("#2B211B");
        }

        Color corDigitacao = ColorTranslator.FromHtml("#be8a3a"); // Caramelo
        Color corPlaceholder = ColorTranslator.FromHtml("#bea989");
        private void txtLoginUsuario_Enter(object sender, EventArgs e)
        {
            // Se o texto for o placeholder, limpa o campo e muda a cor para escuro
            if (txtLoginUsuario.Text == "Usuário")
            {
                txtLoginUsuario.Text = "";
                txtLoginUsuario.ForeColor = corDigitacao;
            }
        }

        private void txtLoginUsuario_Leave(object sender, EventArgs e)
        {
            // Se o usuário saiu e não digitou nada, volta o placeholder e a cor marrom claro
            if (string.IsNullOrWhiteSpace(txtLoginUsuario.Text))
            {
                txtLoginUsuario.Text = "Usuário";
                txtLoginUsuario.ForeColor = corPlaceholder;
            }
        }

        private void txtLoginUsuario_TextChanged(object sender, EventArgs e)
        {
            if (txtLoginUsuario.Text != "Usuário" && !string.IsNullOrWhiteSpace(txtLoginUsuario.Text))
            {
                txtLoginUsuario.ForeColor = corDigitacao; // Caramelo quando você digita
            }
        }

        private void txtLoginSenha_TextChanged(object sender, EventArgs e)
        {
            if (txtLoginSenha.Text != "Senha" && !string.IsNullOrWhiteSpace(txtLoginSenha.Text))
            {
                txtLoginSenha.ForeColor = corDigitacao; // Caramelo quando você digita
            }
        }

        private void txtLoginSenha_Leave(object sender, EventArgs e)
        {
            // Se sair e estiver vazio, volta o placeholder
            if (string.IsNullOrWhiteSpace(txtLoginSenha.Text))
            {
                txtLoginSenha.Text = "Senha";
                txtLoginSenha.ForeColor = corPlaceholder; // Volta a cor clarinha
                txtLoginSenha.UseSystemPasswordChar = false; // Tira as bolinhas para ler "Senha"
            }
        }

        private void txtLoginSenha_Enter(object sender, EventArgs e)
        {
            VerificarCapsLock();
            if (txtLoginSenha.Text == "Senha")
            {
                txtLoginSenha.Text = "";
                txtLoginSenha.ForeColor = corDigitacao; //muda para caramelo quando começa a digitar                                                    
                txtLoginSenha.PasswordChar = '•';// Só ativa a ocultação se o campo for limpo para digitar

                picLoginNaoVerSenha.Image = Properties.Resources.fluent_eye_32_regular;// Se a senha acabou de ser ocultada, o ícone deve ser (Olho Aberto)
            }
        }

        private void FrmLogin_Shown(object sender, EventArgs e)
        {
            // Força o foco para a logo ou para o próprio form, longe dos inputs
            this.ActiveControl = null;
        }

        private void picLoginNaoVerSenha_Click(object sender, EventArgs e)
        {
            if (txtLoginSenha.PasswordChar == '•')
            {
                // AÇÃO: Mostrar a senha
                txtLoginSenha.PasswordChar = '\0';
                // ÍCONE: Mostra o olho cortado (ação de esconder disponível)
                picLoginNaoVerSenha.Image = Properties.Resources.fluent_eye_off_32_regular;
            }
            else
            {
                // AÇÃO: Esconder a senha
                txtLoginSenha.PasswordChar = '•';
                // ÍCONE: Mostra o olho aberto (ação de mostrar disponível)
                picLoginNaoVerSenha.Image = Properties.Resources.fluent_eye_32_regular;
            }
        }


        private void VerificarCapsLock()
        {
            // Control.IsKeyLocked verifica o estado físico da tecla no Windows
            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                lbCaps.Visible = true;
            }
            else
            {
                lbCaps.Visible = false;
            }
        }

        private void FrmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            VerificarCapsLock();
        }

        //ACESSO
        private void btnLoginEntrar_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioBLL bll = new UsuarioBLL();
                // Pegamos o que o user digitou nas TextBoxes
                UsuarioDTO userEncontrado = bll.Autenticar(txtLoginUsuario.Text, txtLoginSenha.Text);

                if (userEncontrado != null)
                {
                    MessageBox.Show($"Bem-vinda, {userEncontrado.Nome}!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Aqui abre o Menu Principal
                    FrmMenuPrincipal menu = new FrmMenuPrincipal();
                    menu.Show();
                    this.Hide(); // Esconde a tela de login
                }
                else
                {
                    MessageBox.Show("Usuário ou senha incorretos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtLoginUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            // Se a tecla pressionada for o Enter
            if (e.KeyCode == Keys.Enter)
            {
                // Evita o som de "beep" do Windows
                e.SuppressKeyPress = true;

                // Chama o método do botão de login que já criamos
                btnLoginEntrar.PerformClick();
            }
        }

        private void txtLoginSenha_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnLoginEntrar.PerformClick(); // Executa o clique do botão via código
            }
        }

        private void lblRedefinirSenha_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Função de redefinir senha ainda não implementada.", "Em Breve", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
