using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utau.Eramakerview.Library;
using Utau.Eramakerview.Sub;
using Utau.Eramakerview;

namespace Utau.Eramakerview.GameData.Html
{
	class EraIndex : HtmlBase
	{
		public EraIndex(string csvpath, OutputData outdata, MainForm mf): base(outdata, mf)
		{
			filepath = csvpath + "EraIndex.html";
		}

		protected override void HtmlList()
		{
			StyleList style = new StyleList(eWriter, output);
			style.WriteStyle();

			eWriter.WriteLine("<frameset cols=\"180,*\" border=\"0\">");
			eWriter.WriteLine("<frame src=\"EraMenu.html\" name=\"menu\">");
			eWriter.WriteLine("<frame src=\"EraData.html\" name=\"detail\">");
			eWriter.WriteLine("</frameset>");
			eWriter.WriteLine("</html>");
		}

	}
}
