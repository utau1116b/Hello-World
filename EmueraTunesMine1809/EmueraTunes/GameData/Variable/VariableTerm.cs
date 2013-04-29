﻿using System;
using System.Collections.Generic;
using System.Text;
using MinorShift.Emuera.Sub;
using MinorShift.Emuera.GameProc;
using MinorShift.Emuera.GameData.Expression;

using System.Windows.Forms;

namespace MinorShift.Emuera.GameData.Variable
{

	internal class VariableTerm : IOperandTerm
	{
		protected VariableTerm(VariableToken token) : base(token.VariableType) { }
		public VariableTerm(VariableToken token, IOperandTerm[] args)
			: base(token.VariableType)
		{
			this.Identifier = token;
			arguments = args;
			transporter = new Int64[arguments.Length];

			allArgIsConst = false;
			for (int i = 0; i < arguments.Length; i++)
			{
				if (!(arguments[i] is SingleTerm))
					return;
				transporter[i] = ((SingleTerm)arguments[i]).Int;
			}
			allArgIsConst = true;
		}
		public VariableToken Identifier;
		private readonly IOperandTerm[] arguments;
		protected Int64[] transporter;
		protected bool allArgIsConst = false;

		public Int64 GetElementInt(int i, ExpressionMediator exm)
		{
			if (allArgIsConst)
				return transporter[i];
			return arguments[i].GetIntValue(exm);
		}

		public bool isAllConst { get { return allArgIsConst; } }
		public int getEl1forArg { get { return (int)transporter[0]; } }

		public override Int64 GetIntValue(ExpressionMediator exm)
		{
			try
			{
				if (!allArgIsConst)
					for (int i = 0; i < arguments.Length; i++)
						transporter[i] = arguments[i].GetIntValue(exm);
				return Identifier.GetIntValue(exm, transporter);
			}
			catch (Exception e)
			{
				if ((e is IndexOutOfRangeException) || (e is ArgumentOutOfRangeException) || (e is OverflowException))
					Identifier.CheckElement(transporter);
				throw e;
			}
		}
		public override string GetStrValue(ExpressionMediator exm)
		{
			try
			{
				if (!allArgIsConst)
					for (int i = 0; i < arguments.Length; i++)
						transporter[i] = arguments[i].GetIntValue(exm);
				string ret = Identifier.GetStrValue(exm, transporter);
				if (ret == null)
					return "";
				return ret;
			}
			catch (Exception e)
			{
				if ((e is IndexOutOfRangeException) || (e is ArgumentOutOfRangeException) || (e is OverflowException))
					Identifier.CheckElement(transporter);
				throw e;
			}
		}
		public virtual void SetValue(Int64 value, ExpressionMediator exm)
		{
			try
			{
				if (!allArgIsConst)
					for (int i = 0; i < arguments.Length; i++)
						transporter[i] = arguments[i].GetIntValue(exm);
				Identifier.SetValue(value, transporter);
			}
			catch (Exception e)
			{
				if ((e is IndexOutOfRangeException) || (e is ArgumentOutOfRangeException) || (e is OverflowException))
					Identifier.CheckElement(transporter);
				throw e;
			}
		}
		public virtual void SetValue(string value, ExpressionMediator exm)
		{
			try
			{
				if (!allArgIsConst)
					for (int i = 0; i < arguments.Length; i++)
						transporter[i] = arguments[i].GetIntValue(exm);
				Identifier.SetValue(value, transporter);
			}
			catch (Exception e)
			{
				if ((e is IndexOutOfRangeException) || (e is ArgumentOutOfRangeException) || (e is OverflowException))
					Identifier.CheckElement(transporter);
				throw e;
			}
		}

		public virtual void SetValue(Int64[] array, ExpressionMediator exm)
		{
			try
			{
				if (!allArgIsConst)
					for (int i = 0; i < arguments.Length; i++)
						transporter[i] = arguments[i].GetIntValue(exm);
				Identifier.SetValue(array, transporter);
			}
			catch (Exception e)
			{
				if ((e is IndexOutOfRangeException) || (e is ArgumentOutOfRangeException) || (e is OverflowException))
				{
					Identifier.CheckElement(transporter);
					throw new CodeEE("配列変数" + Identifier.Name + "の要素数を超えて代入しようとしました");
				}
				throw e;
			}
		}
		public virtual void SetValue(string[] array, ExpressionMediator exm)
		{
			try
			{
				if (!allArgIsConst)
					for (int i = 0; i < arguments.Length; i++)
						transporter[i] = arguments[i].GetIntValue(exm);
				Identifier.SetValue(array, transporter);
			}
			catch (Exception e)
			{
				if ((e is IndexOutOfRangeException) || (e is ArgumentOutOfRangeException) || (e is OverflowException))
				{
					Identifier.CheckElement(transporter);
					throw new CodeEE("配列変数" + Identifier.Name + "の要素数を超えて代入しようとしました");
				}
				throw e;
			}
		}

		public virtual Int64 PlusValue(Int64 value, ExpressionMediator exm)
		{
			try
			{
				if (!allArgIsConst)
					for (int i = 0; i < arguments.Length; i++)
						transporter[i] = arguments[i].GetIntValue(exm);
				return Identifier.PlusValue(value, transporter);
			}
			catch (Exception e)
			{
				if ((e is IndexOutOfRangeException) || (e is ArgumentOutOfRangeException) || (e is OverflowException))
					Identifier.CheckElement(transporter);
				throw e;
			}
		}
		public override SingleTerm GetValue(ExpressionMediator exm)
		{
			if (Identifier.VariableType == typeof(Int64))
				return new SingleTerm(GetIntValue(exm));
			else
				return new SingleTerm(GetStrValue(exm));
		}
		public void SetValue(SingleTerm value, ExpressionMediator exm)
		{
			if (Identifier.VariableType == typeof(Int64))
				SetValue(value.Int, exm);
			else
				SetValue(value.Str, exm);
		}
		public void SetValue(IOperandTerm value, ExpressionMediator exm)
		{
			if (Identifier.VariableType == typeof(Int64))
				SetValue(value.GetIntValue(exm), exm);
			else
				SetValue(value.GetStrValue(exm), exm);
		}
		public Int32 GetLength()
		{
			return Identifier.GetLength();
		}
		public Int32 GetLength(int dimension)
		{
			return Identifier.GetLength(dimension);
		}
		public Int32 GetLastLength()
		{
			if (Identifier.IsArray1D)
				return Identifier.GetLength();
			else if (Identifier.IsArray2D)
				return Identifier.GetLength(1);
			else if (Identifier.IsArray3D)
				return Identifier.GetLength(2);
			return 0;
		}

		public FixedVariableTerm GetFixedVariableTerm(ExpressionMediator exm)
		{
			if (!allArgIsConst)
				for (int i = 0; i < arguments.Length; i++)
					transporter[i] = arguments[i].GetIntValue(exm);
			FixedVariableTerm fp = new FixedVariableTerm(Identifier);
			if (transporter.Length >= 1)
				fp.Index1 = transporter[0];
			if (transporter.Length >= 2)
				fp.Index2 = transporter[1];
			if (transporter.Length >= 3)
				fp.Index3 = transporter[2];
			return fp;
		}

		public override IOperandTerm Restructure(ExpressionMediator exm)
		{
			bool[] canCheck = new bool[arguments.Length];
			allArgIsConst = true;
			for (int i = 0; i < arguments.Length; i++)
			{
				arguments[i] = arguments[i].Restructure(exm);
				if (!(arguments[i] is SingleTerm))
				{
					allArgIsConst = false;
					canCheck[i] = false;
				}
				else
				{
					//キャラクターデータの第1引数はこの時点でチェックしても意味がないのと
					//ARG系は限界超えてても必要な数に拡張されるのでチェックしなくていい
					if ((i == 0 && Identifier.IsCharacterData) || Identifier.Name == "ARG" || Identifier.Name == "ARGS")
						canCheck[i] = false;
					else
						canCheck[i] = true;
					//if (allArgIsConst)
					//チェックのために値が必要
					transporter[i] = arguments[i].GetIntValue(exm);
				}
			}
			if (!Identifier.IsReference)
				Identifier.CheckElement(transporter, canCheck);
			if ((Identifier.CanRestructure) && (allArgIsConst))
				return GetValue(exm);
			else if (allArgIsConst)
				return new FixedVariableTerm(Identifier, transporter);
			return this;
		}
	}

	
    internal sealed class FixedVariableTerm : VariableTerm
    {
		public FixedVariableTerm(VariableToken token)
			: base(token)
		{
			this.Identifier = token;
			transporter = new Int64[3];
			allArgIsConst = true;
		}
		public FixedVariableTerm(VariableToken token, Int64[] args)
			: base(token)
		{
			allArgIsConst = true;
			this.Identifier = token;
			transporter = new Int64[3];
			for(int i = 0;i< args.Length;i++)
				transporter[i] = args[i];
		}
		public Int64 Index1{get{return transporter[0];} set{transporter[0] = value;}}
		public Int64 Index2{get{return transporter[1];} set{transporter[1] = value;}}
        public Int64 Index3{get{return transporter[2];} set{transporter[2] = value;}}
        
		
        public override Int64 GetIntValue(ExpressionMediator exm)
        {
			try
			{
				return Identifier.GetIntValue(exm, transporter);
			}
			catch(Exception e)
			{
				if ((e is IndexOutOfRangeException) || (e is ArgumentOutOfRangeException) || (e is OverflowException))
					Identifier.CheckElement(transporter);
				throw e;
			}
        }
        public override string GetStrValue(ExpressionMediator exm)
        {
			try
			{
				string ret = Identifier.GetStrValue(exm, transporter);
				if (ret == null)
					return "";
				return ret;
			}
			catch(Exception e)
			{
				if ((e is IndexOutOfRangeException) || (e is ArgumentOutOfRangeException) || (e is OverflowException))
					Identifier.CheckElement(transporter);
				throw e;
			}
        }

		public override void SetValue(Int64 value, ExpressionMediator exm)
        {
			try
			{
				Identifier.SetValue(value, transporter);
			}
			catch(Exception e)
			{
				if ((e is IndexOutOfRangeException) || (e is ArgumentOutOfRangeException) || (e is OverflowException))
					Identifier.CheckElement(transporter);
				throw e;
			}
        }
		public override void SetValue(string value, ExpressionMediator exm)
        {
			try
			{
				Identifier.SetValue(value, transporter);
			}
			catch(Exception e)
			{
				if ((e is IndexOutOfRangeException) || (e is ArgumentOutOfRangeException) || (e is OverflowException))
					Identifier.CheckElement(transporter);
				throw e;
			}
        }

		public override Int64 PlusValue(Int64 value, ExpressionMediator exm)
        {
			try
			{
				return Identifier.PlusValue(value, transporter);
			}
			catch(Exception e)
			{
				if ((e is IndexOutOfRangeException) || (e is ArgumentOutOfRangeException) || (e is OverflowException))
					Identifier.CheckElement(transporter);
				throw e;
			}
        }
        
        public override IOperandTerm Restructure(ExpressionMediator exm)
        {
			if (Identifier.CanRestructure)
				return GetValue(exm);
			return this;
        }
        
		public void IsArrayRangeValid(Int64 index1, Int64 index2, string funcName, Int64 i1, Int64 i2)
		{
			Identifier.IsArrayRangeValid(transporter, index1, index2, funcName, i1, i2);
		}
    }
}