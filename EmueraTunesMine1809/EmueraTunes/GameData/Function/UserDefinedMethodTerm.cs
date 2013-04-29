using System;
using System.Collections.Generic;
using System.Text;
using MinorShift.Emuera.GameProc;
using MinorShift.Emuera.GameData.Expression;

namespace MinorShift.Emuera.GameData.Function
{
	
	
	
	internal sealed class UserDefinedMethodTerm : IOperandTerm
	{
		
		/// <summary>
		/// エラーならnullを返す。
		/// </summary>
		public static UserDefinedMethodTerm Create(IOperandTerm[] srcArgs, CalledFunction call, out string errMes)
		{
			UserDefinedFunctionArgument arg = call.ConvertArg(srcArgs, out errMes);
			if (arg == null)
				return null;
			return new UserDefinedMethodTerm(arg, call.TopLabel.MethodType, call);
		}

		private UserDefinedMethodTerm(UserDefinedFunctionArgument arg, Type returnType, CalledFunction call)
			: base(returnType)
		{
			Argument = arg;
			Call = call;
		}
		public UserDefinedFunctionArgument Argument { get; private set; }
        public CalledFunction Call { get; private set; }

        public override long GetIntValue(ExpressionMediator exm)
        {
            SingleTerm term = exm.Process.GetValue(this);
            if (term == null)
                return 0;
            return term.Int;
        }
        public override string GetStrValue(ExpressionMediator exm)
        {
            SingleTerm term = exm.Process.GetValue(this);
            if (term == null)
                return "";
            return term.Str;
        }
        public override SingleTerm GetValue(ExpressionMediator exm)
        {
            SingleTerm term =  exm.Process.GetValue(this);
            if (term == null)
            {
                if (GetOperandType() == typeof(Int64))
                    return new SingleTerm(0);
                else
                    return new SingleTerm("");
            }
            return term;
        }
		public override IOperandTerm Restructure(ExpressionMediator exm)
		{
			Argument.Restructure(exm);
			return this;
		}
		
		
	}
}
