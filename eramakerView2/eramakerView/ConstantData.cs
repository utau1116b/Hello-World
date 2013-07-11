using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Utau.Eramakerview.Library;
using Utau.Eramakerview.Sub;
using Utau.Eramakerview;

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
	internal enum CIntData
	{
		BASE = 0,
		ABL = 1,
		TALENT = 2,
		MARK = 3,
		EXP = 4,
		RELATION = 5,
		CFLAG = 6,
		EQUIP = 7,
		JUEL = 8,
		SOURCE = 9,
		ITEM = 10,
		TRAIN = 11,
		PALAM = 12,
	}

	//キャラデータの読み込みや作成を行う
	public class ConstantData
	{
		public ConstantData(MainForm mainForm)
		{
			mf = mainForm;//メインフォームへの参照
		}

		MainForm mf;
		int tmp;

		//
		private const int ablIndex = 1;
		private const int expIndex = 4;
		private const int talentIndex = 2;
		private const int palamIndex = 12;
		private const int trainIndex = 11;
		private const int markIndex = 3;
		private const int baseIndex = 0;
		private const int sourceIndex = 9;
		private const int equipIndex = 7;
		private const int cflagIndex = 6;
		private const int itemIndex = 10;
		private const int countParam = 13;

		private void setDefaultArrayLength()
		{
			MaxDataList[ablIndex] = 100;
			MaxDataList[talentIndex] = 1000;
			MaxDataList[expIndex] = 100;
			MaxDataList[markIndex] = 100;
			MaxDataList[trainIndex] = 1000;
			MaxDataList[itemIndex] = 1000;
			MaxDataList[baseIndex] = 100;
			MaxDataList[sourceIndex] = 1000;
			MaxDataList[equipIndex] = 100;
			MaxDataList[cflagIndex] = 1000;
		}

		//パラメータのLength値を入れる
		public int[] MaxDataList = new int[countParam];
		//パラメータの名前を入れる
		public string[][] ParamNameList = new string[countParam][];
		//loadDataでcharaにデータを格納
		CharacterTemplate[] charaList;
		//GameBaseのデータを入れる
		GameBaseData gbData = new GameBaseData();

		//charaListを返す
		public CharacterTemplate[] GetChara ()
		{
			return charaList;
		}

		public int[] GetVariableSize() 
		{
			return MaxDataList;
		}

		public GameBaseData GetGameBase ()
		{
			return gbData;
		}

		public string[][] GetParamName ()
		{
			return ParamNameList;
		}

		public void LoadData(string csvPath)
		{
			//GameBase読み込み
			loadGameBaseData(csvPath + "GameBase.csv");

			//VariableSize.csv読み込み
			loadVariableSizeData(csvPath + "VariableSize.csv");

			//ジャグ配列を作る→changeVariableSizeへ
			for (int i = 0; i < countParam; i++)
			{
				ParamNameList[i] = new string[MaxDataList[i]];
			}

			//パラメータ系CSV読み込み
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

			//キャラクター系CSV読み込み
			loadCharaDataTo(csvPath);

			//for (int i = 0; i < 100; i++) 
			//{
			//	mf.WriteLabel(" " + i + "=" + ParamNameList[ablIndex][i]);
			//}
			//loadDataTo(csvPath + "Cstr.csv", ablIndex, null);

			//charaDataをLoad

		}

		//loadDataでcharaにデータを格納
		//CharacterTemplate[] charaList;
		//GameBaseのデータを入れる
		//GameBaseData[] gbdata;

		//自分用
		//プログラムにプログラムを書かせる
		//filepath1 html		filepath2 txt
		public void WriteProgram(string filepath1, string filepath2)
		{
			string str = "";

			if (!File.Exists(filepath1))
				return;

			EraStreamReader eReader = new EraStreamReader(mf);
			EraStreamWriter eWriter = new EraStreamWriter(mf);
			eWriter.MakeFile(filepath2);

			if (!eReader.Open(filepath1))
			{
				//-1が返ったら終了
				return;
			}

			try
			{
				while (true)
				{
					str = eReader.ReadLine();

					if (!eReader.EOF())
					{
						//空文字で無ければ出力
						if (str != "" && str != "\n" && str != null)
						{
							eWriter.WriteLine("eWriter.WriteLine(\"" + str + "\");");
						}
					}
					else
					{
						break;
					}
				}
				eReader.Close();
				eWriter.Close();
			}
			catch
			{
				eReader.Close();
				eWriter.Close();
			}
		}

		//GameBase.ERBを読み込む
		private void loadGameBaseData(string filepath)
		{
			string str = "";//一時的に使う変数

			//ファイルが無い場合、デフォルト値を設定

			if (!File.Exists(filepath))
				return;

			EraStreamReader eReader = new EraStreamReader(mf);

			if (!eReader.Open(filepath))
			{
				//-1が返ったら終了
				return;
			}

			try
			{
				while (true)
				{
					str = eReader.ReadLine();

					if (!eReader.EOF())
					{
						string[] vtoken = str.Split(',');

						switch (vtoken[0])
						{
							case "バージョン":
								gbData.version = vtoken[1];
								break;
							case "タイトル":
								gbData.title = vtoken[1];
								break;
							case "作者":
								gbData.author = vtoken[1];
								break;
							case "製作年":
								gbData.year = vtoken[1];
								break;
							case "追加情報":
								gbData.apendinfo = vtoken[1];
								break;
							case "バージョン違い認める":
								gbData.defver = vtoken[1];
								break;
							default:
								break;
						}
					}
					else 
					{
						eReader.Close();
						break;
					}
				}
			}
			catch
			{
				//不明なエラー
			}
		}

		//variablsesize.csvを読み込む
		private void loadVariableSizeData(string filepath)
		{
			string str = "";//一時的に使う変数

			//ファイルが無い場合、デフォルト値を設定

			if (!File.Exists(filepath))
				return;

			EraStreamReader eReader = new EraStreamReader(mf);
			if (!eReader.Open(filepath))
			{
				//-1が返ったらデフォルト値をセットして終了
				setDefaultArrayLength();
				return;
			}

			try
			{
				string[] array = new string[] { };
				int o;
				while (true)
				{
					str = eReader.ReadLine();//一行読み込み文字取得

					//mf.WriteLabel("readline後 " + str + "←中身 \n" + eReader.EOF());
					//eReader.ReadTest();
					//mf.WriteLabel("a");
					if (eReader.EOF() != true)//読み込めた
					{
						string[] vtoken = str.Split(',');
						//mf.WriteLabel("b");
						try
						{
							int.TryParse(vtoken[1], out o);//配列の二番目を数字に変換できるか

							switch (vtoken[0])
							{
								case "BASENAME":
								case "BASE":
									if (o > MaxDataList[(int)CIntData.BASE])
										MaxDataList[(int)CIntData.BASE] = o;
									//mf.WriteLabel("。。" + vtoken[0] + " = " + MaxDataList[(int)CIntData.BASE] + "。。"+"い"+(int)CIntData.BASE+"い");
									break;
								case "TALENTNAME":
								case "TALENT":
									if (o > MaxDataList[(int)CIntData.TALENT])
										MaxDataList[(int)CIntData.TALENT] = o;
									//mf.WriteLabel("。。" + vtoken[0] + " = " + MaxDataList[(int)CIntData.BASE] + "。。" + "い" + (int)CIntData.BASE + "い");
									break;
								case "EXPNAME":
								case "EXP":
									if (o > MaxDataList[(int)CIntData.EXP])
										MaxDataList[(int)CIntData.EXP] = o;
									//mf.WriteLabel("。。" + vtoken[0] + " = " + MaxDataList[(int)CIntData.EXP] + "。。");
									break;
								case "ABLNAME":
								case "ABL":
									if (o > MaxDataList[(int)CIntData.ABL])
										MaxDataList[(int)CIntData.ABL] = o;
									break;
								case "CFLAGNAME":
								case "CFLAG":
									if (o > MaxDataList[(int)CIntData.CFLAG])
										MaxDataList[(int)CIntData.CFLAG] = o;
									break;
								case "EQUIPNAME":
								case "EQUIP":
									if (o > MaxDataList[(int)CIntData.EQUIP])
										MaxDataList[(int)CIntData.EQUIP] = o;
									break;
								case "PALAMNAME":
								case "PALAM":
									if (o > MaxDataList[(int)CIntData.PALAM])
										MaxDataList[(int)CIntData.PALAM] = o;
									break;
								case "MARKNAME":
								case "MARK":
									if (o > MaxDataList[(int)CIntData.MARK])
										MaxDataList[(int)CIntData.MARK] = o;
									break;
								case "TRAINNAME":
								case "TRAIN":
									if (o > MaxDataList[(int)CIntData.TRAIN])
										MaxDataList[(int)CIntData.TRAIN] = o;
									break;
								case "ITEMNAME":
								case "ITEM":
									if (o > MaxDataList[(int)CIntData.ITEM])
										MaxDataList[(int)CIntData.ITEM] = o;
									break;
								case "SOURCENAME":
								case "SOURCE":
									if (o > MaxDataList[(int)CIntData.SOURCE])
										MaxDataList[(int)CIntData.SOURCE] = o;
									break;
								default:
									//mf.WriteLabel("d。。"+vtoken[0]+" = "+ o +"。。");
									break;
							}
						}
						catch
						{
							tmp++;
							//mf.WriteLabel("e");
							//mf.WriteLabel("数値に変換出来ませんでした。。。\n");
						}

					}
					else
					{
						//mf.WriteLabel("f");
						eReader.Close();//末尾に達した
						break;
					}
				}

			}
			catch
			{
				System.Media.SystemSounds.Hand.Play();
				//予期しないエラーが発生しました
			}
			finally
			{
				for (int i = 0; i < 10; i++)
				{
					mf.WriteLabel("MAXDATALIST[" + i + "] = " + MaxDataList[i] + " \n");

				}
				eReader.Close();
			}

		}

		//値の許容値を変更する
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

		//データ読みこみ
		private void loadDataTo(string csvPath, int targetIndex, Int64[] targetI)
		{
			//mf.WriteLabel("h");
			if (!File.Exists(csvPath))
				return;

			//配列のサイズを宣言
			string[] target = new string[MaxDataList[targetIndex]];

			EraStreamReader eReader = new EraStreamReader(mf);
			if (!eReader.Open(csvPath))
			{
				//オープンに失敗しました
				return;
			}
			try
			{
				while (true)
				{
					string str = eReader.ReadLine();

					string[] tokens = str.Split(',');
					if (!eReader.EOF() == true)
					{
						//mf.WriteLabel("a");
						try
						{
							//mf.WriteLabel("b");
							if (tokens.Length < 2)
							{
								//,が必要です
								continue;
							}
							//mf.WriteLabel("c");
							int index = 0;
							if (!Int32.TryParse(tokens[0], out index))
							{
								//一つ目の値を整数値に変換できません
								continue;
							}
							//mf.WriteLabel("d");
							if ((index < 0) || (target.Length <= index))
							{
								//配列の範囲外です
								continue;
							}
							//mf.WriteLabel("e");
							target[index] = tokens[1];
							//二度手間……
							ParamNameList[targetIndex][index] = target[index];
						}
						catch
						{
							//mf.WriteLabel("f");
						}
					}
					else
					{
						eReader.Close();
						break;
					}
				}
			}
			catch
			{
				mf.WriteLabel("  Index" + targetIndex + "でエラーが発生しました");
				System.Media.SystemSounds.Hand.Play();
				//予期しないエラーが発生しました
			}
			finally
			{
				eReader.Close();
			}
		}

		public void loadCharaDataTo(string path)
		{

			//キャラクターの数をカウント
			int[] targeta = new int[100];
			string[] filepath = Directory.GetFiles(path, "Chara*", System.IO.SearchOption.AllDirectories);

			//中身を開いてテンプレへ保存
			EraStreamReader eReader = new EraStreamReader(mf);
			charaList = new CharacterTemplate[filepath.Length];//配列の要素数を決定
			charaList[0].NAME = "あなた";
			mf.WriteLabel("charaList.Length = " + charaList.Length + " charaList[0] = " + charaList[0].NAME);

			foreach (var paths in filepath.Select((s, i) => new { s, i }))
			{
				if (!eReader.Open(paths.s))
				{
					//開けなかった
					continue;
				}

				if (paths.i == 10)
				{
					mf.WriteLabel("charaList[0].NAME = " + charaList[0].NAME);
					return;
				}

				int count = paths.i;//ループカウンタ

				while (!eReader.EOF() == true)
				{
					string str = eReader.ReadLine();
					string[] tokens = str.Split(',');

					//文字列要素はここ
					switch (tokens[0])
					{
						case "名前":
						case "NAME":
							charaList[count].NAME = tokens[1];
							break;
						case "呼び名":
						case "CALLNAME":
							charaList[count].CALLNAME = tokens[1];
							break;
						default:
							break;
					}

					try
					{
						//*もし時間が掛かり過ぎるならcharalistの全要素をstringを文字形式にして
						//配列にそのまま文字を入れる
						int t1, t2;
						//整数型要素はここ
						//二番目の要素を数値に変換できる？
						try
						{
							int.TryParse(tokens[1], out t1);
							//値を代入していく
							switch (tokens[0])
							{
								case "番号":
								case "NO":
									charaList[count].NO = t1;
									break;
								case "基礎":
								case "BASE":
									t2 = int.Parse(tokens[2]);
									charaList[count].BASE[t1] = t2;
									break;
								case "素質":
								case "TALENT":
									t2 = int.Parse(tokens[2]);
									charaList[count].TALENT[t1] = 1;
									break;
								case "能力":
								case "ABL":
									t2 = int.Parse(tokens[2]);
									charaList[count].ABL[t1] = t2;
									break;
								case "経験":
								case "EXP":
									t2 = int.Parse(tokens[2]);
									charaList[count].EXP[t1] = t2;
									break;
								case "相性":
								case "relation":
									break;
								case "EQUIP":
								case "装着物":
									break;
								case "フラグ":
								case "CFLAG":
									t2 = int.Parse(tokens[2]);
									charaList[count].CFLAG[t1] = t2;
									break;
								default:
									continue;

							}
						}
						//変換できなかったら配列から探す
						catch
						{
							switch (tokens[0])
							{
								case "基礎":
								case "BASE":
									t1 = Array.IndexOf(ParamNameList[baseIndex], tokens[1]);
									if (t1 == -1)
										continue;
									charaList[count].BASE[t1] = int.Parse(tokens[2]);
									break;
								case "素質":
								case "TALENT":
									t1 = Array.IndexOf(ParamNameList[talentIndex], tokens[1]);
									if (t1 == -1)
										continue;
									charaList[count].TALENT[t1] = int.Parse(tokens[2]);
									break;
								case "能力":
								case "ABL":
									t1 = Array.IndexOf(ParamNameList[ablIndex], tokens[1]);
									if (t1 == -1)
										continue;
									charaList[count].ABL[t1] = int.Parse(tokens[2]);
									break;
								case "経験":
								case "EXP":
									t1 = Array.IndexOf(ParamNameList[expIndex], tokens[1]);
									if (t1 == -1)
										continue;
									charaList[count].EXP[t1] = int.Parse(tokens[2]);
									break;
								case "EQUIP":
								case "装着物":
									break;
								case "フラグ":
								case "CFLAG":
									t1 = Array.IndexOf(ParamNameList[cflagIndex], tokens[1]);
									if (t1 == -1)
										continue;
									charaList[count].CFLAG[t1] = int.Parse(tokens[2]);
									break;
								default:
									continue;

							}
						}
					}
					//数値に変換出来なかった
					catch
					{
						continue;
					}
				}

			}
		}//loadCharaDataTo

	}//class

}//namespace


	//キャラクターデータを格納するクラス型配列？
public struct CharacterTemplate
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
	public int[] EQUIP;

	//LENGTH
	//typeはenumでintにした方が綺麗？
	/*
	public void ADD(string type, int no, int data)
	{
		switch (type)
		{
			case "TALENT":
				TALENT[no] = data;
				break;


		}
	}
	 */
}

public struct GameBaseData
{
	public string version;
	public string title;
	public string author;
	public string year;
	public string apendinfo;
	public string defver;
}