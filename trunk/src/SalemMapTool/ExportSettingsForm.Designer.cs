namespace SalemMapTool
{
	partial class ExportSettingsForm
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
			this.textBoxDir = new System.Windows.Forms.TextBox();
			this.buttonDirs = new System.Windows.Forms.Button();
			this.checkBoxTiles = new System.Windows.Forms.CheckBox();
			this.checkBoxMap = new System.Windows.Forms.CheckBox();
			this.checkBoxGrid = new System.Windows.Forms.CheckBox();
			this.radioButtonPng = new System.Windows.Forms.RadioButton();
			this.radioButtonJpeg = new System.Windows.Forms.RadioButton();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// textBoxDir
			// 
			this.textBoxDir.Location = new System.Drawing.Point(12, 12);
			this.textBoxDir.Name = "textBoxDir";
			this.textBoxDir.Size = new System.Drawing.Size(330, 20);
			this.textBoxDir.TabIndex = 0;
			// 
			// buttonDirs
			// 
			this.buttonDirs.Location = new System.Drawing.Point(341, 12);
			this.buttonDirs.Name = "buttonDirs";
			this.buttonDirs.Size = new System.Drawing.Size(28, 20);
			this.buttonDirs.TabIndex = 1;
			this.buttonDirs.Text = "...";
			this.buttonDirs.UseVisualStyleBackColor = true;
			this.buttonDirs.Click += new System.EventHandler(this.buttonDirs_Click);
			// 
			// checkBoxTiles
			// 
			this.checkBoxTiles.AutoSize = true;
			this.checkBoxTiles.Checked = true;
			this.checkBoxTiles.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxTiles.Location = new System.Drawing.Point(12, 38);
			this.checkBoxTiles.Name = "checkBoxTiles";
			this.checkBoxTiles.Size = new System.Drawing.Size(81, 17);
			this.checkBoxTiles.TabIndex = 2;
			this.checkBoxTiles.Text = "Export Tiles";
			this.checkBoxTiles.UseVisualStyleBackColor = true;
			// 
			// checkBoxMap
			// 
			this.checkBoxMap.AutoSize = true;
			this.checkBoxMap.Checked = true;
			this.checkBoxMap.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxMap.Location = new System.Drawing.Point(12, 61);
			this.checkBoxMap.Name = "checkBoxMap";
			this.checkBoxMap.Size = new System.Drawing.Size(114, 17);
			this.checkBoxMap.TabIndex = 3;
			this.checkBoxMap.Text = "Export Whole Map";
			this.checkBoxMap.UseVisualStyleBackColor = true;
			this.checkBoxMap.CheckedChanged += new System.EventHandler(this.checkBoxMap_CheckedChanged);
			// 
			// checkBoxGrid
			// 
			this.checkBoxGrid.AutoSize = true;
			this.checkBoxGrid.Location = new System.Drawing.Point(27, 88);
			this.checkBoxGrid.Name = "checkBoxGrid";
			this.checkBoxGrid.Size = new System.Drawing.Size(83, 17);
			this.checkBoxGrid.TabIndex = 4;
			this.checkBoxGrid.Text = "Include Grid";
			this.checkBoxGrid.UseVisualStyleBackColor = true;
			// 
			// radioButtonPng
			// 
			this.radioButtonPng.AutoSize = true;
			this.radioButtonPng.Checked = true;
			this.radioButtonPng.Location = new System.Drawing.Point(27, 111);
			this.radioButtonPng.Name = "radioButtonPng";
			this.radioButtonPng.Size = new System.Drawing.Size(48, 17);
			this.radioButtonPng.TabIndex = 5;
			this.radioButtonPng.TabStop = true;
			this.radioButtonPng.Text = "PNG";
			this.radioButtonPng.UseVisualStyleBackColor = true;
			// 
			// radioButtonJpeg
			// 
			this.radioButtonJpeg.AutoSize = true;
			this.radioButtonJpeg.Location = new System.Drawing.Point(81, 111);
			this.radioButtonJpeg.Name = "radioButtonJpeg";
			this.radioButtonJpeg.Size = new System.Drawing.Size(52, 17);
			this.radioButtonJpeg.TabIndex = 6;
			this.radioButtonJpeg.Text = "JPEG";
			this.radioButtonJpeg.UseVisualStyleBackColor = true;
			// 
			// buttonOK
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(294, 78);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 7;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(294, 105);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 8;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// ExportSettingsForm
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(381, 140);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.radioButtonJpeg);
			this.Controls.Add(this.radioButtonPng);
			this.Controls.Add(this.checkBoxGrid);
			this.Controls.Add(this.checkBoxMap);
			this.Controls.Add(this.checkBoxTiles);
			this.Controls.Add(this.buttonDirs);
			this.Controls.Add(this.textBoxDir);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExportSettingsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Export Settings";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBoxDir;
		private System.Windows.Forms.Button buttonDirs;
		private System.Windows.Forms.CheckBox checkBoxTiles;
		private System.Windows.Forms.CheckBox checkBoxMap;
		private System.Windows.Forms.CheckBox checkBoxGrid;
		private System.Windows.Forms.RadioButton radioButtonPng;
		private System.Windows.Forms.RadioButton radioButtonJpeg;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
	}
}