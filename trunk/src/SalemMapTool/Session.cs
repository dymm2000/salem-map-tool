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
			byte[] bytes;
			//Image image;

			public Tile(byte[] bytes)
			{
				this.bytes = bytes;
			}
			public Tile(Tile tile): this(tile.bytes)
			{
				this.X = tile.X;
				this.Y = tile.Y;
				this.Width = tile.Width;
				this.Height = tile.Height;
			}

			public int X { get; set; }
			public int Y { get; set; }
			public int Width { get; set; }
			public int Height { get; set; }
			public Image Image 
			{
				get 
				{
					using (MemoryStream m = new MemoryStream(bytes))
						return Image.FromStream(m); 
				} 
			}

			public override string ToString()
			{
				return string.Format("{0}, {1}", X, Y);
			}
		}

		int zoom;
		float[] zooms;
		RectangleF r; // original map
		RectangleF fov; // field of view
		RectangleF p; // area of drawing
		PointF p0; // origin of actual drawing
		Tile selected = null;
		List<Tile> tiles = new List<Tile>();

		public Session(string name)
		{ 
			Name = name;
		}
		public bool Load(string path)
		{
			int left = 0;
			int right = 0;
			int top = 0;
			int bottom = 0;
			foreach (string f in Directory.GetFiles(path, "tile_*.png"))
			{
				string[] p = Path.GetFileNameWithoutExtension(f).Split('_');
				Tile tile = new Tile(File.ReadAllBytes(f));
				//tile.Image = Image.FromFile(f);
				tile.Width = 100;
				tile.Height = 100;
				tile.X = int.Parse(p[1]) * tile.Width;
				tile.Y = int.Parse(p[2]) * tile.Height;

				tiles.Add(tile);

				left = Math.Min(left, tile.X);
				right = Math.Max(right, tile.X + tile.Width);
				top = Math.Min(top, tile.Y);
				bottom = Math.Max(bottom, tile.Y + tile.Height);
			}

			if (tiles.Count <= 12)
				return false;

			foreach (Tile tile in tiles)
			{
				tile.X -= left;
				tile.Y -= top;
			}

			Init(right - left, bottom - top);

			return true;
		}
		public bool Load(IList sessions)
		{
			int left = 0;
			int right = 0;
			int top = 0;
			int bottom = 0;
			foreach (Session session in sessions)
			{
				foreach (Tile tile in session.tiles)
				{
					Tile clone = new Tile(tile)
					{
						//clone.Image = tile.Image;
						X = tile.X - session.selected.X,
						Y = tile.Y - session.selected.Y
					};

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
					right = Math.Max(right, clone.X + tile.Width);
					top = Math.Min(top, clone.Y);
					bottom = Math.Max(bottom, clone.Y + tile.Height);

					if (tile == session.selected)
						selected = clone;
				}
			}

			foreach (Tile tile in tiles)
			{ 
				tile.X -= left;
				tile.Y -= top;
			}

			Init(right - left, bottom - top);

			return true;
		}

		void Init(int w, int h)
		{
			float zoomStep = 1.25f;
			float zoomMin = 0.5f;
			float zoomMax = Math.Max(1f, Math.Max(w / 1000f, h / 1000f));
			zooms = new float[(int)Math.Log(zoomMax, zoomStep) + 1];
			for (int i = 0; i < zooms.Length - 2; i++)
				zooms[i] = (float)Math.Pow(zoomStep, zooms.Length - 1 - i);
			zooms[zooms.Length - 2] = 1;
			zooms[zooms.Length - 1] = zoomMin;
			zoom = 0;

			r = new RectangleF(0, 0, w, h);
		}
		Tile Find(int x, int y)
		{ 
			float xx = fov.Left + (x - p0.X) * zooms[zoom];
			float yy = fov.Top + (y - p0.Y) * zooms[zoom];

			foreach (Tile tile in tiles)
				if (new RectangleF(tile.X, tile.Y, tile.Width, tile.Height).Contains(xx, yy))
					return tile;

			return null;
		}

		public void Hit(int x, int y)
		{
			Tile hit = Find(x, y);

			selected = hit != null && selected == hit ? null : hit;
		}
		public void SetFOV(int w, int h)
		{
			p = new RectangleF(0, 0, w, h);

			float w0 = Math.Min(w, r.Width / zooms[zoom]);
			float h0 = Math.Min(h, r.Height / zooms[zoom]);

			p0 = new PointF((p.Width - w0) / 2, (p.Height - h0) / 2);

			fov = new RectangleF(
				Math.Max(0, Math.Min(fov.Left, r.Right - w0 * zooms[zoom])),
				Math.Max(0, Math.Min(fov.Top, r.Bottom - h0 * zooms[zoom])),
				w0 * zooms[zoom], h0 * zooms[zoom]);
		}
		public void SetZoom(int z, int x, int y)
		{
			float xx = fov.Left + (x - p0.X) * zooms[zoom];
			float yy = fov.Top + (y - p0.Y) * zooms[zoom];

			zoom = Math.Max(0, Math.Min(z - ZoomMin, zooms.Length - 1));

			float w0 = Math.Min(p.Width, r.Width / zooms[zoom]);
			float h0 = Math.Min(p.Height, r.Height / zooms[zoom]);

			p0 = new PointF((p.Width - w0) / 2, (p.Height - h0) / 2);

			this.fov = new RectangleF(
				Math.Max(0, Math.Min(xx - (x - p0.X) * zooms[zoom], r.Right - w0 * zooms[zoom])),
				Math.Max(0, Math.Min(yy - (y - p0.Y) * zooms[zoom], r.Bottom - h0 * zooms[zoom])),
				w0 * zooms[zoom], h0 * zooms[zoom]);
		}
		public void Move(int x, int y)
		{ 
			fov = new RectangleF(
				Math.Max(0, Math.Min(fov.Left - x * zooms[zoom], r.Right - fov.Width)),
				Math.Max(0, Math.Min(fov.Top - y * zooms[zoom], r.Bottom - fov.Height)), 
				fov.Width, fov.Height);
		}
		public void Draw(Graphics g)
		{
			//g.FillRectangle(Brushes.Blue, p);

			foreach (Tile tile in tiles)
			{
				if (!fov.IntersectsWith(new RectangleF(tile.X, tile.Y, tile.Width, tile.Height)))
					continue;

				RectangleF dest = new RectangleF(p0.X + (tile.X - fov.Left) / zooms[zoom], p0.Y + (tile.Y - fov.Top) / zooms[zoom], tile.Width / zooms[zoom], tile.Height / zooms[zoom]);

				g.DrawImage(tile.Image, dest, new RectangleF(0, 0, tile.Width, tile.Height), GraphicsUnit.Pixel);

			}

			if (selected != null)
			{
				RectangleF dest = new RectangleF(p0.X + (selected.X - fov.Left) / zooms[zoom], 
					p0.Y + (selected.Y - fov.Top) / zooms[zoom], 
					selected.Width / zooms[zoom], selected.Height / zooms[zoom]);

				if (zooms[zoom] <= 5)
				{
					g.DrawRectangle(Pens.White, dest.Left, dest.Top, dest.Width - 1, dest.Height - 1);
					g.FillRectangle(new HatchBrush(HatchStyle.DiagonalCross, Color.White, Color.Transparent),
						dest.Left, dest.Top, dest.Width - 1, dest.Height - 1);
				}
				else
				{
					g.FillRectangle(Brushes.White, dest.Left, dest.Top, dest.Width - 1, dest.Height - 1);
				}
			}
		}
		public void Save(string directory)
		{
			string mapdir = Path.Combine(directory, Name);
			Directory.CreateDirectory(mapdir);
			if (selected == null)
			{
				foreach (Tile tile in tiles)
					tile.Image.Save(Path.Combine(mapdir, string.Format("tile_{0}_{1}.png", tile.X / tile.Width, tile.Y / tile.Height)), ImageFormat.Png);
			}
			else
			{ 
				foreach (Tile tile in tiles)
					tile.Image.Save(Path.Combine(mapdir, string.Format("tile_{0}_{1}.png", (tile.X - selected.X) / tile.Width, (tile.Y - selected.Y) / tile.Height)), ImageFormat.Png);
			}

			try
			{
				using (Image i = new Bitmap((int)r.Width, (int)r.Height))
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
			return string.Format("{0} [{1}] [{2}x{3}]", Name, tiles.Count, r.Width, r.Height);
		}

		public int Width
		{
			get { return (int)r.Width; }
		}
		public int Height
		{
			get { return (int)r.Height; }
		}
		public int FOVLeft
		{
			get { return (int)fov.Left; }
			set { fov = new RectangleF(Math.Max(0, Math.Min(value, r.Right - fov.Width)), fov.Top, fov.Width, fov.Height); }
		}
		public int FOVTop
		{
			get { return (int)fov.Top; }
			set { fov = new RectangleF(fov.Left, Math.Max(0, Math.Min(value, r.Bottom - fov.Height)), fov.Width, fov.Height); }
		}
		public int FOVWidth
		{
			get { return (int)fov.Width; }
		}
		public int FOVHeight
		{
			get { return (int)fov.Height; }
		}
		public int Zoom
		{
			get { return ZoomMin + zoom; }
		}
		public int ZoomMin
		{
			get { return 2 - zooms.Length; }
		}
		public int ZoomMax
		{
			get { return 1; }
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
