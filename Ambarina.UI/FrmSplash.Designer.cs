namespace Ambarina.UI
{
    partial class FrmSplash
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            picSplashLogo = new PictureBox();
            progressBar1 = new ProgressBar();
            lbSplashCarregando = new Label();
            tmSplash = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)picSplashLogo).BeginInit();
            SuspendLayout();
            // 
            // picSplashLogo
            // 
            picSplashLogo.Image = Properties.Resources.Ativo_2;
            picSplashLogo.Location = new Point(171, 83);
            picSplashLogo.Name = "picSplashLogo";
            picSplashLogo.Size = new Size(250, 193);
            picSplashLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picSplashLogo.TabIndex = 0;
            picSplashLogo.TabStop = false;
            // 
            // progressBar1
            // 
            progressBar1.ForeColor = Color.FromArgb(15, 67, 16);
            progressBar1.Location = new Point(191, 312);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(211, 10);
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.TabIndex = 1;
            // 
            // lbSplashCarregando
            // 
            lbSplashCarregando.Font = new Font("Montserrat", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbSplashCarregando.ForeColor = Color.FromArgb(190, 138, 58);
            lbSplashCarregando.Location = new Point(191, 325);
            lbSplashCarregando.Name = "lbSplashCarregando";
            lbSplashCarregando.Size = new Size(211, 18);
            lbSplashCarregando.TabIndex = 2;
            lbSplashCarregando.Text = "Carregando...";
            lbSplashCarregando.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tmSplash
            // 
            tmSplash.Enabled = true;
            tmSplash.Tick += tmSplash_Tick;
            // 
            // FrmSplash
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(242, 233, 216);
            ClientSize = new Size(600, 400);
            Controls.Add(lbSplashCarregando);
            Controls.Add(progressBar1);
            Controls.Add(picSplashLogo);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmSplash";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)picSplashLogo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox picSplashLogo;
        private ProgressBar progressBar1;
        private Label lbSplashCarregando;
        private System.Windows.Forms.Timer tmSplash;
    }
}
