namespace AGCV
{
    partial class HOME
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
            panelHeader = new Panel();
            lblUsuario = new Label();
            lblTitulo = new Label();
            panelMain = new Panel();
            pnlOpcion3 = new Panel();
            btnAjustesMain = new Button();
            lblDesc3 = new Label();
            lblOpcion3 = new Label();
            pnlOpcion2 = new Panel();
            btnEstadisticas = new Button();
            lblDesc2 = new Label();
            lblOpcion2 = new Label();
            pnlOpcion1 = new Panel();
            btnConectar = new Button();
            lblDesc1 = new Label();
            lblOpcion1 = new Label();
            pnlSeleccionar = new Panel();
            cmbControles = new ComboBox();
            lblSeleccionar = new Label();
            panelFooter = new Panel();
            btnCerrarSesion = new Button();
            lblVersion = new Label();
            panelHeader.SuspendLayout();
            panelMain.SuspendLayout();
            pnlOpcion3.SuspendLayout();
            pnlOpcion2.SuspendLayout();
            pnlOpcion1.SuspendLayout();
            pnlSeleccionar.SuspendLayout();
            panelFooter.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(52, 73, 94);
            panelHeader.Controls.Add(lblUsuario);
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Padding = new Padding(30, 15, 30, 15);
            panelHeader.Size = new Size(900, 90);
            panelHeader.TabIndex = 0;
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Font = new Font("Segoe UI", 10F);
            lblUsuario.ForeColor = Color.FromArgb(189, 195, 199);
            lblUsuario.Location = new Point(33, 55);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(188, 23);
            lblUsuario.TabIndex = 1;
            lblUsuario.Text = "👤 Usuario: Conectado";
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(30, 9);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(169, 46);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "🎮 AGCV";
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.White;
            panelMain.Controls.Add(pnlOpcion3);
            panelMain.Controls.Add(pnlOpcion2);
            panelMain.Controls.Add(pnlOpcion1);
            panelMain.Controls.Add(pnlSeleccionar);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 90);
            panelMain.Name = "panelMain";
            panelMain.Padding = new Padding(30);
            panelMain.Size = new Size(900, 380);
            panelMain.TabIndex = 1;
            // 
            // pnlOpcion3
            // 
            pnlOpcion3.BackColor = Color.FromArgb(236, 240, 241);
            pnlOpcion3.BorderStyle = BorderStyle.FixedSingle;
            pnlOpcion3.Controls.Add(btnAjustesMain);
            pnlOpcion3.Controls.Add(lblDesc3);
            pnlOpcion3.Controls.Add(lblOpcion3);
            pnlOpcion3.Location = new Point(610, 120);
            pnlOpcion3.Name = "pnlOpcion3";
            pnlOpcion3.Padding = new Padding(20);
            pnlOpcion3.Size = new Size(260, 230);
            pnlOpcion3.TabIndex = 3;
            // 
            // btnAjustesMain
            // 
            btnAjustesMain.BackColor = Color.FromArgb(231, 76, 60);
            btnAjustesMain.Cursor = Cursors.Hand;
            btnAjustesMain.FlatAppearance.BorderSize = 0;
            btnAjustesMain.FlatStyle = FlatStyle.Flat;
            btnAjustesMain.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAjustesMain.ForeColor = Color.White;
            btnAjustesMain.Location = new Point(20, 180);
            btnAjustesMain.Name = "btnAjustesMain";
            btnAjustesMain.Size = new Size(220, 35);
            btnAjustesMain.TabIndex = 2;
            btnAjustesMain.Text = "⚙️ Ir a Ajustes";
            btnAjustesMain.UseVisualStyleBackColor = false;
            btnAjustesMain.Click += pictureBox5_Click;
            // 
            // lblDesc3
            // 
            lblDesc3.Font = new Font("Segoe UI", 10F);
            lblDesc3.ForeColor = Color.FromArgb(127, 140, 141);
            lblDesc3.Location = new Point(20, 50);
            lblDesc3.Name = "lblDesc3";
            lblDesc3.Size = new Size(220, 100);
            lblDesc3.TabIndex = 1;
            lblDesc3.Text = "Agrega nuevos controles, accede al historial y más opciones.";
            // 
            // lblOpcion3
            // 
            lblOpcion3.AutoSize = true;
            lblOpcion3.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblOpcion3.ForeColor = Color.FromArgb(52, 73, 94);
            lblOpcion3.Location = new Point(20, 15);
            lblOpcion3.Name = "lblOpcion3";
            lblOpcion3.Size = new Size(115, 28);
            lblOpcion3.TabIndex = 0;
            lblOpcion3.Text = "🔧 Ajustes";
            // 
            // pnlOpcion2
            // 
            pnlOpcion2.BackColor = Color.FromArgb(236, 240, 241);
            pnlOpcion2.BorderStyle = BorderStyle.FixedSingle;
            pnlOpcion2.Controls.Add(btnEstadisticas);
            pnlOpcion2.Controls.Add(lblDesc2);
            pnlOpcion2.Controls.Add(lblOpcion2);
            pnlOpcion2.Location = new Point(320, 120);
            pnlOpcion2.Name = "pnlOpcion2";
            pnlOpcion2.Padding = new Padding(20);
            pnlOpcion2.Size = new Size(260, 230);
            pnlOpcion2.TabIndex = 2;
            // 
            // btnEstadisticas
            // 
            btnEstadisticas.BackColor = Color.FromArgb(52, 152, 219);
            btnEstadisticas.Cursor = Cursors.Hand;
            btnEstadisticas.FlatAppearance.BorderSize = 0;
            btnEstadisticas.FlatStyle = FlatStyle.Flat;
            btnEstadisticas.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnEstadisticas.ForeColor = Color.White;
            btnEstadisticas.Location = new Point(20, 180);
            btnEstadisticas.Name = "btnEstadisticas";
            btnEstadisticas.Size = new Size(220, 35);
            btnEstadisticas.TabIndex = 2;
            btnEstadisticas.Text = "📈 Ver Estadísticas";
            btnEstadisticas.UseVisualStyleBackColor = false;
            btnEstadisticas.Click += pictureBox3_Click;
            // 
            // lblDesc2
            // 
            lblDesc2.Font = new Font("Segoe UI", 10F);
            lblDesc2.ForeColor = Color.FromArgb(127, 140, 141);
            lblDesc2.Location = new Point(20, 50);
            lblDesc2.Name = "lblDesc2";
            lblDesc2.Size = new Size(220, 100);
            lblDesc2.TabIndex = 1;
            lblDesc2.Text = "Visualiza el uso, conexiones y rendimiento del control seleccionado.";
            // 
            // lblOpcion2
            // 
            lblOpcion2.AutoSize = true;
            lblOpcion2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblOpcion2.ForeColor = Color.FromArgb(52, 73, 94);
            lblOpcion2.Location = new Point(20, 15);
            lblOpcion2.Name = "lblOpcion2";
            lblOpcion2.Size = new Size(156, 28);
            lblOpcion2.TabIndex = 0;
            lblOpcion2.Text = "📊 Estadísticas";
            // 
            // pnlOpcion1
            // 
            pnlOpcion1.BackColor = Color.FromArgb(236, 240, 241);
            pnlOpcion1.BorderStyle = BorderStyle.FixedSingle;
            pnlOpcion1.Controls.Add(btnConectar);
            pnlOpcion1.Controls.Add(lblDesc1);
            pnlOpcion1.Controls.Add(lblOpcion1);
            pnlOpcion1.Location = new Point(30, 120);
            pnlOpcion1.Name = "pnlOpcion1";
            pnlOpcion1.Padding = new Padding(20);
            pnlOpcion1.Size = new Size(260, 230);
            pnlOpcion1.TabIndex = 1;
            // 
            // btnConectar
            // 
            btnConectar.BackColor = Color.FromArgb(46, 204, 113);
            btnConectar.Cursor = Cursors.Hand;
            btnConectar.FlatAppearance.BorderSize = 0;
            btnConectar.FlatStyle = FlatStyle.Flat;
            btnConectar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnConectar.ForeColor = Color.White;
            btnConectar.Location = new Point(20, 180);
            btnConectar.Name = "btnConectar";
            btnConectar.Size = new Size(220, 35);
            btnConectar.TabIndex = 2;
            btnConectar.Text = "✅ Conectar";
            btnConectar.UseVisualStyleBackColor = false;
            btnConectar.Click += pictureBox1_Click;
            // 
            // lblDesc1
            // 
            lblDesc1.Font = new Font("Segoe UI", 10F);
            lblDesc1.ForeColor = Color.FromArgb(127, 140, 141);
            lblDesc1.Location = new Point(20, 50);
            lblDesc1.Name = "lblDesc1";
            lblDesc1.Size = new Size(220, 100);
            lblDesc1.TabIndex = 1;
            lblDesc1.Text = "Conecta tu Joy-Con y prepárate para usarlo en Windows sin problemas de reconocimiento.";
            // 
            // lblOpcion1
            // 
            lblOpcion1.AutoSize = true;
            lblOpcion1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblOpcion1.ForeColor = Color.FromArgb(52, 73, 94);
            lblOpcion1.Location = new Point(20, 15);
            lblOpcion1.Name = "lblOpcion1";
            lblOpcion1.Size = new Size(130, 28);
            lblOpcion1.TabIndex = 0;
            lblOpcion1.Text = "🕹️ Conectar";
            // 
            // pnlSeleccionar
            // 
            pnlSeleccionar.BackColor = Color.FromArgb(236, 240, 241);
            pnlSeleccionar.BorderStyle = BorderStyle.FixedSingle;
            pnlSeleccionar.Controls.Add(cmbControles);
            pnlSeleccionar.Controls.Add(lblSeleccionar);
            pnlSeleccionar.Location = new Point(30, 20);
            pnlSeleccionar.Name = "pnlSeleccionar";
            pnlSeleccionar.Padding = new Padding(20);
            pnlSeleccionar.Size = new Size(840, 80);
            pnlSeleccionar.TabIndex = 0;
            // 
            // cmbControles
            // 
            cmbControles.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbControles.Font = new Font("Segoe UI", 11F);
            cmbControles.FormattingEnabled = true;
            cmbControles.Location = new Point(20, 37);
            cmbControles.Name = "cmbControles";
            cmbControles.Size = new Size(800, 33);
            cmbControles.TabIndex = 0;
            cmbControles.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // lblSeleccionar
            // 
            lblSeleccionar.AutoSize = true;
            lblSeleccionar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblSeleccionar.ForeColor = Color.FromArgb(52, 73, 94);
            lblSeleccionar.Location = new Point(20, 9);
            lblSeleccionar.Name = "lblSeleccionar";
            lblSeleccionar.Size = new Size(313, 25);
            lblSeleccionar.TabIndex = 0;
            lblSeleccionar.Text = "📋 Selecciona un Joy-Con";
            // 
            // panelFooter
            // 
            panelFooter.BackColor = Color.FromArgb(236, 240, 241);
            panelFooter.Controls.Add(btnCerrarSesion);
            panelFooter.Controls.Add(lblVersion);
            panelFooter.Dock = DockStyle.Bottom;
            panelFooter.Location = new Point(0, 470);
            panelFooter.Name = "panelFooter";
            panelFooter.Padding = new Padding(30, 12, 30, 12);
            panelFooter.Size = new Size(900, 60);
            panelFooter.TabIndex = 2;
            // 
            // btnCerrarSesion
            // 
            btnCerrarSesion.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrarSesion.BackColor = Color.FromArgb(127, 140, 141);
            btnCerrarSesion.Cursor = Cursors.Hand;
            btnCerrarSesion.FlatAppearance.BorderSize = 0;
            btnCerrarSesion.FlatStyle = FlatStyle.Flat;
            btnCerrarSesion.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCerrarSesion.ForeColor = Color.White;
            btnCerrarSesion.Location = new Point(727, 12);
            btnCerrarSesion.Name = "btnCerrarSesion";
            btnCerrarSesion.Size = new Size(143, 36);
            btnCerrarSesion.TabIndex = 1;
            btnCerrarSesion.Text = "Cerrar Sesión";
            btnCerrarSesion.UseVisualStyleBackColor = false;
            btnCerrarSesion.Click += btnCerrarSesion_Click;
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Font = new Font("Segoe UI", 9F);
            lblVersion.ForeColor = Color.FromArgb(127, 140, 141);
            lblVersion.Location = new Point(30, 22);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(259, 20);
            lblVersion.TabIndex = 0;
            lblVersion.Text = "AGCV v0.80 - Gestión de Joy-Cons para Switch";
            // 
            // HOME
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(900, 530);
            Controls.Add(panelMain);
            Controls.Add(panelFooter);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "HOME";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AGCV - Gestión de Joy-Cons Nintendo Switch";
            Load += HOME_Load;
            Click += HOME_Click;
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelMain.ResumeLayout(false);
            pnlOpcion3.ResumeLayout(false);
            pnlOpcion3.PerformLayout();
            pnlOpcion2.ResumeLayout(false);
            pnlOpcion2.PerformLayout();
            pnlOpcion1.ResumeLayout(false);
            pnlOpcion1.PerformLayout();
            pnlSeleccionar.ResumeLayout(false);
            pnlSeleccionar.PerformLayout();
            panelFooter.ResumeLayout(false);
            panelFooter.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblTitulo;
        private Label lblUsuario;
        private Button btnAjustes;
        private Panel panelMain;
        private Panel pnlSeleccionar;
        private Label lblSeleccionar;
        private ComboBox cmbControles;
        private Panel pnlOpcion1;
        private Label lblOpcion1;
        private Label lblDesc1;
        private Button btnConectar;
        private Panel pnlOpcion2;
        private Label lblOpcion2;
        private Label lblDesc2;
        private Button btnEstadisticas;
        private Panel pnlOpcion3;
        private Label lblOpcion3;
        private Label lblDesc3;
        private Button btnAjustesMain;
        private Panel panelFooter;
        private Label lblVersion;
        private Button btnCerrarSesion;
    }
}