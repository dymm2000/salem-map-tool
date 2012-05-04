using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Drawing2D;

namespace TileGenerator
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length != 2)
				return;

			int w = int.Parse(args[0]);
			int h = int.Parse(args[1]);

			Image image = new Bitmap(100, 100);
			using (Graphics g = Graphics.FromImage(image))
			{
				g.FillRectangle(Brushes.Green, 0, 0, image.Width, image.Height);
				g.FillRectangle(new HatchBrush(HatchStyle.DiagonalCross, Color.White, Color.Transparent), 0, 0, image.Width - 1, image.Height - 1);
			}

			string dir = string.Format("..\\mockups\\Mockup {0}x{1}", w, h);
			Directory.CreateDirectory(dir);
			for (int i=0; i<w; i++)
				for (int j=0; j<h; j++)
					image.Save(Path.Combine(dir, string.Format("tile_{0}_{1}.png", i, j)), ImageFormat.Png);

		}
	}
}
