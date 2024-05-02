using MinorShift.Emuera.GameData.Expression;
using MinorShift.Emuera.GameProc.Function;
using MinorShift.Emuera.Sub;
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

		public static PluginManager GetInstance()
		{
			if (instance == null)
			{
				instance = new PluginManager();
			}

			return instance;
		}

		private PluginManager()
		{

		}

		static private PluginManager instance = null;

		/// <summary>
		/// Unsafe rudimentary method to execute ERB line of code from Plugin.
		/// Instead of using this method better implement direct API call that works directly with game state, bypassing ERB interpreter
		/// </summary>
		/// <param name="line">Line of code to execute</param>
		public void ExecuteLine(string line)
		{
			var logicalLine = LogicalLineParser.ParseLine(line, null);
			InstructionLine func = (InstructionLine)logicalLine;
			ArgumentParser.SetArgumentTo(func);
			func.Function.Instruction.DoInstruction(expressionMediator, func, processState);
		}

		/// <summary>
		/// Load all DLL plugins from Plugins directory of the game
		/// </summary>
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
					//TODO: throw warning
					continue;
				}

				BasePluginManifest manifest = (BasePluginManifest)Activator.CreateInstance(manifestType);
				if (manifest == null)
				{
					//TODO: throw warning
					continue;
				}

				var methods = manifest.GetPluginMethods();
				foreach (var method in methods)
				{
					AddMethod(method);
				}
			}
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

		internal void SetParent(Process process, ProcessState processState, ExpressionMediator expressionMediator)
		{
			this.process = process;
			this.processState = processState;
			this.expressionMediator = expressionMediator;
		}

		private void ClearMethods()
		{
			methods.Clear();
		}

		private void AddMethod(IPluginMethod method)
		{
			var key = method.Name;
			if (Config.ICFunction)
			{
				key = key.ToUpper();
			}
			methods.Add(key, method);
		}

		private Dictionary<string, IPluginMethod> methods = new Dictionary<string, IPluginMethod>();
		private Process process;
		private ProcessState processState;
		private ExpressionMediator expressionMediator;

	}
}
