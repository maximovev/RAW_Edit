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
            tabPreview.Location = new Point(86, 102);
            tabPreview.Name = "tabPreview";
            tabPreview.SelectedIndex = 0;
            tabPreview.Size = new Size(863, 499);
            tabPreview.TabIndex = 0;
            // 
            // tabRAW1
            // 
            tabRAW1.Controls.Add(picPreview);
            tabRAW1.Location = new Point(4, 24);
            tabRAW1.Name = "tabRAW1";
            tabRAW1.Padding = new Padding(3);
            tabRAW1.Size = new Size(855, 471);
            tabRAW1.TabIndex = 0;
            tabRAW1.Text = "RAW";
            tabRAW1.UseVisualStyleBackColor = true;
            // 
            // picPreview
            // 
            picPreview.Location = new Point(280, 189);
            picPreview.Name = "picPreview";
            picPreview.Size = new Size(447, 231);
            picPreview.TabIndex = 0;
            picPreview.TabStop = false;
            picPreview.Click += picRAWPreview_Click;
            picPreview.Paint += picRAWPreview_Paint;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(855, 471);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // vsbPreview
            // 
            vsbPreview.Location = new Point(1233, 300);
            vsbPreview.Name = "vsbPreview";
            vsbPreview.Size = new Size(17, 282);
            vsbPreview.TabIndex = 1;
            vsbPreview.Scroll += vsbPreview_Scroll;
            // 
            // hsbPreview
            // 
            hsbPreview.Location = new Point(381, 701);
            hsbPreview.Name = "hsbPreview";
            hsbPreview.Size = new Size(298, 17);
            hsbPreview.TabIndex = 2;
            hsbPreview.Scroll += hsbPreview_Scroll;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { editToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1377, 24);
            menuStrip1.TabIndex = 3;
            menuStrip1.Text = "menuStrip1";
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(45, 20);
            editToolStripMenuItem.Text = "Edit1";
            // 
            // frmEdit
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1377, 869);
            Controls.Add(hsbPreview);
            Controls.Add(vsbPreview);
            Controls.Add(tabPreview);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "frmEdit";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Edit Image";
            WindowState = FormWindowState.Maximized;
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