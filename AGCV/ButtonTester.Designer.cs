namespace AGCV
{
    partial class ButtonTester
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
            panelHeader = new Panel();
            lblTitulo = new Label();
            lblStatus = new Label();
            lblEventCount = new Label();
            panelMain = new Panel();
            txtLog = new RichTextBox();
            panelFooter = new Panel();
            btnClear = new Button();
            btnExport = new Button();
            btnGetInfo = new Button();
            btnClose = new Button();
            panelHeader.SuspendLayout();
            panelMain.SuspendLayout();
            panelFooter.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(52, 73, 94);
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Controls.Add(lblStatus);
            panelHeader.Controls.Add(lblEventCount);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1200, 80);
            panelHeader.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoEllipsis = true;
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(20, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(376, 41);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Joy-Con Event Monitor";
            lblTitulo.Click += lblTitulo_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblStatus.ForeColor = Color.LightGray;
            lblStatus.Location = new Point(20, 50);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(152, 25);
            lblStatus.TabIndex = 1;
            lblStatus.Text = "Inicializando...";
            // 
            // lblEventCount
            // 
            lblEventCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblEventCount.Font = new Font("Consolas", 11F, FontStyle.Bold);
            lblEventCount.ForeColor = Color.FromArgb(52, 152, 219);
            lblEventCount.Location = new Point(1000, 30);
            lblEventCount.Name = "lblEventCount";
            lblEventCount.Size = new Size(180, 25);
            lblEventCount.TabIndex = 2;
            lblEventCount.Text = "Events: 0";
            lblEventCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.FromArgb(30, 30, 30);
            panelMain.Controls.Add(txtLog);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 80);
            panelMain.Name = "panelMain";
            panelMain.Padding = new Padding(10);
            panelMain.Size = new Size(1200, 500);
            panelMain.TabIndex = 0;
            // 
            // txtLog
            // 
            txtLog.BackColor = Color.FromArgb(30, 30, 30);
            txtLog.BorderStyle = BorderStyle.None;
            txtLog.Dock = DockStyle.Fill;
            txtLog.Font = new Font("Consolas", 10F);
            txtLog.ForeColor = Color.FromArgb(200, 200, 200);
            txtLog.Location = new Point(10, 10);
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.Size = new Size(1180, 480);
            txtLog.TabIndex = 0;
            txtLog.Text = "";
            txtLog.WordWrap = false;
            // 
            // panelFooter
            // 
            panelFooter.BackColor = Color.FromArgb(236, 240, 241);
            panelFooter.Controls.Add(btnClear);
            panelFooter.Controls.Add(btnExport);
            panelFooter.Controls.Add(btnGetInfo);
            panelFooter.Controls.Add(btnClose);
            panelFooter.Dock = DockStyle.Bottom;
            panelFooter.Location = new Point(0, 580);
            panelFooter.Name = "panelFooter";
            panelFooter.Padding = new Padding(20, 15, 20, 15);
            panelFooter.Size = new Size(1200, 70);
            panelFooter.TabIndex = 1;
            // 
            // btnClear
            // 
            btnClear.BackColor = Color.FromArgb(231, 76, 60);
            btnClear.Cursor = Cursors.Hand;
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnClear.ForeColor = Color.White;
            btnClear.Location = new Point(20, 15);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(140, 40);
            btnClear.TabIndex = 0;
            btnClear.Text = "Clear Log";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += btnClear_Click;
            // 
            // btnExport
            // 
            btnExport.BackColor = Color.FromArgb(52, 152, 219);
            btnExport.Cursor = Cursors.Hand;
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnExport.ForeColor = Color.White;
            btnExport.Location = new Point(170, 15);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(140, 40);
            btnExport.TabIndex = 1;
            btnExport.Text = "Export";
            btnExport.UseVisualStyleBackColor = false;
            btnExport.Click += btnExport_Click;
            // 
            // btnGetInfo
            // 
            btnGetInfo.BackColor = Color.FromArgb(155, 89, 182);
            btnGetInfo.Cursor = Cursors.Hand;
            btnGetInfo.FlatAppearance.BorderSize = 0;
            btnGetInfo.FlatStyle = FlatStyle.Flat;
            btnGetInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnGetInfo.ForeColor = Color.White;
            btnGetInfo.Location = new Point(320, 15);
            btnGetInfo.Name = "btnGetInfo";
            btnGetInfo.Size = new Size(180, 40);
            btnGetInfo.TabIndex = 2;
            btnGetInfo.Text = "Controller Info";
            btnGetInfo.UseVisualStyleBackColor = false;
            btnGetInfo.Click += btnGetInfo_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.FromArgb(127, 140, 141);
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(1040, 15);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(140, 40);
            btnClose.TabIndex = 3;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // ButtonTester
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1200, 650);
            Controls.Add(panelMain);
            Controls.Add(panelHeader);
            Controls.Add(panelFooter);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "ButtonTester";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AGCV - Joy-Con Event Monitor";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelMain.ResumeLayout(false);
            panelFooter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblTitulo;
        private Label lblStatus;
        private Label lblEventCount;
        private Panel panelMain;
        private RichTextBox txtLog;
        private Panel panelFooter;
        private Button btnClear;
        private Button btnExport;
        private Button btnGetInfo;
        private Button btnClose;
    }
}
