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
			this.listBoxSessions = new System.Windows.Forms.ListBox();
			this.contextMenuStripSessions = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItemImport = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemExport = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItemMerge = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItemRemove = new System.Windows.Forms.ToolStripMenuItem();
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.vScrollBar = new System.Windows.Forms.VScrollBar();
			this.hScrollBar = new System.Windows.Forms.HScrollBar();
			this.contextMenuStripSessions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// listBoxSessions
			// 
			this.listBoxSessions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.listBoxSessions.ContextMenuStrip = this.contextMenuStripSessions;
			this.listBoxSessions.Dock = System.Windows.Forms.DockStyle.Left;
			this.listBoxSessions.FormattingEnabled = true;
			this.listBoxSessions.IntegralHeight = false;
			this.listBoxSessions.Location = new System.Drawing.Point(0, 0);
			this.listBoxSessions.Name = "listBoxSessions";
			this.listBoxSessions.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBoxSessions.Size = new System.Drawing.Size(255, 550);
			this.listBoxSessions.TabIndex = 0;
			this.listBoxSessions.SelectedValueChanged += new System.EventHandler(this.listBoxSessions_SelectedValueChanged);
			// 
			// contextMenuStripSessions
			// 
			this.contextMenuStripSessions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemImport,
            this.toolStripMenuItemExport,
            this.toolStripSeparator1,
            this.toolStripMenuItemMerge,
            this.toolStripSeparator2,
            this.toolStripMenuItemRemove});
			this.contextMenuStripSessions.Name = "contextMenuStrip1";
			this.contextMenuStripSessions.Size = new System.Drawing.Size(157, 104);
			this.contextMenuStripSessions.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
			// 
			// toolStripMenuItemImport
			// 
			this.toolStripMenuItemImport.Name = "toolStripMenuItemImport";
			this.toolStripMenuItemImport.Size = new System.Drawing.Size(156, 22);
			this.toolStripMenuItemImport.Text = "Import session";
			this.toolStripMenuItemImport.Click += new System.EventHandler(this.toolStripMenuItemImport_Click);
			// 
			// toolStripMenuItemExport
			// 
			this.toolStripMenuItemExport.Name = "toolStripMenuItemExport";
			this.toolStripMenuItemExport.Size = new System.Drawing.Size(156, 22);
			this.toolStripMenuItemExport.Text = "Export session...";
			this.toolStripMenuItemExport.Click += new System.EventHandler(this.toolStripMenuItemExport_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(153, 6);
			// 
			// toolStripMenuItemMerge
			// 
			this.toolStripMenuItemMerge.Name = "toolStripMenuItemMerge";
			this.toolStripMenuItemMerge.Size = new System.Drawing.Size(156, 22);
			this.toolStripMenuItemMerge.Text = "Merge";
			this.toolStripMenuItemMerge.Click += new System.EventHandler(this.toolStripMenuItemMerge_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(153, 6);
			// 
			// toolStripMenuItemRemove
			// 
			this.toolStripMenuItemRemove.Name = "toolStripMenuItemRemove";
			this.toolStripMenuItemRemove.Size = new System.Drawing.Size(156, 22);
			this.toolStripMenuItemRemove.Text = "Remove session";
			this.toolStripMenuItemRemove.Click += new System.EventHandler(this.toolStripMenuItemRemove_Click);
			// 
			// pictureBox
			// 
			this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox.Location = new System.Drawing.Point(255, 0);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(494, 534);
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
			this.hScrollBar.Location = new System.Drawing.Point(255, 534);
			this.hScrollBar.Name = "hScrollBar";
			this.hScrollBar.Size = new System.Drawing.Size(494, 16);
			this.hScrollBar.TabIndex = 3;
			this.hScrollBar.ValueChanged += new System.EventHandler(this.hScrollBar_ValueChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(765, 550);
			this.Controls.Add(this.pictureBox);
			this.Controls.Add(this.hScrollBar);
			this.Controls.Add(this.vScrollBar);
			this.Controls.Add(this.listBoxSessions);
			this.Name = "MainForm";
			this.Text = "Salem Map Tool";
			this.contextMenuStripSessions.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox listBoxSessions;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripSessions;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImport;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRemove;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExport;
		private System.Windows.Forms.VScrollBar vScrollBar;
		private System.Windows.Forms.HScrollBar hScrollBar;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMerge;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
	}
}

