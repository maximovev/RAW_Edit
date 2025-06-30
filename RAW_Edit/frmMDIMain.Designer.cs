namespace RAW_Edit
{
    partial class frmMDIMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMDIMain));
            toolStrip1 = new ToolStrip();
            toolButtonOpen = new ToolStripButton();
            tlbBtnSaveImage = new ToolStripButton();
            toolStripLabel1 = new ToolStripLabel();
            cmbDCPProfile = new ToolStripComboBox();
            toolStripLabel2 = new ToolStripLabel();
            cmbDemosaic = new ToolStripComboBox();
            toolStripLabel3 = new ToolStripLabel();
            cmbToneCurveSource = new ToolStripComboBox();
            dlgOpenFile = new OpenFileDialog();
            bwOpenFile = new System.ComponentModel.BackgroundWorker();
            statusStrip1 = new StatusStrip();
            tlbStatus = new ToolStripStatusLabel();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            toolStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolButtonOpen, tlbBtnSaveImage, toolStripLabel1, cmbDCPProfile, toolStripLabel2, cmbDemosaic, toolStripLabel3, cmbToneCurveSource });
            toolStrip1.Location = new Point(0, 24);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1357, 27);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolButtonOpen
            // 
            toolButtonOpen.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolButtonOpen.Image = (Image)resources.GetObject("toolButtonOpen.Image");
            toolButtonOpen.ImageTransparentColor = Color.Magenta;
            toolButtonOpen.Name = "toolButtonOpen";
            toolButtonOpen.Size = new Size(24, 24);
            toolButtonOpen.Text = "Open";
            toolButtonOpen.Click += toolButtonOpen_Click;
            // 
            // tlbBtnSaveImage
            // 
            tlbBtnSaveImage.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tlbBtnSaveImage.Image = (Image)resources.GetObject("tlbBtnSaveImage.Image");
            tlbBtnSaveImage.ImageTransparentColor = Color.Magenta;
            tlbBtnSaveImage.Name = "tlbBtnSaveImage";
            tlbBtnSaveImage.Size = new Size(24, 24);
            tlbBtnSaveImage.Text = "toolStripButton1";
            tlbBtnSaveImage.Click += tlbBtnSaveImage_Click;
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(67, 24);
            toolStripLabel1.Text = "DCP Profile";
            // 
            // cmbDCPProfile
            // 
            cmbDCPProfile.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDCPProfile.Name = "cmbDCPProfile";
            cmbDCPProfile.Size = new Size(121, 27);
            cmbDCPProfile.SelectedIndexChanged += cmbDCPProfile_SelectedIndexChanged;
            cmbDCPProfile.Click += cmbDCPProfile_Click;
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(59, 24);
            toolStripLabel2.Text = "Demosaic";
            // 
            // cmbDemosaic
            // 
            cmbDemosaic.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDemosaic.Name = "cmbDemosaic";
            cmbDemosaic.Size = new Size(121, 27);
            cmbDemosaic.SelectedIndexChanged += cmbDemosaic_SelectedIndexChanged;
            // 
            // toolStripLabel3
            // 
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new Size(63, 24);
            toolStripLabel3.Text = "ToneCurve";
            // 
            // cmbToneCurveSource
            // 
            cmbToneCurveSource.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbToneCurveSource.Name = "cmbToneCurveSource";
            cmbToneCurveSource.Size = new Size(121, 27);
            cmbToneCurveSource.SelectedIndexChanged += cmbToneCurveSource_SelectedIndexChanged;
            // 
            // bwOpenFile
            // 
            bwOpenFile.DoWork += bwOpenFile_DoWork;
            bwOpenFile.RunWorkerCompleted += bwOpenFile_RunWorkerCompleted;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { tlbStatus });
            statusStrip1.Location = new Point(0, 769);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1357, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // tlbStatus
            // 
            tlbStatus.Name = "tlbStatus";
            tlbStatus.Size = new Size(0, 17);
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, settingsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1357, 24);
            menuStrip1.TabIndex = 3;
            menuStrip1.Text = "menuStrip1";
            menuStrip1.ItemClicked += menuStrip1_ItemClicked;
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(39, 20);
            editToolStripMenuItem.Text = "Edit";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { optionsToolStripMenuItem });
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(61, 20);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(116, 22);
            optionsToolStripMenuItem.Text = "Options";
            optionsToolStripMenuItem.Click += optionsToolStripMenuItem_Click;
            // 
            // frmMDIMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1357, 791);
            Controls.Add(statusStrip1);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip1);
            IsMdiContainer = true;
            MainMenuStrip = menuStrip1;
            Name = "frmMDIMain";
            Text = "RAW Editor";
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip1;
        private ToolStripButton toolButtonOpen;
        private OpenFileDialog dlgOpenFile;
        private System.ComponentModel.BackgroundWorker bwOpenFile;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel tlbStatus;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripButton tlbBtnSaveImage;
        private ToolStripLabel toolStripLabel1;
        private ToolStripComboBox cmbDCPProfile;
        private ToolStripLabel toolStripLabel2;
        private ToolStripComboBox cmbDemosaic;
        private ToolStripLabel toolStripLabel3;
        private ToolStripComboBox cmbToneCurveSource;
    }
}
