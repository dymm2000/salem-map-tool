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
using System.Collections;

namespace SalemElderTileMerger
{
	public partial class MainForm : Form
	{
		Session selected = null;
		bool updating = false;
		bool ctrlPressed = false;
		bool shftPressed = false;
		int x0, y0, x, y;
		Hashtable parameters = new Hashtable();

		const string s_backColor = "backcolor";
		const string s_importDir = "importdir";
		const string s_exportDir = "exportdir";
		const string s_userprofile = "userprofile";

		public MainForm(string[] args)
		{
			ReadParameters(args);

			InitializeComponent();

			hScrollBar.Enabled = false;
			vScrollBar.Enabled = false;
			trackBarZoom.Visible = false;

			pictureBox.BackColor = (Color)parameters[s_backColor];
			pictureBox.MouseEnter += pictureBox_MouseEnter;
			pictureBox.MouseWheel += pictureBox_MouseWheel;
			pictureBox.KeyDown += pictureBox_PreviewKey;
			pictureBox.KeyUp += pictureBox_PreviewKey;
		}

		void ReadParameters(string[] args)
		{
			string value;
			string userprofile = Environment.GetEnvironmentVariable(s_userprofile);
 
			uint bc; 
			value = ConfigurationManager.AppSettings[s_backColor];
			if (!uint.TryParse(value, NumberStyles.HexNumber, null, out bc))
				bc = 0x4040ff;
			parameters[s_backColor] = Color.FromArgb((int)(0xff000000 | bc));

			value = ConfigurationManager.AppSettings[s_importDir];
			parameters[s_importDir] = value == null ? userprofile : value.ToLower().Replace(string.Format("%{0}%", s_userprofile), userprofile);
			
			value = ConfigurationManager.AppSettings[s_exportDir];
			parameters[s_exportDir] = value == null ? userprofile : value.ToLower().Replace(string.Format("%{0}%", s_userprofile), userprofile);

			foreach (string a in args)
			{ 
				string[] s = a.ToLower().
					Replace("'", "").
					Replace("\"", "").
					Replace(string.Format("%{0}%", s_userprofile), userprofile).
					Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

				if (s.Length != 2)
					continue;

				if (s[0] == s_backColor)
				{ 
					if (uint.TryParse(s[1], NumberStyles.HexNumber, null, out bc))
						parameters[s_backColor] = Color.FromArgb((int)(0xff000000 | bc));
				}
				else if (s[0] == s_importDir)
				{
					parameters[s_importDir] = s[1];
				}
				else if (s[0] == s_exportDir)
				{
					parameters[s_exportDir] = s[1];
				}
			}
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
			toolStripMenuItemCut.Enabled = listBoxSessions.SelectedItem != null;
			toolStripMenuItemCrop.Enabled = listBoxSessions.SelectedItem != null;
			toolStripMenuItemRemove.Enabled = listBoxSessions.SelectedItem != null;
			
			toolStripMenuItemMerge.Enabled = listBoxSessions.SelectedItems.Count > 1;
			foreach (Session session in listBoxSessions.SelectedItems)
				toolStripMenuItemMerge.Enabled &= session.CanMerge;
		}
		private void toolStripMenuItemImport_Click(object sender, EventArgs e)
		{
			using (FolderBrowserDialog d = new FolderBrowserDialog())
			{
				d.SelectedPath = (string)parameters[s_importDir];
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
		private void toolStripMenuItemRemove_Click(object sender, EventArgs e)
		{
			List<Session> selected = new List<Session>();
			foreach (Session session in listBoxSessions.SelectedItems)
				selected.Add(session);
			foreach (Session session in selected)
				listBoxSessions.Items.Remove(session);
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
				d.SelectedPath = (string)parameters[s_exportDir];
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
		private void toolStripMenuItemCrop_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				Session session = new Session(string.Format("Crop {0:yyyy-MM-dd HH.mm.ss}", DateTime.Now));
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
		private void toolStripMenuItemCut_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				Session session = new Session(string.Format("Cut {0:yyyy-MM-dd HH.mm.ss}", DateTime.Now));
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

			if (shftPressed)
			{
				selected.StartSelect(e.X, e.Y);

				pictureBox.Refresh();
			}

			x0 = x = e.X;
			y0 = y = e.Y;
		}
		private void pictureBox_MouseUp(object sender, MouseEventArgs e)
		{
			if (selected == null || e.Button != MouseButtons.Left)
				return;

			if (shftPressed)
			{
				selected.EndSelect(Math.Abs(x0 - e.X) <= 1 && Math.Abs(y0 - e.Y) <= 1);

				pictureBox.Refresh();
			}
			else if (ctrlPressed && Math.Abs(x0 - e.X) <= 1 && Math.Abs(y0 - e.Y) <= 1)
			{
				selected.Choose(e.X, e.Y);

				pictureBox.Refresh();
			}
		}
		private void pictureBox_MouseMove(object sender, MouseEventArgs e)
		{
			if (selected == null || e.Button != MouseButtons.Left)
				return;


			if (shftPressed)
			{
				selected.Move(e.X, e.Y);
			}
			else
			{
				selected.Move(e.X - x, e.Y - y);

				UpdateBars();
			}

			pictureBox.Refresh();
			
			x = e.X;
			y = e.Y;
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
		private void pictureBox_PreviewKey(object sender, KeyEventArgs e)
		{
			ctrlPressed = e.Control;
			shftPressed = e.Shift;
		}
	}
}
