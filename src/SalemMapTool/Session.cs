using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections;

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

		string name;
		Image image;
		bool zoom = true;
		Tile selected = null;
		List<Tile> tiles = new List<Tile>();

		public Session(string name, string path)
		{
			this.name = name;

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

			image = new Bitmap(right - left, bottom - top);

			Draw();
		}
		public Session(string name, IList sessions)
		{
			this.name = name;

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

			image = new Bitmap(right - left, bottom - top);

			Draw();
		}

		void Draw()
		{ 
			using (Graphics g = Graphics.FromImage(image))
			{
				g.FillRectangle(Brushes.Blue, 0, 0, image.Width, image.Height);
				foreach (Tile tile in tiles)
					g.DrawImage(tile.Image, tile.X, tile.Y);

				if (selected != null)
				{
					g.DrawRectangle(Pens.White, selected.X, selected.Y, selected.Image.Width - 1, selected.Image.Height - 1);
					g.FillRectangle(new HatchBrush(HatchStyle.DiagonalCross, Color.White, Color.Transparent), selected.X, selected.Y, selected.Image.Width - 1, selected.Image.Height - 1);
				}
			}
		}

		public bool Zoom()
		{
			return zoom = !zoom;
		}
		public void Hit(int x, int y, int w, int h)
		{
			if (zoom)
				return;

			int x0 = (w - Math.Min(w, image.Width)) / 2;
			int y0 = (h - Math.Min(h, image.Height)) / 2;
			float xx = X * image.Width + x - x0;
			float yy = Y * image.Height + y - y0;

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

			using (Graphics g = Graphics.FromImage(image))
			{ 
				if (selected != null)
					g.DrawImage(selected.Image, selected.X, selected.Y);

				if (selected == hit)
				{
					selected = null;
				}
				else
				{
					selected = hit;

					g.DrawRectangle(Pens.White, selected.X, selected.Y, selected.Image.Width - 1, selected.Image.Height - 1);
					g.FillRectangle(new HatchBrush(HatchStyle.DiagonalCross, Color.White, Color.Transparent), selected.X, selected.Y, selected.Image.Width - 1, selected.Image.Height - 1);
				}
			}
		}
		public void Draw(Graphics g, int w, int h)
		{
			if (zoom)
			{
				float scale = Math.Min(w * 1f / image.Width, h * 1f / image.Height);
				float x0 = (w - scale * image.Width) / 2;
				float y0 = (h - scale * image.Height) / 2;
				g.DrawImage(image, new RectangleF(x0, y0, scale * image.Width, scale * image.Height), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
				g.DrawRectangle(Pens.Red, x0 + scale * X * image.Width, y0 + scale * Y * image.Height, scale * Math.Min(w, image.Width) - 1, scale * Math.Min(h, image.Height) - 1);
			}
			else
			{
				int x0 = (w - Math.Min(w, image.Width)) / 2;
				int y0 = (h - Math.Min(h, image.Height)) / 2;
				g.DrawImage(image, new Rectangle(x0, y0, w, h), new RectangleF(X * image.Width, Y * image.Height, w, h), GraphicsUnit.Pixel);
			}
		}
		public void Save(string directory)
		{
			if (selected != null)
			{ 
				using (Graphics g = Graphics.FromImage(image))
					g.DrawImage(selected.Image, selected.X, selected.Y);
			}
			image.Save(Path.Combine(directory, name + ".png"), ImageFormat.Png);
			if (selected != null)
			{
				using (Graphics g = Graphics.FromImage(image))
				{ 
					g.DrawRectangle(Pens.White, selected.X, selected.Y, selected.Image.Width - 1, selected.Image.Height - 1);
					g.FillRectangle(new HatchBrush(HatchStyle.DiagonalCross, Color.White, Color.Transparent), selected.X, selected.Y, selected.Image.Width - 1, selected.Image.Height - 1);
				}
			}

			string mapdir = Path.Combine(directory, name);
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
		}

		public override string ToString()
		{
			return string.Format("{0} [{1}] [{2}x{3}]", name, tiles.Count, image.Width, image.Height);
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
			get { return image == null ? 0 : image.Width; }
		}
		public int Height
		{
			get { return image == null ? 0 : image.Height; }
		}
		public string Name
		{
			get { return name; }
		}
		public bool CanMerge
		{
			get { return selected != null; }
		}
	}
}
