using MinorShift.Emuera.GameData.Expression;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MinorShift.Emuera.GameProc.PluginSystem
{
	public class PluginManager
	{
		private PluginManager() {

		}
		public static PluginManager GetInstance()
		{
			if (instance == null)
			{
				instance = new PluginManager();
			}

			return instance;
		}

		static private PluginManager instance = null;

		public void LoadPlugins()
		{
			string[] plugins = Directory.GetFiles("Plugins", "*.dll");
			ClearMethods();
			foreach (var pluginPath in plugins)
			{
				Assembly DLL = Assembly.LoadFrom(pluginPath);
				var manifestType = DLL.GetTypes().Where((v) => v.Name == "PluginManifest").FirstOrDefault();
				if (manifestType == null)
				{
					//throw warning
					continue;
				}

				BasePluginManifest manifest = (BasePluginManifest)Activator.CreateInstance(manifestType);
				if (manifest == null)
				{
					//throw warning
					continue;
				}

				var methods = manifest.GetPluginMethods();
				foreach (var method in methods)
				{
					AddMethod(method);
				}
			}
		}

		public void ClearMethods()
		{
			methods.Clear();
		}

		public void AddMethod(IPluginMethod method)
		{
			var key = method.Name;
			if (Config.ICFunction)
			{
				key = key.ToUpper();
			}
			methods.Add(key, method);
		}

		public IPluginMethod GetMethod(string name)
		{
			var key = name;
			if (Config.ICFunction)
			{
				key = key.ToUpper();
			}
			return methods[key];
		}

		public bool HasMethod(string name)
		{
			var key = name;
			if (Config.ICFunction)
			{
				key = key.ToUpper();
			}
			return methods.ContainsKey(key);
		}

		private Dictionary<string, IPluginMethod> methods = new Dictionary<string, IPluginMethod>();

	}
}
