namespace AGCV
{
    partial class LogIn
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogIn));
            panelIzquierdo = new Panel();
            lblDescripcion = new Label();
            panelDerecho = new Panel();
            linkCrearCuenta = new LinkLabel();
            lblNuevaCuenta = new Label();
            btnIniciar = new Button();
            lblOlvidaste = new LinkLabel();
            txtContraseña = new TextBox();
            lblContraseña = new Label();
            txtUsuario = new TextBox();
            lblUsuario = new Label();
            lblTitulo = new Label();
            pictureBox1 = new PictureBox();
            panelIzquierdo.SuspendLayout();
            panelDerecho.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panelIzquierdo
            // 
            panelIzquierdo.BackColor = Color.FromArgb(52, 73, 94);
            panelIzquierdo.Controls.Add(pictureBox1);
            panelIzquierdo.Controls.Add(lblDescripcion);
            panelIzquierdo.Dock = DockStyle.Left;
            panelIzquierdo.Location = new Point(0, 0);
            panelIzquierdo.Name = "panelIzquierdo";
            panelIzquierdo.Padding = new Padding(30);
            panelIzquierdo.Size = new Size(380, 500);
            panelIzquierdo.TabIndex = 0;
            // 
            // lblDescripcion
            // 
            lblDescripcion.Font = new Font("Segoe UI", 11F);
            lblDescripcion.ForeColor = Color.FromArgb(189, 195, 199);
            lblDescripcion.Location = new Point(27, 294);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(320, 139);
            lblDescripcion.TabIndex = 2;
            lblDescripcion.Text = "Gestiona tus Joy-Cons de Nintendo Switch.\r\n\r\nSoluciona problemas de reconocimiento en Windows.\r\n";
            lblDescripcion.Click += lblDescripcion_Click;
            // 
            // panelDerecho
            // 
            panelDerecho.BackColor = Color.White;
            panelDerecho.Controls.Add(linkCrearCuenta);
            panelDerecho.Controls.Add(lblNuevaCuenta);
            panelDerecho.Controls.Add(btnIniciar);
            panelDerecho.Controls.Add(lblOlvidaste);
            panelDerecho.Controls.Add(txtContraseña);
            panelDerecho.Controls.Add(lblContraseña);
            panelDerecho.Controls.Add(txtUsuario);
            panelDerecho.Controls.Add(lblUsuario);
            panelDerecho.Controls.Add(lblTitulo);
            panelDerecho.Dock = DockStyle.Fill;
            panelDerecho.Location = new Point(380, 0);
            panelDerecho.Name = "panelDerecho";
            panelDerecho.Padding = new Padding(40, 60, 40, 40);
            panelDerecho.Size = new Size(420, 500);
            panelDerecho.TabIndex = 1;
            // 
            // linkCrearCuenta
            // 
            linkCrearCuenta.AutoSize = true;
            linkCrearCuenta.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            linkCrearCuenta.LinkColor = Color.FromArgb(46, 204, 113);
            linkCrearCuenta.Location = new Point(236, 410);
            linkCrearCuenta.Name = "linkCrearCuenta";
            linkCrearCuenta.Size = new Size(127, 23);
            linkCrearCuenta.TabIndex = 4;
            linkCrearCuenta.TabStop = true;
            linkCrearCuenta.Text = "Crear una aquí";
            linkCrearCuenta.LinkClicked += linkLabel1_LinkClicked;
            // 
            // lblNuevaCuenta
            // 
            lblNuevaCuenta.AutoSize = true;
            lblNuevaCuenta.Font = new Font("Segoe UI", 10F);
            lblNuevaCuenta.ForeColor = Color.FromArgb(127, 140, 141);
            lblNuevaCuenta.Location = new Point(40, 410);
            lblNuevaCuenta.Name = "lblNuevaCuenta";
            lblNuevaCuenta.Size = new Size(190, 23);
            lblNuevaCuenta.TabIndex = 5;
            lblNuevaCuenta.Text = "¿No tienes una cuenta?";
            // 
            // btnIniciar
            // 
            btnIniciar.BackColor = Color.FromArgb(46, 204, 113);
            btnIniciar.Cursor = Cursors.Hand;
            btnIniciar.FlatAppearance.BorderSize = 0;
            btnIniciar.FlatStyle = FlatStyle.Flat;
            btnIniciar.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnIniciar.ForeColor = Color.White;
            btnIniciar.Location = new Point(40, 313);
            btnIniciar.Name = "btnIniciar";
            btnIniciar.Size = new Size(340, 45);
            btnIniciar.TabIndex = 3;
            btnIniciar.Text = "Iniciar Sesión";
            btnIniciar.UseVisualStyleBackColor = false;
            btnIniciar.Click += button1_Click;
            // 
            // lblOlvidaste
            // 
            lblOlvidaste.AutoSize = true;
            lblOlvidaste.Font = new Font("Segoe UI", 10F);
            lblOlvidaste.LinkColor = Color.FromArgb(52, 152, 219);
            lblOlvidaste.Location = new Point(40, 278);
            lblOlvidaste.Name = "lblOlvidaste";
            lblOlvidaste.Size = new Size(206, 23);
            lblOlvidaste.TabIndex = 2;
            lblOlvidaste.TabStop = true;
            lblOlvidaste.Text = "¿Olvidaste tu contraseña?";
            lblOlvidaste.LinkClicked += lblOlvidaste_LinkClicked;
            // 
            // txtContraseña
            // 
            txtContraseña.BackColor = Color.FromArgb(236, 240, 241);
            txtContraseña.BorderStyle = BorderStyle.FixedSingle;
            txtContraseña.Font = new Font("Segoe UI", 11F);
            txtContraseña.Location = new Point(40, 238);
            txtContraseña.Name = "txtContraseña";
            txtContraseña.PasswordChar = '●';
            txtContraseña.PlaceholderText = "Ingresa tu contraseña";
            txtContraseña.Size = new Size(340, 32);
            txtContraseña.TabIndex = 1;
            txtContraseña.TextChanged += textBox2_TextChanged;
            // 
            // lblContraseña
            // 
            lblContraseña.AutoSize = true;
            lblContraseña.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblContraseña.ForeColor = Color.FromArgb(52, 73, 94);
            lblContraseña.Location = new Point(40, 210);
            lblContraseña.Name = "lblContraseña";
            lblContraseña.Size = new Size(132, 25);
            lblContraseña.TabIndex = 3;
            lblContraseña.Text = "Contraseña: *";
            // 
            // txtUsuario
            // 
            txtUsuario.BackColor = Color.FromArgb(236, 240, 241);
            txtUsuario.BorderStyle = BorderStyle.FixedSingle;
            txtUsuario.Font = new Font("Segoe UI", 11F);
            txtUsuario.Location = new Point(40, 155);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.PlaceholderText = "Ingresa tu usuario";
            txtUsuario.Size = new Size(340, 32);
            txtUsuario.TabIndex = 0;
            txtUsuario.TextChanged += textBox1_TextChanged;
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblUsuario.ForeColor = Color.FromArgb(52, 73, 94);
            lblUsuario.Location = new Point(40, 127);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(100, 25);
            lblUsuario.TabIndex = 1;
            lblUsuario.Text = "Usuario: *";
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(52, 73, 94);
            lblTitulo.Location = new Point(40, 60);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(233, 46);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Iniciar Sesión";
            lblTitulo.Click += lblTitulo_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.InitialImage = (Image)resources.GetObject("pictureBox1.InitialImage");
            pictureBox1.Location = new Point(59, 33);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(242, 237);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // LogIn
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(800, 500);
            Controls.Add(panelDerecho);
            Controls.Add(panelIzquierdo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LogIn";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AGCV - Aplicación de Gestión de Controles de Videojuegos";
            Load += LogIn_Load;
            panelIzquierdo.ResumeLayout(false);
            panelDerecho.ResumeLayout(false);
            panelDerecho.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        #region Fields
        private Panel panelIzquierdo;
        private Label lblDescripcion;
        private Panel panelDerecho;
        private Label lblTitulo;
        private Label lblUsuario;
        private TextBox txtUsuario;
        private Label lblContraseña;
        private TextBox txtContraseña;
        private LinkLabel lblOlvidaste;
        private Button btnIniciar;
        private Label lblNuevaCuenta;
        private LinkLabel linkCrearCuenta;
        #endregion

        private PictureBox pictureBox1;
    }
}
