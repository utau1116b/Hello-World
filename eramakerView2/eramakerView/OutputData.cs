using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utau.Eramakerview.Library;
using Utau.Eramakerview.Sub;
using Utau.Eramakerview;
using Utau.Eramakerview.GameData.Html;

namespace Utau.Eramakerview.GameData
{
	public class OutputData
	{
		public OutputData(MainForm mform, GameBaseData gbData, string[][] paramName, int[] variableData, List<CharacterTemplate> charaList, Dictionary<string, int>[] paramDict)
		{
			mf = mform;
			gb = gbData;
			pDict = paramDict;
			pname = paramName;
			vd = variableData;
			chara = charaList;
			chara.Sort((a, b) => a.NO - b.NO);//キャラを番号順にソート
		}

		public GameBaseData gb;
		public string[][] pname;
		public Dictionary<string, int>[] pDict;
		public int[] vd;
		public List<CharacterTemplate> chara;
		MainForm mf;
		//CharacterStrData cstr;
		public PMEMBER menber;

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
							this.EraData2(csvpath);
						},

			() =>
			{
				this.EraData(csvpath);
			});
		}

		private string GetKey(Dictionary<string, int> Dict, int val)
		{
			string s = "";
			foreach (KeyValuePair<string, int> pair in Dict)
			{
				if (pair.Value == val)
					s = pair.Key;
			}
			return s;
		}

		/*-----------------------------------------------EraIndex.html-------------------------------------------------------------------------*/

		private void MakeEraIndex(string csvpath)
		{
			HtmlBase eraindex = new EraIndex(csvpath, this, mf);
			eraindex.Create();
			eraindex.OutputDataList();
			eraindex.Close();
		}

		/*-------------------------------------------------EraMenu.html-----------------------------------------------------------------------*/

		private void MakeEraMenu(string csvpath)
		{
			HtmlBase eramenu = new EraMenu(csvpath, this, mf);

			eramenu.Create();
			eramenu.OutputDataList();
			eramenu.Close();
		}

		/*-------------------------------------------------------DataSummary.html-----------------------------------------------------------------*/

		private void DateSummary(string csvpath)
		{
			HtmlBase datasummary = new DataSummary(csvpath, this, mf);
			
			datasummary.Create();
			datasummary.OutputDataList();
			datasummary.Close();

			
		}

		/*-----------------------------------------------------------EraData.html-------------------------------------------------------------*/

		private void EraData(string csvpath)
		{
			HtmlBase eradata = new EraData(csvpath, this, mf);

			eradata.Create();
			eradata.OutputDataList();
			eradata.Close();
		}


		/*-----------------------------------------------------EraData2.html-------------------------------------------------------------------*/

		private void EraData2(string csvpath)
		{
			HtmlBase eradata = new EraData2(csvpath, this, mf);

			eradata.Create();
			eradata.OutputDataList();
			eradata.Close();
		}


		/*-----------------------------------------------------------WriteStyle-------------------------------------------------------------*/

		//スタイルを記述する
		private void WriteStyle(EraStreamWriter eWriter)
		{
			
		}

		/*-----------------------------------------------------------SetPID-------------------------------------------------------------*/

		//渡された変数にIDをセットする
		public void SetPID(out PMEMBER o, int x)
		{
			o.ID = 998;
			o.name = "";
			switch (x)
			{
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
		public struct PMEMBER
		{
			public int ID;
			public string name;
		}

	}
}
