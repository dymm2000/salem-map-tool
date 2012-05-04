using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Windows.Forms;

namespace SalemElderTileMerger
{
	public class Session
	{
		class Tile
		{
			public int X { get; set; }
			public int Y { get; set; }
			public Image Image { get; set; }

			public override string ToString()
			{
				return string.Format("{0}, {1}", X, Y);
			}
		}

		Image preview;
		int zoom = -1;
		Tile selected = null;
		List<Tile> tiles = new List<Tile>();

		public Session(string name, string path)
		{
			Name = name;

			int left = 0;
			int right = 0;
			int top = 0;
			int bottom = 0;
			foreach (string f in Directory.GetFiles(path, "tile_*.png"))
			{
				string[] p = Path.GetFileNameWithoutExtension(f).Split('_');
				Tile tile = new Tile();
				tile.Image = Image.FromFile(f);
				tile.X = int.Parse(p[1]) * tile.Image.Width;
				tile.Y = int.Parse(p[2]) * tile.Image.Height;

				tiles.Add(tile);

				left = Math.Min(left, tile.X);
				right = Math.Max(right, tile.X + tile.Image.Width);
				top = Math.Min(top, tile.Y);
				bottom = Math.Max(bottom, tile.Y + tile.Image.Height);
			}

			if (tiles.Count <= 12)
				return;

			foreach (Tile tile in tiles)
			{
				tile.X -= left;
				tile.Y -= top;
			}

			Width = right - left;
			Height = bottom - top;

			DrawPreview();
		}
		public Session(string name, IList sessions)
		{
			Name = name;

			int left = 0;
			int right = 0;
			int top = 0;
			int bottom = 0;
			foreach (Session session in sessions)
			{
				foreach (Tile tile in session.tiles)
				{ 
					Tile clone = new Tile();
					clone.Image = tile.Image;
					clone.X = tile.X - session.selected.X;
					clone.Y = tile.Y - session.selected.Y;

					bool dup = false;
					foreach (Tile t in tiles)
					{
						dup = t.X == clone.X && t.Y == clone.Y;
						if (dup)
							break;
					}
					if (dup)
						continue;

					tiles.Add(clone);

					left = Math.Min(left, clone.X);
					right = Math.Max(right, clone.X + tile.Image.Width);
					top = Math.Min(top, clone.Y);
					bottom = Math.Max(bottom, clone.Y + tile.Image.Height);

					if (tile == session.selected)
						selected = clone;
				}
			}

			foreach (Tile tile in tiles)
			{ 
				tile.X -= left;
				tile.Y -= top;
			}

			Width = right - left;
			Height = bottom - top;

			DrawPreview();
		}

		void DrawPreview()
		{
			float scale = Math.Min(1, Math.Min(5000f / Width, 5000f / Height));

			preview = new Bitmap((int)(scale * Width), (int)(scale * Height));
			using (Graphics g = Graphics.FromImage(preview))
			{
				foreach (Tile tile in tiles)
					g.DrawImage(tile.Image,
						new RectangleF(scale * tile.X, scale * tile.Y, scale * tile.Image.Width, scale * tile.Image.Height),
						new RectangleF(0, 0, tile.Image.Width, tile.Image.Height), GraphicsUnit.Pixel);
			}
		}
		void Draw(Graphics g, int w, int h, int zoom, bool selection)
		{
			if (zoom == -1)
			{
				float scale = Math.Min(w * 1f / preview.Width, h * 1f / preview.Height);
				float x0 = (w - scale * preview.Width) / 2;
				float y0 = (h - scale * preview.Height) / 2;
				g.DrawImage(preview, 
					new RectangleF(x0, y0, scale * preview.Width, scale * preview.Height), 
					new RectangleF(0, 0, preview.Width, preview.Height), GraphicsUnit.Pixel);

				scale = Math.Min(w * 1f / Width, h * 1f / Height);
				x0 = (w - scale * Width) / 2;
				y0 = (h - scale * Height) / 2;
				g.DrawRectangle(
					//Pens.Red,
					new Pen(Brushes.Red, 3), 
					//new Pen(new HatchBrush(HatchStyle.DiagonalCross, Color.Red, Color.Transparent), 10),
					x0 + scale * X * Width, y0 + scale * Y * Height, scale * Math.Min(w, Width) - 1, scale * Math.Min(h, Height) - 1);
			}
			else
			{
				RectangleF source = new RectangleF(X * Width, Y * Height, w, h);
				RectangleF target = new RectangleF((w - Math.Min(w, Width)) / 2, (h - Math.Min(h, Height)) / 2, w, h);
				foreach (Tile tile in tiles)
				{
					if (!source.IntersectsWith(new RectangleF(tile.X, tile.Y, tile.Image.Width, tile.Image.Height)))
						continue;

					float x = target.Left + tile.X - source.Left;
					float y = target.Top + tile.Y - source.Top;
					g.DrawImage(tile.Image, x, y);
				}

				if (selection && selected != null && source.IntersectsWith(new RectangleF(selected.X, selected.Y, selected.Image.Width, selected.Image.Height)))
				{
					float x = target.Left + selected.X - source.Left;
					float y = target.Top + selected.Y - source.Top;
					g.DrawRectangle(Pens.White, x, y, selected.Image.Width - 1, selected.Image.Height - 1);
					g.FillRectangle(new HatchBrush(HatchStyle.DiagonalCross, Color.White, Color.Transparent), x, y, selected.Image.Width - 1, selected.Image.Height - 1);
				}
			}
		}

		public void Zoom(int zoom)
		{
			this.zoom = this.zoom == 0 ? -1 : 0;
		}
		public void Hit(int x, int y, int w, int h)
		{
			if (zoom != 0)
				return;

			int x0 = (w - Math.Min(w, Width)) / 2;
			int y0 = (h - Math.Min(h, Height)) / 2;
			float xx = X * Width + x - x0;
			float yy = Y * Height + y - y0;

			Tile hit = null;
			foreach (Tile tile in tiles)
			{
				if (!new RectangleF(tile.X, tile.Y, tile.Image.Width, tile.Image.Height).Contains(xx, yy))
					continue;

				hit = tile;
				break;
			}

			if (hit == null)
				return;

			//using (Graphics g = Graphics.FromImage(image))
			//{ 
			//    if (selected != null)
			//        g.DrawImage(selected.Image, selected.X, selected.Y);

			    if (selected == hit)
			    {
			        selected = null;
			    }
			    else
			    {
			        selected = hit;

			//        g.DrawRectangle(Pens.White, selected.X, selected.Y, selected.Image.Width - 1, selected.Image.Height - 1);
			//        g.FillRectangle(new HatchBrush(HatchStyle.DiagonalCross, Color.White, Color.Transparent), selected.X, selected.Y, selected.Image.Width - 1, selected.Image.Height - 1);
			    }
			//}
		}
		public void Draw(Graphics g, int w, int h)
		{
			Draw(g, w, h, zoom, true);
		}
		public void Save(string directory)
		{
			string mapdir = Path.Combine(directory, Name);
			Directory.CreateDirectory(mapdir);
			if (selected == null)
			{
				foreach (Tile tile in tiles)
					tile.Image.Save(Path.Combine(mapdir, string.Format("tile_{0}_{1}.png", tile.X / tile.Image.Width, tile.Y / tile.Image.Height)), ImageFormat.Png);
			}
			else
			{ 
				foreach (Tile tile in tiles)
					tile.Image.Save(Path.Combine(mapdir, string.Format("tile_{0}_{1}.png", (tile.X - selected.X) / tile.Image.Width, (tile.Y - selected.Y) / tile.Image.Height)), ImageFormat.Png);
			}

			try
			{
				using (Image i = new Bitmap(Width, Height))
				{
					using (Graphics g = Graphics.FromImage(i))
						Draw(g, Width, Height, 0, false);

					i.Save(Path.Combine(directory, Name + ".png"), ImageFormat.Png);
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(string.Format("{0}\nWhole map can not be saved:\n\n{1}", Name, e.Message), "Error");
			}
		}

		public override string ToString()
		{
			return string.Format("{0} [{1}] [{2}x{3}]", Name, tiles.Count, Width, Height);
		}

		public float X
		{
			get; set;
		}
		public float Y
		{
			get; set;
		}
		public int Width
		{
			get;
			private set;
		}
		public int Height
		{
			get;
			private set;
		}
		public string Name
		{
			get;
			private set;
		}
		public bool CanMerge
		{
			get { return selected != null; }
		}
	}
}
