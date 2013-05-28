using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Utau.Eramakerview.Library
{
	//ディレクトリへのパス
	public static class Library
	{
		static Library()
		{
			ExePath = Application.ExecutablePath;
			ExeDir = Path.GetDirectoryName(ExePath) + "\\";
			ExeName = Path.GetFileName(ExePath);
		}

		//実行ファイルのパス
		public static readonly string ExePath;
		//実行ファイルのディレクトリ。最後に\を付けたstring
		public static readonly string ExeDir;
		//実行ファイルの名前
		public static readonly string ExeName;
	}
}
