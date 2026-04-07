using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Ambarina.UI
{
    public partial class FrmMenuPrincipal : Form
    {
        public FrmMenuPrincipal()
        {
            InitializeComponent();
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea; //limita a area de maximização para a área de trabalho, evitando sobreposição da barra de tarefas
        }

        private void AtualizarCabecalho(string titulo, string descricao)
        {
            //Definir os textos
            lblTituloAba.Text = titulo.ToUpper();
            lblDescricaoAba.Text = descricao;

            // O 'Left' da descrição será o 'Left' do título + a largura dele + um respiro
            // Isso garante que independente do tamanho do título, a frase vem depois
            int respiro = 15;
            lblDescricaoAba.Left = lblTituloAba.Left + lblTituloAba.Width + respiro;

            // 3. Opcional: Alinha a altura da descrição para ficar centralizada com o título
            lblDescricaoAba.Top = lblTituloAba.Top + (lblTituloAba.Height - lblDescricaoAba.Height) / 2 + 2;
        }

        private void FormatarBotaoAba(Control btn, int raio)
        {
            GraphicsPath gp = new GraphicsPath();
            int w = btn.Width;
            int h = btn.Height;

            gp.AddArc(0, 0, raio, raio, 180, 90); // Canto superior esquerdo
            gp.AddLine(raio, 0, w, 0);            // Topo reto até o fim
            gp.AddLine(w, 0, w, h);               // Lateral direita reta (fusão)
            gp.AddLine(w, h, raio, h);            // Base reta
            gp.AddArc(0, h - raio, raio, raio, 90, 90); // Canto inferior esquerdo
            gp.CloseFigure();

            btn.Region = new Region(gp);
        }

        private void ArredondarTodosCantos(Control controle, int raio)
        {
            GraphicsPath gp = new GraphicsPath();
            int w = controle.Width;
            int h = controle.Height;

            gp.AddArc(0, 0, raio, raio, 180, 90);           // Superior Esquerdo
            gp.AddArc(w - raio, 0, raio, raio, 270, 90);    // Superior Direito
            gp.AddArc(w - raio, h - raio, raio, raio, 0, 90); // Inferior Direito
            gp.AddArc(0, h - raio, raio, raio, 90, 90);     // Inferior Esquerdo

            gp.CloseFigure();
            controle.Region = new Region(gp);
        }

        private void SelecionarBotao(Button botaoAtivo)
        {
            // Cores da paleta
            Color corCremeMenu = Color.FromArgb(242, 233, 216);      // #F2E9D8
            Color corOffWhiteConteudo = Color.FromArgb(248, 248, 248); // #F8F8F8
            Color corVerdeAmbarina = Color.FromArgb(15, 67, 16);     // #0F4310

            // 1. Resetar todos os botões do painel lateral
            foreach (Control ctr in pnlLateral.Controls)
            {
                if (ctr is Button btn)
                {
                    btn.BackColor = corCremeMenu;
                    btn.ForeColor = corVerdeAmbarina; // Texto volta para o verde
                    btn.Font = new Font("Montserrat", 11, FontStyle.Regular);
                }
            }

            // 2. Destacar o botão clicado (Efeito de aba conectada)
            botaoAtivo.BackColor = corOffWhiteConteudo;
            botaoAtivo.ForeColor = corVerdeAmbarina;
            botaoAtivo.Font = new Font("Montserrat", 11, FontStyle.Bold);
        }

        private void AbrirPainel(Panel painelSelecionado)
        {
            // Adicione aqui todos os painéis que você criar
            pnlViewDashboard.Visible = false;
            pnlViewAlmoxarifado.Visible = false;
            pnlViewProducao.Visible = false; 
            pnlViewEstoque.Visible = false;
            pnlViewVendas.Visible = false;
            pnlViewFinanceiro.Visible = false;


            // Mostra o escolhido
            painelSelecionado.Visible = true;
            painelSelecionado.BringToFront();
        }

        private void FrmMenuPrincipal_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;// Inicia o formulário maximizado

            string nomeUsuario = "Isabelle"; //depois integrar com banco de dados ou sistema de autenticação para pegar o nome real do usuário logado
            lblSaudacao.Text = $"Olá, {nomeUsuario}!";

            // Arredonda as abas (raio 25 fica bem orgânico)                                             
            FormatarBotaoAba(btnNavDashboard, 25);
            FormatarBotaoAba(btnNavAlmoxarifado, 25);
            FormatarBotaoAba(btnNavProducao, 25);
            FormatarBotaoAba(btnNavEstoque, 25);
            FormatarBotaoAba(btnNavVendas, 25);
            FormatarBotaoAba(btnNavFinanceiro, 25);

            ArredondarTodosCantos(pnlCardFaturamento, 20);
            ArredondarTodosCantos(pnlCardInsumos, 20);
            ArredondarTodosCantos(pnlCardEstoque, 20);
            ArredondarTodosCantos(pnlCardEntradas, 20);
            ArredondarTodosCantos(pnlCardSaidas, 20);
            ArredondarTodosCantos(pnlCardSaldo, 20);

            AbrirPainel(pnlViewDashboard);

            SelecionarBotao(btnNavDashboard);
            AtualizarCabecalho("DASHBOARD", "Visão geral do desempenho e métricas da Ambarina.");

            // Alinha o CONTEÚDO das células à direita
            dgvAlmoxarifado.Columns["colSaldo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvAlmoxarifado.Columns["colMinimo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Alinha o TEXTO DO CABEÇALHO (Título) à direita também para acompanhar
            dgvAlmoxarifado.Columns["colSaldo"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvAlmoxarifado.Columns["colMinimo"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void lbLoginExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnNavAlmoxarifado_Click(object sender, EventArgs e)
        {
            SelecionarBotao((Button)sender);

            AtualizarCabecalho("ALMOXARIFADO", "Gestão de insumos e matérias-primas (Cera, Essências e Pavios).");

            AbrirPainel(pnlViewAlmoxarifado);
        }

        private void btnNavProducao_Click(object sender, EventArgs e)
        {
            SelecionarBotao((Button)sender);

            AtualizarCabecalho("PRODUÇÃO", "Formulação de velas e registro de fabricação com baixa de estoque.");

            AbrirPainel(pnlViewProducao);
        }

        private void btnNavEstoque_Click(object sender, EventArgs e)
        {
            SelecionarBotao((Button)sender);

            AtualizarCabecalho("ESTOQUE", "Controle de produtos finalizados e prontos para o cliente.");

            AbrirPainel(pnlViewEstoque);
        }

        private void btnNavVendas_Click(object sender, EventArgs e)
        {
            SelecionarBotao((Button)sender);

            AtualizarCabecalho("VENDAS E ORÇAMENTOS", "Elaboração de pedidos e geração de documentos comerciais.");

            AbrirPainel(pnlViewVendas);
        }

        private void btnNavFinanceiro_Click(object sender, EventArgs e)
        {
            SelecionarBotao((Button)sender);

            AtualizarCabecalho("FINANCEIRO", "Fluxo de caixa, faturamento e análise de lucratividade.");

            AbrirPainel(pnlViewFinanceiro);
        }

        private void btnNavDashboard_Click(object sender, EventArgs e)
        {
            SelecionarBotao((Button)sender);

            AtualizarCabecalho("DASHBOARD", "Visão geral do desempenho e métricas da Ambarina.");

            AbrirPainel(pnlViewDashboard);

        }

        private void pnlCardInsumos_MouseEnter(object sender, EventArgs e)
        {
            // Se o mouse entrar na Label, vamos pintar o PARENT (o painel) dela
            Control controle = (Control)sender;
            Panel card;

            if (controle is Panel)
                card = (Panel)controle;
            else
                card = (Panel)controle.Parent; // Pega o painel onde a label está dentro

            card.BackColor = Color.FromArgb(248, 248, 248); // Cor de destaque
        }

        private void pnlCardInsumos_MouseLeave(object sender, EventArgs e)
        {
            Control controle = (Control)sender;
            Panel card;

            if (controle is Panel)
                card = (Panel)controle;
            else
                card = (Panel)controle.Parent;

            card.BackColor = Color.White; // Volta ao branco puro
        }

        private void pnlTabelaVendas_Paint(object sender, PaintEventArgs e)
        {
            // Ao clicar no painel, tiramos o foco da tabela e mandamos para a Label de título
            lblTituloVendasRecentes.Focus();
            dgvVendas.ClearSelection();
        }


        ////ALMOXARIFADO
        private void pnlCadastroInsumo_Click(object sender, EventArgs e)
        {
            dgvAlmoxarifado.ClearSelection(); // Limpa a barra carrossel/caramelo
            this.ActiveControl = null;       // Tira o foco de qualquer campo
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
