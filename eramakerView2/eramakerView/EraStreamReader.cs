using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Utau.Eramakerview.Sub
{
	class EraStreamReader : IDisposable
	{
		//テスト用コンストラクタ
		public EraStreamReader(MainForm mf) 
		{
			mForm = mf;
		}

		MainForm mForm;
		string filepath;
		string filename;
		//int lineNo = 0;
		StreamReader reader;
		FileStream stream;
		Encoding Encode = Encoding.GetEncoding("SHIFT-JIS");

		public bool Open(string path)
		{
			return Open(path, Path.GetFileName(path));
		}

		public bool Open(string path, string name)
		{
			filepath = path;
			filename = name;
			
			try
			{
				stream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Write);
				reader = new StreamReader(stream, Encode);
			}
			catch
			{
				return false;
			}
			return true;
		}

		public string ReadLine()
		{
			return reader.ReadLine();
		}

		public void ReadTest() 
		{
			while (!reader.EndOfStream) 
			{
				//mForm.WriteLabel(reader.ReadLine());
			}
		}

		public void Close() { this.Dispose(); }
		bool disposed = false;

		#region Idisposableメンバ

		public bool EOF()
		{
			if (reader.EndOfStream)
				return true;
			else
				return false;

		}

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
