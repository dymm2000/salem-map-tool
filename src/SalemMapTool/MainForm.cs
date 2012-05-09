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
using System.Configuration;
using System.Globalization;

namespace SalemElderTileMerger
{
	public partial class MainForm : Form
	{
		Session selected = null;
		bool updating = false;
		int x0, y0, x, y;
		string importDir;
		string exportDir;

		public MainForm()
		{
			InitializeComponent();

			hScrollBar.Enabled = false;
			vScrollBar.Enabled = false;
			trackBarZoom.Visible = false;

			uint bc; 
			if (!uint.TryParse(ConfigurationManager.AppSettings["backColor"], NumberStyles.HexNumber, null, out bc))
				bc = 0x4040ff;
			pictureBox.BackColor = Color.FromArgb((int)(0xff000000 | bc));
			pictureBox.MouseEnter += pictureBox_MouseEnter;
			pictureBox.MouseWheel += pictureBox_MouseWheel;

			importDir = ConfigurationManager.AppSettings["importDir"].ToLower().Replace("%userprofile%", Environment.GetEnvironmentVariable("userprofile"));
			exportDir = ConfigurationManager.AppSettings["exportDir"].ToLower().Replace("%userprofile%", Environment.GetEnvironmentVariable("userprofile"));
		}

		void UpdateBars()
		{ 
			if (selected != null)
			{
				updating = true;

				selected.SetFOV(pictureBox.Width, pictureBox.Height);

				hScrollBar.LargeChange = selected.FOVWidth;
				hScrollBar.SmallChange = hScrollBar.LargeChange / 4;
				hScrollBar.Maximum = selected.Width;
				hScrollBar.Value = selected.FOVLeft;

				vScrollBar.LargeChange = selected.FOVHeight;
				vScrollBar.SmallChange = vScrollBar.LargeChange / 4;
				vScrollBar.Maximum = selected.Height;
				vScrollBar.Value = selected.FOVTop;

				trackBarZoom.Minimum = selected.ZoomMin;
				trackBarZoom.Maximum = selected.ZoomMax;
				trackBarZoom.Value = selected.Zoom;

				updating = false;
			}
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
				d.SelectedPath = importDir;
				if (d.ShowDialog() == DialogResult.OK)
				{
					object selected = listBoxSessions.SelectedItem;

					Cursor.Current = Cursors.WaitCursor;
					try
					{
						Session session = new Session(Path.GetFileName(d.SelectedPath));
						if (session.Load(d.SelectedPath))
						{
							listBoxSessions.SelectedItems.Clear();
							listBoxSessions.Items.Add(session);
							selected = session;
						}

						foreach (string folder in Directory.GetDirectories(d.SelectedPath, "*", SearchOption.AllDirectories))
						{
							session = new Session(Path.GetFileName(folder));
							if (session.Load(folder))
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
				Session session = new Session(string.Format("Merge {0:yyyy-MM-dd HH.mm.ss}", DateTime.Now));
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
			using (FolderBrowserDialog d = new FolderBrowserDialog())
			{
				d.SelectedPath = exportDir;
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
			selected = listBoxSessions.SelectedItems.Count == 1 ? listBoxSessions.SelectedItem as Session : null;

			hScrollBar.Enabled = selected != null;
			vScrollBar.Enabled = selected != null;
			trackBarZoom.Visible = selected != null;

			UpdateBars();
			
			pictureBox.Refresh();
		}

		private void hScrollBar_ValueChanged(object sender, EventArgs e)
		{
			if (selected == null || updating)
				return;

			selected.FOVLeft = hScrollBar.Value;

			pictureBox.Refresh();
		}
		private void vScrollBar_ValueChanged(object sender, EventArgs e)
		{
			if (selected == null || updating)
				return;

			selected.FOVTop = vScrollBar.Value;

			pictureBox.Refresh();
		}
		private void trackBarZoom_Scroll(object sender, EventArgs e)
		{
			if (selected == null || updating)
				return;

			selected.SetZoom(trackBarZoom.Value, pictureBox.Width / 2, pictureBox.Height / 2);
			
			UpdateBars();

			pictureBox.Refresh();
		}
		
		private void pictureBox_Resize(object sender, EventArgs e)
		{
			if (selected == null || pictureBox.Width == 0 || pictureBox.Height == 0)
				return;

			selected.SetFOV(pictureBox.Width, pictureBox.Height);

			UpdateBars();
			
			pictureBox.Refresh();
		}
		private void pictureBox_Paint(object sender, PaintEventArgs e)
		{
			if (selected == null)
				e.Graphics.Clear(BackColor);
			else
				selected.Draw(e.Graphics);
		}
		private void pictureBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (selected == null || e.Button != MouseButtons.Left)
				return;

			x0 = x = e.X;
			y0 = y = e.Y;
		}
		private void pictureBox_MouseUp(object sender, MouseEventArgs e)
		{
			if (selected == null || e.Button != MouseButtons.Left)
				return;

			if (Math.Abs(x0 - e.X) <= 1 && Math.Abs(y0 - e.Y) <= 1)
			{
				selected.Hit(e.X, e.Y);

				pictureBox.Refresh();
			}
		}
		private void pictureBox_MouseMove(object sender, MouseEventArgs e)
		{
			if (selected == null || e.Button != MouseButtons.Left)
				return;

			selected.Move(e.X - x, e.Y - y);
			
			x = e.X;
			y = e.Y;

			UpdateBars();

			pictureBox.Refresh();
		}
		private void pictureBox_MouseEnter(object sender, EventArgs e)
		{
			pictureBox.Focus();
		}
		private void pictureBox_MouseWheel(object sender, MouseEventArgs e)
		{
			if (selected == null)
				return;

			selected.SetZoom(selected.Zoom + (e.Delta > 0 ? 1 : -1), e.X, e.Y);

			UpdateBars();

			pictureBox.Refresh();
		}
	}
}
