﻿using System;
using System.Collections.Generic;
using System.Text;
using MinorShift.Emuera.Sub;
using MinorShift.Emuera.GameProc;
using MinorShift.Emuera.GameData.Expression;

namespace MinorShift.Emuera.GameData.Variable
{
	//IndexOutOfRangeException, ArgumentOutOfRangeExceptionを投げることがある。VariableTermの方で処理すること。
	//引数は整数しか受け付けない。*.csvを利用した置換はVariableTermの方で処理すること
	internal abstract class VariableToken
	{
		protected VariableToken(VariableCode varCode, VariableData varData)
		{
			Code = varCode;
			VariableType = ((varCode & VariableCode.__INTEGER__) == VariableCode.__INTEGER__) ? typeof(Int64) : typeof(string);
			VarCodeInt = (int)(varCode & VariableCode.__LOWERCASE__);
			varName = varCode.ToString();
			this.varData = varData;
			IsPrivate = false;
			IsReference = false;
			Dimension = 0;
			IsGlobal = (Code == VariableCode.GLOBAL) || (Code == VariableCode.GLOBALS);
			if ((Code & VariableCode.__ARRAY_1D__) == VariableCode.__ARRAY_1D__)
				Dimension = 1;
			if ((Code & VariableCode.__ARRAY_2D__) == VariableCode.__ARRAY_2D__)
				Dimension = 2;
			if ((Code & VariableCode.__ARRAY_3D__) == VariableCode.__ARRAY_3D__)
				Dimension = 3;


			IsSavedata = false;
			if ((Code == VariableCode.GLOBAL) || (Code == VariableCode.GLOBALS))
				IsSavedata = true;
			else if ((Code & VariableCode.__SAVE_EXTENDED__) == VariableCode.__SAVE_EXTENDED__)
			{
				IsSavedata = true;
			}
			else if (((Code & VariableCode.__EXTENDED__) != VariableCode.__EXTENDED__)
				&& ((Code & VariableCode.__CALC__) != VariableCode.__CALC__)
				&& ((Code & VariableCode.__UNCHANGEABLE__) != VariableCode.__UNCHANGEABLE__)
				&& ((Code & VariableCode.__LOCAL__) != VariableCode.__LOCAL__)
				&& (!varName.StartsWith("NOTUSE_")))
			{
				VariableCode flag = Code & (VariableCode.__ARRAY_1D__ | VariableCode.__ARRAY_2D__ | VariableCode.__ARRAY_3D__ | VariableCode.__STRING__ | VariableCode.__INTEGER__ | VariableCode.__CHARACTER_DATA__);
				switch (flag)
				{
					case VariableCode.__CHARACTER_DATA__ | VariableCode.__INTEGER__:
						if (VarCodeInt < (int)VariableCode.__COUNT_SAVE_CHARACTER_INTEGER__)
							IsSavedata = true;
						break;
					case VariableCode.__CHARACTER_DATA__ | VariableCode.__STRING__:
						if (VarCodeInt < (int)VariableCode.__COUNT_SAVE_CHARACTER_STRING__)
							IsSavedata = true;
						break;
					case VariableCode.__CHARACTER_DATA__ | VariableCode.__INTEGER__ | VariableCode.__ARRAY_1D__:
						if (VarCodeInt < (int)VariableCode.__COUNT_SAVE_CHARACTER_INTEGER_ARRAY__)
							IsSavedata = true;
						break;
					case VariableCode.__CHARACTER_DATA__ | VariableCode.__STRING__ | VariableCode.__ARRAY_1D__:
						if (VarCodeInt < (int)VariableCode.__COUNT_SAVE_CHARACTER_STRING_ARRAY__)
							IsSavedata = true;
						break;
					case VariableCode.__INTEGER__:
						if (VarCodeInt < (int)VariableCode.__COUNT_SAVE_INTEGER__)
							IsSavedata = true;
						break;
					case VariableCode.__STRING__:
						if (VarCodeInt < (int)VariableCode.__COUNT_SAVE_STRING__)
							IsSavedata = true;
						break;
					case VariableCode.__INTEGER__ | VariableCode.__ARRAY_1D__:
						if (VarCodeInt < (int)VariableCode.__COUNT_SAVE_INTEGER_ARRAY__)
							IsSavedata = true;
						break;
					case VariableCode.__STRING__ | VariableCode.__ARRAY_1D__:
						if (VarCodeInt < (int)VariableCode.__COUNT_SAVE_STRING_ARRAY__)
							IsSavedata = true;
						break;
				}
			}
		}

		public readonly VariableCode Code;
		public readonly int VarCodeInt;
		protected readonly VariableData varData;
		protected string varName;
		public Type VariableType { get; protected set; }
		public bool CanRestructure { get; protected set; }
		public string Name { get { return varName; } }


		//CodeEEにしているけど実際はExeEEかもしれない
		public virtual Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
		{ throw new CodeEE("整数型でない変数" + varName + "を整数型として呼び出しました"); }
		public virtual string GetStrValue(ExpressionMediator exm, Int64[] arguments)
		{ throw new CodeEE("文字列型でない変数" + varName + "を文字列型として呼び出しました"); }
		public virtual void SetValue(Int64 value, Int64[] arguments)
		{ throw new CodeEE("整数型でない変数" + varName + "を整数型として呼び出しました"); }
		public virtual void SetValue(string value, Int64[] arguments)
		{ throw new CodeEE("文字列型でない変数" + varName + "を文字列型として呼び出しました"); }
		public virtual void SetValue(Int64[] values, Int64[] arguments)
		{ throw new CodeEE("整数型配列でない変数" + varName + "を整数型配列として呼び出しました"); }
		public virtual void SetValue(string[] values, Int64[] arguments)
		{ throw new CodeEE("文字列型配列でない変数" + varName + "を文字列型配列として呼び出しました"); }
		public virtual void SetValueAll(Int64 value, int start, int end, int charaPos)
		{ throw new CodeEE("整数型配列でない変数" + varName + "を整数型配列として呼び出しました"); }
		public virtual void SetValueAll(string value, int start, int end, int charaPos)
		{ throw new CodeEE("文字列型配列でない変数" + varName + "を文字列型配列として呼び出しました"); }
		public virtual Int64 PlusValue(Int64 value, Int64[] arguments)
		{ throw new CodeEE("整数型でない変数" + varName + "を整数型として呼び出しました"); }
		public virtual Int32 GetLength()
		{ throw new CodeEE("配列型でない変数" + varName + "の長さを取得しようとしました"); }
		public virtual Int32 GetLength(int dimension)
		{ throw new CodeEE("配列型でない変数" + varName + "の長さを取得しようとしました"); }
		public virtual object GetArray()
		{
			if (IsCharacterData)
				throw new CodeEE("キャラクタ変数" + varName + "を非キャラ変数として呼び出しました");
			throw new CodeEE("配列型でない変数" + varName + "の配列を取得しようとしました");
		}
		public virtual object GetArrayChara(int charano)
		{
			if (!IsCharacterData)
				throw new CodeEE("非キャラクタ変数" + varName + "をキャラ変数として呼び出しました");
			throw new CodeEE("配列型でない変数" + varName + "の配列を取得しようとしました");
		}

		public void throwOutOfRangeException(Int64[] arguments, Exception e)
		{
			CheckElement(arguments, new bool[] { true, true, true });
			throw e;
		}
		public virtual void CheckElement(Int64[] arguments, bool[] doCheck) { }
        public void CheckElement(Int64[] arguments)
        {
            CheckElement(arguments, new bool[] { true, true, true });
        }
        public virtual void IsArrayRangeValid(Int64[] arguments, Int64 index1, Int64 index2, string funcName, Int64 i1, Int64 i2)
		{
            CheckElement(arguments, new bool[] { true, true, true });
			return;
		}

		public int CodeInt
		{ get { return VarCodeInt; } }
		public VariableCode CodeFlag
		{ get { return Code & VariableCode.__UPPERCASE__; } }

		public bool IsNull
		{
			get
			{
				return Code == VariableCode.__NULL__;
			}
		}
		public bool IsCharacterData
		{
			get
			{
				return ((Code & VariableCode.__CHARACTER_DATA__) == VariableCode.__CHARACTER_DATA__);
			}
		}
		public bool IsInteger
		{
			get
			{
				return ((Code & VariableCode.__INTEGER__) == VariableCode.__INTEGER__);
			}
		}
		public bool IsString
		{
			get
			{
				return ((Code & VariableCode.__STRING__) == VariableCode.__STRING__);
			}
		}
		public bool IsArray1D
		{
			get
			{
				return ((Code & VariableCode.__ARRAY_1D__) == VariableCode.__ARRAY_1D__);
			}
		}
		public bool IsArray2D
		{
			get
			{
				return ((Code & VariableCode.__ARRAY_2D__) == VariableCode.__ARRAY_2D__);
			}
		}
		public bool IsArray3D
		{
			get
			{
				return ((Code & VariableCode.__ARRAY_3D__) == VariableCode.__ARRAY_3D__);
			}
		}
		public bool Readonly
		{
			get
			{
				return ((Code & VariableCode.__UNCHANGEABLE__) == VariableCode.__UNCHANGEABLE__);
			}
		}
		public bool IsCalc
		{
			get
			{
				return ((Code & VariableCode.__CALC__) == VariableCode.__CALC__);
			}
		}
		public bool IsLocal
		{
			get
			{
				return ((Code & VariableCode.__LOCAL__) == VariableCode.__LOCAL__);
			}
		}
		public bool IsPrivate { get; protected set; }
		public bool IsGlobal { get; protected set; }
		public bool IsSavedata { get; protected set; }
		public bool IsReference { get; protected set; }
		public int Dimension { get; protected set; }

	}

	internal abstract class UserDefinedVariableToken : VariableToken
	{
		protected UserDefinedVariableToken(VariableCode varCode, UserDefinedVariableData data)
			: base(varCode, null)
		{
			varName = data.Name;
			IsPrivate = data.Private;
			this.sizes = data.Lengths;
			this.IsGlobal = data.Global;
			this.IsSavedata = data.Save;
			//Dimension = sizes.Length;
			totalSize = 1;
			for (int i = 0; i < sizes.Length; i++)
				totalSize *= sizes[i];
		}

		public abstract void SetDefault();
		protected int[] sizes;
		protected int totalSize;
		//public bool IsGlobal { get; protected set; }
		//public bool IsSavedata { get; protected set; }
		public override Int32 GetLength()
		{
			if (this.Dimension == 1)
				return sizes[0];
			throw new CodeEE(Dimension.ToString() + "次元配列型変数" + varName + "の長さを取得しようとしました");
		}

		public override Int32 GetLength(int dimension)
		{
			if (dimension < this.Dimension)
				return sizes[dimension];
			throw new CodeEE("配列型変数" + varName + "の存在しない次元の長さを取得しようとしました");
		}
		public override void CheckElement(Int64[] arguments, bool[] doCheck)
		{
			//if (array == null)
			//	throw new ExeEE("プライベート変数" + varName + "の配列が用意されていない");

			if (doCheck[0] && ((arguments[0] < 0) || (arguments[0] >= sizes[0])) )
				throw new CodeEE("配列型変数" + varName + "の第１引数(" + arguments[0].ToString() + ")は配列の範囲外です");
			if (sizes.Length >= 2 && ((arguments[1] < 0) || (arguments[1] >= sizes[1])))
				throw new CodeEE("配列型変数" + varName + "の第２引数(" + arguments[1].ToString() + ")は配列の範囲外です");
			if (sizes.Length >= 3 && ((arguments[2] < 0) || (arguments[2] >= sizes[2])))
				throw new CodeEE("配列型変数" + varName + "の第３引数(" + arguments[2].ToString() + ")は配列の範囲外です");
		}
		public override void IsArrayRangeValid(Int64[] arguments, Int64 index1, Int64 index2, string funcName, Int64 i1, Int64 i2)
		{
			CheckElement(arguments);
			if ((index1 < 0) || (index1 > sizes[Dimension - 1]))
				throw new CodeEE(funcName + "命令の第" + i1.ToString() + "引数(" + index1.ToString() + ")は配列" + varName + "の範囲外です");
			if ((index2 < 0) || (index2 > sizes[Dimension - 1]))
				throw new CodeEE(funcName + "命令の第" + i2.ToString() + "引数(" + index2.ToString() + ")は配列" + varName + "の範囲外です");
		}
		public abstract void In();
		public abstract void Out();
		public bool IsStatic { get; protected set; }
	}

	//1808beta009 廃止 UserDefinedVariableTokenで一括して扱う
	//internal abstract class PrivateVariableToken : UserDefinedVariableToken
	//{
	//    protected PrivateVariableToken(VariableCode varCode, UserDefinedVariableData data)
	//        : base(varCode, data)
	//    {
	//        IsPrivate = true;
	//    }
	//}

	/// <summary>
	/// 1808beta009 追加
	/// 参照型。今のところ引数の参照渡しにしか使えない。
	/// 将来privateでない参照型も追加するかもしれないが多分しない。
	/// </summary>
	internal abstract class ReferenceToken : UserDefinedVariableToken
	{
		protected ReferenceToken(VariableCode varCode, UserDefinedVariableData data)
			: base(varCode, data)
		{
			CanRestructure = false;
			IsStatic = !data.Private;
			IsReference = true;
			arrayList = new List<Array>();
		}
		protected List<Array> arrayList = null;
		protected Array array = null;
		public override void SetDefault()
		{//Defaultのセットは参照元がやるべき
		}
		public override Int32 GetLength()
		{
			if (array == null)
				throw new CodeEE("参照型変数" + varName + "は何も参照していません");
			if (this.Dimension != 1)
				throw new CodeEE(Dimension.ToString() + "次元配列型変数" + varName + "の長さを取得しようとしました");
			return array.Length;
		}

		public override Int32 GetLength(int dimension)
		{
			if (array == null)
				throw new CodeEE("参照型変数" + varName + "は何も参照していません");
			if (dimension < this.Dimension)
				return array.GetLength(dimension);
			throw new CodeEE("配列型変数" + varName + "の存在しない次元の長さを取得しようとしました");
		}
		public override void CheckElement(Int64[] arguments, bool[] doCheck)
		{
			if (array == null)
				throw new CodeEE("参照型変数" + varName + "は何も参照していません");
			if (doCheck[0] && ((arguments[0] < 0) || (arguments[0] >= array.GetLength(0))))
				throw new CodeEE("配列型変数" + varName + "の第１引数(" + arguments[0].ToString() + ")は配列の範囲外です");
			if (Dimension >= 2 && ((arguments[1] < 0) || (arguments[1] >= array.GetLength(1))))
				throw new CodeEE("配列型変数" + varName + "の第２引数(" + arguments[1].ToString() + ")は配列の範囲外です");
			if (Dimension >= 3 && ((arguments[2] < 0) || (arguments[2] >= array.GetLength(2))))
				throw new CodeEE("配列型変数" + varName + "の第３引数(" + arguments[2].ToString() + ")は配列の範囲外です");
		}
		public override void IsArrayRangeValid(Int64[] arguments, Int64 index1, Int64 index2, string funcName, Int64 i1, Int64 i2)
		{
			CheckElement(arguments);
			if ((index1 < 0) || (index1 > array.GetLength(Dimension -1)))
				throw new CodeEE(funcName + "命令の第" + i1.ToString() + "引数(" + index1.ToString() + ")は配列" + varName + "の範囲外です");
			if ((index2 < 0) || (index2 > array.GetLength(Dimension - 1)))
				throw new CodeEE(funcName + "命令の第" + i2.ToString() + "引数(" + index2.ToString() + ")は配列" + varName + "の範囲外です");
		}

		int counter = 0;
		public override void In()
		{
			if (counter > 0)
				arrayList.Add(array);
			counter++;
			array = null;
		}

		public override void Out()
		{
			//arrayList.RemoveAt(arrayList.Count - 1);
			if (arrayList.Count > 0)
			{
				array = arrayList[arrayList.Count - 1];
				arrayList.RemoveAt(arrayList.Count - 1);
			}
			else
				array = null;
			counter--;
		}
		public override object GetArray()
		{
			if (array == null)
				throw new CodeEE("参照型変数" + varName + "は何も参照していません");
			return array;
		}
		public void SetRef(Array refArray)
		{
			array = refArray;
		}
	}

	internal abstract class LocalVariableToken : VariableToken
	{
		public LocalVariableToken(VariableCode varCode, VariableData varData, string subId, int size)
			: base(varCode, varData)
		{
			CanRestructure = false;
			this.subID = subId;
			this.size = size;
		}
		public abstract void SetDefault();
		public abstract void resize(int newSize);
		protected string subID;
		protected int size;
		public override Int32 GetLength()
		{
			return size;
		}
		public override Int32 GetLength(int dimension)
		{
			if (dimension == 0)
				return size;
			throw new CodeEE("配列型変数" + varName + "の存在しない次元の長さを取得しようとしました");
		}
		public override void CheckElement(Int64[] arguments, bool[] doCheck)
		{
			//if (array == null)
			//	throw new ExeEE("プライベート変数" + varName + "の配列が用意されていない");
            if (doCheck[0] && ((arguments[0] < 0) || (arguments[0] >= size)))
				throw new CodeEE("配列変数" + varName + "の第１引数(" + arguments[0].ToString() + ")は配列の範囲外です");
		}
		public override void IsArrayRangeValid(Int64[] arguments, Int64 index1, Int64 index2, string funcName, Int64 i1, Int64 i2)
		{
			CheckElement(arguments);
			if ((index1 < 0) || (index1 > size))
				throw new CodeEE(funcName + "命令の第" + i1.ToString() + "引数(" + index1.ToString() + ")は配列" + varName + "の範囲外です");
			if ((index2 < 0) || (index2 > size))
				throw new CodeEE(funcName + "命令の第" + i2.ToString() + "引数(" + index2.ToString() + ")は配列" + varName + "の範囲外です");
		}
	}



	//サブクラスの詳細はVariableData以外は知らなくてよい
	internal sealed partial class VariableData
	{
		#region 変数
		private sealed class IntVariableToken : VariableToken
		{
			public IntVariableToken(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				CanRestructure = false;
				array = varData.DataInteger;
			}
			Int64[] array;
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				return array[VarCodeInt];
			}

			public override void SetValue(Int64 value, Int64[] arguments)
			{
				array[VarCodeInt] = value;
			}

			public override void SetValueAll(long value, int start, int end, int charaPos)
			{
				array[VarCodeInt] = value;
			}

			public override Int64 PlusValue(Int64 value, Int64[] arguments)
			{
				array[VarCodeInt] += value;
				return array[VarCodeInt];
			}
		}

		private sealed class Int1DVariableToken : VariableToken
		{
			public Int1DVariableToken(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				CanRestructure = false;
				array = varData.DataIntegerArray[VarCodeInt];
			}
			Int64[] array;
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				return array[arguments[0]];
			}

			public override void SetValue(Int64 value, Int64[] arguments)
			{
				array[arguments[0]] = value;
			}

			public override void SetValue(Int64[] values, Int64[] arguments)
			{
				int start = (int)arguments[0];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					array[i] = values[i - start];
			}

			public override void SetValueAll(long value, int start, int end, int charaPos)
			{
				for (int i = start; i < end; i++)
					array[i] = value;
			}

			public override Int64 PlusValue(Int64 value, Int64[] arguments)
			{
				array[arguments[0]] += value;
				return array[arguments[0]];
			}
			public override Int32 GetLength()
			{ return array.Length; }
			public override Int32 GetLength(int dimension)
			{
				if (dimension == 0)
					return array.Length;
				throw new CodeEE("配列型変数" + varName + "の存在しない次元の長さを取得しようとしました");
			}
			public override object GetArray() { return array; }

			public override void CheckElement(Int64[] arguments, bool[] doCheck)
			{
				if (doCheck[0] && ((arguments[0] < 0) || (arguments[0] >= array.Length)))
					throw new CodeEE("配列変数" + varName + "の第１引数(" + arguments[0].ToString() + ")は配列の範囲外です");
			}
			public override void IsArrayRangeValid(Int64[] arguments, Int64 index1, Int64 index2, string funcName, Int64 i1, Int64 i2)
			{
				CheckElement(arguments);
				if ((index1 < 0) || (index1 > array.Length))
					throw new CodeEE(funcName + "命令の第" + i1.ToString() + "引数(" + index1.ToString() + ")は配列" + varName + "の範囲外です");
				if ((index2 < 0) || (index2 > array.Length))
					throw new CodeEE(funcName + "命令の第" + i2.ToString() + "引数(" + index2.ToString() + ")は配列" + varName + "の範囲外です");
			}
		}

		private sealed class Int2DVariableToken : VariableToken
		{
			public Int2DVariableToken(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				CanRestructure = false;
				array = varData.DataIntegerArray2D[VarCodeInt];
			}
			Int64[,] array;
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				return array[arguments[0], arguments[1]];
			}

			public override void SetValue(Int64 value, Int64[] arguments)
			{
				array[arguments[0], arguments[1]] = value;
			}

			public override void SetValue(Int64[] values, Int64[] arguments)
			{
				int start = (int)arguments[1];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					array[arguments[0], i] = values[i - start];
			}

			public override void SetValueAll(long value, int start, int end, int charaPos)
			{
				int a1 = array.GetLength(0);
				int a2 = array.GetLength(1);
				for (int i = 0; i < a1; i++)
					for (int j = 0; j < a2; j++)
						array[i, j] = value;
			}
			public override Int64 PlusValue(Int64 value, Int64[] arguments)
			{
				array[arguments[0], arguments[1]] += value;
				return array[arguments[0], arguments[1]];
			}
			public override Int32 GetLength()
			{ throw new CodeEE("2次元配列型変数" + varName + "の長さを取得しようとしました"); }
			public override Int32 GetLength(int dimension)
			{
				if ((dimension == 0) || (dimension == 1))
					return array.GetLength(dimension);
				throw new CodeEE("配列型変数" + varName + "の存在しない次元の長さを取得しようとしました");
			}
			public override object GetArray() { return array; }

			public override void CheckElement(Int64[] arguments, bool[] doCheck)
			{
				if (doCheck[0] && ((arguments[0] < 0) || (arguments[0] >= array.GetLength(0))))
					throw new CodeEE("二次元配列" + varName + "の第１引数(" + arguments[0].ToString() + ")は配列の範囲外です");
				if (doCheck[1] && ((arguments[1] < 0) || (arguments[1] >= array.GetLength(1))))
					throw new CodeEE("二次元配列" + varName + "の第２引数(" + arguments[1].ToString() + ")は配列の範囲外です");
			}
			public override void IsArrayRangeValid(Int64[] arguments, Int64 index1, Int64 index2, string funcName, Int64 i1, Int64 i2)
			{
				CheckElement(arguments);
				if ((index1 < 0) || (index1 > array.GetLength(1)))
					throw new CodeEE(funcName + "命令の第" + i1.ToString() + "引数(" + index1.ToString() + ")は配列" + varName + "の範囲外です");
				if ((index2 < 0) || (index2 > array.GetLength(1)))
					throw new CodeEE(funcName + "命令の第" + i2.ToString() + "引数(" + index2.ToString() + ")は配列" + varName + "の範囲外です");
			}
		}

		private sealed class Int3DVariableToken : VariableToken
		{
			public Int3DVariableToken(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				CanRestructure = false;
				array = varData.DataIntegerArray3D[VarCodeInt];
			}
			Int64[, ,] array;
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				return array[arguments[0], arguments[1], arguments[2]];
			}

			public override void SetValue(Int64 value, Int64[] arguments)
			{
				array[arguments[0], arguments[1], arguments[2]] = value;
			}
			public override void SetValue(Int64[] values, Int64[] arguments)
			{
				int start = (int)arguments[2];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					array[arguments[0], arguments[1], i] = values[i - start];
			}

			public override void SetValueAll(long value, int start, int end, int charaPos)
			{
				int a1 = array.GetLength(0);
				int a2 = array.GetLength(1);
				int a3 = array.GetLength(2);
				for (int i = 0; i < a1; i++)
					for (int j = 0; j < a2; j++)
						for (int k = 0; k < a3; k++)
							array[i, j, k] = value;
			}

			public override Int64 PlusValue(Int64 value, Int64[] arguments)
			{
				array[arguments[0], arguments[1], arguments[2]] += value;
				return array[arguments[0], arguments[1], arguments[2]];
			}
			public override Int32 GetLength()
			{ throw new CodeEE("3次元配列型変数" + varName + "の長さを取得しようとしました"); }
			public override Int32 GetLength(int dimension)
			{
				if ((dimension == 0) || (dimension == 1) || (dimension == 2))
					return array.GetLength(dimension);
				throw new CodeEE("配列型変数" + varName + "の存在しない次元の長さを取得しようとしました");
			}
			public override object GetArray() { return array; }

			public override void CheckElement(Int64[] arguments, bool[] doCheck)
			{
				if (doCheck[0] && ((arguments[0] < 0) || (arguments[0] >= array.GetLength(0))))
					throw new CodeEE("三次元配列" + varName + "の第１引数(" + arguments[0].ToString() + ")は配列の範囲外です");
				if (doCheck[1] && ((arguments[1] < 0) || (arguments[1] >= array.GetLength(1))))
					throw new CodeEE("三次元配列" + varName + "の第２引数(" + arguments[1].ToString() + ")は配列の範囲外です");
				if (doCheck[2] && ((arguments[2] < 0) || (arguments[2] >= array.GetLength(2))))
					throw new CodeEE("三次元配列" + varName + "の第３引数(" + arguments[2].ToString() + ")は配列の範囲外です");
			}
			public override void IsArrayRangeValid(Int64[] arguments, Int64 index1, Int64 index2, string funcName, Int64 i1, Int64 i2)
			{
				CheckElement(arguments);
				if ((index1 < 0) || (index1 > array.GetLength(2)))
					throw new CodeEE(funcName + "命令の第" + i1.ToString() + "引数(" + index1.ToString() + ")は配列" + varName + "の範囲外です");
				if ((index2 < 0) || (index2 > array.GetLength(2)))
					throw new CodeEE(funcName + "命令の第" + i2.ToString() + "引数(" + index2.ToString() + ")は配列" + varName + "の範囲外です");
			}
		}

		private sealed class StrVariableToken : VariableToken
		{
			public StrVariableToken(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				CanRestructure = false;
				array = varData.DataString;
			}
			string[] array;
			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				return array[VarCodeInt];
			}

			public override void SetValue(string value, Int64[] arguments)
			{
				array[VarCodeInt] = value;
			}

			public override void SetValueAll(string value, int start, int end, int charaPos)
			{
				array[VarCodeInt] = value;
			}

		}

		private sealed class Str1DVariableToken : VariableToken
		{
			public Str1DVariableToken(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				CanRestructure = false;
				array = varData.DataStringArray[VarCodeInt];
			}
			string[] array;
			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				return array[arguments[0]];
			}

			public override void SetValue(string value, Int64[] arguments)
			{
				array[arguments[0]] = value;
			}
			public override void SetValue(string[] values, Int64[] arguments)
			{
				int start = (int)arguments[0];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					array[i] = values[i - start];
			}
			public override void SetValueAll(string value, int start, int end, int charaPos)
			{
				for (int i = start; i < end; i++)
					array[i] = value;
			}
			public override Int32 GetLength()
			{ return array.Length; }
			public override Int32 GetLength(int dimension)
			{
				if (dimension == 0)
					return array.Length;
				throw new CodeEE("配列型変数" + varName + "の存在しない次元の長さを取得しようとしました");
			}
			public override object GetArray() { return array; }

			public override void CheckElement(Int64[] arguments, bool[] doCheck)
			{
				if (doCheck[0] && ((arguments[0] < 0) || (arguments[0] >= array.Length)))
					throw new CodeEE("配列変数" + varName + "の第１引数(" + arguments[0].ToString() + ")は配列の範囲外です");
			}
			public override void IsArrayRangeValid(Int64[] arguments, Int64 index1, Int64 index2, string funcName, Int64 i1, Int64 i2)
			{
				CheckElement(arguments);
				if ((index1 < 0) || (index1 > array.Length))
					throw new CodeEE(funcName + "命令の第" + i1.ToString() + "引数(" + index1.ToString() + ")は配列" + varName + "の範囲外です");
				if ((index2 < 0) || (index2 > array.Length))
					throw new CodeEE(funcName + "命令の第" + i2.ToString() + "引数(" + index2.ToString() + ")は配列" + varName + "の範囲外です");
			}
		}

		private sealed class Str2DVariableToken : VariableToken
		{
			public Str2DVariableToken(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				CanRestructure = false;
				array = varData.DataStringArray2D[VarCodeInt];
			}
			string[,] array;
			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				return array[arguments[0], arguments[1]];
			}

			public override void SetValue(string value, Int64[] arguments)
			{
				array[arguments[0], arguments[1]] = value;
			}

			public override void SetValue(string[] values, Int64[] arguments)
			{
				int start = (int)arguments[1];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					array[arguments[0], i] = values[i - start];
			}
			public override void SetValueAll(string value, int start, int end, int charaPos)
			{
				int a1 = array.GetLength(0);
				int a2 = array.GetLength(1);
				for (int i = 0; i < a1; i++)
					for (int j = 0; j < a2; j++)
						array[i, j] = value;
			}
			public override Int32 GetLength()
			{ throw new CodeEE("2次元配列型変数" + varName + "の長さを取得しようとしました"); }
			public override Int32 GetLength(int dimension)
			{
				if ((dimension == 0) || (dimension == 1))
					return array.GetLength(dimension);
				throw new CodeEE("配列型変数" + varName + "の存在しない次元の長さを取得しようとしました");
			}
			public override object GetArray() { return array; }

			public override void CheckElement(Int64[] arguments, bool[] doCheck)
			{
				if (doCheck[0] && ((arguments[0] < 0) || (arguments[0] >= array.GetLength(0))))
					throw new CodeEE("二次元配列" + varName + "の第１引数(" + arguments[0].ToString() + ")は配列の範囲外です");
				if (doCheck[1] && ((arguments[1] < 0) || (arguments[1] >= array.GetLength(1))))
					throw new CodeEE("二次元配列" + varName + "の第２引数(" + arguments[1].ToString() + ")は配列の範囲外です");
			}
			public override void IsArrayRangeValid(Int64[] arguments, Int64 index1, Int64 index2, string funcName, Int64 i1, Int64 i2)
			{
				CheckElement(arguments);
				if ((index1 < 0) || (index1 > array.GetLength(1)))
					throw new CodeEE(funcName + "命令の第" + i1.ToString() + "引数(" + index1.ToString() + ")は配列" + varName + "の範囲外です");
				if ((index2 < 0) || (index2 > array.GetLength(1)))
					throw new CodeEE(funcName + "命令の第" + i2.ToString() + "引数(" + index2.ToString() + ")は配列" + varName + "の範囲外です");
			}
		}

		private sealed class Str3DVariableToken : VariableToken
		{
			public Str3DVariableToken(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				CanRestructure = false;
				array = varData.DataStringArray3D[VarCodeInt];
			}
			string[, ,] array;
			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				return array[arguments[0], arguments[1], arguments[2]];
			}

			public override void SetValue(string value, Int64[] arguments)
			{
				array[arguments[0], arguments[1], arguments[2]] = value;
			}

			public override void SetValue(string[] values, Int64[] arguments)
			{
				int start = (int)arguments[2];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					array[arguments[0], arguments[1], i] = values[i - start];
			}
			public override void SetValueAll(string value, int start, int end, int charaPos)
			{
				int a1 = array.GetLength(0);
				int a2 = array.GetLength(1);
				int a3 = array.GetLength(2);
				for (int i = 0; i < a1; i++)
					for (int j = 0; j < a2; j++)
						for (int k = 0; k < a3; k++)
							array[i, j, k] = value;
			}
			public override Int32 GetLength()
			{ throw new CodeEE("3次元配列型変数" + varName + "の長さを取得しようとしました"); }
			public override Int32 GetLength(int dimension)
			{
				if ((dimension == 0) || (dimension == 1) || (dimension == 2))
					return array.GetLength(dimension);
				throw new CodeEE("配列型変数" + varName + "の存在しない次元の長さを取得しようとしました");
			}
			public override object GetArray() { return array; }

			public override void CheckElement(Int64[] arguments, bool[] doCheck)
			{
				if (doCheck[0] && ((arguments[0] < 0) || (arguments[0] >= array.GetLength(0))))
					throw new CodeEE("三次元配列" + varName + "の第１引数(" + arguments[0].ToString() + ")は配列の範囲外です");
				if (doCheck[1] && ((arguments[1] < 0) || (arguments[1] >= array.GetLength(1))))
					throw new CodeEE("三次元配列" + varName + "の第２引数(" + arguments[1].ToString() + ")は配列の範囲外です");
				if (doCheck[2] && ((arguments[2] < 0) || (arguments[2] >= array.GetLength(2))))
					throw new CodeEE("三次元配列" + varName + "の第３引数(" + arguments[2].ToString() + ")は配列の範囲外です");
			}
			public override void IsArrayRangeValid(Int64[] arguments, Int64 index1, Int64 index2, string funcName, Int64 i1, Int64 i2)
			{
				CheckElement(arguments);
				if ((index1 < 0) || (index1 > array.GetLength(2)))
					throw new CodeEE(funcName + "命令の第" + i1.ToString() + "引数(" + index1.ToString() + ")は配列" + varName + "の範囲外です");
				if ((index2 < 0) || (index2 > array.GetLength(2)))
					throw new CodeEE(funcName + "命令の第" + i2.ToString() + "引数(" + index2.ToString() + ")は配列" + varName + "の範囲外です");
			}
		}

		private sealed class CharaIntVariableToken : VariableToken
		{
			public CharaIntVariableToken(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				CanRestructure = false;
			}
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				CharacterData chara = varData.CharacterList[(int)arguments[0]];
				return chara.DataInteger[VarCodeInt];
			}

			public override void SetValue(Int64 value, Int64[] arguments)
			{
				CharacterData chara = varData.CharacterList[(int)arguments[0]];
				chara.DataInteger[VarCodeInt] = value;
			}


			public override void SetValueAll(long value, int start, int end, int charaPos)
			{
				varData.CharacterList[charaPos].setValueAll(VarCodeInt, value);
				//CharacterData chara = varData.CharacterList[charaPos];
				//chara.DataInteger[VarCodeInt] = value;
			}

			public override Int64 PlusValue(Int64 value, Int64[] arguments)
			{
				CharacterData chara = varData.CharacterList[(int)arguments[0]];
				chara.DataInteger[VarCodeInt] += value;
				return chara.DataInteger[VarCodeInt];
			}
			public override void CheckElement(Int64[] arguments, bool[] doCheck)
			{
				if (doCheck[0] && ((arguments[0] < 0) || (arguments[0] >= varData.CharacterList.Count)))
					throw new CodeEE("キャラクタ配列変数" + varName + "の第１引数(" + arguments[0].ToString() + ")はキャラ登録番号の範囲外です");
			}
		}

		private sealed class CharaInt1DVariableToken : VariableToken
		{
			public CharaInt1DVariableToken(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				CanRestructure = false;

			}
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				CharacterData chara = varData.CharacterList[(int)arguments[0]];
				return chara.DataIntegerArray[VarCodeInt][arguments[1]];
			}

			public override void SetValue(Int64 value, Int64[] arguments)
			{
				CharacterData chara = varData.CharacterList[(int)arguments[0]];
				chara.DataIntegerArray[VarCodeInt][arguments[1]] = value;
			}

			public override void SetValue(Int64[] values, Int64[] arguments)
			{
				CharacterData chara = varData.CharacterList[(int)arguments[0]];
				Int64[] array = chara.DataIntegerArray[VarCodeInt];
				int start = (int)arguments[1];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					array[i] = values[i - start];
			}

			public override void SetValueAll(long value, int start, int end, int charaPos)
			{
				varData.CharacterList[charaPos].setValueAll1D(VarCodeInt, value, start, end);
				//CharacterData chara = varData.CharacterList[charaPos];
				//Int64[] array = chara.DataIntegerArray[VarCodeInt];
				//for (int i = start; i < end; i++)
				//    array[i] = value;
			}

			public override Int64 PlusValue(Int64 value, Int64[] arguments)
			{
				CharacterData chara = varData.CharacterList[(int)arguments[0]];
				chara.DataIntegerArray[VarCodeInt][arguments[1]] += value;
				return chara.DataIntegerArray[VarCodeInt][arguments[1]];
			}
			public override Int32 GetLength()
			{ return varData.Constant.CharacterIntArrayLength[VarCodeInt]; }
			public override Int32 GetLength(int dimension)
			{
				if (dimension == 0)
					return varData.Constant.CharacterIntArrayLength[VarCodeInt];
				throw new CodeEE("配列型変数" + varName + "の存在しない次元の長さを取得しようとしました");
			}

			public override object GetArrayChara(int charano)
			{
				CharacterData chara = varData.CharacterList[charano];
				return chara.DataIntegerArray[VarCodeInt];
			}

			public override void CheckElement(Int64[] arguments, bool[] doCheck)
			{
				if (doCheck[0] && ((arguments[0] < 0) || (arguments[0] >= varData.CharacterList.Count)))
					throw new CodeEE("キャラクタ配列変数" + varName + "の第１引数(" + arguments[0].ToString() + ")はキャラ登録番号の範囲外です");
				//少しでも処理を少なく
				if (!doCheck[1])
					return;
				Int64 arraySize = doCheck[0] ? varData.CharacterList[(int)arguments[0]].DataIntegerArray[VarCodeInt].Length : GlobalStatic.ConstantData.DummyChara.DataIntegerArray[VarCodeInt].Length; ;
				if ((arguments[1] < 0) || (arguments[1] >= arraySize))
					throw new CodeEE("キャラクタ配列変数" + varName + "の第２引数(" + arguments[1].ToString() + ")は配列の範囲外です");
			}
			public override void IsArrayRangeValid(Int64[] arguments, Int64 index1, Int64 index2, string funcName, Int64 i1, Int64 i2)
			{
				CheckElement(arguments);
				CharacterData chara = varData.CharacterList[(int)arguments[0]];
				Int64[] array = chara.DataIntegerArray[VarCodeInt];
				if ((index1 < 0) || (index1 > array.Length))
					throw new CodeEE(funcName + "命令の第" + i1.ToString() + "引数(" + index1.ToString() + ")は配列" + varName + "の範囲外です");
				if ((index2 < 0) || (index2 > array.Length))
					throw new CodeEE(funcName + "命令の第" + i2.ToString() + "引数(" + index2.ToString() + ")は配列" + varName + "の範囲外です");
			}
		}


		private sealed class CharaStrVariableToken : VariableToken
		{
			public CharaStrVariableToken(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				CanRestructure = false;
			}
			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				CharacterData chara = varData.CharacterList[(int)arguments[0]];
				return chara.DataString[VarCodeInt];
			}

			public override void SetValue(string value, Int64[] arguments)
			{
				CharacterData chara = varData.CharacterList[(int)arguments[0]];
				chara.DataString[VarCodeInt] = value;
			}

			public override void SetValueAll(string value, int start, int end, int charaPos)
			{
				varData.CharacterList[charaPos].setValueAll(VarCodeInt, value);
				//CharacterData chara = varData.CharacterList[charaPos];
				//chara.DataString[VarCodeInt] = value;
			}

			public override void CheckElement(Int64[] arguments, bool[] doCheck)
			{
				if (doCheck[0] && ((arguments[0] < 0) || (arguments[0] >= varData.CharacterList.Count)))
					throw new CodeEE("キャラクタ配列変数" + varName + "の第１引数(" + arguments[0].ToString() + ")はキャラ登録番号の範囲外です");
			}

		}

		private sealed class CharaStr1DVariableToken : VariableToken
		{
			public CharaStr1DVariableToken(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				CanRestructure = false;
			}
			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				CharacterData chara = varData.CharacterList[(int)arguments[0]];
				return chara.DataStringArray[VarCodeInt][arguments[1]];
			}

			public override void SetValue(string value, Int64[] arguments)
			{
				CharacterData chara = varData.CharacterList[(int)arguments[0]];
				chara.DataStringArray[VarCodeInt][arguments[1]] = value;
			}

			public override void SetValue(string[] values, Int64[] arguments)
			{
				CharacterData chara = varData.CharacterList[(int)arguments[0]];
				string[] array = chara.DataStringArray[VarCodeInt];
				int start = (int)arguments[1];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					array[i] = values[i - start];
			}

			public override void SetValueAll(string value, int start, int end, int charaPos)
			{
				varData.CharacterList[charaPos].setValueAll1D(VarCodeInt, value, start, end);
				//CharacterData chara = varData.CharacterList[charaPos];
				//String[] array = chara.DataStringArray[VarCodeInt];
				//for (int i = start; i < end; i++)
				//    array[i] = value;
			}
			public override Int32 GetLength()
			{ return varData.Constant.CharacterStrArrayLength[VarCodeInt]; }
			public override Int32 GetLength(int dimension)
			{
				if (dimension == 0)
					return varData.Constant.CharacterStrArrayLength[VarCodeInt];
				throw new CodeEE("配列型変数" + varName + "の存在しない次元の長さを取得しようとしました");
			}


			public override object GetArrayChara(int charano)
			{
				CharacterData chara = varData.CharacterList[charano];
				return chara.DataStringArray[VarCodeInt];
			}

			public override void CheckElement(Int64[] arguments, bool[] doCheck)
			{
				if (doCheck[0] && ((arguments[0] < 0) || (arguments[0] >= varData.CharacterList.Count)))
					throw new CodeEE("キャラクタ配列変数" + varName + "の第１引数(" + arguments[0].ToString() + ")はキャラ登録番号の範囲外です");
				//少しでも処理を少なく
				if (!doCheck[1])
					return;
				Int64 arraySize = doCheck[0] ? varData.CharacterList[(int)arguments[0]].DataStringArray[VarCodeInt].Length : GlobalStatic.ConstantData.DummyChara.DataStringArray[VarCodeInt].Length; ;
				if ((arguments[1] < 0) || (arguments[1] >= arraySize))
					throw new CodeEE("キャラクタ配列変数" + varName + "の第２引数(" + arguments[1].ToString() + ")は配列の範囲外です");
			}
			public override void IsArrayRangeValid(Int64[] arguments, Int64 index1, Int64 index2, string funcName, Int64 i1, Int64 i2)
			{
				CheckElement(arguments);
				CharacterData chara = varData.CharacterList[(int)arguments[0]];
				string[] array = chara.DataStringArray[VarCodeInt];
				if ((index1 < 0) || (index1 > array.Length))
					throw new CodeEE(funcName + "命令の第" + i1.ToString() + "引数(" + index1.ToString() + ")は配列" + varName + "の範囲外です");
				if ((index2 < 0) || (index2 > array.Length))
					throw new CodeEE(funcName + "命令の第" + i2.ToString() + "引数(" + index2.ToString() + ")は配列" + varName + "の範囲外です");
			}
		}

		private sealed class CharaInt2DVariableToken : VariableToken
		{
			public CharaInt2DVariableToken(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				CanRestructure = false;
			}

			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				CharacterData chara = varData.CharacterList[(int)arguments[0]];
				return chara.DataIntegerArray2D[VarCodeInt][arguments[1], arguments[2]];
			}

			public override void SetValue(Int64 value, Int64[] arguments)
			{
				CharacterData chara = varData.CharacterList[(int)arguments[0]];
				chara.DataIntegerArray2D[VarCodeInt][arguments[1], arguments[2]] = value;
			}

			public override void SetValue(Int64[] values, Int64[] arguments)
			{
				CharacterData chara = varData.CharacterList[(int)arguments[0]];
				Int64[,] array = chara.DataIntegerArray2D[VarCodeInt];
				int start = (int)arguments[2];
				int end = start + values.Length;
				int index1 = (int)arguments[1];
				for (int i = start; i < end; i++)
					array[index1, i] = values[i - start];
			}

			public override void SetValueAll(long value, int start, int end, int charaPos)
			{
				varData.characterList[charaPos].setValueAll2D(VarCodeInt, value);
				//CharacterData chara = varData.CharacterList[charaPos];
				//Int64[,] array = chara.DataIntegerArray2D[VarCodeInt];
				//int a1 = array.GetLength(0);
				//int a2 = array.GetLength(1);
				//for (int i = 0; i < a1; i++)
				//    for (int j = 0; j < a2; j++)
				//        array[i, j] = value;
			}

			public override Int64 PlusValue(Int64 value, Int64[] arguments)
			{
				CharacterData chara = varData.CharacterList[(int)arguments[0]];
				chara.DataIntegerArray2D[VarCodeInt][arguments[1], arguments[2]] += value;
				return chara.DataIntegerArray2D[VarCodeInt][arguments[1], arguments[2]];
			}
			public override Int32 GetLength()
			{ throw new CodeEE("2次元配列型変数" + varName + "の長さを取得しようとしました"); }
			public override Int32 GetLength(int dimension)
			{
				if (dimension == 0)
					return (int)(varData.Constant.CharacterIntArray2DLength[VarCodeInt] >> 32);
				if (dimension == 1)
					return (int)(varData.Constant.CharacterIntArray2DLength[VarCodeInt] & 0x7FFFFFFF);
				throw new CodeEE("配列型変数" + varName + "の存在しない次元の長さを取得しようとしました");
			}

			public override object GetArrayChara(int charano)
			{
				CharacterData chara = varData.CharacterList[charano];
				return chara.DataIntegerArray2D[VarCodeInt];
			}

			public override void CheckElement(Int64[] arguments, bool[] doCheck)
			{
				if (doCheck[0] && ((arguments[0] < 0) || (arguments[0] >= varData.CharacterList.Count)))
					throw new CodeEE("キャラクタ配列変数" + varName + "の第１引数(" + arguments[0].ToString() + ")はキャラ登録番号の範囲外です");
				//少しでも処理を少なく
				if (!doCheck[1] && !doCheck[2])
					return;
				Int64[,] array = doCheck[0] ? varData.CharacterList[(int)arguments[0]].DataIntegerArray2D[VarCodeInt] : GlobalStatic.ConstantData.DummyChara.DataIntegerArray2D[VarCodeInt];
				if (doCheck[1] && ((arguments[1] < 0) || (arguments[1] >= array.GetLength(0))))
					throw new CodeEE("キャラクタ配列変数" + varName + "の第２引数(" + arguments[1].ToString() + ")は配列の範囲外です");
				if (doCheck[2] && ((arguments[2] < 0) || (arguments[2] >= array.GetLength(1))))
					throw new CodeEE("キャラクタ配列変数" + varName + "の第３引数(" + arguments[1].ToString() + ")は配列の範囲外です");
			}
			public override void IsArrayRangeValid(Int64[] arguments, Int64 index1, Int64 index2, string funcName, Int64 i1, Int64 i2)
			{
				CheckElement(arguments);
				CharacterData chara = varData.CharacterList[(int)arguments[0]];
				Int64[,] array = chara.DataIntegerArray2D[VarCodeInt];
				if ((index1 < 0) || (index1 > array.GetLength(1)))
					throw new CodeEE(funcName + "命令の第" + i1.ToString() + "引数(" + index1.ToString() + ")は配列" + varName + "の範囲外です");
				if ((index2 < 0) || (index2 > array.GetLength(1)))
					throw new CodeEE(funcName + "命令の第" + i2.ToString() + "引数(" + index2.ToString() + ")は配列" + varName + "の範囲外です");
			}
		}

		private sealed class CharaStr2DVariableToken : VariableToken
		{
			public CharaStr2DVariableToken(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				CanRestructure = false;
			}

			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				CharacterData chara = varData.CharacterList[(int)arguments[0]];
				return chara.DataStringArray2D[VarCodeInt][arguments[1], arguments[2]];
			}

			public override void SetValue(string value, Int64[] arguments)
			{
				CharacterData chara = varData.CharacterList[(int)arguments[0]];
				chara.DataStringArray2D[VarCodeInt][arguments[1], arguments[2]] = value;
			}

			public override void SetValue(string[] values, Int64[] arguments)
			{
				CharacterData chara = varData.CharacterList[(int)arguments[0]];
				string[,] array = chara.DataStringArray2D[VarCodeInt];
				int start = (int)arguments[2];
				int end = start + values.Length;
				int index1 = (int)arguments[1];
				for (int i = start; i < end; i++)
					array[index1, i] = values[i - start];
			}

			public override void SetValueAll(string value, int start, int end, int charaPos)
			{
				varData.characterList[charaPos].setValueAll2D(VarCodeInt, value);
				//CharacterData chara = varData.CharacterList[charaPos];
				//String[,] array = chara.DataStringArray2D[VarCodeInt];
				//int a1 = array.GetLength(0);
				//int a2 = array.GetLength(1);
				//for (int i = 0; i < a1; i++)
				//    for (int j = 0; j < a2; j++)
				//        array[i, j] = value;
			}

			public override Int32 GetLength()
			{ throw new CodeEE("2次元配列型変数" + varName + "の長さを取得しようとしました"); }
			public override Int32 GetLength(int dimension)
			{
				if (dimension == 0)
					return (int)(varData.Constant.CharacterStrArray2DLength[VarCodeInt] >> 32);
				if (dimension == 1)
					return (int)(varData.Constant.CharacterStrArray2DLength[VarCodeInt] & 0x7FFFFFFF);
				throw new CodeEE("配列型変数" + varName + "の存在しない次元の長さを取得しようとしました");
			}

			public override object GetArrayChara(int charano)
			{
				CharacterData chara = varData.CharacterList[charano];
				return chara.DataStringArray2D[VarCodeInt];
			}

			public override void CheckElement(Int64[] arguments, bool[] doCheck)
			{
				if (doCheck[0] && ((arguments[0] < 0) || (arguments[0] >= varData.CharacterList.Count)))
					throw new CodeEE("キャラクタ配列変数" + varName + "の第１引数(" + arguments[0].ToString() + ")はキャラ登録番号の範囲外です");
				//少しでも処理を少なく
				if (!doCheck[1] && !doCheck[2])
					return;
				string[,] array = doCheck[0] ? varData.CharacterList[(int)arguments[0]].DataStringArray2D[VarCodeInt] : GlobalStatic.ConstantData.DummyChara.DataStringArray2D[VarCodeInt];
				if (doCheck[1] && ((arguments[1] < 0) || (arguments[1] >= array.GetLength(0))))
					throw new CodeEE("キャラクタ配列変数" + varName + "の第２引数(" + arguments[1].ToString() + ")は配列の範囲外です");
				if (doCheck[2] && ((arguments[2] < 0) || (arguments[2] >= array.GetLength(1))))
					throw new CodeEE("キャラクタ配列変数" + varName + "の第３引数(" + arguments[1].ToString() + ")は配列の範囲外です");
			}
			public override void IsArrayRangeValid(Int64[] arguments, Int64 index1, Int64 index2, string funcName, Int64 i1, Int64 i2)
			{
				CheckElement(arguments);
				CharacterData chara = varData.CharacterList[(int)arguments[0]];
				string[,] array = chara.DataStringArray2D[VarCodeInt];
				if ((index1 < 0) || (index1 > array.GetLength(1)))
					throw new CodeEE(funcName + "命令の第" + i1.ToString() + "引数(" + index1.ToString() + ")は配列" + varName + "の範囲外です");
				if ((index2 < 0) || (index2 > array.GetLength(1)))
					throw new CodeEE(funcName + "命令の第" + i2.ToString() + "引数(" + index2.ToString() + ")は配列" + varName + "の範囲外です");
			}
		}
		#endregion
		#region 定数
		private abstract class ConstantToken : VariableToken
		{
			public ConstantToken(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				CanRestructure = true;
			}
			public override void SetValue(Int64 value, Int64[] arguments)
			{ throw new CodeEE("読み取り専用の変数" + varName + "に代入しようとしました"); }
			public override void SetValue(string value, Int64[] arguments)
			{ throw new CodeEE("読み取り専用の変数" + varName + "に代入しようとしました"); }
			public override void SetValue(Int64[] values, Int64[] arguments)
			{ throw new CodeEE("読み取り専用の変数" + varName + "に代入しようとしました"); }
			public override void SetValue(string[] values, Int64[] arguments)
			{ throw new CodeEE("読み取り専用の変数" + varName + "に代入しようとしました"); }
			public override Int64 PlusValue(Int64 value, Int64[] arguments)
			{ throw new CodeEE("読み取り専用の変数" + varName + "に代入しようとしました"); }
		}

		private sealed class IntConstantToken : ConstantToken
		{
			public IntConstantToken(VariableCode varCode, VariableData varData, Int64 i)
				: base(varCode, varData)
			{
				this.i = i;
			}
			Int64 i;
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				return i;
			}
		}
		private sealed class StrConstantToken : ConstantToken
		{
			public StrConstantToken(VariableCode varCode, VariableData varData, string s)
				: base(varCode, varData)
			{
				this.s = s;
			}
			string s;
			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				return s;
			}
		}
		private sealed class Int1DConstantToken : ConstantToken
		{
			public Int1DConstantToken(VariableCode varCode, VariableData varData, Int64[] array)
				: base(varCode, varData)
			{
				this.array = array;
			}
			Int64[] array = null;
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				return array[arguments[0]];
			}
			public override Int32 GetLength()
			{ return array.Length; }
			public override Int32 GetLength(int dimension)
			{
				if (dimension == 0)
					return array.Length;
				throw new CodeEE("配列型変数" + varName + "の存在しない次元の長さを取得しようとしました");
			}
			public override object GetArray() { return array; }

			public override void CheckElement(Int64[] arguments, bool[] doCheck)
			{
				if (doCheck[0] && ((arguments[0] < 0) || (arguments[0] >= array.Length)))
					throw new CodeEE("配列変数" + varName + "の第１引数(" + arguments[0].ToString() + ")は配列の範囲外です");
			}
			public override void IsArrayRangeValid(Int64[] arguments, Int64 index1, Int64 index2, string funcName, Int64 i1, Int64 i2)
			{
				CheckElement(arguments);
				if ((index1 < 0) || (index1 > array.Length))
					throw new CodeEE(funcName + "命令の第" + i1.ToString() + "引数(" + index1.ToString() + ")は配列" + varName + "の範囲外です");
				if ((index2 < 0) || (index2 > array.Length))
					throw new CodeEE(funcName + "命令の第" + i2.ToString() + "引数(" + index2.ToString() + ")は配列" + varName + "の範囲外です");
			}
		}

		private sealed class Str1DConstantToken : ConstantToken
		{
			public Str1DConstantToken(VariableCode varCode, VariableData varData, string[] array)
				: base(varCode, varData)
			{
				this.array = array;
			}
			public Str1DConstantToken(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				this.array = varData.constant.GetCsvNameList(varCode);
			}

			string[] array = null;
			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				return array[arguments[0]];
			}
			public override Int32 GetLength()
			{ return array.Length; }
			public override Int32 GetLength(int dimension)
			{
				if (dimension == 0)
					return array.Length;
				throw new CodeEE("配列型変数" + varName + "の存在しない次元の長さを取得しようとしました");
			}
			public override object GetArray() { return array; }

			public override void CheckElement(Int64[] arguments, bool[] doCheck)
			{
				if (doCheck[0] && ((arguments[0] < 0) || (arguments[0] >= array.Length)))
					throw new CodeEE("配列変数" + varName + "の第１引数(" + arguments[0].ToString() + ")は配列の範囲外です");
			}
			public override void IsArrayRangeValid(Int64[] arguments, Int64 index1, Int64 index2, string funcName, Int64 i1, Int64 i2)
			{
				CheckElement(arguments);
				if ((index1 < 0) || (index1 > array.Length))
					throw new CodeEE(funcName + "命令の第" + i1.ToString() + "引数(" + index1.ToString() + ")は配列" + varName + "の範囲外です");
				if ((index2 < 0) || (index2 > array.Length))
					throw new CodeEE(funcName + "命令の第" + i2.ToString() + "引数(" + index2.ToString() + ")は配列" + varName + "の範囲外です");
			}
		}

		#endregion
		#region 特殊処理

		private abstract class PseudoVariableToken : VariableToken
		{
			protected PseudoVariableToken(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				CanRestructure = false;
			}
			public override void SetValue(Int64 value, Int64[] arguments)
			{ throw new CodeEE("擬似変数" + varName + "に代入しようとしました"); }
			public override void SetValue(string value, Int64[] arguments)
			{ throw new CodeEE("擬似変数" + varName + "に代入しようとしました"); }
			public override void SetValue(Int64[] values, Int64[] arguments)
			{ throw new CodeEE("擬似変数" + varName + "に代入しようとしました"); }
			public override void SetValue(string[] values, Int64[] arguments)
			{ throw new CodeEE("擬似変数" + varName + "に代入しようとしました"); }
			public override Int64 PlusValue(Int64 value, Int64[] arguments)
			{ throw new CodeEE("擬似変数" + varName + "に代入しようとしました"); }
			public override Int32 GetLength()
			{ throw new CodeEE("擬似変数" + varName + "の長さを取得しようとしました"); }
			public override Int32 GetLength(int dimension)
			{ throw new CodeEE("擬似変数" + varName + "の長さを取得しようとしました"); }
			public override object GetArray()
			{ throw new CodeEE("擬似変数" + varName + "の配列を取得しようとしました"); }
		}


		private sealed class RandToken : PseudoVariableToken
		{
			public RandToken(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
			}
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				Int64 i = arguments[0];
				if (i <= 0)
					throw new CodeEE("RANDの引数に0以下の値(" + i.ToString() + ")が指定されました");
				return exm.VEvaluator.GetNextRand(i);
			}
		}
		private sealed class CompatiRandToken : PseudoVariableToken
		{
			public CompatiRandToken(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
			}
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				Int64 i = arguments[0];
				if (i == 0)
					return 0L;
				else if (i < 0)
					i = -i;
				return exm.VEvaluator.GetNextRand(32768) % i;//0-32767の乱数を引数で除算した余り
			}
		}

		private sealed class CHARANUM_Token : PseudoVariableToken
		{
			public CHARANUM_Token(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
			}
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				return varData.CharacterList.Count;
			}
		}

		private sealed class LASTLOAD_TEXT_Token : PseudoVariableToken
		{
			public LASTLOAD_TEXT_Token(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
			}
			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				return varData.LastLoadText;
			}
		}

		private sealed class LASTLOAD_VERSION_Token : PseudoVariableToken
		{
			public LASTLOAD_VERSION_Token(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
			}
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				return varData.LastLoadVersion;
			}
		}

		private sealed class LASTLOAD_NO_Token : PseudoVariableToken
		{
			public LASTLOAD_NO_Token(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
			}
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				return varData.LastLoadNo;
			}
		}
		private sealed class LINECOUNT_Token : PseudoVariableToken
		{
			public LINECOUNT_Token(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
			}
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				return exm.Console.LineCount;
			}
		}

		private sealed class WINDOW_TITLE_Token : VariableToken
		{
			public WINDOW_TITLE_Token(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				CanRestructure = false;
			}
			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				return GlobalStatic.Console.GetWindowTitle();
			}
			public override void SetValue(string value, Int64[] arguments)
			{
				GlobalStatic.Console.SetWindowTitle(value);
			}
		}

		private sealed class MONEYLABEL_Token : VariableToken
		{
			public MONEYLABEL_Token(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				CanRestructure = true;
			}
			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				return Config.MoneyLabel;
			}
		}

        private sealed class DRAWLINESTR_Token : VariableToken
        {
            public DRAWLINESTR_Token(VariableCode varCode, VariableData varData)
                : base(varCode, varData)
            {
                CanRestructure = true;
            }
            public override string GetStrValue(ExpressionMediator exm, long[] arguments)
            {
                return exm.Console.getDefStBar();
            }
        }

		private sealed class EmptyStrToken : PseudoVariableToken
		{
			public EmptyStrToken(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				CanRestructure = true;
			}
			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				return "";
			}
		}
		private sealed class EmptyIntToken : PseudoVariableToken
		{
			public EmptyIntToken(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				CanRestructure = true;
			}
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				return 0L;
			}
		}

		private sealed class Debug__FILE__Token : PseudoVariableToken
		{
			public Debug__FILE__Token(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				CanRestructure = true;
			}
			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				LogicalLine line = exm.Process.GetScaningLine();
				if ((line == null) || (line.Position == null))
					return "";
				return line.Position.Filename;
			}
		}

		private sealed class Debug__FUNCTION__Token : PseudoVariableToken
		{
			public Debug__FUNCTION__Token(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				CanRestructure = true;
			}
			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				LogicalLine line = exm.Process.GetScaningLine();
				if ((line == null) || (line.ParentLabelLine == null))
					return "";//システム待機中のデバッグモードから呼び出し
				return line.ParentLabelLine.LabelName;
			}
		}
		private sealed class Debug__LINE__Token : PseudoVariableToken
		{
			public Debug__LINE__Token(VariableCode varCode, VariableData varData)
				: base(varCode, varData)
			{
				CanRestructure = true;
			}
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				LogicalLine line = exm.Process.GetScaningLine();
				if ((line == null) || (line.Position == null))
					return -1L;
				return line.Position.LineNo;
			}
		}

        private sealed class ISTIMEOUTToken : PseudoVariableToken
        {
            public ISTIMEOUTToken(VariableCode varCode, VariableData varData)
                :base(varCode, varData)
            {
                CanRestructure = false;
            }
            public override long GetIntValue(ExpressionMediator exm, long[] arguments)
            {
                return Convert.ToInt64(GlobalStatic.Console.IsTimeOut);
            }
        }

        private sealed class __INT_MAX__Token : PseudoVariableToken
        {
            public __INT_MAX__Token(VariableCode varCode, VariableData varData)
                :base(varCode, varData)
            {
                CanRestructure = true;
            }
            public override long GetIntValue(ExpressionMediator exm, long[] arguments)
            {
                return Int64.MaxValue;
            }
        }
        private sealed class __INT_MIN__Token : PseudoVariableToken
        {
            public __INT_MIN__Token(VariableCode varCode, VariableData varData)
                : base(varCode, varData)
            {
                CanRestructure = true;
            }
            public override long GetIntValue(ExpressionMediator exm, long[] arguments)
            {
                return Int64.MinValue;
            }
        }
        #endregion
		#region LOCAL


		private sealed class LocalInt1DVariableToken : LocalVariableToken
		{
			public LocalInt1DVariableToken(VariableCode varCode, VariableData varData, string subId, int size)
				: base(varCode, varData, subId, size)
			{
			}
			Int64[] array = null;

			public override void SetDefault()
			{
				if (array != null)
					Array.Clear(array, 0, size);
			}

			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				if (array == null)
					array = new Int64[size];
				return array[arguments[0]];
			}

			public override void SetValue(Int64 value, Int64[] arguments)
			{
				if (array == null)
					array = new Int64[size];
				array[arguments[0]] = value;
			}

			public override void SetValue(Int64[] values, Int64[] arguments)
			{
				if (array == null)
					array = new Int64[size];
				int start = (int)arguments[0];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					array[i] = values[i - start];
			}

			public override void SetValueAll(long value, int start, int end, int charaPos)
			{
				if (array == null)
					array = new Int64[size];
				for (int i = start; i < end; i++)
					array[i] = value;
			}

			public override Int64 PlusValue(Int64 value, Int64[] arguments)
			{
				if (array == null)
					array = new Int64[size];
				array[arguments[0]] += value;
				return array[arguments[0]];
			}

			public override object GetArray()
			{
				if (array == null)
					array = new Int64[size];
				return array;
			}

			public override void resize(int newSize)
			{
				this.size = newSize;
				array = null;
			}
		}

		private sealed class LocalStr1DVariableToken : LocalVariableToken
		{
			public LocalStr1DVariableToken(VariableCode varCode, VariableData varData, string subId, int size)
				: base(varCode, varData, subId, size)
			{
			}
			string[] array = null;
			public override void SetDefault()
			{
				if (array != null)
					Array.Clear(array, 0, size);
			}

			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				if (array == null)
					array = new string[size];
				return array[arguments[0]];
			}

			public override void SetValue(string value, Int64[] arguments)
			{
				if (array == null)
					array = new string[size];
				array[arguments[0]] = value;
			}

			public override void SetValue(string[] values, Int64[] arguments)
			{
				if (array == null)
					array = new string[size];
				int start = (int)arguments[0];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					array[i] = values[i - start];
			}

			public override void SetValueAll(string value, int start, int end, int charaPos)
			{
				if (array == null)
					array = new string[size];
				for (int i = start; i < end; i++)
					array[i] = value;
			}

			public override object GetArray()
			{
				if (array == null)
					array = new string[size];
				return array;
			}

			public override void resize(int newSize)
			{
				this.size = newSize;
				array = null;
			}

		}

		#endregion
		#region userdef

		//1808beta009 廃止 private static と統合
		//private sealed class UserDefinedInt1DVariableToken : UserDefinedVariableToken

		#region static (広域変数とprivate static の両方を含む)
		private sealed class StaticInt1DVariableToken : UserDefinedVariableToken
		{
			public StaticInt1DVariableToken(UserDefinedVariableData data)
				: base(VariableCode.VAR, data)
			{
				CanRestructure = false;
				int[] sizes = data.Lengths;
				IsStatic = true;
				array = new Int64[sizes[0]];
			}
			Int64[] array = null;
			public override void SetDefault()
			{
				Array.Clear(array, 0, totalSize);
			}
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				return array[arguments[0]];
			}

			public override void SetValue(Int64 value, Int64[] arguments)
			{
				array[arguments[0]] = value;
			}

			public override void SetValue(Int64[] values, Int64[] arguments)
			{
				int start = (int)arguments[0];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					array[i] = values[i - start];
			}

			public override void SetValueAll(long value, int start, int end, int charaPos)
			{
				for (int i = start; i < end; i++)
					array[i] = value;
			}

			public override Int64 PlusValue(Int64 value, Int64[] arguments)
			{
				array[arguments[0]] += value;
				return array[arguments[0]];
			}
			public override object GetArray() { return array; }
			public override void In() { }
			public override void Out() { }

		}

		private sealed class StaticInt2DVariableToken : UserDefinedVariableToken
		{
			public StaticInt2DVariableToken(UserDefinedVariableData data)
				: base(VariableCode.VAR2D, data)
			{
				CanRestructure = false;
				int[] sizes = data.Lengths;
				IsStatic = true;
				array = new Int64[sizes[0], sizes[1]];
			}
			Int64[,] array = null;
			public override void SetDefault()
			{
				Array.Clear(array, 0, totalSize);
			}
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				return array[arguments[0], arguments[1]];
			}
			public override void SetValue(Int64 value, Int64[] arguments)
			{
				array[arguments[0], arguments[1]] = value;
			}

			public override void SetValue(Int64[] values, Int64[] arguments)
			{
				int start = (int)arguments[1];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					array[arguments[0], i] = values[i - start];
			}

			public override void SetValueAll(long value, int start, int end, int charaPos)
			{
				int a1 = array.GetLength(0);
				int a2 = array.GetLength(1);
				for (int i = 0; i < a1; i++)
					for (int j = 0; j < a2; j++)
						array[i, j] = value;
			}
			public override Int64 PlusValue(Int64 value, Int64[] arguments)
			{
				array[arguments[0], arguments[1]] += value;
				return array[arguments[0], arguments[1]];
			}
			public override object GetArray() { return array; }
			public override void In() { }
			public override void Out() { }

		}
		private sealed class StaticInt3DVariableToken : UserDefinedVariableToken
		{
			public StaticInt3DVariableToken(UserDefinedVariableData data)
				: base(VariableCode.VAR3D, data)
			{
				CanRestructure = false;
				int[] sizes = data.Lengths;
				IsStatic = true;
				array = new Int64[sizes[0], sizes[1], sizes[2]];
			}
			Int64[, ,] array = null;
			public override void SetDefault()
			{
				Array.Clear(array, 0, totalSize);
			}
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				return array[arguments[0], arguments[1], arguments[2]];
			}

			public override void SetValue(Int64 value, Int64[] arguments)
			{
				array[arguments[0], arguments[1], arguments[2]] = value;
			}
			public override void SetValue(Int64[] values, Int64[] arguments)
			{
				int start = (int)arguments[2];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					array[arguments[0], arguments[1], i] = values[i - start];
			}

			public override void SetValueAll(long value, int start, int end, int charaPos)
			{
				int a1 = array.GetLength(0);
				int a2 = array.GetLength(1);
				int a3 = array.GetLength(2);
				for (int i = 0; i < a1; i++)
					for (int j = 0; j < a2; j++)
						for (int k = 0; k < a3; k++)
							array[i, j, k] = value;
			}

			public override Int64 PlusValue(Int64 value, Int64[] arguments)
			{
				array[arguments[0], arguments[1], arguments[2]] += value;
				return array[arguments[0], arguments[1], arguments[2]];
			}
			public override object GetArray() { return array; }
			public override void In() { }
			public override void Out() { }

		}
		private sealed class StaticStr1DVariableToken : UserDefinedVariableToken
		{
			public StaticStr1DVariableToken(UserDefinedVariableData data)
				: base(VariableCode.VARS, data)
			{
				CanRestructure = false;
				int[] sizes = data.Lengths;
				IsStatic = true;
				array = new string[sizes[0]];
			}
			string[] array = null;
			public override void SetDefault()
			{
				Array.Clear(array, 0, totalSize);
			}
			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				return array[arguments[0]];
			}

			public override void SetValue(string value, Int64[] arguments)
			{
				array[arguments[0]] = value;
			}

			public override void SetValue(string[] values, Int64[] arguments)
			{
				int start = (int)arguments[0];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					array[i] = values[i - start];
			}
			public override void SetValueAll(string value, int start, int end, int charaPos)
			{
				for (int i = start; i < end; i++)
					array[i] = value;
			}
			public override object GetArray() { return array; }
			public override void In() { }
			public override void Out() { }
		}
		private sealed class StaticStr2DVariableToken : UserDefinedVariableToken
		{
			public StaticStr2DVariableToken(UserDefinedVariableData data)
				: base(VariableCode.VARS2D, data)
			{
				CanRestructure = false;
				int[] sizes = data.Lengths;
				IsStatic = true;
				array = new string[sizes[0], sizes[1]];
			}
			string[,] array = null;
			public override void SetDefault()
			{
				Array.Clear(array, 0, totalSize);
			}
			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				return array[arguments[0], arguments[1]];
			}

			public override void SetValue(string value, Int64[] arguments)
			{
				array[arguments[0], arguments[1]] = value;
			}

			public override void SetValue(string[] values, Int64[] arguments)
			{
				int start = (int)arguments[1];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					array[arguments[0], i] = values[i - start];
			}
			public override void SetValueAll(string value, int start, int end, int charaPos)
			{
				int a1 = array.GetLength(0);
				int a2 = array.GetLength(1);
				for (int i = 0; i < a1; i++)
					for (int j = 0; j < a2; j++)
						array[i, j] = value;
			}
			public override object GetArray() { return array; }
			public override void In() { }
			public override void Out() { }
		}

		private sealed class StaticStr3DVariableToken : UserDefinedVariableToken
		{
			public StaticStr3DVariableToken(UserDefinedVariableData data)
				: base(VariableCode.VARS3D, data)
			{
				CanRestructure = false;
				int[] sizes = data.Lengths;
				IsStatic = true;
				array = new string[sizes[0], sizes[1], sizes[2]];
			}
			string[, ,] array = null;
			public override void SetDefault()
			{
				Array.Clear(array, 0, totalSize);
			}
			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				return array[arguments[0], arguments[1], arguments[2]];
			}

			public override void SetValue(string value, Int64[] arguments)
			{
				array[arguments[0], arguments[1], arguments[2]] = value;
			}

			public override void SetValue(string[] values, Int64[] arguments)
			{
				int start = (int)arguments[2];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					array[arguments[0], arguments[1], i] = values[i - start];
			}
			public override void SetValueAll(string value, int start, int end, int charaPos)
			{
				int a1 = array.GetLength(0);
				int a2 = array.GetLength(1);
				int a3 = array.GetLength(2);
				for (int i = 0; i < a1; i++)
					for (int j = 0; j < a2; j++)
						for (int k = 0; k < a3; k++)
							array[i, j, k] = value;
			}
			public override object GetArray() { return array; }
			public override void In() { }
			public override void Out() { }
		}
		#endregion
		#region private dynamic

		private sealed class PrivateInt1DVariableToken : UserDefinedVariableToken
		{
			public PrivateInt1DVariableToken(UserDefinedVariableData data)
				: base(VariableCode.VAR, data)
			{
				CanRestructure = false;
				int[] sizes = data.Lengths;
				IsStatic = false;
				arrayList = new List<Int64[]>();
			}
			readonly List<Int64[]> arrayList = null;
			Int64[] array = null;
			//int counter = 0;
			public override void SetDefault()
			{
			}
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				return array[arguments[0]];
			}

			public override void SetValue(Int64 value, Int64[] arguments)
			{
				array[arguments[0]] = value;
			}

			public override void SetValue(Int64[] values, Int64[] arguments)
			{
				int start = (int)arguments[0];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					array[i] = values[i - start];
			}

			public override void SetValueAll(long value, int start, int end, int charaPos)
			{
				for (int i = start; i < end; i++)
					array[i] = value;
			}

			public override Int64 PlusValue(Int64 value, Int64[] arguments)
			{
				array[arguments[0]] += value;
				return array[arguments[0]];
			}
			public override object GetArray() { return array; }

			public override void In()
			{
				if (array != null)
					arrayList.Add(array);
				//counter++;
				array = new Int64[sizes[0]];
			}

			public override void Out()
			{
				//counter--;
				//arrayList.RemoveAt(arrayList.Count - 1);
				if (arrayList.Count > 0)
				{
					array = arrayList[arrayList.Count - 1];
					arrayList.RemoveAt(arrayList.Count - 1);
				}
				else
					array = null;
			}
		}
		private sealed class PrivateInt2DVariableToken : UserDefinedVariableToken
		{
			public PrivateInt2DVariableToken(UserDefinedVariableData data)
				: base(VariableCode.VAR2D, data)
			{
				CanRestructure = false;
				int[] sizes = data.Lengths;
				IsStatic = false;
				arrayList = new List<Int64[,]>();
			}
			readonly List<Int64[,]> arrayList = null;
			Int64[,] array = null;
			//int counter = 0;
			public override void SetDefault() { }
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				return array[arguments[0], arguments[1]];
			}
			public override void SetValue(Int64 value, Int64[] arguments)
			{
				array[arguments[0], arguments[1]] = value;
			}

			public override void SetValue(Int64[] values, Int64[] arguments)
			{
				int start = (int)arguments[1];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					array[arguments[0], i] = values[i - start];
			}

			public override void SetValueAll(long value, int start, int end, int charaPos)
			{
				int a1 = array.GetLength(0);
				int a2 = array.GetLength(1);
				for (int i = 0; i < a1; i++)
					for (int j = 0; j < a2; j++)
						array[i, j] = value;
			}
			public override Int64 PlusValue(Int64 value, Int64[] arguments)
			{
				array[arguments[0], arguments[1]] += value;
				return array[arguments[0], arguments[1]];
			}
			public override object GetArray() { return array; }

			public override void In()
			{
				if (array != null)
					arrayList.Add(array);
				//counter++;
				array = new Int64[sizes[0], sizes[1]];
			}

			public override void Out()
			{
				//counter--;
				//arrayList.RemoveAt(arrayList.Count - 1);
				if (arrayList.Count > 0)
				{
					array = arrayList[arrayList.Count - 1];
					arrayList.RemoveAt(arrayList.Count - 1);
				}
				else
					array = null;
			}
		}
		private sealed class PrivateInt3DVariableToken : UserDefinedVariableToken
		{
			public PrivateInt3DVariableToken(UserDefinedVariableData data)
				: base(VariableCode.VAR3D, data)
			{
				CanRestructure = false;
				int[] sizes = data.Lengths;
				IsStatic = false;
				arrayList = new List<Int64[, ,]>();
			}
			readonly List<Int64[, ,]> arrayList = null;
			Int64[, ,] array = null;
			//int counter = 0;
			public override void SetDefault() { }
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				return array[arguments[0], arguments[1], arguments[2]];
			}

			public override void SetValue(Int64 value, Int64[] arguments)
			{
				array[arguments[0], arguments[1], arguments[2]] = value;
			}
			public override void SetValue(Int64[] values, Int64[] arguments)
			{
				int start = (int)arguments[2];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					array[arguments[0], arguments[1], i] = values[i - start];
			}

			public override void SetValueAll(long value, int start, int end, int charaPos)
			{
				int a1 = array.GetLength(0);
				int a2 = array.GetLength(1);
				int a3 = array.GetLength(2);
				for (int i = 0; i < a1; i++)
					for (int j = 0; j < a2; j++)
						for (int k = 0; k < a3; k++)
							array[i, j, k] = value;
			}

			public override Int64 PlusValue(Int64 value, Int64[] arguments)
			{
				array[arguments[0], arguments[1], arguments[2]] += value;
				return array[arguments[0], arguments[1], arguments[2]];
			}
			public override object GetArray() { return array; }

			public override void In()
			{
				if (array != null)
					arrayList.Add(array);
				//counter++;
				array = new Int64[sizes[0], sizes[1], sizes[2]];
			}

			public override void Out()
			{
				//counter--;
				//arrayList.RemoveAt(arrayList.Count - 1);
				if (arrayList.Count > 0)
				{
					array = arrayList[arrayList.Count - 1];
					arrayList.RemoveAt(arrayList.Count - 1);
				}
				else
					array = null;
			}
		}

		private sealed class PrivateStr1DVariableToken : UserDefinedVariableToken
		{
			public PrivateStr1DVariableToken(UserDefinedVariableData data)
				: base(VariableCode.VARS, data)
			{
				CanRestructure = false;
				int[] sizes = data.Lengths;
				IsStatic = false;
				arrayList = new List<string[]>();
			}
			//int counter = 0;
			readonly List<string[]> arrayList = null;
			string[] array = null;
			public override void SetDefault()
			{
			}
			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				return array[arguments[0]];
			}

			public override void SetValue(string value, Int64[] arguments)
			{
				array[arguments[0]] = value;
			}

			public override void SetValue(string[] values, Int64[] arguments)
			{
				int start = (int)arguments[0];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					array[i] = values[i - start];
			}
			public override void SetValueAll(string value, int start, int end, int charaPos)
			{
				for (int i = start; i < end; i++)
					array[i] = value;
			}
			public override object GetArray() { return array; }
			public override void In()
			{
				//counter++;
				if (array != null)
					arrayList.Add(array);
				array = new string[sizes[0]];
				//arrayList.Add(array);
			}

			public override void Out()
			{
				//counter--;
				//arrayList.RemoveAt(arrayList.Count - 1);
				if (arrayList.Count > 0)
				{
					array = arrayList[arrayList.Count - 1];
					arrayList.RemoveAt(arrayList.Count - 1);
				}
				else
					array = null;
			}
		}

		private sealed class PrivateStr2DVariableToken : UserDefinedVariableToken
		{
			public PrivateStr2DVariableToken(UserDefinedVariableData data)
				: base(VariableCode.VARS2D, data)
			{
				CanRestructure = false;
				int[] sizes = data.Lengths;
				IsStatic = false;
				arrayList = new List<string[,]>();
			}
			//int counter = 0;
			readonly List<string[,]> arrayList = null;
			string[,] array = null;
			public override void SetDefault()
			{
			}

			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				return array[arguments[0], arguments[1]];
			}

			public override void SetValue(string value, Int64[] arguments)
			{
				array[arguments[0], arguments[1]] = value;
			}

			public override void SetValue(string[] values, Int64[] arguments)
			{
				int start = (int)arguments[1];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					array[arguments[0], i] = values[i - start];
			}
			public override void SetValueAll(string value, int start, int end, int charaPos)
			{
				int a1 = array.GetLength(0);
				int a2 = array.GetLength(1);
				for (int i = 0; i < a1; i++)
					for (int j = 0; j < a2; j++)
						array[i, j] = value;
			}
			public override object GetArray() { return array; }
			public override void In()
			{
				//counter++;
				if (array != null)
					arrayList.Add(array);
				array = new string[sizes[0], sizes[1]];
				//arrayList.Add(array);
			}

			public override void Out()
			{
				//counter--;
				//arrayList.RemoveAt(arrayList.Count - 1);
				if (arrayList.Count > 0)
				{
					array = arrayList[arrayList.Count - 1];
					arrayList.RemoveAt(arrayList.Count - 1);
				}
				else
					array = null;
			}
		}

		private sealed class PrivateStr3DVariableToken : UserDefinedVariableToken
		{
			public PrivateStr3DVariableToken(UserDefinedVariableData data)
				: base(VariableCode.VARS3D, data)
			{
				CanRestructure = false;
				int[] sizes = data.Lengths;
				IsStatic = false;
				arrayList = new List<string[, ,]>();
			}
			//int counter = 0;
			readonly List<string[, ,]> arrayList = null;
			string[, ,] array = null;
			public override void SetDefault() { }

			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				return array[arguments[0], arguments[1], arguments[2]];
			}

			public override void SetValue(string value, Int64[] arguments)
			{
				array[arguments[0], arguments[1], arguments[2]] = value;
			}

			public override void SetValue(string[] values, Int64[] arguments)
			{
				int start = (int)arguments[2];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					array[arguments[0], arguments[1], i] = values[i - start];
			}
			public override void SetValueAll(string value, int start, int end, int charaPos)
			{
				int a1 = array.GetLength(0);
				int a2 = array.GetLength(1);
				int a3 = array.GetLength(2);
				for (int i = 0; i < a1; i++)
					for (int j = 0; j < a2; j++)
						for (int k = 0; k < a3; k++)
							array[i, j, k] = value;
			}
			public override object GetArray() { return array; }
			public override void In()
			{
				//counter++;
				if (array != null)
					arrayList.Add(array);
				array = new string[sizes[0], sizes[1], sizes[2]];
				//arrayList.Add(array);
			}

			public override void Out()
			{
				//counter--;
				//arrayList.RemoveAt(arrayList.Count - 1);
				if (arrayList.Count > 0)
				{
					array = arrayList[arrayList.Count - 1];
					arrayList.RemoveAt(arrayList.Count - 1);
				}
				else
					array = null;
			}
		}


		#endregion
		#region ref
		//1808beta009で追加
		/// <summary>
		/// public staticとprivate dynamicをクラスレベルでは区別しない
		/// 1808beta009時点ではprivate dynamicのみ
		/// </summary>
		private sealed class ReferenceInt1DToken : ReferenceToken
		{
			public ReferenceInt1DToken(UserDefinedVariableData data)
				: base(VariableCode.REF, data)
			{
				CanRestructure = false;
				IsStatic = !data.Private;
			}
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				if (array == null)
					throw new CodeEE("参照型変数" + varName + "は何も参照していません");
				return ((Int64[])array)[arguments[0]];
			}

			public override void SetValue(Int64 value, Int64[] arguments)
			{
				if (array == null)
					throw new CodeEE("参照型変数" + varName + "は何も参照していません");
				((Int64[])array)[arguments[0]] = value;
			}

			public override void SetValue(Int64[] values, Int64[] arguments)
			{
				if (array == null)
					throw new CodeEE("参照型変数" + varName + "は何も参照していません");
				int start = (int)arguments[0];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					((Int64[])array)[i] = values[i - start];
			}

			public override void SetValueAll(long value, int start, int end, int charaPos)
			{
				if (array == null)
					throw new CodeEE("参照型変数" + varName + "は何も参照していません");
				for (int i = start; i < end; i++)
					((Int64[])array)[i] = value;
			}

			public override Int64 PlusValue(Int64 value, Int64[] arguments)
			{
				if (array == null)
					throw new CodeEE("参照型変数" + varName + "は何も参照していません");
				((Int64[])array)[arguments[0]] += value;
				return ((Int64[])array)[arguments[0]];
			}

		}

		private sealed class ReferenceInt2DToken : ReferenceToken
		{
			public ReferenceInt2DToken(UserDefinedVariableData data)
				: base(VariableCode.REF2D, data)
			{
				CanRestructure = false;
				IsStatic = !data.Private;
			}
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				if (array == null)
					throw new CodeEE("参照型変数" + varName + "は何も参照していません");
				return ((Int64[,])array)[arguments[0], arguments[1]];
			}

			public override void SetValue(Int64 value, Int64[] arguments)
			{
				if (array == null)
					throw new CodeEE("参照型変数" + varName + "は何も参照していません");
				((Int64[,])array)[arguments[0], arguments[1]] = value;
			}

			public override void SetValue(Int64[] values, Int64[] arguments)
			{
				int start = (int)arguments[2];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					((Int64[,])array)[arguments[0], i] = values[i - start];
			}

			public override void SetValueAll(long value, int start, int end, int charaPos)
			{
				int a1 = array.GetLength(0);
				int a2 = array.GetLength(1);
				for (int i = 0; i < a1; i++)
					for (int j = 0; j < a2; j++)
						((Int64[,])array)[i, j] = value;
			}


			public override Int64 PlusValue(Int64 value, Int64[] arguments)
			{
				if (array == null)
					throw new CodeEE("参照型変数" + varName + "は何も参照していません");
				((Int64[,])array)[arguments[0], arguments[1]] += value;
				return ((Int64[,])array)[arguments[0], arguments[1]];
			}
		}

		private sealed class ReferenceInt3DToken : ReferenceToken
		{
			public ReferenceInt3DToken(UserDefinedVariableData data)
				: base(VariableCode.REF3D, data)
			{
				CanRestructure = false;
				IsStatic = !data.Private;
			}
			public override Int64 GetIntValue(ExpressionMediator exm, Int64[] arguments)
			{
				if (array == null)
					throw new CodeEE("参照型変数" + varName + "は何も参照していません");
				return ((Int64[, ,])array)[arguments[0], arguments[1], arguments[2]];
			}

			public override void SetValue(Int64 value, Int64[] arguments)
			{
				if (array == null)
					throw new CodeEE("参照型変数" + varName + "は何も参照していません");
				((Int64[, ,])array)[arguments[0], arguments[1], arguments[2]] = value;
			}

			public override void SetValue(Int64[] values, Int64[] arguments)
			{
				int start = (int)arguments[2];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					((Int64[, ,])array)[arguments[0], arguments[1], i] = values[i - start];
			}

			public override void SetValueAll(long value, int start, int end, int charaPos)
			{
				int a1 = array.GetLength(0);
				int a2 = array.GetLength(1);
				int a3 = array.GetLength(2);
				for (int i = 0; i < a1; i++)
					for (int j = 0; j < a2; j++)
						for (int k = 0; k < a3; k++)
							((Int64[, ,])array)[i, j, k] = value;
			}


			public override Int64 PlusValue(Int64 value, Int64[] arguments)
			{
				if (array == null)
					throw new CodeEE("参照型変数" + varName + "は何も参照していません");
				((Int64[, ,])array)[arguments[0], arguments[1], arguments[2]] += value;
				return ((Int64[, ,])array)[arguments[0], arguments[1], arguments[2]];
			}

		}
		private sealed class ReferenceStr1DToken : ReferenceToken
		{
			public ReferenceStr1DToken(UserDefinedVariableData data)
				: base(VariableCode.REFS, data)
			{
				CanRestructure = false;
				IsStatic = !data.Private;
			}
			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				if (array == null)
					throw new CodeEE("参照型変数" + varName + "は何も参照していません");
				return ((string[])array)[arguments[0]];
			}

			public override void SetValue(string value, Int64[] arguments)
			{
				if (array == null)
					throw new CodeEE("参照型変数" + varName + "は何も参照していません");
				((string[])array)[arguments[0]] = value;
			}

			public override void SetValue(string[] values, Int64[] arguments)
			{
				if (array == null)
					throw new CodeEE("参照型変数" + varName + "は何も参照していません");
				int start = (int)arguments[0];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					((string[])array)[i] = values[i - start];
			}

			public override void SetValueAll(string value, int start, int end, int charaPos)
			{
				if (array == null)
					throw new CodeEE("参照型変数" + varName + "は何も参照していません");
				for (int i = start; i < end; i++)
					((string[])array)[i] = value;
			}
		}

		private sealed class ReferenceStr2DToken : ReferenceToken
		{
			public ReferenceStr2DToken(UserDefinedVariableData data)
				: base(VariableCode.REFS2D, data)
			{
				CanRestructure = false;
				IsStatic = !data.Private;
			}
			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				if (array == null)
					throw new CodeEE("参照型変数" + varName + "は何も参照していません");
				return ((string[,])array)[arguments[0], arguments[1]];
			}

			public override void SetValue(string value, Int64[] arguments)
			{
				if (array == null)
					throw new CodeEE("参照型変数" + varName + "は何も参照していません");
				((string[,])array)[arguments[0], arguments[1]] = value;
			}

			public override void SetValue(string[] values, Int64[] arguments)
			{
				int start = (int)arguments[2];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					((string[,])array)[arguments[0], i] = values[i - start];
			}

			public override void SetValueAll(string value, int start, int end, int charaPos)
			{
				int a1 = array.GetLength(0);
				int a2 = array.GetLength(1);
				for (int i = 0; i < a1; i++)
					for (int j = 0; j < a2; j++)
						((string[,])array)[i, j] = value;
			}
		}

		private sealed class ReferenceStr3DToken : ReferenceToken
		{
			public ReferenceStr3DToken(UserDefinedVariableData data)
				: base(VariableCode.REFS3D, data)
			{
				CanRestructure = false;
				IsStatic = !data.Private;
			}
			public override string GetStrValue(ExpressionMediator exm, Int64[] arguments)
			{
				if (array == null)
					throw new CodeEE("参照型変数" + varName + "は何も参照していません");
				return ((string[, ,])array)[arguments[0], arguments[1], arguments[2]];
			}

			public override void SetValue(string value, Int64[] arguments)
			{
				if (array == null)
					throw new CodeEE("参照型変数" + varName + "は何も参照していません");
				((string[, ,])array)[arguments[0], arguments[1], arguments[2]] = value;
			}

			public override void SetValue(string[] values, Int64[] arguments)
			{
				int start = (int)arguments[2];
				int end = start + values.Length;
				for (int i = start; i < end; i++)
					((string[, ,])array)[arguments[0], arguments[1], i] = values[i - start];
			}

			public override void SetValueAll(string value, int start, int end, int charaPos)
			{
				int a1 = array.GetLength(0);
				int a2 = array.GetLength(1);
				int a3 = array.GetLength(2);
				for (int i = 0; i < a1; i++)
					for (int j = 0; j < a2; j++)
						for (int k = 0; k < a3; k++)
							((string[, ,])array)[i, j, k] = value;
			}

		}
		#endregion
		#endregion
	}
}