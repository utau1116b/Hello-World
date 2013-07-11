using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Utau.Eramakerview.Sub
{
	class EraStreamWriter:IDisposable
	{
		//このクラスではhtmlを作成する
		public EraStreamWriter(MainForm mform) 
		{
			mf = mform;
		}

		//string filepath;
		//string filename;
		StreamWriter writer;
		FileStream stream;
		Encoding Encode = Encoding.GetEncoding("SHIFT-JIS");
		MainForm mf;

		public bool MakeFile(string filepath)
		{
			try
			{
				stream = new FileStream(filepath, FileMode.Create, FileAccess.Write);
				writer = new StreamWriter(stream, Encode);
			}
			catch
			{
				return false;
			}
			return true;
		}

		public void WriteLine(string word) 
		{
			writer.WriteLine(word);
		}

		public void Close() { this.Dispose(); }
		bool disposed = false;

		#region Idisposableメンバ

		public void Dispose()
		{
			if (disposed)
				return;
			if (writer != null)
				writer.Close();
			else if (stream != null)
				stream.Close();
			//filepath = null;
			//filename = null;
			writer = null;
			stream = null;
			disposed = true;
		}

		#endregion

	}
}