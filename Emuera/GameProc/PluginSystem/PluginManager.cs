using MinorShift.Emuera.GameData.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinorShift.Emuera.GameProc.PluginSystem
{
	public class PluginManager
	{
		private PluginManager() { }
		public static PluginManager GetInstance()
		{
			if (instance == null)
			{
				instance = new PluginManager();
			}

			return instance;
		}

		static private PluginManager instance = null;

		public void AddMethod(IPluginMethod method)
		{
			methods.Add(method.Name, method);
		}

		public IPluginMethod GetMethod(string name)
		{
			return methods[name];
		}

		public bool HasMethod(string name)
		{
			return methods.ContainsKey(name);
		}

		private Dictionary<string, IPluginMethod> methods = new Dictionary<string, IPluginMethod>();

	}
}
