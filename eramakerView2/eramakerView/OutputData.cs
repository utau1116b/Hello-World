using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utau.Eramakerview.Library;
using Utau.Eramakerview.Sub;
using Utau.Eramakerview;

namespace Utau.Eramakerview.GameData
{
	public class OutputData
	{
		public OutputData(MainForm mform, GameBaseData gbData, string[][] paramName, int[] variableData, CharacterTemplate[] charaList)
		{
			mf = mform;
			gb = gbData;
			pname = paramName;
			vd = variableData;
			chara = charaList;
		}

		GameBaseData gb;
		string[][] pname;
		int[] vd;
		CharacterTemplate[] chara;
		MainForm mf;
		CharacterStrData cstr;
		CIntData cint;

		//palamNameList(),charaList(),GameBaseData()
		//makeFile
		// method makeEraIndex～makeEraData2
		//
		//makeFile
		public void MakeHtml(string csvpath)
		{
			this.MakeEraIndex(csvpath);
		}

		//EraIndex.htmlを作成
		private void MakeEraIndex(string csvpath)
		{
			string filepath = csvpath + "EraIndex.html";
			EraStreamWriter eWriter = new EraStreamWriter(mf);

			//ファイルをクリエイト
			eWriter.MakeFile(filepath);

			//書式設定を記述
			this.WriteStyle(eWriter);

			eWriter.WriteLine("<frameset cols=\"180,*\" border=\"0\">");
			eWriter.WriteLine("<frame src=\"EraMenu.html\" name=\"menu\">");
			eWriter.WriteLine("<frame src=\"EraData.html\" name=\"detail\">");
			eWriter.WriteLine("</frameset>");
			
			eWriter.Close();
		}


		//EraMenu.htmlを作成する
		private void MakeEraMenu(string csvpath)
		{
			string filepath = csvpath + "EraMenu.html";
			EraStreamWriter eWriter = new EraStreamWriter(mf);
			
			//ファイルをクリエイト
			eWriter.MakeFile(filepath);

			//書式設定を記述
			this.WriteStyle(eWriter);

			eWriter.WriteLine("<body class=\"menu\">");
			eWriter.WriteLine("<div class=\"menu_base\">");
			eWriter.WriteLine("<h1 class=\"menu\">MENU</h1>");
			eWriter.WriteLine("<h2 class=\"menu\"><a class=\"h\" href=\"EraData.html#gamebase\"  target=\"detail\">ゲーム基本情報</a></h2>");
			eWriter.WriteLine("<h2 class=\"menu\"><a class=\"h\" href=\"EraData.html#char_list\" target=\"detail\">キャラ情報</a></h2>");

			for (int i = 0; i < chara.Length; i++)
			{

				//キャラクター記述
				if (chara[i].NO != null)
				{
					eWriter.WriteLine("<div class=\"menu_char\"><a class=\"menu_char\" href=\"EraData.html#c000" + chara[i].NO + "\" target=\"detail\">" + chara[i].NAME + "</a></div>");
				}
				//eWriter.WriteLine("<div class=\"menu_char\"><a class=\"menu_char\" href=\"EraData.html#c0000\" target=\"detail\">"+"</a></div>");
				//eWriter.WriteLine("<div class=\"menu_char\"><a class=\"menu_char\" href=\"EraData.html#c0001\" target=\"detail\">天海春香</a></div>");
				//eWriter.WriteLine("<div class=\"menu_char\"><a class=\"menu_char\" href=\"EraData.html#c0002\" target=\"detail\">如月千早</a></div>");
				//eWriter.WriteLine("<div class=\"menu_char\"><a class=\"menu_char\" href=\"EraData.html#c0003\" target=\"detail\">萩原雪歩</a></div>");
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

			eWriter.Close();

		}


		private void DateSummary(string csvparh)
		{
			string filepath = csvparh + "DataSummary.html";
			EraStreamWriter eWriter = new EraStreamWriter(mf);
			int i,j;//ループ用変数
			j = 0;
			//ファイルをクリエイト
			eWriter.MakeFile(filepath);

			//書式設定を記述
			this.WriteStyle(eWriter);

			eWriter.WriteLine("<body>");
			eWriter.WriteLine("<div class=\"base\">");
			eWriter.WriteLine("<h1>" + gb.title  +"</h1>");
			eWriter.WriteLine("<a name=\"summary\">");
			eWriter.WriteLine("<h2>EraData Summary</h2>");


			eWriter.WriteLine("<div class=\"datasummary\">");
			eWriter.WriteLine("  <table class=\"datasummary\">");
			eWriter.WriteLine("    <tr><th class=\"summarytag\" colspan=\"2\">PALAM</th></tr>");
			eWriter.WriteLine("  </table>");
			eWriter.WriteLine("  <table class=\"datasummary\">");

			for (i = 0; i < pname[(int)CIntData.PALAM].Length; i++)
			{
				if (j == 0)
				{
					eWriter.WriteLine("<div class=\"datasummary\">");
					eWriter.WriteLine("  <table class=\"datasummary\">");
					eWriter.WriteLine("    <tr><th class=\"summarytag\" colspan=\"2\">PALAM</th></tr>");
					eWriter.WriteLine("  </table>");
					eWriter.WriteLine("  <table class=\"datasummary\">");
				}
	
				eWriter.WriteLine("    <tr><td class=\"summarytag\">" + (int)(CIntData.PALAM) + "</td><td class=\"summarydata\">" + pname[(int)CIntData.PALAM][i] + "</td></tr>");
				
				j++;
				if (j == 18) j = 0;
			}
			//空いたスペースを補う
			for (i = j; i < 18; i++)
			{
				eWriter.WriteLine("    <tr><td class=\"summarytag_empty\">　</td><td class=\"summarydata_empty\"></td></tr>");
			}
		}

		//スタイルを記述する
		private void WriteStyle(EraStreamWriter eWriter)
		{
			eWriter.WriteLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01//EN\">");
			eWriter.WriteLine("<html>");
			eWriter.WriteLine("<head>");
			eWriter.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html;charset=Shift_JIS\">");
			eWriter.WriteLine("<title>" + gb.title + "</title>");
			eWriter.WriteLine("<style type=\"text/css\">");
			eWriter.WriteLine("<!--");
			eWriter.WriteLine("body {");
			eWriter.WriteLine("  color: #CCCCCC;");
			eWriter.WriteLine("  background-color: black;");
			eWriter.WriteLine("  font-family : Trebuchet MS, Arial, Helvetica, sans-serif;");
			eWriter.WriteLine("  font-weight : normal;");
			eWriter.WriteLine("  text-align : left;");
			eWriter.WriteLine("  line-height: 1.2em;");
			eWriter.WriteLine("  margin: 0px;");
			eWriter.WriteLine("  padding: 0px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("a {");
			eWriter.WriteLine("  text-decoration: none;");
			eWriter.WriteLine("  margin: 0px;");
			eWriter.WriteLine("  padding: 0px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("a:hover {");
			eWriter.WriteLine("  text-decoration: underline;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("a.h {");
			eWriter.WriteLine("  color: #EE0000;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("a.menu_char {");
			eWriter.WriteLine("  color: #EE0000;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("h1 {");
			eWriter.WriteLine("  color: #EE0000;");
			eWriter.WriteLine("  font-size: 24pt;");
			eWriter.WriteLine("  line-height: 1.4em;");
			eWriter.WriteLine("  background-color: black;");
			eWriter.WriteLine("  margin: 4px 0px 12px 0px;");
			eWriter.WriteLine("  padding: 4px 8px 4px 8px;");
			eWriter.WriteLine("  border-top:   2px #990000 solid;;");
			eWriter.WriteLine("  border-bottom:2px #990000 solid;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("h2 {");
			eWriter.WriteLine("  color: #EE0000;");
			eWriter.WriteLine("  font-size: 18pt;");
			eWriter.WriteLine("  line-height: 1.4em;");
			eWriter.WriteLine("  background-color: black;");
			eWriter.WriteLine("  margin: 4px 0px 12px 0px;");
			eWriter.WriteLine("  padding: 2px 4px 2px 16px;");
			eWriter.WriteLine("  border-top:   2px #990000 solid;;");
			eWriter.WriteLine("  border-bottom:2px #990000 solid;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("h3 {");
			eWriter.WriteLine("  color: #EE0000;");
			eWriter.WriteLine("  font-size: 14pt;");
			eWriter.WriteLine("  line-height: 1.4em;");
			eWriter.WriteLine("  background-color: black;");
			eWriter.WriteLine("  margin: 0px;");
			eWriter.WriteLine("  padding: 1px 0px 0px 16px;");
			eWriter.WriteLine("  border-top:   2px #990000 solid;;");
			eWriter.WriteLine("  border-bottom:2px #990000 solid;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("h4 {");
			eWriter.WriteLine("  color: #DD0000;");
			eWriter.WriteLine("  font-size: 12pt;");
			eWriter.WriteLine("  line-height: 1.4em;");
			eWriter.WriteLine("  background-color: black;");
			eWriter.WriteLine("  margin: 0px;");
			eWriter.WriteLine("  padding: 4px 0px 0px 32px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine(".base {");
			eWriter.WriteLine("  width : 700px;");
			eWriter.WriteLine("  background-color : #333333;");
			eWriter.WriteLine("  margin : 0 auto;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("table {");
			eWriter.WriteLine("  border: none;");
			eWriter.WriteLine("  border-spacing: 4px;");
			eWriter.WriteLine("  margin: 0px;");
			eWriter.WriteLine("  padding: 0px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("table.gamebase {");
			eWriter.WriteLine("  margin: 0px 0px 32px 70px;");
			eWriter.WriteLine("  border-spacing: 16px 4px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("table.Ability {");
			eWriter.WriteLine("  margin: 0px 0px 32px 70px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("table.Palam {");
			eWriter.WriteLine("  margin: 0px 0px 32px 70px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("table.Talent {");
			eWriter.WriteLine("  margin: 0px 0px 32px 70px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("table.Item {");
			eWriter.WriteLine("  margin: 0px 0px 32px 70px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("table.Mark {");
			eWriter.WriteLine("  margin: 0px 0px 32px 70px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("table.Train {");
			eWriter.WriteLine("  margin: 0px 0px 32px 70px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("table.Exp {");
			eWriter.WriteLine("  margin: 0px 0px 32px 70px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("td.tag {");
			eWriter.WriteLine("  font-weight: bold;");
			eWriter.WriteLine("	text-align: right;");
			eWriter.WriteLine("  white-space: nowrap;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("td.data {");
			eWriter.WriteLine("  padding: 0px 16px 0px 0px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("table.char {");
			eWriter.WriteLine("  width: 600px;");
			eWriter.WriteLine("  border: 0px;");
			eWriter.WriteLine("  background: #555555;");
			eWriter.WriteLine("  margin: 8px 0px 48px 70px;");
			eWriter.WriteLine("  padding: 0px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("td.char_palam {");
			eWriter.WriteLine("  width: 180px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("span.life {");
			eWriter.WriteLine("  color: #EE0000;");
			eWriter.WriteLine("  font-weight: bold;");
			eWriter.WriteLine("  margin-left: 42px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("span.mind {");
			eWriter.WriteLine("  color: #2222FF;");
			eWriter.WriteLine("  font-weight: bold;");
			eWriter.WriteLine("  margin-left: 42px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("span.sei {");
			eWriter.WriteLine("  color: #EEEEEE;");
			eWriter.WriteLine("  font-weight: bold;");
			eWriter.WriteLine("  margin-left: 42px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("table.char_ability {");
			eWriter.WriteLine("  min-width: 190px;");
			eWriter.WriteLine("  font-weight: bold;");
			eWriter.WriteLine("  text-align: right;");
			eWriter.WriteLine("  margin-left: 42px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("td.char_ability_tag {");
			eWriter.WriteLine("  width: 124px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("td.char_ability_data {");
			eWriter.WriteLine("  width: 32px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("div.char_talent {");
			eWriter.WriteLine("  font-weight: bold;");
			eWriter.WriteLine("  line-height: 1.6em;");
			eWriter.WriteLine("  margin-left: 42px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("span.talent {");
			eWriter.WriteLine("  white-space: nowrap;");
			eWriter.WriteLine("  margin: 0px 5px 0px 0px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("table.char_exp {");
			eWriter.WriteLine("  font-weight: bold;");
			eWriter.WriteLine("  text-align: right;");
			eWriter.WriteLine("  margin-left: 42px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("td.char_exp_tag {");
			eWriter.WriteLine("  width: 124px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("td.char_exp_data {");
			eWriter.WriteLine("  width: 32px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("table.char_relation {");
			eWriter.WriteLine("  font-weight: bold;");
			eWriter.WriteLine("  text-align: right;");
			eWriter.WriteLine("  margin-left: 42px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("td.relation {");
			eWriter.WriteLine("  width: 124px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("a.relation_posi {");
			eWriter.WriteLine("  color: #EEAAAA;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("a.relation_nega {");
			eWriter.WriteLine("  color: #9999EE;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("a.relation_normal {");
			eWriter.WriteLine("  color: #CCCCCC;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("td.char_relation_data {");
			eWriter.WriteLine("  width: 32px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("table.char_flag {");
			eWriter.WriteLine("  font-weight: bold;");
			eWriter.WriteLine("  text-align: right;");
			eWriter.WriteLine("  margin-left: 42px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("td.char_flag_tag {");
			eWriter.WriteLine("  width: 124px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("td.char_flag_data {");
			eWriter.WriteLine("  width: 32px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("body.menu_base {");
			eWriter.WriteLine("  color: #CCCCCC;");
			eWriter.WriteLine("  background-color: black;");
			eWriter.WriteLine("  font-family : Trebuchet MS, Arial, Helvetica, sans-serif;");
			eWriter.WriteLine("  font-weight : normal;");
			eWriter.WriteLine("  text-align : left;");
			eWriter.WriteLine("  line-height: 1.2em;");
			eWriter.WriteLine("  margin: 0px;");
			eWriter.WriteLine("  padding: 0px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine(".menu_base {");
			eWriter.WriteLine("  width : 140px;");
			eWriter.WriteLine("  margin : 0 auto;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("h1.menu {");
			eWriter.WriteLine("  font-size: 14pt;");
			eWriter.WriteLine("  margin: 2px 0px 2px 0px;");
			eWriter.WriteLine("  padding: 6px 0px 6px 8px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("h2.menu {");
			eWriter.WriteLine("  font-size: 11pt;");
			eWriter.WriteLine("  margin: 12px 0px 2px 0px;");
			eWriter.WriteLine("  padding: 2px 0px 2px 14px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("div.menu_char {");
			eWriter.WriteLine("  font-size: 10px;");
			eWriter.WriteLine("  font-weight: bold;");
			eWriter.WriteLine("  margin: 0px 0px 0px 24px;");
			eWriter.WriteLine("  padding: 0px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("div.datasummary {");
			eWriter.WriteLine("  float: left;");
			eWriter.WriteLine("  margin: 0px 0px 4px 1px;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("table.datasummary {");
			eWriter.WriteLine("  table-layout: fixed;");
			eWriter.WriteLine("  width: 130px;");
			eWriter.WriteLine("  padding: 0px;");
			eWriter.WriteLine("  line-height: 1.05em;");
			eWriter.WriteLine("  border-collapse: collapse; ");
			eWriter.WriteLine("  letter-spacing: -0.05em;");
			eWriter.WriteLine("  overflow: hidden;");
			eWriter.WriteLine("  white-space: nowrap;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("th.summarytag {");
			eWriter.WriteLine("  width: 130px;");
			eWriter.WriteLine("  color: #EE0000;");
			eWriter.WriteLine("  background-color: #black;");
			eWriter.WriteLine("  padding-right: 4px;");
			eWriter.WriteLine("  text-align: center;");
			eWriter.WriteLine("  font-weight: bold;");
			eWriter.WriteLine("  border: 1px #666666 solid;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("th.summarytag_empty {");
			eWriter.WriteLine("  color:            #333333;");
			eWriter.WriteLine("  background-color: #333333;");
			eWriter.WriteLine("  border: none;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("td.summarytag {");
			eWriter.WriteLine("  width: 26px;");
			eWriter.WriteLine("  color: #990000;");
			eWriter.WriteLine("  background-color: #black;");
			eWriter.WriteLine("  padding-right: 4px;");
			eWriter.WriteLine("  text-align: right;");
			eWriter.WriteLine("  font-weight: bold;");
			eWriter.WriteLine("  border: 1px #666666 solid;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("td.summarytag_empty {");
			eWriter.WriteLine("  color:            #333333;");
			eWriter.WriteLine("  background-color: #333333;");
			eWriter.WriteLine("  border: 1px #333333 solid;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("td.summarydata {");
			eWriter.WriteLine("  width: 98px;");
			eWriter.WriteLine("  background-color: #222222;");
			eWriter.WriteLine("  padding-left: 4px;");
			eWriter.WriteLine("  font-size: small;");
			eWriter.WriteLine("  border: 1px #666666 solid;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("td.summarydata_empty {");
			eWriter.WriteLine("  color:            #333333;");
			eWriter.WriteLine("  background-color: #333333;");
			eWriter.WriteLine("  border: 1px #333333 solid;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("-->");
			eWriter.WriteLine("</style>");
			eWriter.WriteLine("</head>");
			/*
			eWriter.WriteLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01//EN\">");
			eWriter.WriteLine("<html>");
			eWriter.WriteLine("<head>");
			eWriter.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html;charset=Shift_JIS\">");
			eWriter.WriteLine("<title>eraIm@sP</title>");
			eWriter.WriteLine("<style type=\"text/css\">");
			
			eWriter.WriteLine("<!--");
			eWriter.WriteLine("body {");
			eWriter.WriteLine("color: #CCCCCC;");
			eWriter.WriteLine("background-color: black;");
			eWriter.WriteLine("font-family : Trebuchet MS, Arial, Helvetica, sans-serif;");
			eWriter.WriteLine("font-weight : normal;");
			eWriter.WriteLine("text-align : left;");
			eWriter.WriteLine("line-height: 1.2em;");
			eWriter.WriteLine("margin: 0px;");
			eWriter.WriteLine("padding: 0px;");
			eWriter.WriteLine("}");

			eWriter.WriteLine("a {");
			eWriter.WriteLine("text-decoration: none;");
			eWriter.WriteLine("margin: 0px;");
			eWriter.WriteLine("padding: 0px;");
			eWriter.WriteLine("}");

			eWriter.WriteLine("a:hover{");

			eWriter.WriteLine("text-decoration: underline;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("");
			eWriter.WriteLine("");
			eWriter.WriteLine("");
			eWriter.WriteLine("");
			eWriter.WriteLine("");
			eWriter.WriteLine("");
			eWriter.WriteLine("");
			eWriter.WriteLine("");

			*/
		}

	}
}
