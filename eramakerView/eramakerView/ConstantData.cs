using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utau.Eramakerview.Library;

namespace Utau.Eramakerview.GameData
{
    internal enum CharacterStrData
    {
        NAME = 0,
        CALLNAME = 1,
        NICKNAME = 2,
        MASTERNAME = 3,
        CSTR = 4,
    }

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

		public void LoadData(string csvPath)
		{

			//VariableSize.csv読み込み
			//LimitSize
			loadVariableSizeData(csvPath);

			//name[][]
			//if Range(範囲)
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

        private void loadVariableSizeData(string csvpath)
        {
            //ファイルが無い場合、デフォルト値を設定
			if (!File.Exists(csvpath))
                return;


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

		private void loadDataTo(string csvPath, int targetIndex, Int64[] targetI) { }
}

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
        public void ADD(string type ,int no,int data)
        {
            switch (type)
            {
                case "TALENT":
                    TALENT[no] = data;
                    break;

                    
            }
        }
    }

    //フェイルソフトな配列
    //variableSizeでフィルタを掛ける
    public class FailSoftArray
    {
        public FailSoftArray() { }

        int[] a;

        public int this[int index]
        {
            get
            {
                return a[index];
            }

            set
            {
                a[index] = value;
            }
        }
    }

}
