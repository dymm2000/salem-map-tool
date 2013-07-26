namespace SalemMapTool.MergeWizard
{
    partial class MergeWizard
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
            this.mwPartStage = new SalemMapTool.MergeWizard.MergeWizardPart();
            this.mwFinishStage = new SalemMapTool.MergeWizard.MergeWizardFinish();
            this.SuspendLayout();
            // 
            // mwPartStage
            // 
            this.mwPartStage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mwPartStage.Location = new System.Drawing.Point(0, 0);
            this.mwPartStage.Name = "mwPartStage";
            this.mwPartStage.Size = new System.Drawing.Size(891, 495);
            this.mwPartStage.TabIndex = 0;
            // 
            // mwFinishStage
            // 
            this.mwFinishStage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mwFinishStage.FinalSession = null;
            this.mwFinishStage.Location = new System.Drawing.Point(0, 0);
            this.mwFinishStage.Name = "mwFinishStage";
            this.mwFinishStage.Size = new System.Drawing.Size(891, 495);
            this.mwFinishStage.TabIndex = 1;
            this.mwFinishStage.Visible = false;
            // 
            // MergeWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 495);
            this.Controls.Add(this.mwFinishStage);
            this.Controls.Add(this.mwPartStage);
            this.Name = "MergeWizard";
            this.Text = "MergeWizard";
            this.ResumeLayout(false);

        }

        #endregion

        private MergeWizardPart mwPartStage;
        private MergeWizardFinish mwFinishStage;
    }
}