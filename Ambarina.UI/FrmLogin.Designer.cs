namespace Ambarina.UI
{
    partial class FrmLogin
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

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
            panel1 = new Panel();
            lblRedefinirSenha = new Label();
            lbCaps = new Label();
            picLoginNaoVerSenha = new PictureBox();
            picLoginIconUser = new PictureBox();
            panel3 = new Panel();
            panel2 = new Panel();
            btnLoginEntrar = new Button();
            picLoginLogo = new PictureBox();
            txtLoginSenha = new TextBox();
            txtLoginUsuario = new TextBox();
            lblInstrucoesRedefinicao = new Label();
            txtLoginNovaSenha = new TextBox();
            txtLoginConfirmaSenha = new TextBox();
            btnSalvarSenha = new Button();
            btnCancelarRedefinicao = new Button();
            lbLoginExit = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLoginNaoVerSenha).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLoginIconUser).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLoginLogo).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(248, 248, 248);
            panel1.Controls.Add(lblRedefinirSenha);
            panel1.Controls.Add(lbCaps);
            panel1.Controls.Add(picLoginNaoVerSenha);
            panel1.Controls.Add(picLoginIconUser);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(btnLoginEntrar);
            panel1.Controls.Add(picLoginLogo);
            panel1.Controls.Add(txtLoginSenha);
            panel1.Controls.Add(txtLoginUsuario);
            panel1.Controls.Add(lblInstrucoesRedefinicao);
            panel1.Controls.Add(txtLoginNovaSenha);
            panel1.Controls.Add(txtLoginConfirmaSenha);
            panel1.Controls.Add(btnSalvarSenha);
            panel1.Controls.Add(btnCancelarRedefinicao);
            panel1.Location = new Point(477, 102);
            panel1.Name = "panel1";
            panel1.Size = new Size(270, 400);
            panel1.TabIndex = 0;
            // 
            // lblRedefinirSenha
            // 
            lblRedefinirSenha.Font = new Font("Montserrat", 8.999999F, FontStyle.Underline, GraphicsUnit.Point, 0);
            lblRedefinirSenha.ForeColor = Color.FromArgb(190, 169, 137);
            lblRedefinirSenha.Location = new Point(34, 355);
            lblRedefinirSenha.Name = "lblRedefinirSenha";
            lblRedefinirSenha.Size = new Size(202, 23);
            lblRedefinirSenha.TabIndex = 12;
            lblRedefinirSenha.Text = "esqueci a senha / redefinir senha";
            lblRedefinirSenha.TextAlign = ContentAlignment.MiddleCenter;
            lblRedefinirSenha.Click += lblRedefinirSenha_Click;
            // 
            // lbCaps
            // 
            lbCaps.BackColor = Color.Transparent;
            lbCaps.Font = new Font("Montserrat", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbCaps.ForeColor = Color.FromArgb(190, 169, 137);
            lbCaps.Location = new Point(34, 267);
            lbCaps.Name = "lbCaps";
            lbCaps.Size = new Size(202, 20);
            lbCaps.TabIndex = 12;
            lbCaps.Text = "Caps Lock ligado!";
            lbCaps.TextAlign = ContentAlignment.MiddleCenter;
            lbCaps.Visible = false;
            // 
            // picLoginNaoVerSenha
            // 
            picLoginNaoVerSenha.BackColor = Color.White;
            picLoginNaoVerSenha.Cursor = Cursors.Hand;
            picLoginNaoVerSenha.Image = Properties.Resources.fluent_eye_32_regular;
            picLoginNaoVerSenha.Location = new Point(212, 235);
            picLoginNaoVerSenha.Name = "picLoginNaoVerSenha";
            picLoginNaoVerSenha.Size = new Size(15, 15);
            picLoginNaoVerSenha.SizeMode = PictureBoxSizeMode.Zoom;
            picLoginNaoVerSenha.TabIndex = 11;
            picLoginNaoVerSenha.TabStop = false;
            picLoginNaoVerSenha.Click += picLoginNaoVerSenha_Click;
            // 
            // picLoginIconUser
            // 
            picLoginIconUser.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            picLoginIconUser.BackColor = Color.White;
            picLoginIconUser.Image = Properties.Resources.Vector__1_;
            picLoginIconUser.Location = new Point(214, 174);
            picLoginIconUser.Name = "picLoginIconUser";
            picLoginIconUser.Size = new Size(13, 13);
            picLoginIconUser.SizeMode = PictureBoxSizeMode.Zoom;
            picLoginIconUser.TabIndex = 9;
            picLoginIconUser.TabStop = false;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(190, 138, 58);
            panel3.Location = new Point(34, 254);
            panel3.Name = "panel3";
            panel3.Size = new Size(200, 1);
            panel3.TabIndex = 8;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(190, 138, 58);
            panel2.Location = new Point(34, 192);
            panel2.Name = "panel2";
            panel2.Size = new Size(200, 1);
            panel2.TabIndex = 7;
            // 
            // btnLoginEntrar
            // 
            btnLoginEntrar.BackColor = Color.FromArgb(190, 138, 58);
            btnLoginEntrar.Cursor = Cursors.Hand;
            btnLoginEntrar.FlatStyle = FlatStyle.Flat;
            btnLoginEntrar.Font = new Font("Montserrat Medium", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLoginEntrar.ForeColor = Color.White;
            btnLoginEntrar.Location = new Point(81, 301);
            btnLoginEntrar.Name = "btnLoginEntrar";
            btnLoginEntrar.Size = new Size(107, 35);
            btnLoginEntrar.TabIndex = 3;
            btnLoginEntrar.Text = "Entrar";
            btnLoginEntrar.UseVisualStyleBackColor = false;
            btnLoginEntrar.Click += btnLoginEntrar_Click;
            // 
            // picLoginLogo
            // 
            picLoginLogo.Image = (Image)resources.GetObject("picLoginLogo.Image");
            picLoginLogo.Location = new Point(63, 58);
            picLoginLogo.Name = "picLoginLogo";
            picLoginLogo.Size = new Size(142, 47);
            picLoginLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLoginLogo.TabIndex = 0;
            picLoginLogo.TabStop = false;
            // 
            // txtLoginSenha
            // 
            txtLoginSenha.Anchor = AnchorStyles.Left;
            txtLoginSenha.BackColor = Color.White;
            txtLoginSenha.BorderStyle = BorderStyle.None;
            txtLoginSenha.Font = new Font("Montserrat", 9.749999F);
            txtLoginSenha.ForeColor = Color.FromArgb(190, 169, 137);
            txtLoginSenha.Location = new Point(34, 230);
            txtLoginSenha.Margin = new Padding(5);
            txtLoginSenha.Multiline = true;
            txtLoginSenha.Name = "txtLoginSenha";
            txtLoginSenha.Size = new Size(200, 25);
            txtLoginSenha.TabIndex = 2;
            txtLoginSenha.Text = "Senha";
            txtLoginSenha.TextChanged += txtLoginSenha_TextChanged;
            txtLoginSenha.Enter += txtLoginSenha_Enter;
            txtLoginSenha.KeyDown += txtLoginSenha_KeyDown;
            txtLoginSenha.Leave += txtLoginSenha_Leave;
            // 
            // txtLoginUsuario
            // 
            txtLoginUsuario.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtLoginUsuario.AutoCompleteSource = AutoCompleteSource.HistoryList;
            txtLoginUsuario.BackColor = Color.White;
            txtLoginUsuario.BorderStyle = BorderStyle.None;
            txtLoginUsuario.Font = new Font("Montserrat", 9.749999F);
            txtLoginUsuario.ForeColor = Color.FromArgb(190, 169, 137);
            txtLoginUsuario.Location = new Point(34, 168);
            txtLoginUsuario.Margin = new Padding(5);
            txtLoginUsuario.Multiline = true;
            txtLoginUsuario.Name = "txtLoginUsuario";
            txtLoginUsuario.Size = new Size(200, 25);
            txtLoginUsuario.TabIndex = 1;
            txtLoginUsuario.Text = "Usuário";
            txtLoginUsuario.TextChanged += txtLoginUsuario_TextChanged;
            txtLoginUsuario.Enter += txtLoginUsuario_Enter;
            txtLoginUsuario.KeyDown += txtLoginUsuario_KeyDown;
            txtLoginUsuario.Leave += txtLoginUsuario_Leave;
            // 
            // lblInstrucoesRedefinicao
            // 
            lblInstrucoesRedefinicao.Font = new Font("Montserrat", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblInstrucoesRedefinicao.ForeColor = Color.FromArgb(190, 169, 137);
            lblInstrucoesRedefinicao.Location = new Point(34, 130);
            lblInstrucoesRedefinicao.Name = "lblInstrucoesRedefinicao";
            lblInstrucoesRedefinicao.Size = new Size(200, 34);
            lblInstrucoesRedefinicao.TabIndex = 20;
            lblInstrucoesRedefinicao.Text = "Informe usuário, nova senha e confirme";
            lblInstrucoesRedefinicao.TextAlign = ContentAlignment.MiddleCenter;
            lblInstrucoesRedefinicao.Visible = false;
            // 
            // txtLoginNovaSenha
            // 
            txtLoginNovaSenha.Anchor = AnchorStyles.Left;
            txtLoginNovaSenha.BackColor = Color.White;
            txtLoginNovaSenha.BorderStyle = BorderStyle.None;
            txtLoginNovaSenha.Font = new Font("Montserrat", 9.749999F);
            txtLoginNovaSenha.ForeColor = Color.FromArgb(190, 169, 137);
            txtLoginNovaSenha.Location = new Point(34, 199);
            txtLoginNovaSenha.Margin = new Padding(5);
            txtLoginNovaSenha.Multiline = true;
            txtLoginNovaSenha.Name = "txtLoginNovaSenha";
            txtLoginNovaSenha.Size = new Size(200, 25);
            txtLoginNovaSenha.TabIndex = 21;
            txtLoginNovaSenha.Text = "Nova senha";
            txtLoginNovaSenha.Visible = false;
            txtLoginNovaSenha.TextChanged += txtLoginSenha_TextChanged;
            txtLoginNovaSenha.Enter += txtLoginNovaSenha_Enter;
            txtLoginNovaSenha.Leave += txtLoginNovaSenha_Leave;
            // 
            // txtLoginConfirmaSenha
            // 
            txtLoginConfirmaSenha.Anchor = AnchorStyles.Left;
            txtLoginConfirmaSenha.BackColor = Color.White;
            txtLoginConfirmaSenha.BorderStyle = BorderStyle.None;
            txtLoginConfirmaSenha.Font = new Font("Montserrat", 9.749999F);
            txtLoginConfirmaSenha.ForeColor = Color.FromArgb(190, 169, 137);
            txtLoginConfirmaSenha.Location = new Point(34, 230);
            txtLoginConfirmaSenha.Margin = new Padding(5);
            txtLoginConfirmaSenha.Multiline = true;
            txtLoginConfirmaSenha.Name = "txtLoginConfirmaSenha";
            txtLoginConfirmaSenha.Size = new Size(200, 25);
            txtLoginConfirmaSenha.TabIndex = 22;
            txtLoginConfirmaSenha.Text = "Confirmar senha";
            txtLoginConfirmaSenha.Visible = false;
            txtLoginConfirmaSenha.TextChanged += txtLoginSenha_TextChanged;
            txtLoginConfirmaSenha.Enter += txtLoginConfirmaSenha_Enter;
            txtLoginConfirmaSenha.Leave += txtLoginConfirmaSenha_Leave;
            // 
            // btnSalvarSenha
            // 
            btnSalvarSenha.BackColor = Color.FromArgb(190, 138, 58);
            btnSalvarSenha.Cursor = Cursors.Hand;
            btnSalvarSenha.FlatStyle = FlatStyle.Flat;
            btnSalvarSenha.Font = new Font("Montserrat Medium", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSalvarSenha.ForeColor = Color.White;
            btnSalvarSenha.Location = new Point(34, 301);
            btnSalvarSenha.Name = "btnSalvarSenha";
            btnSalvarSenha.Size = new Size(200, 35);
            btnSalvarSenha.TabIndex = 23;
            btnSalvarSenha.Text = "Salvar nova senha";
            btnSalvarSenha.UseVisualStyleBackColor = false;
            btnSalvarSenha.Visible = false;
            btnSalvarSenha.Click += btnSalvarSenha_Click;
            // 
            // btnCancelarRedefinicao
            // 
            btnCancelarRedefinicao.BackColor = Color.White;
            btnCancelarRedefinicao.Cursor = Cursors.Hand;
            btnCancelarRedefinicao.FlatStyle = FlatStyle.Flat;
            btnCancelarRedefinicao.Font = new Font("Montserrat", 8.249999F);
            btnCancelarRedefinicao.ForeColor = Color.FromArgb(190, 169, 137);
            btnCancelarRedefinicao.Location = new Point(34, 341);
            btnCancelarRedefinicao.Name = "btnCancelarRedefinicao";
            btnCancelarRedefinicao.Size = new Size(200, 30);
            btnCancelarRedefinicao.TabIndex = 24;
            btnCancelarRedefinicao.Text = "Cancelar";
            btnCancelarRedefinicao.UseVisualStyleBackColor = false;
            btnCancelarRedefinicao.Visible = false;
            btnCancelarRedefinicao.Click += btnCancelarRedefinicao_Click;
            // 
            // lbLoginExit
            // 
            lbLoginExit.BackColor = Color.Transparent;
            lbLoginExit.Cursor = Cursors.Hand;
            lbLoginExit.Font = new Font("Montserrat Medium", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbLoginExit.ForeColor = Color.FromArgb(190, 138, 58);
            lbLoginExit.Location = new Point(761, 9);
            lbLoginExit.Name = "lbLoginExit";
            lbLoginExit.Size = new Size(27, 27);
            lbLoginExit.TabIndex = 1;
            lbLoginExit.Text = "X";
            lbLoginExit.TextAlign = ContentAlignment.MiddleCenter;
            lbLoginExit.Click += lbLoginExit_Click;
            lbLoginExit.MouseEnter += lbLoginExit_MouseEnter;
            lbLoginExit.MouseLeave += lbLoginExit_MouseLeave;
            // 
            // FrmLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BackgroundImage = Properties.Resources.bgLoginSistema;
            ClientSize = new Size(800, 600);
            Controls.Add(lbLoginExit);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            KeyPreview = true;
            Name = "FrmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmLogin";
            Load += FrmLogin_Load;
            Shown += FrmLogin_Shown;
            KeyDown += FrmLogin_KeyDown;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picLoginNaoVerSenha).EndInit();
            ((System.ComponentModel.ISupportInitialize)picLoginIconUser).EndInit();
            ((System.ComponentModel.ISupportInitialize)picLoginLogo).EndInit();
            ResumeLayout(false);
        }



        private Panel panel1;
        private PictureBox picLoginLogo;
        private Button btnLoginEntrar;
        private TextBox txtLoginSenha;
        private TextBox txtLoginUsuario;
        private Label lbLoginExit;
        private Panel panel2;
        private Panel panel3;
        private PictureBox picLoginIconUser;
        private PictureBox picLoginNaoVerSenha;
        private Label lbCaps;
        private Label lblRedefinirSenha;

        //CONTROLES
        private Label lblInstrucoesRedefinicao;
        private TextBox txtLoginNovaSenha;
        private TextBox txtLoginConfirmaSenha;
        private Button btnSalvarSenha;
        private Button btnCancelarRedefinicao;
    }
}