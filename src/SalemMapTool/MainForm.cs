using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace SalemMapTool
{
	public partial class MainForm : Form
	{
	    Session selected;

		public MainForm(string[] args)
		{
			Common.Instance.ReadParameters(args);

			InitializeComponent();

			contextMenuStripSessions.Items.Add("Import...", null, toolStripMenuItemImport_Click);
			contextMenuStripSessions.Items.Add("Export...", null, toolStripMenuItemExport_Click);
			contextMenuStripSessions.Items.Add(new ToolStripSeparator());
			contextMenuStripSessions.Items.Add("Merge...", null, toolStripMenuItemMerge_Click);
			contextMenuStripSessions.Items.Add("Crop...", null, toolStripMenuItemCrop_Click);
			contextMenuStripSessions.Items.Add("Cut...", null, toolStripMenuItemCut_Click);
			contextMenuStripSessions.Items.Add(new ToolStripSeparator());
			contextMenuStripSessions.Items.Add("Remove", null, toolStripMenuItemRemove_Click);
            contextMenuStripSessions.Items.Add(new ToolStripSeparator());
            contextMenuStripSessions.Items.Add("Merge Wizard", null, toolStripMenuItemMergeWizard_Click);

			toolStripMenuItemSession.DropDownItems.Add("Import...", null, toolStripMenuItemImport_Click);
			toolStripMenuItemSession.DropDownItems.Add("Export...", null, toolStripMenuItemExport_Click);
			toolStripMenuItemSession.DropDownItems.Add(new ToolStripSeparator());
			toolStripMenuItemSession.DropDownItems.Add("Merge...", null, toolStripMenuItemMerge_Click);
			toolStripMenuItemSession.DropDownItems.Add("Crop...", null, toolStripMenuItemCrop_Click);
			toolStripMenuItemSession.DropDownItems.Add("Cut...", null, toolStripMenuItemCut_Click);
			toolStripMenuItemSession.DropDownItems.Add(new ToolStripSeparator());
			toolStripMenuItemSession.DropDownItems.Add("Remove", null, toolStripMenuItemRemove_Click);
            toolStripMenuItemSession.DropDownItems.Add(new ToolStripSeparator());
            toolStripMenuItemSession.DropDownItems.Add("Merge Wizard", null, toolStripMenuItemMergeWizard_Click);
		}

		void UpdateMenu()
		{ 
			bool selectedSingle = selected != null;
			bool selectedMulti = listBoxSessions.SelectedItems.Count > 0;
			bool canMerge = listBoxSessions.SelectedItems.Count > 1;
			foreach (Session session in listBoxSessions.SelectedItems)
				canMerge &= session.CanMerge;

			contextMenuStripSessions.Items[1].Enabled = selectedSingle;
			contextMenuStripSessions.Items[3].Enabled = canMerge;
			contextMenuStripSessions.Items[4].Enabled = selectedSingle;
			contextMenuStripSessions.Items[5].Enabled = selectedSingle;
			contextMenuStripSessions.Items[7].Enabled = selectedMulti;
            contextMenuStripSessions.Items[9].Enabled = selectedSingle;


			toolStripMenuItemSession.DropDownItems[1].Enabled = selectedSingle;
			toolStripMenuItemSession.DropDownItems[3].Enabled = canMerge;
			toolStripMenuItemSession.DropDownItems[4].Enabled = selectedSingle;
			toolStripMenuItemSession.DropDownItems[5].Enabled = selectedSingle;
			toolStripMenuItemSession.DropDownItems[7].Enabled = selectedMulti;
            toolStripMenuItemSession.DropDownItems[9].Enabled = selectedSingle;
        }
		bool NameQuery(ref string name)
		{
			name = Microsoft.VisualBasic.Interaction.InputBox("Session name", "Input", name);

			return name != "";
		}

		private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			UpdateMenu();
		}
		private void menuStripMain_MenuActivate(object sender, EventArgs e)
		{
			UpdateMenu();
		}
		private void toolStripMenuItemImport_Click(object sender, EventArgs e)
		{
			using (var d = new FolderBrowserDialog())
			{
				d.SelectedPath = (string)Common.Instance.Parameters[Consts.s_importDir];
				if (d.ShowDialog() == DialogResult.OK)
				{
					var selectedItem = listBoxSessions.SelectedItem;

					Cursor.Current = Cursors.WaitCursor;
					try
					{
						var session = new Session(Path.GetFileName(d.SelectedPath));
                        if (session.Load(d.SelectedPath, (uint)Common.Instance.Parameters[Consts.s_importMinSize]))
						{
							listBoxSessions.SelectedItems.Clear();
							listBoxSessions.Items.Add(session);
							selectedItem = session;
						}

						foreach (string folder in Directory.GetDirectories(d.SelectedPath, "*", SearchOption.AllDirectories))
						{
							session = new Session(Path.GetFileName(folder));
                            if (session.Load(folder, (uint)Common.Instance.Parameters[Consts.s_importMinSize]))
							{
								listBoxSessions.SelectedItems.Clear();
								listBoxSessions.Items.Add(session);
								selectedItem = session;
							}
						}
					}
					finally
					{
						Cursor.Current = Cursors.Default;
					}

					if (selectedItem != null)
						listBoxSessions.SelectedItem = selectedItem;
				}
			}
		}
		
        private void toolStripMenuItemMerge_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				string name = string.Format("Merge {0:yyyy-MM-dd HH.mm.ss}", DateTime.Now);
				if (!NameQuery(ref name))
					return;

				var session = new Session(name);
				if (session.Load(listBoxSessions.SelectedItems))
				{
					listBoxSessions.SelectedItems.Clear();
					listBoxSessions.Items.Add(session);
					listBoxSessions.SelectedItem = session;
				}
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}
		private void toolStripMenuItemExport_Click(object sender, EventArgs e)
		{
            if (ExportSettingsForm.Show(Common.Instance.ExportParams))
			{
				Cursor.Current = Cursors.WaitCursor;
				try
				{
					foreach (Session session in listBoxSessions.SelectedItems)
						try
						{
                            session.Save(Common.Instance.ExportParams);
						}
						catch (Exception ex)
						{
							MessageBox.Show(string.Format("Session '{0}'\n{1}", session.Name, ex.Message), @"Error");
						}
				}
				finally
				{
					Cursor.Current = Cursors.Default;
				}
			}
		}
		private void toolStripMenuItemCrop_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				var name = string.Format("Crop {0:yyyy-MM-dd HH.mm.ss}", DateTime.Now);
				if (!NameQuery(ref name))
					return;

				var session = new Session(name);
				if (session.Load(selected, Session.Inheritance.Crop))
				{
					listBoxSessions.SelectedItems.Clear();
					listBoxSessions.Items.Add(session);
					listBoxSessions.SelectedItem = session;
				}
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}
        private void toolStripMenuItemMergeWizard_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                var matches = new List<Session>();

                var originalTilesHash = selected.GenerateHash();
                foreach (var sessionObject in listBoxSessions.Items)
                {
                    var session = sessionObject as Session;
                    if (session == null || session == selected)
                        continue;

                    var currentTilesHash = session.GenerateHash();
                    if (currentTilesHash.Keys.Any(originalTilesHash.ContainsKey))
                        matches.Add(session);
                }

                if (matches.Count == 0)
                {
                    MessageBox.Show(@"No matches found", @"Merge Finished", MessageBoxButtons.OK);
                    return;
                }

                var mergeWizard = new MergeWizard.MergeWizard(selected, matches);
                if (mergeWizard.StartWizard() == DialogResult.OK)
                {
                    if (MessageBox.Show(@"The merge process was sucessfull.
Should the merged sessions be removed?",
                                        @"Merge Finished", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        listBoxSessions.Items.Remove(selected);
                        foreach (var mergeSession in mergeWizard.MatchingSessions)
                            if (!mergeSession.Skipped) listBoxSessions.Items.Remove(mergeSession.Session);
                    }

                    selected = mergeWizard.FinalSession;
                    listBoxSessions.Items.Add(selected);
                    listBoxSessions.SelectedItem = selected;
                }
            }
            finally 
            {
                Cursor.Current = Cursors.Default;
            }
        }
		private void toolStripMenuItemCut_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				var name = string.Format("Cut {0:yyyy-MM-dd HH.mm.ss}", DateTime.Now);
				if (!NameQuery(ref name))
					return;

				var session = new Session(name);
				if (session.Load(selected, Session.Inheritance.Cut))
				{
					listBoxSessions.SelectedItems.Clear();
					listBoxSessions.Items.Add(session);
					listBoxSessions.SelectedItem = session;
				}
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}
		private void toolStripMenuItemRemove_Click(object sender, EventArgs e)
		{
			var sessions = listBoxSessions.SelectedItems.Cast<Session>().ToList();
		    foreach (var session in sessions)
				listBoxSessions.Items.Remove(session);
		}

		private void linkLabelHome_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("http://code.google.com/p/salem-map-tool/");
		}
		private void listBoxSessions_SelectedValueChanged(object sender, EventArgs e)
		{
			selected = listBoxSessions.SelectedItems.Count == 1 ? listBoxSessions.SelectedItem as Session : null;
            pictureBox.UpdateSession(selected);

		}
    }
}
