
using MinorShift.Emuera;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace EvilMask.Emuera
{
    internal sealed class Lang
    {
        public sealed class TranslatableString
        {
            public TranslatableString(string text)
            {
                this.text = text;
                this.tr = null;
            }

            public void Clear()
            {
                tr = null;
            }

            public void Set(string tr)
            {
                this.tr = tr;
            }

            private string text;
            private string tr;

            public string Text { get { return tr == null ? text : tr; } }
        }

        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
        sealed class Managed : Attribute { }

        [AttributeUsage(AttributeTargets.Class)]
        sealed class Translate : Attribute 
        {
            public Translate(string text)
            {
                String = text;
            }
            public string String { get; private set; }
        }

        [Managed]
        public sealed class UI
        {
            [Managed]
            public sealed class MainWindow
            {
                [Translate("ファイル(&F)"), Managed]
                public sealed class File
                {
                    public static string Text { get { return trClass[typeof(File)].Text; } }
                    [Managed] public static TranslatableString Restart { get; } = new TranslatableString("再起動(&R)");
                    [Managed] public static TranslatableString SaveLog { get; } = new TranslatableString("ログを保存する...(&S)");
                    [Managed] public static TranslatableString CopyLogToClipboard { get; } = new TranslatableString("ログをクリップボードにコピー(&C)");
                    [Managed] public static TranslatableString BackToTitle { get; } = new TranslatableString("タイトル画面へ戻る(&T)");
                    [Managed] public static TranslatableString ReloadAllScripts { get; } = new TranslatableString("全コードを読み直す(&C)");
                    [Managed] public static TranslatableString ReloadFolder { get; } = new TranslatableString("フォルダを読み直す(&F)");
                    [Managed] public static TranslatableString ReloadScriptFile { get; } = new TranslatableString("ファイルを読み直す(&A)");
                    [Managed] public static TranslatableString Exit { get; } = new TranslatableString("終了(&X)");
                }

                [Translate("デバッグ(&D)"), Managed]
                public sealed class Debug
                {
                    public static string Text { get { return trClass[typeof(Debug)].Text; } }
                    [Managed] public static TranslatableString OpenDebugWindow { get; } = new TranslatableString("デバッグウインドウを開く");
                    [Managed] public static TranslatableString UpdateDebugInfo { get; } = new TranslatableString("デバッグ情報の更新");
                }

                [Translate("ヘルプ(&H)"), Managed]
                public sealed class Help
                {
                    public static string Text { get { return trClass[typeof(Help)].Text; } }
                    [Managed] public static TranslatableString Config { get; } = new TranslatableString("設定(&C)");
                }

                [Managed]
                public sealed class ContextMenu
                {
                    [Managed] public static TranslatableString KeyboardMacro { get; } = new TranslatableString("マクロ");

                    [Translate("マクログループ"), Managed]
                    public sealed class KeyboardMacroGroup
                    {
                        public static string Text { get { return trClass[typeof(KeyboardMacroGroup)].Text; } }
                        [Managed] public static TranslatableString Group { get; } = new TranslatableString("グループ");
                    }

                    [Managed] public static TranslatableString Cut { get; } = new TranslatableString("切り取り");
                    [Managed] public static TranslatableString Copy { get; } = new TranslatableString("コピー");
                    [Managed] public static TranslatableString Paste { get; } = new TranslatableString("貼り付け");
                    [Managed] public static TranslatableString Delete { get; } = new TranslatableString("削除");
                    [Managed] public static TranslatableString Execute { get; } = new TranslatableString("実行");
                }

                [Managed]
                public sealed class MsgBox
                {
                    [Managed] public static TranslatableString InstaceExists { get; } = new TranslatableString("既に起動しています");
                    [Managed] public static TranslatableString MultiInstanceInfo { get; } = new TranslatableString("多重起動を許可する場合、emuera.configを書き換えて下さい");
                    [Managed] public static TranslatableString FolderNotFound { get; } = new TranslatableString("フォルダなし");
                    [Managed] public static TranslatableString NoCsvFolder { get; } = new TranslatableString("csvフォルダが見つかりません");
                    [Managed] public static TranslatableString NoErbFolder { get; } = new TranslatableString("erbフォルダが見つかりません");
                }
            }

        }

        static public void LoadLanguageFile()
        {
            foreach (var pair in trItems) pair.Value.Clear();
            foreach(var path in Directory.EnumerateFiles(langDir, "emuera.*.xml", SearchOption.AllDirectories))
            {
                XmlDocument xml = new XmlDocument();
                try
                {
                    xml.Load(path);
                }
                catch
                {
                    continue;
                }
                var node = xml.SelectSingleNode("/lang/name");
                if (node != null)
                {
                    langList.Add(node.InnerText, path);
                    if (Config.EmueraLang == node.InnerText)
                    {
                        var nodes = xml.SelectNodes("/lang/tr");
                        for (int i = 0; i < nodes.Count; i++)
                        {
                            var attr = nodes[i].Attributes["id"];
                            if (attr != null && trItems.ContainsKey(attr.Value))
                                trItems[attr.Value].Set(nodes[i].InnerText);
                        }
                    }
                }
            }
        }

        static public void GenerateDefaultLangFile()
        {
            FileStream fs = new FileStream(langDir + "emuera.default.xml", FileMode.Create);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            XmlWriter writer = XmlWriter.Create(fs, settings);
            XmlDocument xml = new XmlDocument();
            var root = xml.CreateElement("lang");
            xml.AppendChild(root);
            var name = xml.CreateElement("name");
            name.InnerText = "日本語";
            root.AppendChild(name);
            foreach (var item in trItems)
            {
                var tr = xml.CreateElement("tr");
                var id = xml.CreateAttribute("id");
                id.Value = item.Key;
                tr.Attributes.Append(id);
                tr.AppendChild(xml.CreateCDataSection(item.Value.Text));
                root.AppendChild(tr);
            }
            xml.WriteTo(writer);
            writer.Flush();
        }

        static readonly string langDir = "lang/";
        static readonly Dictionary<string, string> langList = new Dictionary<string, string>();
        static readonly Dictionary<string, TranslatableString> trItems = new Dictionary<string, TranslatableString>();
        static readonly Dictionary<Type, TranslatableString> trClass = new Dictionary<Type, TranslatableString>();
    

        static Lang()
        {
            queryManagedClass(typeof(Lang), string.Empty, trItems);
        }

        static void queryManagedClass(Type t, string addr, Dictionary<string, TranslatableString> trItems)
        {
            if (addr.Length > 0) addr += '.';
            foreach (var nt in t.GetNestedTypes())
            {
                string tr = null;
                bool managed = false;
                foreach (var attr in nt.GetCustomAttributes(false))
                {
                    if (attr is Managed) managed = true;
                    else if (attr is Translate trAttr) tr = trAttr.String;
                }
                if (managed)
                {
                    if (tr != null)
                    {
                        var item = new TranslatableString(tr);
                        trItems.Add(addr + nt.Name, item);
                        trClass.Add(nt, item);
                    }
                    foreach (var prop in nt.GetProperties())
                    {
                        if (prop.PropertyType == typeof(TranslatableString))
                        {
                            foreach (var pattr in prop.GetCustomAttributes(false))
                            {
                                if (pattr is Managed)
                                {
                                    trItems.Add(addr + nt.Name + '.' + prop.Name, prop.GetValue(null, null) as TranslatableString);
                                }
                            }
                        }
                    }
                    queryManagedClass(nt, addr + nt.Name, trItems);
                }
            }
        }
    }
}
