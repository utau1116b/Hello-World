using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Utau.Eramakerview.Library;
using Utau.Eramakerview.Sub;

namespace Utau.Eramakerview.GameData
{
	//文字列変数をIDで識別する
	internal enum CharacterStrData
	{
		NAME = 0,
		CALLNAME = 1,
		NICKNAME = 2,
		MASTERNAME = 3,
		CSTR = 4,
	}

	//値型変数でIDを識別する
	internal enum CharacterIntData
	{
		BASE = 0,
		ABL = 1,
		TALET = 2,
		MARK = 3,
		EXP = 4,
		RELATION = 5,
		CFLAG = 6,
		EQUIP = 7,
		JUEL = 8,
	}


	/*
	 * 次はVariablesizeの読み込み
	 * →読み込み用機能
	 * →StreamReader
	 * →Ablなどの読み込みも考えた設計
	 * →設計図
	 */

	//キャラデータの読み込みや作成を行う
	public class ConstantData
	{
		private const int ablIndex = 0;
		private const int expIndex = 0;
		private const int talentIndex = 0;
		private const int palamIndex = 0;
		private const int trainIndex = 0;
		private const int markIndex = 0;
		private const int baseIndex = 0;
		private const int sourceIndex = 0;
		private const int equipIndex = 0;
		private const int cflagIndex = 0;
		private const int itemIndex = 0;

		public int[] MaxDataList = new int[0];

		public string[][] names = new string[0][];

		public void LoadData(string csvPath)
		{

			//VariableSize.csv読み込み
			loadVariableSizeData(csvPath);

			loadDataTo(csvPath + "Abl.csv", ablIndex, null);
			loadDataTo(csvPath + "Exp.csv", expIndex, null);
			loadDataTo(csvPath + "Talent.csv", talentIndex, null);
			loadDataTo(csvPath + "Palam.csv", palamIndex, null);
			loadDataTo(csvPath + "Train.csv", trainIndex, null);
			loadDataTo(csvPath + "Mark.csv", markIndex, null);
			loadDataTo(csvPath + "Item.csv", itemIndex, null);
			loadDataTo(csvPath + "Base.csv", baseIndex, null);
			loadDataTo(csvPath + "Source.csv", sourceIndex, null);
			loadDataTo(csvPath + "Equip.csv", equipIndex, null);
			loadDataTo(csvPath + "Cflag.csv", cflagIndex, null);
			//loadDataTo(csvPath + "Cstr.csv", ablIndex, null);

			//charaDataをLoad

		}

		//キャラクターの数
		private CharacterTemplate[] chara = new CharacterTemplate[9999];
		//CharacterTemplate charaData = new CharacterTemplate();

		//loadDataでcharaにデータを格納
		CharacterTemplate charaData = new CharacterTemplate();
		//chara[0] = charaData;

		/*
		 * charaData[x]+=loadData(Abl,x)
		 * or
		 * 
		 * for i=0,i<99;i++
		 * ReadLineで一列ずつ読む
		 * token[0][1][2]   ->  [能力,親密,1 or ABL,0,1]
		 *  switch token[0]
		 *      case "TALENT":
		 *      case "素質":
		 *          a=token[1]
		 *          if ToInt(a) != -1 ?
		 *             talent[a]=token[2]
		 *          else
		 *             a=ToIntTalent(a)
		 *             talent[a]=token[2]
		 *             
		 * token[2]->
		 *          
		 */

		//variablsesize.csvを読み込む
		private void loadVariableSizeData(string csvpath)
		{
			//ファイルが無い場合、デフォルト値を設定
			if (!File.Exists(csvpath))
				return;

			EraStreamReader eReader = new EraStreamReader();
			if (!eReader.Open(csvpath))
			{
				return;
			}
			try
			{
				StringStream st = null;
				while ((st = eReader.ReadEnabledLine()) != null)
				{
					//position=scriptposition
					//changeVariableSizeData
				}
			}
			catch
			{
				System.Media.SystemSounds.Hand.Play();
				return;
				//予期しないエラーが発生しました
			}
			finally
			{
				eReader.Close();
			}
			/*
			 * isOpen
			 * 
			 * while readline
			 * 
			 * サイズ調整
			 */



			/*
			 * chara型配列を宣言  Chara[]
			 * chara.toCharacterData(Chara chara, string tokens[])
			 */


		}

		//
		private void changeVariableSizeData(string line)
		{
			string[] tokens = line.Split(',');
			int length = 0;
			if (tokens.Length < 2)
			{
				//,が必要です
				return;
			}
			string idtoken = tokens[0].Trim();
			//idをゲットする
			//if (id識別不可)
			//if (2番目の値を整数値に変換できるかチェック)
			//変換できたらLengthに入れる
			//if (値が1000000以上)

			//switchでコードによって分岐させ、MaxDataListにlength値を入れる
			MaxDataList[0] = length;
		}

		//
		private void loadDataTo(string csvPath, int targetIndex, Int64[] targetI)
		{
			if (!File.Exists(csvPath))
				return;
			//配列のサイズを宣言
			string[] target = names[targetIndex];
			EraStreamReader eReader = new EraStreamReader();
			if (!eReader.Open(csvPath))
			{
 				//オープンに失敗しました
			}
			try
			{
				StringStream st = null;
				while ((st = eReader.ReadEnabledLine()) != null)
				{
					string[] tokens = st.Substring().Split(',');
					if (tokens.Length < 2)
					{
						//,が必要です
						continue;
					}
					int index = 0;
					if (!Int32.TryParse(tokens[0], out index))
					{
						//一つ目の値を整数値に変換できません
						continue;
					}
					if ((index < 0) || (target.Length <= index))
					{
						//配列の範囲外です
						continue;
					}
					target[index] = tokens[1];
				}
			}
			catch
			{
				System.Media.SystemSounds.Hand.Play();
				//予期しないエラーが発生しました
			}
			finally
			{
				eReader.Close();
			}
		}
	}

	//キャラクターデータを格納するクラス型配列？
	public class CharacterTemplate
	{
		public string NAME;
		public string CALLNAME;

		//配列はインデクサーかプロパティを使い、フェイルソフトに
		public int NO;
		public int[] BASE;
		public int[] TALENT;
		public int[] ABL;
		public int[] EXP;
		public int[] RELATION;
		public int[] CFLAG;

		//LENGTH
		//typeはenumでintにした方が綺麗？
		public void ADD(string type, int no, int data)
		{
			switch (type)
			{
				case "TALENT":
					TALENT[no] = data;
					break;


			}
		}
	}
}
