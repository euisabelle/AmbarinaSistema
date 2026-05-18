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
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Ambarina.UI
{
    public partial class FrmLogin : Form
    {
        private bool redefinicaoAtiva = false;

        public FrmLogin()
        {
            InitializeComponent();
            this.ActiveControl = null; // Isso faz o foco começar no formulário, permitindo ver os placeholders

            // GARANTE que o sistema abre sem segredo e com a cor clarinha
            txtLoginSenha.PasswordChar = '\0'; // Caractere nulo (texto limpo)
            txtLoginSenha.ForeColor = corPlaceholder;
            picLoginNaoVerSenha.Image = Properties.Resources.fluent_eye_off_32_regular;

            VerificarCapsLock();

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
                    // Armazena o usuário na variável estática do Program
                    Program.UsuarioLogado = userEncontrado;

                    MessageBox.Show($"Bem-vinda, {userEncontrado.Nome}!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Aqui abre o Menu Principal
                    FrmMenuPrincipal menu = new FrmMenuPrincipal();
                    menu.Show();
                    this.Hide(); // Esconde a tela de login
                }
                else
                {
                    MessageBox.Show("Usuário ou senha incorretos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLoginSenha.Clear(); // Limpa o campo de senha para nova tentativa
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
                btnLoginEntrar.PerformClick(); // Apenas dispara o clique e deixa o método do botão processar tudo
            }
        }

        private void lblRedefinirSenha_Click(object sender, EventArgs e)
        {
            // Alterna entre modo login e modo redefinição
            lblRedefinirSenha.Visible = false; // Esconde o link de "Esqueci minha senha" para evitar confusão
            SetModoRedefinicao(!redefinicaoAtiva);
            VerificarCapsLock();

        }

        private void SetModoRedefinicao(bool ativar)
        {
            redefinicaoAtiva = ativar;

            // Mostrar/ocultar controles
            lblInstrucoesRedefinicao.Visible = ativar;
            txtLoginNovaSenha.Visible = ativar;
            txtLoginConfirmaSenha.Visible = ativar;
            btnSalvarSenha.Visible = ativar;
            btnCancelarRedefinicao.Visible = ativar;

            // Esconder controles de login que conflitam
            txtLoginSenha.Visible = !ativar;
            picLoginNaoVerSenha.Visible = !ativar;
            btnLoginEntrar.Visible = !ativar;
            panel2.Visible = !ativar;
            panel3.Visible = !ativar;
            lbCaps.Visible = false;

            // Ajusta placeholders (reutiliza txtLoginUsuario para usuário no fluxo de redefinição)
            if (ativar)
            {
                // Garante que o usuário esteja apto a digitar nome (se estava placeholder, mantém)
                if (string.IsNullOrWhiteSpace(txtLoginUsuario.Text) || txtLoginUsuario.Text == "Usuário")
                {
                    txtLoginUsuario.Text = "Usuário";
                    txtLoginUsuario.ForeColor = corPlaceholder;
                }

                // Prepara placeholders dos campos de senha
                txtLoginNovaSenha.Text = "Nova senha";
                txtLoginNovaSenha.ForeColor = corPlaceholder;
                txtLoginNovaSenha.PasswordChar = '\0';

                txtLoginConfirmaSenha.Text = "Confirmar senha";
                txtLoginConfirmaSenha.ForeColor = corPlaceholder;
                txtLoginConfirmaSenha.PasswordChar = '\0';
            }
            else
            {
                // volta ao estado normal
                if (string.IsNullOrWhiteSpace(txtLoginSenha.Text))
                {
                    txtLoginSenha.Text = "Senha";
                    txtLoginSenha.ForeColor = corPlaceholder;
                    txtLoginSenha.PasswordChar = '\0';
                }
            }
        }

        private void btnCancelarRedefinicao_Click(object sender, EventArgs e)
        {
            SetModoRedefinicao(false);
            lblRedefinirSenha.Visible = true; // Mostra o link de "Esqueci minha senha" novamente
        }

        private void btnSalvarSenha_Click(object sender, EventArgs e)
        {
            try
            {
                string usuario = txtLoginUsuario.Text?.Trim() ?? "";
                string nova = txtLoginNovaSenha.Text ?? "";
                string confirma = txtLoginConfirmaSenha.Text ?? "";

                // 1. Valida se o usuário foi preenchido
                if (string.IsNullOrWhiteSpace(usuario) || usuario == "Usuário")
                {
                    MessageBox.Show("Informe o usuário para redefinir a senha.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Verifica se os campos ainda estão com o texto de Placeholder
                if (nova == "Nova senha" || confirma == "Confirmar senha")
                {
                    MessageBox.Show("Informe a nova senha e confirme.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LimparCamposSenhaRedefinicao();
                    return;
                }

                // 3. Verifica se as senhas são iguais
                if (nova != confirma)
                {
                    MessageBox.Show("As senhas não conferem.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LimparCamposSenhaRedefinicao();
                    return;
                }

                // 4. Valida as regras de complexidade (Regex)
                if (!ValidarSenha(nova, out string motivo))
                {
                    MessageBox.Show($"Senha inválida: {motivo}", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LimparCamposSenhaRedefinicao();
                    return;
                }

                UsuarioBLL bll = new UsuarioBLL();

                // 5. Valida se é igual à senha anterior
                if (!bll.ValidarSenhaAnterior(usuario, nova))
                {
                    MessageBox.Show("A nova senha não pode ser igual à senha anterior.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LimparCamposSenhaRedefinicao();
                    return;
                }

                // 6. Tenta persistir no banco
                if (bll.AlterarSenha(usuario, nova))
                {
                    MessageBox.Show("Senha alterada com sucesso. Faça login com a nova senha.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SetModoRedefinicao(false);
                }
                else
                {
                    MessageBox.Show("Usuário não encontrado ou erro ao salvar a senha.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtLoginUsuario.Clear();
                    LimparCamposSenhaRedefinicao();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LimparCamposSenhaRedefinicao();
            }
        }

        // Método auxiliar para evitar repetição de código (DRY - Don't Repeat Yourself)
        private void LimparCamposSenhaRedefinicao()
        {
            txtLoginNovaSenha.Clear();
            txtLoginConfirmaSenha.Clear();

            // Opcional: Voltar o foco para o primeiro campo de senha
            txtLoginNovaSenha.Focus();
        }

        // Valida regras de segurança da senha
        // Regras aplicadas:
        // - mínimo 8 caracteres
        // - pelo menos uma letra maiúscula
        // - pelo menos uma letra minúscula
        // - pelo menos um dígito
        // - pelo menos um caractere especial
        private bool ValidarSenha(string senha, out string motivo)
        {
            motivo = "";
            if (string.IsNullOrEmpty(senha) || senha.Length < 8)
            {
                motivo = "mínimo de 8 caracteres.";
                return false;
            }
            if (!Regex.IsMatch(senha, "[A-Z]"))
            {
                motivo = "precisa conter pelo menos uma letra maiúscula.";
                return false;
            }
            if (!Regex.IsMatch(senha, "[a-z]"))
            {
                motivo = "precisa conter pelo menos uma letra minúscula.";
                return false;
            }
            if (!Regex.IsMatch(senha, "[0-9]"))
            {
                motivo = "precisa conter pelo menos um dígito.";
                return false;
            }
            if (!Regex.IsMatch(senha, "[^a-zA-Z0-9]"))
            {
                motivo = "precisa conter pelo menos um caractere especial (ex: !@#$%).";
                return false;
            }
            return true;
        }

        // Placeholders e eventos para novos campos de senha
        private void txtLoginNovaSenha_Enter(object sender, EventArgs e)
        {
            if (txtLoginNovaSenha.Text == "Nova senha")
            {
                txtLoginNovaSenha.Text = "";
                txtLoginNovaSenha.ForeColor = corDigitacao;
                txtLoginNovaSenha.PasswordChar = '•';
            }
        }

        private void txtLoginNovaSenha_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLoginNovaSenha.Text))
            {
                txtLoginNovaSenha.Text = "Nova senha";
                txtLoginNovaSenha.ForeColor = corPlaceholder;
                txtLoginNovaSenha.PasswordChar = '\0';
            }
        }

        private void txtLoginConfirmaSenha_Enter(object sender, EventArgs e)
        {
            if (txtLoginConfirmaSenha.Text == "Confirmar senha")
            {
                txtLoginConfirmaSenha.Text = "";
                txtLoginConfirmaSenha.ForeColor = corDigitacao;
                txtLoginConfirmaSenha.PasswordChar = '•';
            }
        }

        private void txtLoginConfirmaSenha_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLoginConfirmaSenha.Text))
            {
                txtLoginConfirmaSenha.Text = "Confirmar senha";
                txtLoginConfirmaSenha.ForeColor = corPlaceholder;
                txtLoginConfirmaSenha.PasswordChar = '\0';
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }

        private void txtLoginConfirmaSenha_KeyDown(object sender, KeyEventArgs e)
        {
            // Verifica se a tecla pressionada foi o Enter
            if (e.KeyCode == Keys.Enter)
            {
                // Evita o som de "beep" do Windows
                e.SuppressKeyPress = true;

                // Executa a ação do botão Salvar
                btnSalvarSenha.PerformClick();
            }
        }
    }
}
