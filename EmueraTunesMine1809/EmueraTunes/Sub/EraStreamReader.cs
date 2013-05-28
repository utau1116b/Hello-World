using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MinorShift.Emuera.Sub
{
	internal sealed class EraStreamReader : IDisposable
	{

		string filepath;
		string filename;
		int lineNo = 0;
		StreamReader reader;
		FileStream stream;
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
			lineNo = 0;
			try
			{
				stream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				reader = new StreamReader(stream, Config.Encode);
			}
			catch
			{
				this.Dispose();
				return false;
			}
			return true;
		}

		public string ReadLine()
		{
			lineNo++;
			return reader.ReadLine();
		}

		/// <summary>
		/// 次の有効な行を読む。LexicalAnalyzer経由でConfigを参照するのでConfig完成までつかわないこと。
		/// </summary>
		public StringStream ReadEnabledLine()
		{
			string line = null;
			StringStream st = null;
			while (true)
			{
				line = reader.ReadLine();
				lineNo++;
				if (line == null)
					return null;
				if (line.Length == 0)
					continue;
				st = new StringStream(line);
				LexicalAnalyzer.SkipWhiteSpace(st);
				if (st.EOS)
					continue;
				return st;
			}
		}

		/// <summary>
		/// 直前に読んだ行の行番号
		/// </summary>
		public int LineNo
		{ get { return lineNo; } }
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
			if (reader != null)
				reader.Close();
			else if (stream != null)
				stream.Close();
			filepath = null;
			filename = null;
			reader = null;
			stream = null;
			disposed = true;
		}

		#endregion
	}
}
