using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinorShift.Emuera.GameProc.PluginSystem
{
	public class BasePluginManifest
	{
		public BasePluginManifest() { }

		public List<IPluginMethod> GetPluginMethods()
		{
			return methods;
		}

		protected List<IPluginMethod> methods = new List<IPluginMethod>();
	}
}
