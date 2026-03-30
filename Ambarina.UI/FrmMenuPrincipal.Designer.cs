namespace Ambarina.UI
{
    partial class FrmMenuPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMenuPrincipal));
            lbLoginExit = new Label();
            pnlLateral = new Panel();
            btnNavFinanceiro = new Button();
            btnNavVendas = new Button();
            btnNavEstoque = new Button();
            btnNavProducao = new Button();
            btnNavAlmoxarifado = new Button();
            btnNavDashboard = new Button();
            picMenuLogo = new PictureBox();
            pnlTopo = new Panel();
            lblDescricaoAba = new Label();
            lblSaudacao = new Label();
            lblTituloAba = new Label();
            pnlConteudo = new Panel();
            pnlViewAlmoxarifado = new Panel();
            dgvAlmoxarifado = new DataGridView();
            colID = new DataGridViewTextBoxColumn();
            colNome = new DataGridViewTextBoxColumn();
            colCategoria = new DataGridViewTextBoxColumn();
            colSaldo = new DataGridViewTextBoxColumn();
            colUnDeMedida = new DataGridViewTextBoxColumn();
            colMinimo = new DataGridViewTextBoxColumn();
            pnlCadastroInsumo = new Panel();
            btnSalvarInsumo = new Button();
            txtCustoInicial = new TextBox();
            lblCustoInicial = new Label();
            txtEstoqueMinimo = new TextBox();
            lblEstoqueMinimo = new Label();
            txtQtdInicial = new TextBox();
            lblQtdeInicial = new Label();
            cmbUnidade = new ComboBox();
            lblUnidadeDeMedida = new Label();
            cmbCategoria = new ComboBox();
            lblCategoria = new Label();
            txtNomeInsumo = new TextBox();
            lblNomeInsumo = new Label();
            pnlViewDashboard = new Panel();
            pnlTabelaVendas = new Panel();
            lblTituloVendasRecentes = new Label();
            dgvVendas = new DataGridView();
            colData = new DataGridViewTextBoxColumn();
            colCliente = new DataGridViewTextBoxColumn();
            colProduto = new DataGridViewTextBoxColumn();
            colValor = new DataGridViewTextBoxColumn();
            flowCards = new FlowLayoutPanel();
            pnlCardFaturamento = new Panel();
            lblStatusFaturamento = new Label();
            lblValFaturamento = new Label();
            lblCapFaturamento = new Label();
            pnlCardInsumos = new Panel();
            lblDescInsumos = new Label();
            lblValInsumos = new Label();
            lblCapInsumos = new Label();
            pnlCardEstoque = new Panel();
            lblDescEstoque = new Label();
            lblValEstoque = new Label();
            lblCapEstoque = new Label();
            pnlLateral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picMenuLogo).BeginInit();
            pnlTopo.SuspendLayout();
            pnlConteudo.SuspendLayout();
            pnlViewAlmoxarifado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAlmoxarifado).BeginInit();
            pnlCadastroInsumo.SuspendLayout();
            pnlViewDashboard.SuspendLayout();
            pnlTabelaVendas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvVendas).BeginInit();
            flowCards.SuspendLayout();
            pnlCardFaturamento.SuspendLayout();
            pnlCardInsumos.SuspendLayout();
            pnlCardEstoque.SuspendLayout();
            SuspendLayout();
            // 
            // lbLoginExit
            // 
            lbLoginExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lbLoginExit.BackColor = Color.FromArgb(242, 233, 216);
            lbLoginExit.Cursor = Cursors.Hand;
            lbLoginExit.Font = new Font("Montserrat Medium", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbLoginExit.ForeColor = Color.FromArgb(190, 138, 58);
            lbLoginExit.Location = new Point(981, 14);
            lbLoginExit.Name = "lbLoginExit";
            lbLoginExit.Size = new Size(30, 30);
            lbLoginExit.TabIndex = 2;
            lbLoginExit.Text = "X";
            lbLoginExit.TextAlign = ContentAlignment.MiddleCenter;
            lbLoginExit.Click += lbLoginExit_Click;
            // 
            // pnlLateral
            // 
            pnlLateral.BackColor = Color.FromArgb(242, 233, 216);
            pnlLateral.Controls.Add(btnNavFinanceiro);
            pnlLateral.Controls.Add(btnNavVendas);
            pnlLateral.Controls.Add(btnNavEstoque);
            pnlLateral.Controls.Add(btnNavProducao);
            pnlLateral.Controls.Add(btnNavAlmoxarifado);
            pnlLateral.Controls.Add(btnNavDashboard);
            pnlLateral.Controls.Add(picMenuLogo);
            pnlLateral.Dock = DockStyle.Left;
            pnlLateral.Location = new Point(0, 0);
            pnlLateral.Name = "pnlLateral";
            pnlLateral.Size = new Size(250, 720);
            pnlLateral.TabIndex = 3;
            // 
            // btnNavFinanceiro
            // 
            btnNavFinanceiro.Anchor = AnchorStyles.Left;
            btnNavFinanceiro.FlatAppearance.BorderSize = 0;
            btnNavFinanceiro.FlatAppearance.MouseDownBackColor = Color.FromArgb(248, 248, 248);
            btnNavFinanceiro.FlatAppearance.MouseOverBackColor = Color.FromArgb(190, 138, 58);
            btnNavFinanceiro.FlatStyle = FlatStyle.Flat;
            btnNavFinanceiro.ForeColor = Color.FromArgb(15, 67, 16);
            btnNavFinanceiro.Location = new Point(20, 530);
            btnNavFinanceiro.Name = "btnNavFinanceiro";
            btnNavFinanceiro.Size = new Size(230, 50);
            btnNavFinanceiro.TabIndex = 7;
            btnNavFinanceiro.Text = "      Financeiro";
            btnNavFinanceiro.UseVisualStyleBackColor = true;
            btnNavFinanceiro.Click += btnNavFinanceiro_Click;
            // 
            // btnNavVendas
            // 
            btnNavVendas.Anchor = AnchorStyles.Left;
            btnNavVendas.FlatAppearance.BorderSize = 0;
            btnNavVendas.FlatAppearance.MouseDownBackColor = Color.FromArgb(248, 248, 248);
            btnNavVendas.FlatAppearance.MouseOverBackColor = Color.FromArgb(190, 138, 58);
            btnNavVendas.FlatStyle = FlatStyle.Flat;
            btnNavVendas.ForeColor = Color.FromArgb(15, 67, 16);
            btnNavVendas.Location = new Point(20, 474);
            btnNavVendas.Name = "btnNavVendas";
            btnNavVendas.Size = new Size(230, 50);
            btnNavVendas.TabIndex = 6;
            btnNavVendas.Text = "      Vendas e Orçamentos";
            btnNavVendas.UseVisualStyleBackColor = true;
            btnNavVendas.Click += btnNavVendas_Click;
            // 
            // btnNavEstoque
            // 
            btnNavEstoque.Anchor = AnchorStyles.Left;
            btnNavEstoque.FlatAppearance.BorderSize = 0;
            btnNavEstoque.FlatAppearance.MouseDownBackColor = Color.FromArgb(248, 248, 248);
            btnNavEstoque.FlatAppearance.MouseOverBackColor = Color.FromArgb(190, 138, 58);
            btnNavEstoque.FlatStyle = FlatStyle.Flat;
            btnNavEstoque.ForeColor = Color.FromArgb(15, 67, 16);
            btnNavEstoque.Location = new Point(20, 418);
            btnNavEstoque.Name = "btnNavEstoque";
            btnNavEstoque.Size = new Size(230, 50);
            btnNavEstoque.TabIndex = 5;
            btnNavEstoque.Text = "      Pronta Entrega";
            btnNavEstoque.UseVisualStyleBackColor = true;
            btnNavEstoque.Click += btnNavEstoque_Click;
            // 
            // btnNavProducao
            // 
            btnNavProducao.Anchor = AnchorStyles.Left;
            btnNavProducao.FlatAppearance.BorderSize = 0;
            btnNavProducao.FlatAppearance.MouseDownBackColor = Color.FromArgb(248, 248, 248);
            btnNavProducao.FlatAppearance.MouseOverBackColor = Color.FromArgb(190, 138, 58);
            btnNavProducao.FlatStyle = FlatStyle.Flat;
            btnNavProducao.ForeColor = Color.FromArgb(15, 67, 16);
            btnNavProducao.Location = new Point(20, 362);
            btnNavProducao.Name = "btnNavProducao";
            btnNavProducao.Size = new Size(230, 50);
            btnNavProducao.TabIndex = 4;
            btnNavProducao.Text = "      Produção";
            btnNavProducao.UseVisualStyleBackColor = true;
            btnNavProducao.Click += btnNavProducao_Click;
            // 
            // btnNavAlmoxarifado
            // 
            btnNavAlmoxarifado.Anchor = AnchorStyles.Left;
            btnNavAlmoxarifado.FlatAppearance.BorderSize = 0;
            btnNavAlmoxarifado.FlatAppearance.MouseDownBackColor = Color.FromArgb(248, 248, 248);
            btnNavAlmoxarifado.FlatAppearance.MouseOverBackColor = Color.FromArgb(190, 138, 58);
            btnNavAlmoxarifado.FlatStyle = FlatStyle.Flat;
            btnNavAlmoxarifado.ForeColor = Color.FromArgb(15, 67, 16);
            btnNavAlmoxarifado.Location = new Point(20, 306);
            btnNavAlmoxarifado.Name = "btnNavAlmoxarifado";
            btnNavAlmoxarifado.Size = new Size(230, 50);
            btnNavAlmoxarifado.TabIndex = 3;
            btnNavAlmoxarifado.Text = "      Almoxarifado";
            btnNavAlmoxarifado.UseVisualStyleBackColor = true;
            btnNavAlmoxarifado.Click += btnNavAlmoxarifado_Click;
            // 
            // btnNavDashboard
            // 
            btnNavDashboard.Anchor = AnchorStyles.Left;
            btnNavDashboard.FlatAppearance.BorderSize = 0;
            btnNavDashboard.FlatAppearance.MouseDownBackColor = Color.FromArgb(248, 248, 248);
            btnNavDashboard.FlatAppearance.MouseOverBackColor = Color.FromArgb(190, 138, 58);
            btnNavDashboard.FlatStyle = FlatStyle.Flat;
            btnNavDashboard.ForeColor = Color.FromArgb(15, 67, 16);
            btnNavDashboard.Location = new Point(20, 250);
            btnNavDashboard.Name = "btnNavDashboard";
            btnNavDashboard.Size = new Size(230, 50);
            btnNavDashboard.TabIndex = 2;
            btnNavDashboard.Text = "      Dashboard";
            btnNavDashboard.UseVisualStyleBackColor = true;
            btnNavDashboard.Click += btnNavDashboard_Click;
            // 
            // picMenuLogo
            // 
            picMenuLogo.Image = Properties.Resources.Ativo_2;
            picMenuLogo.Location = new Point(28, 60);
            picMenuLogo.Name = "picMenuLogo";
            picMenuLogo.Size = new Size(193, 180);
            picMenuLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picMenuLogo.TabIndex = 1;
            picMenuLogo.TabStop = false;
            // 
            // pnlTopo
            // 
            pnlTopo.BackColor = Color.FromArgb(190, 138, 58);
            pnlTopo.Controls.Add(lblDescricaoAba);
            pnlTopo.Controls.Add(lblSaudacao);
            pnlTopo.Controls.Add(lblTituloAba);
            pnlTopo.Controls.Add(lbLoginExit);
            pnlTopo.Dock = DockStyle.Top;
            pnlTopo.ForeColor = Color.FromArgb(190, 138, 58);
            pnlTopo.Location = new Point(250, 0);
            pnlTopo.Name = "pnlTopo";
            pnlTopo.Size = new Size(1030, 60);
            pnlTopo.TabIndex = 4;
            // 
            // lblDescricaoAba
            // 
            lblDescricaoAba.AutoSize = true;
            lblDescricaoAba.Font = new Font("Montserrat", 8.999999F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblDescricaoAba.ForeColor = Color.FromArgb(242, 233, 216);
            lblDescricaoAba.Location = new Point(180, 21);
            lblDescricaoAba.Name = "lblDescricaoAba";
            lblDescricaoAba.Size = new Size(331, 18);
            lblDescricaoAba.TabIndex = 5;
            lblDescricaoAba.Text = "Visão geral do desempenho e métricas da Ambarina.";
            // 
            // lblSaudacao
            // 
            lblSaudacao.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSaudacao.ForeColor = Color.FromArgb(242, 233, 216);
            lblSaudacao.Location = new Point(749, 19);
            lblSaudacao.Name = "lblSaudacao";
            lblSaudacao.Size = new Size(212, 20);
            lblSaudacao.TabIndex = 4;
            lblSaudacao.Text = "Olá, usuário!";
            lblSaudacao.TextAlign = ContentAlignment.TopRight;
            // 
            // lblTituloAba
            // 
            lblTituloAba.AutoSize = true;
            lblTituloAba.Font = new Font("Montserrat", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTituloAba.ForeColor = Color.FromArgb(248, 248, 248);
            lblTituloAba.Location = new Point(20, 14);
            lblTituloAba.Name = "lblTituloAba";
            lblTituloAba.Size = new Size(144, 30);
            lblTituloAba.TabIndex = 3;
            lblTituloAba.Text = "DASHBOARD";
            // 
            // pnlConteudo
            // 
            pnlConteudo.BackColor = Color.FromArgb(248, 248, 248);
            pnlConteudo.Controls.Add(pnlViewAlmoxarifado);
            pnlConteudo.Controls.Add(pnlViewDashboard);
            pnlConteudo.Dock = DockStyle.Fill;
            pnlConteudo.Location = new Point(250, 60);
            pnlConteudo.Name = "pnlConteudo";
            pnlConteudo.Size = new Size(1030, 660);
            pnlConteudo.TabIndex = 5;
            // 
            // pnlViewAlmoxarifado
            // 
            pnlViewAlmoxarifado.BackColor = Color.FromArgb(248, 248, 248);
            pnlViewAlmoxarifado.Controls.Add(dgvAlmoxarifado);
            pnlViewAlmoxarifado.Controls.Add(pnlCadastroInsumo);
            pnlViewAlmoxarifado.Dock = DockStyle.Fill;
            pnlViewAlmoxarifado.Location = new Point(0, 0);
            pnlViewAlmoxarifado.Name = "pnlViewAlmoxarifado";
            pnlViewAlmoxarifado.Size = new Size(1030, 660);
            pnlViewAlmoxarifado.TabIndex = 2;
            pnlViewAlmoxarifado.Visible = false;
            // 
            // dgvAlmoxarifado
            // 
            dgvAlmoxarifado.BackgroundColor = Color.White;
            dgvAlmoxarifado.BorderStyle = BorderStyle.None;
            dgvAlmoxarifado.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvAlmoxarifado.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(15, 67, 16);
            dataGridViewCellStyle1.Font = new Font("Montserrat", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.Padding = new Padding(5, 0, 0, 0);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(15, 67, 16);
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvAlmoxarifado.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvAlmoxarifado.ColumnHeadersHeight = 40;
            dgvAlmoxarifado.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvAlmoxarifado.Columns.AddRange(new DataGridViewColumn[] { colID, colNome, colCategoria, colSaldo, colUnDeMedida, colMinimo });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Window;
            dataGridViewCellStyle4.Font = new Font("Montserrat", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(190, 138, 58);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(190, 138, 58);
            dataGridViewCellStyle4.SelectionForeColor = Color.White;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            dgvAlmoxarifado.DefaultCellStyle = dataGridViewCellStyle4;
            dgvAlmoxarifado.Dock = DockStyle.Fill;
            dgvAlmoxarifado.EnableHeadersVisualStyles = false;
            dgvAlmoxarifado.GridColor = Color.FromArgb(242, 233, 216);
            dgvAlmoxarifado.Location = new Point(350, 0);
            dgvAlmoxarifado.Margin = new Padding(20);
            dgvAlmoxarifado.Name = "dgvAlmoxarifado";
            dgvAlmoxarifado.ReadOnly = true;
            dgvAlmoxarifado.RowHeadersVisible = false;
            dgvAlmoxarifado.RowTemplate.Height = 35;
            dgvAlmoxarifado.ScrollBars = ScrollBars.Vertical;
            dgvAlmoxarifado.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAlmoxarifado.Size = new Size(680, 660);
            dgvAlmoxarifado.TabIndex = 1;
            // 
            // colID
            // 
            colID.HeaderText = "ID";
            colID.Name = "colID";
            colID.ReadOnly = true;
            colID.Visible = false;
            // 
            // colNome
            // 
            colNome.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colNome.HeaderText = "INSUMO";
            colNome.Name = "colNome";
            colNome.ReadOnly = true;
            // 
            // colCategoria
            // 
            colCategoria.HeaderText = "CATEGORIA";
            colCategoria.Name = "colCategoria";
            colCategoria.ReadOnly = true;
            colCategoria.Width = 150;
            // 
            // colSaldo
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Padding = new Padding(0, 0, 10, 0);
            colSaldo.DefaultCellStyle = dataGridViewCellStyle2;
            colSaldo.HeaderText = "SALDO ATUAL";
            colSaldo.Name = "colSaldo";
            colSaldo.ReadOnly = true;
            colSaldo.Width = 160;
            // 
            // colUnDeMedida
            // 
            colUnDeMedida.HeaderText = "UN. DE MEDIDA";
            colUnDeMedida.Name = "colUnDeMedida";
            colUnDeMedida.ReadOnly = true;
            colUnDeMedida.Width = 150;
            // 
            // colMinimo
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Padding = new Padding(0, 0, 10, 0);
            colMinimo.DefaultCellStyle = dataGridViewCellStyle3;
            colMinimo.HeaderText = "ESTOQUE MÍNIMO";
            colMinimo.Name = "colMinimo";
            colMinimo.ReadOnly = true;
            colMinimo.Width = 160;
            // 
            // pnlCadastroInsumo
            // 
            pnlCadastroInsumo.BackColor = SystemColors.ButtonHighlight;
            pnlCadastroInsumo.BorderStyle = BorderStyle.FixedSingle;
            pnlCadastroInsumo.Controls.Add(btnSalvarInsumo);
            pnlCadastroInsumo.Controls.Add(txtCustoInicial);
            pnlCadastroInsumo.Controls.Add(lblCustoInicial);
            pnlCadastroInsumo.Controls.Add(txtEstoqueMinimo);
            pnlCadastroInsumo.Controls.Add(lblEstoqueMinimo);
            pnlCadastroInsumo.Controls.Add(txtQtdInicial);
            pnlCadastroInsumo.Controls.Add(lblQtdeInicial);
            pnlCadastroInsumo.Controls.Add(cmbUnidade);
            pnlCadastroInsumo.Controls.Add(lblUnidadeDeMedida);
            pnlCadastroInsumo.Controls.Add(cmbCategoria);
            pnlCadastroInsumo.Controls.Add(lblCategoria);
            pnlCadastroInsumo.Controls.Add(txtNomeInsumo);
            pnlCadastroInsumo.Controls.Add(lblNomeInsumo);
            pnlCadastroInsumo.Dock = DockStyle.Left;
            pnlCadastroInsumo.Font = new Font("Montserrat", 8.999999F);
            pnlCadastroInsumo.ForeColor = Color.FromArgb(190, 138, 58);
            pnlCadastroInsumo.Location = new Point(0, 0);
            pnlCadastroInsumo.Name = "pnlCadastroInsumo";
            pnlCadastroInsumo.Padding = new Padding(20);
            pnlCadastroInsumo.Size = new Size(350, 660);
            pnlCadastroInsumo.TabIndex = 0;
            pnlCadastroInsumo.Click += pnlCadastroInsumo_Click;
            // 
            // btnSalvarInsumo
            // 
            btnSalvarInsumo.BackColor = Color.FromArgb(15, 67, 16);
            btnSalvarInsumo.FlatStyle = FlatStyle.Flat;
            btnSalvarInsumo.ForeColor = Color.White;
            btnSalvarInsumo.Location = new Point(23, 388);
            btnSalvarInsumo.Margin = new Padding(3, 20, 3, 3);
            btnSalvarInsumo.Name = "btnSalvarInsumo";
            btnSalvarInsumo.Size = new Size(300, 36);
            btnSalvarInsumo.TabIndex = 13;
            btnSalvarInsumo.Text = "SALVAR INSUMO";
            btnSalvarInsumo.UseVisualStyleBackColor = false;
            // 
            // txtCustoInicial
            // 
            txtCustoInicial.BackColor = SystemColors.ButtonHighlight;
            txtCustoInicial.BorderStyle = BorderStyle.FixedSingle;
            txtCustoInicial.Font = new Font("Montserrat", 8.999999F);
            txtCustoInicial.ForeColor = Color.FromArgb(190, 138, 58);
            txtCustoInicial.Location = new Point(23, 327);
            txtCustoInicial.Multiline = true;
            txtCustoInicial.Name = "txtCustoInicial";
            txtCustoInicial.Size = new Size(140, 26);
            txtCustoInicial.TabIndex = 12;
            // 
            // lblCustoInicial
            // 
            lblCustoInicial.AutoSize = true;
            lblCustoInicial.ForeColor = Color.FromArgb(190, 138, 58);
            lblCustoInicial.Location = new Point(20, 302);
            lblCustoInicial.Name = "lblCustoInicial";
            lblCustoInicial.Size = new Size(127, 18);
            lblCustoInicial.TabIndex = 11;
            lblCustoInicial.Text = "CUSTO INICIAL (R$)";
            // 
            // txtEstoqueMinimo
            // 
            txtEstoqueMinimo.BackColor = SystemColors.ButtonHighlight;
            txtEstoqueMinimo.BorderStyle = BorderStyle.FixedSingle;
            txtEstoqueMinimo.Font = new Font("Montserrat", 8.999999F);
            txtEstoqueMinimo.ForeColor = Color.FromArgb(190, 138, 58);
            txtEstoqueMinimo.Location = new Point(183, 254);
            txtEstoqueMinimo.Multiline = true;
            txtEstoqueMinimo.Name = "txtEstoqueMinimo";
            txtEstoqueMinimo.Size = new Size(140, 26);
            txtEstoqueMinimo.TabIndex = 10;
            // 
            // lblEstoqueMinimo
            // 
            lblEstoqueMinimo.AutoSize = true;
            lblEstoqueMinimo.ForeColor = Color.FromArgb(190, 138, 58);
            lblEstoqueMinimo.Location = new Point(180, 229);
            lblEstoqueMinimo.Name = "lblEstoqueMinimo";
            lblEstoqueMinimo.Size = new Size(120, 18);
            lblEstoqueMinimo.TabIndex = 9;
            lblEstoqueMinimo.Text = "ESTOQUE MÍNIMO";
            // 
            // txtQtdInicial
            // 
            txtQtdInicial.BackColor = Color.White;
            txtQtdInicial.BorderStyle = BorderStyle.FixedSingle;
            txtQtdInicial.Font = new Font("Montserrat", 8.999999F);
            txtQtdInicial.ForeColor = Color.FromArgb(190, 138, 58);
            txtQtdInicial.Location = new Point(23, 254);
            txtQtdInicial.Multiline = true;
            txtQtdInicial.Name = "txtQtdInicial";
            txtQtdInicial.Size = new Size(140, 26);
            txtQtdInicial.TabIndex = 8;
            // 
            // lblQtdeInicial
            // 
            lblQtdeInicial.AutoSize = true;
            lblQtdeInicial.ForeColor = Color.FromArgb(190, 138, 58);
            lblQtdeInicial.Location = new Point(20, 229);
            lblQtdeInicial.Name = "lblQtdeInicial";
            lblQtdeInicial.Size = new Size(96, 18);
            lblQtdeInicial.TabIndex = 7;
            lblQtdeInicial.Text = "QTDE. INICIAL";
            // 
            // cmbUnidade
            // 
            cmbUnidade.BackColor = Color.White;
            cmbUnidade.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUnidade.FlatStyle = FlatStyle.Flat;
            cmbUnidade.Font = new Font("Montserrat", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbUnidade.ForeColor = Color.FromArgb(190, 138, 58);
            cmbUnidade.FormattingEnabled = true;
            cmbUnidade.Items.AddRange(new object[] { "gramas", "ml", "L", "un", "m" });
            cmbUnidade.Location = new Point(23, 180);
            cmbUnidade.Name = "cmbUnidade";
            cmbUnidade.Size = new Size(300, 26);
            cmbUnidade.TabIndex = 6;
            // 
            // lblUnidadeDeMedida
            // 
            lblUnidadeDeMedida.AutoSize = true;
            lblUnidadeDeMedida.ForeColor = Color.FromArgb(190, 138, 58);
            lblUnidadeDeMedida.Location = new Point(20, 157);
            lblUnidadeDeMedida.Name = "lblUnidadeDeMedida";
            lblUnidadeDeMedida.Size = new Size(144, 18);
            lblUnidadeDeMedida.TabIndex = 5;
            lblUnidadeDeMedida.Text = "UNIDADE DE MEDIDA";
            // 
            // cmbCategoria
            // 
            cmbCategoria.BackColor = Color.White;
            cmbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategoria.FlatStyle = FlatStyle.Flat;
            cmbCategoria.Font = new Font("Montserrat", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbCategoria.ForeColor = Color.FromArgb(190, 138, 58);
            cmbCategoria.FormattingEnabled = true;
            cmbCategoria.Items.AddRange(new object[] { "Matéria-prima", "Embalagem", "Materiais de Produção" });
            cmbCategoria.Location = new Point(23, 111);
            cmbCategoria.Name = "cmbCategoria";
            cmbCategoria.Size = new Size(300, 26);
            cmbCategoria.TabIndex = 4;
            // 
            // lblCategoria
            // 
            lblCategoria.AutoSize = true;
            lblCategoria.ForeColor = Color.FromArgb(190, 138, 58);
            lblCategoria.Location = new Point(20, 88);
            lblCategoria.Name = "lblCategoria";
            lblCategoria.Size = new Size(82, 18);
            lblCategoria.TabIndex = 2;
            lblCategoria.Text = "CATEGORIA";
            // 
            // txtNomeInsumo
            // 
            txtNomeInsumo.BackColor = Color.White;
            txtNomeInsumo.BorderStyle = BorderStyle.FixedSingle;
            txtNomeInsumo.Font = new Font("Montserrat", 8.999999F);
            txtNomeInsumo.ForeColor = Color.FromArgb(190, 138, 58);
            txtNomeInsumo.Location = new Point(23, 45);
            txtNomeInsumo.Multiline = true;
            txtNomeInsumo.Name = "txtNomeInsumo";
            txtNomeInsumo.Size = new Size(300, 26);
            txtNomeInsumo.TabIndex = 1;
            // 
            // lblNomeInsumo
            // 
            lblNomeInsumo.AutoSize = true;
            lblNomeInsumo.ForeColor = Color.FromArgb(190, 138, 58);
            lblNomeInsumo.Location = new Point(20, 20);
            lblNomeInsumo.Name = "lblNomeInsumo";
            lblNomeInsumo.Size = new Size(124, 18);
            lblNomeInsumo.TabIndex = 0;
            lblNomeInsumo.Text = "NOME DO INSUMO";
            // 
            // pnlViewDashboard
            // 
            pnlViewDashboard.Controls.Add(pnlTabelaVendas);
            pnlViewDashboard.Controls.Add(flowCards);
            pnlViewDashboard.Dock = DockStyle.Fill;
            pnlViewDashboard.Location = new Point(0, 0);
            pnlViewDashboard.Name = "pnlViewDashboard";
            pnlViewDashboard.Size = new Size(1030, 660);
            pnlViewDashboard.TabIndex = 5;
            // 
            // pnlTabelaVendas
            // 
            pnlTabelaVendas.BackColor = Color.Transparent;
            pnlTabelaVendas.Controls.Add(lblTituloVendasRecentes);
            pnlTabelaVendas.Controls.Add(dgvVendas);
            pnlTabelaVendas.Dock = DockStyle.Fill;
            pnlTabelaVendas.Location = new Point(0, 180);
            pnlTabelaVendas.Name = "pnlTabelaVendas";
            pnlTabelaVendas.Padding = new Padding(30, 40, 30, 30);
            pnlTabelaVendas.Size = new Size(1030, 480);
            pnlTabelaVendas.TabIndex = 1;
            pnlTabelaVendas.Paint += pnlTabelaVendas_Paint;
            // 
            // lblTituloVendasRecentes
            // 
            lblTituloVendasRecentes.AutoSize = true;
            lblTituloVendasRecentes.Font = new Font("Montserrat", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTituloVendasRecentes.ForeColor = Color.FromArgb(15, 67, 16);
            lblTituloVendasRecentes.Location = new Point(30, 0);
            lblTituloVendasRecentes.Name = "lblTituloVendasRecentes";
            lblTituloVendasRecentes.Size = new Size(173, 25);
            lblTituloVendasRecentes.TabIndex = 1;
            lblTituloVendasRecentes.Text = "VENDAS RECENTES";
            // 
            // dgvVendas
            // 
            dgvVendas.AllowUserToAddRows = false;
            dgvVendas.BackgroundColor = Color.White;
            dgvVendas.BorderStyle = BorderStyle.None;
            dgvVendas.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(15, 67, 16);
            dataGridViewCellStyle5.Font = new Font("Montserrat", 9.749999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle5.ForeColor = Color.White;
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(15, 67, 16);
            dataGridViewCellStyle5.SelectionForeColor = Color.White;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            dgvVendas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dgvVendas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvVendas.Columns.AddRange(new DataGridViewColumn[] { colData, colCliente, colProduto, colValor });
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = SystemColors.Window;
            dataGridViewCellStyle6.Font = new Font("Montserrat", 9.749999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle6.ForeColor = Color.FromArgb(190, 138, 58);
            dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(190, 138, 58);
            dataGridViewCellStyle6.SelectionForeColor = Color.White;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            dgvVendas.DefaultCellStyle = dataGridViewCellStyle6;
            dgvVendas.Dock = DockStyle.Fill;
            dgvVendas.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvVendas.EnableHeadersVisualStyles = false;
            dgvVendas.GridColor = Color.FromArgb(242, 233, 216);
            dgvVendas.Location = new Point(30, 40);
            dgvVendas.Name = "dgvVendas";
            dgvVendas.ReadOnly = true;
            dgvVendas.RowHeadersVisible = false;
            dgvVendas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVendas.Size = new Size(970, 410);
            dgvVendas.TabIndex = 0;
            // 
            // colData
            // 
            colData.HeaderText = "DATA";
            colData.Name = "colData";
            colData.ReadOnly = true;
            // 
            // colCliente
            // 
            colCliente.HeaderText = "CLIENTE";
            colCliente.Name = "colCliente";
            colCliente.ReadOnly = true;
            colCliente.Width = 250;
            // 
            // colProduto
            // 
            colProduto.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colProduto.HeaderText = "PRODUTO";
            colProduto.Name = "colProduto";
            colProduto.ReadOnly = true;
            // 
            // colValor
            // 
            colValor.HeaderText = "VALOR (R$)";
            colValor.Name = "colValor";
            colValor.ReadOnly = true;
            colValor.Width = 120;
            // 
            // flowCards
            // 
            flowCards.BackColor = Color.Transparent;
            flowCards.Controls.Add(pnlCardFaturamento);
            flowCards.Controls.Add(pnlCardInsumos);
            flowCards.Controls.Add(pnlCardEstoque);
            flowCards.Dock = DockStyle.Top;
            flowCards.Location = new Point(0, 0);
            flowCards.Name = "flowCards";
            flowCards.Padding = new Padding(20);
            flowCards.Size = new Size(1030, 180);
            flowCards.TabIndex = 0;
            // 
            // pnlCardFaturamento
            // 
            pnlCardFaturamento.BackColor = Color.White;
            pnlCardFaturamento.Controls.Add(lblStatusFaturamento);
            pnlCardFaturamento.Controls.Add(lblValFaturamento);
            pnlCardFaturamento.Controls.Add(lblCapFaturamento);
            pnlCardFaturamento.Cursor = Cursors.Hand;
            pnlCardFaturamento.Location = new Point(30, 30);
            pnlCardFaturamento.Margin = new Padding(10);
            pnlCardFaturamento.Name = "pnlCardFaturamento";
            pnlCardFaturamento.Size = new Size(280, 130);
            pnlCardFaturamento.TabIndex = 0;
            pnlCardFaturamento.MouseEnter += pnlCardInsumos_MouseEnter;
            pnlCardFaturamento.MouseLeave += pnlCardInsumos_MouseLeave;
            // 
            // lblStatusFaturamento
            // 
            lblStatusFaturamento.AutoSize = true;
            lblStatusFaturamento.Font = new Font("Montserrat", 8.249999F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblStatusFaturamento.ForeColor = Color.FromArgb(190, 169, 137);
            lblStatusFaturamento.Location = new Point(15, 90);
            lblStatusFaturamento.Name = "lblStatusFaturamento";
            lblStatusFaturamento.Size = new Size(193, 17);
            lblStatusFaturamento.TabIndex = 2;
            lblStatusFaturamento.Text = "↑ 12% em relação ao mês anterior";
            lblStatusFaturamento.MouseEnter += pnlCardInsumos_MouseEnter;
            lblStatusFaturamento.MouseLeave += pnlCardInsumos_MouseLeave;
            // 
            // lblValFaturamento
            // 
            lblValFaturamento.AutoSize = true;
            lblValFaturamento.Font = new Font("Montserrat Medium", 17.9999981F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblValFaturamento.ForeColor = Color.FromArgb(15, 67, 16);
            lblValFaturamento.Location = new Point(15, 40);
            lblValFaturamento.Name = "lblValFaturamento";
            lblValFaturamento.Size = new Size(108, 38);
            lblValFaturamento.TabIndex = 1;
            lblValFaturamento.Text = "R$ 0,00";
            lblValFaturamento.MouseEnter += pnlCardInsumos_MouseEnter;
            lblValFaturamento.MouseLeave += pnlCardInsumos_MouseLeave;
            // 
            // lblCapFaturamento
            // 
            lblCapFaturamento.AutoSize = true;
            lblCapFaturamento.Font = new Font("Montserrat Medium", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCapFaturamento.ForeColor = Color.FromArgb(190, 138, 58);
            lblCapFaturamento.Location = new Point(15, 15);
            lblCapFaturamento.Name = "lblCapFaturamento";
            lblCapFaturamento.Size = new Size(160, 18);
            lblCapFaturamento.TabIndex = 0;
            lblCapFaturamento.Text = "FATURAMENTO MENSAL";
            lblCapFaturamento.MouseEnter += pnlCardInsumos_MouseEnter;
            lblCapFaturamento.MouseLeave += pnlCardInsumos_MouseLeave;
            // 
            // pnlCardInsumos
            // 
            pnlCardInsumos.BackColor = Color.White;
            pnlCardInsumos.Controls.Add(lblDescInsumos);
            pnlCardInsumos.Controls.Add(lblValInsumos);
            pnlCardInsumos.Controls.Add(lblCapInsumos);
            pnlCardInsumos.Cursor = Cursors.Hand;
            pnlCardInsumos.Location = new Point(330, 30);
            pnlCardInsumos.Margin = new Padding(10);
            pnlCardInsumos.Name = "pnlCardInsumos";
            pnlCardInsumos.Size = new Size(280, 130);
            pnlCardInsumos.TabIndex = 3;
            pnlCardInsumos.MouseEnter += pnlCardInsumos_MouseEnter;
            pnlCardInsumos.MouseLeave += pnlCardInsumos_MouseLeave;
            // 
            // lblDescInsumos
            // 
            lblDescInsumos.AutoSize = true;
            lblDescInsumos.Font = new Font("Montserrat", 8.249999F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblDescInsumos.ForeColor = Color.FromArgb(190, 169, 137);
            lblDescInsumos.Location = new Point(15, 90);
            lblDescInsumos.Name = "lblDescInsumos";
            lblDescInsumos.Size = new Size(146, 17);
            lblDescInsumos.TabIndex = 2;
            lblDescInsumos.Text = "Necessitam de reposição";
            lblDescInsumos.MouseEnter += pnlCardInsumos_MouseEnter;
            lblDescInsumos.MouseLeave += pnlCardInsumos_MouseLeave;
            // 
            // lblValInsumos
            // 
            lblValInsumos.AutoSize = true;
            lblValInsumos.Font = new Font("Montserrat Medium", 17.9999981F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblValInsumos.ForeColor = Color.FromArgb(15, 67, 16);
            lblValInsumos.Location = new Point(15, 40);
            lblValInsumos.Name = "lblValInsumos";
            lblValInsumos.Size = new Size(110, 38);
            lblValInsumos.TabIndex = 1;
            lblValInsumos.Text = "0 ITENS";
            lblValInsumos.MouseEnter += pnlCardInsumos_MouseEnter;
            lblValInsumos.MouseLeave += pnlCardInsumos_MouseLeave;
            // 
            // lblCapInsumos
            // 
            lblCapInsumos.AutoSize = true;
            lblCapInsumos.Font = new Font("Montserrat Medium", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCapInsumos.ForeColor = Color.FromArgb(190, 138, 58);
            lblCapInsumos.Location = new Point(15, 15);
            lblCapInsumos.Name = "lblCapInsumos";
            lblCapInsumos.Size = new Size(139, 18);
            lblCapInsumos.TabIndex = 0;
            lblCapInsumos.Text = "ALERTAS DE INSUMO";
            lblCapInsumos.MouseEnter += pnlCardInsumos_MouseEnter;
            lblCapInsumos.MouseLeave += pnlCardInsumos_MouseLeave;
            // 
            // pnlCardEstoque
            // 
            pnlCardEstoque.BackColor = Color.White;
            pnlCardEstoque.Controls.Add(lblDescEstoque);
            pnlCardEstoque.Controls.Add(lblValEstoque);
            pnlCardEstoque.Controls.Add(lblCapEstoque);
            pnlCardEstoque.Cursor = Cursors.Hand;
            pnlCardEstoque.Location = new Point(630, 30);
            pnlCardEstoque.Margin = new Padding(10);
            pnlCardEstoque.Name = "pnlCardEstoque";
            pnlCardEstoque.Size = new Size(280, 130);
            pnlCardEstoque.TabIndex = 4;
            pnlCardEstoque.MouseEnter += pnlCardInsumos_MouseEnter;
            pnlCardEstoque.MouseLeave += pnlCardInsumos_MouseLeave;
            // 
            // lblDescEstoque
            // 
            lblDescEstoque.AutoSize = true;
            lblDescEstoque.Font = new Font("Montserrat", 8.249999F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblDescEstoque.ForeColor = Color.FromArgb(190, 169, 137);
            lblDescEstoque.Location = new Point(15, 90);
            lblDescEstoque.Name = "lblDescEstoque";
            lblDescEstoque.Size = new Size(121, 17);
            lblDescEstoque.TabIndex = 2;
            lblDescEstoque.Text = "Unidades finalizadas";
            lblDescEstoque.MouseEnter += pnlCardInsumos_MouseEnter;
            lblDescEstoque.MouseLeave += pnlCardInsumos_MouseLeave;
            // 
            // lblValEstoque
            // 
            lblValEstoque.AutoSize = true;
            lblValEstoque.Font = new Font("Montserrat Medium", 17.9999981F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblValEstoque.ForeColor = Color.FromArgb(15, 67, 16);
            lblValEstoque.Location = new Point(15, 40);
            lblValEstoque.Name = "lblValEstoque";
            lblValEstoque.Size = new Size(181, 38);
            lblValEstoque.TabIndex = 1;
            lblValEstoque.Text = "0 PRODUTOS";
            lblValEstoque.MouseEnter += pnlCardInsumos_MouseEnter;
            lblValEstoque.MouseLeave += pnlCardInsumos_MouseLeave;
            // 
            // lblCapEstoque
            // 
            lblCapEstoque.AutoSize = true;
            lblCapEstoque.Font = new Font("Montserrat Medium", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCapEstoque.ForeColor = Color.FromArgb(190, 138, 58);
            lblCapEstoque.Location = new Point(15, 15);
            lblCapEstoque.Name = "lblCapEstoque";
            lblCapEstoque.Size = new Size(125, 18);
            lblCapEstoque.TabIndex = 0;
            lblCapEstoque.Text = "PRONTA ENTREGA";
            lblCapEstoque.MouseEnter += pnlCardInsumos_MouseEnter;
            lblCapEstoque.MouseLeave += pnlCardInsumos_MouseLeave;
            // 
            // FrmMenuPrincipal
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(253, 251, 249);
            ClientSize = new Size(1280, 720);
            Controls.Add(pnlConteudo);
            Controls.Add(pnlTopo);
            Controls.Add(pnlLateral);
            Font = new Font("Montserrat", 9.749999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            Name = "FrmMenuPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Ambarina Gestão";
            Load += FrmMenuPrincipal_Load;
            pnlLateral.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picMenuLogo).EndInit();
            pnlTopo.ResumeLayout(false);
            pnlTopo.PerformLayout();
            pnlConteudo.ResumeLayout(false);
            pnlViewAlmoxarifado.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAlmoxarifado).EndInit();
            pnlCadastroInsumo.ResumeLayout(false);
            pnlCadastroInsumo.PerformLayout();
            pnlViewDashboard.ResumeLayout(false);
            pnlTabelaVendas.ResumeLayout(false);
            pnlTabelaVendas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvVendas).EndInit();
            flowCards.ResumeLayout(false);
            pnlCardFaturamento.ResumeLayout(false);
            pnlCardFaturamento.PerformLayout();
            pnlCardInsumos.ResumeLayout(false);
            pnlCardInsumos.PerformLayout();
            pnlCardEstoque.ResumeLayout(false);
            pnlCardEstoque.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label lbLoginExit;
        private Panel pnlLateral;
        private Panel pnlTopo;
        private Panel pnlConteudo;
        private PictureBox picMenuLogo;
        private Button btnNavDashboard;
        private Button btnNavFinanceiro;
        private Button btnNavVendas;
        private Button btnNavEstoque;
        private Button btnNavProducao;
        private Button btnNavAlmoxarifado;
        private Label lblTituloAba;
        private Label lblSaudacao;
        private Label lblDescricaoAba;
        private Panel pnlTabelaVendas;
        private DataGridView dgvVendas;
        private Label lblTituloVendasRecentes;
        private DataGridViewTextBoxColumn colData;
        private DataGridViewTextBoxColumn colCliente;
        private DataGridViewTextBoxColumn colProduto;
        private DataGridViewTextBoxColumn colValor;
        private Panel pnlViewDashboard;
        private FlowLayoutPanel flowCards;
        private Panel pnlCardFaturamento;
        private Label lblStatusFaturamento;
        private Label lblValFaturamento;
        private Label lblCapFaturamento;
        private Panel pnlCardInsumos;
        private Label lblDescInsumos;
        private Label lblValInsumos;
        private Label lblCapInsumos;
        private Panel pnlCardEstoque;
        private Label lblDescEstoque;
        private Label lblValEstoque;
        private Label lblCapEstoque;
        private Panel pnlViewAlmoxarifado;
        private Panel pnlCadastroInsumo;
        private Label lblNomeInsumo;
        private ComboBox cmbCategoria;
        private Label lblCategoria;
        private TextBox txtNomeInsumo;
        private ComboBox cmbUnidade;
        private Label lblUnidadeDeMedida;
        private TextBox txtEstoqueMinimo;
        private Label lblEstoqueMinimo;
        private TextBox txtQtdInicial;
        private Label lblQtdeInicial;
        private Button btnSalvarInsumo;
        private TextBox txtCustoInicial;
        private Label lblCustoInicial;
        private DataGridView dgvAlmoxarifado;
        private DataGridViewTextBoxColumn colID;
        private DataGridViewTextBoxColumn colNome;
        private DataGridViewTextBoxColumn colCategoria;
        private DataGridViewTextBoxColumn colSaldo;
        private DataGridViewTextBoxColumn colUnDeMedida;
        private DataGridViewTextBoxColumn colMinimo;
    }
}