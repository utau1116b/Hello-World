﻿using System;
using System.Collections.Generic;
using System.Text;
using MinorShift.Emuera.Sub;
using System.Text.RegularExpressions;
using MinorShift.Emuera.GameData.Variable;
using MinorShift.Emuera.GameData.Expression;
using MinorShift.Emuera.GameView;
using MinorShift.Emuera.GameData;
using MinorShift.Emuera.GameData.Function;
using MinorShift.Emuera.GameProc.Function;

namespace MinorShift.Emuera.GameProc
{
	internal sealed class UserDefinedVariableData
	{
		private UserDefinedVariableData()
		{
		}
		public string Name = null;
		public int Dimension = 1;
		public int[] Lengths = null;
		public bool TypeStr = false;
		public bool Global = false;
		public bool Save = false;
		public bool Static = true;
		public bool Private = false;
		public bool Reference = false;
		public static UserDefinedVariableData Create(WordCollection wc, bool dims, bool isPrivate, ScriptPosition sc)
		{
			string dimtype = dims ? "#DIM" : "#DIMS";
			UserDefinedVariableData ret = new UserDefinedVariableData();
			ret.TypeStr = dims;

			IdentifierWord idw = null;
			bool staticDefined = false;
			string keyword = dimtype;
			while (!wc.EOL && (idw = wc.Current as IdentifierWord) != null)
			{
				wc.ShiftNext();
				keyword = idw.Code;
				if (Config.ICVariable)
					keyword = keyword.ToUpper();
				switch (keyword)
				{
					case "REF":
						//TODO 1808beta009
						throw new CodeEE("未実装の機能です", sc);
						if (!isPrivate)
							throw new CodeEE("広域変数の宣言に" + keyword + "キーワードは指定できません", sc);
						if (staticDefined && ret.Static)
							throw new CodeEE("STATICとREFキーワードは同時に指定できません", sc);
						if (ret.Reference)
							throw new CodeEE(keyword + "キーワードが二重に指定されています", sc);
						ret.Reference = true;
						ret.Static = true;
						break;
					case "DYNAMIC":
						if (!isPrivate)
							throw new CodeEE("広域変数の宣言に" + keyword + "キーワードは指定できません", sc);
						if (staticDefined)
							if (ret.Static)
								throw new CodeEE("STATICとDYNAMICキーワードは同時に指定できません", sc);
							else
								throw new CodeEE(keyword + "キーワードが二重に指定されています", sc);
						staticDefined = true;
						ret.Static = false;
						break;
					case "STATIC":
						if (!isPrivate)
							throw new CodeEE("広域変数の宣言に" + keyword + "キーワードは指定できません", sc);
						if (staticDefined)
							if (!ret.Static)
								throw new CodeEE("STATICとDYNAMICキーワードは同時に指定できません", sc);
							else
								throw new CodeEE(keyword + "キーワードが二重に指定されています", sc);
						if (ret.Reference)
							throw new CodeEE("STATICとREFキーワードは同時に指定できません", sc);
						staticDefined = true;
						ret.Static = true;
						break;
					case "GLOBAL":
						if (isPrivate)
							throw new CodeEE("ローカル変数の宣言に" + keyword + "キーワードは指定できません", sc);
						if (ret.Global)
							throw new CodeEE(keyword + "キーワードが二重に指定されています", sc);
						ret.Global = true;
						break;
					case "SAVEDATA":
						if (isPrivate)
							throw new CodeEE("ローカル変数の宣言に" + keyword + "キーワードは指定できません", sc);
						if (ret.Save)
							throw new CodeEE(keyword + "キーワードが二重に指定されています", sc);
						ret.Save = true;
						break;
					case "CHARDATA":
						throw new CodeEE("キャラ変数の宣言は実装されていません", sc);
					default:
						ret.Name = keyword;
						goto whilebreak;
				}
			}
		whilebreak:
			if (ret.Name == null)
				throw new CodeEE(keyword + "の後に有効な変数名が指定されていません", sc);
			string errMes = "";
			int errLevel = -1;
			if (isPrivate)
				GlobalStatic.IdentifierDictionary.CheckUserPrivateVarName(ref errMes, ref errLevel, ret.Name);
			else
				GlobalStatic.IdentifierDictionary.CheckUserVarName(ref errMes, ref errLevel, ret.Name);
			if (errLevel >= 0)
			{
				if (errLevel >= 2)
					throw new CodeEE(errMes, sc);
				ParserMediator.Warn(errMes, sc, errLevel);
			}
			List<int> sizeNum = new List<int>();
			while (!wc.EOL)
			{
				if (wc.Current.Type != ',')
					throw new CodeEE("書式が間違っています", sc);
				wc.ShiftNext();
				if (ret.Reference)//参照型の場合は要素数不要
				{
					if (wc.EOL)
						break;
					if (wc.Current.Type == ',')
					{
						sizeNum.Add(0);
						continue;
					}
				}
				if (wc.EOL)
					throw new CodeEE("カンマの後に有効な定数式が指定されていません", sc);
				IOperandTerm arg = ExpressionParser.ReduceIntegerTerm(wc, TermEndWith.Comma);
				SingleTerm sizeTerm = arg.Restructure(null) as SingleTerm;
				if ((sizeTerm == null) || (sizeTerm.GetOperandType() != typeof(Int64)))
					throw new CodeEE("カンマの後に有効な定数式が指定されていません", sc);
				if (ret.Reference)//参照型には要素数指定不可(0にするか書かないかどっちか
				{
					if (sizeTerm.Int != 0)
						throw new CodeEE("参照型変数にはサイズを指定できません(サイズを省略するか0を指定してください)", sc);
					continue;
				}
				else if ((sizeTerm.Int <= 0) || (sizeTerm.Int > 1000000))
					throw new CodeEE("ユーザー定義変数のサイズは1以上1000000以下でなければなりません", sc);
				sizeNum.Add((int)sizeTerm.Int);
			}
			if (sizeNum.Count == 0)
				sizeNum.Add(1);
			ret.Private = isPrivate;
			ret.Dimension = sizeNum.Count;
			if (ret.Dimension > 3)
				throw new CodeEE("4次元以上の配列変数を宣言することはできません", sc);
			ret.Lengths = new int[sizeNum.Count];
			if (ret.Reference)
				return ret;
			Int64 totalBytes = 1;
			for (int i = 0; i < sizeNum.Count; i++)
			{
				ret.Lengths[i] = sizeNum[i];
				totalBytes *= ret.Lengths[i];
			}
			if ((totalBytes <= 0) || (totalBytes > 1000000))
				throw new CodeEE("ユーザー定義変数のサイズは1以上1000000以下でなければなりません", sc);
			if (!isPrivate && dims && ret.Dimension > 1 && ret.Save && !Config.SystemSaveInBinary)
				throw new CodeEE("文字列型の多次元配列変数にSAVEDATAフラグを付ける場合には「バイナリ型セーブ」オプションが必要です", sc);

			return ret;
		}
	}
}