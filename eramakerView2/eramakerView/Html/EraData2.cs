using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utau.Eramakerview.Library;
using Utau.Eramakerview.Sub;
using Utau.Eramakerview;

namespace Utau.Eramakerview.GameData.Html
{
	class EraData2:HtmlBase
	{
		public EraData2(string csvpath, OutputData outdata, MainForm mf): base(outdata, mf)
		{
			filepath = csvpath + "EraData2.html";
			param = outdata.pname;
		}

		private string[][] param;
		//static SortedDictionary<int, int> sdict;

		protected override void HtmlList()
		{
			StyleList style = new StyleList(eWriter, output);
			style.WriteStyle();

			eWriter.WriteLine("<body>");
			eWriter.WriteLine("<div class=\"base\">");
			eWriter.WriteLine("<h1>"+output.gb.title+"</h1>");
			//eWriter.WriteLine("<a name=\"palam\">");
			//eWriter.WriteLine("<h2>パラメータ</h2>");
			//eWriter.WriteLine("<table class=\"Palam\">");
			
			int count = 0;
			int index = 0;
			string str0 = "", str1 = "", str2 = "";

			for (int j = 0; j < 7; j++)
			{
				switch (j)
				{
					case 0:
						index = (int)CIntData.SOURCE;
						str0 = "palam";
						str1 = "パラメーター";
						str2 = "Palam";
						break;
					case 1:
						index = (int)CIntData.TALENT;
						str0 = "talent";
						str1 = "素質";
						str2 = "Talent";
						break;
					case 2:
						index = (int)CIntData.ABL;
						str0 = "ability";
						str1 = "能力";
						str2 = "Ability";
						break;
					case 3:
						index = (int)CIntData.MARK;
						str0 = "mark";
						str1 = "刻印";
						str2 = "Mark";
						break;
					case 4:
						index = (int)CIntData.EXP;
						str0 = "exp";
						str1 = "経験";
						str2 = "Exp";
						break;
					case 5:
						index = (int)CIntData.TRAIN;
						str0 = "train";
						str1 = "調教";
						str2 = "Train";
						break;
					case 6:
						index = (int)CIntData.ITEM;
						str0 = "item";
						str1 = "アイテム";
						str2 = "Item";
						break;
				}

				eWriter.WriteLine("<a name=\""+str0+"\">");
				eWriter.WriteLine("<h2>"+str1+"</h2>");
				eWriter.WriteLine("<table class=\""+str2+"\">");

				count = 0;

				for (int i = 0; i < param[index].Length; i++)
				{
					if (param[index][i] != "" && param[index][i] != null)
					{
						if (count == 0)
						{
							eWriter.WriteLine("  <tr>");
						}

						eWriter.WriteLine("      <td class=\"tag\">" + i + ".</td>");
						eWriter.WriteLine("      <td class=\"data\">" + param[index][i] + "</td>");


						count++;

						if (count == 2)
						{
							eWriter.WriteLine("  </tr>");
							count = 0;
						}
					}
				}
				if (count == 1)
				{
					eWriter.WriteLine("  </tr>");
				}
				eWriter.WriteLine("</table>");
				eWriter.WriteLine("</a>");

			}


			eWriter.WriteLine("</div>");
			eWriter.WriteLine("</body>");
			eWriter.WriteLine("</html>");
			
		}
	}
}
