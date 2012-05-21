namespace SalemElderTileMerger
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
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.vScrollBar = new System.Windows.Forms.VScrollBar();
			this.hScrollBar = new System.Windows.Forms.HScrollBar();
			this.trackBarZoom = new System.Windows.Forms.TrackBar();
			this.menuStripMain = new System.Windows.Forms.MenuStrip();
			this.toolStripMenuItemSession = new System.Windows.Forms.ToolStripMenuItem();
			this.linkLabelHome = new System.Windows.Forms.LinkLabel();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarZoom)).BeginInit();
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
			this.listBoxSessions.Size = new System.Drawing.Size(255, 510);
			this.listBoxSessions.TabIndex = 0;
			this.listBoxSessions.SelectedValueChanged += new System.EventHandler(this.listBoxSessions_SelectedValueChanged);
			// 
			// contextMenuStripSessions
			// 
			this.contextMenuStripSessions.Name = "contextMenuStrip1";
			this.contextMenuStripSessions.Size = new System.Drawing.Size(61, 4);
			this.contextMenuStripSessions.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
			// 
			// pictureBox
			// 
			this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox.Location = new System.Drawing.Point(255, 24);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(494, 510);
			this.pictureBox.TabIndex = 1;
			this.pictureBox.TabStop = false;
			this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
			this.pictureBox.Resize += new System.EventHandler(this.pictureBox_Resize);
			this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
			this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Paint);
			this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseUp);
			// 
			// vScrollBar
			// 
			this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
			this.vScrollBar.Location = new System.Drawing.Point(749, 0);
			this.vScrollBar.Name = "vScrollBar";
			this.vScrollBar.Size = new System.Drawing.Size(16, 550);
			this.vScrollBar.TabIndex = 2;
			this.vScrollBar.ValueChanged += new System.EventHandler(this.vScrollBar_ValueChanged);
			// 
			// hScrollBar
			// 
			this.hScrollBar.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.hScrollBar.Location = new System.Drawing.Point(0, 534);
			this.hScrollBar.Name = "hScrollBar";
			this.hScrollBar.Size = new System.Drawing.Size(749, 16);
			this.hScrollBar.TabIndex = 3;
			this.hScrollBar.ValueChanged += new System.EventHandler(this.hScrollBar_ValueChanged);
			// 
			// trackBarZoom
			// 
			this.trackBarZoom.Location = new System.Drawing.Point(261, 27);
			this.trackBarZoom.Name = "trackBarZoom";
			this.trackBarZoom.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.trackBarZoom.Size = new System.Drawing.Size(42, 104);
			this.trackBarZoom.TabIndex = 4;
			this.trackBarZoom.TabStop = false;
			this.trackBarZoom.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.trackBarZoom.Scroll += new System.EventHandler(this.trackBarZoom_Scroll);
			// 
			// menuStripMain
			// 
			this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSession});
			this.menuStripMain.Location = new System.Drawing.Point(0, 0);
			this.menuStripMain.Name = "menuStripMain";
			this.menuStripMain.Size = new System.Drawing.Size(749, 24);
			this.menuStripMain.TabIndex = 5;
			this.menuStripMain.MenuActivate += new System.EventHandler(this.menuStripMain_MenuActivate);
			// 
			// toolStripMenuItemSession
			// 
			this.toolStripMenuItemSession.Name = "toolStripMenuItemSession";
			this.toolStripMenuItemSession.Size = new System.Drawing.Size(55, 20);
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
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(765, 550);
			this.Controls.Add(this.linkLabelHome);
			this.Controls.Add(this.trackBarZoom);
			this.Controls.Add(this.pictureBox);
			this.Controls.Add(this.listBoxSessions);
			this.Controls.Add(this.menuStripMain);
			this.Controls.Add(this.hScrollBar);
			this.Controls.Add(this.vScrollBar);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStripMain;
			this.Name = "MainForm";
			this.Text = "Salem Map Tool";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarZoom)).EndInit();
			this.menuStripMain.ResumeLayout(false);
			this.menuStripMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox listBoxSessions;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.VScrollBar vScrollBar;
		private System.Windows.Forms.HScrollBar hScrollBar;
		private System.Windows.Forms.TrackBar trackBarZoom;
		private System.Windows.Forms.MenuStrip menuStripMain;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSession;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripSessions;
		private System.Windows.Forms.LinkLabel linkLabelHome;
	}
}

