using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utau.Eramakerview.GameData;
using Utau.Eramakerview;

namespace Utau.Eramakerview
{
	public partial class MainForm : Form
    {
        public MainForm()
        {
           InitializeComponent();
		   label1.Text = "";
        }

		//CharacterTemplate[] charaList;
		int[] valiableList;
		string[][] pNameList;
		GameBaseData gbData;

		Dictionary<string, int>[] pNameDict = null;
		List<CharacterTemplate> cList = null;

		//TextBox1.Textを取得、設定するためのプロパティ
		public string TextBoxText 
		{
			set 
			{
				textBox1.Text = value;
			}
		}

		private void Start_Click(object sender, EventArgs e)
		{
			ConstantData cd = new ConstantData(this);
			ExeDir = Library.Library.ExeDir;
			CsvDir = ExeDir + "csv\\";

			//時間測定
			//DateTime dt = new DateTime();
			Stopwatch sw = new Stopwatch();
			sw.Start();
			//System.Threading.Thread.Sleep(1000);
			//データ読み込み
			cd.LoadData(CsvDir);
			sw.Stop();
			long millisec = sw.ElapsedMilliseconds;
			WriteLabel(millisec + "ms");
			//時間測定/

			//データコピー
			//this.charaList = cd.GetChara();
			cd.GetCharaRef(out this.cList);
			this.valiableList = cd.GetVariableSize();
			cd.GetParamDict(out this.pNameDict);
			this.pNameList = cd.GetParamName();
			this.gbData = cd.GetGameBase();

			//例：春香さんデータ展開
			/*
			foreach (int i in cList[11].Exp.Keys)
			{
				int key = i;
				this.WriteLabel("key = " + key + ",");
			}
			foreach (int i in cList[11].Exp.Values)
			{
				int key = i;
				this.WriteLabel("value = " + key + ",");
			}
			 */ 
			//this.WriteLabel("春香さんEXP:0 = " + cList[11].Exp[0]);
			//this.WriteLabel("春香さんEXP:1 = " + cList[11].Exp[1]);
			//this.WriteLabel("春香さんEXP:10 = " + cList[11].Exp[10]);
			//this.WriteLabel("春香さんEXP:93 = " + cList[93].Exp[93]);
			
			//↓パラレルだからか毎回cList[1]の中身が違う
			/*
			foreach (KeyValuePair<int, int> pair in cList[1].Cflag)
			{
					//if (pair.Value >= 0)
					//{
						this.WriteLabel(cList[1].CALLNAME+"さんCFLAG" + pair.Key + ": = " + pair.Value);
					//}
			}
			*/
			//return;
			//ここまで
			//
			//int temp;
			//temp = charaList[0].BASE[0];

			//データ書き込み
			Stopwatch sw2 = new Stopwatch();
			sw2.Start();
			OutputData od = new OutputData(this, gbData, pNameList, valiableList, cList,pNameDict);
			od.MakeHtml(CsvDir);
			sw2.Stop();
			long millisec2 = sw2.ElapsedMilliseconds;
			WriteLabel(millisec2 + "ms");
			//自分用
			//cd.WriteProgram(CsvDir + "EraIndex.html", CsvDir + "EraIndex.txt");
			//cd.WriteProgram(CsvDir + "EraMenu.html", CsvDir + "EraMenu.txt");
			//cd.WriteProgram(CsvDir + "EraData.html", CsvDir + "EraData.txt");
			//cd.WriteProgram(CsvDir + "EraData2.html", CsvDir + "EraData2.txt");
			//cd.WriteProgram(CsvDir + "DataSummary.html", CsvDir + "DataSummary.txt");
		}
		public static string ExeDir { get; private set; }
		public static string CsvDir { get; private set; }

		private void MainForm_Load(object sender, EventArgs e)
		{

		}

		private void label1_Click(object sender, EventArgs e)
		{
			
		}
		public void WriteLabel(string str) 
		{
			textBox1.Text += str;
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{

		}
    }
}
