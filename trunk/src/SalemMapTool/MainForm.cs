using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Drawing.Imaging;

namespace SalemElderTileMerger
{
	public partial class MainForm : Form
	{
		int mergeCounter = 1;
		bool updating = false;

		public MainForm()
		{
			InitializeComponent();

			hScrollBar.Enabled = false;
			vScrollBar.Enabled = false;
		}

		private void toolStripMenuItemRemove_Click(object sender, EventArgs e)
		{
			List<Session> selected = new List<Session>();
			foreach (Session session in listBoxSessions.SelectedItems)
				selected.Add(session);
			foreach (Session session in selected)
				listBoxSessions.Items.Remove(session);
		}

		private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
		{
			toolStripMenuItemEsport.Enabled = listBoxSessions.SelectedItem != null;
			toolStripMenuItemRemove.Enabled = listBoxSessions.SelectedItem != null;
			
			toolStripMenuItemMerge.Enabled = listBoxSessions.SelectedItems.Count > 1;
			foreach (Session session in listBoxSessions.SelectedItems)
				toolStripMenuItemMerge.Enabled &= session.CanMerge;
		}

		private void listBoxSessions_SelectedValueChanged(object sender, EventArgs e)
		{
			hScrollBar.Enabled = listBoxSessions.SelectedItems.Count == 1;
			vScrollBar.Enabled = listBoxSessions.SelectedItems.Count == 1;

			pictureBox_Resize(null, null);
		}

		private void pictureBox_Resize(object sender, EventArgs e)
		{
			if (listBoxSessions.SelectedItems.Count == 1)
			{
				updating = true;

				hScrollBar.SmallChange = pictureBox.Width / 4;
				hScrollBar.LargeChange = pictureBox.Width;
				hScrollBar.Maximum = (listBoxSessions.SelectedItem as Session).Width;
				hScrollBar.Value = (int)((listBoxSessions.SelectedItem as Session).X * hScrollBar.Maximum);

				vScrollBar.SmallChange = pictureBox.Height / 4;
				vScrollBar.LargeChange = pictureBox.Height;
				vScrollBar.Maximum = (listBoxSessions.SelectedItem as Session).Height;
				vScrollBar.Value = (int)((listBoxSessions.SelectedItem as Session).Y * vScrollBar.Maximum);

				updating = false;
			}

			pictureBox.Refresh();
		}

		private void pictureBox1_Paint(object sender, PaintEventArgs e)
		{
			if (listBoxSessions.SelectedItems.Count == 1)
				(listBoxSessions.SelectedItem as Session).Draw(e.Graphics, 
					pictureBox.Width, pictureBox.Height);
		}

		private void scrollBar_ValueChanged(object sender, EventArgs e)
		{
			if (!updating)
			{
				(listBoxSessions.SelectedItem as Session).X = hScrollBar.Value * 1f / hScrollBar.Maximum;
				(listBoxSessions.SelectedItem as Session).Y = vScrollBar.Value * 1f / vScrollBar.Maximum;

				pictureBox.Refresh();
			}
		}

		private void pictureBox_MouseClick(object sender, MouseEventArgs e)
		{
			if (listBoxSessions.SelectedItems.Count != 1)
				return;

			if (e.Button == MouseButtons.Left)
			{
				(listBoxSessions.SelectedItem as Session).Hit(e.X, e.Y, pictureBox.Width, pictureBox.Height);

				pictureBox.Refresh();
			}
			else if (e.Button == MouseButtons.Right)
			{
				bool zoom = (listBoxSessions.SelectedItem as Session).Zoom();

				pictureBox.Refresh();
			}
		}

		private void toolStripMenuItemExport_Click(object sender, EventArgs e)
		{
			using (FolderBrowserDialog d = new FolderBrowserDialog())
			{
				d.SelectedPath = Directory.GetCurrentDirectory();
				if (d.ShowDialog() == DialogResult.OK)
				{
					foreach (Session session in listBoxSessions.SelectedItems)
						session.Save(d.SelectedPath);
				}
			}
		}

		private void toolStripMenuItemImport_Click(object sender, EventArgs e)
		{
			using (FolderBrowserDialog d = new FolderBrowserDialog())
			{
				d.SelectedPath = Directory.GetCurrentDirectory();
				if (d.ShowDialog() == DialogResult.OK)
				{
					object selected = listBoxSessions.SelectedItem;

					Session session = new Session(Path.GetFileName(d.SelectedPath), d.SelectedPath);
					if (session.Width != 0)
					{
						listBoxSessions.SelectedItems.Clear();
						listBoxSessions.Items.Add(session);
						selected = session;
					}

					foreach (string folder in Directory.GetDirectories(d.SelectedPath, "*", SearchOption.AllDirectories))
					{
						session = new Session(Path.GetFileName(folder), folder);
						if (session.Width != 0)
						{
							listBoxSessions.SelectedItems.Clear();
							listBoxSessions.Items.Add(session);
							selected = session;
						}
					}

					if (selected != null)
						listBoxSessions.SelectedItem = selected;
				}
			}
		}

		private void toolStripMenuItemMerge_Click(object sender, EventArgs e)
		{
			Session session = new Session(string.Format("Merge {0}", mergeCounter++), listBoxSessions.SelectedItems);
			if (session.Width != 0)
			{
				listBoxSessions.SelectedItems.Clear();
				listBoxSessions.Items.Add(session);
				listBoxSessions.SelectedItem = session;
			}
		}
	}
}
