namespace AGCV
{
    partial class AdministrarUsuarios
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
            dgvUsuarios = new DataGridView();
            btnCambiarContraseña = new Button();
            btnCambiarRol = new Button();
            btnActivarDesactivar = new Button();
            btnEliminar = new Button();
            btnRefrescar = new Button();
            btnCerrar = new Button();
            lblTotalUsuarios = new Label();
            panel1 = new Panel();
            panel2 = new Panel();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(900, 62);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Administrar Usuarios";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // dgvUsuarios
            // 
            dgvUsuarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvUsuarios.BackgroundColor = Color.White;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsuarios.Location = new Point(20, 88);
            dgvUsuarios.Margin = new Padding(3, 4, 3, 4);
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.RowHeadersWidth = 51;
            dgvUsuarios.Size = new Size(860, 475);
            dgvUsuarios.TabIndex = 1;
            // 
            // btnCambiarContraseña
            // 
            btnCambiarContraseña.BackColor = Color.FromArgb(0, 150, 215);
            btnCambiarContraseña.Cursor = Cursors.Hand;
            btnCambiarContraseña.FlatAppearance.BorderSize = 0;
            btnCambiarContraseña.FlatStyle = FlatStyle.Flat;
            btnCambiarContraseña.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCambiarContraseña.ForeColor = Color.White;
            btnCambiarContraseña.Location = new Point(20, 12);
            btnCambiarContraseña.Margin = new Padding(3, 4, 3, 4);
            btnCambiarContraseña.Name = "btnCambiarContraseña";
            btnCambiarContraseña.Size = new Size(182, 50);
            btnCambiarContraseña.TabIndex = 2;
            btnCambiarContraseña.Text = "Cambiar Contraseña";
            btnCambiarContraseña.UseVisualStyleBackColor = false;
            btnCambiarContraseña.Click += btnCambiarContraseña_Click;
            // 
            // btnCambiarRol
            // 
            btnCambiarRol.BackColor = Color.FromArgb(230, 0, 18);
            btnCambiarRol.Cursor = Cursors.Hand;
            btnCambiarRol.FlatAppearance.BorderSize = 0;
            btnCambiarRol.FlatStyle = FlatStyle.Flat;
            btnCambiarRol.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCambiarRol.ForeColor = Color.White;
            btnCambiarRol.Location = new Point(208, 12);
            btnCambiarRol.Margin = new Padding(3, 4, 3, 4);
            btnCambiarRol.Name = "btnCambiarRol";
            btnCambiarRol.Size = new Size(127, 50);
            btnCambiarRol.TabIndex = 3;
            btnCambiarRol.Text = "Cambiar Rol";
            btnCambiarRol.UseVisualStyleBackColor = false;
            btnCambiarRol.Click += btnCambiarRol_Click;
            // 
            // btnActivarDesactivar
            // 
            btnActivarDesactivar.BackColor = Color.FromArgb(0, 150, 215);
            btnActivarDesactivar.Cursor = Cursors.Hand;
            btnActivarDesactivar.FlatAppearance.BorderSize = 0;
            btnActivarDesactivar.FlatStyle = FlatStyle.Flat;
            btnActivarDesactivar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnActivarDesactivar.ForeColor = Color.White;
            btnActivarDesactivar.Location = new Point(341, 12);
            btnActivarDesactivar.Margin = new Padding(3, 4, 3, 4);
            btnActivarDesactivar.Name = "btnActivarDesactivar";
            btnActivarDesactivar.Size = new Size(171, 50);
            btnActivarDesactivar.TabIndex = 4;
            btnActivarDesactivar.Text = "Activar/Desactivar";
            btnActivarDesactivar.UseVisualStyleBackColor = false;
            btnActivarDesactivar.Click += btnActivarDesactivar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.BackColor = Color.FromArgb(230, 0, 18);
            btnEliminar.Cursor = Cursors.Hand;
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Location = new Point(518, 12);
            btnEliminar.Margin = new Padding(3, 4, 3, 4);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(150, 50);
            btnEliminar.TabIndex = 5;
            btnEliminar.Text = "Eliminar Usuario";
            btnEliminar.UseVisualStyleBackColor = false;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // btnRefrescar
            // 
            btnRefrescar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefrescar.BackColor = Color.FromArgb(0, 150, 215);
            btnRefrescar.Cursor = Cursors.Hand;
            btnRefrescar.FlatAppearance.BorderSize = 0;
            btnRefrescar.FlatStyle = FlatStyle.Flat;
            btnRefrescar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRefrescar.ForeColor = Color.White;
            btnRefrescar.Location = new Point(674, 12);
            btnRefrescar.Margin = new Padding(3, 4, 3, 4);
            btnRefrescar.Name = "btnRefrescar";
            btnRefrescar.Size = new Size(100, 50);
            btnRefrescar.TabIndex = 6;
            btnRefrescar.Text = "Refrescar";
            btnRefrescar.UseVisualStyleBackColor = false;
            btnRefrescar.Click += btnRefrescar_Click;
            // 
            // btnCerrar
            // 
            btnCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrar.BackColor = Color.FromArgb(127, 140, 141);
            btnCerrar.Cursor = Cursors.Hand;
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Location = new Point(780, 12);
            btnCerrar.Margin = new Padding(3, 4, 3, 4);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 50);
            btnCerrar.TabIndex = 7;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // lblTotalUsuarios
            // 
            lblTotalUsuarios.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblTotalUsuarios.AutoSize = true;
            lblTotalUsuarios.Font = new Font("Segoe UI", 10F);
            lblTotalUsuarios.ForeColor = Color.FromArgb(127, 140, 141);
            lblTotalUsuarios.Location = new Point(20, 575);
            lblTotalUsuarios.Name = "lblTotalUsuarios";
            lblTotalUsuarios.Size = new Size(156, 23);
            lblTotalUsuarios.TabIndex = 8;
            lblTotalUsuarios.Text = "Total de usuarios: 0";
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(236, 240, 241);
            panel1.Controls.Add(btnCambiarContraseña);
            panel1.Controls.Add(btnCambiarRol);
            panel1.Controls.Add(btnActivarDesactivar);
            panel1.Controls.Add(btnEliminar);
            panel1.Controls.Add(btnRefrescar);
            panel1.Controls.Add(btnCerrar);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 613);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(900, 75);
            panel1.TabIndex = 9;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(230, 0, 18);
            panel2.Controls.Add(lblTitulo);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Margin = new Padding(3, 4, 3, 4);
            panel2.Name = "panel2";
            panel2.Size = new Size(900, 62);
            panel2.TabIndex = 10;
            // 
            // AdministrarUsuarios
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(900, 688);
            Controls.Add(panel2);
            Controls.Add(lblTotalUsuarios);
            Controls.Add(dgvUsuarios);
            Controls.Add(panel1);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(900, 676);
            Name = "AdministrarUsuarios";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Administrar Usuarios - AGCV";
            Load += AdministrarUsuarios_Load;
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.DataGridView dgvUsuarios;
        private System.Windows.Forms.Button btnCambiarContraseña;
        private System.Windows.Forms.Button btnCambiarRol;
        private System.Windows.Forms.Button btnActivarDesactivar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnRefrescar;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Label lblTotalUsuarios;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}
