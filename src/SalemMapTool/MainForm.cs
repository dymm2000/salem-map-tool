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
		int x0, y0, x, y;

		public MainForm()
		{
			InitializeComponent();

			hScrollBar.Enabled = false;
			vScrollBar.Enabled = false;

			MouseWheel += new MouseEventHandler(pictureBox_MouseWheel);
			pictureBox.MouseEnter += pictureBox_MouseEnter;
			pictureBox.MouseWheel += pictureBox_MouseWheel;
		}

		private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			toolStripMenuItemExport.Enabled = listBoxSessions.SelectedItem != null;
			toolStripMenuItemRemove.Enabled = listBoxSessions.SelectedItem != null;
			
			toolStripMenuItemMerge.Enabled = listBoxSessions.SelectedItems.Count > 1;
			foreach (Session session in listBoxSessions.SelectedItems)
				toolStripMenuItemMerge.Enabled &= session.CanMerge;
		}
		private void toolStripMenuItemRemove_Click(object sender, EventArgs e)
		{
			List<Session> selected = new List<Session>();
			foreach (Session session in listBoxSessions.SelectedItems)
				selected.Add(session);
			foreach (Session session in selected)
				listBoxSessions.Items.Remove(session);
		}
		private void toolStripMenuItemImport_Click(object sender, EventArgs e)
		{
			using (FolderBrowserDialog d = new FolderBrowserDialog())
			{
				d.SelectedPath = Directory.GetCurrentDirectory();
				if (d.ShowDialog() == DialogResult.OK)
				{
					object selected = listBoxSessions.SelectedItem;

					Cursor.Current = Cursors.WaitCursor;
					try
					{
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
					}
					finally
					{
						Cursor.Current = Cursors.Default;
					}

					if (selected != null)
						listBoxSessions.SelectedItem = selected;
				}
			}
		}
		private void toolStripMenuItemMerge_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				Session session = new Session(string.Format("Merge {0:yyyy-MM-dd HH.mm.ss}", DateTime.Now), listBoxSessions.SelectedItems);
				if (session.Width != 0)
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
			using (FolderBrowserDialog d = new FolderBrowserDialog())
			{
				d.SelectedPath = Directory.GetCurrentDirectory();
				if (d.ShowDialog() == DialogResult.OK)
				{
					Cursor.Current = Cursors.WaitCursor;
					try
					{
						foreach (Session session in listBoxSessions.SelectedItems)
							session.Save(d.SelectedPath);
					}
					finally
					{
						Cursor.Current = Cursors.Default;
					}
				}
			}
		}

		private void listBoxSessions_SelectedValueChanged(object sender, EventArgs e)
		{
			hScrollBar.Enabled = listBoxSessions.SelectedItems.Count == 1;
			vScrollBar.Enabled = listBoxSessions.SelectedItems.Count == 1;

			if (listBoxSessions.SelectedItems.Count != 1)
				return;

			hScrollBar.SmallChange = pictureBox.Width / 4;
			hScrollBar.LargeChange = (int)(pictureBox.Width * (listBoxSessions.SelectedItem as Session).Zoom);
			hScrollBar.Maximum = (listBoxSessions.SelectedItem as Session).Width;
			hScrollBar.Value = Math.Max(hScrollBar.Minimum, Math.Min((int)((listBoxSessions.SelectedItem as Session).X * hScrollBar.Maximum), hScrollBar.Maximum - hScrollBar.LargeChange));

			vScrollBar.SmallChange = pictureBox.Height / 4;
			vScrollBar.LargeChange = (int)(pictureBox.Height * (listBoxSessions.SelectedItem as Session).Zoom);
			vScrollBar.Maximum = (listBoxSessions.SelectedItem as Session).Height;
			vScrollBar.Value = Math.Max(vScrollBar.Minimum, Math.Min((int)((listBoxSessions.SelectedItem as Session).Y * vScrollBar.Maximum), vScrollBar.Maximum - vScrollBar.LargeChange));
			
			pictureBox.Refresh();
		}

		private void scrollBar_ValueChanged(object sender, EventArgs e)
		{
			(listBoxSessions.SelectedItem as Session).X = hScrollBar.Value * 1f / hScrollBar.Maximum;
			(listBoxSessions.SelectedItem as Session).Y = vScrollBar.Value * 1f / vScrollBar.Maximum;

			pictureBox.Refresh();
		}
		
		private void pictureBox_Resize(object sender, EventArgs e)
		{
			if (listBoxSessions.SelectedItems.Count != 1)
				return;

			hScrollBar.SmallChange = pictureBox.Width / 4;
			hScrollBar.LargeChange = (int)(pictureBox.Width * (listBoxSessions.SelectedItem as Session).Zoom);
			hScrollBar.Value = Math.Max(hScrollBar.Minimum, Math.Min((int)((listBoxSessions.SelectedItem as Session).X * hScrollBar.Maximum), hScrollBar.Maximum - hScrollBar.LargeChange));

			vScrollBar.SmallChange = pictureBox.Height / 4;
			vScrollBar.LargeChange = (int)(pictureBox.Height * (listBoxSessions.SelectedItem as Session).Zoom);
			vScrollBar.Value = Math.Max(vScrollBar.Minimum, Math.Min((int)((listBoxSessions.SelectedItem as Session).Y * vScrollBar.Maximum), vScrollBar.Maximum - vScrollBar.LargeChange));
			
			pictureBox.Refresh();
		}
		private void pictureBox_Paint(object sender, PaintEventArgs e)
		{
			if (listBoxSessions.SelectedItems.Count == 1)
				(listBoxSessions.SelectedItem as Session).Draw(e.Graphics, 
					pictureBox.Width, pictureBox.Height);
		}
		private void pictureBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (listBoxSessions.SelectedItems.Count != 1 || e.Button != MouseButtons.Left)
				return;

			x0 = x = e.X;
			y0 = y = e.Y;
		}
		private void pictureBox_MouseUp(object sender, MouseEventArgs e)
		{
			if (listBoxSessions.SelectedItems.Count != 1 || e.Button != MouseButtons.Left)
				return;

			if (Math.Abs(x0 - e.X) <= 1 && Math.Abs(y0 - e.Y) <= 1)
			{
				(listBoxSessions.SelectedItem as Session).Hit(e.X, e.Y, pictureBox.Width, pictureBox.Height);

				pictureBox.Refresh();
			}
		}
		private void pictureBox_MouseMove(object sender, MouseEventArgs e)
		{
			if (listBoxSessions.SelectedItems.Count != 1 || e.Button != MouseButtons.Left)
				return;

			hScrollBar.Value = Math.Max(hScrollBar.Minimum, Math.Min(hScrollBar.Value + x - e.X, hScrollBar.Maximum - hScrollBar.LargeChange));
			vScrollBar.Value = Math.Max(vScrollBar.Minimum, Math.Min(vScrollBar.Value + y - e.Y, vScrollBar.Maximum - vScrollBar.LargeChange));
			
			x = e.X;
			y = e.Y;
		}
		private void pictureBox_MouseEnter(object sender, EventArgs e)
		{
			pictureBox.Focus();
		}
		private void pictureBox_MouseWheel(object sender, MouseEventArgs e)
		{
			if (listBoxSessions.SelectedItems.Count != 1)
				return;

			(listBoxSessions.SelectedItem as Session).SetZoom(e.Delta, e.X, e.Y, pictureBox.Width, pictureBox.Height);

			hScrollBar.LargeChange = (int)(pictureBox.Width * (listBoxSessions.SelectedItem as Session).Zoom);
			hScrollBar.Value = Math.Max(hScrollBar.Minimum, Math.Min((int)((listBoxSessions.SelectedItem as Session).X * hScrollBar.Maximum), hScrollBar.Maximum - hScrollBar.LargeChange));

			vScrollBar.LargeChange = (int)(pictureBox.Height * (listBoxSessions.SelectedItem as Session).Zoom);
			vScrollBar.Value = Math.Max(vScrollBar.Minimum, Math.Min((int)((listBoxSessions.SelectedItem as Session).Y * vScrollBar.Maximum), vScrollBar.Maximum - vScrollBar.LargeChange));

			pictureBox.Refresh();
		}
	}
}
