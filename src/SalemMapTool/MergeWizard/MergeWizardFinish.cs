using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SalemMapTool.MergeWizard
{
    public partial class MergeWizardFinish : UserControl
    {
        public MergeWizardFinish()
        {
            InitializeComponent();
        }

        public Session FinalSession { get; set; }

        public void LoadSession(Session session)
        {
            FinalSession = session;
            sessionPictureBox.UpdateSession(FinalSession);
        }

        private void btnMergeNext_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
