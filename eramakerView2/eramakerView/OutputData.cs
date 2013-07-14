using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
		PMEMBER menber;


		public void MakeHtml(string csvpath)
		{
			Parallel.Invoke(
			() =>
			{
				this.MakeEraIndex(csvpath);
			},

			() =>
			{
				this.MakeEraMenu(csvpath);
			},

			() =>
			{
				this.DateSummary(csvpath);
			},
			() =>
			{
				this.EraData(csvpath);
			});
		}

		/*-----------------------------------------------EraIndex.html-------------------------------------------------------------------------*/			

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
			eWriter.WriteLine("</html>");
			
			eWriter.Close();
		}

		/*-------------------------------------------------EraMenu.html-----------------------------------------------------------------------*/

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
				if (chara[i].NAME != null)
				{
					eWriter.WriteLine("<div class=\"menu_char\"><a class=\"menu_char\" href=\"EraData.html#c000" + chara[i].NO + "\" target=\"detail\">" + chara[i].NAME + "</a></div>");
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

			eWriter.Close();

		}

		/*-------------------------------------------------------DataSummary.html-----------------------------------------------------------------*/

		private void DateSummary(string csvparh)
		{
			string filepath = csvparh + "DataSummary.html";
			EraStreamWriter eWriter = new EraStreamWriter(mf);
			int i,j,k,count;//ループ用変数
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

			/* DataSummary 本文 */

			for (count = 0; count < 13; count++)
			{
				SetPID(out menber, count);

				if (menber.ID == 998)//エラーならループし直し
					continue;
				k = 0;
				for (i = 0; i < pname[menber.ID].Length; i++)
				{
					if (j == 0 && k == 0)
					{
						k = 1;
						eWriter.WriteLine("<div class=\"datasummary\">");
						eWriter.WriteLine("  <table class=\"datasummary\">");
						eWriter.WriteLine("    <tr><th class=\"summarytag\" colspan=\"2\">" + menber.name +"</th></tr>");
						eWriter.WriteLine("  </table>");
						eWriter.WriteLine("  <table class=\"datasummary\">");
					}
					if (pname[menber.ID][i] != null && pname[menber.ID][i] != "")
					{
						eWriter.WriteLine("    <tr><td class=\"summarytag\">" + (int)(i) + "</td><td class=\"summarydata\">" + pname[menber.ID][i] + "</td></tr>");
						j++;

					}
					if (j == 18) {
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

		/*-----------------------------------------------------------EraData.html-------------------------------------------------------------*/

		private void EraData(string csvpath)
		{
			string filepath = csvpath + "EraData.html";
			EraStreamWriter eWriter = new EraStreamWriter(mf);
			eWriter.MakeFile(filepath);
			int i, j,line;
			i = 0;
			j = 0;

			eWriter.WriteLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01//EN\">");
			eWriter.WriteLine("<html xmlns:v=\"urn:schemas-microsoft-com:vml\"");
			eWriter.WriteLine("xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
			eWriter.WriteLine("xmlns:w=\"urn:schemas-microsoft-com:office:word\"");
			eWriter.WriteLine("xmlns=\"http://www.w3.org/TR/REC-html40\">");
			eWriter.WriteLine("<head>");
			eWriter.WriteLine("<meta http-equiv=Content-Type content=\"text/html; charset=shift_jis\">");
			eWriter.WriteLine("<meta name=ProgId content=Word.Document>");
			eWriter.WriteLine("<meta name=Generator content=\"Microsoft Word 10\">");
			eWriter.WriteLine("<meta name=Originator content=\"Microsoft Word 10\">");
			eWriter.WriteLine("<link rel=File-List href=\"EraData.files/filelist.xml\">");
			eWriter.WriteLine("<title>eraIm@sP</title>");
			eWriter.WriteLine("<!--[if gte mso 9]><xml>");
			eWriter.WriteLine(" <o:OfficeDocumentSettings>");
			eWriter.WriteLine("  <o:RemovePersonalInformation/>");
			eWriter.WriteLine(" </o:OfficeDocumentSettings>");
			eWriter.WriteLine("</xml><![endif]--><!--[if gte mso 9]><xml>");
			eWriter.WriteLine(" <w:WordDocument>");
			eWriter.WriteLine("  <w:Compatibility>");
			eWriter.WriteLine("   <w:UseFELayout/>");
			eWriter.WriteLine("  </w:Compatibility>");
			eWriter.WriteLine("  <w:BrowserLevel>MicrosoftInternetExplorer4</w:BrowserLevel>");
			eWriter.WriteLine(" </w:WordDocument>");
			eWriter.WriteLine("</xml><![endif]-->");
			eWriter.WriteLine("<style>");
			eWriter.WriteLine("<!--");
			eWriter.WriteLine("a:hover {");
			eWriter.WriteLine("  text-decoration: underline;");
			eWriter.WriteLine("}");
			eWriter.WriteLine("table");
			eWriter.WriteLine("	{border-spacing: 4px;}");
			eWriter.WriteLine("table.GAMEBASE");
			eWriter.WriteLine("	{border-spacing: 16px 4px;}");
			eWriter.WriteLine("table.CHAR\\_ABILITY");
			eWriter.WriteLine("	{min-width: 190px;}");
			eWriter.WriteLine("div.DATASUMMARY");
			eWriter.WriteLine("	{float:left;}");
			eWriter.WriteLine("table.DATASUMMARY");
			eWriter.WriteLine("	{overflow:hidden;}");
			eWriter.WriteLine("th.SUMMARYTAG");
			eWriter.WriteLine("	{background-color: #black;}");
			eWriter.WriteLine("td.SUMMARYTAG");
			eWriter.WriteLine("	{background-color: #black;}");
			eWriter.WriteLine(" /* Font Definitions */");
			eWriter.WriteLine(" @font-face");
			eWriter.WriteLine("	{font-family:Helvetica;");
			eWriter.WriteLine("	panose-1:2 11 5 4 2 2 2 2 2 4;");
			eWriter.WriteLine("	mso-font-charset:0;");
			eWriter.WriteLine("	mso-generic-font-family:swiss;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:3 0 0 0 1 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:Courier;");
			eWriter.WriteLine("	panose-1:2 7 4 9 2 2 5 2 4 4;");
			eWriter.WriteLine("	mso-font-charset:0;");
			eWriter.WriteLine("	mso-generic-font-family:modern;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:fixed;");
			eWriter.WriteLine("	mso-font-signature:3 0 0 0 1 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:\"Tms Rmn\";");
			eWriter.WriteLine("	panose-1:2 2 6 3 4 5 5 2 3 4;");
			eWriter.WriteLine("	mso-font-charset:0;");
			eWriter.WriteLine("	mso-generic-font-family:roman;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:3 0 0 0 1 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:Helv;");
			eWriter.WriteLine("	panose-1:2 11 6 4 2 2 2 3 2 4;");
			eWriter.WriteLine("	mso-font-charset:0;");
			eWriter.WriteLine("	mso-generic-font-family:swiss;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:3 0 0 0 1 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:\"New York\";");
			eWriter.WriteLine("	panose-1:2 4 5 3 6 5 6 2 3 4;");
			eWriter.WriteLine("	mso-font-charset:0;");
			eWriter.WriteLine("	mso-generic-font-family:roman;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:3 0 0 0 1 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:System;");
			eWriter.WriteLine("	panose-1:0 0 0 0 0 0 0 0 0 0;");
			eWriter.WriteLine("	mso-font-charset:128;");
			eWriter.WriteLine("	mso-generic-font-family:auto;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:auto;");
			eWriter.WriteLine("	mso-font-signature:1 134676480 16 0 131072 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:Wingdings;");
			eWriter.WriteLine("	panose-1:5 0 0 0 0 0 0 0 0 0;");
			eWriter.WriteLine("	mso-font-charset:2;");
			eWriter.WriteLine("	mso-generic-font-family:auto;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:0 268435456 0 0 -2147483648 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:\"ＭＳ 明朝\";");
			eWriter.WriteLine("	panose-1:2 2 6 9 4 2 5 8 3 4;");
			eWriter.WriteLine("	mso-font-alt:\"MS Mincho\";");
			eWriter.WriteLine("	mso-font-charset:128;");
			eWriter.WriteLine("	mso-generic-font-family:roman;");
			eWriter.WriteLine("	mso-font-pitch:fixed;");
			eWriter.WriteLine("	mso-font-signature:-1610612033 1757936891 16 0 131231 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:Batang;");
			eWriter.WriteLine("	panose-1:2 3 6 0 0 1 1 1 1 1;");
			eWriter.WriteLine("	mso-font-alt:\\BC14\\D0D5;");
			eWriter.WriteLine("	mso-font-charset:129;");
			eWriter.WriteLine("	mso-generic-font-family:auto;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:fixed;");
			eWriter.WriteLine("	mso-font-signature:1 151388160 16 0 524288 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:SimSun;");
			eWriter.WriteLine("	panose-1:2 1 6 0 3 1 1 1 1 1;");
			eWriter.WriteLine("	mso-font-alt:宋体;");
			eWriter.WriteLine("	mso-font-charset:134;");
			eWriter.WriteLine("	mso-generic-font-family:auto;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:1 135135232 16 0 262144 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:PMingLiU;");
			eWriter.WriteLine("	panose-1:2 1 6 1 0 1 1 1 1 1;");
			eWriter.WriteLine("	mso-font-alt:新細明體;");
			eWriter.WriteLine("	mso-font-charset:136;");
			eWriter.WriteLine("	mso-generic-font-family:auto;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:1 134742016 16 0 1048576 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:\"ＭＳ ゴシック\";");
			eWriter.WriteLine("	panose-1:2 11 6 9 7 2 5 8 2 4;");
			eWriter.WriteLine("	mso-font-alt:\"MS Gothic\";");
			eWriter.WriteLine("	mso-font-charset:128;");
			eWriter.WriteLine("	mso-generic-font-family:modern;");
			eWriter.WriteLine("	mso-font-pitch:fixed;");
			eWriter.WriteLine("	mso-font-signature:-1610612033 1757936891 16 0 131231 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:Dotum;");
			eWriter.WriteLine("	panose-1:2 11 6 0 0 1 1 1 1 1;");
			eWriter.WriteLine("	mso-font-alt:\\B3CB\\C6C0;");
			eWriter.WriteLine("	mso-font-charset:129;");
			eWriter.WriteLine("	mso-generic-font-family:modern;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:fixed;");
			eWriter.WriteLine("	mso-font-signature:1 151388160 16 0 524288 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:SimHei;");
			eWriter.WriteLine("	panose-1:2 1 6 0 3 1 1 1 1 1;");
			eWriter.WriteLine("	mso-font-alt:黑体;");
			eWriter.WriteLine("	mso-font-charset:134;");
			eWriter.WriteLine("	mso-generic-font-family:modern;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:fixed;");
			eWriter.WriteLine("	mso-font-signature:1 135135232 16 0 262144 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:MingLiU;");
			eWriter.WriteLine("	panose-1:2 1 6 9 0 1 1 1 1 1;");
			eWriter.WriteLine("	mso-font-alt:細明體;");
			eWriter.WriteLine("	mso-font-charset:136;");
			eWriter.WriteLine("	mso-generic-font-family:modern;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:fixed;");
			eWriter.WriteLine("	mso-font-signature:1 134742016 16 0 1048576 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:Mincho;");
			eWriter.WriteLine("	panose-1:2 2 6 9 4 3 5 8 3 5;");
			eWriter.WriteLine("	mso-font-alt:明朝;");
			eWriter.WriteLine("	mso-font-charset:128;");
			eWriter.WriteLine("	mso-generic-font-family:roman;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:fixed;");
			eWriter.WriteLine("	mso-font-signature:1 134676480 16 0 131072 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:Gulim;");
			eWriter.WriteLine("	panose-1:2 11 6 0 0 1 1 1 1 1;");
			eWriter.WriteLine("	mso-font-alt:\\AD74\\B9BC;");
			eWriter.WriteLine("	mso-font-charset:129;");
			eWriter.WriteLine("	mso-generic-font-family:roman;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:fixed;");
			eWriter.WriteLine("	mso-font-signature:1 151388160 16 0 524288 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:Century;");
			eWriter.WriteLine("	panose-1:2 4 6 4 5 5 5 2 3 4;");
			eWriter.WriteLine("	mso-font-charset:0;");
			eWriter.WriteLine("	mso-generic-font-family:roman;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:647 0 0 0 159 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:\"Angsana New\";");
			eWriter.WriteLine("	panose-1:2 2 6 3 5 4 5 2 3 4;");
			eWriter.WriteLine("	mso-font-charset:222;");
			eWriter.WriteLine("	mso-generic-font-family:roman;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:16777217 0 0 0 65536 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:\"Cordia New\";");
			eWriter.WriteLine("	panose-1:2 11 3 4 2 2 2 2 2 4;");
			eWriter.WriteLine("	mso-font-charset:222;");
			eWriter.WriteLine("	mso-generic-font-family:roman;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:16777217 0 0 0 65536 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:Mangal;");
			eWriter.WriteLine("	panose-1:0 0 4 0 0 0 0 0 0 0;");
			eWriter.WriteLine("	mso-font-charset:1;");
			eWriter.WriteLine("	mso-generic-font-family:roman;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:32768 0 0 0 0 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:Latha;");
			eWriter.WriteLine("	panose-1:0 0 4 0 0 0 0 0 0 0;");
			eWriter.WriteLine("	mso-font-charset:1;");
			eWriter.WriteLine("	mso-generic-font-family:roman;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:1048576 0 0 0 0 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:Sylfaen;");
			eWriter.WriteLine("	panose-1:1 10 5 2 5 3 6 3 3 3;");
			eWriter.WriteLine("	mso-font-charset:0;");
			eWriter.WriteLine("	mso-generic-font-family:roman;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:16778883 0 512 0 13 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:Vrinda;");
			eWriter.WriteLine("	panose-1:0 0 4 0 0 0 0 0 0 0;");
			eWriter.WriteLine("	mso-font-charset:1;");
			eWriter.WriteLine("	mso-generic-font-family:roman;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:0 0 0 0 0 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:Raavi;");
			eWriter.WriteLine("	panose-1:0 0 4 0 0 0 0 0 0 0;");
			eWriter.WriteLine("	mso-font-charset:1;");
			eWriter.WriteLine("	mso-generic-font-family:roman;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:0 0 0 0 0 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:Shruti;");
			eWriter.WriteLine("	panose-1:0 0 4 0 0 0 0 0 0 0;");
			eWriter.WriteLine("	mso-font-charset:1;");
			eWriter.WriteLine("	mso-generic-font-family:roman;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:0 0 0 0 0 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:Sendnya;");
			eWriter.WriteLine("	panose-1:0 0 4 0 0 0 0 0 0 0;");
			eWriter.WriteLine("	mso-font-charset:1;");
			eWriter.WriteLine("	mso-generic-font-family:roman;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:0 0 0 0 0 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:Gautami;");
			eWriter.WriteLine("	panose-1:0 0 4 0 0 0 0 0 0 0;");
			eWriter.WriteLine("	mso-font-charset:1;");
			eWriter.WriteLine("	mso-generic-font-family:roman;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:0 0 0 0 0 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:Tunga;");
			eWriter.WriteLine("	panose-1:0 0 4 0 0 0 0 0 0 0;");
			eWriter.WriteLine("	mso-font-charset:1;");
			eWriter.WriteLine("	mso-generic-font-family:roman;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:0 0 0 0 0 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:\"Estrangella Edessa\";");
			eWriter.WriteLine("	panose-1:0 0 0 0 0 0 0 0 0 0;");
			eWriter.WriteLine("	mso-font-charset:1;");
			eWriter.WriteLine("	mso-generic-font-family:roman;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:0 0 0 0 0 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:\"Arial Unicode MS\";");
			eWriter.WriteLine("	panose-1:0 0 0 0 0 0 0 0 0 0;");
			eWriter.WriteLine("	mso-font-charset:0;");
			eWriter.WriteLine("	mso-generic-font-family:roman;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:3 0 0 0 1 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:\"MS UI Gothic\";");
			eWriter.WriteLine("	panose-1:0 0 0 0 0 0 0 0 0 0;");
			eWriter.WriteLine("	mso-font-charset:128;");
			eWriter.WriteLine("	mso-generic-font-family:modern;");
			eWriter.WriteLine("	mso-font-format:other;");
			eWriter.WriteLine("	mso-font-pitch:auto;");
			eWriter.WriteLine("	mso-font-signature:1 134676480 16 0 131072 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:\"ＭＳ Ｐゴシック\";");
			eWriter.WriteLine("	panose-1:2 11 6 0 7 2 5 8 2 4;");
			eWriter.WriteLine("	mso-font-charset:128;");
			eWriter.WriteLine("	mso-generic-font-family:modern;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:-1610612033 1757936891 16 0 131231 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:\"Trebuchet MS\";");
			eWriter.WriteLine("	panose-1:2 11 6 3 2 2 2 2 2 4;");
			eWriter.WriteLine("	mso-font-charset:0;");
			eWriter.WriteLine("	mso-generic-font-family:swiss;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:647 0 0 0 159 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:\"\\@ＭＳ Ｐゴシック\";");
			eWriter.WriteLine("	panose-1:2 11 6 0 7 2 5 8 2 4;");
			eWriter.WriteLine("	mso-font-charset:128;");
			eWriter.WriteLine("	mso-generic-font-family:modern;");
			eWriter.WriteLine("	mso-font-pitch:variable;");
			eWriter.WriteLine("	mso-font-signature:-1610612033 1757936891 16 0 131231 0;}");
			eWriter.WriteLine("@font-face");
			eWriter.WriteLine("	{font-family:\"\\@ＭＳ 明朝\";");
			eWriter.WriteLine("	panose-1:2 2 6 9 4 2 5 8 3 4;");
			eWriter.WriteLine("	mso-font-charset:128;");
			eWriter.WriteLine("	mso-generic-font-family:roman;");
			eWriter.WriteLine("	mso-font-pitch:fixed;");
			eWriter.WriteLine("	mso-font-signature:-1610612033 1757936891 16 0 131231 0;}");
			eWriter.WriteLine(" /* Style Definitions */");
			eWriter.WriteLine(" p.MsoNormal, li.MsoNormal, div.MsoNormal");
			eWriter.WriteLine("	{mso-style-parent:\"\";");
			eWriter.WriteLine("	margin:0mm;");
			eWriter.WriteLine("	margin-bottom:.0001pt;");
			eWriter.WriteLine("	mso-pagination:widow-orphan;");
			eWriter.WriteLine("	font-size:12.0pt;");
			eWriter.WriteLine("	font-family:\"ＭＳ Ｐゴシック\";");
			eWriter.WriteLine("	mso-bidi-font-family:\"ＭＳ Ｐゴシック\";}");
			eWriter.WriteLine("h1");
			eWriter.WriteLine("	{margin-top:3.0pt;");
			eWriter.WriteLine("	margin-right:0mm;");
			eWriter.WriteLine("	margin-bottom:9.0pt;");
			eWriter.WriteLine("	margin-left:0mm;");
			eWriter.WriteLine("	mso-pagination:widow-orphan;");
			eWriter.WriteLine("	mso-outline-level:1;");
			eWriter.WriteLine("	background:black;");
			eWriter.WriteLine("	font-size:24.0pt;");
			eWriter.WriteLine("	font-family:\"ＭＳ Ｐゴシック\";");
			eWriter.WriteLine("	mso-bidi-font-family:\"ＭＳ Ｐゴシック\";");
			eWriter.WriteLine("	color:#EE0000;}");
			eWriter.WriteLine("h2");
			eWriter.WriteLine("	{margin-top:3.0pt;");
			eWriter.WriteLine("	margin-right:0mm;");
			eWriter.WriteLine("	margin-bottom:9.0pt;");
			eWriter.WriteLine("	margin-left:0mm;");
			eWriter.WriteLine("	mso-pagination:widow-orphan;");
			eWriter.WriteLine("	mso-outline-level:2;");
			eWriter.WriteLine("	background:black;");
			eWriter.WriteLine("	font-size:18.0pt;");
			eWriter.WriteLine("	font-family:\"ＭＳ Ｐゴシック\";");
			eWriter.WriteLine("	mso-bidi-font-family:\"ＭＳ Ｐゴシック\";");
			eWriter.WriteLine("	color:#EE0000;}");
			eWriter.WriteLine("h3");
			eWriter.WriteLine("	{margin:0mm;");
			eWriter.WriteLine("	margin-bottom:.0001pt;");
			eWriter.WriteLine("	line-height:16.8pt;");
			eWriter.WriteLine("	mso-pagination:widow-orphan;");
			eWriter.WriteLine("	mso-outline-level:3;");
			eWriter.WriteLine("	background:black;");
			eWriter.WriteLine("	font-size:14.0pt;");
			eWriter.WriteLine("	font-family:\"ＭＳ Ｐゴシック\";");
			eWriter.WriteLine("	mso-bidi-font-family:\"ＭＳ Ｐゴシック\";");
			eWriter.WriteLine("	color:#EE0000;}");
			eWriter.WriteLine("h4");
			eWriter.WriteLine("	{margin:0mm;");
			eWriter.WriteLine("	margin-bottom:.0001pt;");
			eWriter.WriteLine("	line-height:16.8pt;");
			eWriter.WriteLine("	mso-pagination:widow-orphan;");
			eWriter.WriteLine("	mso-outline-level:4;");
			eWriter.WriteLine("	background:black;");
			eWriter.WriteLine("	font-size:12.0pt;");
			eWriter.WriteLine("	font-family:\"ＭＳ Ｐゴシック\";");
			eWriter.WriteLine("	mso-bidi-font-family:\"ＭＳ Ｐゴシック\";");
			eWriter.WriteLine("	color:#DD0000;}");
			eWriter.WriteLine("a:link, span.MsoHyperlink");
			eWriter.WriteLine("	{color:blue;");
			eWriter.WriteLine("	mso-text-animation:none;");
			eWriter.WriteLine("	text-decoration:none;");
			eWriter.WriteLine("	text-underline:none;");
			eWriter.WriteLine("	text-decoration:none;");
			eWriter.WriteLine("	text-line-through:none;}");
			eWriter.WriteLine("a:visited, span.MsoHyperlinkFollowed");
			eWriter.WriteLine("	{color:blue;");
			eWriter.WriteLine("	mso-text-animation:none;");
			eWriter.WriteLine("	text-decoration:none;");
			eWriter.WriteLine("	text-underline:none;");
			eWriter.WriteLine("	text-decoration:none;");
			eWriter.WriteLine("	text-line-through:none;}");
			eWriter.WriteLine("p.base, li.base, div.base");
			eWriter.WriteLine("	{mso-style-name:base;");
			eWriter.WriteLine("	margin:0mm;");
			eWriter.WriteLine("	margin-bottom:.0001pt;");
			eWriter.WriteLine("	mso-pagination:widow-orphan;");
			eWriter.WriteLine("	background:#333333;");
			eWriter.WriteLine("	font-size:12.0pt;");
			eWriter.WriteLine("	font-family:\"ＭＳ Ｐゴシック\";");
			eWriter.WriteLine("	mso-bidi-font-family:\"ＭＳ Ｐゴシック\";}");
			eWriter.WriteLine("p.menubase, li.menubase, div.menubase");
			eWriter.WriteLine("	{mso-style-name:menubase;");
			eWriter.WriteLine("	margin:0mm;");
			eWriter.WriteLine("	margin-bottom:.0001pt;");
			eWriter.WriteLine("	mso-pagination:widow-orphan;");
			eWriter.WriteLine("	font-size:12.0pt;");
			eWriter.WriteLine("	font-family:\"ＭＳ Ｐゴシック\";");
			eWriter.WriteLine("	mso-bidi-font-family:\"ＭＳ Ｐゴシック\";}");
			eWriter.WriteLine("p.charlist, li.charlist, div.charlist");
			eWriter.WriteLine("	{mso-style-name:charlist;");
			eWriter.WriteLine("	mso-margin-top-alt:auto;");
			eWriter.WriteLine("	margin-right:0mm;");
			eWriter.WriteLine("	mso-margin-bottom-alt:auto;");
			eWriter.WriteLine("	margin-left:0mm;");
			eWriter.WriteLine("	mso-pagination:widow-orphan;");
			eWriter.WriteLine("	font-size:12.0pt;");
			eWriter.WriteLine("	font-family:\"ＭＳ Ｐゴシック\";");
			eWriter.WriteLine("	mso-bidi-font-family:\"ＭＳ Ｐゴシック\";}");
			eWriter.WriteLine("span.life");
			eWriter.WriteLine("	{mso-style-name:life;");
			eWriter.WriteLine("	color:#EE0000;");
			eWriter.WriteLine("	font-weight:bold;}");
			eWriter.WriteLine("span.mind");
			eWriter.WriteLine("	{mso-style-name:mind;");
			eWriter.WriteLine("	color:#2222FF;");
			eWriter.WriteLine("	font-weight:bold;}");
			eWriter.WriteLine("span.sei");
			eWriter.WriteLine("	{mso-style-name:sei;");
			eWriter.WriteLine("	color:#EEEEEE;");
			eWriter.WriteLine("	font-weight:bold;}");
			eWriter.WriteLine("span.msoins0");
			eWriter.WriteLine("	{mso-style-name:msoins0;");
			eWriter.WriteLine("	text-decoration:underline;");
			eWriter.WriteLine("	text-underline:single;}");
			eWriter.WriteLine("span.msodel0");
			eWriter.WriteLine("	{mso-style-name:msodel0;");
			eWriter.WriteLine("	color:red;");
			eWriter.WriteLine("	text-decoration:line-through;}");
			eWriter.WriteLine("span.msoins1");
			eWriter.WriteLine("	{mso-style-name:msoins1;");
			eWriter.WriteLine("	text-decoration:underline;");
			eWriter.WriteLine("	text-underline:single;}");
			eWriter.WriteLine("span.msodel1");
			eWriter.WriteLine("	{mso-style-name:msodel1;");
			eWriter.WriteLine("	color:red;");
			eWriter.WriteLine("	text-decoration:line-through;}");
			eWriter.WriteLine("span.msoins2");
			eWriter.WriteLine("	{mso-style-name:msoins2;");
			eWriter.WriteLine("	text-decoration:underline;");
			eWriter.WriteLine("	text-underline:single;}");
			eWriter.WriteLine("span.msodel2");
			eWriter.WriteLine("	{mso-style-name:msodel2;");
			eWriter.WriteLine("	color:red;");
			eWriter.WriteLine("	text-decoration:line-through;}");
			eWriter.WriteLine("span.msoins3");
			eWriter.WriteLine("	{mso-style-name:msoins3;");
			eWriter.WriteLine("	text-decoration:underline;");
			eWriter.WriteLine("	text-underline:single;}");
			eWriter.WriteLine("span.msodel3");
			eWriter.WriteLine("	{mso-style-name:msodel3;");
			eWriter.WriteLine("	color:red;");
			eWriter.WriteLine("	text-decoration:line-through;}");
			eWriter.WriteLine("span.msoins4");
			eWriter.WriteLine("	{mso-style-name:msoins;");
			eWriter.WriteLine("	text-decoration:underline;");
			eWriter.WriteLine("	text-underline:single;}");
			eWriter.WriteLine("span.msodel4");
			eWriter.WriteLine("	{mso-style-name:msodel;");
			eWriter.WriteLine("	color:red;");
			eWriter.WriteLine("	text-decoration:line-through;}");
			eWriter.WriteLine("ins");
			eWriter.WriteLine("	{mso-style-type:export-only;");
			eWriter.WriteLine("	text-decoration:none;}");
			eWriter.WriteLine("span.msoIns");
			eWriter.WriteLine("	{mso-style-type:export-only;");
			eWriter.WriteLine("	mso-style-name:\"\";");
			eWriter.WriteLine("	text-decoration:underline;");
			eWriter.WriteLine("	text-underline:single;}");
			eWriter.WriteLine("span.msoDel");
			eWriter.WriteLine("	{mso-style-type:export-only;");
			eWriter.WriteLine("	mso-style-name:\"\";");
			eWriter.WriteLine("	text-decoration:line-through;");
			eWriter.WriteLine("	color:red;}");
			eWriter.WriteLine("@page Section1");
			eWriter.WriteLine("	{size:595.3pt 841.9pt;");
			eWriter.WriteLine("	margin:99.25pt 30.0mm 30.0mm 30.0mm;");
			eWriter.WriteLine("	mso-header-margin:42.55pt;");
			eWriter.WriteLine("	mso-footer-margin:49.6pt;");
			eWriter.WriteLine("	mso-paper-source:0;}");
			eWriter.WriteLine("div.Section1");
			eWriter.WriteLine("	{page:Section1;}");
			eWriter.WriteLine("-->");
			eWriter.WriteLine("</style>");
			eWriter.WriteLine("<!--[if gte mso 10]>");
			eWriter.WriteLine("<style>");
			eWriter.WriteLine(" /* Style Definitions */");
			eWriter.WriteLine(" table.MsoNormalTable");
			eWriter.WriteLine("	{mso-style-name:標準の表;");
			eWriter.WriteLine("	mso-tstyle-rowband-size:0;");
			eWriter.WriteLine("	mso-tstyle-colband-size:0;");
			eWriter.WriteLine("	mso-style-noshow:yes;");
			eWriter.WriteLine("	mso-style-parent:\"\";");
			eWriter.WriteLine("	mso-padding-alt:0mm 5.4pt 0mm 5.4pt;");
			eWriter.WriteLine("	mso-para-margin:0mm;");
			eWriter.WriteLine("	mso-para-margin-bottom:.0001pt;");
			eWriter.WriteLine("	mso-pagination:widow-orphan;");
			eWriter.WriteLine("	font-size:10.0pt;");
			eWriter.WriteLine("	font-family:\"Times New Roman\";");
			eWriter.WriteLine("	mso-fareast-font-family:\"Times New Roman\";}");
			eWriter.WriteLine("</style>");
			eWriter.WriteLine("<![endif]--><!--[if gte mso 9]><xml>");
			eWriter.WriteLine(" <o:shapedefaults v:ext=\"edit\" spidmax=\"6146\">");
			eWriter.WriteLine("  <v:textbox inset=\"5.85pt,.7pt,5.85pt,.7pt\"/>");
			eWriter.WriteLine(" </o:shapedefaults></xml><![endif]--><!--[if gte mso 9]><xml>");
			eWriter.WriteLine(" <o:shapelayout v:ext=\"edit\">");
			eWriter.WriteLine("  <o:idmap v:ext=\"edit\" data=\"1\"/>");
			eWriter.WriteLine(" </o:shapelayout></xml><![endif]-->");
			eWriter.WriteLine("</head>");

			/*						EraData.html</style>					*/

			/*						EraData.html　ゲーム基本情報			*/

			eWriter.WriteLine("<body bgcolor=black lang=JA link=blue vlink=blue style='tab-interval:42.0pt'>");
			eWriter.WriteLine("<div class=Section1>");
			eWriter.WriteLine("<div>");
			eWriter.WriteLine("<div style='border-top:solid #990000 1.5pt;border-left:none;border-bottom:solid #990000 1.5pt;");
			eWriter.WriteLine("border-right:none;padding:3.0pt 0mm 3.0pt 0mm'>");
			eWriter.WriteLine("<h1><span lang=EN-US style='font-family:\"Trebuchet MS\"'>eraIm@sP</span></h1>");
			eWriter.WriteLine("</div>");
			eWriter.WriteLine("<div style='border-top:solid #990000 1.5pt;border-left:none;border-bottom:solid #990000 1.5pt;");
			eWriter.WriteLine("border-right:none;padding:2.0pt 0mm 2.0pt 0mm'>");
			eWriter.WriteLine("<h2><a name=gamebase>ゲーム基本情報</a></h2>");
			eWriter.WriteLine("</div>");
			eWriter.WriteLine("<table class=MsoNormalTable border=0 cellpadding=0 style='mso-cellspacing:1.5pt;");
			eWriter.WriteLine(" margin-left:52.5pt;mso-padding-alt:0mm 0mm 0mm 0mm'>");

			for (i = 0; i < 5; i++)
			{
				string temp = "";
				string temp2 = "";
				int count = 0;

				switch (i)
				{
					case 0:
						temp = "タイトル";
						temp2 = gb.title;
						break;
					case 1:
						temp = "作者";
						temp2 = gb.author;
						break;
					case 2:
						temp = "コード";
						temp2 = gb.code;
						break;
					case 3:
						temp = "バージョン";
						temp2 = gb.version;
						break;
					case 4:
						temp = "追加情報";
						temp2 = gb.apendinfo;
						break;

				}
				eWriter.WriteLine(" <tr style='mso-yfti-irow:" + i + "'>");
				eWriter.WriteLine("  <td nowrap style='padding:0mm 0mm 0mm 0mm'>");
				eWriter.WriteLine("  <p class=MsoNormal align=right style='margin-bottom:24.0pt;text-align:right;");
				eWriter.WriteLine("  line-height:14.4pt'><b><span style='color:#CCCCCC'>" + temp + "</span></b></p>");
				eWriter.WriteLine("  </td>");
				eWriter.WriteLine("  <td style='padding:0mm 12.0pt 0mm 0mm'>");
				eWriter.WriteLine("  <p class=MsoNormal style='margin-bottom:24.0pt;line-height:14.4pt'><span");
				eWriter.WriteLine("  lang=EN-US style='font-family:\"Trebuchet MS\";color:#CCCCCC'>" + temp2 + "</span></p>");
				eWriter.WriteLine("  </td>");
				eWriter.WriteLine(" </tr>");

			}
			/*-----------------------------------------------------------------*/

			eWriter.WriteLine("<div style='border-top:solid #990000 1.5pt;border-left:none;border-bottom:solid #990000 1.5pt;");
			eWriter.WriteLine("border-right:none;padding:2.0pt 0mm 2.0pt 0mm'>");
			eWriter.WriteLine("<h2><a name=\"char_list\">キャラ情報</a></h2>");
			eWriter.WriteLine("</div>");

			/*-----------------------------------------------------------------*/

			/*							キャラ情報								*/
			for (i = 0; i < chara.Length; i++)
			{
				//参照先がない
				if (chara[i].NAME != null && chara[i].NAME != "" && chara[i].BASE != null)
					continue;

				//基本情報
				eWriter.WriteLine("<p class=charlist style='margin:0mm;margin-bottom:.0001pt;line-height:14.4pt;");
				eWriter.WriteLine("background:#333333'><a name=c0001><span lang=EN-US style='font-family:\"Trebuchet MS\";");
				eWriter.WriteLine("color:#CCCCCC;display:none;mso-hide:all'>&nbsp;</span></a></p>");

				eWriter.WriteLine("<table class=MsoNormalTable border=0 cellpadding=0 width=600 style='width:450.0pt;");
				eWriter.WriteLine(" mso-cellspacing:1.5pt;margin-left:52.5pt;background:#555555;mso-padding-alt:");
				eWriter.WriteLine(" 0mm 0mm 0mm 0mm'>");
				eWriter.WriteLine(" <tr style='mso-yfti-irow:0'>");
				eWriter.WriteLine("  <td colspan=4 style='padding:0mm 0mm 0mm 0mm'>");
				eWriter.WriteLine("  <div style='border-top:solid #990000 1.5pt;border-left:none;border-bottom:");
				eWriter.WriteLine("  solid #990000 1.5pt;border-right:none;padding:1.0pt 0mm 0mm 0mm'>");
				eWriter.WriteLine("  <h3><span lang=EN-US style='font-family:\"Trebuchet MS\"'>1. </span>天海春香<span");
				eWriter.WriteLine("  lang=EN-US style='font-family:\"Trebuchet MS\"'>(</span>春香<span lang=EN-US");
				eWriter.WriteLine("  style='font-family:\"Trebuchet MS\"'>)</span></h3>");
				eWriter.WriteLine("  </div>");
				eWriter.WriteLine("  </td>");
				eWriter.WriteLine(" </tr>");

				line = 1;

				eWriter.WriteLine(" <tr style='mso-yfti-irow:"+line+"'>");
				line++;
				eWriter.WriteLine("  <td colspan=4 style='padding:0mm 0mm 0mm 0mm'>");
				eWriter.WriteLine("  <h4>基本情報</h4>");
				eWriter.WriteLine("  </td>");
				eWriter.WriteLine(" </tr>");

				//------------------------BASE-------------------------------------

				int index, count,flag;
				count = 0;
				flag = 0;
				j = 0;
				eWriter.WriteLine(" <tr style='mso-yfti-irow:"+line+"'>");
				index = Array.IndexOf(pname[(int)CIntData.BASE], "体力");
				//テスト
				int test,temp1,temp2,temp3;
				string tests;
				bool tempb;
				CharacterTemplate ch;
				ch = chara[i];

				tests = chara[i].NAME;
				temp1 = chara[i].BASE.Length;
				tempb = chara[i].BASE.IsSynchronized;
				temp3 = chara[i].ABL[0];
				test = chara[i].BASE[1];
				test = chara[i].BASE[0];
				chara[i].BASE[j] = 1;
				test = chara[i].BASE[index];
				//
				if (index != -1 && chara[i].BASE[index] !=null && chara[i].BASE[index] != 0)
				{
					eWriter.WriteLine("  <td width=180 style='width:135.0pt;padding:0mm 0mm 0mm 0mm'>");
					eWriter.WriteLine("  <p class=MsoNormal style='line-height:14.4pt'><span class=life>体力<span");
					eWriter.WriteLine("  lang=EN-US>:</span></span><span lang=EN-US style='font-family:\"Trebuchet MS\";");
					eWriter.WriteLine("  color:#CCCCCC'> <span class=charparamdata><span style='font-family:\"Trebuchet MS\";");
					eWriter.WriteLine("  mso-bidi-font-family:\"ＭＳ Ｐゴシック\"'>"+chara[i].BASE[index]+"</span></span></span></p>");
					eWriter.WriteLine("  </td>");
					count++;
				}
				index = Array.IndexOf(pname[(int)CIntData.BASE], "気力");
				if (index != -1 && chara[i].BASE[index] != 0)
				{
					eWriter.WriteLine("  <td width=180 style='width:135.0pt;padding:0mm 0mm 0mm 0mm'>");
					eWriter.WriteLine("  <p class=MsoNormal style='line-height:14.4pt'><span class=mind>気力</span><span");
					eWriter.WriteLine("  class=mind><span lang=EN-US style='font-family:\"Trebuchet MS\"'>:</span></span><span");
					eWriter.WriteLine("  lang=EN-US style='font-family:\"Trebuchet MS\";color:#CCCCCC'> <span");
					eWriter.WriteLine("  class=charparamdata><span style='font-family:\"Trebuchet MS\";mso-bidi-font-family:");
					eWriter.WriteLine("  \"ＭＳ Ｐゴシック\"'>"+chara[i].BASE[index]+"</span></span></span></p>");
					eWriter.WriteLine("  </td>");
					count++;
				}

				for (j = 0; j < chara[i].BASE.Length; j++)
				{
					if (flag == 1) 
					{
						eWriter.WriteLine(" <tr style='mso-yfti-irow:"+line+"'>");
						line++;
						flag = 0;
					}
					if (chara[i].BASE[j] != 0 && chara[i].BASE[j] != null && pname[(int)CIntData.BASE][j] != "体力" && pname[(int)CIntData.BASE][j] != "気力")
					{
						eWriter.WriteLine("  <td width=180 style='width:135.0pt;padding:0mm 0mm 0mm 0mm'>");
						eWriter.WriteLine("  <p class=MsoNormal style='line-height:14.4pt'><span class=basepalam11><span");
						eWriter.WriteLine("  style='font-family:\"ＭＳ Ｐゴシック\";mso-bidi-font-family:\"ＭＳ Ｐゴシック\";color:#CCCCCC'>誕生月</span></span><span");
						eWriter.WriteLine("  class=basepalam11><span lang=EN-US style='font-family:\"ＭＳ Ｐゴシック\";mso-bidi-font-family:");
						eWriter.WriteLine("  \"ＭＳ Ｐゴシック\"'>:</span></span><span lang=EN-US style='font-family:\"Trebuchet MS\";");
						eWriter.WriteLine("  color:#CCCCCC'> <span class=charparamdata><span style='font-family:\"Trebuchet MS\";");
						eWriter.WriteLine("  mso-bidi-font-family:\"ＭＳ Ｐゴシック\"'>4</span></span></span></p>");
						eWriter.WriteLine("  </td>");
						count++;
						if (count == 3) 
						{
							eWriter.WriteLine("  <td style='padding:0mm 0mm 0mm 0mm'>");
							eWriter.WriteLine("  <p class=MsoNormal><span lang=EN-US style='font-size:10.0pt;font-family:Century'>&nbsp;</span></p>");
							eWriter.WriteLine("  </td>");
							eWriter.WriteLine(" </tr>");
							count = 0;
							flag = 1;
							//ここで改行すると最後が揃わないときがある
						}
					}
				}
				//for
				if (count != 0) 
				{
					eWriter.WriteLine("  <td style='padding:0mm 0mm 0mm 0mm'>");
					eWriter.WriteLine("  <p class=MsoNormal><span lang=EN-US style='font-size:10.0pt;font-family:Century'>&nbsp;</span></p>");
					eWriter.WriteLine("  </td>");
					eWriter.WriteLine(" </tr>");
				}
				count = 0;
				flag = 0;

			}




			eWriter.Close();
		}


		/*-----------------------------------------------------EraData2.html-------------------------------------------------------------------*/




		/*-----------------------------------------------------------WriteStyle-------------------------------------------------------------*/

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
		}

		/*-----------------------------------------------------------SetPID-------------------------------------------------------------*/

		//渡された変数にIDをセットする
		private void SetPID(out PMEMBER o, int x) 
		{
			o.ID = 998;
			o.name = "";
			switch(x){
				case (int)CIntData.PALAM:
					o.ID = x;
					o.name = "PALAM";
					break;
				case (int)CIntData.BASE:
					o.ID = x;
					o.name = "BASE";
					break;
				case (int)CIntData.ABL:
					o.ID = x;
					o.name = "ABL";
					break;

				case (int)CIntData.TALENT:
					o.ID = x;
					o.name = "TALENT";
					break;

				case (int)CIntData.MARK:
					o.ID = x;
					o.name = "MARK";
					break;
				case (int)CIntData.EXP:
					o.ID = x;
					o.name = "EXP";
					break;
					/*
				case (int)CIntData.RELATION:
					o.ID = x;
					o.name = "RELATION";
					break;
					 * */
					/*
				case (int)CIntData.CFLAG:
					o.ID = x;
					o.name = "CFLAG";
					break;
					 * */
					/*
				case (int)CIntData.EQUIP:
					o.ID = x;
					o.name = "EQUIP";
					break;
					 */
					/*
				case (int)CIntData.JUEL:
					o.ID = x;
					o.name = "JUEL";
					break;
					 * */
				case (int)CIntData.SOURCE:
					o.ID = x;
					o.name = "SOURCE";
					break;
				case (int)CIntData.ITEM:
					o.ID = x;
					o.name = "ITEM";
					break;
					/*
				case (int)CIntData.TRAIN:
					o.ID = x;
					o.name = "TRAIN";
					break;
					 * */

			}
		}

		/*-------------------------------------------------------PMEMBER-----------------------------------------------------------------*/

		//privateにできない？
		private struct PMEMBER
		{
			public int ID;
			public string name;
		}

	}
}
