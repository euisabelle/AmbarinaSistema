using Ambarina.BLL;
using Ambarina.DTO;
using Ambarina.DAL;
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
        int idProdutoSelecionado = 0; // Para produtos no Estoque

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
            this.WindowState = FormWindowState.Maximized; // Inicia o formulário maximizado

            try
            {
                // Acessa diretamente o usuário logado armazenado no Program
                if (Program.UsuarioLogado != null && !string.IsNullOrEmpty(Program.UsuarioLogado.Nome))
                {
                    string nomeUsuario = Program.UsuarioLogado.Nome;
                    lblSaudacao.Text = $"Olá, {nomeUsuario}!";
                }
                else
                {
                    lblSaudacao.Text = "Olá, Usuário!";
                }
            }
            catch (InvalidOperationException)
            {
                // Fallback caso nenhum usuário esteja logado
                lblSaudacao.Text = "Olá, Usuário!";
            }

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
            dgvAlmoxarifado.Columns["colQtdeAtual"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvAlmoxarifado.Columns["colMinimo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Alinha o TEXTO DO CABEÇALHO (Título) à direita também para acompanhar
            dgvAlmoxarifado.Columns["colQtdeAtual"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvAlmoxarifado.Columns["colMinimo"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            //atualizar grid almoxarifado
            AtualizarGrid();

            //carregar cmb de insumos na produção
            CarregarComboInsumos();

            txtLote.Text = new ProducaoBLL().ObterProximoLote(); // Gera o próximo lote sequencial para ficar pronto na tela

            //atualizar grid produção
            AtualizarGridProducao();

            //RECEITA: Deixa o campo de unidade de medida protegido, pois ele é preenchido automaticamente com base no insumo selecionado
            txtUnidadeReceita.ReadOnly = true;
            txtUnidadeReceita.BackColor = Color.FromArgb(240, 240, 240); // Feedback visual de bloqueado
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
                // DESLIGA o evento temporariamente para o Windows Forms não dar disparos fantasmas
                cmbInsumo.SelectedIndexChanged -= cmbInsumo_SelectedIndexChanged;

                // CARREGAMENTO DE DADOS SINCRONIZADO
                CarregarComboInsumos();    // Lista de matérias-primas
                CarregarComboProdutos();   // Lista de produtos para a área de Produção (baixo)
                CarregarProdutosBase();    // Lista de produtos para a área de Receita (cima)
                CarregarComboAromas();     // Lista de aromas vindos das receitas cadastradas
                AtualizarGradeReceitas();  // Lista o catálogo de receitas na grid da direita
            }
            finally
            {
                // REATIVA o evento agora que todos os combos já estão carregados e estáveis
                cmbInsumo.SelectedIndexChanged += cmbInsumo_SelectedIndexChanged;
            }

            AbrirPainel(pnlViewProducao);
        }

        private void btnNavEstoque_Click(object sender, EventArgs e)
        {
            SelecionarBotao((Button)sender);

            AtualizarCabecalho("ESTOQUE", "Controle de produtos finalizados e prontos para o cliente.");

            AtualizarGridProdutos(); // Carrega a lista de produtos

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

        private void btnSalvarInsumo_Click(object sender, EventArgs e)
        {
            try
            {
                //Pegamos os valores brutos da tela
                decimal qtdTotal = Convert.ToDecimal(txtQtdInicial.Text);
                decimal custoTotal = Convert.ToDecimal(txtCustoInicial.Text);

                //Calculamos quanto custa 1 unidade (1g ou 1ml)
                // Se a pessoa digitar 100g e 55 reais, o custo unitário será 0,55
                decimal custoFracionado = custoTotal / qtdTotal;

                //Preencher o DTO com o valor calculado
                InsumoDTO novoInsumo = new InsumoDTO();
                novoInsumo.Nome = txtNomeInsumo.Text;
                novoInsumo.Categoria = cmbCategoria.Text;
                novoInsumo.UnidadeMedida = cmbUnidade.Text;

                //Aqui salvamos a quantidade real (ex: 100) e o custo de cada grama (ex: 0,55)
                novoInsumo.QtdeInicial = qtdTotal;
                novoInsumo.CustoInicial = custoFracionado;

                novoInsumo.EstoqueMinimo = Convert.ToDecimal(txtEstoqueMinimo.Text);

                InsumoBLL bll = new InsumoBLL();

                //Lógica de decisão (Salvar ou Editar)
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
                    pnlViewAlmoxarifado.BackColor = Color.White; // Retorna à cor normal
                }

                AtualizarGrid();
                LimparCamposAlmoxarifado();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimparCamposAlmoxarifado()
        {
            txtNomeInsumo.Clear();
            txtQtdInicial.Clear();
            txtCustoInicial.Clear();
            txtEstoqueMinimo.Clear();
            cmbCategoria.SelectedIndex = -1;
            cmbUnidade.SelectedIndex = -1;
            txtNomeInsumo.Focus();
        }

        private void AtualizarGrid()
        {
            try
            {
                dgvAlmoxarifado.AutoGenerateColumns = false; // Esta linha impede que o C# crie colunas extras

                InsumoBLL bll = new InsumoBLL();
                dgvAlmoxarifado.DataSource = bll.ListarInsumos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar a lista: " + ex.Message);
            }
        }

        private void dgvAlmoxarifado_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Pega o ID da linha 
            int id = Convert.ToInt32(dgvAlmoxarifado.Rows[e.RowIndex].Cells["colID"].Value);

            // Lógica Excluir
            if (dgvAlmoxarifado.Columns[e.ColumnIndex].Name == "colExcluirAlmox")
            {
                if (MessageBox.Show("Deseja excluir este item?", "Ambarina", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    new InsumoBLL().ExcluirInsumo(id);
                    AtualizarGrid();
                }
            }

            // Lógica Editar
            if (dgvAlmoxarifado.Columns[e.ColumnIndex].Name == "colEditarAlmox")
            {
                pnlViewAlmoxarifado.BackColor = Color.FromArgb(255, 252, 240);
                idInsumoSelecionado = id; // Guarda o ID para o Update
                txtNomeInsumo.Text = dgvAlmoxarifado.Rows[e.RowIndex].Cells["colNome"].Value.ToString();
                cmbCategoria.Text = dgvAlmoxarifado.Rows[e.RowIndex].Cells["colCategoria"].Value.ToString();
                cmbUnidade.Text = dgvAlmoxarifado.Rows[e.RowIndex].Cells["colUnDeMedida"].Value.ToString();
                txtQtdInicial.Text = dgvAlmoxarifado.Rows[e.RowIndex].Cells["colQtdeAtual"].Value.ToString();
                txtCustoInicial.Text = dgvAlmoxarifado.Rows[e.RowIndex].Cells["colCustoTotalInsumo"].Value.ToString();
                txtEstoqueMinimo.Text = dgvAlmoxarifado.Rows[e.RowIndex].Cells["colMinimo"].Value.ToString();

                btnSalvarInsumo.Text = "ATUALIZAR INSUMO"; // Botão correto
                txtNomeInsumo.Focus(); // Leva o foco para o primeiro campo
            }
        }

        ////PRODUCAO
        private void CarregarProdutosBase()
        {
            try
            {
                ProdutoBLL bll = new ProdutoBLL();
                // Buscamos os modelos cadastrados no catálogo
                cmbProdutoBase.DataSource = bll.ListarProdutosCombo();
                cmbProdutoBase.DisplayMember = "nome";
                cmbProdutoBase.ValueMember = "id_produto";
                cmbProdutoBase.SelectedIndex = -1;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void CarregarComboInsumos()
        {
            try
            {
                InsumoBLL bll = new InsumoBLL();
                DataTable dt = bll.ListarInsumosCombo();

                cmbInsumo.DataSource = dt;
                cmbInsumo.DisplayMember = "nome";       // Nome que aparece na lista
                cmbInsumo.ValueMember = "id_insumo";     // ID que fica "escondido" por trás

                cmbInsumo.SelectedIndex = -1; // Inicia vazio para não selecionar o primeiro item direto
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar insumos na produção: " + ex.Message);
            }
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
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar produtos: " + ex.Message);
            }
        }

        private void ExecutarProducao()
        {
            try
            {
                //Validações básicas de Interface
                if (cmbProduto.SelectedValue == null)
                {
                    MessageBox.Show("Selecione um produto antes de finalizar a produção.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtQtdeProduzida.Text, out int qtdProduzida) || qtdProduzida <= 0)
                {
                    MessageBox.Show("Informe uma quantidade produzida válida e maior que zero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //Alimentando o DTO de Produção com as regras estabelecidas
                ProducaoDTO novaProducao = new ProducaoDTO();
                novaProducao.IdProduto = Convert.ToInt32(cmbProduto.SelectedValue);
                novaProducao.DataProducao = dtpData.Value;
                novaProducao.QtdeProduzida = qtdProduzida;
                novaProducao.Lote = txtLote.Text;
                novaProducao.Status = "EM CURA"; // Definido que toda produção inicia EM CURA

                //Executando o processamento completo na camada de negócio
                ProducaoBLL producaoBll = new ProducaoBLL();
                producaoBll.ProcessarProducaoCompleta(novaProducao);

                // Feedback de sucesso para o usuário
                MessageBox.Show($"Produção do lote {novaProducao.Lote} registrada com sucesso!\nEstoques de insumos atualizados.",
                                "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Resetando e atualizando a tela para a próxima operação
                txtQtdeProduzida.Clear();
                cmbProduto.SelectedIndex = -1;
                cmbAroma.SelectedIndex = -1;

                // Recarrega as grids para mostrar os novos saldos na hora
                AtualizarGrid(); // Atualiza Almoxarifado
                if (typeof(FrmMenuPrincipal).GetMethod("AtualizarGridProdutos") != null) AtualizarGridProdutos(); // Atualiza Estoque

                // forçar a Grid de Produção a se atualizar!
                AtualizarGridProducao();

                // Gera o próximo lote sequencial para ficar pronto na tela
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

        ////PRODUCAO - RECEITAS

        private void CarregarComboAromas()
        {
            try
            {
                ReceitaBLL bll = new ReceitaBLL();
                DataTable dt = bll.ListarAromas();

                cmbAroma.DataSource = dt;
                cmbAroma.DisplayMember = "aroma_padrao";
                cmbAroma.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar aromas: " + ex.Message);
            }
        }

        private void cmbInsumo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Só age se o usuário selecionou uma linha válida e o valor não for nulo/vazio
            if (cmbInsumo.SelectedIndex != -1 && cmbInsumo.SelectedValue != null && !string.IsNullOrEmpty(cmbInsumo.SelectedValue.ToString()))
            {
                // Evita executar o código caso o valor seja o próprio objeto do sistema durante transições
                if (cmbInsumo.SelectedValue.ToString() == "System.Data.DataRowView") return;

                try
                {
                    if (int.TryParse(cmbInsumo.SelectedValue.ToString(), out int idInsumo))
                    {
                        InsumoBLL bll = new InsumoBLL();
                        string unidade = bll.ObterUnidadeMedidaInsumo(idInsumo);

                        // Injeta na tela
                        txtUnidadeReceita.Text = unidade;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao atualizar unidade de medida: " + ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void AtualizarGradeReceitas()
        {
            try
            {
                ReceitaBLL bll = new ReceitaBLL();
                dgvListaReceitas.DataSource = bll.ListarReceitas();

                // Formatação básica
                dgvListaReceitas.Columns["id_receita"].Visible = false;
                //dgvListaReceitas.Columns["Produto"].Width = 150;
                //dgvListaReceitas.Columns["Aroma"].Width = 150;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar lista de receitas: " + ex.Message);
            }
        }

        private void CarregarInsumosDaReceita(int idReceita)
        {
            try
            {
                // 1. DESATIVA a criação automática de colunas
                dgvItensReceita.AutoGenerateColumns = false;

                ReceitaBLL bll = new ReceitaBLL();
                // 2. Carrega os dados
                dgvItensReceita.DataSource = bll.ListarItensDaReceita(idReceita);

                if (dgvItensReceita.Columns.Contains("id_insumo"))
                    dgvItensReceita.Columns["id_insumo"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar insumos: " + ex.Message);
            }
        }

        private void dgvListaReceitas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // Se clicar em coluna de ação, ignora
            if (dgvListaReceitas.Columns[e.ColumnIndex].Name == "colEditarReceita" ||
                dgvListaReceitas.Columns[e.ColumnIndex].Name == "colExcluirReceita")
                return;

            // Pega os dados da linha clicada
            int idReceitaSelecionada = Convert.ToInt32(dgvListaReceitas.Rows[e.RowIndex].Cells["id_receita"].Value);
            string produtoNome = dgvListaReceitas.Rows[e.RowIndex].Cells["Produto"].Value.ToString();
            string aromaNome = dgvListaReceitas.Rows[e.RowIndex].Cells["Aroma"].Value.ToString();

            // Carrega os insumos na grid
            CarregarInsumosDaReceita(idReceitaSelecionada);

            // Preenche a área de PRODUÇÃO
            cmbProduto.Text = produtoNome;
            cmbAroma.Text = aromaNome;

            // Foca na quantidade
            txtQtdeProduzida.Focus();
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

                // CORREÇÃO: Pega o texto gerado automaticamente no seu TextBox e joga na coluna da Grid!
                dgvItensReceita.Rows[n].Cells["colUnidade"].Value = txtUnidadeReceita.Text;

                // Limpa campos
                cmbInsumo.SelectedIndex = -1;
                txtQtdInsumo.Clear();
                txtUnidadeReceita.Clear(); // Limpa o campo de unidade também para o próximo
                cmbInsumo.Focus();
            }
        }

        private void dgvListaReceitas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int id = Convert.ToInt32(dgvListaReceitas.Rows[e.RowIndex].Cells["id_receita"].Value);

            // Lógica Excluir
            if (dgvListaReceitas.Columns[e.ColumnIndex].Name == "ColBtnExcluirReceita")
            {
                if (MessageBox.Show("Deseja excluir permanentemente esta receita da Ambarina?", "Excluir",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    try
                    {
                        new ReceitaBLL().ExcluirReceita(id);
                        MessageBox.Show("Receita excluída com sucesso!");
                        AtualizarGradeReceitas();
                        LimparCamposReceita();
                    }
                    catch (Exception ex) { MessageBox.Show("Erro ao excluir: " + ex.Message); }
                }
            }

            // Lógica Editar
            if (dgvListaReceitas.Columns[e.ColumnIndex].Name == "ColBtnEditarReceita")
            {
                idReceitaSelecionada = Convert.ToInt32(dgvListaReceitas.Rows[e.RowIndex].Cells["id_receita"].Value);
                cmbProdutoBase.Text = dgvListaReceitas.Rows[e.RowIndex].Cells["Produto"].Value.ToString();
                txtAroma.Text = dgvListaReceitas.Rows[e.RowIndex].Cells["Aroma"].Value.ToString();

                // ESSENCIAL: Remove o vínculo e limpa a grid antes de adicionar as novas linhas
                dgvItensReceita.DataSource = null;
                dgvItensReceita.Rows.Clear();

                ReceitaBLL bll = new ReceitaBLL();
                DataTable dtItens = bll.ListarItensDaReceita(idReceitaSelecionada);

                // Verificar se o DataTable tem dados
                if (dtItens != null && dtItens.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtItens.Rows)
                    {
                        // Adiciona a linha e preenche usando os nomes exatos das colunas do seu DataTable
                        int rowIndex = dgvItensReceita.Rows.Add();

                        // Preenchimento seguro das colunas
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

            // Se o valor da célula de insumo for nulo, ignora o clique para não quebrar
            if (dgvItensReceita.Rows[e.RowIndex].Cells["colInsumo"].Value == null) return;

            // Nomes possíveis da coluna de excluir (conforme Designer)
            string nomeColExcluir1 = "colBtnExcluirItensReceita";
            string nomeColExcluir2 = "colExcluirItensReceita"; // fallback se tiver variação
            string nomeColEditar = "ColBtnEditarItensReceita";

            // Excluir item
            if (dgvItensReceita.Columns[e.ColumnIndex].Name == nomeColExcluir1 ||
                dgvItensReceita.Columns[e.ColumnIndex].Name == nomeColExcluir2)
            {
                if (MessageBox.Show("Deseja remover este insumo da lista?", "Remover Insumo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                // Se a grid estiver vinculada a um DataSource (vinda do banco),
                // desconecta e converte para linhas manuais para permitir remoção e edição local
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

                // Remove a linha (agora em modo manual)
                if (e.RowIndex >= 0 && e.RowIndex < dgvItensReceita.Rows.Count)
                    dgvItensReceita.Rows.RemoveAt(e.RowIndex);

                return;
            }

            // Editar item (comportamento existente)
            if (dgvItensReceita.Columns[e.ColumnIndex].Name == nomeColEditar)
            {
                cmbInsumo.Text = dgvItensReceita.Rows[e.RowIndex].Cells["colInsumo"].Value.ToString();
                txtQtdInsumo.Text = dgvItensReceita.Rows[e.RowIndex].Cells["colQtd"].Value.ToString();

                // CORREÇÃO: Devolve o valor da unidade para o seu TextBox ao invés do combo antigo
                txtUnidadeReceita.Text = dgvItensReceita.Rows[e.RowIndex].Cells["colUnidade"].Value?.ToString() ?? "";

                dgvItensReceita.Rows.RemoveAt(e.RowIndex);
                txtQtdInsumo.Focus();
            }
        }

        private void btnSalvarReceitaCompleta_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validações Iniciais
                if (cmbProdutoBase.SelectedIndex == -1 || string.IsNullOrEmpty(txtAroma.Text))
                {
                    MessageBox.Show("Selecione um Produto Base e digite o Aroma!", "Ambarina");
                    return;
                }

                // 2. Criar o DTO da Receita
                ReceitaDTO receita = new ReceitaDTO();
                receita.Id = idReceitaSelecionada; // Se for 0, o banco entende que é nova
                receita.IdProduto = Convert.ToInt32(cmbProdutoBase.SelectedValue);
                receita.AromaPadrao = txtAroma.Text;

                // 3. Detectar insumos duplicados na grid antes de montar a lista
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
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("A receita contém insumos duplicados:");
                    foreach (var d in duplicados)
                        sb.AppendLine($"- {d.Key} ({d.Value}x)");
                    sb.AppendLine();
                    sb.AppendLine("Edite o insumo existente na lista usando o ícone ✎ em vez de adicionar um duplicado.");
                    MessageBox.Show(sb.ToString(), "Insumo duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 4. Criar a lista de insumos percorrendo a Grid da Esquerda
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
                    MessageBox.Show("Adicione pelo menos um insumo válido à receita!", "Ambarina");
                    return;
                }

                ReceitaBLL bll = new ReceitaBLL();

                // 5. Lógica de Salvar ou Editar
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

                // 6. Resetar o formulário para o estado inicial
                idReceitaSelecionada = 0; // Importantíssimo para a próxima receita não virar edição
                btnSalvarReceitaCompleta.Text = "SALVAR RECEITA";
                pnlCardReceita.BackColor = Color.White; // Volta a cor original se você mudou no editar

                // 7. Atualizar Grids e Limpar
                AtualizarGradeReceitas();
                if (typeof(FrmMenuPrincipal).GetMethod("CarregarComboAromas") != null) CarregarComboAromas();

                LimparCamposReceita();
                dgvItensReceita.Rows.Clear(); // Limpa a grid de montagem
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao processar receita: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimparCamposReceita()
        {
            txtAroma.Clear();
            txtQtdInsumo.Clear();
            dgvItensReceita.DataSource = null;
            dgvItensReceita.Rows.Clear();
            cmbProdutoBase.SelectedIndex = -1;
            cmbInsumo.SelectedIndex = -1;
            //cmbUnidadeReceita.SelectedIndex = -1;
            idReceitaSelecionada = 0;
            btnSalvarReceitaCompleta.Text = "SALVAR RECEITA";
            pnlCardReceita.BackColor = Color.White;
            cmbProdutoBase.Focus();
        }

        private void AtualizarGridProducao()
        {
            try
            {
                ProducaoBLL bll = new ProducaoBLL();
                DataTable dt = bll.ListarProducoes();

                // Desvincula eventos temporariamente para não disparar validações falsas no carregamento
                dgvProducao.CellValueChanged -= dgvProducao_CellValueChanged;

                dgvProducao.Rows.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    int rowIndex = dgvProducao.Rows.Add();
                    DataGridViewRow gridRow = dgvProducao.Rows[rowIndex];

                    gridRow.Cells["dataGridViewTextBoxColumnColID"].Value = row["id_producao"];
                    gridRow.Cells["dataGridViewTextBoxColData"].Value = Convert.ToDateTime(row["data_producao"]).ToString("dd/MM/yyyy");
                    gridRow.Cells["dataGridViewTextBoxColProduto"].Value = row["nome_produto"];
                    gridRow.Cells["ColAroma"].Value = row["aroma_padrao"] != DBNull.Value ? row["aroma_padrao"].ToString() : "Sem Aroma";
                    gridRow.Cells["dataGridViewTextBoxColLote"].Value = row["lote"];
                    gridRow.Cells["dataGridViewTextBoxColStatus"].Value = row["status"].ToString();
                    gridRow.Cells["colRemoverProd"].Value = "Excluir";

                    gridRow.Tag = new { IdProduto = row["id_produto"], Qtd = row["qtde_produzida"] };

                    // TRAVA SÊNIOR: Se o status já for EMBALADA, bloqueia a linha imediatamente
                    string statusAtual = row["status"].ToString().Trim().ToUpper();
                    if (statusAtual == "EMBALADA")
                    {
                        // Deixa a célula de Status como ReadOnly (não dá para abrir o combo)
                        gridRow.Cells["dataGridViewTextBoxColStatus"].ReadOnly = true;

                        // Transforma a célula de exclusão em texto puro e limpa o valor para o botão sumir/ficar inativo
                        gridRow.Cells["colRemoverProd"].Value = "---";
                        gridRow.Cells["colRemoverProd"].ReadOnly = true;

                        // Feedback visual: pinta a linha com um cinza bem levinho sofisticado
                        gridRow.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
                        gridRow.DefaultCellStyle.ForeColor = Color.DarkGray;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar listagem de produção: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dgvProducao.CellValueChanged += dgvProducao_CellValueChanged;
            }
        }

        private void dgvProducao_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvProducao.Columns[e.ColumnIndex].Name == "dataGridViewTextBoxColStatus")
            {
                DataGridViewRow row = dgvProducao.Rows[e.RowIndex];
                int idProducao = Convert.ToInt32(row.Cells["dataGridViewTextBoxColumnColID"].Value);
                string novoStatus = row.Cells["dataGridViewTextBoxColStatus"].Value.ToString().Trim().ToUpper();

                dynamic dadosOcultos = row.Tag;
                int idProduto = Convert.ToInt32(dadosOcultos.IdProduto);
                int qtdProduzida = Convert.ToInt32(dadosOcultos.Qtd);

                if (novoStatus == "EMBALADA")
                {
                    DialogResult resposta = MessageBox.Show(
                        $"Você tem certeza que deseja alterar o status do lote para EMBALADA?\n\nIsso irá adicionar automaticamente {qtdProduzida} unidades deste produto ao seu estoque de Pronta Entrega e esta ação trancará este lote contra alterações.",
                        "Confirmação de Entrada de Estoque",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (resposta == DialogResult.No)
                    {
                        MessageBox.Show("Alteração cancelada. O estoque permanece inalterado.", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        AtualizarGridProducao();
                        return;
                    }
                }

                try
                {
                    ProducaoBLL bll = new ProducaoBLL();
                    bll.AtualizarStatus(idProducao, novoStatus, idProduto, qtdProduzida);

                    MessageBox.Show($"Status do lote atualizado para {novoStatus} com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Recarrega a grid para aplicar o visual cinza e as travas ReadOnly de forma limpa direto do banco
                    AtualizarGridProducao();

                    if (typeof(FrmMenuPrincipal).GetMethod("AtualizarGridProdutos") != null) AtualizarGridProdutos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao atualizar status: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AtualizarGridProducao();
                }
            }
        }

        private void dgvProducao_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica se o clique foi de fato na coluna de remover/excluir e não em uma linha inválida (-1)
            if (e.RowIndex >= 0 && dgvProducao.Columns[e.ColumnIndex].Name == "colRemoverProd")
            {
                DataGridViewRow row = dgvProducao.Rows[e.RowIndex];

                // Coleta os dados da linha selecionada
                int idProducao = Convert.ToInt32(row.Cells["dataGridViewTextBoxColumnColID"].Value);
                string lote = row.Cells["dataGridViewTextBoxColLote"].Value.ToString();
                string status = row.Cells["dataGridViewTextBoxColStatus"].Value.ToString();

                // Recupera os IDs e quantidades que guardamos no Tag da linha ao carregar a grid
                dynamic dadosOcultos = row.Tag;
                int idProduto = Convert.ToInt32(dadosOcultos.IdProduto);
                int qtdProduzida = Convert.ToInt32(dadosOcultos.Qtd);

                // Caixa de diálogo confirmando se o usuário quer mesmo apagar a digitação incorreta
                DialogResult confirmacao = MessageBox.Show(
                    $"Deseja realmente excluir o registro do lote {lote}?\n\nEsta ação apagará o histórico permanentemente e devolverá todos os insumos consumidos de volta ao Almoxarifado.",
                    "Confirmar Exclusão de Registro",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (confirmacao == DialogResult.Yes)
                {
                    try
                    {
                        // Dispara o estorno e deleção através da BLL
                        ProducaoBLL bll = new ProducaoBLL();
                        bll.ProcessarExclusaoComEstorno(idProducao, idProduto, qtdProduzida, status);

                        MessageBox.Show($"Produção do lote {lote} excluída e insumos devolvidos com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Recarrega a Grid de Produção para sumir com a linha deletada na hora
                        AtualizarGridProducao();

                        // Atualiza o lote do formulário de inserção, pois o número livre mudou!
                        txtLote.Text = bll.ObterProximoLote();
                    }
                    catch (Exception ex)
                    {
                        // Se cair na trava da BLL (ex: status Embalada) ou der erro no MySQL, exibe aqui em formato amigável
                        MessageBox.Show(ex.Message, "Aviso de Segurança", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        ////ESTOQUE - VISUALIZAR E GERENCIAR PRODUTOS
        private void AtualizarGridProdutos()
        {
            try
            {
                dgvProdutos.AutoGenerateColumns = false;

                ProdutoBLL bll = new ProdutoBLL();
                dgvProdutos.DataSource = bll.ListarProdutos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar a lista de produtos: " + ex.Message);
            }
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
                // Validação básica
                if (string.IsNullOrEmpty(txtNomeProduto.Text))
                {
                    MessageBox.Show("Digite o nome do produto!", "Validação");
                    return;
                }

                if (cmbCategoriaProduto.SelectedIndex == -1)
                {
                    MessageBox.Show("Selecione uma categoria!", "Validação");
                    return;
                }

                // Instanciar o DTO com os dados da tela
                ProdutoDTO novoModelo = new ProdutoDTO();
                novoModelo.Nome = txtNomeProduto.Text;
                novoModelo.Categoria = cmbCategoriaProduto.Text;

                // Conversão com tratamento para evitar erros se o campo estiver vazio
                novoModelo.MargemLucro = string.IsNullOrEmpty(txtMargemLucro.Text) ? 0 : Convert.ToDecimal(txtMargemLucro.Text);
                novoModelo.EstoqueMinimo = string.IsNullOrEmpty(txtEstoqueMin.Text) ? 0 : Convert.ToInt32(txtEstoqueMin.Text);

                // Chamar a BLL para salvar ou editar
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

                // Feedback e Limpeza
                AtualizarGridProdutos();
                CarregarProdutosBase(); // Atualiza combo de produtos em Produção
                LimparCamposCadastroProduto();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar/editar produto: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvProdutos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Pega o ID da linha
            int id = Convert.ToInt32(dgvProdutos.Rows[e.RowIndex].Cells["colIDProduto"].Value);

            // Lógica Excluir
            if (dgvProdutos.Columns[e.ColumnIndex].Name == "colExcluirProd")
            {
                if (MessageBox.Show("Deseja excluir este produto? Esta ação é irreversível.", "Ambarina", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        new ProdutoBLL().ExcluirProduto(id);
                        MessageBox.Show("Produto excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        AtualizarGridProdutos();
                        CarregarProdutosBase(); // Atualiza combo em Produção
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao excluir: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            // Lógica Editar
            if (dgvProdutos.Columns[e.ColumnIndex].Name == "colEditarProd")
            {
                pnlFormCadastroProduto.BackColor = Color.FromArgb(255, 252, 240); // Cor de destaque
                idProdutoSelecionado = id;
                txtNomeProduto.Text = dgvProdutos.Rows[e.RowIndex].Cells["colNomeProduto"].Value.ToString();
                cmbCategoriaProduto.Text = dgvProdutos.Rows[e.RowIndex].Cells["colCategoriaProduto"].Value.ToString();
                txtMargemLucro.Text = dgvProdutos.Rows[e.RowIndex].Cells["colMargemLucro"].Value.ToString();
                txtEstoqueMin.Text = dgvProdutos.Rows[e.RowIndex].Cells["colEstoqueMinimo"].Value.ToString();

                btnSalvarProduto.Text = "ATUALIZAR PRODUTO";
                txtNomeProduto.Focus();
            }
        }

        
    }
}
