using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Utau.Eramakerview.Sub
{
	class EraStreamReader : IDisposable
	{
		string filepath;
		string filename;
		int lineNo = 0;
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
			lineNo = 0;
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
			lineNo++;
			return reader.ReadLine();
		}

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
				if (st.EOS)
					continue;
				return st;
			}
		}
		

		bool disposed = false;
		#region Idisposableメンバ

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
