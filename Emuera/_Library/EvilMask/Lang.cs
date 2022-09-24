
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
                    [Managed] public static TranslatableString KeyMacro { get; } = new TranslatableString("マクロ");

                    [Translate("マクログループ"), Managed]
                    public sealed class KeyMacroGroup
                    {
                        public static string Text { get { return trClass[typeof(KeyMacroGroup)].Text; } }
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

            [Translate("ClipBoardDialog"), Managed]
            public sealed class ClipBoardDialog
            {
                public static string Text { get { return trClass[typeof(ClipBoardDialog)].Text; } }
            }

            [Translate("ConfigDialog"), Managed]
            public sealed class ConfigDialog
            {
                public static string Text { get { return trClass[typeof(ConfigDialog)].Text; } }

                [Translate("環境"), Managed]
                public sealed class Environment
                {
                    public static string Text { get { return trClass[typeof(Environment)].Text; } }
                    [Managed] public static TranslatableString UseMouse { get; } = new TranslatableString("マウスを使用する");
                    [Managed] public static TranslatableString UseMenu { get; } = new TranslatableString("メニューを使用する");
                    [Managed] public static TranslatableString UseDebugCommand { get; } = new TranslatableString("デバッグコマンドを使用する");
                    [Managed] public static TranslatableString AllowMultipleInstances { get; } = new TranslatableString("多重起動を許可する");
                    [Managed] public static TranslatableString UseKeyMacro { get; } = new TranslatableString("キーボードマクロを使用する");
                    [Managed] public static TranslatableString AutoSave { get; } = new TranslatableString("オートセーブを行なう");
                    [Managed] public static TranslatableString UseSaveFolder { get; } = new TranslatableString("セーブデータをsavフォルダ内に作成する");
                    [Managed] public static TranslatableString MaxLog { get; } = new TranslatableString("履歴ログの行数");
                    [Managed] public static TranslatableString InfiniteLoopAlertTime { get; } = new TranslatableString("無限ループ警告までのミリ秒");
                    [Managed] public static TranslatableString SaveDataPerPage { get; } = new TranslatableString("使用するセーブデータ数");
                    [Managed] public static TranslatableString TextEditor { get; } = new TranslatableString("関連づけるテキストエディタ");
                    [Managed] public static TranslatableString Browse { get; } = new TranslatableString("選択");

                    [Translate("コマンドライン引数"), Managed]
                    public sealed class TextEditorCommandline
                    {
                        public static string Text { get { return trClass[typeof(TextEditorCommandline)].Text; } }
                        [Managed] public static TranslatableString UserSetting { get; } = new TranslatableString("選択");
                    }
                }


                [Translate("表示"), Managed]
                public sealed class Display
                {
                    public static string Text { get { return trClass[typeof(Display)].Text; } }
                    [Managed] public static TranslatableString TextDrawingMode { get; } = new TranslatableString("描画インターフェース");
                    [Managed] public static TranslatableString FPS { get; } = new TranslatableString("フレーム毎秒");
                    [Managed] public static TranslatableString PrintCPerLine { get; } = new TranslatableString("PRINTCを並べる数");
                    [Managed] public static TranslatableString PrintCLength { get; } = new TranslatableString("PRINTCの文字数");
                    [Managed] public static TranslatableString ButtonWrap { get; } = new TranslatableString("ボタンの途中で行を折りかえさない");
                }

                [Translate("ウィンドウ"), Managed]
                public sealed class Window
                {
                    public static string Text { get { return trClass[typeof(Window)].Text; } }
                    [Managed] public static TranslatableString WindowWidth { get; } = new TranslatableString("ウィンドウ幅");
                    [Managed] public static TranslatableString WindowHeight { get; } = new TranslatableString("ウィンドウ高さ");
                    [Managed] public static TranslatableString GetWindowSize { get; } = new TranslatableString("現在のウィンドウサイズを取得");
                    [Managed] public static TranslatableString ChangeableWindowHeight { get; } = new TranslatableString("ウィンドウの高さを可変にする");
                    [Managed] public static TranslatableString WindowMaximixed { get; } = new TranslatableString("起動時にウィンドウを最大化する");
                    [Managed] public static TranslatableString SetWindowPos { get; } = new TranslatableString("起動時のウィンドウの位置を固定する");
                    [Managed] public static TranslatableString WindowX { get; } = new TranslatableString("ウィンドウ位置X");
                    [Managed] public static TranslatableString WindowY { get; } = new TranslatableString("ウィンドウ位置Y");
                    [Managed] public static TranslatableString GetWindowPos { get; } = new TranslatableString("現在のウィンドウ位置を取得");
                    [Managed] public static TranslatableString LinesPerScroll { get; } = new TranslatableString("スクロールの行数");
                }

                [Translate("フォント"), Managed]
                public sealed class Font
                {
                    public static string Text { get { return trClass[typeof(Font)].Text; } }
                    [Managed] public static TranslatableString BackgroundColor { get; } = new TranslatableString("背景色");
                    [Managed] public static TranslatableString TextColor { get; } = new TranslatableString("文字色");
                    [Managed] public static TranslatableString HighlightColor { get; } = new TranslatableString("選択中文字色");
                    [Managed] public static TranslatableString LogHistoryColor { get; } = new TranslatableString("履歴文字色");
                    [Managed] public static TranslatableString FontName { get; } = new TranslatableString("フォント名");
                    [Managed] public static TranslatableString GetFontNames { get; } = new TranslatableString("フォント名一覧を取得");
                    [Managed] public static TranslatableString FontSize { get; } = new TranslatableString("フォントサイズ");
                    [Managed] public static TranslatableString LineHeight { get; } = new TranslatableString("一行の高さ");
                }

                [Translate("システム"), Managed]
                public sealed class System
                {
                    public static string Text { get { return trClass[typeof(System)].Text; } }
                    [Managed] public static TranslatableString Warning { get; } = new TranslatableString("※システムの項目を変化させた場合、\nERBスクリプトが正常に動作しないことがあります");
                    [Managed] public static TranslatableString IgnoreCase { get; } = new TranslatableString("大文字小文字の違いを無視する");
                    [Managed] public static TranslatableString UseRename { get; } = new TranslatableString("_Rename.csvを利用する");
                    [Managed] public static TranslatableString UseReplace { get; } = new TranslatableString("_Replace.csvを利用する");
                    [Managed] public static TranslatableString SearchSubfolder { get; } = new TranslatableString("サブディレクトリを検索する");
                    [Managed] public static TranslatableString SortFileNames { get; } = new TranslatableString("読み込み順をファイル名順にソートする");
                    [Managed] public static TranslatableString SystemFuncOverride { get; } = new TranslatableString("システム関数の上書きを許可する");
                    [Managed] public static TranslatableString SystemFuncOverrideWarn { get; } = new TranslatableString("システム関数が上書きされたとき警告を表示する");
                    [Managed] public static TranslatableString DuplicateFuncWarn { get; } = new TranslatableString("同名の非イベント関数が複数定義されたとき警告する");
                    [Managed] public static TranslatableString WSIncludesFullWidth { get; } = new TranslatableString("全角スペースをホワイトスペースに含める");
                    [Managed] public static TranslatableString ANSI { get; } = new TranslatableString("内部で使用する東アジア言語");
                }

                [Translate("システム2"), Managed]
                public sealed class System2
                {
                    public static string Text { get { return trClass[typeof(System2)].Text; } }
                    [Managed] public static TranslatableString IgnoreTripleSymbol { get; } = new TranslatableString("FORM中の三連記号を展開しない");
                    [Managed] public static TranslatableString SaveInBinary { get; } = new TranslatableString("セーブデータをバイナリ形式で保存する");
                    [Managed] public static TranslatableString SaveInUTF8 { get; } = new TranslatableString("セーブデータをUTF-8で保存する(非バイナリ時のみ)");
                    [Managed] public static TranslatableString CompressSave { get; } = new TranslatableString("セーブデータを圧縮して保存する(バイナリ時のみ)");
                    [Managed] public static TranslatableString NoAutoCompleteCVar { get; } = new TranslatableString("キャラクタ変数の引数を補完しない");
                    [Managed] public static TranslatableString DisallowUpdateCheck { get; } = new TranslatableString("UPDATECHECKを許可しない");
                    [Managed] public static TranslatableString UseERD { get; } = new TranslatableString("ERD機能を利用する");
                    [Managed] public static TranslatableString SaveLoadExt { get; } = new TranslatableString("LOADTEXTとSAVETEXTで使える拡張子");
                }

                [Translate("互換性"), Managed]
                public sealed class Compatibility
                {
                    public static string Text { get { return trClass[typeof(Compatibility)].Text; } }
                    [Managed] public static TranslatableString Warning { get; } = new TranslatableString("※eramakerとEmueraで動作が違う、\nEmueraの過去のバージョンで動作したものが動作しない、\nなどの問題を解決するためのオプションです\n標準で問題ない場合は変更しないでください");
                    [Managed] public static TranslatableString ExecuteErrorLine { get; } = new TranslatableString("解釈不可能な行があっても実行する");
                    [Managed] public static TranslatableString NameForCallname { get; } = new TranslatableString("CALLNAMEが空文字列の時にNAMEを代入する");
                    [Managed] public static TranslatableString EramakerRAND { get; } = new TranslatableString("擬似変数RANDの仕様をeramakerに合わせる");
                    [Managed] public static TranslatableString EramakerTIMES { get; } = new TranslatableString("TIMESの計算をeramakerにあわせる");
                    [Managed] public static TranslatableString NoIgnoreCase { get; } = new TranslatableString("関数・属性については大文字小文字を無視しない");
                    [Managed] public static TranslatableString CallEvent { get; } = new TranslatableString("イベント関数のCALLを許可する");
                    [Managed] public static TranslatableString UseSPCharacters { get; } = new TranslatableString("SPキャラを使用する");
                    [Managed] public static TranslatableString ButtonWarp { get; } = new TranslatableString("ver1739以前の非ボタン折り返しを再現する");
                    [Managed] public static TranslatableString OmitArgs { get; } = new TranslatableString("ユーザー関数の全ての引数の省略を許可する");
                    [Managed] public static TranslatableString AutoTOSTR { get; } = new TranslatableString("ユーザー関数の引数に自動的にTOSTRを補完する");
                    [Managed] public static TranslatableString EramakerStandard { get; } = new TranslatableString("eramakerの仕様にする");
                    [Managed] public static TranslatableString EmueraStandard { get; } = new TranslatableString("Emuera標準仕様にする");
                }

                [Translate("解析"), Managed]
                public sealed class Debug
                {
                    public static string Text { get { return trClass[typeof(Debug)].Text; } }
                    [Managed] public static TranslatableString CompatibilityWarn { get; } = new TranslatableString("eramaker互換性に関する警告を表示する");
                    [Managed] public static TranslatableString LoadingReport { get; } = new TranslatableString("ロード時にレポートを表示する");

                    [Translate("ロード時に引数を解析する"), Managed]
                    public sealed class ReduceArgs
                    {
                        public static string Text { get { return trClass[typeof(ReduceArgs)].Text; } }
                        [Managed] public static TranslatableString Never { get; } = new TranslatableString("常に行わない");
                        [Managed] public static TranslatableString OnUpdate { get; } = new TranslatableString("更新されていれば行う");
                        [Managed] public static TranslatableString Always { get; } = new TranslatableString("常に行う");
                    }

                    [Translate("表示する最低警告レベル"), Managed]
                    public sealed class WarnLevel
                    {
                        public static string Text { get { return trClass[typeof(WarnLevel)].Text; } }
                        [Managed] public static TranslatableString Level0 { get; } = new TranslatableString("0:標準でない文法");
                        [Managed] public static TranslatableString Level1 { get; } = new TranslatableString("1:無視可能なエラー");
                        [Managed] public static TranslatableString Level2 { get; } = new TranslatableString("2:動作しないエラー");
                        [Managed] public static TranslatableString Level3 { get; } = new TranslatableString("3:致命的エラー");
                    }

                    [Managed] public static TranslatableString IgnoreUnusedFuncs { get; } = new TranslatableString("呼び出されなかった関数を無視する");

                    [Managed]
                    public sealed class WarnSetting
                    {
                        [Managed] public static TranslatableString Ignore { get; } = new TranslatableString("無視");
                        [Managed] public static TranslatableString TotalNumber { get; } = new TranslatableString("総数のみ表示する");
                        [Managed] public static TranslatableString OncePerFile { get; } = new TranslatableString("ファイル毎に一度だけ表示する");
                        [Managed] public static TranslatableString Always { get; } = new TranslatableString("表示する");
                    }

                    [Managed] public static TranslatableString FuncNotFoundWarn { get; } = new TranslatableString("関数が見つからない警告の扱い");
                    [Managed] public static TranslatableString UnusedFuncWarn { get; } = new TranslatableString("関数が呼び出されなかった警告の扱い");
                    [Managed] public static TranslatableString PlayerStandard { get; } = new TranslatableString("ユーザー向けの設定にする");
                    [Managed] public static TranslatableString DeveloperStandard { get; } = new TranslatableString("開発者向けの設定にする");
                }

                [Managed] public static TranslatableString ChangeWontTakeEffectUntilRestart { get; } = new TranslatableString("※変更は再起動するまで反映されません");
                [Managed] public static TranslatableString Save { get; } = new TranslatableString("保存");
                [Managed] public static TranslatableString SaveAndRestart { get; } = new TranslatableString("保存して再起動");
                [Managed] public static TranslatableString Cancel { get; } = new TranslatableString("キャンセル");
            }


            [Translate("ConfigDialog"), Managed]
            public sealed class DebugConfigDialog
            {
                public static string Text { get { return trClass[typeof(DebugConfigDialog)].Text; } }
                [Managed] public static TranslatableString Name { get; } = new TranslatableString("デバッグ");
                [Managed] public static TranslatableString Warning { get; } = new TranslatableString("※デバッグ関連のオプションはコマンドライン引数に-Debug\nを指定して起動した時のみ有効です");
                [Managed] public static TranslatableString OpenDebugWindowOnStartup { get; } = new TranslatableString("起動時にデバッグウインドウを表示する");
                [Managed] public static TranslatableString AlwaysOnTop { get; } = new TranslatableString("デバッグウインドウを最前面に表示する");
                [Managed] public static TranslatableString WindowWidth { get; } = new TranslatableString("デバッグウィンドウ幅");
                [Managed] public static TranslatableString WindowHeight { get; } = new TranslatableString("デバッグウィンドウ高さ");
                // [Managed] public static TranslatableString GetWindowSize { get; } = new TranslatableString("現在のウィンドウサイズを取得");
                // Lang.UI.ConfigDialog.Window.GetWindowSize
                [Managed] public static TranslatableString SetWindowPos { get; } = new TranslatableString("デバッグウィンドウ位置を指定する");
                [Managed] public static TranslatableString WindowX { get; } = new TranslatableString("デバッグウィンドウ位置X");
                [Managed] public static TranslatableString WindowY { get; } = new TranslatableString("デバッグウィンドウ位置Y");
                // [Managed] public static TranslatableString Warning { get; } = new TranslatableString("現在のウィンドウ位置を取得");
                // Lang.UI.ConfigDialog.Window.GetWindowPos
            }

            [Translate("CodeEE"), Managed]
            public sealed class CodeEE
            {
                public static string Text { get { return trClass[typeof(CodeEE)].Text; } }
                [Managed] public static TranslatableString NotExistColorSpecifier { get; } = new TranslatableString("値をColor指定子として認識できません");
                [Managed] public static TranslatableString ContainsNonNumericCharacters { get; } = new TranslatableString("数字でない文字が含まれています");
                [Managed] public static TranslatableString InvalidSpecification { get; } = new TranslatableString("不正な指定です");
                [Managed] public static TranslatableString DoesNotMatchCdflagElements { get; } = new TranslatableString("CDFLAGの要素数とCDFLAGNAME1及びCDFLAGNAME2の要素数が一致していません");
                [Managed] public static TranslatableString TooManyCdflagElements { get; } = new TranslatableString("CDFLAGの要素数が多すぎます（CDFLAGNAME1とCDFLAGNAME2の要素数の積が100万を超えています）");
                [Managed] public static TranslatableString DuplicateErdKey { get; } = new TranslatableString("変数\"{0}\"の置き換え名前\"{1}\"の定義が重複しています。（ファイル1 - {2}）（ファイル2 - {3}）");
                [Managed] public static TranslatableString DuplicateVariableDefine { get; } = new TranslatableString("変数{0}の定義が重複しています。");
                [Managed] public static TranslatableString NotDefinedErdKey { get; } = new TranslatableString("変数\"{0}\"には\"{1}\"の定義がありません");
                [Managed] public static TranslatableString KeywordsCannotBeEmpty { get; } = new TranslatableString("キーワードを空には出来ません");
                [Managed] public static TranslatableString CannotSpecifiedByString { get; } = new TranslatableString("配列変数\"{0}\"の要素を文字列で指定することはできません");
                [Managed] public static TranslatableString NotDefinedKey { get; } = new TranslatableString("\"{0}\"の中に\"{1}\"の定義がありません");
                [Managed] public static TranslatableString CannotIndexSpecifiedByString{ get; } = new TranslatableString("配列変数\"{0}\"の{1}番目の要素を文字列で指定することはできません");
                [Managed] public static TranslatableString UseCdflagname { get; } = new TranslatableString("CDFLAGの要素の取得にはCDFLAGNAME1又はCDFLAGNAME2を使用します");
                [Managed] public static TranslatableString NotExistKey { get; } = new TranslatableString("存在しないキーを参照しました");
                //[Managed] public static TranslatableString { get; } = new TranslatableString("");

            }


        }

        static public void LoadLanguageFile()
        {
            foreach (var pair in trItems) pair.Value.Clear();
            if (Directory.Exists(langDir))
            {
                foreach (var path in Directory.EnumerateFiles(langDir, "emuera.*.xml", SearchOption.TopDirectoryOnly))
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
        }

        static public void GenerateDefaultLangFile()
        {
            if (!Directory.Exists(langDir))
                Directory.CreateDirectory(langDir);
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
