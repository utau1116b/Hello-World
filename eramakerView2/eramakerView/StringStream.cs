﻿using System;
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
		}

		public string source;

	}
}
