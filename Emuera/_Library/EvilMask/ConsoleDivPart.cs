﻿using EvilMask.Emuera;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EvilMask.Emuera.Shape;
using static EvilMask.Emuera.Utils;

namespace MinorShift.Emuera.GameView
{
	class ConsoleDivPart : AConsoleDisplayPart
	{
		public ConsoleDivPart(MixedNum xPos, MixedNum yPos, MixedNum width, MixedNum height, int depth, int color, StyledBoxModel box, bool isRelative, ConsoleDisplayLine[] childs)
		{
			backgroundColor = color >= 0 ? Color.FromArgb((int)(color | 0xff000000)) : Color.Transparent;
			StringBuilder sb = new StringBuilder();
			width.num = Math.Abs(width.num);
			height.num = Math.Abs(height.num);
			sb.Append("<div");
			Utils.AddTagMixedNumArg(sb, "xpos", xPos);
			Utils.AddTagMixedNumArg(sb, "ypos", yPos);
			Utils.AddTagMixedNumArg(sb, "width", width);
			Utils.AddColorParam(sb, "color", backgroundColor);
			Utils.AddTagMixedNumArg(sb, "height", height);
			if (box != null)
			{
				Utils.AddTagMixedParam(sb, "margin", box.margin);
				Utils.MixedNum4ToInt4(box.margin, ref margin);
				Utils.AddTagMixedParam(sb, "padding", box.padding);
				Utils.MixedNum4ToInt4(box.padding, ref padding);
				Utils.AddTagMixedParam(sb, "border", box.border);
				Utils.MixedNum4ToInt4(box.border, ref border);
				Utils.AddTagMixedParam(sb, "radius", box.radius);
				Utils.MixedNum4ToInt4(box.radius, ref radius);
				if (box.color != null)
				{
					borderColors = new Color[4];
					for (int i = 0; i < 4; i++)
						borderColors[i] = box.color[i] >= 0 ? Color.FromArgb((int)(box.color[i] | 0xff000000)) : Color.Transparent;
					Utils.AddColorParam4(sb, "bcolor", borderColors);
				}
			}
			sb.Append(">");
			altHeadTag = sb.ToString();
			Str = string.Empty;
			xOffset = MixedNum.ToPixel(xPos, 0);
			PointY = MixedNum.ToPixel(yPos, 0);
			this.width = MixedNum.ToPixel(width, 0);
			Height = MixedNum.ToPixel(height, 0);
			children = childs;
			Depth = depth;
			IsRelative = isRelative;
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
		int[] margin, padding, radius, border;
		Color[] borderColors;
		Color backgroundColor;
		string altHeadTag;
		readonly ConsoleDisplayLine[] children;
		public bool IsEscaped { get; set; } = false;
		public override int Top { get { return PointY; } }
		public override int Bottom { get { return PointY + Height; } }
		public bool IsRelative { get; private set; }

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
			var rect = IsRelative ? new Rectangle(PointX + xOffset, pointY + PointY, width, Height) 
				: new Rectangle(xOffset, PointY, width, Height);

			if (margin != null)
				rect = new Rectangle(rect.X + margin[Direction.Left], rect.Y + margin[Direction.Top],
					 rect.Width - margin[Direction.Left] - margin[Direction.Right], rect.Height - margin[Direction.Top] - margin[Direction.Bottom]);
			graph.SetClip(rect, CombineMode.Replace);

			Shape.BoxBorder.DrawBorder(graph, rect, border, radius, borderColors, backgroundColor);

			if (border != null)
				rect = new Rectangle(rect.X + border[Direction.Left], rect.Y + border[Direction.Top],
					 rect.Width - border[Direction.Left] - border[Direction.Right], rect.Height - border[Direction.Top] - border[Direction.Bottom]);

			if (padding != null)
				rect = new Rectangle(rect.X + padding[Direction.Left], rect.Y + padding[Direction.Top],
					 rect.Width - padding[Direction.Left] - padding[Direction.Right], rect.Height - padding[Direction.Top] - padding[Direction.Bottom]);

			graph.SetClip(rect, CombineMode.Replace);

			foreach (var child in children)
			{
				child.DrawTo(graph, rect.Y, isBackLog, true, mode);
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
				line.BuildString(sb);
				sb.Append("\r\n");
			}
			sb.Append("</div>");
			return sb.ToString();
		}
		public override StringBuilder BuildString(StringBuilder sb)
		{
			sb.Append(altHeadTag);
			foreach (var line in children)
			{
				line.BuildString(sb);
				sb.Append("\r\n");
			}
			sb.Append("</div>");
			return sb;
		}
	}
}
