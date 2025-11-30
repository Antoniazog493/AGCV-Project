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
            pnlHistorial = new Panel();
            iconHistorial = new Label();
            btnHistorial = new Button();
            lblHistorialDescripcion = new Label();
            lblHistorialTitulo = new Label();
            panelHeader.SuspendLayout();
            panelContent.SuspendLayout();
            pnlHistorial.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(52, 73, 94);
            panelHeader.Controls.Add(lblTituloText);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Padding = new Padding(30, 25, 30, 25);
            panelHeader.Size = new Size(800, 100);
            panelHeader.TabIndex = 0;
            // 
            // lblTituloText
            // 
            lblTituloText.AutoSize = true;
            lblTituloText.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTituloText.ForeColor = Color.White;
            lblTituloText.Location = new Point(30, 25);
            lblTituloText.Name = "lblTituloText";
            lblTituloText.Size = new Size(209, 50);
            lblTituloText.TabIndex = 0;
            lblTituloText.Text = "⚙️ Ajustes";
            // 
            // panelContent
            // 
            panelContent.BackColor = Color.White;
            panelContent.Controls.Add(pnlHistorial);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(0, 100);
            panelContent.Name = "panelContent";
            panelContent.Padding = new Padding(30);
            panelContent.Size = new Size(800, 350);
            panelContent.TabIndex = 1;
            // 
            // pnlHistorial
            // 
            pnlHistorial.Anchor = AnchorStyles.None;
            pnlHistorial.BackColor = Color.FromArgb(236, 240, 241);
            pnlHistorial.BorderStyle = BorderStyle.FixedSingle;
            pnlHistorial.Controls.Add(iconHistorial);
            pnlHistorial.Controls.Add(btnHistorial);
            pnlHistorial.Controls.Add(lblHistorialDescripcion);
            pnlHistorial.Controls.Add(lblHistorialTitulo);
            pnlHistorial.Location = new Point(230, 35);
            pnlHistorial.Name = "pnlHistorial";
            pnlHistorial.Padding = new Padding(20);
            pnlHistorial.Size = new Size(340, 280);
            pnlHistorial.TabIndex = 1;
            // 
            // iconHistorial
            // 
            iconHistorial.AutoSize = true;
            iconHistorial.Font = new Font("Segoe UI", 30F);
            iconHistorial.Location = new Point(222, 120);
            iconHistorial.Name = "iconHistorial";
            iconHistorial.Size = new Size(98, 67);
            iconHistorial.TabIndex = 3;
            iconHistorial.Text = "📈";
            // 
            // btnHistorial
            // 
            btnHistorial.BackColor = Color.FromArgb(52, 152, 219);
            btnHistorial.Cursor = Cursors.Hand;
            btnHistorial.FlatAppearance.BorderSize = 0;
            btnHistorial.FlatStyle = FlatStyle.Flat;
            btnHistorial.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnHistorial.ForeColor = Color.White;
            btnHistorial.Location = new Point(20, 225);
            btnHistorial.Name = "btnHistorial";
            btnHistorial.Size = new Size(300, 40);
            btnHistorial.TabIndex = 2;
            btnHistorial.Text = "📋 Ver Historial";
            btnHistorial.UseVisualStyleBackColor = false;
            btnHistorial.Click += button2_Click;
            // 
            // lblHistorialDescripcion
            // 
            lblHistorialDescripcion.Font = new Font("Segoe UI", 11F);
            lblHistorialDescripcion.ForeColor = Color.FromArgb(127, 140, 141);
            lblHistorialDescripcion.Location = new Point(20, 60);
            lblHistorialDescripcion.Name = "lblHistorialDescripcion";
            lblHistorialDescripcion.Size = new Size(300, 100);
            lblHistorialDescripcion.TabIndex = 1;
            lblHistorialDescripcion.Text = "Visualiza el registro completo de todas tus conexiones de Joy-Cons.\r\n\r\nConsulta fechas, controles conectados y estado de cada sesión.";
            // 
            // lblHistorialTitulo
            // 
            lblHistorialTitulo.AutoSize = true;
            lblHistorialTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblHistorialTitulo.ForeColor = Color.FromArgb(52, 73, 94);
            lblHistorialTitulo.Location = new Point(10, 20);
            lblHistorialTitulo.Name = "lblHistorialTitulo";
            lblHistorialTitulo.Size = new Size(326, 32);
            lblHistorialTitulo.TabIndex = 0;
            lblHistorialTitulo.Text = "📊 Historial de Conexiones";
            lblHistorialTitulo.Click += lblHistorialTitulo_Click;
            // 
            // Ajustes
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
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
        private Label iconHistorial;
    }
}