namespace BoxeeStarter
{
    partial class SettingsForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.settingsButton = new System.Windows.Forms.ToolStripMenuItem();
            this.runAtStartupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitButton = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Boxee Starter";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.OnTrayIconDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsButton,
            this.runAtStartupToolStripMenuItem,
            this.exitButton});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 92);
            this.contextMenuStrip1.Opened += new System.EventHandler(this.OnRightClickMenu_Opened);
            // 
            // settingsButton
            // 
            this.settingsButton.Enabled = false;
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(152, 22);
            this.settingsButton.Text = "Settings...";
            this.settingsButton.ToolTipText = "There are no additional settings yet!\r\nPlease email boxeestarter@bakermatt.com fo" +
                "r suggestions.";
            this.settingsButton.Click += new System.EventHandler(this.OnSettingsMenuItem_Click);
            // 
            // runAtStartupToolStripMenuItem
            // 
            this.runAtStartupToolStripMenuItem.CheckOnClick = true;
            this.runAtStartupToolStripMenuItem.Name = "runAtStartupToolStripMenuItem";
            this.runAtStartupToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.runAtStartupToolStripMenuItem.Text = "Run At Startup";
            this.runAtStartupToolStripMenuItem.Click += new System.EventHandler(this.OnRunAtStartupMenuItem_Click);
            // 
            // exitButton
            // 
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(152, 22);
            this.exitButton.Text = "Exit";
            this.exitButton.Click += new System.EventHandler(this.OnExitMenuItem_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Name = "SettingsForm";
            this.Text = "Settings...";
            this.Load += new System.EventHandler(this.OnLoad);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Resize += new System.EventHandler(this.OnFormResize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsButton;
        private System.Windows.Forms.ToolStripMenuItem exitButton;
        private System.Windows.Forms.ToolStripMenuItem runAtStartupToolStripMenuItem;
    }
}

