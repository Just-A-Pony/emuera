using System;
using System.Collections.Generic;
using System.Text;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace MinorShift._Library
{
	[Obfuscation(ApplyToMembers = true, Exclude = false)]
	internal class AclAccessRights
	{
		private bool rights = false;
		internal protected bool Deny;
		internal protected bool Allow
		{
			get { return (!Deny && rights); }
			set { rights = value; }
		}
	}
	[Obfuscation(ApplyToMembers = true, Exclude = false)]
	public static class Sys
	{
		///<summary>
		/// 起動時、起動フォルダの読み書き権限チェック追加: 権限のないところにEmueraがあるときは悪用の疑いあり、終了させる (禁止のほうが優先される)
		///</summary>
		static Sys()
		{
			ExePath = Application.ExecutablePath;
			ExeDir = Path.GetDirectoryName(ExePath) + Path.DirectorySeparatorChar;
			ExeName = Path.GetFileName(ExePath);

			// アクセス権確認
			AccessCheck();
		}

		/// <remarks>そのインスタンスでのExeDirのアクセス許可確認</remarks>
		/// <comment>
		/// Windowsでは、Un*xのように簡単にシステム下にあるかどうかが判定できない
		/// 一部のPCでは、FAT系FSにシステムがインストールされていたりACLがOFFだったりするので、必ずしもACL制御が効いている保証がない
		/// TODO: Windows＆NTFS以外の条件で検証要
		/// </comment>
		[Obfuscation(Exclude = false)]
		private static void AccessCheck()
		{
			DirectorySecurity ExeDirACL;
			var AclAccess = new AclAccessRights();
			var AclWrite = new AclAccessRights();

			try
			{
				try
				{
					ExeDirACL = Directory.GetAccessControl(ExeDir);
					if (ExeDirACL == default) throw new PlatformNotSupportedException();
				}
				catch (Exception e) when (e is PlatformNotSupportedException || e is NotImplementedException)
				{
					DirectoryInfo d = new DirectoryInfo(ExeDir);
					if (d.Attributes.HasFlag(FileAttributes.ReadOnly) || d.Attributes.HasFlag(FileAttributes.System))
					{
						AclAccessDeny = false;
						AclWriteDeny = true;
					}
					else
					{
						AclAccessDeny = false;
						AclWriteDeny = false;
					}
					return;
				}
				// ACL取得できた時はそれに従う
				// TODO: Windows以外ってどうなるの？
				AuthorizationRuleCollection lists = ExeDirACL.GetAccessRules(true, true, typeof(SecurityIdentifier));
				var curIdent = WindowsIdentity.GetCurrent();
				if (null == curIdent || null == lists)
					throw new System.Security.SecurityException();
				var currentuser = new WindowsPrincipal(curIdent);
				foreach (FileSystemAccessRule rule in lists)
				{
					if (rule.IdentityReference.Value.StartsWith("S-1-"))
					{
						var sid = new SecurityIdentifier(rule.IdentityReference.Value);
						if (!currentuser.IsInRole(sid)) continue;
					}
					else
					{
						if (!currentuser.IsInRole(rule.IdentityReference.Value)) continue;
					}
					// 読み取り権限ある？
					if ((FileSystemRights.Read & rule.FileSystemRights) == FileSystemRights.Read)
					{
						if (rule.AccessControlType == AccessControlType.Deny)
							AclAccess.Deny = true;
						else if (rule.AccessControlType == AccessControlType.Allow)
							AclAccess.Allow = true;
					}
					// 書き込み権限ある？
					if ((FileSystemRights.Write & rule.FileSystemRights) == FileSystemRights.Write)
					{
						if (rule.AccessControlType == AccessControlType.Deny)
							AclWrite.Deny = true;
						else if (rule.AccessControlType == AccessControlType.Allow)
							AclWrite.Allow = true;
					}
				}
				currentuser = default;
				AclAccessDeny = AclAccess.Deny || !AclAccess.Allow;
				AclWriteDeny = AclWrite.Deny || !AclWrite.Allow;
			}
			catch (Exception e) when (e is UnauthorizedAccessException || e is IOException || e is System.Security.SecurityException)
			{
				// 権限アクセス自体拒否
				AclAccessDeny = true;
				AclWriteDeny = true;
			}
			catch (Exception ex) when (ex is PlatformNotSupportedException || ex is SystemException || ex is NotImplementedException)
			{
				// 多分 Windows以外またはACL非対応 (でもMonoはとかはそれなり対応してるはず)
				AclAccessDeny = false;
				AclWriteDeny = false;
			}
			finally
			{
				WriteEnable = false;
				GC.Collect();	// アクセス権解放されないと、リムーバブルドライブが取り出せない…
			}
		}
		/// <remarks>
		/// そのインスタンスの場所が読み取り許可されていないことを示す。AclWriteDeny,WriteEnableより優先
		/// (lock機構付き)
		/// </remarks>
		public static bool AclAccessDeny {
			get {
				lock (_aclaccessdeny_lock)
				return aclaccessdeny_;
			}
			set {
				lock (_aclaccessdeny_lock)
				aclaccessdeny_ = value;
			}
		}
		private static bool aclaccessdeny_ = false;
		private static readonly object _aclaccessdeny_lock = new object();

		/// <remarks>
		/// そのインスタンスの場所が書き込み許可されていないことを示す。WriteEnableより優先
		/// (lock機構付き)
		/// </remarks>
		public static bool AclWriteDeny {
			get {
				lock (_wd_lock)
				return !AclAccessDeny && _writedeny;
			}
			set {
				lock (_wd_lock)
				_writedeny = value;
			}
		}
		private static bool _writedeny = false;
		private static readonly object _wd_lock = new object();

		/// <remarks>
		/// そのインスタンスでの書き込み処理の許可。CsvDir/ExeDirが存在すること、または設定で保存が選択されたとき、すでにemuera.configがあるときにtrueになる
		/// AclAccessDeny,AclWriteDenyが true の時は true が返らない
		/// (lock機構付き)
		/// </remarks>
		public static bool WriteEnable {
			get {
				lock (_we_lock)
				return !AclAccessDeny && !AclWriteDeny && _writeenable;
			}
			set { 
				lock (_we_lock)
				_writeenable = value;
			}
		}
		private static bool _writeenable = false;
		private static readonly object _we_lock = new object();

		/// <summary>
		/// 実行ファイルのパス
		/// </summary>
		public static readonly string ExePath;

		/// <summary>
		/// 実行ファイルのディレクトリ。最後にパスセパレータを付けたstring
		/// </summary>
		public static readonly string ExeDir;

		/// <summary>
		/// 実行ファイルの名前。ディレクトリなし
		/// </summary>
		public static readonly string ExeName;

		/// <summary>
		/// 2重起動防止。既に同名exeが実行されているならばtrueを返す
		/// </summary>
		/// <returns></returns>
		public static bool PrevInstance()
		{
			string thisProcessName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
			if (System.Diagnostics.Process.GetProcessesByName(thisProcessName).Length > 1)
			{
				return true;
			}
			return false;

		}
	}
}

