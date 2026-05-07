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
            dgvAlmoxarifado.Columns["colQtdeAtual"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvAlmoxarifado.Columns["colMinimo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Alinha o TEXTO DO CABEÇALHO (Título) à direita também para acompanhar
            dgvAlmoxarifado.Columns["colQtdeAtual"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvAlmoxarifado.Columns["colMinimo"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            //atualizar grid almoxarifado
            AtualizarGrid();

            //carregar cmb de insumos na produção
            CarregarComboInsumos();
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

            CarregarComboInsumos();

            AbrirPainel(pnlViewProducao);

            CarregarComboProdutos();

            CarregarProdutosBase();
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
                txtCustoInicial.Text = dgvAlmoxarifado.Rows[e.RowIndex].Cells["colCustoUnit"].Value.ToString();
                txtEstoqueMinimo.Text = dgvAlmoxarifado.Rows[e.RowIndex].Cells["colMinimo"].Value.ToString();

                btnAdicionarInsumo.Text = "ATUALIZAR INSUMO"; // Muda o visual do botão
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
                //Validação e Conversão Segura dos dados da tela
                if (cmbInsumo.SelectedValue == null || cmbProduto.SelectedValue == null)
                {
                    MessageBox.Show("Selecione o insumo e o produto antes de finalizar.");
                    return;
                }

                int idInsumo = Convert.ToInt32(cmbInsumo.SelectedValue);
                int idProduto = Convert.ToInt32(cmbProduto.SelectedValue);

                //Uso do TryParse para evitar erros de formato (campo vazio ou letras)
                if (!decimal.TryParse(txtQtdInsumo.Text, out decimal qtdInsumo))
                {
                    MessageBox.Show("Quantidade de insumo inválida.");
                    return;
                }

                if (!int.TryParse(txtQtdeProduzida.Text, out int qtdProduzida))
                {
                    MessageBox.Show("Quantidade produzida inválida.");
                    return;
                }

                //Chamar as BLLs
                InsumoBLL insumoBll = new InsumoBLL();
                ProdutoBLL produtoBll = new ProdutoBLL();

                //Executar as ações no banco
                insumoBll.RegistrarConsumoInsumo(idInsumo, qtdInsumo);
                produtoBll.AdicionarEstoqueProduto(idProduto, qtdProduzida);

                MessageBox.Show("Produção finalizada! Estoques atualizados com sucesso.", "Ambarina", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //Limpar e atualizar
                txtQtdInsumo.Clear();
                txtQtdeProduzida.Clear();
                AtualizarGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao processar produção: " + ex.Message);
            }
        }
        private void btnFinalizarProducao_Click(object sender, EventArgs e)
        {
            ExecutarProducao();
        }

        private void AtualizarGradeReceitas()
        {
            try
            {
                ReceitaBLL bll = new ReceitaBLL();
                dgvListaReceitas.DataSource = bll.ListarReceitas();

                // Formatação básica
                dgvListaReceitas.Columns["id_receita"].Visible = false; // Esconde o ID
                dgvListaReceitas.Columns["Produto"].Width = 150;
                dgvListaReceitas.Columns["Aroma"].Width = 150;
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
                // Aqui usamos a BLL de Itens ou a própria ReceitaBLL 
                // para buscar os insumos vinculados a esse ID
                ReceitaBLL bll = new ReceitaBLL();

                dgvItensReceita.DataSource = bll.ListarItensDaReceita(idReceita);

                // Ajuste das colunas da grade da direita
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
            //Pega os dados da linha clicada
            int idReceitaSelecionada = Convert.ToInt32(dgvListaReceitas.Rows[e.RowIndex].Cells["id_receita"].Value);
            string produtoNome = dgvListaReceitas.Rows[e.RowIndex].Cells["Produto"].Value.ToString();
            string aromaNome = dgvListaReceitas.Rows[e.RowIndex].Cells["Aroma"].Value.ToString();

            //Carrega os insumos na grid da direita (OK)
            CarregarInsumosDaReceita(idReceitaSelecionada);

            //Preenche a área de PRODUÇÃO (Baixo)
            cmbProduto.Text = produtoNome;
            cmbAroma.Text = aromaNome;

            //Já focar o cursor na quantidade para ganhar tempo
            txtQtdeProduzida.Focus();
        }

        private void dgvListaReceitas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Pega o ID da linha (Certifique-se que o nome da coluna no banco/grid seja id_receita)
            int id = Convert.ToInt32(dgvListaReceitas.Rows[e.RowIndex].Cells["id_receita"].Value);

            // Lógica Excluir (Certifique-se que o Name da coluna de botão seja colExcluirReceita)
            if (dgvListaReceitas.Columns[e.ColumnIndex].Name == "colExcluirReceita")
            {
                if (MessageBox.Show("Deseja excluir esta receita?", "Ambarina", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    new ReceitaBLL().ExcluirReceita(id);
                    AtualizarGradeReceitas(); // Sua função que dá o Refresh na grid
                }
            }

            // Lógica Editar (Certifique-se que o Name da coluna de botão seja colEditarReceita)
            if (dgvListaReceitas.Columns[e.ColumnIndex].Name == "colEditarReceita")
            {
                //Muda a cor do painel ou sinaliza a edição (opcional)
                //pnlReceitas.BackColor = Color.FromArgb(255, 252, 240); 

                //Guarda o ID para o Update
                idReceitaSelecionada = id;

                //Joga os dados de volta para os campos de cima
                cmbProdutoBase.Text = dgvListaReceitas.Rows[e.RowIndex].Cells["Produto"].Value.ToString();
                txtAroma.Text = dgvListaReceitas.Rows[e.RowIndex].Cells["Aroma"].Value.ToString();

                //Carrega os insumos dessa receita na grid lateral (aquela da direita)
                CarregarInsumosDaReceita(id);

                //Muda o visual do botão de salvar
                btnSalvarReceitaCompleta.Text = "ATUALIZAR RECEITA";
            }
        }
        private void btnAdicionarInsumo_Click(object sender, EventArgs e)
        {
            if (cmbInsumo.SelectedIndex != -1 && !string.IsNullOrEmpty(txtQtdInsumo.Text))
            {
                //Adiciona uma linha vazia e pega o índice dela
                int n = dgvItensReceita.Rows.Add();

                //Preenche cada célula pelo NOME da coluna (ajuste os nomes se forem diferentes no Designer)
                dgvItensReceita.Rows[n].Cells["colInsumo"].Value = cmbInsumo.Text;
                dgvItensReceita.Rows[n].Cells["colQtd"].Value = txtQtdInsumo.Text;
                dgvItensReceita.Rows[n].Cells["colUnidade"].Value = cmbUnidadeReceita.Text;

                //Limpa apenas os campos de insumo, mantendo o Produto Base e Aroma intactos
                cmbInsumo.SelectedIndex = -1;
                txtQtdInsumo.Clear();
                cmbUnidadeReceita.SelectedIndex = -1;
                cmbInsumo.Focus();
            }
        }

        private void btnSalvarReceitaCompleta_Click(object sender, EventArgs e)
        {
            try
            {
                //Validação básica
                if (cmbProdutoBase.SelectedIndex == -1 || string.IsNullOrEmpty(txtAroma.Text))
                {
                    MessageBox.Show("Selecione um Produto Base e digite o Aroma!", "Ambarina");
                    return;
                }

                //Criar o objeto DTO da Receita
                ReceitaDTO receita = new ReceitaDTO();
                receita.IdProduto = Convert.ToInt32(cmbProdutoBase.SelectedValue);
                receita.AromaPadrao = txtAroma.Text;

                //Criar a lista de insumos baseada na Grid da direita
                List<ItensReceitaDTO> listaItens = new List<ItensReceitaDTO>();
                foreach (DataGridViewRow row in dgvItensReceita.Rows)
                {
                    if (row.Cells[0].Value != null)
                    {
                        listaItens.Add(new ItensReceitaDTO
                        {
                            NomeInsumo = row.Cells["Insumo"].Value.ToString(),
                            Quantidade = Convert.ToDecimal(row.Cells["Qtd"].Value)
                        });
                    }
                }

                //Chamar a BLL para salvar tudo de uma vez
                ReceitaBLL bll = new ReceitaBLL();
                bll.SalvarReceitaCompleta(receita, listaItens);

                MessageBox.Show("Receita de " + txtAroma.Text + " salva com sucesso!", "Ambarina");

                //Limpar e atualizar
                AtualizarGradeReceitas();
                LimparCamposReceita();
            }
            catch (Exception ex) { MessageBox.Show("Erro ao salvar: " + ex.Message); }
        }

        private void LimparCamposReceita()
        {
            txtAroma.Clear();
            txtQtdInsumo.Clear();
            cmbProdutoBase.SelectedIndex = -1;
            cmbInsumo.SelectedIndex = -1;
            cmbUnidadeReceita.SelectedIndex = -1;
            txtNomeInsumo.Focus();
        }

        private void dgvItensReceita_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            //Lógica para EXCLUIR o insumo da lista temporária
            if (dgvItensReceita.Columns[e.ColumnIndex].Name == "colExcluirItem") // Ajuste para o nome da sua coluna de X
            {
                dgvItensReceita.Rows.RemoveAt(e.RowIndex);
            }

            //Lógica para EDITAR (devolve para os campos e remove da grid)
            if (dgvItensReceita.Columns[e.ColumnIndex].Name == "colEditarItem") // Ajuste para o nome da sua coluna de Lápis
            {
                cmbInsumo.Text = dgvItensReceita.Rows[e.RowIndex].Cells["Insumo"].Value.ToString();
                txtQtdInsumo.Text = dgvItensReceita.Rows[e.RowIndex].Cells["Qtd"].Value.ToString();
                cmbUnidadeReceita.Text = dgvItensReceita.Rows[e.RowIndex].Cells["Unid"].Value.ToString();

                dgvItensReceita.Rows.RemoveAt(e.RowIndex);
                txtQtdInsumo.Focus();
            }
        }


        ////ESTOQUE OU PRONTA ENTREGA
        private void btnSalvarProduto_Click(object sender, EventArgs e)
        {
            try
            {
                //Instanciar o DTO com os dados da tela
                ProdutoDTO novoModelo = new ProdutoDTO();
                novoModelo.Nome = txtNomeProduto.Text; // Ex: Vela Aurora 180g
                novoModelo.Categoria = cmbCategoriaProduto.Text; // Ex: Vela

                //Conversão com tratamento para evitar erros se o campo estiver vazio
                novoModelo.MargemLucro = string.IsNullOrEmpty(txtMargemLucro.Text) ? 0 : Convert.ToDecimal(txtMargemLucro.Text);
                novoModelo.EstoqueMinimo = string.IsNullOrEmpty(txtEstoqueMin.Text) ? 0 : Convert.ToInt32(txtEstoqueMin.Text);

                //Chamar a BLL para salvar
                ProdutoBLL bll = new ProdutoBLL();
                bll.SalvarProduto(novoModelo);

                //Feedback e Limpeza
                MessageBox.Show("Modelo de produto cadastrado no catálogo com sucesso!", "Ambarina", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimparCamposCadastroProduto();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar modelo: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método auxiliar para limpar os campos após cadastrar
        private void LimparCamposCadastroProduto()
        {
            txtNomeProduto.Clear();
            cmbCategoriaProduto.SelectedIndex = -1;
            txtMargemLucro.Clear();
            txtEstoqueMin.Clear();
        }

        
    }
}
