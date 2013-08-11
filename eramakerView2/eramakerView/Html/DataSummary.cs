using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utau.Eramakerview.GameData.Html
{
	class DataSummary:HtmlBase
	{

		public DataSummary(string csvpath, OutputData outdata, MainForm mf): base(outdata, mf)
		{
			filepath = csvpath + "DataSummary.html";
		}

		protected override void HtmlList()
		{
			int i, j, k, count;//ループ用変数
			StyleList style = new StyleList(eWriter, output);
			i = 0;
			j = 0;
			k = 0;
			count = 0;
			
			style.WriteStyle();

			eWriter.WriteLine("<body>");
			eWriter.WriteLine("<div class=\"base\">");
			eWriter.WriteLine("<h1>" + output.gb.title + "</h1>");
			eWriter.WriteLine("<a name=\"summary\">");
			eWriter.WriteLine("<h2>EraData Summary</h2>");

			/* DataSummary 本文 */

			for (count = 0; count < 13; count++)
			{
				output.SetPID(out output.menber, count);

				if (output.menber.ID == 998)//エラーならループし直し
					continue;
				k = 0;
				for (i = 0; i < output.pname[output.menber.ID].Length; i++)
				{
					if (j == 0 && k == 0)
					{
						k = 1;
						eWriter.WriteLine("<div class=\"datasummary\">");
						eWriter.WriteLine("  <table class=\"datasummary\">");
						eWriter.WriteLine("    <tr><th class=\"summarytag\" colspan=\"2\">" + output.menber.name + "</th></tr>");
						eWriter.WriteLine("  </table>");
						eWriter.WriteLine("  <table class=\"datasummary\">");
					}
					if (output.pname[output.menber.ID][i] != null && output.pname[output.menber.ID][i] != "")
					{
						eWriter.WriteLine("    <tr><td class=\"summarytag\">" + (int)(i) + "</td><td class=\"summarydata\">" + output.pname[output.menber.ID][i] + "</td></tr>");
						j++;

					}
					if (j == 18)
					{
						eWriter.WriteLine("  </table>");
						eWriter.WriteLine("</div>");
						k = 0;
						j = 0;//ループリセット
					}
				}

				int check = 0;

				//空いたスペースを補う
				for (i = j; i < 18; i++)
				{
					check++;
					eWriter.WriteLine("    <tr><td class=\"summarytag_empty\">　</td><td class=\"summarydata_empty\"></td></tr>");
				}
				if (check > 1)
				{
					eWriter.WriteLine("  </table>");
					eWriter.WriteLine("</div>");
				}
				i = 0;
				j = 0;
				k = 0;
			}
			eWriter.WriteLine("</a>");
			eWriter.WriteLine("</div>");
			eWriter.WriteLine("</body>");

		}


	}
}
