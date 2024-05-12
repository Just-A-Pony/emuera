using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinorShift.Emuera.GameProc.PluginSystem
{
	public class PluginMethodParameter
	{
		public PluginMethodParameter(string initialValue = "")
		{
			value = initialValue;
		}

		public string value;
	}
}
