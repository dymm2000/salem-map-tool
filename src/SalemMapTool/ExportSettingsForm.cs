using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace SalemMapTool
{
	public partial class ExportSettingsForm : Form
	{
		ExportParams p;

		protected ExportSettingsForm()
		{
			InitializeComponent();
		}

		public static bool Show(ExportParams p)
		{
			using (ExportSettingsForm f = new ExportSettingsForm())
			{
				f.textBoxDir.Text = p.Directory;
				f.checkBoxTiles.Checked = p.ExportTiles;
				f.checkBoxMap.Checked = p.ExportMap;
				f.checkBoxGrid.Checked = p.ShowGrid;
				f.radioButtonPng.Checked = p.Format == ImageFormat.Png;
				f.radioButtonJpeg.Checked = p.Format == ImageFormat.Jpeg;

				bool result = f.ShowDialog() == DialogResult.OK;
				if (result)
				{
					p.Directory = f.textBoxDir.Text;
					p.ExportTiles = f.checkBoxTiles.Checked;
					p.ExportMap = f.checkBoxMap.Checked;
					p.ShowGrid = f.checkBoxGrid.Checked;
					p.Format = f.radioButtonPng.Checked ? ImageFormat.Png : ImageFormat.Jpeg;
				}

				return result;
			}
		}

		private void buttonDirs_Click(object sender, EventArgs e)
		{
			using (FolderBrowserDialog d = new FolderBrowserDialog())
			{
				d.SelectedPath = textBoxDir.Text;
				if (d.ShowDialog() == DialogResult.OK)
					textBoxDir.Text = d.SelectedPath;
			}
		}
		private void checkBoxMap_CheckedChanged(object sender, EventArgs e)
		{
			radioButtonPng.Enabled = checkBoxMap.Checked;
			radioButtonJpeg.Enabled = checkBoxMap.Checked;
			checkBoxGrid.Enabled = checkBoxMap.Checked;
		}
	}

	public class ExportParams
	{
		public string Directory { get; set; }
		public bool ExportTiles { get; set; }
		public bool ExportMap { get; set; }
		public bool ShowGrid { get; set; }
		public ImageFormat Format { get; set; }

		public static ExportParams Default
		{
			get 
			{
				return new ExportParams()
				{
					ExportTiles = true,
					ExportMap = true,
					ShowGrid = false,
					Format = ImageFormat.Jpeg
				};
			}
		}
	}

}
