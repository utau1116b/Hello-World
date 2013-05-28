using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utau.Eramakerview.Sub
{
	internal sealed class StringStream
	{
		public StringStream(string s)
		{
			source = s;
			if (source == null)
				source = "";
			pointer = 0;
		}

		string source;
		int pointer;

		public string Substring()
		{
			if (pointer >= source.Length)
				return "";
			else if (pointer == 0)
				return source;
			return source.Substring(pointer);
		}

		public string Substring(int start, int length)
		{
			if (start >= source.Length || length == 0)
				return "";
			if (start + length > source.Length)
				length = source.Length - start;
			return source.Substring(start, length);
		}

		public bool EOS { get { return pointer >= source.Length; } }
	}
}
