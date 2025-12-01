namespace AGCV
{
    partial class CambiarContraseñaAdmin
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            lblUsuario = new Label();
            lblNuevaContraseña = new Label();
            txtNuevaContraseña = new TextBox();
            lblConfirmarContraseña = new Label();
            txtConfirmarContraseña = new TextBox();
            chkMostrarContraseña = new CheckBox();
            btnGuardar = new Button();
            btnCancelar = new Button();
            panelHeader = new Panel();
            panelHeader.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Fill;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(450, 62);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Cambiar Contraseña";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblUsuario
            // 
            lblUsuario.Font = new Font("Segoe UI", 10F);
            lblUsuario.ForeColor = Color.FromArgb(44, 62, 80);
            lblUsuario.Location = new Point(30, 88);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(390, 38);
            lblUsuario.TabIndex = 1;
            lblUsuario.Text = "Cambiar contraseña de:";
            lblUsuario.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNuevaContraseña
            // 
            lblNuevaContraseña.AutoSize = true;
            lblNuevaContraseña.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblNuevaContraseña.ForeColor = Color.FromArgb(44, 62, 80);
            lblNuevaContraseña.Location = new Point(30, 138);
            lblNuevaContraseña.Name = "lblNuevaContraseña";
            lblNuevaContraseña.Size = new Size(159, 23);
            lblNuevaContraseña.TabIndex = 2;
            lblNuevaContraseña.Text = "Nueva Contraseña:";
            // 
            // txtNuevaContraseña
            // 
            txtNuevaContraseña.Font = new Font("Segoe UI", 10F);
            txtNuevaContraseña.Location = new Point(30, 175);
            txtNuevaContraseña.Margin = new Padding(3, 4, 3, 4);
            txtNuevaContraseña.Name = "txtNuevaContraseña";
            txtNuevaContraseña.Size = new Size(390, 30);
            txtNuevaContraseña.TabIndex = 3;
            txtNuevaContraseña.UseSystemPasswordChar = true;
            // 
            // lblConfirmarContraseña
            // 
            lblConfirmarContraseña.AutoSize = true;
            lblConfirmarContraseña.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblConfirmarContraseña.ForeColor = Color.FromArgb(44, 62, 80);
            lblConfirmarContraseña.Location = new Point(30, 238);
            lblConfirmarContraseña.Name = "lblConfirmarContraseña";
            lblConfirmarContraseña.Size = new Size(191, 23);
            lblConfirmarContraseña.TabIndex = 4;
            lblConfirmarContraseña.Text = "Confirmar Contraseña:";
            // 
            // txtConfirmarContraseña
            // 
            txtConfirmarContraseña.Font = new Font("Segoe UI", 10F);
            txtConfirmarContraseña.Location = new Point(30, 275);
            txtConfirmarContraseña.Margin = new Padding(3, 4, 3, 4);
            txtConfirmarContraseña.Name = "txtConfirmarContraseña";
            txtConfirmarContraseña.Size = new Size(390, 30);
            txtConfirmarContraseña.TabIndex = 5;
            txtConfirmarContraseña.UseSystemPasswordChar = true;
            // 
            // chkMostrarContraseña
            // 
            chkMostrarContraseña.AutoSize = true;
            chkMostrarContraseña.Font = new Font("Segoe UI", 9F);
            chkMostrarContraseña.Location = new Point(30, 338);
            chkMostrarContraseña.Margin = new Padding(3, 4, 3, 4);
            chkMostrarContraseña.Name = "chkMostrarContraseña";
            chkMostrarContraseña.Size = new Size(158, 24);
            chkMostrarContraseña.TabIndex = 6;
            chkMostrarContraseña.Text = "Mostrar contraseña";
            chkMostrarContraseña.UseVisualStyleBackColor = true;
            chkMostrarContraseña.CheckedChanged += chkMostrarContraseña_CheckedChanged;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(0, 150, 215);
            btnGuardar.Cursor = Cursors.Hand;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(220, 400);
            btnGuardar.Margin = new Padding(3, 4, 3, 4);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(100, 50);
            btnGuardar.TabIndex = 7;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.FromArgb(230, 0, 18);
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(330, 400);
            btnCancelar.Margin = new Padding(3, 4, 3, 4);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(100, 50);
            btnCancelar.TabIndex = 8;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(230, 0, 18);
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Margin = new Padding(3, 4, 3, 4);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(450, 62);
            panelHeader.TabIndex = 9;
            // 
            // CambiarContraseñaAdmin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(450, 475);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(chkMostrarContraseña);
            Controls.Add(txtConfirmarContraseña);
            Controls.Add(lblConfirmarContraseña);
            Controls.Add(txtNuevaContraseña);
            Controls.Add(lblNuevaContraseña);
            Controls.Add(lblUsuario);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CambiarContraseñaAdmin";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cambiar Contraseña - AGCV";
            Load += CambiarContraseñaAdmin_Load;
            panelHeader.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label lblNuevaContraseña;
        private System.Windows.Forms.TextBox txtNuevaContraseña;
        private System.Windows.Forms.Label lblConfirmarContraseña;
        private System.Windows.Forms.TextBox txtConfirmarContraseña;
        private System.Windows.Forms.CheckBox chkMostrarContraseña;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Panel panelHeader;
    }
}
