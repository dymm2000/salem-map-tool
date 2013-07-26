namespace SalemMapTool.MergeWizard
{
    partial class MergeWizardPart
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rightSessionPictureBox = new SalemMapTool.SessionPictureBox();
            this.leftSessionPictureBox = new SalemMapTool.SessionPictureBox();
            this.btnMergeNext = new System.Windows.Forms.Button();
            this.btnSkip = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnPrevMatch = new System.Windows.Forms.Button();
            this.btnNextMatch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rightSessionPictureBox
            // 
            this.rightSessionPictureBox.Location = new System.Drawing.Point(459, 3);
            this.rightSessionPictureBox.Name = "rightSessionPictureBox";
            this.rightSessionPictureBox.Size = new System.Drawing.Size(427, 416);
            this.rightSessionPictureBox.TabIndex = 0;
            // 
            // leftSessionPictureBox
            // 
            this.leftSessionPictureBox.Location = new System.Drawing.Point(3, 3);
            this.leftSessionPictureBox.Name = "leftSessionPictureBox";
            this.leftSessionPictureBox.Size = new System.Drawing.Size(427, 416);
            this.leftSessionPictureBox.TabIndex = 1;
            // 
            // btnMergeNext
            // 
            this.btnMergeNext.Location = new System.Drawing.Point(779, 466);
            this.btnMergeNext.Name = "btnMergeNext";
            this.btnMergeNext.Size = new System.Drawing.Size(107, 25);
            this.btnMergeNext.TabIndex = 2;
            this.btnMergeNext.Text = "Merge and Next";
            this.btnMergeNext.UseVisualStyleBackColor = true;
            this.btnMergeNext.Click += new System.EventHandler(this.btnMergeNext_Click);
            // 
            // btnSkip
            // 
            this.btnSkip.Location = new System.Drawing.Point(666, 466);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(107, 25);
            this.btnSkip.TabIndex = 3;
            this.btnSkip.Text = "Skip";
            this.btnSkip.UseVisualStyleBackColor = true;
            this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(553, 466);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(107, 25);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnPrevMatch
            // 
            this.btnPrevMatch.Location = new System.Drawing.Point(210, 466);
            this.btnPrevMatch.Name = "btnPrevMatch";
            this.btnPrevMatch.Size = new System.Drawing.Size(107, 25);
            this.btnPrevMatch.TabIndex = 5;
            this.btnPrevMatch.Text = "<<";
            this.btnPrevMatch.UseVisualStyleBackColor = true;
            this.btnPrevMatch.Visible = false;
            this.btnPrevMatch.Click += new System.EventHandler(this.btnPrevMatch_Click);
            // 
            // btnNextMatch
            // 
            this.btnNextMatch.Location = new System.Drawing.Point(323, 466);
            this.btnNextMatch.Name = "btnNextMatch";
            this.btnNextMatch.Size = new System.Drawing.Size(107, 25);
            this.btnNextMatch.TabIndex = 6;
            this.btnNextMatch.Text = ">>";
            this.btnNextMatch.UseVisualStyleBackColor = true;
            this.btnNextMatch.Visible = false;
            this.btnNextMatch.Click += new System.EventHandler(this.btnNextMatch_Click);
            // 
            // MergeWizardPart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnNextMatch);
            this.Controls.Add(this.btnPrevMatch);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSkip);
            this.Controls.Add(this.btnMergeNext);
            this.Controls.Add(this.leftSessionPictureBox);
            this.Controls.Add(this.rightSessionPictureBox);
            this.Name = "MergeWizardPart";
            this.Size = new System.Drawing.Size(889, 494);
            this.ResumeLayout(false);

        }

        #endregion

        private SessionPictureBox rightSessionPictureBox;
        private SessionPictureBox leftSessionPictureBox;
        private System.Windows.Forms.Button btnMergeNext;
        private System.Windows.Forms.Button btnSkip;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnPrevMatch;
        private System.Windows.Forms.Button btnNextMatch;
    }
}
