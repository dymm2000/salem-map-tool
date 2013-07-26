namespace SalemMapTool
{
	partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.listBoxSessions = new System.Windows.Forms.ListBox();
            this.contextMenuStripSessions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemSession = new System.Windows.Forms.ToolStripMenuItem();
            this.linkLabelHome = new System.Windows.Forms.LinkLabel();
            this.pictureBox = new SalemMapTool.SessionPictureBox();
            this.menuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxSessions
            // 
            this.listBoxSessions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxSessions.ContextMenuStrip = this.contextMenuStripSessions;
            this.listBoxSessions.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBoxSessions.FormattingEnabled = true;
            this.listBoxSessions.IntegralHeight = false;
            this.listBoxSessions.Location = new System.Drawing.Point(0, 24);
            this.listBoxSessions.Name = "listBoxSessions";
            this.listBoxSessions.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxSessions.Size = new System.Drawing.Size(255, 526);
            this.listBoxSessions.TabIndex = 0;
            this.listBoxSessions.SelectedValueChanged += new System.EventHandler(this.listBoxSessions_SelectedValueChanged);
            // 
            // contextMenuStripSessions
            // 
            this.contextMenuStripSessions.Name = "contextMenuStrip1";
            this.contextMenuStripSessions.Size = new System.Drawing.Size(61, 4);
            this.contextMenuStripSessions.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSession});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(765, 24);
            this.menuStripMain.TabIndex = 5;
            this.menuStripMain.MenuActivate += new System.EventHandler(this.menuStripMain_MenuActivate);
            // 
            // toolStripMenuItemSession
            // 
            this.toolStripMenuItemSession.Name = "toolStripMenuItemSession";
            this.toolStripMenuItemSession.Size = new System.Drawing.Size(58, 20);
            this.toolStripMenuItemSession.Text = "Session";
            // 
            // linkLabelHome
            // 
            this.linkLabelHome.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabelHome.AutoSize = true;
            this.linkLabelHome.BackColor = System.Drawing.SystemColors.Control;
            this.linkLabelHome.Location = new System.Drawing.Point(647, 5);
            this.linkLabelHome.Name = "linkLabelHome";
            this.linkLabelHome.Size = new System.Drawing.Size(99, 13);
            this.linkLabelHome.TabIndex = 6;
            this.linkLabelHome.TabStop = true;
            this.linkLabelHome.Text = "Project Home Page";
            this.linkLabelHome.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelHome_LinkClicked);
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(261, 27);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(504, 523);
            this.pictureBox.TabIndex = 7;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 550);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.listBoxSessions);
            this.Controls.Add(this.linkLabelHome);
            this.Controls.Add(this.menuStripMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "MainForm";
            this.Text = "Salem Map Tool";
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.ListBox listBoxSessions;
		private System.Windows.Forms.MenuStrip menuStripMain;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSession;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripSessions;
        private System.Windows.Forms.LinkLabel linkLabelHome;
        private SessionPictureBox pictureBox;
	}
}

