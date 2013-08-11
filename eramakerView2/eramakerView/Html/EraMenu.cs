using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utau.Eramakerview.Library;
using Utau.Eramakerview.Sub;
using Utau.Eramakerview;

namespace Utau.Eramakerview.GameData.Html
{
	class EraMenu:HtmlBase
	{
		public EraMenu(string csvpath, OutputData outdata, MainForm mf): base(outdata, mf)
		{
			filepath = csvpath + "EraMenu.html";
		}

		protected override void HtmlList()
		{
			StyleList style = new StyleList(eWriter, output);
			style.WriteStyle();

			eWriter.WriteLine("<body class=\"menu\">");
			eWriter.WriteLine("<div class=\"menu_base\">");
			eWriter.WriteLine("<h1 class=\"menu\">MENU</h1>");
			eWriter.WriteLine("<h2 class=\"menu\"><a class=\"h\" href=\"EraData.html#gamebase\"  target=\"detail\">ゲーム基本情報</a></h2>");
			eWriter.WriteLine("<h2 class=\"menu\"><a class=\"h\" href=\"EraData.html#char_list\" target=\"detail\">キャラ情報</a></h2>");

			for (int i = 0; i < output.chara.Count; i++)
			{

				//キャラクター記述
				if (output.chara[i].NAME != null)
				{
					eWriter.WriteLine("<div class=\"menu_char\"><a class=\"menu_char\" href=\"EraData.html#c000" + output.chara[i].NO + "\" target=\"detail\">" + output.chara[i].NAME + "</a></div>");
				}
			}

			eWriter.WriteLine("<h2 class=\"menu\"><a class=\"h\" href=\"DataSummary.html#summary\"   target=\"detail\">Data Summary</a></h2>");
			eWriter.WriteLine("<h2 class=\"menu\"><a class=\"h\" href=\"EraData2.html#palam\"     target=\"detail\">パラメータ</a></h2>");
			eWriter.WriteLine("<h2 class=\"menu\"><a class=\"h\" href=\"EraData2.html#talent\"    target=\"detail\">素質</a></h2>");
			eWriter.WriteLine("<h2 class=\"menu\"><a class=\"h\" href=\"EraData2.html#ability\"   target=\"detail\">能力</a></h2>");
			eWriter.WriteLine("<h2 class=\"menu\"><a class=\"h\" href=\"EraData2.html#mark\"      target=\"detail\">刻印</a></h2>");
			eWriter.WriteLine("<h2 class=\"menu\"><a class=\"h\" href=\"EraData2.html#exp\"       target=\"detail\">経験</a></h2>");
			eWriter.WriteLine("<h2 class=\"menu\"><a class=\"h\" href=\"EraData2.html#train\"     target=\"detail\">調教</a></h2>");
			eWriter.WriteLine("<h2 class=\"menu\"><a class=\"h\" href=\"EraData2.html#item\"      target=\"detail\">アイテム</a></h2>");
			eWriter.WriteLine("</div>");
			eWriter.WriteLine("</body>");

		}
	}
}
