using System.Collections.Generic;
using System.Drawing;
using MinorShift.Emuera;

namespace Emuera
{
	internal class FontFactory
	{

		static readonly Dictionary<(string fontname, FontStyle fontStyle), Font> fontDic = [];

		public static Font GetFont(string requestFontName, FontStyle style)
		{
			string fontname = requestFontName;
			if (string.IsNullOrEmpty(requestFontName))
				fontname = Config.FontName;
			if (!fontDic.ContainsKey((fontname, style)))
			{
				var font = new Font(fontname, Config.FontSize, style, GraphicsUnit.Pixel);
				if (font == null)
				{
					return null;
				}
				else
				{
					fontDic.Add((fontname, style), font);
				}

			}
			#region EE_フォントファイル対応
			int fontsize = Config.FontSize;
			Font styledFont;
			foreach (FontFamily ff in GlobalStatic.Pfc.Families)
			{
				if (ff.Name == fontname)
				{
					styledFont = new Font(ff, fontsize, style, GraphicsUnit.Pixel);
					break;
				}
			}
			#endregion
			return fontDic[(fontname, style)];

			/**
			string fn = theFontname;
			if (string.IsNullOrEmpty(theFontname))
				fn = FontName;
			if (!fontDic.ContainsKey(fn))
				fontDic.Add(fn, new Dictionary<FontStyle, Font>());
			Dictionary<FontStyle, Font> fontStyleDic = fontDic[fn];
			if (!fontStyleDic.ContainsKey(style))
				{
				int fontsize = FontSize;
				Font styledFont;
				try
				{
				#region EE_フォントファイル対応
				foreach (FontFamily ff in GlobalStatic.Pfc.Families)
				{
						if (ff.Name == fn)
					{
						styledFont = new Font(ff, fontsize, style, GraphicsUnit.Pixel);
						goto foundfont;
					}
				}
				styledFont = new Font(fn, fontsize, style, GraphicsUnit.Pixel);
				}
				catch
				{
					return null;
				}
			foundfont:
				#endregion
				fontStyleDic.Add(style, styledFont);
			return fontStyleDic[style];
			**/
		}

		public static void ClearFont()
		{
			foreach (var font in fontDic)
			{
				font.Value.Dispose();
			}
			fontDic.Clear();
		}
	}
}
