namespace RAW_Edit
{
    partial class frmSettings
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
            tabSettingsMain = new TabControl();
            tabPage1 = new TabPage();
            btnClose = new Button();
            groupBox1 = new GroupBox();
            btnColorMatrixFileClear = new Button();
            btnSelectColorMatrixFile = new Button();
            txtPathColorMatrixFile = new TextBox();
            lblColorMatrix = new Label();
            tabPage2 = new TabPage();
            dlgOpenfile = new OpenFileDialog();
            tabSettingsMain.SuspendLayout();
            tabPage1.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // tabSettingsMain
            // 
            tabSettingsMain.Controls.Add(tabPage1);
            tabSettingsMain.Controls.Add(tabPage2);
            tabSettingsMain.Dock = DockStyle.Fill;
            tabSettingsMain.Location = new Point(0, 0);
            tabSettingsMain.Name = "tabSettingsMain";
            tabSettingsMain.SelectedIndex = 0;
            tabSettingsMain.Size = new Size(1054, 729);
            tabSettingsMain.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(btnClose);
            tabPage1.Controls.Add(groupBox1);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1046, 701);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "File Path";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(494, 652);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 23);
            btnClose.TabIndex = 1;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnColorMatrixFileClear);
            groupBox1.Controls.Add(btnSelectColorMatrixFile);
            groupBox1.Controls.Add(txtPathColorMatrixFile);
            groupBox1.Controls.Add(lblColorMatrix);
            groupBox1.Location = new Point(8, 6);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1030, 617);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Defaults";
            // 
            // btnColorMatrixFileClear
            // 
            btnColorMatrixFileClear.Location = new Point(949, 36);
            btnColorMatrixFileClear.Name = "btnColorMatrixFileClear";
            btnColorMatrixFileClear.Size = new Size(75, 23);
            btnColorMatrixFileClear.TabIndex = 3;
            btnColorMatrixFileClear.Text = "Clear";
            btnColorMatrixFileClear.UseVisualStyleBackColor = true;
            btnColorMatrixFileClear.Click += btnColorMatrixFileClear_Click;
            // 
            // btnSelectColorMatrixFile
            // 
            btnSelectColorMatrixFile.Location = new Point(868, 36);
            btnSelectColorMatrixFile.Name = "btnSelectColorMatrixFile";
            btnSelectColorMatrixFile.Size = new Size(75, 23);
            btnSelectColorMatrixFile.TabIndex = 2;
            btnSelectColorMatrixFile.Text = "Select";
            btnSelectColorMatrixFile.UseVisualStyleBackColor = true;
            btnSelectColorMatrixFile.Click += btnSelectColorMatrixFile_Click;
            // 
            // txtPathColorMatrixFile
            // 
            txtPathColorMatrixFile.Location = new Point(149, 37);
            txtPathColorMatrixFile.Name = "txtPathColorMatrixFile";
            txtPathColorMatrixFile.ReadOnly = true;
            txtPathColorMatrixFile.Size = new Size(713, 23);
            txtPathColorMatrixFile.TabIndex = 1;
            // 
            // lblColorMatrix
            // 
            lblColorMatrix.AutoSize = true;
            lblColorMatrix.Location = new Point(15, 40);
            lblColorMatrix.Name = "lblColorMatrix";
            lblColorMatrix.Size = new Size(70, 15);
            lblColorMatrix.TabIndex = 0;
            lblColorMatrix.Text = "ColorMatrix";
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1046, 701);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // dlgOpenfile
            // 
            dlgOpenfile.FileName = "openFileDialog1";
            // 
            // frmSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1054, 729);
            Controls.Add(tabSettingsMain);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmSettings";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Program Settings";
            Load += frmSettings_Load;
            tabSettingsMain.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabSettingsMain;
        private TabPage tabPage1;
        private GroupBox groupBox1;
        private Label lblColorMatrix;
        private TabPage tabPage2;
        private Button btnSelectColorMatrixFile;
        private TextBox txtPathColorMatrixFile;
        private OpenFileDialog dlgOpenfile;
        private Button btnClose;
        private Button btnColorMatrixFileClear;
    }
}