namespace Ambarina.UI
{
    public partial class FrmSplash : Form
    {
        public FrmSplash()
        {
            InitializeComponent();
        }

        int ciclos = 0;
        private void tmSplash_Tick(object sender, EventArgs e)
        {
            ciclos++;

            // --- ESTÁGIO 1: EFEITO DE PULSAR A LOGO ---
            // Faz a logo crescer e diminuir sutilmente usando o resto da divisão (%)
            if (ciclos % 20 < 10)
            {
                picSplashLogo.Width += 2;
                picSplashLogo.Height += 2;
                picSplashLogo.Top -= 1;  // Ajusta para manter centralizado
                picSplashLogo.Left -= 1;
            }
            else
            {
                picSplashLogo.Width -= 2;
                picSplashLogo.Height -= 2;
                picSplashLogo.Top += 1;
                picSplashLogo.Left += 1;
            }

            // --- ESTÁGIO 2: APARECIMENTO DA BARRA E LABEL ---
            // Após 15 ciclos (cerca de 1 segundo se o timer estiver em 50ms), mostramos a barra
            if (ciclos == 15)
            {
                progressBar1.Visible = true;
                lbSplashCarregando.Visible = true;
            }

            // --- ESTÁGIO 3: PROGRESSO DA BARRA ---
            // A barra só começa a carregar depois que se torna visível
            if (progressBar1.Visible && progressBar1.Value < 100)
            {
                progressBar1.Value += 2;

                // mensagens personalizadas
                if (progressBar1.Value == 30) lbSplashCarregando.Text = "Carregando insumos...";
                if (progressBar1.Value == 60) lbSplashCarregando.Text = "Preparando estoque pronto...";
                if (progressBar1.Value == 90) lbSplashCarregando.Text = "Quase pronto...";
            }
            else if (progressBar1.Value == 100)
            {
                tmSplash.Stop();

                // Cria a instância da tela de login
                FrmLogin telaLogin = new FrmLogin();

                // Mostra o login e esconde o splash
                telaLogin.Show();
                this.Hide();
            }
        }
    }
}
