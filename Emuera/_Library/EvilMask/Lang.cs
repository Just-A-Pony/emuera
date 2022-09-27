
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
        }

        [Managed]
        public sealed class Error
        {
            public static string Text { get { return trClass[typeof(Error)].Text; } }
            [Managed] public static TranslatableString NotExistColorSpecifier { get; } = new TranslatableString("値をColor指定子として認識できません");
            [Managed] public static TranslatableString ContainsNonNumericCharacters { get; } = new TranslatableString("数字でない文字が含まれています");
            [Managed] public static TranslatableString InvalidSpecification { get; } = new TranslatableString("不正な指定です");
            [Managed] public static TranslatableString DoesNotMatchCdflagElements { get; } = new TranslatableString("CDFLAGの要素数とCDFLAGNAME1及びCDFLAGNAME2の要素数が一致していません");
            [Managed] public static TranslatableString TooManyCdflagElements { get; } = new TranslatableString("CDFLAGの要素数が多すぎます（CDFLAGNAME1とCDFLAGNAME2の要素数の積が100万を超えています）");
            [Managed] public static TranslatableString DuplicateErdKey { get; } = new TranslatableString("変数\"{0}\"の置き換え名前\"{1}\"の定義が重複しています。（ファイル1 - {2}）（ファイル2 - {3}）");
            [Managed] public static TranslatableString DuplicateVariableDefine { get; } = new TranslatableString("変数{0}の定義が重複しています。");
            [Managed] public static TranslatableString NotDefinedErdKey { get; } = new TranslatableString("変数\"{0}\"には\"{1}\"の定義がありません");
            [Managed] public static TranslatableString KeywordsCannotBeEmpty { get; } = new TranslatableString("キーワードを空には出来ません");
            [Managed] public static TranslatableString InvalidProhibitedVar { get; } = new TranslatableString("CanForbidでない変数\"{0}\"にIsForbidがついている");
            [Managed] public static TranslatableString CanNotSpecifiedByString { get; } = new TranslatableString("配列変数\"{0}\"の要素を文字列で指定することはできません");
            [Managed] public static TranslatableString NotDefinedKey { get; } = new TranslatableString("\"{0}\"の中に\"{1}\"の定義がありません");
            [Managed] public static TranslatableString CannotIndexSpecifiedByString { get; } = new TranslatableString("配列変数\"{0}\"の{1}番目の要素を文字列で指定することはできません");
            [Managed] public static TranslatableString UseCdflagname { get; } = new TranslatableString("CDFLAGの要素の取得にはCDFLAGNAME1又はCDFLAGNAME2を使用します");
            [Managed] public static TranslatableString NotExistKey { get; } = new TranslatableString("存在しないキーを参照しました");
            [Managed] public static TranslatableString UsedAtForPrivVar { get; } = new TranslatableString("プライベート変数\"{0}\"に対して@が使われました");
            [Managed] public static TranslatableString UsedProhibitedVar { get; } = new TranslatableString("呼び出された変数\"{0}\"は設定により使用が禁止されています");
            [Managed] public static TranslatableString CannotGetKeyNotExistRunningFunction { get; } = new TranslatableString("実行中の関数が存在しないため\"{0}\"を取得又は変更できませんでした");
            [Managed] public static TranslatableString UsedAtForGlobalVar { get; } = new TranslatableString("ローカル変数でない変数{0}に対して@が使われました");
            [Managed] public static TranslatableString InvalidAt { get; } = new TranslatableString("@の使い方が不正です");
            [Managed] public static TranslatableString CallfNonMethodFunc { get; } = new TranslatableString("#FUNCTIONが指定されていない関数\"@{0}\"をCALLF系命令で呼び出そうとしました");
            [Managed] public static TranslatableString UsedNonMethodFunc { get; } = new TranslatableString("#FUNCTIONが定義されていない関数({0}:{1}行目)を式中で呼び出そうとしました");
            [Managed] public static TranslatableString DeclaringDisable { get; } = new TranslatableString("\"{0}\"は#DISABLEが宣言されています");
            [Managed] public static TranslatableString VarNotDefinedThisFunc { get; } = new TranslatableString("変数\"{0}\"はこの関数中では定義されていません");
            [Managed] public static TranslatableString IllegalUseReservedWord { get; } = new TranslatableString("Emueraの予約語\"{0}\"が不正な使われ方をしています");
            [Managed] public static TranslatableString UseVarLikeFunc { get; } = new TranslatableString("変数名\"{0}\"が関数のように使われています");
            [Managed] public static TranslatableString UseFuncLikeVar { get; } = new TranslatableString("関数名\"{0}\"が変数のように使われています");
            [Managed] public static TranslatableString UnexpectedMacro { get; } = new TranslatableString("予期しないマクロ名\"{0}\"です");
            [Managed] public static TranslatableString UseInstructionLikeFunc { get; } = new TranslatableString("命令名\"{0}\"が関数のように使われています");
            [Managed] public static TranslatableString UseInstructionLikeVar { get; } = new TranslatableString("命令名\"{0}\"が変数のように使われています");
            [Managed] public static TranslatableString CannotInterpreted { get; } = new TranslatableString("\"{0}\"は解釈できない識別子です");
            [Managed] public static TranslatableString AbnormalFirstOperand { get; } = new TranslatableString("三項演算子\\@の第一オペランドが異常です");
            [Managed] public static TranslatableString EmptyBrace { get; } = new TranslatableString("{}の中に式が存在しません");
            [Managed] public static TranslatableString EmptyPer { get; } = new TranslatableString("%%の中に式が存在しません");
            [Managed] public static TranslatableString NotSpecifiedLR { get; } = new TranslatableString("','の後にRIGHT又はLEFTがありません");
            [Managed] public static TranslatableString OtherThanLR { get; } = new TranslatableString("','の後にRIGHT又はLEFT以外の単語があります");
            [Managed] public static TranslatableString ExtraCharacterLR { get; } = new TranslatableString("RIGHT又はLEFTの後に余分な文字があります");
            [Managed] public static TranslatableString IsNotNumericBrace { get; } = new TranslatableString("{}の中の式が数式ではありません");
            [Managed] public static TranslatableString IsNotStringPer { get; } = new TranslatableString("%%の中の式が文字列式ではありません");
            [Managed] public static TranslatableString OoRForcekanaArg { get; } = new TranslatableString("命令FORCEKANAの引数が指定可能な範囲(0～3)を超えています");
            [Managed] public static TranslatableString MaxBarNotPositive { get; } = new TranslatableString("BARの最大値が正の値ではありません");
            [Managed] public static TranslatableString BarNotPositive { get; } = new TranslatableString("BARの長さが正の値ではありません");
            [Managed] public static TranslatableString TooLongBar { get; } = new TranslatableString("BARが長すぎます");
            [Managed] public static TranslatableString NotCloseSBrackets { get; } = new TranslatableString("'['に対応する']'が見つかりません");
            [Managed] public static TranslatableString NotCloseBrackets { get; } = new TranslatableString("'('に対応する')'が見つかりません");
            [Managed] public static TranslatableString UnexpectedBrackets { get; } = new TranslatableString("構文解析中に予期しない')'を発見しました");
            [Managed] public static TranslatableString UnexpectedSBrackets { get; } = new TranslatableString("構文解析中に予期しない']'を発見しました");
            [Managed] public static TranslatableString CannotOmitFuncArg { get; } = new TranslatableString("関数定義の引数は省略できません");
            [Managed] public static TranslatableString NoExpressionAfterEqual { get; } = new TranslatableString("'='の後に式がありません");
            [Managed] public static TranslatableString DoesNotMatchEqual { get; } = new TranslatableString("'='の前後で型が一致しません");
            [Managed] public static TranslatableString CanNotInterpretedExpression { get; } = new TranslatableString("構文を式として解釈できません");
            [Managed] public static TranslatableString ExpressionResultIsNotNumeric { get; } = new TranslatableString("式の結果が数値ではありません");
            [Managed] public static TranslatableString EmptyStream { get; } = new TranslatableString("空のストリームを渡された");
            [Managed] public static TranslatableString SBracketsFuncNotImprement { get; } = new TranslatableString("[]を使った機能はまだ実装されていません");
            [Managed] public static TranslatableString ThrowFailed { get; } = new TranslatableString("エラー投げ損ねた");
            [Managed] public static TranslatableString NoOpAfterIs { get; } = new TranslatableString("ISキーワードの後に演算子がありません");
            [Managed] public static TranslatableString NotBinaryOpAfterThis { get; } = new TranslatableString("ISキーワードの後の演算子が2項演算子ではありません");
            [Managed] public static TranslatableString NothingAfterIs { get; } = new TranslatableString("ISキーワードの後に式がありません");
            [Managed] public static TranslatableString CanNotOmitCaseArg { get; } = new TranslatableString("CASEの引数は省略できません");
            [Managed] public static TranslatableString NoExpressionAfterTo { get; } = new TranslatableString("TOキーワードの後に式がありません");
            [Managed] public static TranslatableString DuplicateTo { get; } = new TranslatableString("TOキーワードが2度使われています");
            [Managed] public static TranslatableString DoesNotMatchTo { get; } = new TranslatableString("TOキーワードの前後の型が一致していません");
            [Managed] public static TranslatableString InvalidTo { get; } = new TranslatableString("TOキーワードはここでは使用できません");
            [Managed] public static TranslatableString InvalidIs { get; } = new TranslatableString("ISキーワードはここでは使用できません");
            [Managed] public static TranslatableString UnexpectedOpInVarArg { get; } = new TranslatableString("変数の引数の読み取り中に予期しない演算子を発見しました");
            [Managed] public static TranslatableString EqualInExpression { get; } = new TranslatableString("式中で代入演算子'='が使われています(等価比較には'=='を使用してください)");
            [Managed] public static TranslatableString ComparisonOpContinuous { get; } = new TranslatableString("（構文上の注意）比較演算子が連続しています。");
            [Managed] public static TranslatableString MissingQuestion { get; } = new TranslatableString("対応する'?'のない'#'です");
            [Managed] public static TranslatableString NoContainExpressionInBrackets { get; } = new TranslatableString("かっこ\"(\"～\")\"の中に式が含まれていません");
            [Managed] public static TranslatableString UnexpectedSymbol { get; } = new TranslatableString("構文解釈中に予期しない記号\"{0}\"を発見しました");
            [Managed] public static TranslatableString TernaryBinaryError { get; } = new TranslatableString("'?'と'#'の数が正しく対応していません");
            [Managed] public static TranslatableString FailedSolveMacro { get; } = new TranslatableString("マクロ解決失敗");
            [Managed] public static TranslatableString UnrecognizedSyntax { get; } = new TranslatableString("式が異常です");
            [Managed] public static TranslatableString MultipleUnaryOp { get; } = new TranslatableString("後置の単項演算子が複数存在しています");
            [Managed] public static TranslatableString DuplicateIncrementDecrement { get; } = new TranslatableString("インクリメント・デクリメントを前置・後置両方同時に使うことはできません");
            [Managed] public static TranslatableString InsufficientExpression { get; } = new TranslatableString("式の数が不足しています");
            [Managed] public static TranslatableString EmptyFramelist { get; } = new TranslatableString("totaltime > 0なのにFrameListが空");
            [Managed] public static TranslatableString OoRLasframe { get; } = new TranslatableString("SpriteAnime:最終フレームが範囲外");
            [Managed] public static TranslatableString SpriteTimeOut { get; } = new TranslatableString("SpriteAnime:時間外参照");
            [Managed] public static TranslatableString IncrementNonVar { get; } = new TranslatableString("変数以外をインクリメントすることはできません");
            [Managed] public static TranslatableString IncrementConst { get; } = new TranslatableString("変更できない変数をインクリメントすることはできません");
            [Managed] public static TranslatableString NumericType { get; } = new TranslatableString("数値型");
            [Managed] public static TranslatableString StringType { get; } = new TranslatableString("文字列型");
            [Managed] public static TranslatableString UnknownType { get; } = new TranslatableString("不定型");
            [Managed] public static TranslatableString CanNotAppliedUnaryOp { get; } = new TranslatableString("に単項演算子\"{0}\"は適用できません");
            [Managed] public static TranslatableString NumericTypeAnd { get; } = new TranslatableString("数値型と");
            [Managed] public static TranslatableString StringTypeAnd { get; } = new TranslatableString("文字列型と");
            [Managed] public static TranslatableString UnknownTypeAnd { get; } = new TranslatableString("不定型と");
            [Managed] public static TranslatableString ANumericType { get; } = new TranslatableString("数値型の");
            [Managed] public static TranslatableString AStringType { get; } = new TranslatableString("文字列型の");
            [Managed] public static TranslatableString AnUnknownType { get; } = new TranslatableString("不定型の");
            [Managed] public static TranslatableString CanNotAppliedBinaryOp { get; } = new TranslatableString("演算に二項演算子\"{0}\"は適用できません");
            [Managed] public static TranslatableString InvalidTernaryOp { get; } = new TranslatableString("三項演算子の使用法が不正です");
            [Managed] public static TranslatableString MultiplyNegativeToStr { get; } = new TranslatableString("文字列に負の値({0})を乗算しようとしました");
            [Managed] public static TranslatableString Multiply10kToStr { get; } = new TranslatableString("文字列に10000以上の値({0})を乗算しようとしました");
            [Managed] public static TranslatableString DivideByZero { get; } = new TranslatableString("0による除算が行なわれました");
            [Managed] public static TranslatableString XmlGetError { get; } = new TranslatableString("XML_GET関数:\"{0}\"の解析エラー:{1}");
            [Managed] public static TranslatableString XmlGetPathError { get; } = new TranslatableString("XML_GET関数:XPath\"{0}\"の解析エラー:{1}");
            [Managed] public static TranslatableString FirstArg { get; } = new TranslatableString("1番目の引数");
            [Managed] public static TranslatableString NotVarFunc { get; } = new TranslatableString("{0}関数:{1}が変数ではありません");
            [Managed] public static TranslatableString IsCharaVarFunc { get; } = new TranslatableString("{0}関数:{1}がキャラクタ変数です");
            [Managed] public static TranslatableString Not1DFuncArg { get; } = new TranslatableString("{0}関数:{1}番目の引数が一次元文字列配列ではありません");
            [Managed] public static TranslatableString NotDimVarFunc { get; } = new TranslatableString("{0}関数:{1}が配列変数ではありません");
            [Managed] public static TranslatableString AbnormalArray { get; } = new TranslatableString("異常な配列");
            [Managed] public static TranslatableString SetStrToInt { get; } = new TranslatableString("文字列型でない変数\"{0}\"に文字列型を代入しようとしました");
            [Managed] public static TranslatableString SetIntToStr { get; } = new TranslatableString("整数型でない変数\"{0}\"に整数値を代入しようとしました");
            [Managed] public static TranslatableString InvalidRegexArg { get; } = new TranslatableString("第{1}引数が正規表現として不正です：{0}");
            [Managed] public static TranslatableString XmlSetError { get; } = new TranslatableString("XML_SET関数:\"{0}\"の解析エラー:{1}");
            [Managed] public static TranslatableString XmlSetPathError { get; } = new TranslatableString("XML_SET関数:XPath\"{0}\"の解析エラー:{1}");
            [Managed] public static TranslatableString ReturnTypeDefferentOrNotImpelemnt { get; } = new TranslatableString("戻り値の型が違う or 未実装");
            [Managed] public static TranslatableString NotImplement { get; } = new TranslatableString("未実装？");
            [Managed] public static TranslatableString EmptyRefFunc { get; } = new TranslatableString("何も参照していない関数参照\"{0}\"を呼び出しました");
            [Managed] public static TranslatableString RefFuncHasNotArg { get; } = new TranslatableString("引数のない関数参照\"{0}\"を呼び出しました");
            [Managed] public static TranslatableString AbnormalData { get; } = new TranslatableString("データ異常");
            [Managed] public static TranslatableString OoRSortKey { get; } = new TranslatableString("ソートキーが配列外を参照しています");
            [Managed] public static TranslatableString AbnormalVarDeclaration { get; } = new TranslatableString("異常な変数宣言");
            [Managed] public static TranslatableString SetToConst { get; } = new TranslatableString("読み取り専用の変数\"{0}\"に代入しようとしました");
            [Managed] public static TranslatableString OoRCharaVar { get; } = new TranslatableString("キャラクタ配列変数\"{0}\"の第{1}引数({2})は配列の範囲外です");
            [Managed] public static TranslatableString OoRArrayShift { get; } = new TranslatableString("命令ARRAYSHIFTの第4引数({0})が配列\"{1}\"の範囲を超えています");
            [Managed] public static TranslatableString OoRArrayRemove { get; } = new TranslatableString("命令ARRAYREMOVEの第2引数({0})が配列\"{1}\"の範囲を超えています");
            [Managed] public static TranslatableString OoRArraySort { get; } = new TranslatableString("命令ARRAYSORTの第3引数({0})が配列\"{1}\"の範囲を超えています");
            [Managed] public static TranslatableString OoRCharaNum { get; } = new TranslatableString("存在しない登録キャラクタを参照しようとしました");
            [Managed] public static TranslatableString AddedUndefinedChara { get; } = new TranslatableString("定義していないキャラクタを作成しようとしました");
            [Managed] public static TranslatableString OoRDelChara { get; } = new TranslatableString("存在しない登録キャラクタ({0})を削除しようとしました");
            [Managed] public static TranslatableString DuplicateDelChara { get; } = new TranslatableString("同一の登録キャラクタ番号({0})が複数回指定されました");
            [Managed] public static TranslatableString NotExistFromCopyChara { get; } = new TranslatableString("コピー元のキャラクタが存在しません");
            [Managed] public static TranslatableString NotExistToCopyChara { get; } = new TranslatableString("コピー先のキャラクタが存在しません");
            [Managed] public static TranslatableString OoRSwapChara { get; } = new TranslatableString("存在しない登録キャラクタを入れ替えようとしました");
            [Managed] public static TranslatableString RefUndefinedChara { get; } = new TranslatableString("定義していないキャラクタを参照しようとしました");
            [Managed] public static TranslatableString OoRCstr { get; } = new TranslatableString("CSTRの参照可能範囲外を参照しました");
            [Managed] public static TranslatableString RefDoesNotExistData { get; } = new TranslatableString("存在しないデータを参照しようとしました");
            [Managed] public static TranslatableString RefOoR { get; } = new TranslatableString("参照可能範囲外を参照しました");
            [Managed] public static TranslatableString FailedCreateDataFolder { get; } = new TranslatableString("datフォルダーの作成に失敗しました");
            [Managed] public static TranslatableString NothingFileName { get; } = new TranslatableString("ファイル名が指定されていません");
            [Managed] public static TranslatableString InvalidFileName { get; } = new TranslatableString("ファイル名に不正な文字が含まれています");
            [Managed] public static TranslatableString DifferentGame { get; } = new TranslatableString("異なるゲームのセーブデータです");
            [Managed] public static TranslatableString DifferentVersion { get; } = new TranslatableString("セーブデータのバーションが異なります");
            [Managed] public static TranslatableString CorruptedSaveData { get; } = new TranslatableString("セーブデータが壊れています");
            [Managed] public static TranslatableString LoadError { get; } = new TranslatableString("読み込み中にエラーが発生しました");
            [Managed] public static TranslatableString ErrorSavingGlobalData { get; } = new TranslatableString("グローバルデータの保存中にエラーが発生しました");
            [Managed] public static TranslatableString NotExistPath { get; } = new TranslatableString("存在しないパスを指定しました");
            [Managed] public static TranslatableString DelReadOnlyFile { get; } = new TranslatableString("指定されたファイル\"{0}\"は読み込み専用のため削除できません");
            [Managed] public static TranslatableString TooMany2DCharaVarArg { get; } = new TranslatableString("キャラクタ二次元配列変数\"{0}\"の引数が多すぎます");
            [Managed] public static TranslatableString TooMany1DCharaVarArg { get; } = new TranslatableString("キャラクタ配列変数\"{0}\"の引数が多すぎます");
            [Managed] public static TranslatableString TooManyCharaVarArg { get; } = new TranslatableString("キャラクタ変数\"{0}\"の引数が多すぎます");
            //[Managed] public static TranslatableString CanNotOmit2DCharaVarArg { get; } = new TranslatableString("キャラクタ二次元配列変数\"{0}\"の引数は省略できません");
            [Managed] public static TranslatableString CanNotOmit1DCharaVarArg1 { get; } = new TranslatableString("キャラクタ配列変数\"{0}\"の引数は省略できません");
            [Managed] public static TranslatableString CanNotOmit1DCharaVarArg2 { get; } = new TranslatableString("キャラクタ配列変数\"{0}\"の引数は省略できません(コンフィグにより禁止が選択されています)");
            [Managed] public static TranslatableString CanNotOmitCharaVarArg1 { get; } = new TranslatableString("キャラクタ変数\"{0}\"の引数は省略できません");
            [Managed] public static TranslatableString CanNotOmitCharaVarArg2 { get; } = new TranslatableString("キャラクタ変数\"{0}\"の引数は省略できません(コンフィグにより禁止が選択されています)");
            [Managed] public static TranslatableString CanNotOmit3DVarArg { get; } = new TranslatableString("三次元配列変数\"{0}\"の引数は省略できません");
            [Managed] public static TranslatableString CanNotOmit2DVarArg { get; } = new TranslatableString("二次元配列変数\"{0}\"の引数は省略できません");
            [Managed] public static TranslatableString TooMany2DVarArg { get; } = new TranslatableString("二次元配列変数\"{0}\"の引数が多すぎます");
            [Managed] public static TranslatableString TooMany1DVarArg { get; } = new TranslatableString("一次元変数\"{0}\"の引数が多すぎます");
            [Managed] public static TranslatableString OmittedRandArg { get; } = new TranslatableString("RANDの引数が省略されています");
            [Managed] public static TranslatableString RandArgIsZero { get; } = new TranslatableString("RANDの引数に0が与えられています");
            [Managed] public static TranslatableString ZeroDVarHasArg { get; } = new TranslatableString("配列でない変数\"{0}\"を引数付きで呼び出しています");
            [Managed] public static TranslatableString KeywordCanNotEmpty { get; } = new TranslatableString("キーワードを空にはできません");
            [Managed] public static TranslatableString AssignToVarOoR { get; } = new TranslatableString("配列変数\"{0}\"の要素数を超えて代入しようとしました");
            [Managed] public static TranslatableString MissingVarArg { get; } = new TranslatableString("変数\"{0}\"に必要な引数が不足しています");
            [Managed] public static TranslatableString CallStrAsInt { get; } = new TranslatableString("整数型でない変数\"{0}\"を整数型として呼び出しました");
            [Managed] public static TranslatableString CallIntAsStr { get; } = new TranslatableString("文字列型でない変数\"{0}\"を文字列型として呼び出しました");
            [Managed] public static TranslatableString CallNDStrAsInt { get; } = new TranslatableString("整数型配列でない変数\"{0}\"を整数型配列として呼び出しました");
            [Managed] public static TranslatableString CallNDIntAsStr { get; } = new TranslatableString("文字列型配列でない変数\"{0}\"を文字列型配列として呼び出しました");
            [Managed] public static TranslatableString GetSize0DVar { get; } = new TranslatableString("配列型でない変数\"{0}\"の長さを取得しようとしました");
            [Managed] public static TranslatableString CallCharaVarAsVar { get; } = new TranslatableString("キャラクタ変数\"{0}\"を非キャラ変数として呼び出しました");
            [Managed] public static TranslatableString CallVarAsCharaVar { get; } = new TranslatableString("非キャラクタ変数\"{0}\"をキャラ変数として呼び出しました");
            [Managed] public static TranslatableString GetSize0DCharaVar { get; } = new TranslatableString("配列型でないキャラクタ変数\"{0}\"の長さを取得しようとしました");
            [Managed] public static TranslatableString GetSizeCharaVarWithoutDim { get; } = new TranslatableString("{0}次元配列型のキャラ変数\"{1}\"の長さを次元を指定せずに取得しようとしました");
            [Managed] public static TranslatableString GetSizeCharaVarNonExistDim { get; } = new TranslatableString("配列型変数のキャラ変数\"{0}\"の存在しない次元の長さを取得しようとしました");
            [Managed] public static TranslatableString OoRCharaVarFirstArg { get; } = new TranslatableString("キャラクタ配列変数\"{0}\"の第1引数({1})はキャラ登録番号の範囲外です");
            [Managed] public static TranslatableString OoRCharaVarSecondArg { get; } = new TranslatableString("キャラクタ配列変数\"{0}\"の第2引数({1})は配列の範囲外です");
            [Managed] public static TranslatableString OoRCharaVarThirdArg { get; } = new TranslatableString("キャラクタ配列変数\"{0}\"の第3引数({1})は配列の範囲外です");
            [Managed] public static TranslatableString OoRInstructionArg { get; } = new TranslatableString("\"{0}\"命令の第{1}引数({2})は配列\"{3}\"の範囲外です");
            [Managed] public static TranslatableString GetSizeDimError { get; } = new TranslatableString("{0}次元配列型変数\"{1}\"の長さを取得しようとしました");
            [Managed] public static TranslatableString GetSizeNonExistDim { get; } = new TranslatableString("配列型変数\"{0}\"の存在しない次元の長さを取得しようとしました");
            [Managed] public static TranslatableString OoRVarArg { get; } = new TranslatableString("配列変数\"{0}\"の第{1}引数({2})は配列の範囲外です");
            [Managed] public static TranslatableString EmptyRefVar { get; } = new TranslatableString("参照型変数\"{0}\"は何も参照していません");
            [Managed] public static TranslatableString CanNotOmitRefToVar { get; } = new TranslatableString("参照先変数は省略できません");
            [Managed] public static TranslatableString CanNotRefPseudoVar { get; } = new TranslatableString("疑似変数は参照できません");
            [Managed] public static TranslatableString CanNotRefConstVar { get; } = new TranslatableString("定数は参照できません");
            [Managed] public static TranslatableString CanNotGlobalRefLocalVar { get; } = new TranslatableString("広域の参照変数はローカル変数を参照できません");
            [Managed] public static TranslatableString CanNotRefCharaVar { get; } = new TranslatableString("キャラ変数は参照できません");
            [Managed] public static TranslatableString CanNotRefDifferentType { get; } = new TranslatableString("型が異なる変数は参照できません");
            [Managed] public static TranslatableString CanNotRefDifferentDim { get; } = new TranslatableString("次元数が異なる変数は参照できません");
            [Managed] public static TranslatableString SetToPseudoVar { get; } = new TranslatableString("擬似変数\"{0}\"に代入しようとしました");
            [Managed] public static TranslatableString GetSizePseudoVar { get; } = new TranslatableString("擬似変数\"{0}\"の長さを取得しようとしました");
            [Managed] public static TranslatableString GetDimPseudoVar { get; } = new TranslatableString("擬似変数\"{0}\"の配列を取得しようとしました");
            [Managed] public static TranslatableString RandArgIsNegative { get; } = new TranslatableString("RANDの引数に0以下の値({0})が指定されました");
            [Managed] public static TranslatableString SpriteNameAlreadyUsed { get; } = new TranslatableString("同名のリソースがすでに作成されています:{0}");
            [Managed] public static TranslatableString NotDeclaredAnimationSpriteSize { get; } = new TranslatableString("アニメーションスプライトのサイズが宣言されていません");
            [Managed] public static TranslatableString InvalidAnimationSpriteSize { get; } = new TranslatableString("アニメーションスプライトのサイズの指定が適切ではありません");
            [Managed] public static TranslatableString MissingSecondArgumentExtension { get; } = new TranslatableString("第二引数に拡張子がありません:{0}");
            [Managed] public static TranslatableString NotExistImageFile { get; } = new TranslatableString("指定された画像ファイルが見つかりませんでした:{0}");
            [Managed] public static TranslatableString FailedLoadFile { get; } = new TranslatableString("指定されたファイルの読み込みに失敗しました:{0}");
            [Managed] public static TranslatableString TooLargeImageFile { get; } = new TranslatableString("指定された画像ファイルの大きさが大きすぎます(幅及び高さを{0}px以下にすることを強く推奨します):{1}");
            [Managed] public static TranslatableString FailedCreateResource { get; } = new TranslatableString("画像リソースの作成に失敗しました:{0}");
            [Managed] public static TranslatableString SpriteCreateFromFailedResource { get; } = new TranslatableString("作成に失敗したリソースを元にスプライトを作成しようとしました:{0}");
            [Managed] public static TranslatableString SpriteSizeIsNegatibe { get; } = new TranslatableString("スプライトの高さ又は幅には正の値のみ指定できます:{0}");
            [Managed] public static TranslatableString OoRParentImage { get; } = new TranslatableString("親画像の範囲外を参照しています:{0}");
            [Managed] public static TranslatableString FrameTimeIsNegative { get; } = new TranslatableString("フレーム表示時間には正の値のみ指定できます:{0}");
            [Managed] public static TranslatableString FailedAddSpriteFrame { get; } = new TranslatableString("アニメーションスプライトのフレームの追加に失敗しました:{0}");
            [Managed] public static TranslatableString InvalidConfigName { get; } = new TranslatableString("文字列\"{0}\"は適切なコンフィグ名ではありません");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            [Managed] public static TranslatableString CannotRecommendCallLocalVar { get; } = new TranslatableString("コード中でローカル変数を@付きで呼ぶことは推奨されません(代わりに*.ERHファイルの利用を検討してください)");

            [Managed] public static TranslatableString LabelNameMissing { get; } = new TranslatableString("ラベル名がありません");
            [Managed] public static TranslatableString LabelContainsOtherThanUnderline { get; } = new TranslatableString("ラベル名\"{0}\"に\"_\"以外の記号が含まれています");
            [Managed] public static TranslatableString LabelStartedHalfDigit { get; } = new TranslatableString("ラベル名\"{0}\"が半角数字から始まっています");
            [Managed] public static TranslatableString LabelConflictReservedWord1 { get; } = new TranslatableString("関数名\"{0}\"はEmueraの予約語と衝突しています。Emuera専用構文の構文解析に支障をきたす恐れがあります");
            [Managed] public static TranslatableString LabelConflictReservedWord2 { get; } = new TranslatableString("関数名\"{0}\"はEmueraの予約語です");
            [Managed] public static TranslatableString LabelOverwriteInternalExpression { get; } = new TranslatableString("関数名\"{0}\"はEmueraの式中関数を上書きします");
            [Managed] public static TranslatableString LabelNameAlreadyUsedInternalExpression { get; } = new TranslatableString("関数名\"{0}\"はEmueraの式中関数名として使われています");
            [Managed] public static TranslatableString LabelNameAlreadyUsedInternalVariable { get; } = new TranslatableString("関数名\"{0}\"はEmueraの変数で使われています");
            [Managed] public static TranslatableString LabelNameAlreadyUsedInternalInstruction { get; } = new TranslatableString("関数名\"{0}\"はEmueraの変数もしくは命令で使われています");
            [Managed] public static TranslatableString LabelNameAlreadyUsedMacro { get; } = new TranslatableString("関数名\"{0}\"はマクロに使用されています");
            [Managed] public static TranslatableString LabelNameAlreadyUsedRefFunction { get; } = new TranslatableString("関数名\"{0}\"は参照型関数の名称に使用されています");

            [Managed] public static TranslatableString VarContainsOtherThanUnderline { get; } = new TranslatableString("変数名\"{0}\"に\"_\"以外の記号が含まれています");
            [Managed] public static TranslatableString VarConflictReservedWord { get; } = new TranslatableString("変数名\"{0}\"はEmueraの予約語です");
            [Managed] public static TranslatableString VarNameAlreadyUsedInternalInstruction { get; } = new TranslatableString("変数目\"{0}\"はEmueraの変数もしくは命令で使われています");
            [Managed] public static TranslatableString VarNameAlreadyUsedInternalVariable { get; } = new TranslatableString("変数名\"{0}\"はEmueraの変数名として使われています");
            [Managed] public static TranslatableString VarNameAlreadyUsedMacro { get; } = new TranslatableString("変数名\"{0}\"はマクロに使用されています");
            [Managed] public static TranslatableString VarNameAlreadyUsedGlobalVariable { get; } = new TranslatableString("変数名\"{0}\"はユーザー定義の広域変数名に使用されています");
            [Managed] public static TranslatableString VarNameAlreadyUsedRefFunction { get; } = new TranslatableString("変数名\"{0}\"は参照型関数の名称に使用されています");
            [Managed] public static TranslatableString VarStartedHalfDigit { get; } = new TranslatableString("変数名\"{0}\"が半角数字から始まっています");

            [Managed] public static TranslatableString MacroContainsOtherThanUnderline { get; } = new TranslatableString("マクロ名\"{0}\"に\"_\"以外の記号が含まれています");
            [Managed] public static TranslatableString MacroConflictReservedWord { get; } = new TranslatableString("マクロ名\"{0}\"はEmueraの予約語です");
            [Managed] public static TranslatableString MacroNameAlreadyUsedInternalInstruction { get; } = new TranslatableString("マクロ名\"{0}\"はEmueraの変数もしくは命令で使われています");
            [Managed] public static TranslatableString MacroNameAlreadyUsedInternalVariable { get; } = new TranslatableString("マクロ名\"{0}\"はEmueraの変数名として使われています");
            [Managed] public static TranslatableString MacroNameAlreadyUsedMacro { get; } = new TranslatableString("マクロ名\"{0}\"はマクロに使用されています");
            [Managed] public static TranslatableString MacroNameAlreadyUsedGlobalVariable { get; } = new TranslatableString("マクロ名\"{0}\"はユーザー定義の広域変数名に使用されています");
            [Managed] public static TranslatableString MacroNameAlreadyUsedRefFunction { get; } = new TranslatableString("マクロ名\"{0}\"は参照型関数の名称に使用されています");


            [Managed] public static TranslatableString ArgCanNotBeNull { get; } = new TranslatableString("{0}関数: 第{1}引数は省略できません");
            [Managed] public static TranslatableString ArgIsNotStr { get; } = new TranslatableString("{0}関数: 第{1}引数は文字列ではありません");
            [Managed] public static TranslatableString ArgIsNotInt { get; } = new TranslatableString("{0}関数: 第{1}引数は整数ではありません");
            [Managed] public static TranslatableString ArgIsNotVar { get; } = new TranslatableString("{0}関数: 第{1}引数は変数ではありません");
            [Managed] public static TranslatableString ArgIsNotStrVar { get; } = new TranslatableString("{0}関数: 第{1}引数は文字列型変数ではありません");
            [Managed] public static TranslatableString ArgIsNotIntVar { get; } = new TranslatableString("{0}関数: 第{1}引数は整数型変数ではありません");
            [Managed] public static TranslatableString ArgIsNotNDArray { get; } = new TranslatableString("{0}関数: 第{1}引数は{2}次元配列変数ではありません");
            [Managed] public static TranslatableString ArgIsNotNDStrArray { get; } = new TranslatableString("{0}関数: 第{1}引数は文字列型{2}次元配列変数ではありません");
            [Managed] public static TranslatableString ArgIsNotNDIntArray { get; } = new TranslatableString("{0}関数: 第{1}引数は整数型{2}次元配列変数ではありません");
            [Managed] public static TranslatableString TooManyFuncArgs { get; } = new TranslatableString("{0}関数: 引数が多すぎます");
            [Managed] public static TranslatableString NotEnoughArgs { get; } = new TranslatableString("{0}関数: 少なくとも{1}つの引数が必要です");
            [Managed] public static TranslatableString NotValidArgs { get; } = new TranslatableString("{0}関数: 引数がどの書式にも合わせていません | {1}");
            [Managed] public static TranslatableString NotValidArgsReason { get; } = new TranslatableString("書式{0}: {1}");

            [Managed] public static TranslatableString ErrArgsCount { get; } = new TranslatableString("{0}関数: 引数の数が間違っています");

            [Managed] public static TranslatableString IsNotVar { get; } = new TranslatableString("\"{0}\"が変数ではありません");
            [Managed] public static TranslatableString IsNotInt { get; } = new TranslatableString("\"{0}\"が整数型ではありません");
            [Managed] public static TranslatableString IsNotStr { get; } = new TranslatableString("\"{0}\"が文字列型ではありません");



        }
        [Managed]
        public sealed class MessageBox
        {
            public static string Text { get { return trClass[typeof(MessageBox)].Text; } }
            [Managed] public static TranslatableString ConfigError { get; } = new TranslatableString("設定のエラー");
            [Managed] public static TranslatableString TooSmallFontSize { get; } = new TranslatableString("フォントサイズが小さすぎます(8が下限)");
            [Managed] public static TranslatableString LineHeightLessThanFontSize { get; } = new TranslatableString("行の高さがフォントサイズより小さいため、フォントサイズと同じ高さと解釈されます");
            [Managed] public static TranslatableString TooSmallDisplaySaveData { get; } = new TranslatableString("表示するセーブデータ数が少なすぎます(20が下限)");
            [Managed] public static TranslatableString TooLargeDisplaySaveData { get; } = new TranslatableString("表示するセーブデータ数が多すぎます(80が上限)");
            [Managed] public static TranslatableString TooSmallLogSize { get; } = new TranslatableString("ログ表示行数が少なすぎます(500が下限)");
            [Managed] public static TranslatableString FolderCreationFailure { get; } = new TranslatableString("フォルダ作成失敗");
            [Managed] public static TranslatableString FailedCreateSavFolder { get; } = new TranslatableString("savフォルダの作成に失敗しました");
            [Managed] public static TranslatableString SavFolderCreated { get; } = new TranslatableString("savフォルダを作成しました\n現在のデータをsavフォルダ内に移動しますか？");
            [Managed] public static TranslatableString DataTransfer { get; } = new TranslatableString("データ移動");
            [Managed] public static TranslatableString MissingSavFolder { get; } = new TranslatableString("savフォルダは作成されましたが見つかりません\n削除しましたか？");
            [Managed] public static TranslatableString DataTransferFailure { get; } = new TranslatableString("データ移動失敗");
            [Managed] public static TranslatableString FailedMoveSavFiles { get; } = new TranslatableString("savファイルの移動に失敗しました");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");
            //[Managed] public static TranslatableString { get; } = new TranslatableString("");

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
