using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utau.Eramakerview.Library;
using Utau.Eramakerview.Sub;
using Utau.Eramakerview;

namespace Utau.Eramakerview.GameData.Html
{
	class HtmlBase
	{
		protected string filepath;
		protected EraStreamWriter eWriter;
		protected OutputData output;

		public HtmlBase(OutputData opdata, MainForm form)
		{
			eWriter = new EraStreamWriter(form);
			output = opdata;
		}

		//StreamWriterの準備
		public virtual void Create()
		{
			eWriter.MakeFile(filepath);
		}

		public void OutputDataList() 
		{
			this.HtmlList();
		}

		public void Close() 
		{
			eWriter.Close();
		}

		protected virtual void HtmlList() 
		{

		}
	}
}
