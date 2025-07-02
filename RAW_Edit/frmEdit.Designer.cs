namespace RAW_Edit
{
    partial class frmEdit
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
            tabPreview = new TabControl();
            tabRAW1 = new TabPage();
            picPreview = new PictureBox();
            tabPage2 = new TabPage();
            vsbPreview = new VScrollBar();
            hsbPreview = new HScrollBar();
            menuStrip1 = new MenuStrip();
            editToolStripMenuItem = new ToolStripMenuItem();
            tabPreview.SuspendLayout();
            tabRAW1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picPreview).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // tabPreview
            // 
            tabPreview.Controls.Add(tabRAW1);
            tabPreview.Controls.Add(tabPage2);
            tabPreview.Location = new Point(98, 136);
            tabPreview.Margin = new Padding(3, 4, 3, 4);
            tabPreview.Name = "tabPreview";
            tabPreview.SelectedIndex = 0;
            tabPreview.Size = new Size(986, 665);
            tabPreview.TabIndex = 0;
            // 
            // tabRAW1
            // 
            tabRAW1.Controls.Add(picPreview);
            tabRAW1.Location = new Point(4, 29);
            tabRAW1.Margin = new Padding(3, 4, 3, 4);
            tabRAW1.Name = "tabRAW1";
            tabRAW1.Padding = new Padding(3, 4, 3, 4);
            tabRAW1.Size = new Size(978, 632);
            tabRAW1.TabIndex = 0;
            tabRAW1.Text = "RAW";
            tabRAW1.UseVisualStyleBackColor = true;
            // 
            // picPreview
            // 
            picPreview.Location = new Point(320, 252);
            picPreview.Margin = new Padding(3, 4, 3, 4);
            picPreview.Name = "picPreview";
            picPreview.Size = new Size(511, 308);
            picPreview.TabIndex = 0;
            picPreview.TabStop = false;
            picPreview.Click += picRAWPreview_Click;
            picPreview.Paint += picRAWPreview_Paint;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 29);
            tabPage2.Margin = new Padding(3, 4, 3, 4);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3, 4, 3, 4);
            tabPage2.Size = new Size(978, 632);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // vsbPreview
            // 
            vsbPreview.Location = new Point(1409, 400);
            vsbPreview.Name = "vsbPreview";
            vsbPreview.Size = new Size(17, 376);
            vsbPreview.TabIndex = 1;
            vsbPreview.Scroll += vsbPreview_Scroll;
            // 
            // hsbPreview
            // 
            hsbPreview.Location = new Point(439, 928);
            hsbPreview.Name = "hsbPreview";
            hsbPreview.Size = new Size(341, 17);
            hsbPreview.TabIndex = 2;
            hsbPreview.Scroll += hsbPreview_Scroll;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { editToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(7, 3, 0, 3);
            menuStrip1.Size = new Size(1574, 30);
            menuStrip1.TabIndex = 3;
            menuStrip1.Text = "menuStrip1";
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(57, 24);
            editToolStripMenuItem.Text = "Edit1";
            // 
            // frmEdit
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1574, 1055);
            Controls.Add(hsbPreview);
            Controls.Add(vsbPreview);
            Controls.Add(tabPreview);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(3, 4, 3, 4);
            Name = "frmEdit";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Edit Image";
            WindowState = FormWindowState.Maximized;
            FormClosed += frmEdit_FormClosed;
            Load += frmEdit_Load;
            ResizeEnd += frmEdit_ResizeEnd;
            Paint += frmEdit_Paint;
            Resize += frmEdit_Resize;
            tabPreview.ResumeLayout(false);
            tabRAW1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picPreview).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl tabPreview;
        private TabPage tabRAW1;
        private TabPage tabPage2;
        private VScrollBar vsbPreview;
        private HScrollBar hsbPreview;
        private PictureBox picPreview;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem editToolStripMenuItem;
    }
}