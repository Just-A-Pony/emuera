using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinorShift.Emuera.Content;

internal abstract class AbstractImage : IDisposable
{
	public const int MAX_IMAGESIZE = 8192;
	public abstract Bitmap Bitmap { get; set; }
	public IntPtr GDIhDC { get; protected set; }
	protected Graphics g;

	public abstract bool IsCreated { get; }

	public abstract void Dispose();
}
