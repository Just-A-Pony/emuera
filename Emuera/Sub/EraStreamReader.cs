﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using trerror = EvilMask.Emuera.Lang.Error;
using Emuera;
using System.Text.RegularExpressions;
using System.Linq;

namespace MinorShift.Emuera.Sub;

internal sealed partial class EraStreamReader : IDisposable
{
	public EraStreamReader(bool useRename)
	{
		this.useRename = useRename;
	}

	string filepath;
	string filename;
	readonly bool useRename = false;
	int curNo = 0;
	int nextNo = 0;
	string[] _fileLine;
	public bool Open(string path)
	{
		return Open(path, Path.GetFileName(path));
	}

	public bool Open(string path, string name)
	{
		//そんなお行儀の悪いことはしていない
		//if (disposed)
		//    throw new ExeEE("破棄したオブジェクトを再利用しようとした");
		//if ((reader != null) || (stream != null) || (filepath != null))
		//    throw new ExeEE("使用中のオブジェクトを別用途に再利用しようとした");
		filepath = path;
		filename = name;
		curNo = 0;
		nextNo = 0;
		try
		{
			_fileLine = File.ReadAllLines(filepath, Config.Encode);
		}
		catch
		{
			this.Dispose();
			return false;
		}
		return true;
	}
	public bool OpenOnCache(string path)
	{

		return OpenOnCache(path, Path.GetFileName(path));
	}

	public bool OpenOnCache(string path, string name)
	{
		filepath = path;
		filename = name;
		curNo = 0;
		nextNo = 0;
		_fileLine = Preload.GetFileLines(path.ToUpperInvariant());
		return true;
	}


	public string ReadLine()
	{
		string ret = null;
		curNo = nextNo;
		if (_fileLine.Length > curNo)
		{
			ret = _fileLine[curNo];
			nextNo++;
		}
		return ret;
	}

	[GeneratedRegex(@"\[\[.*?\]\]")]
	private static partial Regex regexRenameIdentifer();

	/// <summary>
	/// 次の有効な行を読む。LexicalAnalyzer経由でConfigを参照するのでConfig完成までつかわないこと。
	/// </summary>
	public StringStream ReadEnabledLine(bool disabled = false)
	{
		string line;
		StringStream st;
		while (true)
		{
			line = ReadLine(); 
			if (line == null)
				return null;
			if (line.Length == 0)
				continue;

			//Ordinal消して大丈夫なのかわからないのでコメントアウト
			//if (useRename && (line.IndexOf("[[", StringComparison.Ordinal) >= 0) && (line.IndexOf("]]", StringComparison.Ordinal) >= 0))
			if (useRename && regexRenameIdentifer().IsMatch(line))
			{
				var match = regexRenameIdentifer().Match(line);
				while (match.Success)
				{
					//この段階でマッチしないパターンもある
					if (ParserMediator.RenameDic.TryGetValue(match.Value, out var targetStr))
					{
						line = line.Replace(match.Value, targetStr);
					}

					match = match.NextMatch();
				}
			}
			st = new StringStream(line);
			LexicalAnalyzer.SkipWhiteSpace(st);
			if (st.EOS)
				continue;
			//[SKIPSTART]～[SKIPEND]中にここが誤爆するので無効化
			if (!disabled)
			{
				if (st.Current == '}')
					throw new CodeEE(trerror.UnexpectedContinuationEnd.Text, new ScriptPosition(filename, curNo));
				if (st.Current == '{')
				{
					if (line.Trim() != "{")
						throw new CodeEE(trerror.CharacterAfterContinuation.Text, new ScriptPosition(filename, curNo));
					break;
				}
			}
			return st;
		}
		//curNoはこの後加算しない(始端記号の行を行番号とする)
		StringBuilder b = new();
		while (true)
		{
			line = ReadLine();
			if (line == null)
			{
				throw new CodeEE(trerror.NotCloseLineContinuation.Text, new ScriptPosition(filename, curNo));
			}

			if (useRename && (line.IndexOf("[[", StringComparison.Ordinal) >= 0) && (line.IndexOf("]]", StringComparison.Ordinal) >= 0))
			{
				foreach (KeyValuePair<string, string> pair in ParserMediator.RenameDic)
					line = line.Replace(pair.Key, pair.Value);
			}
			string test = line.TrimStart();
			if (test.Length > 0)
			{
				if (test[0] == '}')
				{
					if (test.Trim() != "}")
						throw new CodeEE(trerror.CharacterAfterContinuationEnd.Text, new ScriptPosition(filename, curNo));
					break;
				}
				//行連結文字なら1字でないとおかしい、というか、こうしないとFORMの数値変数処理が誤爆する。
				//{
				//A}
				//みたいなどうしようもないコードは知ったこっちゃない
				if (test[0] == '{' && test.Length == 1)
					throw new CodeEE(trerror.UnexpectedContinuation.Text, new ScriptPosition(filename, curNo));
			}
			b.Append(line);
			b.Append(" ");
		}
		st = new StringStream(b.ToString());
		LexicalAnalyzer.SkipWhiteSpace(st);
		return st;
	}

	/// <summary>
	/// 直前に読んだ行の行番号
	/// </summary>
	public int LineNo
	{ get { return curNo; } }
	public string Filename
	{
		get
		{
			return filename;
		}
	}
	//public string Filepath
	//{
	//    get
	//    {
	//        return filepath;
	//    }
	//}

	public void Close() { this.Dispose(); }
	bool disposed = false;
	#region IDisposable メンバ

	public void Dispose()
	{
		if (disposed)
			return;
		filepath = null;
		filename = null;
		disposed = true;
	}

	#endregion
}
