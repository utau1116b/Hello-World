using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			cTempList = new List<CharacterTemplate>();
		}

		MainForm mf;
		int tmp;
		int barval;

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
		//GameBaseのデータを入れる
		GameBaseData gbData = new GameBaseData();
		//キャラクターデータを入れる
		private List<CharacterTemplate> cTempList;
		private Dictionary<string, int>[] RevDict = new Dictionary<string, int>[13];//逆引き辞書
		private Dictionary<string, int> Dict = new Dictionary<string, int>();
		private Dictionary<int, string> Dict2 = new Dictionary<int, string>();

		private static Object lockObj = new Object();

		//charaListを返す
		public void GetCharaRef(out List<CharacterTemplate> chara)
		{
			chara = cTempList;
		}

		public int[] GetVariableSize()
		{
			return MaxDataList;
		}

		public GameBaseData GetGameBase()
		{
			return gbData;
		}

		public string[][] GetParamName()
		{
			return ParamNameList;
		}
		
		public void GetParamDict(out Dictionary<string, int>[] pname)
		{
			pname = RevDict;
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

			//テストINVOKE
			Parallel.Invoke(
				() =>
				{
					//パラメータ系CSV読み込み
					loadDataTo(csvPath + "Abl.csv", ablIndex, null);
					loadDataTo(csvPath + "Exp.csv", expIndex, null);
					loadDataTo(csvPath + "Talent.csv", talentIndex, null);
					loadDataTo(csvPath + "Palam.csv", palamIndex, null);

				},
				() =>
				{
					loadDataTo(csvPath + "Train.csv", trainIndex, null);
					loadDataTo(csvPath + "Mark.csv", markIndex, null);
					loadDataTo(csvPath + "Item.csv", itemIndex, null);
					loadDataTo(csvPath + "Base.csv", baseIndex, null);
				},
				() =>
				{
					loadDataTo(csvPath + "Source.csv", sourceIndex, null);
					loadDataTo(csvPath + "Equip.csv", equipIndex, null);
					loadDataTo(csvPath + "Cflag.csv", cflagIndex, null);

				});

			//逆引き辞書を作成
			for (int i = 0; i < countParam; i++)
			{
				for (int j = 0; j < ParamNameList[i].Length; j++)
				{
					if (ParamNameList[i][j] != null && ParamNameList[i][j] != "")
					{
						if (Dict.ContainsKey(ParamNameList[i][j]))//今のところキーが重複していたら追加しない
							continue;
						Dict.Add(ParamNameList[i][j], j);
					}

				}

				//RevDict[i] = Dict;
				RevDict[i] = new Dictionary<string, int>(Dict);
				//ここまではRevDictにちゃんと追加されている、ということは……
				Dict.Clear();
			}

			//キャラクター系CSV読み込み
			loadCharaDataFile(csvPath);
		}

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
							case "コード":
								gbData.code = vtoken[1];
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

					if (eReader.EOF() != true)//読み込めた
					{
						string[] vtoken = str.Split(',');
						try
						{
							int.TryParse(vtoken[1], out o);//配列の二番目を数字に変換できるか

							switch (vtoken[0])
							{
								case "BASENAME":
								case "BASE":
									if (o > MaxDataList[(int)CIntData.BASE])
										MaxDataList[(int)CIntData.BASE] = o;
									
									break;
								case "TALENTNAME":
								case "TALENT":
									if (o > MaxDataList[(int)CIntData.TALENT])
										MaxDataList[(int)CIntData.TALENT] = o;
							
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
									break;
							}
						}
						catch
						{
							tmp++;
						}
					}
					else
					{
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
				//for (int i = 0; i < 10; i++)
				//{
					//mf.WriteLabel("MAXDATALIST[" + i + "] = " + MaxDataList[i] + " \n");
				//}
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

			//switchでコードによって分岐させ、MaxDataListにlength値を入れる
			MaxDataList[0] = length;
		}

		//データ読みこみ
		private void loadDataTo(string csvPath, int targetIndex, Int64[] targetI)
		{
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
						try
						{
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
							//二度手間……
							ParamNameList[targetIndex][index] = target[index];
						}
						catch
						{

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
				System.Media.SystemSounds.Hand.Play();
				//予期しないエラーが発生しました
			}
			finally
			{
				eReader.Close();
			}
		}

		//キャラデータをリストに追加
		public void loadCharaDataFile(string path)
		{
			string[] filepath = Directory.GetFiles(path, "Chara*", System.IO.SearchOption.AllDirectories);
			//Parallel.ForEach(filepath.Select((s, i) => new { s, i }), paths =>

			barval = 0;
			//プログレスバーの最大値を設定
			mf.SetBarMax(filepath.Length  * 10);
			mf.SetBarMin(0);
			mf.SetBarVal(0);
			//var syncObject = new object();

			Parallel.For(0, filepath.Length, i =>
			{
				//lock (lockObj)
				//{
					string csvpath = filepath[i];
					CharacterTemplate templ = null;

					loadCharaDataTo(csvpath, out templ);

					if (templ != null)
						cTempList.Add(templ);

					/*
					System.Threading.Monitor.Enter(lockObj); // ロック取得
					try
					{
						barval += 10;
					 */
						
						System.Threading.Thread.Sleep(1);
						mf.SetBarVal(barval);
					/*
					 * 
					 * 
					 * 
					 * 
					}
					finally 
					{
						System.Threading.Monitor.Exit(lockObj);
					}
					 */ 
					//lock (mf) 
					//{
						//barval += 10;
						//mf.SetBarVal(barval);
					//}
				//}

			});
		}

		public void loadCharaDataTo(string path, out CharacterTemplate templa)
		{
			string filepath = path;
			CharacterTemplate templb = new CharacterTemplate();
			//中身を開いてテンプレへ保存
			EraStreamReader eReader = new EraStreamReader(mf);
			Dictionary<string, int> tDics = null;
			int flag0;
			flag0 = 0;

			if (!eReader.Open(filepath))
			{
				//開けなかった
				templa = null;
				return;
			}

			while (!eReader.EOF() == true)
			{
				string str = eReader.ReadLine();
				string[] tokens = str.Split(',');

				//文字列要素はここ
				switch (tokens[0])
				{
					case "名前":
					case "NAME":
						templb.NAME = tokens[1];
						break;
					case "呼び名":
					case "CALLNAME":
						templb.CALLNAME = tokens[1];
						break;
					default:
						break;
				}
				try
				{
					//*もし時間が掛かり過ぎるならcharalistの全要素をstringを文字形式にして
					//配列にそのまま文字を入れる
					int t1, t2,flag;
					flag = 0;
					//整数型要素はここ
					//二番目の要素を数値に変換できる？
					try
					{
						int.TryParse(tokens[1], out t1);
						if (int.TryParse(tokens[1], out t1) == true)
						{
							flag = 1;
						}
						//値を代入していく
						switch (tokens[0])
						{
							case "番号":
							case "NO":
								templb.NO = t1;
								break;
							case "基礎":
							case "BASE":
								t2 = int.Parse(tokens[2]);
								
								//0番の処理
								if (flag == 1 && t1 == 0)
								{
									templb.Base[0] = t2;
								}
								
								templb.Base.Add(t1, t2);
								break;
							case "素質":
							case "TALENT":
								//templb.Talent[t1] = 1;
								//0番の処理
								if (flag == 1 && t1 == 0)
								{
									templb.Talent[0] = 1;
								}
								templb.Talent.Add(t1, 1);
								break;
							case "能力":
							case "ABL":
								t2 = int.Parse(tokens[2]);
								//0番の処理
								if (flag == 1 && t1 == 0)
								{
									templb.Abl[0] = t2;
								}
								templb.Abl.Add(t1, t2);
								break;
							case "経験":
							case "EXP":
								t2 = int.Parse(tokens[2]);
								//0番の処理
								if (flag == 1 && t1 == 0)
								{
									templb.Exp[0] = t2;
								}
								templb.Exp.Add(t1, t2);
								break;
							case "相性":
							case "RELATION":
								t2 = int.Parse(tokens[2]);
								
								//0番の処理
								if (flag == 1 && t1 == 0)
								{
									templb.Relation[0] = t2;
								}
								
								templb.Relation.Add(t1, t2);
								break;
							case "EQUIP":
							case "装着物":
								break;
							case "フラグ":
							case "CFLAG":
								t2 = int.Parse(tokens[2]);
								//0番の処理
								if (flag == 1 && t1 == 0)
								{
									templb.Cflag[0] = t2;
									flag0 = 1;
								}
								templb.Cflag.Add(t1, t2);
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
								//mf.WriteLabel(templb.NO + "番 in Base");
								tDics = RevDict[baseIndex];
								if (tDics.ContainsKey(tokens[1]) == false)
									continue;
								t1 = tDics[tokens[1]];
								//if (t1 == 0) { templb.Base.Remove(0); }
								//0番の処理
								if (t1 == 0)
								{
									templb.Base[0] = int.Parse(tokens[2]);
								}
								templb.Base.Add(t1, int.Parse(tokens[2]));
								//templb.Base[t1] = int.Parse(tokens[2]);
								break;
							case "素質":
							case "TALENT":
								tDics = RevDict[talentIndex];
								if (tDics.ContainsKey(tokens[1]) == false)
									continue;
								t1 = tDics[tokens[1]];
								//0番の処理
								if (t1 == 0)
								{
									templb.Talent[0] = 1;
								}
								templb.Talent.Add(t1, 1);
								//templb.Talent[t1] = 1;
								break;
							case "能力":
							case "ABL":
								tDics = RevDict[ablIndex];
								if (tDics.ContainsKey(tokens[1]) == false)
									continue;
								t1 = tDics[tokens[1]];
								//0番の処理
								if (t1 == 0)
								{
									templb.Abl[0] = int.Parse(tokens[2]);
								}
								templb.Abl.Add(t1, int.Parse(tokens[2]));
								break;
							case "経験":
							case "EXP":
								tDics = RevDict[expIndex];
								if (tDics.ContainsKey(tokens[1]) == false)
									continue;
								t1 = tDics[tokens[1]];
								//0番の処理
								if (t1 == 0)
								{
									templb.Exp[0] = int.Parse(tokens[2]);
								}
								templb.Exp.Add(t1, int.Parse(tokens[2]));
								//templb.Exp[t1] = int.Parse(tokens[2]);
								//デバッグ
								//if (templb.CALLNAME == "春香") 
								//{
								//	mf.WriteLabel("EXP:" + t1 + " = " + int.Parse(tokens[2]));
								//}
								break;
							case "EQUIP":
							case "装着物":
								break;
							case "フラグ":
							case "CFLAG":
								tDics = RevDict[cflagIndex];
								if (tDics.ContainsKey(tokens[1]) == false)
									continue;
								t1 = tDics[tokens[1]];
								//0番の処理
								if (t1 == 0)
								{
									templb.Cflag[0] = int.Parse(tokens[2]);
									flag0 = 1;
								}
								templb.Cflag.Add(t1, int.Parse(tokens[2]));
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

			//Cflag[0]が0かつ、CSVに明記されていない
			if (flag0 == 0 && templb.Cflag[0] == 0)
			{
				templb.Cflag.Remove(0);
			}
			//SortedDictionary<int, int> stempl = new SortedDictionary<int, int>(templb.Cflag);
			//List<int, int> stempl = new List<KeyValuePair<int, int>>(templb.Cflag);
			//stempl.Sort(CompareKeyValuePair);
			//templb.Cflag = stempl.ToDictionary(x => x.Key);
			//Dictionary<KeyValuePair<int,int>>stempl2 = new Dictionary<KeyValuePair< int, int>>(stempl);

			templa = templb;

			//barval += 10;
			//mf.SetBarVal(barval);

			//});
		}//loadCharaDataTo

		// 二つのKeyValuePair<string, int>を比較するためのメソッド
		//static int CompareKeyValuePair(KeyValuePair<int, int> x, KeyValuePair<int, int> y);
	}//class

}//namespace

//キャラクターデータを格納するクラス型配列？
public class CharacterTemplate
{
	/*
	public CharacterTemplate() 
	{
		Base.Clear();
		Talent.Clear();
		Abl.Clear();
		Exp.Clear();
		Relation.Clear();
		Cflag.Clear();
	}
	 */ 

	public string NAME;
	public string CALLNAME;
	public int NO;

	//初期化しない場合、一番最初の要素を0番目に入れてしまう
	//初期化すると0番の要素を格納できない
	public Dictionary<int, int> Base = new Dictionary<int, int>() { { 0, 0 } };
	public Dictionary<int, int> Talent = new Dictionary<int, int>() { { 0, 0 } };
	public Dictionary<int, int> Abl = new Dictionary<int, int>() { { 0, 0 } };
	public Dictionary<int, int> Exp = new Dictionary<int, int>() { { 0, 0 } };
	public Dictionary<int, int> Relation = new Dictionary<int, int>() { { 0, 0 } };
	public Dictionary<int, int> Cflag = new Dictionary<int, int>() { { 0, 0 } };
	//public Dictionary<int, int> Equip = new Dictionary<int, int>();
}

public struct GameBaseData
{
	public string version;
	public string title;
	public string author;
	public string year;
	public string apendinfo;
	public string defver;
	public string code;
}

