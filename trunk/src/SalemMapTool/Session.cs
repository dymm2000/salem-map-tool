﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections;

namespace SalemMapTool
{
	public class Session
	{
		public enum Inheritance
		{
			Crop,
			Cut
		}

	    public class Tile
		{
			//byte[] bytes;
	        readonly Image image;
		    private readonly byte[] _bytes;
			public Tile(byte[] bytes)
			{
				_bytes = bytes;
				using (var m = new MemoryStream(bytes))
					image = Image.FromStream(m); 
			}
			//public Tile(Tile tile): this(tile.bytes)
			//{
			//    this.X = tile.X;
			//    this.Y = tile.Y;
			//    this.Width = tile.Width;
			//    this.Height = tile.Height;
			//}
			public Tile(Tile tile)
			{
				X = tile.X;
				Y = tile.Y;
				Width = tile.Width;
				Height = tile.Height;
				image = tile.image;
			    _bytes = tile.Bytes;
			}

			public int X { get; set; }
			public int Y { get; set; }
			public int Width { get; set; }
			public int Height { get; set; }
			public Image Image 
			{
				get 
				{
					//using (MemoryStream m = new MemoryStream(bytes))
					//    return Image.FromStream(m); 
					return image;
				} 
			}
		    public byte[] Bytes
		    {
		        get { return _bytes; }
		    }

			public override string ToString()
			{
				return string.Format("{0}, {1}", X, Y);
			}
		}

		int tileSize = 100;

		int izoom;
		float zoom;
		Array zooms;
		RectangleF r; // original map
		RectangleF fov; // field of view
		RectangleF p; // area of drawing
		PointF p0; // origin of actual drawing
		PointF s0; // selection start
		Tile chosen = null;
		bool selecting = false;
		RectangleF selection;
		List<Tile> tiles = new List<Tile>();

        public Dictionary<string, Tile> GenerateHash()
        {
            var result = new Dictionary<string, Tile>();


            using (MD5 md5 = MD5.Create())
            {
                foreach (var tile in tiles)
                {
                    var hashBytes = md5.ComputeHash(tile.Bytes);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(hashBytes[i].ToString("X2"));
                    }

                    string hash = sb.ToString();
                    if (!result.ContainsKey(hash))
                        result.Add(hash, tile);
                }
            }

            return result;
        }

		public Session(string name)
		{ 
			Name = name;
		}
		public bool Load(string path, uint minSize)
		{
			int left = int.MaxValue;
			int right = int.MinValue;
			int top = int.MaxValue;
			int bottom = int.MinValue;
			foreach (string f in Directory.GetFiles(path, "tile_*.png"))
			{
			    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(f);
			    if (fileNameWithoutExtension == null) continue;
			    
                var filename = fileNameWithoutExtension.Split('_');
			    var tile = new Tile(File.ReadAllBytes(f));
			    //tile.Image = Image.FromFile(f);
			    tile.Width = tileSize;
			    tile.Height = tileSize;
			    tile.X = int.Parse(filename[1]) * tile.Width;
			    tile.Y = int.Parse(filename[2]) * tile.Height;

			    tiles.Add(tile);

			    left = Math.Min(left, tile.X);
			    right = Math.Max(right, tile.X + tile.Width);
			    top = Math.Min(top, tile.Y);
			    bottom = Math.Max(bottom, tile.Y + tile.Height);
			}

		    if (tiles.Count < minSize)
				return false;

			foreach (Tile tile in tiles)
			{
				if (tile.X == 0 && tile.Y == 0)
					chosen = tile;

				tile.X -= left;
				tile.Y -= top;
			}

			Init(right - left, bottom - top);

			return true;
		}
		public bool Load(IList sessions)
		{
			int left = int.MaxValue;
			int right = int.MinValue;
			int top = int.MaxValue;
			int bottom = int.MinValue;
			foreach (Session session in sessions)
			{
				foreach (Tile tile in session.tiles)
				{
					Tile clone = new Tile(tile)
					{
						//clone.Image = tile.Image;
						X = tile.X - session.chosen.X,
						Y = tile.Y - session.chosen.Y
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

					if (tile == session.chosen)
						chosen = clone;
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
		public bool Load(Session session, Inheritance inheritance)
		{
			int left = int.MaxValue;
			int right = int.MinValue;
			int top = int.MaxValue;
			int bottom = int.MinValue;
			foreach (Tile tile in session.tiles)
			{
				if (session.selection.Contains(tile.X, tile.Y) ^ inheritance == Inheritance.Crop)
					continue;

				Tile clone = new Tile(tile);

				tiles.Add(clone);

				left = Math.Min(left, clone.X);
				right = Math.Max(right, clone.X + tile.Width);
				top = Math.Min(top, clone.Y);
				bottom = Math.Max(bottom, clone.Y + tile.Height);

				if (tile == session.chosen)
					chosen = clone;
			}

			foreach (Tile tile in tiles)
			{ 
				tile.X -= left;
				tile.Y -= top;
			}

			Init(right - left, bottom - top);

			return tiles.Count > 0;
		}

		void Init(int w, int h)
		{
			var zoomStep = (float)Math.Sqrt(2);
			var zoomMax = 2;
			var zoomMin = 
                Math.Min(1f, (float)Math.Pow(zoomStep, Math.Floor(Math.Log(1000f / Math.Max(w, h), zoomStep))));

			zooms = Array.CreateInstance(typeof(float), 
				new[] { (int)Math.Ceiling(Math.Log(zoomMax, zoomStep)) - (int)Math.Ceiling(Math.Log(zoomMin, zoomStep)) },
				new[] { (int)Math.Ceiling(Math.Log(zoomMin, zoomStep)) });
			
			for (int i = ZoomMin; i <= ZoomMax; i++)
				zooms.SetValue((float)Math.Pow(zoomStep, i), i);

			izoom = ZoomMin;
			zoom = (float)zooms.GetValue(izoom);

			r = new RectangleF(0, 0, w, h);
			selection = r;
		}
		Tile Find(int x, int y)
		{ 
			float xx = fov.Left + (x - p0.X) / zoom;
			float yy = fov.Top + (y - p0.Y) / zoom;

			foreach (Tile tile in tiles)
				if (new RectangleF(tile.X, tile.Y, tile.Width, tile.Height).Contains(xx, yy))
					return tile;

			return null;
		}

		public void StartSelect(int x, int y)
		{
			selecting = true;

			float xx = fov.Left + (x - p0.X) / zoom;
			float yy = fov.Top + (y - p0.Y) / zoom;

			s0 = new PointF(xx, yy);

			selection = new RectangleF(xx, yy, 0, 0);
		}
		public void EndSelect(bool cancel)
		{ 
			selecting = false;

			if (cancel)
				selection = r;
		}
		public void Choose(int x, int y)
		{
			Tile hit = Find(x, y);

			chosen = hit != null && chosen == hit ? null : hit;
		}
		public void SetFOV(Rectangle area)
		{
			p = area;

			float w0 = Math.Min(area.Width / zoom, r.Width);
			float h0 = Math.Min(area.Height / zoom, r.Height);

			p0 = new PointF(p.Left + (p.Width - w0 * zoom) / 2, p.Top + (p.Height - h0 * zoom) / 2);

			fov = new RectangleF(
				Math.Max(0, Math.Min(fov.Left, r.Right - w0)),
				Math.Max(0, Math.Min(fov.Top, r.Bottom - h0)),
				w0, h0);
		}
		public void SetZoom(int z, int x, int y)
		{
			float xx = fov.Left + (x - p0.X) / zoom;
			float yy = fov.Top + (y - p0.Y) / zoom;

			izoom = Math.Max(ZoomMin, Math.Min(z, ZoomMax));
			zoom = (float)zooms.GetValue(izoom);

			float w0 = Math.Min(p.Width / zoom, r.Width);
			float h0 = Math.Min(p.Height / zoom, r.Height);

			p0 = new PointF(p.Left + (p.Width - w0 * zoom) / 2, p.Top + (p.Height - h0 * zoom) / 2);

			fov = new RectangleF(
				Math.Max(0, Math.Min(xx - (x - p0.X) / zoom, r.Right - w0)),
				Math.Max(0, Math.Min(yy - (y - p0.Y) / zoom, r.Bottom - h0)),
				w0, h0);
		}
		public void Move(int x, int y)
		{ 
			if (selecting)
			{
				float xx = fov.Left + (x - p0.X) / zoom;
				float yy = fov.Top + (y - p0.Y) / zoom;

				selection = new RectangleF(xx > s0.X ? s0.X : xx, 
					yy > s0.Y ? s0.Y : yy,
					Math.Abs(xx - s0.X), Math.Abs(yy - s0.Y));

				selection = new RectangleF((float)Math.Floor(selection.Left / tileSize) * tileSize,
					(float)Math.Floor(selection.Top / tileSize) * tileSize,
					(float)(Math.Ceiling(selection.Right / tileSize) - Math.Floor(selection.Left / tileSize)) * tileSize, 
					(float)(Math.Ceiling(selection.Bottom / tileSize) - Math.Floor(selection.Top / tileSize)) * tileSize);

				selection.Intersect(r);
			}
			else 
			{
				fov = new RectangleF(
					Math.Max(0, Math.Min(fov.Left - x / zoom, r.Right - fov.Width)),
					Math.Max(0, Math.Min(fov.Top - y / zoom, r.Bottom - fov.Height)), 
					fov.Width, fov.Height);
			}
		}
		public void Draw(Graphics g)
		{
			RectangleF dst;
			RectangleF src;

			foreach (Tile tile in tiles)
			{
				src = new RectangleF(tile.X, tile.Y, tile.Width, tile.Height);
				src.Intersect(fov);
				
				if (src.Width.Equals(0) || src.Height.Equals(0))
					continue;

				dst = new RectangleF(p0.X + (tile.X - fov.Left) * zoom, p0.Y + (tile.Y - fov.Top) * zoom, tile.Width * zoom, tile.Height * zoom);
				dst.Intersect(p);

				src = new RectangleF(src.Left - tile.X, src.Top - tile.Y, src.Width, src.Height);

				g.DrawImage(tile.Image, dst, src, GraphicsUnit.Pixel);
			}

			if (chosen != null)
			{
				dst = new RectangleF(p0.X + (chosen.X - fov.Left) * zoom, 
					p0.Y + (chosen.Y - fov.Top) * zoom, 
					chosen.Width * zoom, chosen.Height * zoom);
				dst.Intersect(p);

				if (zoom > 0.2)
				{
					g.DrawRectangle(Pens.White, dst.Left, dst.Top, dst.Width - 1, dst.Height - 1);
					g.FillRectangle(new HatchBrush(HatchStyle.DiagonalCross, Color.White, Color.Transparent),
						dst.Left, dst.Top, dst.Width - 1, dst.Height - 1);
				}
				else
				{
					g.FillRectangle(Brushes.White, dst.Left, dst.Top, dst.Width - 1, dst.Height - 1);
				}
			}

			if (!selection.Width.Equals(r.Width) || !selection.Height.Equals(r.Height))
			{
				dst = new RectangleF(p0.X + (selection.Left - fov.Left) * zoom,
						p0.Y + (selection.Top - fov.Top) * zoom,
						selection.Width * zoom - 1, selection.Height * zoom - 1);
				dst.Intersect(p);
				g.DrawRectangle(new Pen(Brushes.Yellow, 3), dst.Left, dst.Top, dst.Width, dst.Height);
			}
		}
		public void Save(ExportParams exportParams)
		{
			//if (Directory.Exists(exportParams.Directory))
			//    throw new Exception(string.Format("Directory exists:\n{0}", exportParams.Directory));
			//Directory.CreateDirectory(exportParams.Directory);

			if (exportParams.ExportTiles)
			{
				string mapdir = Path.Combine(exportParams.Directory, Name);
				if (Directory.Exists(mapdir))
					throw new Exception(string.Format("Directory exists:\n{0}", mapdir));
				Directory.CreateDirectory(mapdir);

				if (chosen == null)
				{
					foreach (Tile tile in tiles)
						tile.Image.Save(Path.Combine(mapdir, string.Format("tile_{0}_{1}.png", tile.X / tile.Width, tile.Y / tile.Height)), ImageFormat.Png);
				}
				else
				{
					foreach (Tile tile in tiles)
						tile.Image.Save(Path.Combine(mapdir, string.Format("tile_{0}_{1}.png", (tile.X - chosen.X) / tile.Width, (tile.Y - chosen.Y) / tile.Height)), ImageFormat.Png);
				}
			}

			if (exportParams.ExportMap)
			{ 
				var mapfile = Path.Combine(exportParams.Directory,
                    String.Concat(Name, (Equals(exportParams.Format, ImageFormat.Png) ? ".png" : ".jpg")));

				if (File.Exists(mapfile))
					throw new Exception(string.Format("File exists:\n{0}", mapfile));

				try
				{
					using (Image i = new Bitmap((int)r.Width, (int)r.Height))
					{
						using (Graphics g = Graphics.FromImage(i))
						{
							foreach (Tile tile in tiles)
								g.DrawImage(tile.Image, tile.X, tile.Y);

							if (exportParams.ShowGrid)
							{ 
								if (chosen == null)
								{
									foreach (Tile tile in tiles)
									{
										g.DrawRectangle(Pens.White, tile.X, tile.Y, tile.Width, tile.Height);
										g.DrawString(string.Format("({0},{1})", tile.X / tile.Width, tile.Y / tile.Height), new Font(FontFamily.GenericSansSerif, 8), Brushes.White, tile.X + 2, tile.Y + 2);
									}
								}
								else
								{ 
									foreach (Tile tile in tiles)
									{
										g.DrawRectangle(Pens.White, tile.X, tile.Y, tile.Width, tile.Height);
										g.DrawString(string.Format("({0},{1})", (tile.X - chosen.X) / tile.Width, (tile.Y - chosen.Y) / tile.Height), new Font(FontFamily.GenericSansSerif, 8), Brushes.White, tile.X + 2, tile.Y + 2);
									}
								}
							}
						}

						i.Save(mapfile, exportParams.Format);
					}
				}
				catch (Exception e)
				{
					throw new Exception(string.Format("Whole map can not be saved:\n{0}", e.Message));
				}
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
			get { return izoom; }
		}
		public int ZoomMin
		{
			get { return zooms.GetLowerBound(0); }
		}
		public int ZoomMax
		{
			get { return zooms.GetUpperBound(0); }
		}
		public string Name
		{
			get;
			private set;
		}
		public bool CanMerge
		{
			get { return chosen != null; }
		}
	}
}
