using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utau.Eramakerview.GameData.Html
{
	class EraData : HtmlBase
	{
		public EraData(string csvpath, OutputData outdata, MainForm mf)
			: base(outdata, mf)
		{
			filepath = csvpath + "EraData.html";
		}

		static SortedDictionary<int, int> sdict;

		protected override void HtmlList()
		{
			int i, line;
			i = 0;

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
			eWriter.WriteLine("<title>" + output.gb.title + "</title>");
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
			eWriter.WriteLine("<h1><span lang=EN-US style='font-family:\"Trebuchet MS\"'>" + output.gb.title + "</span></h1>");
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

				switch (i)
				{
					case 0:
						temp = "タイトル";
						temp2 = output.gb.title;
						break;
					case 1:
						temp = "作者";
						temp2 = output.gb.author;
						break;
					case 2:
						temp = "コード";
						temp2 = output.gb.code;
						break;
					case 3:
						temp = "バージョン";
						temp2 = output.gb.version;
						break;
					case 4:
						temp = "追加情報";
						temp2 = output.gb.apendinfo;
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

			eWriter.WriteLine("</table>");
			eWriter.WriteLine("<div style='border-top:solid #990000 1.5pt;border-left:none;border-bottom:solid #990000 1.5pt;");
			eWriter.WriteLine("border-right:none;padding:2.0pt 0mm 2.0pt 0mm'>");
			eWriter.WriteLine("<h2><a name=\"char_list\">キャラ情報</a></h2>");
			eWriter.WriteLine("</div>");

			/*-----------------------------------------------------------------*/

			/*							キャラ情報								*/
			for (i = 0; i < output.chara.Count; i++)
			{
				//参照先がない
				if (output.chara[i].NAME == null || output.chara[i].NAME == "")
					continue;

				//基本情報
				eWriter.WriteLine("<p class=charlist style='margin:0mm;margin-bottom:.0001pt;line-height:14.4pt;");
				eWriter.WriteLine("background:#333333'><a name=c000" + output.chara[i].NO + "><span lang=EN-US style='font-family:\"Trebuchet MS\";");
				eWriter.WriteLine("color:#CCCCCC;display:none;mso-hide:all'>&nbsp;</span></a></p>");

				eWriter.WriteLine("<table class=MsoNormalTable border=0 cellpadding=0 width=600 style='width:450.0pt;");
				eWriter.WriteLine(" mso-cellspacing:1.5pt;margin-left:52.5pt;background:#555555;mso-padding-alt:");
				eWriter.WriteLine(" 0mm 0mm 0mm 0mm'>");
				eWriter.WriteLine(" <tr style='mso-yfti-irow:0'>");
				eWriter.WriteLine("  <td colspan=4 style='padding:0mm 0mm 0mm 0mm'>");
				eWriter.WriteLine("  <div style='border-top:solid #990000 1.5pt;border-left:none;border-bottom:");
				eWriter.WriteLine("  solid #990000 1.5pt;border-right:none;padding:1.0pt 0mm 0mm 0mm'>");
				eWriter.WriteLine("  <h3><span lang=EN-US style='font-family:\"Trebuchet MS\"'>" + output.chara[i].NO + ". </span>" + output.chara[i].NAME + "<span");
				eWriter.WriteLine("  lang=EN-US style='font-family:\"Trebuchet MS\"'>(</span>" + output.chara[i].CALLNAME + "<span lang=EN-US");
				eWriter.WriteLine("  style='font-family:\"Trebuchet MS\"'>)</span></h3>");
				eWriter.WriteLine("  </div>");
				eWriter.WriteLine("  </td>");
				eWriter.WriteLine(" </tr>");

				line = 1;

				eWriter.WriteLine(" <tr style='mso-yfti-irow:" + line + "'>");
				line++;
				eWriter.WriteLine("  <td colspan=4 style='padding:0mm 0mm 0mm 0mm'>");
				eWriter.WriteLine("  <h4>基本情報</h4>");
				eWriter.WriteLine("  </td>");
				eWriter.WriteLine(" </tr>");

				//------------------------BASE-------------------------------------

				int index, count, flag;
				count = 0;
				flag = 0;
				index = 0;
				eWriter.WriteLine(" <tr style='mso-yfti-irow:" + line + "'>");
				index = Array.IndexOf(output.pname[(int)CIntData.BASE], "体力");
				//テスト
				if (index != -1)
				{
					eWriter.WriteLine("  <td width=180 style='width:135.0pt;padding:0mm 0mm 0mm 0mm'>");
					eWriter.WriteLine("  <p class=MsoNormal style='line-height:14.4pt'><span class=life>体力<span");
					eWriter.WriteLine("  lang=EN-US>:</span></span><span lang=EN-US style='font-family:\"Trebuchet MS\";");
					eWriter.WriteLine("  color:#CCCCCC'> <span class=charparamdata><span style='font-family:\"Trebuchet MS\";");
					eWriter.WriteLine("  mso-bidi-font-family:\"ＭＳ Ｐゴシック\"'>" + output.chara[i].Base[index] + "</span></span></span></p>");
					eWriter.WriteLine("  </td>");
					count++;
				}

				index = Array.IndexOf(output.pname[(int)CIntData.BASE], "気力");

				// && output.chara[i].Base[index] != 0
				//string ss = output.chara[i].NAME;
				if (index != -1 && output.chara[i].Base[index] != 0)
				{
					eWriter.WriteLine("  <td width=180 style='width:135.0pt;padding:0mm 0mm 0mm 0mm'>");
					eWriter.WriteLine("  <p class=MsoNormal style='line-height:14.4pt'><span class=mind>気力</span><span");
					eWriter.WriteLine("  class=mind><span lang=EN-US style='font-family:\"Trebuchet MS\"'>:</span></span><span");
					eWriter.WriteLine("  lang=EN-US style='font-family:\"Trebuchet MS\";color:#CCCCCC'> <span");
					eWriter.WriteLine("  class=charparamdata><span style='font-family:\"Trebuchet MS\";mso-bidi-font-family:");
					eWriter.WriteLine("  \"ＭＳ Ｐゴシック\"'>" + output.chara[i].Base[index] + "</span></span></span></p>");
					eWriter.WriteLine("  </td>");
					count++;
				}

				sdict = new SortedDictionary<int, int>(output.chara[i].Base);

				foreach (KeyValuePair<int, int> pair in sdict)
				{
					if (output.pname[(int)CIntData.BASE][pair.Key] == "体力" || output.pname[(int)CIntData.BASE][pair.Key] == "気力")
						continue;

					if (flag == 1)
					{
						eWriter.WriteLine(" <tr style='mso-yfti-irow:" + line + "'>");
						line++;
						flag = 0;
					}
					if (pair.Value > 0)
					{
						eWriter.WriteLine("  <td width=180 style='width:135.0pt;padding:0mm 0mm 0mm 0mm'>");
						eWriter.WriteLine("  <p class=MsoNormal style='line-height:14.4pt'><span class=basepalam11><span");
						eWriter.WriteLine("  style='font-family:\"ＭＳ Ｐゴシック\";mso-bidi-font-family:\"ＭＳ Ｐゴシック\";color:#CCCCCC'>" + output.pname[(int)CIntData.BASE][pair.Key] + "</span></span><span");
						eWriter.WriteLine("  class=basepalam11><span lang=EN-US style='font-family:\"ＭＳ Ｐゴシック\";mso-bidi-font-family:");
						eWriter.WriteLine("  \"ＭＳ Ｐゴシック\"'>:</span></span><span lang=EN-US style='font-family:\"Trebuchet MS\";");
						eWriter.WriteLine("  color:#CCCCCC'> <span class=charparamdata><span style='font-family:\"Trebuchet MS\";");
						eWriter.WriteLine("  mso-bidi-font-family:\"ＭＳ Ｐゴシック\"'>" + pair.Value + "</span></span></span></p>");
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

				//------------------------TALENT-------------------------------------

				eWriter.WriteLine(" <tr style='mso-yfti-irow:" + line + "'>");
				line++;
				eWriter.WriteLine("  <td colspan=4 style='padding:0mm 0mm 0mm 0mm'>");
				eWriter.WriteLine("  <h4>素質</h4>");
				eWriter.WriteLine("  </td>");
				eWriter.WriteLine(" </tr>");

				eWriter.WriteLine(" <tr style='mso-yfti-irow:" + line + "'>");
				line++;//line:7
				eWriter.WriteLine("  <td colspan=3 style='padding:0mm 0mm 0mm 0mm'>");
				eWriter.WriteLine("  <div style='margin-left:31.5pt'>");

				count = 0;
				flag = 0;
				eWriter.WriteLine("  <p class=MsoNormal style='line-height:19.2pt'><span class=talent><b><span");

				sdict = new SortedDictionary<int, int>(output.chara[i].Talent);

				//素質表示
				foreach (KeyValuePair<int, int> pair in sdict)
				{
					if (pair.Value <= 0)
						continue;

					if (count >= 7 && (count + 1) % 8 == 0)
					{
						eWriter.WriteLine("  style='font-family:\"ＭＳ Ｐゴシック\";mso-bidi-font-family:\"ＭＳ Ｐゴシック\";color:#CCCCCC'>" + output.pname[(int)CIntData.TALENT][pair.Key] + "</span></b></span><b><span");
						eWriter.WriteLine("  style='font-family:\"Trebuchet MS\";color:#CCCCCC'> </span></b></p>");
						eWriter.WriteLine("  <p class=MsoNormal style='line-height:19.2pt'><span class=talent><b><span");

					}
					else
					{
						eWriter.WriteLine("  style='font-family:\"ＭＳ Ｐゴシック\";mso-bidi-font-family:\"ＭＳ Ｐゴシック\";color:#CCCCCC'>" + output.pname[(int)CIntData.TALENT][pair.Key] + "</span></b></span><b><span");
						eWriter.WriteLine("  style='font-family:\"Trebuchet MS\";color:#CCCCCC'> </span><span class=talent><span");
					}
					count++;
				}
				//if (flag == 1)
				//{
				eWriter.WriteLine("  style='font-family:\"Trebuchet MS\";color:#CCCCCC'> </span></b></p>");
				//}
				eWriter.WriteLine("  </div>");
				eWriter.WriteLine("  </td>");
				eWriter.WriteLine("  <td style='padding:0mm 0mm 0mm 0mm'>");
				eWriter.WriteLine("  <p class=MsoNormal><span lang=EN-US style='font-size:10.0pt;font-family:Century'>&nbsp;</span></p>");
				eWriter.WriteLine("  </td>");
				eWriter.WriteLine(" </tr>");

				//------------------------ABL-------------------------------------

				eWriter.WriteLine(" <tr style='mso-yfti-irow:8'>");
				eWriter.WriteLine("  <td colspan=4 style='padding:0mm 0mm 0mm 0mm'>");
				eWriter.WriteLine("  <h4>初期能力</h4>");
				eWriter.WriteLine("  </td>");
				eWriter.WriteLine(" </tr>");

				eWriter.WriteLine(" <tr style='mso-yfti-irow:9'>");
				eWriter.WriteLine("  <td colspan=3 style='padding:0mm 0mm 0mm 0mm'>");
				eWriter.WriteLine("  <table class=MsoNormalTable border=0 cellpadding=0 style='mso-cellspacing:");
				eWriter.WriteLine("   1.5pt;margin-left:31.5pt;mso-padding-alt:0mm 0mm 0mm 0mm'>");

				//変数宣言
				int line2 = 0;
				sdict = new SortedDictionary<int, int>(output.chara[i].Abl);

				//初期能力表示
				foreach (KeyValuePair<int, int> pair in sdict)
				{
					if (pair.Value > 0)
					{
						eWriter.WriteLine("   <tr style='mso-yfti-irow:" + line2 + "'>");
						eWriter.WriteLine("    <td width=124 style='width:93.0pt;padding:0mm 0mm 0mm 0mm'>");
						eWriter.WriteLine("    <p class=MsoNormal align=right style='text-align:right;line-height:14.4pt'><b><span");
						eWriter.WriteLine("    style='color:#CCCCCC'>" + output.pname[(int)CIntData.ABL][pair.Key] + "</span></b></p>");
						eWriter.WriteLine("    </td>");
						eWriter.WriteLine("    <td width=32 style='width:24.0pt;padding:0mm 0mm 0mm 0mm'>");
						eWriter.WriteLine("    <p class=MsoNormal align=right style='text-align:right;line-height:14.4pt'><b><span");
						eWriter.WriteLine("    lang=EN-US style='font-family:\"Trebuchet MS\";color:#CCCCCC'>" + pair.Value + "</span></b></p>");
						eWriter.WriteLine("    </td>");
						eWriter.WriteLine("   </tr>");

						line2++;
					}
				}

				eWriter.WriteLine("  </table>");
				eWriter.WriteLine("  <p class=MsoNormal><span lang=EN-US><o:p></o:p></span></p>");
				eWriter.WriteLine("  </td>");
				eWriter.WriteLine("  <td style='padding:0mm 0mm 0mm 0mm'>");
				eWriter.WriteLine("  <p class=MsoNormal><span lang=EN-US style='font-size:10.0pt;font-family:Century'>&nbsp;</span></p>");
				eWriter.WriteLine("  </td>");
				eWriter.WriteLine(" </tr>");

				//------------------------EXP-------------------------------------

				line2 = 0;

				eWriter.WriteLine(" <tr style='mso-yfti-irow:" + line + "'>");

				line++;

				eWriter.WriteLine("  <td colspan=4 style='padding:0mm 0mm 0mm 0mm'>");
				eWriter.WriteLine("  <h4>初期経験</h4>");
				eWriter.WriteLine("  </td>");
				eWriter.WriteLine(" </tr>");
				eWriter.WriteLine(" <tr style='mso-yfti-irow:" + line + "'>");

				line++;

				eWriter.WriteLine("  <td colspan=3 style='padding:0mm 0mm 0mm 0mm'>");
				eWriter.WriteLine("  <table class=MsoNormalTable border=0 cellpadding=0 style='mso-cellspacing:");
				eWriter.WriteLine("   1.5pt;margin-left:31.5pt;mso-padding-alt:0mm 0mm 0mm 0mm'>");

				line2 = 0;
				sdict = new SortedDictionary<int, int>(output.chara[i].Exp);

				//初期経験表示
				foreach (KeyValuePair<int, int> pair in sdict)
				{
					if (pair.Value > 0)
					{
						eWriter.WriteLine("   <tr style='mso-yfti-irow:" + line2 + "'>");
						eWriter.WriteLine("    <td width=124 style='width:93.0pt;padding:0mm 0mm 0mm 0mm'>");
						eWriter.WriteLine("    <p class=MsoNormal align=right style='text-align:right;line-height:14.4pt'><b><span");
						eWriter.WriteLine("    style='color:#CCCCCC'>" + output.pname[(int)CIntData.EXP][pair.Key] + "</span></b></p>");
						eWriter.WriteLine("    </td>");
						eWriter.WriteLine("    <td width=32 style='width:24.0pt;padding:0mm 0mm 0mm 0mm'>");
						eWriter.WriteLine("    <p class=MsoNormal align=right style='text-align:right;line-height:14.4pt'><b><span");
						eWriter.WriteLine("    lang=EN-US style='font-family:\"Trebuchet MS\";color:#CCCCCC'>" + pair.Value + "</span></b></p>");
						eWriter.WriteLine("    </td>");
						eWriter.WriteLine("   </tr>");
					}
				}

				eWriter.WriteLine("  </table>");
				eWriter.WriteLine("  <p class=MsoNormal><span lang=EN-US><o:p></o:p></span></p>");
				eWriter.WriteLine("  </td>");
				eWriter.WriteLine("  <td style='padding:0mm 0mm 0mm 0mm'>");
				eWriter.WriteLine("  <p class=MsoNormal><span lang=EN-US style='font-size:10.0pt;font-family:Century'>&nbsp;</span></p>");
				eWriter.WriteLine("  </td>");
				eWriter.WriteLine(" </tr>");

				//------------------------RELATION-------------------------------------

				eWriter.WriteLine(" <tr style='mso-yfti-irow:" + line + "'>");

				line++;

				eWriter.WriteLine("  <td colspan=4 style='padding:0mm 0mm 0mm 0mm'>");
				eWriter.WriteLine("  <h4>相性</h4>");
				eWriter.WriteLine("  </td>");
				eWriter.WriteLine(" </tr>");
				eWriter.WriteLine(" <tr style='mso-yfti-irow:" + line + "'>");

				line++;

				eWriter.WriteLine("  <td colspan=3 style='padding:0mm 0mm 0mm 0mm'>");
				eWriter.WriteLine("  <table class=MsoNormalTable border=0 cellpadding=0 style='mso-cellspacing:");
				eWriter.WriteLine("   1.5pt;margin-left:31.5pt;mso-padding-alt:0mm 0mm 0mm 0mm'>");

				line2 = 0;
				sdict = new SortedDictionary<int, int>(output.chara[i].Relation);

				//相性表示
				foreach (KeyValuePair<int, int> pair in sdict)
				{
					if (pair.Value > 0 && GetName(pair.Key) != "")
					{
						eWriter.WriteLine("   <tr style='mso-yfti-irow:" + line2 + "'>");

						line2++;

						string color;

						if (pair.Value > 100)
						{
							//color = 0xEEAAAA;
							color = "EEAAAA";
						}
						else
						{
							color = "9999EE";
						}

						eWriter.WriteLine("    <td width=124 style='width:93.0pt;padding:0mm 0mm 0mm 0mm'>");
						eWriter.WriteLine("    <p class=MsoNormal align=right style='text-align:right;line-height:14.4pt'><b><span");
						eWriter.WriteLine("    lang=EN-US style='font-family:\"Trebuchet MS\";color:#CCCCCC'><a href=\"#c0007\"><span");
						eWriter.WriteLine("    style='font-family:\"ＭＳ Ｐゴシック\";color:#" + color + "'>" + GetName(pair.Key) + "</span></a></span></b></p>");
						eWriter.WriteLine("    </td>");
						eWriter.WriteLine("    <td width=32 style='width:24.0pt;padding:0mm 0mm 0mm 0mm'>");
						eWriter.WriteLine("    <p class=MsoNormal align=right style='text-align:right;line-height:14.4pt'><b><span");
						eWriter.WriteLine("    lang=EN-US style='font-family:\"Trebuchet MS\";color:#CCCCCC'>" + pair.Value + "</span></b></p>");
						eWriter.WriteLine("    </td>");
						eWriter.WriteLine("   </tr>");
					}
				}

				eWriter.WriteLine("  </table>");
				eWriter.WriteLine("  <p class=MsoNormal><span lang=EN-US><o:p></o:p></span></p>");
				eWriter.WriteLine("  </td>");
				eWriter.WriteLine("  <td style='padding:0mm 0mm 0mm 0mm'>");
				eWriter.WriteLine("  <p class=MsoNormal><span lang=EN-US style='font-size:10.0pt;font-family:Century'>&nbsp;</span></p>");
				eWriter.WriteLine("  </td>");
				eWriter.WriteLine(" </tr>");

				//------------------------CFLAG-------------------------------------

				eWriter.WriteLine(" <tr style='mso-yfti-irow:" + line + "'>");

				line++;

				eWriter.WriteLine("  <td colspan=4 style='padding:0mm 0mm 0mm 0mm'>");
				eWriter.WriteLine("  <h4>フラグ</h4>");
				eWriter.WriteLine("  </td>");
				eWriter.WriteLine(" </tr>");

				eWriter.WriteLine(" <tr style='mso-yfti-irow:15;mso-yfti-lastrow:yes'>");
				eWriter.WriteLine("  <td colspan=3 style='padding:0mm 0mm 0mm 0mm'>");
				eWriter.WriteLine("  <table class=MsoNormalTable border=0 cellpadding=0 style='mso-cellspacing:");
				eWriter.WriteLine("   1.5pt;margin-left:31.5pt;mso-padding-alt:0mm 0mm 0mm 0mm'>");

				count = 0;
				line2 = 0;

				//SortedDictionary<int, int> sdict = new SortedDictionary<int, int>(output.chara[i].Cflag);
				sdict = new SortedDictionary<int, int>(output.chara[i].Cflag);


				//フラグ記述開始
				//foreach (KeyValuePair<int, int> pair in output.chara[i].Cflag)
				foreach (KeyValuePair<int, int> pair in sdict)
				{
					if ((pair.Value >= 0 || pair.Value < 0) && (count + 1) == output.chara[i].Cflag.Count)
					{
						eWriter.WriteLine("   <tr style='mso-yfti-irow:" + line2 + ";mso-yfti-lastrow:yes'>");
						line2++;
					}
					else
					{
						eWriter.WriteLine("   <tr style='mso-yfti-irow:" + line2 + "'>");
						line2++;
					}
					eWriter.WriteLine("    <td width=124 style='width:93.0pt;padding:0mm 0mm 0mm 0mm'>");
					eWriter.WriteLine("    <p class=MsoNormal align=right style='text-align:right;line-height:14.4pt'><b><span");
					eWriter.WriteLine("    lang=EN-US style='font-family:\"Trebuchet MS\";color:#CCCCCC'>" + +pair.Key + "</span></b></p>");
					eWriter.WriteLine("    </td>");
					eWriter.WriteLine("    <td width=32 style='width:24.0pt;padding:0mm 0mm 0mm 0mm'>");
					eWriter.WriteLine("    <p class=MsoNormal align=right style='text-align:right;line-height:14.4pt'><b><span");
					eWriter.WriteLine("    lang=EN-US style='font-family:\"Trebuchet MS\";color:#CCCCCC'>" + pair.Value + "</span></b></p>");
					eWriter.WriteLine("    </td>");
					eWriter.WriteLine("   </tr>");

					count++;
				}
				eWriter.WriteLine("  </table>");
				eWriter.WriteLine("  <p class=MsoNormal><span lang=EN-US><o:p></o:p></span></p>");
				eWriter.WriteLine("  </td>");
				eWriter.WriteLine("  <td style='padding:0mm 0mm 0mm 0mm'>");
				eWriter.WriteLine("  <p class=MsoNormal><span lang=EN-US style='font-size:10.0pt;font-family:Century'>&nbsp;</span></p>");
				eWriter.WriteLine("  </td>");
				eWriter.WriteLine(" </tr>");
				eWriter.WriteLine("</table>");

			}//----------------------------------------キャラ情報END-------------------------------------------------------

			eWriter.WriteLine("<p class=MsoNormal><span lang=EN-US>&nbsp;</span></p>");
			eWriter.WriteLine("</div>");
			eWriter.WriteLine("</div>");
			eWriter.WriteLine("</body>");
			eWriter.WriteLine("</html>");

		}//HtmlList

		private string GetName(int no)
		{
			foreach (var list in output.chara)
			{
				if (list.NO == no)
					return list.NAME;
			}
			return "";
		}

	}//class
}//namespace
