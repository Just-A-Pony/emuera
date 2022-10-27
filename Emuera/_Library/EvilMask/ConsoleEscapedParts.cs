using MinorShift.Emuera;
using MinorShift.Emuera.GameView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvilMask.Emuera
{
	internal sealed class ConsoleEscapedParts
	{
		static readonly DataTable dt = new DataTable();
		static readonly Dictionary<Int64, AConsoleDisplayPart> parts = new Dictionary<long, AConsoleDisplayPart>();
		static ConsoleEscapedParts()
		{
			dt.Columns.Add("line", typeof(int));
			dt.Columns.Add("depth", typeof(int));
			dt.Columns.Add("top", typeof(int));
			dt.Columns.Add("bottom", typeof(int));
			dt.Columns.Add("id", typeof(Int64));
		}
		public static void Clear()
		{
			dt.Clear();
			parts.Clear();
		}
		public static void Add(AConsoleDisplayPart part, int line, int depth, int top, int bottom)
		{
			var id = Utils.TimePoint();
			var row = dt.NewRow();
			row[0] = line;
			row[1] = depth;
			row[2] = top;
			row[3] = bottom;
			row[4] = id;
			dt.Rows.Add(row);
			parts.Add(id, part);
		}
		public static void Remove(int line)
		{
			foreach (var row in DataTableExtensions.AsEnumerable(dt).Where(r => (int)r[0] >= line).ToArray())
			{
				parts.Remove((Int64)row[4]);
				dt.Rows.Remove(row);
			}
		}
		public static void RemoveAt(int line)
		{
			foreach (var row in DataTableExtensions.AsEnumerable(dt).Where(r => (int)r[0] == line).ToArray())
			{
				parts.Remove((Int64)row[4]);
				dt.Rows.Remove(row);
			}
		}
		public static void GetPartsInRange(int top, int bottom, Dictionary<int, List<AConsoleDisplayPart>> rmap)
		{
			rmap.Clear();
			foreach (var row in DataTableExtensions.AsEnumerable(dt)
				.Where(r => (int)r[2] <= bottom && (int)r[3] >= top && r[0] is int line 
				&& ((int)r[1] !=0 || top > line || line > bottom)))
			{
				List<AConsoleDisplayPart> list = null;
				rmap.TryGetValue((int)row[1], out list);
				if (list == null)
				{
					list = new List<AConsoleDisplayPart>();
					rmap.Add((int)row[1], list);
				}
				list.Add(parts[(Int64)row[4]]);
			}
		}
	}
}
