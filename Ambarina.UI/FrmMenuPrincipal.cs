using Ambarina.BLL;
using Ambarina.DAL;
using Ambarina.DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ambarina.UI
{
    public partial class FrmMenuPrincipal : Form
    {
        int idInsumoSelecionado = 0;
        int idReceitaSelecionada = 0; // Para controlar se estamos Editando ou Salvando uma nova receita
        int idReceitaParaProducao = 0;
        int idProdutoSelecionado = 0; // Para produtos no Estoque

        public FrmMenuPrincipal()
        {
            InitializeComponent();
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea; //limita a area de maximização para a área de trabalho, evitando sobreposição da barra de tarefas
        }

        private void Utilitarios_Layout() { } // Apenas um marcador visual de organização

        private void AtualizarCabecalho(string titulo, string descricao)
        {
            lblTituloAba.Text = titulo.ToUpper();
            lblDescricaoAba.Text = descricao;

            int respiro = 15;
            lblDescricaoAba.Left = lblTituloAba.Left + lblTituloAba.Width + respiro;
            lblDescricaoAba.Top = lblTituloAba.Top + (lblTituloAba.Height - lblDescricaoAba.Height) / 2 + 2;
        }

        private void FormatarBotaoAba(Control btn, int raio)
        {
            GraphicsPath gp = new GraphicsPath();
            int w = btn.Width;
            int h = btn.Height;

            gp.AddArc(0, 0, raio, raio, 180, 90);
            gp.AddLine(raio, 0, w, 0);
            gp.AddLine(w, 0, w, h);
            gp.AddLine(w, h, raio, h);
            gp.AddArc(0, h - raio, raio, raio, 90, 90);
            gp.CloseFigure();

            btn.Region = new Region(gp);
        }

        private void ArredondarTodosCantos(Control controle, int raio)
        {
            GraphicsPath gp = new GraphicsPath();
            int w = controle.Width;
            int h = controle.Height;

            gp.AddArc(0, 0, raio, raio, 180, 90);
            gp.AddArc(w - raio, 0, raio, raio, 270, 90);
            gp.AddArc(w - raio, h - raio, raio, raio, 0, 90);
            gp.AddArc(0, h - raio, raio, raio, 90, 90);

            gp.CloseFigure();
            controle.Region = new Region(gp);
        }

        private void SelecionarBotao(Button botaoAtivo)
        {
            Color corCremeMenu = Color.FromArgb(242, 233, 216);
            Color corOffWhiteConteudo = Color.FromArgb(248, 248, 248);
            Color corVerdeAmbarina = Color.FromArgb(15, 67, 16);

            foreach (Control ctr in pnlLateral.Controls)
            {
                if (ctr is Button btn)
                {
                    btn.BackColor = corCremeMenu;
                    btn.ForeColor = corVerdeAmbarina;
                    btn.Font = new Font("Montserrat", 11, FontStyle.Regular);
                }
            }

            botaoAtivo.BackColor = corOffWhiteConteudo;
            botaoAtivo.ForeColor = corVerdeAmbarina;
            botaoAtivo.Font = new Font("Montserrat", 11, FontStyle.Bold);
        }

        private void AbrirPainel(Panel painelSelecionado)
        {
            pnlViewDashboard.Visible = false;
            pnlViewAlmoxarifado.Visible = false;
            pnlViewProducao.Visible = false;
            pnlViewEstoque.Visible = false;
            pnlViewVendas.Visible = false;
            pnlViewFinanceiro.Visible = false;

            painelSelecionado.Visible = true;
            painelSelecionado.BringToFront();
        }

        private void FrmMenuPrincipal_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            try
            {
                if (Program.UsuarioLogado != null && !string.IsNullOrEmpty(Program.UsuarioLogado.Nome))
                {
                    lblSaudacao.Text = $"Olá, {Program.UsuarioLogado.Nome}!";
                }
                else
                {
                    lblSaudacao.Text = "Olá, Usuário!";
                }
            }
            catch (InvalidOperationException)
            {
                lblSaudacao.Text = "Olá, Usuário!";
            }

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
            ArredondarTodosCantos(pnlCardProducao, 20);


            AbrirPainel(pnlViewDashboard);
            SelecionarBotao(btnNavDashboard);
            AtualizarCabecalho("DASHBOARD", "Visão geral do desempenho e métricas da Ambarina.");

            dgvAlmoxarifado.Columns["colQtdeAtual"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvAlmoxarifado.Columns["colMinimo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvAlmoxarifado.Columns["colQtdeAtual"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvAlmoxarifado.Columns["colMinimo"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            AtualizarGrid();
            CarregarComboInsumos();

            txtLote.Text = new ProducaoBLL().ObterProximoLote();
            AtualizarGridProducao();
            AtualizarGridProdutos();

            txtUnidadeReceita.ReadOnly = true;
            txtUnidadeReceita.BackColor = Color.FromArgb(240, 240, 240);

            // Vincular os motores de busca instantâneos da Pronta Entrega
            txtBuscarProduto.TextChanged += (s, ev) => ObterDadosEstoqueFiltrados();
            cmbFiltroCategoria.SelectedIndexChanged += (s, ev) => ObterDadosEstoqueFiltrados();
            cmbFiltroStatus.SelectedIndexChanged += (s, ev) => ObterDadosEstoqueFiltrados();
            dgvEstoque.AutoGenerateColumns = false;

            btnLimparFiltros.Click += (s, ev) =>
            {
                txtBuscarProduto.Clear();
                cmbFiltroCategoria.SelectedIndex = 0;
                cmbFiltroStatus.SelectedIndex = 0;
                ObterDadosEstoqueFiltrados();
            };

            // Força a Grid de Pronta Entrega a aceitar a nossa montagem manual linha por linha
            dgvEstoque.AutoGenerateColumns = false;

            CarregarDadosDashboard();

            // Aplica o design das dgvs logo na inicialização
            EstilizarTodasAsGrids();
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
            AtualizarGrid();
        }

        private void btnNavProducao_Click(object sender, EventArgs e)
        {
            SelecionarBotao((Button)sender);
            AtualizarCabecalho("PRODUÇÃO", "Formulação de velas e registro de fabricação com baixa de estoque.");

            try
            {
                cmbInsumo.SelectedIndexChanged -= cmbInsumo_SelectedIndexChanged;

                CarregarComboInsumos();
                CarregarComboProdutos();
                CarregarProdutosBase();
                CarregarComboAromas();
                AtualizarGradeReceitas();
                AtualizarGridProducao();
            }
            finally
            {
                cmbInsumo.SelectedIndexChanged += cmbInsumo_SelectedIndexChanged;
            }

            AbrirPainel(pnlViewProducao);
        }

        private void btnNavEstoque_Click(object sender, EventArgs e)
        {
            SelecionarBotao((Button)sender);
            AtualizarCabecalho("ESTOQUE", "Controle de produtos finalizados e prontos para o cliente.");
            ObterDadosEstoqueFiltrados();
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
            CarregarDadosDashboard();
        }

        private void pnlCardInsumos_MouseEnter(object sender, EventArgs e)
        {
            Control controle = (Control)sender;
            Panel card = (controle is Panel) ? (Panel)controle : (Panel)controle.Parent;
            card.BackColor = Color.FromArgb(248, 248, 248);
        }

        private void pnlCardInsumos_MouseLeave(object sender, EventArgs e)
        {
            Control controle = (Control)sender;
            Panel card = (controle is Panel) ? (Panel)controle : (Panel)controle.Parent;
            card.BackColor = Color.White;
        }

        private void pnlTabelaVendas_Paint(object sender, PaintEventArgs e)
        {
            lblTituloVendasRecentes.Focus();
            dgvVendas.ClearSelection();
        }


        private void EstilizarTodasAsGrids()
        {
            // Lista contendo todas as tabelas mapeadas no seu formulário
            List<DataGridView> grids = new List<DataGridView> {
        dgvVendas, dgvAlmoxarifado, dgvProducao, dgvEstoque, dgvProdutos, dgvListaReceitas, dgvItensReceita
    };

            Color corCremeMenu = Color.FromArgb(242, 233, 216); //
            Color corVerdeAmbarina = Color.FromArgb(15, 67, 16); //

            foreach (var dgv in grids)
            {
                if (dgv == null) continue;

                // 1. Altura das linhas e do cabeçalho para dar o "respiro"
                dgv.RowTemplate.Height = 38;
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dgv.ColumnHeadersHeight = 35;

                // 2. Alinhamento vertical centralizado à esquerda
                dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                // 3. Tipografia elegante: Aumento minimalista da fonte
                dgv.DefaultCellStyle.Font = new Font("Montserrat", 9.5F, FontStyle.Regular);
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Montserrat", 9.5F, FontStyle.Regular);

                // 4. Identidade Visual: Remove o azul genérico do Windows por cores da marca
                dgv.DefaultCellStyle.SelectionBackColor = corCremeMenu;
                dgv.DefaultCellStyle.SelectionForeColor = corVerdeAmbarina;

                // 5. Acabamento Clean
                dgv.RowHeadersVisible = false; // Oculta a primeira coluna cinza vazia
                dgv.BorderStyle = BorderStyle.None;
            }
        }

        // ==========================================
        // SEÇÃO: DASHBOARD (HOME)
        // ==========================================
        DashboardBLL dashboardBLL = new DashboardBLL();

        private void CarregarDadosDashboard()
        {
            try
            {
                int totalQuantidadesProducaoAtiva = dashboardBLL.CarregarTotalEmProducaoAtiva(); //

                // --- 1. CONFIGURAÇÃO E CARREGAMENTO DA GRID ---
                if (dgvVendas != null)
                {
                    dgvVendas.AutoGenerateColumns = false;
                    dgvVendas.Columns.Clear();

                    // O segredo para remover as linhas verticais e deixar apenas as horizontais
                    dgvVendas.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

                    // 1. Coluna Data
                    dgvVendas.Columns.Add(new DataGridViewTextBoxColumn { Name = "DataFabrica", DataPropertyName = "DataFabrica", HeaderText = "DATA", ReadOnly = true });

                    // 2. Coluna Produto
                    dgvVendas.Columns.Add(new DataGridViewTextBoxColumn { Name = "Produto", DataPropertyName = "Produto", HeaderText = "PRODUTO", ReadOnly = true });

                    // 3. Coluna Quantidade
                    dgvVendas.Columns.Add(new DataGridViewTextBoxColumn { Name = "Qtd", DataPropertyName = "Qtd", HeaderText = "QTDE.", ReadOnly = true });

                    // 4. Coluna Lote
                    dgvVendas.Columns.Add(new DataGridViewTextBoxColumn { Name = "Lote", DataPropertyName = "Lote", HeaderText = "LOTE", ReadOnly = true });

                    // 5. Coluna Status (Nossa ComboBox)
                    DataGridViewComboBoxColumn colStatus = new DataGridViewComboBoxColumn();
                    colStatus.Name = "Status";
                    colStatus.DataPropertyName = "Status";
                    colStatus.HeaderText = "Status";
                    colStatus.Items.AddRange("EM CURA", "PRONTA", "EMBALADA");
                    colStatus.FlatStyle = FlatStyle.Flat;
                    colStatus.ReadOnly = false;
                    dgvVendas.Columns.Add(colStatus);

                    // 6. Coluna Oculta (ID da Produção) - Invisível, mas vital para o código!
                    dgvVendas.Columns.Add(new DataGridViewTextBoxColumn { Name = "id_producao", DataPropertyName = "id_producao", Visible = false });

                    // 7. Colunas Ocultas Novas (Produto e Receita)
                    dgvVendas.Columns.Add(new DataGridViewTextBoxColumn { Name = "id_produto", DataPropertyName = "id_produto", Visible = false });
                    dgvVendas.Columns.Add(new DataGridViewTextBoxColumn { Name = "id_receita", DataPropertyName = "id_receita", Visible = false });

                    DataTable dtProducoes = dashboardBLL.CarregarProducoesAtivasGrid();
                    dgvVendas.DataSource = dtProducoes;

                    dgvVendas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvVendas.AllowUserToAddRows = false;
                    dgvVendas.ReadOnly = false;
                    dgvVendas.EditMode = DataGridViewEditMode.EditOnEnter;
                }

                // --- 2. ATUALIZAÇÃO DOS LABELS COM REGRA DE PLURALIZAÇÃO DINÂMICA ---

                // Card Insumos Críticos
                int insumosCriticos = dashboardBLL.CarregarInsumosCriticos();
                string sufixoInsumos = insumosCriticos == 1 ? " ITEM" : " ITENS";
                lblValInsumos.Text = insumosCriticos.ToString() + sufixoInsumos;
                lblValInsumos.ForeColor = insumosCriticos > 0 ? Color.Firebrick : Color.ForestGreen;

                // Card Pronta Entrega (Estoque Geral)
                int totalEstoque = dashboardBLL.CarregarTotalProntaEntrega();
                string sufixoEstoque = totalEstoque == 1 ? " PRODUTO" : " PRODUTOS";
                lblValEstoque.Text = totalEstoque.ToString() + sufixoEstoque;

                // Card Produção Ativa (Soma real das quantidades das unidades em andamento)
                string sufixoProducao = totalQuantidadesProducaoAtiva == 1 ? " PRODUTO" : " PRODUTOS";
                lblValProducao.Text = totalQuantidadesProducaoAtiva.ToString() + sufixoProducao;

                // Card Faturamento
                lblValFaturamento.Text = "R$ 0,00";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados do Dashboard: " + ex.Message, "Aviso do Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void pnlCardInsumos_DoubleClick(object sender, EventArgs e)
        {
            // Passamos o botão real do Almoxarifado como o "sender"! 
            btnNavAlmoxarifado_Click(btnNavAlmoxarifado, null);
        }

        private void pnlCardProducao_DoubleClick(object sender, EventArgs e)
        {
            // Passamos o botão real da Produção como o "sender"!
            btnNavProducao_Click(btnNavProducao, null);
        }

        private void pnlCardEstoque_DoubleClick(object sender, EventArgs e)
        {
            // Passamos o botão real do Estoque como o "sender"!
            btnNavEstoque_Click(btnNavEstoque, null);
        }

        // 1. Força a grid a confirmar a edição no momento exato do clique na ComboBox
        private void dgvVendas_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvVendas.IsCurrentCellDirty && dgvVendas.CurrentCell is DataGridViewComboBoxCell)
            {
                dgvVendas.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        // 2. Deteta a alteração, atualiza a base de dados e recarrega a tela
        private void dgvVendas_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvVendas.Columns[e.ColumnIndex].Name == "Status")
            {
                try
                {
                    // Captura os dados da linha que foi alterada
                    string novoStatus = dgvVendas.Rows[e.RowIndex].Cells["Status"].Value.ToString();
                    int idProducao = Convert.ToInt32(dgvVendas.Rows[e.RowIndex].Cells["id_producao"].Value);
                    int quantidade = Convert.ToInt32(dgvVendas.Rows[e.RowIndex].Cells["Qtd"].Value);
                    int idProduto = Convert.ToInt32(dgvVendas.Rows[e.RowIndex].Cells["id_produto"].Value);
                    int idReceita = Convert.ToInt32(dgvVendas.Rows[e.RowIndex].Cells["id_receita"].Value);

                    // Chama o método correto com os 5 parâmetros exigidos
                    new Ambarina.BLL.ProducaoBLL().AtualizarStatus(idProducao, novoStatus, idProduto, quantidade, idReceita);

                    // Recarrega o Dashboard para sumir com a linha (caso vire EMBALADA) e atualizar os cards
                    CarregarDadosDashboard();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao atualizar o status: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CarregarDadosDashboard(); // Recarrega para voltar o status ao que era antes do erro
                }
            }
        }

        // ==========================================
        // SEÇÃO: ALMOXARIFADO (MATÉRIAS-PRIMAS)
        // ==========================================
        private void pnlCadastroInsumo_Click(object sender, EventArgs e)
        {
            dgvAlmoxarifado.ClearSelection();
            this.ActiveControl = null;
        }

        private void btnSalvarInsumo_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtNomeInsumo.Text) || string.IsNullOrEmpty(txtQtdeInicialInsumo.Text) || string.IsNullOrEmpty(txtQtdInicial.Text))
                {
                    MessageBox.Show("Por favor, preencha o Nome, a Quantidade da Embalagem e o Estoque Atual.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                InsumoDTO novoInsumo = new InsumoDTO();
                novoInsumo.Nome = txtNomeInsumo.Text;
                novoInsumo.Categoria = cmbCategoria.Text;
                novoInsumo.UnidadeMedida = cmbUnidade.Text;
                novoInsumo.EstoqueMinimo = Convert.ToDecimal(txtEstoqueMinimo.Text);

                // CORREÇÃO CRÍTICA DO MAPEAMENTO:
                novoInsumo.CustoInicial = Convert.ToDecimal(txtCustoInicial.Text); // Dinheiro vai para o campo de Custo
                novoInsumo.CustoInicial = Convert.ToDecimal(txtCustoInicial.Text);     // Resguardo de propriedade

                novoInsumo.QtdeInicial = Convert.ToDecimal(txtQtdeInicialInsumo.Text); // Embalagem fixa vai para QuantidadeInicial
                novoInsumo.EstoqueAtual = Convert.ToDecimal(txtQtdInicial.Text);             // Saldo atual vai para EstoqueAtual

                InsumoBLL bll = new InsumoBLL();

                if (idInsumoSelecionado == 0)
                {
                    bll.SalvarInsumo(novoInsumo);
                    MessageBox.Show("Insumo cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    novoInsumo.Id = idInsumoSelecionado;
                    bll.EditarInsumo(novoInsumo);
                    MessageBox.Show("Insumo atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    idInsumoSelecionado = 0;
                    btnSalvarInsumo.Text = "SALVAR INSUMO";
                    pnlViewAlmoxarifado.BackColor = Color.White;
                }

                AtualizarGrid();
                LimparCamposAlmoxarifado();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao Gravar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimparCamposAlmoxarifado()
        {
            txtNomeInsumo.Clear();
            txtQtdInicial.Clear();
            txtQtdeInicialInsumo.Clear();
            txtCustoInicial.Clear();
            txtEstoqueMinimo.Clear();
            cmbCategoria.SelectedIndex = -1;
            cmbUnidade.SelectedIndex = -1;
            txtNomeInsumo.Focus();
        }

        private void dgvAlmoxarifado_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int id = Convert.ToInt32(dgvAlmoxarifado.Rows[e.RowIndex].Cells["colID"].Value);

            if (dgvAlmoxarifado.Columns[e.ColumnIndex].Name == "colExcluirAlmox")
            {
                if (MessageBox.Show("Deseja excluir este item?", "Ambarina", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    new InsumoBLL().ExcluirInsumo(id);
                    AtualizarGrid();
                }
            }

            if (dgvAlmoxarifado.Columns[e.ColumnIndex].Name == "colEditarAlmox")
            {
                pnlViewAlmoxarifado.BackColor = Color.FromArgb(255, 252, 240);
                idInsumoSelecionado = id;

                txtNomeInsumo.Text = dgvAlmoxarifado.Rows[e.RowIndex].Cells["colNome"].Value?.ToString() ?? "";
                cmbCategoria.Text = dgvAlmoxarifado.Rows[e.RowIndex].Cells["colCategoria"].Value?.ToString() ?? "";
                cmbUnidade.Text = dgvAlmoxarifado.Rows[e.RowIndex].Cells["colUnDeMedida"].Value?.ToString() ?? "";

                //Garante que cada valor volta de forma correta da Grid para a caixa certa
                txtQtdeInicialInsumo.Text = dgvAlmoxarifado.Rows[e.RowIndex].Cells["colQtdeInicial"].Value?.ToString() ?? "0"; // Qtd Embalagem
                txtQtdInicial.Text = dgvAlmoxarifado.Rows[e.RowIndex].Cells["colQtdeAtual"].Value?.ToString() ?? "0";        // Estoque Atual

                txtCustoInicial.Text = dgvAlmoxarifado.Rows[e.RowIndex].Cells["colCustoTotalInsumo"].Value?.ToString() ?? "0";
                txtEstoqueMinimo.Text = dgvAlmoxarifado.Rows[e.RowIndex].Cells["colMinimo"].Value?.ToString() ?? "0";

                btnSalvarInsumo.Text = "ATUALIZAR INSUMO";
                txtNomeInsumo.Focus();
            }
        }

        private void AtualizarGrid()
        {
            try
            {
                dgvAlmoxarifado.AutoGenerateColumns = false;

                dgvAlmoxarifado.Columns["colID"].DataPropertyName = "ID";
                dgvAlmoxarifado.Columns["colNome"].DataPropertyName = "Nome";
                dgvAlmoxarifado.Columns["colCategoria"].DataPropertyName = "Categoria";
                dgvAlmoxarifado.Columns["colUnDeMedida"].DataPropertyName = "Unidade";

                if (dgvAlmoxarifado.Columns.Contains("colQtdeInicial"))
                    dgvAlmoxarifado.Columns["colQtdeInicial"].DataPropertyName = "Qtd Embalagem";

                dgvAlmoxarifado.Columns["colQtdeAtual"].DataPropertyName = "Estoque Atual";

                // CORREÇÃO DOS MAPEAMENTOS DA GRID:
                // Se a sua coluna de custo unitário na Grid se chamar colCustoUnit, aponte para "Custo Unit"
                if (dgvAlmoxarifado.Columns.Contains("colCustoUnit"))
                    dgvAlmoxarifado.Columns["colCustoUnit"].DataPropertyName = "Custo Unit";

                // colCustoTotalInsumo vai guardar o valor bruto de fábrica nos bastidores da linha
                dgvAlmoxarifado.Columns["colCustoTotalInsumo"].DataPropertyName = "Custo Total";

                // Mapeia a coluna de custo Total restante
                if (dgvAlmoxarifado.Columns.Contains("colTotal"))
                    dgvAlmoxarifado.Columns["colTotal"].DataPropertyName = "Total";

                dgvAlmoxarifado.Columns["colMinimo"].DataPropertyName = "Mínimo";

                InsumoBLL bll = new InsumoBLL();
                dgvAlmoxarifado.DataSource = bll.ListarInsumos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar a lista: " + ex.Message);
            }
        }

        // ==========================================
        // SEÇÃO: REGISTRO DE PRODUÇÃO
        // ==========================================
        private void CarregarComboInsumos()
        {
            try
            {
                InsumoBLL bll = new InsumoBLL();
                cmbInsumo.DataSource = bll.ListarInsumosCombo();
                cmbInsumo.DisplayMember = "nome";
                cmbInsumo.ValueMember = "id_insumo";
                cmbInsumo.SelectedIndex = -1;
            }
            catch (Exception ex) { MessageBox.Show("Erro ao carregar insumos: " + ex.Message); }
        }

        private void CarregarComboProdutos()
        {
            try
            {
                ProdutoBLL bll = new ProdutoBLL();
                cmbProduto.DataSource = bll.ListarProdutosCombo();
                cmbProduto.DisplayMember = "nome";
                cmbProduto.ValueMember = "id_produto";
                cmbProduto.SelectedIndex = -1;
            }
            catch (Exception ex) { MessageBox.Show("Erro ao carregar produtos: " + ex.Message); }
        }

        private void ExecutarProducao()
        {
            try
            {
                if (cmbProduto.SelectedValue == null)
                {
                    MessageBox.Show("Selecione um produto antes de finalizar a produção.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbAroma.SelectedItem == null || string.IsNullOrEmpty(cmbAroma.Text))
                {
                    MessageBox.Show("Selecione o aroma planejado para este lote.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idProd = Convert.ToInt32(cmbProduto.SelectedValue);
                string aromaSelecionado = cmbAroma.Text;

                // INTELIGÊNCIA COMBOBOX: Se o usuário não clicou na grade, buscamos o ID dinamicamente pelo banco
                if (idReceitaParaProducao == 0)
                {
                    idReceitaParaProducao = new ReceitaBLL().ObterIdReceitaPorProdutoEAroma(idProd, aromaSelecionado);
                }

                // Se mesmo buscando no banco continuar 0, significa que aquela combinação de produto+aroma de fato não tem receita
                if (idReceitaParaProducao == 0)
                {
                    MessageBox.Show($"Não existe nenhuma receita cadastrada para o produto selecionado com o aroma '{aromaSelecionado}'.\nPor favor, cadastre a receita antes de registrar a produção.", "Receita Não Encontrada", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (!int.TryParse(txtQtdeProduzida.Text, out int qtdProduzida) || qtdProduzida <= 0)
                {
                    MessageBox.Show("Informe uma quantidade produzida válida e maior que zero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ProducaoDTO novaProducao = new ProducaoDTO();
                novaProducao.IdProduto = idProd;
                novaProducao.DataProducao = dtpData.Value;
                novaProducao.QtdeProduzida = qtdProduzida; // Alterado para bater com a propriedade correta da DTO
                novaProducao.Lote = txtLote.Text;
                novaProducao.Status = "EM CURA";

                ProducaoBLL producaoBll = new ProducaoBLL();
                producaoBll.ProcessarProducaoCompleta(novaProducao, idReceitaParaProducao);

                MessageBox.Show($"Produção do lote {novaProducao.Lote} registrada com sucesso!\nEstoques de insumos e pronta entrega atualizados.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtQtdeProduzida.Clear();
                cmbProduto.SelectedIndex = -1;
                cmbAroma.SelectedIndex = -1;
                idReceitaParaProducao = 0; // Reseta para a próxima rodada

                AtualizarGrid();
                ObterDadosEstoqueFiltrados();
                AtualizarGridProducao();

                txtLote.Text = producaoBll.ObterProximoLote();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro no Processamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFinalizarProducao_Click(object sender, EventArgs e)
        {
            ExecutarProducao();
        }

        public void AtualizarGridProducao()
        {
            try
            {
                // Impede a criação de colunas duplicadas automaticamente
                dgvProducao.AutoGenerateColumns = false;

                // Mapeamento exato entre a Coluna do Designer (esquerda) e o Campo do Banco/SQL (direita)
                if (dgvProducao.Columns.Contains("dataGridViewTextBoxColumnColID"))
                    dgvProducao.Columns["dataGridViewTextBoxColumnColID"].DataPropertyName = "id_producao";

                if (dgvProducao.Columns.Contains("dataGridViewTextBoxColData"))
                    dgvProducao.Columns["dataGridViewTextBoxColData"].DataPropertyName = "data_producao";

                if (dgvProducao.Columns.Contains("dataGridViewTextBoxColProduto"))
                    dgvProducao.Columns["dataGridViewTextBoxColProduto"].DataPropertyName = "nome_produto";

                if (dgvProducao.Columns.Contains("ColAroma"))
                    dgvProducao.Columns["ColAroma"].DataPropertyName = "aroma_padrao";

                if (dgvProducao.Columns.Contains("ColQtdeProduzida"))
                    dgvProducao.Columns["ColQtdeProduzida"].DataPropertyName = "qtde_produzida";

                if (dgvProducao.Columns.Contains("dataGridViewTextBoxColLote"))
                    dgvProducao.Columns["dataGridViewTextBoxColLote"].DataPropertyName = "lote";

                if (dgvProducao.Columns.Contains("dataGridViewTextBoxColStatus"))
                    dgvProducao.Columns["dataGridViewTextBoxColStatus"].DataPropertyName = "status";

                if (dgvProducao.Columns.Contains("id_produto"))
                    dgvProducao.Columns["id_produto"].DataPropertyName = "id_produto";

                // Carrega os dados da camada de negócio
                ProducaoBLL bll = new ProducaoBLL();
                dgvProducao.DataSource = bll.ListarProducoes();

                // Força o DataBinding para garantir que as linhas já estão renderizadas
                dgvProducao.DataBindingComplete += dgvProducao_DataBindingComplete;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar grid de produção: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Novo evento para colorir e travar as linhas após a renderização da grid
        private void dgvProducao_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvProducao.Rows)
            {
                if (row.Cells["dataGridViewTextBoxColStatus"].Value != null &&
                    row.Cells["dataGridViewTextBoxColStatus"].Value.ToString().ToUpper() == "EMBALADA")
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(230, 230, 230);
                    row.DefaultCellStyle.ForeColor = Color.Gray;

                    // Trava todas as células da linha
                    row.ReadOnly = true;

                    // Adicional: Se tiver combo box na coluna, também desativa
                    if (dgvProducao.Columns.Contains("dataGridViewTextBoxColStatus") && row.Cells["dataGridViewTextBoxColStatus"] is DataGridViewComboBoxCell comboCell)
                    {
                        comboCell.ReadOnly = true;
                    }
                }
            }
        }

        private void dgvProducao_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // Força o WinForms a comitar a alteração no exato momento que o usuário escolhe no ComboBox
            if (dgvProducao.IsCurrentCellDirty)
            {
                dgvProducao.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void dgvProducao_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvProducao.Columns[e.ColumnIndex].Name == "dataGridViewTextBoxColStatus")
            {
                try
                {
                    DataGridViewRow row = dgvProducao.Rows[e.RowIndex];

                    // Pegando o ID da Produção exatamente pela coluna correta
                    int idProducao = Convert.ToInt32(row.Cells["dataGridViewTextBoxColumnColID"].Value);

                    // Resgatando o id_produto direto da fonte de dados
                    DataRowView dataRowView = (DataRowView)row.DataBoundItem;
                    int idProd = Convert.ToInt32(dataRowView["id_produto"]);

                    string statusNovo = row.Cells["dataGridViewTextBoxColStatus"].Value.ToString();
                    int quantidade = Convert.ToInt32(row.Cells["ColQtdeProduzida"].Value);
                    string aromaNome = row.Cells["ColAroma"].Value.ToString();

                    int idReceita = new ReceitaBLL().ObterIdReceitaPorProdutoEAroma(idProd, aromaNome);

                    new ProducaoBLL().AtualizarStatus(idProducao, statusNovo, idProd, quantidade, idReceita);

                    MessageBox.Show($"Lote atualizado para {statusNovo} com sucesso!", "Status Atualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ObterDadosEstoqueFiltrados();
                    AtualizarGridProducao();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro ao Mudar Status", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvProducao_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvProducao.Columns[e.ColumnIndex].Name == "colRemoverProd")
            {
                DataGridViewRow row = dgvProducao.Rows[e.RowIndex];

                // Pegando o ID da Produção exatamente pela coluna correta
                int idProducao = Convert.ToInt32(row.Cells["dataGridViewTextBoxColumnColID"].Value);

                string lote = row.Cells["dataGridViewTextBoxColLote"].Value.ToString();
                string status = row.Cells["dataGridViewTextBoxColStatus"].Value.ToString();
                string aromaNome = row.Cells["ColAroma"].Value.ToString();
                int qtdProduzida = Convert.ToInt32(row.Cells["ColQtdeProduzida"].Value);

                // Resgatando o id_produto direto da fonte de dados (sem precisar de coluna física)
                DataRowView dataRowView = (DataRowView)row.DataBoundItem;
                int idProduto = Convert.ToInt32(dataRowView["id_produto"]);

                DialogResult confirmacao = MessageBox.Show(
                    $"Deseja realmente excluir o registro do lote {lote}?\n\nEsta ação devolverá todos os insumos de volta ao Almoxarifado.",
                    "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirmacao == DialogResult.Yes)
                {
                    try
                    {
                        int idReceita = new ReceitaBLL().ObterIdReceitaPorProdutoEAroma(idProduto, aromaNome);

                        ProducaoBLL bll = new ProducaoBLL();
                        bll.ProcessarExclusaoComEstorno(idProducao, idProduto, qtdProduzida, status, idReceita);

                        MessageBox.Show($"Lote {lote} excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        AtualizarGridProducao();
                        txtLote.Text = bll.ObterProximoLote();
                    }
                    catch (Exception ex)
                    {
                        // Ícone ajustado para Warning para evitar crash visual no WinForms
                        MessageBox.Show(ex.Message, "Aviso de Segurança", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        // ==========================================
        // SEÇÃO: CONFIGURAÇÃO DE RECEITAS
        // ==========================================
        private void CarregarProdutosBase()
        {
            try
            {
                ProdutoBLL bll = new ProdutoBLL();
                cmbProdutoBase.DataSource = bll.ListarProdutosCombo();
                cmbProdutoBase.DisplayMember = "nome";
                cmbProdutoBase.ValueMember = "id_produto";
                cmbProdutoBase.SelectedIndex = -1;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void CarregarComboAromas()
        {
            try
            {
                ReceitaBLL bll = new ReceitaBLL();
                cmbAroma.DataSource = bll.ListarAromas();
                cmbAroma.DisplayMember = "aroma_padrao";
                cmbAroma.SelectedIndex = -1;
            }
            catch (Exception ex) { MessageBox.Show("Erro ao carregar aromas: " + ex.Message); }
        }

        private void cmbInsumo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbInsumo.SelectedIndex != -1 && cmbInsumo.SelectedValue != null && !string.IsNullOrEmpty(cmbInsumo.SelectedValue.ToString()))
            {
                if (cmbInsumo.SelectedValue.ToString() == "System.Data.DataRowView") return;

                try
                {
                    if (int.TryParse(cmbInsumo.SelectedValue.ToString(), out int idInsumo))
                    {
                        InsumoBLL bll = new InsumoBLL();
                        txtUnidadeReceita.Text = bll.ObterUnidadeMedidaInsumo(idInsumo);
                    }
                }
                catch (Exception ex) { MessageBox.Show("Erro ao atualizar unidade: " + ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
        }

        private void btnAdicionarInsumo_Click(object sender, EventArgs e)
        {
            if (cmbInsumo.SelectedIndex != -1 && !string.IsNullOrEmpty(txtQtdInsumo.Text))
            {
                if (dgvItensReceita.DataSource != null)
                {
                    dgvItensReceita.DataSource = null;
                    dgvItensReceita.Rows.Clear();
                }

                int n = dgvItensReceita.Rows.Add();
                dgvItensReceita.Rows[n].Cells["colInsumo"].Value = cmbInsumo.Text;
                dgvItensReceita.Rows[n].Cells["colQtd"].Value = txtQtdInsumo.Text;
                dgvItensReceita.Rows[n].Cells["colUnidade"].Value = txtUnidadeReceita.Text;

                cmbInsumo.SelectedIndex = -1;
                txtQtdInsumo.Clear();
                txtUnidadeReceita.Clear();
                cmbInsumo.Focus();
            }
        }

        private void AtualizarGradeReceitas()
        {
            try
            {
                ReceitaBLL bll = new ReceitaBLL();
                dgvListaReceitas.DataSource = bll.ListarReceitas();

                // Garante que as colunas de ID fiquem totalmente ocultas para o usuário
                if (dgvListaReceitas.Columns.Contains("id_receita"))
                    dgvListaReceitas.Columns["id_receita"].Visible = false;

                if (dgvListaReceitas.Columns.Contains("id_produto"))
                    dgvListaReceitas.Columns["id_produto"].Visible = false;
            }
            catch (Exception ex) { MessageBox.Show("Erro ao carregar lista de receitas: " + ex.Message); }
        }

        private void dgvListaReceitas_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Força a ocultação caso o WinForms tente recriar as colunas após o bind
            if (dgvListaReceitas.Columns.Contains("id_produto")) dgvListaReceitas.Columns["id_produto"].Visible = false;
            if (dgvListaReceitas.Columns.Contains("id_receita")) dgvListaReceitas.Columns["id_receita"].Visible = false;
        }

        private void CarregarInsumosDaReceita(int idReceita)
        {
            try
            {
                dgvItensReceita.AutoGenerateColumns = false;
                ReceitaBLL bll = new ReceitaBLL();
                dgvItensReceita.DataSource = bll.ListarItensDaReceita(idReceita);

                if (dgvItensReceita.Columns.Contains("id_insumo"))
                    dgvItensReceita.Columns["id_insumo"].Visible = false;
            }
            catch (Exception ex) { MessageBox.Show("Erro ao carregar insumos: " + ex.Message); }
        }

        private void dgvListaReceitas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvListaReceitas.Columns[e.ColumnIndex].Name == "colEditarReceita" || dgvListaReceitas.Columns[e.ColumnIndex].Name == "colExcluirReceita") return;

            // Apenas capturamos o ID da receita clicada para efeitos de visualização
            int idReceitaClicada = Convert.ToInt32(dgvListaReceitas.Rows[e.RowIndex].Cells["id_receita"].Value);

            // Carrega a sub-grelha de insumos no ecrã apenas para consulta da fórmula
            CarregarInsumosDaReceita(idReceitaClicada);

            // ❌ REMOVIDO: A atribuição automática ao cmbProduto, cmbAroma e txtQtdeProduzida.
            // ❌ REMOVIDO: A alteração da variável global 'idReceitaParaProducao'.

            // Agora o utilizador tem controlo total e manual sobre o que vai produzir!
        }

        private void dgvListaReceitas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int id = Convert.ToInt32(dgvListaReceitas.Rows[e.RowIndex].Cells["id_receita"].Value);

            if (dgvListaReceitas.Columns[e.ColumnIndex].Name == "ColBtnExcluirReceita")
            {
                if (MessageBox.Show("Deseja excluir permanentemente esta receita?", "Excluir", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    try
                    {
                        new ReceitaBLL().ExcluirReceita(id);
                        MessageBox.Show("Receita excluída!");
                        AtualizarGradeReceitas();
                        LimparCamposReceita();
                    }
                    catch (Exception ex) { MessageBox.Show("Erro ao excluir: " + ex.Message); }
                }
            }

            if (dgvListaReceitas.Columns[e.ColumnIndex].Name == "ColBtnEditarReceita")
            {
                idReceitaSelecionada = id;
                cmbProdutoBase.Text = dgvListaReceitas.Rows[e.RowIndex].Cells["Produto"].Value.ToString();
                txtAroma.Text = dgvListaReceitas.Rows[e.RowIndex].Cells["Aroma"].Value.ToString();

                dgvItensReceita.DataSource = null;
                dgvItensReceita.Rows.Clear();

                ReceitaBLL bll = new ReceitaBLL();
                DataTable dtItens = bll.ListarItensDaReceita(idReceitaSelecionada);

                if (dtItens != null && dtItens.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtItens.Rows)
                    {
                        int rowIndex = dgvItensReceita.Rows.Add();
                        dgvItensReceita.Rows[rowIndex].Cells["colInsumo"].Value = dr["Insumo"]?.ToString() ?? "";
                        dgvItensReceita.Rows[rowIndex].Cells["colQtd"].Value = dr["Qtd"]?.ToString() ?? "0";
                        dgvItensReceita.Rows[rowIndex].Cells["colUnidade"].Value = dr["Unid"]?.ToString() ?? "";
                    }
                }

                btnSalvarReceitaCompleta.Text = "ATUALIZAR RECEITA";
                pnlCardReceita.BackColor = Color.FromArgb(255, 252, 240);
            }
        }

        private void dgvItensReceita_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvItensReceita.Rows[e.RowIndex].Cells["colInsumo"].Value == null) return;

            string nomeColExcluir1 = "colBtnExcluirItensReceita";
            string nomeColExcluir2 = "colExcluirItensReceita";
            string nomeColEditar = "ColBtnEditarItensReceita";

            if (dgvItensReceita.Columns[e.ColumnIndex].Name == nomeColExcluir1 || dgvItensReceita.Columns[e.ColumnIndex].Name == nomeColExcluir2)
            {
                if (MessageBox.Show("Deseja remover este insumo?", "Remover", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                if (dgvItensReceita.DataSource != null)
                {
                    var dt = dgvItensReceita.DataSource as DataTable;
                    dgvItensReceita.DataSource = null;
                    dgvItensReceita.Rows.Clear();

                    if (dt != null)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            int idx = dgvItensReceita.Rows.Add();
                            dgvItensReceita.Rows[idx].Cells["colInsumo"].Value = dr["Insumo"]?.ToString() ?? "";
                            dgvItensReceita.Rows[idx].Cells["colQtd"].Value = dr["Qtd"]?.ToString() ?? "0";
                            dgvItensReceita.Rows[idx].Cells["colUnidade"].Value = dr["Unid"]?.ToString() ?? "";
                        }
                    }
                }

                dgvItensReceita.Rows.RemoveAt(e.RowIndex);
                return;
            }

            if (dgvItensReceita.Columns[e.ColumnIndex].Name == nomeColEditar)
            {
                cmbInsumo.Text = dgvItensReceita.Rows[e.RowIndex].Cells["colInsumo"].Value.ToString();
                txtQtdInsumo.Text = dgvItensReceita.Rows[e.RowIndex].Cells["colQtd"].Value.ToString();
                txtUnidadeReceita.Text = dgvItensReceita.Rows[e.RowIndex].Cells["colUnidade"].Value?.ToString() ?? "";

                dgvItensReceita.Rows.RemoveAt(e.RowIndex);
                txtQtdInsumo.Focus();
            }
        }

        private void btnSalvarReceitaCompleta_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbProdutoBase.SelectedIndex == -1 || string.IsNullOrEmpty(txtAroma.Text))
                {
                    MessageBox.Show("Selecione um Produto Base e digite o Aroma!", "Ambarina");
                    return;
                }

                ReceitaDTO receita = new ReceitaDTO();
                receita.Id = idReceitaSelecionada;
                receita.IdProduto = Convert.ToInt32(cmbProdutoBase.SelectedValue);
                receita.AromaPadrao = txtAroma.Text;

                var nomeCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                foreach (DataGridViewRow row in dgvItensReceita.Rows)
                {
                    if (!row.IsNewRow && row.Cells["colInsumo"].Value != null)
                    {
                        string nome = row.Cells["colInsumo"].Value.ToString().Trim();
                        if (string.IsNullOrEmpty(nome)) continue;
                        if (nomeCounts.ContainsKey(nome)) nomeCounts[nome]++;
                        else nomeCounts[nome] = 1;
                    }
                }

                var duplicados = nomeCounts.Where(kvp => kvp.Value > 1).ToList();
                if (duplicados.Any())
                {
                    MessageBox.Show("A receita contém insumos duplicados! Agrupe as quantidades.", "Insumo duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                List<ItensReceitaDTO> listaItens = new List<ItensReceitaDTO>();
                foreach (DataGridViewRow row in dgvItensReceita.Rows)
                {
                    if (!row.IsNewRow && row.Cells["colInsumo"].Value != null)
                    {
                        listaItens.Add(new ItensReceitaDTO
                        {
                            NomeInsumo = row.Cells["colInsumo"].Value.ToString(),
                            Quantidade = Convert.ToDecimal(row.Cells["colQtd"].Value)
                        });
                    }
                }

                if (listaItens.Count == 0)
                {
                    MessageBox.Show("Adicione pelo menos um insumo válido!", "Ambarina");
                    return;
                }

                ReceitaBLL bll = new ReceitaBLL();

                if (idReceitaSelecionada == 0)
                {
                    bll.SalvarReceitaCompleta(receita, listaItens);
                    MessageBox.Show($"Receita de {txtAroma.Text} salva com sucesso!", "Ambarina", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    bll.EditarReceitaCompleta(receita, listaItens);
                    MessageBox.Show($"Receita de {txtAroma.Text} atualizada com sucesso!", "Ambarina", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                idReceitaSelecionada = 0;
                btnSalvarReceitaCompleta.Text = "SALVAR RECEITA";
                pnlCardReceita.BackColor = Color.White;

                AtualizarGradeReceitas();
                CarregarComboAromas();
                LimparCamposReceita();
            }
            catch (Exception ex) { MessageBox.Show("Erro ao processar receita: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void LimparCamposReceita()
        {
            txtAroma.Clear();
            txtQtdInsumo.Clear();
            dgvItensReceita.DataSource = null;
            dgvItensReceita.Rows.Clear();
            cmbProdutoBase.SelectedIndex = -1;
            cmbInsumo.SelectedIndex = -1;
            idReceitaSelecionada = 0;
            btnSalvarReceitaCompleta.Text = "SALVAR RECEITA";
            pnlCardReceita.BackColor = Color.White;
            cmbProdutoBase.Focus();
        }

        // ==========================================
        // SEÇÃO: PRONTA ENTREGA (GERENCIAR ESTOQUE)
        // ==========================================
        private void ObterDadosEstoqueFiltrados()
        {
            try
            {
                string busca = txtBuscarProduto.Text;
                string categoria = cmbFiltroCategoria.SelectedItem?.ToString() ?? "TODAS";
                string status = cmbFiltroStatus.SelectedItem?.ToString() ?? "TODOS";

                ProdutoBLL bll = new ProdutoBLL();
                DataTable dt = bll.FiltrarEstoqueProntaEntrega(busca, categoria, status);

                dgvEstoque.Rows.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    int rowIndex = dgvEstoque.Rows.Add();
                    DataGridViewRow gridRow = dgvEstoque.Rows[rowIndex];

                    // CORREÇÃO DE IDENTIFICAÇÃO: Usamos o id_receita para gerar o SKU único por aroma!
                    int idReceita = row["id_receita"] != DBNull.Value ? Convert.ToInt32(row["id_receita"]) : 0;
                    string sku = "PRD-" + idReceita.ToString("D3");

                    gridRow.Cells["ColIDProntaEntrega"].Value = Convert.ToInt32(row["id_produto"]);
                    gridRow.Cells["dataGridViewTextBoxColCod"].Value = sku;

                    // CORREÇÃO DO NOME: row["nome_produto"] já traz o formato "Nome (Aroma)" direto do banco!
                    gridRow.Cells["dataGridViewTextBoxColProd"].Value = row["nome_produto"].ToString();
                    gridRow.Cells["dataGridViewTextBoxColCategoria"].Value = row["categoria"].ToString();
                    gridRow.Cells["dataGridViewTextBoxColQtdeDisponivel"].Value = row["estoque_atual"];

                    decimal precoVenda = Convert.ToDecimal(row["preco_venda_sugerido"]);
                    gridRow.Cells["dataGridViewTextBoxColPrecoUnitario"].Value = precoVenda.ToString("C2");

                    gridRow.Cells["colEditarProduto"].Value = "✎";
                    gridRow.Cells["colExcluirProduto"].Value = "X";

                    gridRow.Tag = idReceita;

                    int estoqueAtual = Convert.ToInt32(row["estoque_atual"]);
                    int estoqueMin = Convert.ToInt32(row["estoque_minimo"]);

                    if (estoqueAtual <= estoqueMin && estoqueAtual > 0)
                    {
                        gridRow.Cells["dataGridViewTextBoxColQtdeDisponivel"].Style.ForeColor = Color.DarkOrange;
                        gridRow.Cells["dataGridViewTextBoxColQtdeDisponivel"].Style.Font = new Font("Montserrat", 9, FontStyle.Bold);
                    }
                    else if (estoqueAtual == 0)
                    {
                        gridRow.Cells["dataGridViewTextBoxColQtdeDisponivel"].Style.ForeColor = Color.Red;
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Erro ao atualizar estoque: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void dgvEstoque_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int idProduto = Convert.ToInt32(dgvEstoque.Rows[e.RowIndex].Cells["ColIDProntaEntrega"].Value);
            string nomeProduto = dgvEstoque.Rows[e.RowIndex].Cells["dataGridViewTextBoxColProd"].Value.ToString();

            if (dgvEstoque.Columns[e.ColumnIndex].Name == "colEditarProduto")
            {
                dgvEstoque.ReadOnly = false;
                foreach (DataGridViewColumn col in dgvEstoque.Columns)
                {
                    col.ReadOnly = (col.Name != "dataGridViewTextBoxColQtdeDisponivel");
                }

                dgvEstoque.CurrentCell = dgvEstoque.Rows[e.RowIndex].Cells["dataGridViewTextBoxColQtdeDisponivel"];
                dgvEstoque.BeginEdit(true);
            }

            if (dgvEstoque.Columns[e.ColumnIndex].Name == "colExcluirProduto")
            {
                DialogResult confirmacao = MessageBox.Show(
                    $"Deseja realmente excluir permanentemente o produto '{nomeProduto}' do catálogo de Pronta Entrega?\n\nEsta ação removerá o registro completamente.",
                    "Confirmar Exclusão Total",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (confirmacao == DialogResult.Yes)
                {
                    try
                    {
                        ProdutoBLL bll = new ProdutoBLL();
                        bll.ExcluirProduto(idProduto);

                        MessageBox.Show("Produto removido com sucesso do catálogo!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ObterDadosEstoqueFiltrados();
                        CarregarProdutosBase();
                    }
                    catch (Exception ex)
                    {
                        // Captura amigável: se houver receitas vinculadas, explica o motivo em vez de quebrar a aplicação
                        if (ex.Message.Contains("1451") || (ex.InnerException != null && ex.InnerException.Message.Contains("1451")))
                        {
                            MessageBox.Show(
                                $"Não é possível excluir o produto '{nomeProduto}' porque ele está vinculado a uma receita existente no sistema.\n\nSugestão: Se você não trabalha mais com este aroma, use a edição rápida para zerar o estoque disponível.",
                                "Aviso de Segurança de Dados",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );
                        }
                        else
                        {
                            MessageBox.Show("Erro ao excluir produto: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void dgvEstoque_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvEstoque.Columns[e.ColumnIndex].Name == "dataGridViewTextBoxColQtdeDisponivel")
            {
                DataGridViewRow row = dgvEstoque.Rows[e.RowIndex];
                int idProduto = Convert.ToInt32(row.Cells["ColIDProntaEntrega"].Value);

                // PASSO NOVO: Captura o ID da Receita da linha para saber qual aroma estamos editando
                int idReceita = Convert.ToInt32(row.Tag);

                string valorDigitado = row.Cells["dataGridViewTextBoxColQtdeDisponivel"].Value?.ToString() ?? "0";

                try
                {
                    if (int.TryParse(valorDigitado, out int novaQtd) && novaQtd >= 0)
                    {
                        ProdutoBLL bll = new ProdutoBLL();

                        // CORREÇÃO: Enviamos os 3 parâmetros exigidos pela BLL e DAL
                        bll.AjustarQuantidadeFisica(idProduto, idReceita, novaQtd);

                        dgvEstoque.ReadOnly = true;
                        ObterDadosEstoqueFiltrados();
                    }
                    else
                    {
                        MessageBox.Show("Quantidade inválida.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ObterDadosEstoqueFiltrados();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao salvar estoque: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ObterDadosEstoqueFiltrados();
                }
            }
        }

        // ==========================================
        // SEÇÃO: CATÁLOGO BASE DE MODELOS (EXISTENTE)
        // ==========================================
        private void AtualizarGridProdutos()
        {
            try
            {
                // Como o AutoGenerateColumns está false, precisamos dizer qual propriedade mapeia qual coluna
                dgvProdutos.AutoGenerateColumns = false;

                dgvProdutos.Columns["colIDProduto"].DataPropertyName = "Id";
                dgvProdutos.Columns["colNomeProduto"].DataPropertyName = "Nome";
                dgvProdutos.Columns["colCategoriaProduto"].DataPropertyName = "Categoria";
                dgvProdutos.Columns["colMargemLucro"].DataPropertyName = "MargemLucro";
                dgvProdutos.Columns["colEstoqueMinimo"].DataPropertyName = "EstoqueMinimo";

                ProdutoBLL bll = new ProdutoBLL();
                dgvProdutos.DataSource = bll.ListarProdutos();
            }
            catch (Exception ex) { MessageBox.Show("Erro ao carregar a lista de produtos: " + ex.Message); }
        }

        private void LimparCamposCadastroProduto()
        {
            txtNomeProduto.Clear();
            cmbCategoriaProduto.SelectedIndex = -1;
            txtMargemLucro.Clear();
            txtEstoqueMin.Clear();
            idProdutoSelecionado = 0;
            btnSalvarProduto.Text = "CADASTRAR PRODUTO";
            pnlFormCadastroProduto.BackColor = Color.White;
            txtNomeProduto.Focus();
        }

        private void btnSalvarProduto_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtNomeProduto.Text) || cmbCategoriaProduto.SelectedIndex == -1)
                {
                    MessageBox.Show("Preencha o nome e a categoria!", "Validação");
                    return;
                }

                ProdutoDTO novoModelo = new ProdutoDTO();
                novoModelo.Nome = txtNomeProduto.Text;
                novoModelo.Categoria = cmbCategoriaProduto.Text;
                novoModelo.MargemLucro = string.IsNullOrEmpty(txtMargemLucro.Text) ? 0 : Convert.ToDecimal(txtMargemLucro.Text);
                novoModelo.EstoqueMinimo = string.IsNullOrEmpty(txtEstoqueMin.Text) ? 0 : Convert.ToInt32(txtEstoqueMin.Text);

                ProdutoBLL bll = new ProdutoBLL();

                if (idProdutoSelecionado == 0)
                {
                    bll.SalvarProduto(novoModelo);
                    MessageBox.Show("Produto cadastrado com sucesso!", "Ambarina", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    novoModelo.Id = idProdutoSelecionado;
                    bll.EditarProduto(novoModelo);
                    MessageBox.Show("Produto atualizado com sucesso!", "Ambarina", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                AtualizarGridProdutos();
                CarregarProdutosBase();
                LimparCamposCadastroProduto();
            }
            catch (Exception ex) { MessageBox.Show("Erro ao salvar produto: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void dgvProdutos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int id = Convert.ToInt32(dgvProdutos.Rows[e.RowIndex].Cells["colIDProduto"].Value);

            if (dgvProdutos.Columns[e.ColumnIndex].Name == "colExcluirProd")
            {
                if (MessageBox.Show("Deseja excluir este produto? A ação é irreversível.", "Ambarina", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        new ProdutoBLL().ExcluirProduto(id);
                        MessageBox.Show("Produto excluído!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        AtualizarGridProdutos();
                        CarregarProdutosBase();
                    }
                    catch (Exception ex) { MessageBox.Show("Erro ao excluir: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }

            if (dgvProdutos.Columns[e.ColumnIndex].Name == "colEditarProd")
            {
                pnlFormCadastroProduto.BackColor = Color.FromArgb(255, 252, 240);
                idProdutoSelecionado = id;
                txtNomeProduto.Text = dgvProdutos.Rows[e.RowIndex].Cells["colNomeProduto"].Value.ToString();
                cmbCategoriaProduto.Text = dgvProdutos.Rows[e.RowIndex].Cells["colCategoriaProduto"].Value.ToString();
                txtMargemLucro.Text = dgvProdutos.Rows[e.RowIndex].Cells["colMargemLucro"].Value.ToString();
                txtEstoqueMin.Text = dgvProdutos.Rows[e.RowIndex].Cells["colEstoqueMinimo"].Value.ToString();

                btnSalvarProduto.Text = "ATUALIZAR PRODUTO";
                txtNomeProduto.Focus();
            }
        }


        //// LOGOUT E GERENCIAMENTO DE SESSÃO
        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show("Deseja realmente encerrar a sessão atual?", "Fazer Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                // 1. Limpa a variável global do usuário logado por segurança
                Program.UsuarioLogado = null;

                // 2. Procura pelo formulário de Login que está oculto na aplicação
                Form frmLogin = Application.OpenForms["FrmLogin"];

                if (frmLogin != null)
                {
                    frmLogin.Show(); // Traz o Login de volta à tela

                    // Opcional: Limpa os campos de senha do formulário de login para o próximo acesso
                    if (frmLogin.Controls.Find("txtLoginSenha", true).Length > 0)
                    {
                        var txtSenha = (TextBox)frmLogin.Controls.Find("txtLoginSenha", true)[0];
                        txtSenha.Clear();
                        txtSenha.Text = "Senha";
                        txtSenha.ForeColor = Color.FromArgb(190, 169, 137); // Cor do placeholder original
                        txtSenha.PasswordChar = '\0';
                    }
                }
                else
                {
                    // Caso o formulário tenha sido fechado da memória por algum motivo, cria uma nova instância
                    FrmLogin login = new FrmLogin();
                    login.Show();
                }

                // 3. Fecha o menu principal atual sem encerrar o processo inteiro do Application.Exit
                this.Close();
            }
        }

        
    }
}