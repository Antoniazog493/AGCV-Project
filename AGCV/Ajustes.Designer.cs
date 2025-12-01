namespace AGCV
{
    partial class Ajustes
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
            lblTituloText = new Label();
            panelContent = new Panel();
            pnlAdministrarUsuarios = new Panel();
            btnAdministrarUsuarios = new Button();
            lblAdministrarUsuariosDescripcion = new Label();
            lblAdministrarUsuariosTitulo = new Label();
            pnlHistorial = new Panel();
            btnHistorial = new Button();
            lblHistorialDescripcion = new Label();
            lblHistorialTitulo = new Label();
            panelHeader.SuspendLayout();
            panelContent.SuspendLayout();
            pnlAdministrarUsuarios.SuspendLayout();
            pnlHistorial.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(230, 0, 18); // Rojo Nintendo Switch
            panelHeader.Controls.Add(lblTituloText);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(800, 100);
            panelHeader.TabIndex = 0;
            // 
            // lblTituloText
            // 
            lblTituloText.AutoSize = true;
            lblTituloText.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTituloText.ForeColor = Color.White; // Texto blanco sobre fondo rojo
            lblTituloText.Location = new Point(30, 30);
            lblTituloText.Name = "lblTituloText";
            lblTituloText.Size = new Size(137, 45);
            lblTituloText.TabIndex = 0;
            lblTituloText.Text = "Ajustes";
            // 
            // panelContent
            // 
            panelContent.BackColor = Color.FromArgb(245, 245, 245);
            panelContent.Controls.Add(pnlAdministrarUsuarios);
            panelContent.Controls.Add(pnlHistorial);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(0, 100);
            panelContent.Name = "panelContent";
            panelContent.Padding = new Padding(40, 30, 40, 30);
            panelContent.Size = new Size(800, 350);
            panelContent.TabIndex = 1;
            // 
            // pnlAdministrarUsuarios
            // 
            pnlAdministrarUsuarios.BackColor = Color.White;
            pnlAdministrarUsuarios.BorderStyle = BorderStyle.FixedSingle;
            pnlAdministrarUsuarios.Controls.Add(btnAdministrarUsuarios);
            pnlAdministrarUsuarios.Controls.Add(lblAdministrarUsuariosDescripcion);
            pnlAdministrarUsuarios.Controls.Add(lblAdministrarUsuariosTitulo);
            pnlAdministrarUsuarios.Location = new Point(420, 50);
            pnlAdministrarUsuarios.Name = "pnlAdministrarUsuarios";
            pnlAdministrarUsuarios.Size = new Size(340, 250);
            pnlAdministrarUsuarios.TabIndex = 2;
            pnlAdministrarUsuarios.Visible = false;
            // 
            // btnAdministrarUsuarios
            // 
            btnAdministrarUsuarios.BackColor = Color.FromArgb(230, 0, 18); // Rojo Nintendo Switch
            btnAdministrarUsuarios.Cursor = Cursors.Hand;
            btnAdministrarUsuarios.FlatAppearance.BorderSize = 0;
            btnAdministrarUsuarios.FlatStyle = FlatStyle.Flat;
            btnAdministrarUsuarios.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnAdministrarUsuarios.ForeColor = Color.White;
            btnAdministrarUsuarios.Location = new Point(20, 195);
            btnAdministrarUsuarios.Name = "btnAdministrarUsuarios";
            btnAdministrarUsuarios.Size = new Size(298, 40);
            btnAdministrarUsuarios.TabIndex = 2;
            btnAdministrarUsuarios.Text = "Gestionar Usuarios";
            btnAdministrarUsuarios.UseVisualStyleBackColor = false;
            btnAdministrarUsuarios.Click += btnAdministrarUsuarios_Click;
            // 
            // lblAdministrarUsuariosDescripcion
            // 
            lblAdministrarUsuariosDescripcion.Font = new Font("Segoe UI", 10F);
            lblAdministrarUsuariosDescripcion.ForeColor = Color.FromArgb(127, 140, 141);
            lblAdministrarUsuariosDescripcion.Location = new Point(20, 65);
            lblAdministrarUsuariosDescripcion.Name = "lblAdministrarUsuariosDescripcion";
            lblAdministrarUsuariosDescripcion.Size = new Size(298, 115);
            lblAdministrarUsuariosDescripcion.TabIndex = 1;
            lblAdministrarUsuariosDescripcion.Text = "Gestiona usuarios del sistema.\r\n\r\nCambia contraseñas, roles y permisos de acceso.";
            // 
            // lblAdministrarUsuariosTitulo
            // 
            lblAdministrarUsuariosTitulo.AutoSize = true;
            lblAdministrarUsuariosTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblAdministrarUsuariosTitulo.ForeColor = Color.FromArgb(44, 62, 80); // Gris oscuro legible
            lblAdministrarUsuariosTitulo.Location = new Point(20, 20);
            lblAdministrarUsuariosTitulo.Name = "lblAdministrarUsuariosTitulo";
            lblAdministrarUsuariosTitulo.Size = new Size(226, 25);
            lblAdministrarUsuariosTitulo.TabIndex = 0;
            lblAdministrarUsuariosTitulo.Text = "Administrar Usuarios";
            // 
            // pnlHistorial
            // 
            pnlHistorial.BackColor = Color.White;
            pnlHistorial.BorderStyle = BorderStyle.FixedSingle;
            pnlHistorial.Controls.Add(btnHistorial);
            pnlHistorial.Controls.Add(lblHistorialDescripcion);
            pnlHistorial.Controls.Add(lblHistorialTitulo);
            pnlHistorial.Location = new Point(40, 50);
            pnlHistorial.Name = "pnlHistorial";
            pnlHistorial.Size = new Size(340, 250);
            pnlHistorial.TabIndex = 1;
            // 
            // btnHistorial
            // 
            btnHistorial.BackColor = Color.FromArgb(0, 150, 215); // Azul Nintendo Switch
            btnHistorial.Cursor = Cursors.Hand;
            btnHistorial.FlatAppearance.BorderSize = 0;
            btnHistorial.FlatStyle = FlatStyle.Flat;
            btnHistorial.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnHistorial.ForeColor = Color.White;
            btnHistorial.Location = new Point(20, 195);
            btnHistorial.Name = "btnHistorial";
            btnHistorial.Size = new Size(298, 40);
            btnHistorial.TabIndex = 2;
            btnHistorial.Text = "Ver Historial";
            btnHistorial.UseVisualStyleBackColor = false;
            btnHistorial.Click += button2_Click;
            // 
            // lblHistorialDescripcion
            // 
            lblHistorialDescripcion.Font = new Font("Segoe UI", 10F);
            lblHistorialDescripcion.ForeColor = Color.FromArgb(127, 140, 141);
            lblHistorialDescripcion.Location = new Point(20, 65);
            lblHistorialDescripcion.Name = "lblHistorialDescripcion";
            lblHistorialDescripcion.Size = new Size(298, 115);
            lblHistorialDescripcion.TabIndex = 1;
            lblHistorialDescripcion.Text = "Visualiza el registro completo de todas tus conexiones de Joy-Cons.\r\n\r\nConsulta fechas, controles conectados y estado de cada sesión.";
            // 
            // lblHistorialTitulo
            // 
            lblHistorialTitulo.AutoSize = true;
            lblHistorialTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblHistorialTitulo.ForeColor = Color.FromArgb(44, 62, 80); // Gris oscuro legible
            lblHistorialTitulo.Location = new Point(20, 20);
            lblHistorialTitulo.Name = "lblHistorialTitulo";
            lblHistorialTitulo.Size = new Size(227, 25);
            lblHistorialTitulo.TabIndex = 0;
            lblHistorialTitulo.Text = "Historial de Conexiones";
            lblHistorialTitulo.Click += lblHistorialTitulo_Click;
            // 
            // Ajustes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(800, 450);
            Controls.Add(panelContent);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Ajustes";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AGCV - Ajustes y Configuración";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelContent.ResumeLayout(false);
            pnlAdministrarUsuarios.ResumeLayout(false);
            pnlAdministrarUsuarios.PerformLayout();
            pnlHistorial.ResumeLayout(false);
            pnlHistorial.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblTituloText;
        private Panel panelContent;
        private Panel pnlHistorial;
        private Label lblHistorialTitulo;
        private Label lblHistorialDescripcion;
        private Button btnHistorial;
        private Panel pnlAdministrarUsuarios;
        private Button btnAdministrarUsuarios;
        private Label lblAdministrarUsuariosDescripcion;
        private Label lblAdministrarUsuariosTitulo;
    }
}