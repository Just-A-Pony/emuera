using EvilMask.Emuera;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinorShift.Emuera.GameView
{
	class ConsoleDivPart : AConsoleDisplayPart
	{
		public ConsoleDivPart(MixedNum xPos, MixedNum yPos, MixedNum width, MixedNum height, int depth, int color, ConsoleDisplayLine[] childs)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<div");
			Utils.AddTagMixedNumArg(sb, "xpos", xPos);
			Utils.AddTagMixedNumArg(sb, "ypos", yPos);
			Utils.AddTagMixedNumArg(sb, "width", width);
			Utils.AddTagMixedNumArg(sb, "height", height);
			sb.Append(">");
			altHeadTag = sb.ToString();
			Str = string.Empty;
			xOffset = xPos != null ? (xPos.isPx ? xPos.num : xPos.num * Config.FontSize / 100) : 0;
			PointY = yPos != null ? (yPos.isPx ? yPos.num : yPos.num * Config.FontSize / 100) : 0;
			this.width = width.isPx ? width.num : width.num * Config.FontSize / 100;
			Height = height.isPx ? height.num : height.num * Config.FontSize / 100;
			children = childs;
			Depth = depth;
			this.color = color >= 0 ? Color.FromArgb((int)(color | 0xff000000)) : Color.Transparent;
		}
		int pointX = 0;
		int xOffset;
		int width;
		public override int PointX {
			get { return pointX; }
			set { pointX = value;
				foreach (var child in children)
					child.ShiftPositionX(value + xOffset);
			} }
		int PointY;
		int Height;
		Color color;
		string altHeadTag;
		readonly ConsoleDisplayLine[] children;

		public override int Top { get { return PointY; } }
		public override int Bottom { get { return PointY + Height; } }

		public override bool CanDivide => false;
		public ConsoleButtonString TestChildHitbox(int pointX, int pointY, int relPointY)
		{
			ConsoleButtonString pointing = null;
			var rect = new Rectangle(PointX + xOffset, relPointY + PointY, width, Height);
			if (!rect.Contains(pointX, pointY)) return null;
			relPointY = rect.Y;
			foreach (var line in children)
			{
				for (int b = 0; b < line.Buttons.Length; b++)
				{
					ConsoleButtonString button = line.Buttons[line.Buttons.Length - b - 1];
					if (button == null || button.StrArray == null)
						continue;
					if ((button.PointX <= pointX) && (button.PointX + button.Width >= pointX))
					{
						//if (relPointY >= 0 && relPointY <= Config.FontSize)
						//{
						//	pointing = button;
						//	if(pointing.IsButton)
						//		goto breakfor;
						//}
						foreach (AConsoleDisplayPart part in button.StrArray)
						{
							if (part == null)
								continue;
							if ((part.PointX <= pointX) && (part.PointX + part.Width >= pointX)
								&& (relPointY + part.Top <= pointY) && (relPointY + part.Bottom >= pointY))
							{
								pointing = button;
								if (pointing.IsButton)
									return pointing;
							}
						}
					}
				}
				relPointY += Config.LineHeight;
			}
			return pointing;
		}
		public override void DrawTo(Graphics graph, int pointY, bool isSelecting, bool isBackLog, TextDrawingMode mode)
		{
			var rect = new Rectangle(PointX + xOffset, pointY + PointY, width, Height);
			graph.SetClip(rect, System.Drawing.Drawing2D.CombineMode.Replace);
			if (color != Color.Transparent)
				graph.FillRectangle(new SolidBrush(color), rect);
			foreach (var child in children)
			{
				child.DrawTo(graph, pointY + PointY, isBackLog, true, mode);
				pointY += Config.LineHeight;
			}
			graph.ResetClip();
		}

		public override void GDIDrawTo(int pointY, bool isSelecting, bool isBackLog)
		{
			// WINAPI では使えない
		}

		public override void SetWidth(StringMeasure sm, float subPixel)
		{
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(altHeadTag);
			foreach (var line in children)
			{
				sb.Append(line.ToString());
				sb.Append("\r\n");
			}
			sb.Append("</div>");
			return sb.ToString();
		}
	}
}
