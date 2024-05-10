using MinorShift.Emuera.GameData;
using MinorShift.Emuera.GameData.Expression;
using MinorShift.Emuera.GameData.Variable;
using MinorShift.Emuera.GameProc.Function;
using MinorShift.Emuera.Sub;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using static EvilMask.Emuera.Utils;
using static MinorShift.Emuera.GameProc.Function.FunctionIdentifier;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

		public void Print(string text)
		{
			expressionMediator.Console.Print(text);
		}
		public void PrintError(string text)
		{
			expressionMediator.Console.PrintError(text);
		}
		public void PrintC(string text, bool aligmentRight = false)
		{
			expressionMediator.Console.PrintC(text, aligmentRight);
		}
		public void PrintPlain(string text)
		{
			expressionMediator.Console.PrintPlain(text);
		}
		public void PrintPlainWithSingleLine(string text)
		{
			expressionMediator.Console.PrintPlainwithSingleLine(text);
		}
		public void PrintSingleLine(string text)
		{
			expressionMediator.Console.PrintSingleLine(text);
		}
		public void PrintSystemLine(string text)
		{
			expressionMediator.Console.PrintSystemLine(text);
		}
		public void PrintTemporaryLine(string text)
		{
			expressionMediator.Console.PrintTemporaryLine(text);
		}
		public void PrintButton(string text, long id)
		{
			expressionMediator.Console.PrintButton(text, id);
		}
		public void PrintButtonC(string text, long id, bool aligmentRight = false)
		{
			expressionMediator.Console.PrintButtonC(text, id, aligmentRight);
		}
		public void PrintBar(string text = "", bool isConst = true)
		{
			if (text == "")
			{
				expressionMediator.Console.PrintBar();
			} else
			{
				expressionMediator.Console.printCustomBar(text, isConst);
			}
		}
		public void PrintHtml(string htmlText, bool toBuffer = false)
		{
			expressionMediator.Console.PrintHtml(htmlText, toBuffer);
		}
		public void PrintImage(string resourceName, int width, int height, int y, string buttonResourceName = null, string mapResourceName = null)
		{
			MixedNum widthNum = new MixedNum();
			widthNum.isPx = true;
			widthNum.num = width;
			MixedNum heightNum = new MixedNum();
			heightNum.isPx = true;
			heightNum.num = height;
			MixedNum yNum = new MixedNum();
			yNum.isPx = true;
			yNum.num = y;
			expressionMediator.Console.PrintImg(resourceName, buttonResourceName, mapResourceName, heightNum, widthNum, yNum);
		}
		public void PrintNewLine()
		{
			expressionMediator.Console.NewLine();
		}
		public void FlushConsole(bool force = false)
		{
			expressionMediator.Console.PrintFlush(force);
		}
		public void DebugPrint(string text)
		{
			expressionMediator.Console.DebugPrint(text);
		}

		public void ClearDisplay()
		{
			expressionMediator.Console.ClearDisplay();
		}
		public void SetBgColor(Color color)
		{
			expressionMediator.Console.SetBgColor(color);
		}
		public void SetFont(string fontName)
		{
			expressionMediator.Console.SetFont(fontName);
		}
		public Point GetMousePosition()
		{
			return expressionMediator.Console.GetMousePosition();
		}
		public void WaitInput(bool oneInput = true, int timelimit = -1)
		{
			InputRequest request = new InputRequest();
			request.OneInput = oneInput;
			request.Timelimit = timelimit;
			expressionMediator.Console.WaitInput(request);
		}
		public void ReadAnyKey()
		{
			expressionMediator.Console.ReadAnyKey();

		}
		public void Await(int time)
		{
			expressionMediator.Console.Await(time);
		}
		public void ForceStopTimer()
		{
			expressionMediator.Console.forceStopTimer();
		}
		public void Quit(bool force = false)
		{
			if (force)
			{
				expressionMediator.Console.ForceQuit();
			}  else
			{
				expressionMediator.Console.Quit();
			}
		}

		public long GetIntVar(string name, int index = 0)
		{
			return expressionMediator.VEvaluator.VariableData.GetVarTokenDic()[name].GetIntValue(expressionMediator, [index]);
		}
		public string GetStrVar(string name, int index = 0)
		{
			return expressionMediator.VEvaluator.VariableData.GetVarTokenDic()[name].GetStrValue(expressionMediator, [index]);
		}
		public void SetIntVar(string name, long val, int index = 0)
		{
			expressionMediator.VEvaluator.VariableData.GetVarTokenDic()[name].SetValue(val, [index]);
		}
		public void SetStrVar(string name, string val, int index = 0)
		{
			expressionMediator.VEvaluator.VariableData.GetVarTokenDic()[name].SetValue(val, [index]);
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

				PluginManifestAbstract manifest = (PluginManifestAbstract)Activator.CreateInstance(manifestType);
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

			InitializeBuiltinFunctions();
		}

		internal void InitializeBuiltinFunctions()
		{
			functionDictionary = FunctionIdentifier.GetInstructionNameDic();
			var maxValue = (int)Enum.GetValues(typeof(FunctionCode)).Cast<FunctionCode>().Last() + 1;
			systemFunctions = new List<FunctionIdentifier>(maxValue);
			for (int i = 0; i < maxValue; i++)
			{
				systemFunctions.Add(null);
			}
			systemFunctions[(int)FunctionCode.SET] = FunctionIdentifier.SETFunction;
			foreach (var code in Enum.GetValues(typeof(FunctionCode)))
			{
				var name = code.ToString();
				if (Config.ICFunction)
				{
					name = name.ToUpper();	
				}
				if (functionDictionary.ContainsKey(name))
				{
					systemFunctions[(int)code] = functionDictionary[name];
				}
			}
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
		private Dictionary<string, FunctionIdentifier> functionDictionary;
		private List<FunctionIdentifier> systemFunctions;

	}
}
