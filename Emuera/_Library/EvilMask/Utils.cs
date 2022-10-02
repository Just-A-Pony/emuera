﻿using System.Drawing;
using System.IO;

namespace EvilMask.Emuera
{
	internal sealed class Utils
	{
		public static string GetValidPath(string path)
		{
			path =  path.Replace('/', '\\').Replace("..\\", "");
			try
			{ 
				if (Path.GetPathRoot(path) != string.Empty)
					return null;
			}
			catch
			{
				return null;
			}
			return path;
		}

		// filepathの安全性(ゲームフォルダ以外のフォルダか)を確認しない
		static public Bitmap LoadImage(string filepath)
		{
			Bitmap bmp = null;
			FileStream fs = null;
			if (!File.Exists(filepath)) return null;

			try
			{
				fs = new FileStream(filepath, FileMode.Open);
				var factory = new ImageProcessor.ImageFactory();
				factory.Load(fs);
				bmp = (Bitmap)factory.Image;
			}
			catch { }
			finally
			{
				fs?.Close();
				fs?.Dispose();
			}
			return bmp;

		}
		// ビットマップファイルからアイコンファイルをつくる
		public static Icon MakeIconFromBmpFile(Bitmap bmp)
		{
			Image img = bmp;
 
			Bitmap bitmap = new Bitmap(32, 32, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			Graphics g = Graphics.FromImage(bitmap);
			g.DrawImage(img, new Rectangle(0, 0, 32, 32));
			g.Dispose();
 
			Icon icon = Icon.FromHandle(bitmap.GetHicon());
 
			img.Dispose();
			bitmap.Dispose();
			return icon;
		}
	}
}
