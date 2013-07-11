using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

		CharacterTemplate[] charaList;
		int[] valiableList;
		string[][] pNameList;
		GameBaseData gbData;

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
			//データ読み込み
			cd.LoadData(CsvDir);

			//データコピー
			this.charaList = cd.GetChara();
			this.valiableList = cd.GetVariableSize();
			this.pNameList = cd.GetParamName();
			this.gbData = cd.GetGameBase();

			

			//データ書き込み
			OutputData od = new OutputData(this, gbData, pNameList, valiableList, charaList);
			od.MakeHtml(CsvDir);

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
