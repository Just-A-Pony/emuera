using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MinorShift.Emuera.Runtime.Utils;
static partial class Preload
{
	static Dictionary<string, string[]> files = new(StringComparer.OrdinalIgnoreCase);

	public static string[] GetFileLines(string path)
	{
		return files[path];
	}

	// Opens as UTF8BOM if starts with BOM, else use DetectEncoding
	private static string[] readAllLinesDetectEncoding(string path)
	{
		using var file = File.Open(path, FileMode.Open);
		Span<byte> bom = stackalloc byte[3];
		_ = file.Read(bom);
		file.Close();
		if (bom.SequenceEqual<byte>([0xEF, 0xBB, 0xBF]))
		{
			return File.ReadAllLines(path, EncodingHandler.UTF8BOMEncoding);
		}
		else
		{
			return File.ReadAllLines(path, EncodingHandler.DetectEncoding(path));
		}
	}

	public static async Task Load(string path)
	{
		var startTime = DateTime.Now;
		Debug.WriteLine($"Load: {path} : Start");

		if (Directory.Exists(path))
		{
			await Task.Run(() =>
			{
				Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories).AsParallel().ForAll((childPath) =>
				{
					var key = childPath;
					var value = readAllLinesDetectEncoding(childPath);
					lock (files)
					{
						files[key] = value;
					}

				});
			});
		}
		else
		{
			var key = path;
			var value = readAllLinesDetectEncoding(path);
			lock (files)
			{
				files[key] = value;
			}
		};

		Debug.WriteLine($"Load: {path} : End in {(DateTime.Now - startTime).TotalMilliseconds}ms");
	}

	public static async Task Load(IEnumerable<string> paths)
	{
		foreach (var path in paths)
		{
			await Load(path);
		}
	}

	public static void Clear()
	{
		files.Clear();
	}
}
