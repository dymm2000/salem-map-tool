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

		float zoom;
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

			Init();
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

			Init();
		}

		void Init()
		{
			ZoomMin = 0.5f;
			ZoomMax = 10;

			zoom = ZoomMax;
		}
		void Draw(Graphics g, int w, int h, float zoom)
		{
			g.FillRectangle(Brushes.Blue, 0, 0, w, h);

			RectangleF source = new RectangleF(X * Width, Y * Height, w * zoom, h * zoom);
			RectangleF target = new RectangleF((w - Math.Min(w, Width / zoom)) / 2, (h - Math.Min(h, Height / zoom)) / 2, w, h);
			
			foreach (Tile tile in tiles)
			{
				if (!source.IntersectsWith(new RectangleF(tile.X, tile.Y, tile.Image.Width, tile.Image.Height)))
					continue;

				RectangleF tt = new RectangleF(target.Left + (tile.X - source.Left) / zoom, target.Top + (tile.Y - source.Top) / zoom, tile.Image.Width / zoom, tile.Image.Height / zoom);

				g.DrawImage(tile.Image, tt, new RectangleF(0, 0, tile.Image.Width, tile.Image.Height), GraphicsUnit.Pixel);

			}

			if (selected != null)
			{
				RectangleF tt = new RectangleF(target.Left + (selected.X - source.Left) / zoom, target.Top + (selected.Y - source.Top) / zoom, selected.Image.Width / zoom, selected.Image.Height / zoom);
				
				if (zoom <= 5)
				{
					g.DrawRectangle(Pens.White, tt.Left, tt.Top, tt.Width - 1, tt.Height - 1);
					g.FillRectangle(new HatchBrush(HatchStyle.DiagonalCross, Color.White, Color.Transparent),
						tt.Left, tt.Top, tt.Width - 1, tt.Height - 1);
				}
				else
				{ 
					g.FillRectangle(Brushes.White, tt.Left, tt.Top, tt.Width - 1, tt.Height - 1);
				}
			}
		}
		Tile Find(int x, int y, int w, int h)
		{ 
			float x0 = (w - Math.Min(w, Width / zoom)) / 2;
			float y0 = (h - Math.Min(h, Height / zoom)) / 2;
			float xx = X * Width + (x - x0) * zoom;
			float yy = Y * Height + (y - y0) * zoom;

			foreach (Tile tile in tiles)
				if (new RectangleF(tile.X, tile.Y, tile.Image.Width, tile.Image.Height).Contains(xx, yy))
					return tile;

			return null;
		}

		public void Hit(int x, int y, int w, int h)
		{
			Tile hit = Find(x, y, w, h);

			selected = hit != null && selected == hit ? null : hit;
		}
		public void SetZoom(int delta, int x, int y, int w, int h)
		{
			zoom = Math.Max(ZoomMin, Math.Min((float)Math.Round(zoom * (delta < 0 ? 1.1f : 0.9f), 1), ZoomMax));
		}
		public void Draw(Graphics g, int w, int h)
		{
			Draw(g, w, h, zoom);
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
						foreach (Tile tile in tiles)
							g.DrawImage(tile.Image, tile.X, tile.Y);

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
		public float Zoom
		{
			get { return zoom; }
		}
		public float ZoomMin
		{
			get; private set;
		}
		public float ZoomMax
		{
			get; private set;
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
