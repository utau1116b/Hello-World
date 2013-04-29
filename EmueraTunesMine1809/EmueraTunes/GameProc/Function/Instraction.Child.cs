﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using MinorShift.Emuera.GameData.Expression;
using MinorShift.Emuera.Sub;
using MinorShift.Emuera.GameData.Variable;
using MinorShift.Emuera.GameData;
using MinorShift._Library;
using MinorShift.Emuera.GameData.Function;
using MinorShift.Emuera.GameSound;
using MinorShift.Emuera.GameView;
using System.Drawing;
using Microsoft.DirectX.AudioVideoPlayback;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using WindowsGame;
using WindowsGame.PlaySound;


namespace MinorShift.Emuera.GameProc.Function
{

	internal sealed partial class FunctionIdentifier
	{
		#region normalFunction

        public static GameMain gm = new GameMain();

        private sealed class MUSIC_Instruction : AbstractInstruction
        {
            

            public  MUSIC_Instruction(string name)
            {
                StringStream st = new StringStream(name);
                st.Jump(5);

                isSet = false;
                isClose = false;
                isPlay = false;
                isStop = false;
                isLoop = false;
                isSe = false;
                isXwb = false;
                if (st.CurrentEqualTo("X"))
                {
                    isXwb = true;
                    isSet = true;
                    st.Jump(1);
                }
                if (st.CurrentEqualTo("SE"))
                {
                    isSe = true;
                    isPlay = true;
                    st.Jump(2);
                }
                if (st.CurrentEqualTo("PLAY"))
                {
                    isSet = true;
                    isStop = false;
                    isPlay = true;
                    st.Jump(4);
                }
                if (st.CurrentEqualTo("STOP"))
                {
                    isPlay = false;
                    isClose = true;
                    isStop = true;
                    st.Jump(4);
                }
                if (st.CurrentEqualTo("LOOP"))
                {
                    isPlay = true;
                    isStop = true;
                    isLoop = true;
                    st.Jump(4);
                }

                ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.STR_NULLABLE);
                flag |= METHOD_SAFE;
				if ((ArgBuilder == null) || (!st.EOS))
					throw new ExeEE("SOUND異常");
            }

            readonly bool isPlay;
            readonly bool isStop;
            readonly bool isLoop;
            readonly bool isSe;
            readonly bool isSet;
            readonly bool isClose;
            readonly bool isXwb;
            private bool fileExist;
            //public static GameMain gm = new GameMain();
            private static string bgmname = null;
            private static string bgmname2 = null;

            private static MainMDXS2 mdxs = new MainMDXS2();
            SoundEffect _soundEffect;
            SoundEffectInstance _soundInstance;

            public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
            {

                string str = null;
                str = ((ExpressionArgument)func.Argument).Term.GetStrValue(exm);
                string fileName = Program.MusicDir + str;
                bgmname = str;
                //XACTを使用する時
                if (isXwb == true)
                {
                    //exm.Console.DrawImage();
                    //BGM再生
                    if (isSe == false)
                    {
                        if (isPlay == true)
                        {
                            if (bgmname == bgmname2 && (bgmname != null && bgmname2 != null)) { return; }
                            else
                            {
                                gm.Dispose();
                                gm.CloseSound();
                                gm.SetSound();
                                gm.PlaySound(str);
                                bgmname2 = str;
                            }
                        }
                        else if (isStop == true)
                        {
                            //this.SoundStopBGM();
                            gm.Dispose();
                            gm.StopSound();
                            bgmname = null;
                            bgmname2 = null;
                        }
                    }
                    //SE再生
                    else 
                    {
                        if (isPlay == true)
                        {
                            gm.Dispose();
                            gm.SetSoundSE();
                            gm.PlaySoundSE(str);
                        }
                        else if (isStop == true)
                        {
                            gm.Dispose();
                            gm.StopSoundSE(str);
                        }

                    }
                }
                //XACT未使用
                else
                {
                    fileExist = (File.Exists(fileName));

                    //ファイルが存在しない時リターン
                    if (fileExist == false) return;

                    //BGM再生はManagedDirectXを使う
                    if (isSe == false && isXwb == false)
                    {
                        if (isSet == true)
                        {
                            mdxs.CloseSound2();
                            Uri u1 = new Uri(Program.ExeDir);
                            Uri u2 = new Uri(u1, fileName);
                            string uri = u1.MakeRelativeUri(u2).ToString();
                            mdxs.SetSound2(uri);


                        }
                        if (isClose == true)
                        {
                            mdxs.CloseSound2();
                        }
                        if (isPlay == true)
                        {
                            mdxs.PlaySound2(isLoop);
                        }
                        if (isStop == true)
                        {
                            mdxs.StopSound2();
                        }
                    }
                    //SE再生はXNAを使う
                    if (isSe == true)
                    {
                        Uri u1 = new Uri(Program.ExeDir);
                        Uri u2 = new Uri(u1, fileName);
                        Stream stream = TitleContainer.OpenStream(u1.MakeRelativeUri(u2).ToString());
                        _soundEffect = SoundEffect.FromStream(stream);
                        FrameworkDispatcher.Update();
                        if (isPlay == true)
                        {
                            _soundInstance = _soundEffect.CreateInstance();

                            if (isLoop == true)
                                _soundInstance.IsLooped = true;
                            _soundEffect.Play();
                        }
                        if (isStop == true)
                        {
                            _soundInstance.Stop();
                        }
                    }
                }
            }
        }


        //Picture描画
        private sealed class PICTURE_Instruction : AbstractInstruction
        {

            public PICTURE_Instruction(string name)
            {
                StringStream st = new StringStream(name);
                st.Jump(7);

                isOpen = false;
                isClose = false;
                if (st.CurrentEqualTo("OPEN"))
                {
                    isOpen = true;
                    st.Jump(4);
                }
                if (st.CurrentEqualTo("CLOSE"))
                {
                    isClose = true;
                    st.Jump(5);
                }


                ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.STR_NULLABLE);
                flag |= METHOD_SAFE;
                if ((ArgBuilder == null) || (!st.EOS))
                    throw new ExeEE("Picture異常");
            }

            readonly bool isOpen;
            readonly bool isClose;
            private bool fileExist;

            public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
            {

                string str = null;
                str = ((ExpressionArgument)func.Argument).Term.GetStrValue(exm);
                string fileName = Program.PictureDir + str;
               
                 fileExist = (File.Exists(fileName));

                 if (isOpen == true)
                 {
                     //ファイルが存在しない時リターン
                     if (fileExist == false) return;
                     exm.Console.DrawImage(fileName);
                 }
                 else if (isClose == true) 
                 {
                     exm.Console.DeleteImage();
                 }
            }

                    
               
        }



        //Picture描画
        /*
        private sealed class PICTURE_Instruction : AbstractInstruction 
        {
            public PICTURE_Instruction(string name) 
            {
                StringStream st = new StringStream(name);

                st.Jump(7);

                isOpen = false;
                isClose = false;
                if (st.CurrentEqualTo("OPEN")) 
                {
                    isOpen = true;
                    st.Jump(4); 
                }
                if (st.CurrentEqualTo("CLOSE"))
                {
                    isClose = true;
                    st.Jump(5);
                }
                

            }

            readonly bool isOpen;
            readonly bool isClose;
            
            public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
            {

                //string str = null;
                //str = ((ExpressionArgument)func.Argument).Term.GetStrValue(exm);
                //string fileName = Program.PictureDir + str;

                if (isOpen == true && isClose == true)
                {

                    if (isOpen == true)
                    {
                        exm.Console.DrawImage();
                        // console.DrawImage();
                        //mw.ImageOpen();
                    }
                    else if (isClose == true)
                    {

                        //mw.ImageClose();
                    }
                }
            }
        }
        */
		private sealed class PRINT_Instruction : AbstractInstruction
		{
			public PRINT_Instruction(string name)
			{
				//PRINT(|V|S|FORM|FORMS)(|K)(|D)(|L|W) コレと
				//PRINTSINGLE(|V|S|FORM|FORMS)(|K)(|D) コレと
				//PRINT(|FORM)(C|LC)(|K)(|D) コレ
				//PRINTDATA(|K)(|D)(|L|W) ←は別クラス
				flag = IS_PRINT;
				StringStream st = new StringStream(name);
				st.Jump(5);//PRINT
				if (st.CurrentEqualTo("SINGLE"))
				{
					flag |= PRINT_SINGLE | EXTENDED;
					st.Jump(6);
				}

				if (st.CurrentEqualTo("V"))
				{
					ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_PRINTV);
					isPrintV = true;
					st.Jump(1);
				}
				else if (st.CurrentEqualTo("S"))
				{
					ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.STR_EXPRESSION);
					st.Jump(1);
				}
				else if (st.CurrentEqualTo("FORMS"))
				{
					ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.STR_EXPRESSION);
					isForms = true;
					st.Jump(5);
				}
				else if (st.CurrentEqualTo("FORM"))
				{
					ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.FORM_STR_NULLABLE);
					st.Jump(4);
				}
				else
				{
					ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.STR_NULLABLE);
				}
				if (st.CurrentEqualTo("LC"))
				{
					flag |= EXTENDED;
					isLC = true;
					st.Jump(2);
				}
				else if (st.CurrentEqualTo("C"))
				{
					if (name == "PRINTFORMC")
						flag |= EXTENDED;
					isC = true;
					st.Jump(1);
				}
				if (st.CurrentEqualTo("K"))
				{
					flag |= ISPRINTKFUNC | EXTENDED;
					st.Jump(1);
				}
				if (st.CurrentEqualTo("D"))
				{
					flag |= ISPRINTDFUNC | EXTENDED;
					st.Jump(1);
				}
				if (st.CurrentEqualTo("L"))
				{
					flag |= PRINT_NEWLINE;
					flag |= METHOD_SAFE;
					st.Jump(1);
				}
				else if (st.CurrentEqualTo("W"))
				{
					flag |= PRINT_NEWLINE | PRINT_WAITINPUT;
					st.Jump(1);
				}
				else
				{
					flag |= METHOD_SAFE;
				}
				if ((ArgBuilder == null) || (!st.EOS))
					throw new ExeEE("PRINT異常");
			}

			readonly bool isPrintV;
			readonly bool isLC;
			readonly bool isC;
			readonly bool isForms;
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				exm.Console.UseUserStyle = !func.Function.IsPrintDFunction();
				string str = null;
				if (func.Argument.IsConst)
					str = func.Argument.ConstStr;
				else if (isPrintV)
				{
					StringBuilder builder = new StringBuilder();
					IOperandTerm[] terms = ((SpPrintVArgument)func.Argument).Terms;
					foreach (IOperandTerm termV in terms)
					{
						if (termV.GetOperandType() == typeof(Int64))
							builder.Append(termV.GetIntValue(exm).ToString());
						else
							builder.Append(termV.GetStrValue(exm));
					}
					str = builder.ToString();
				}
				else
				{
					str = ((ExpressionArgument)func.Argument).Term.GetStrValue(exm);
					if (isForms)
					{
						str = exm.CheckEscape(str);
						StrFormWord wt = LexicalAnalyzer.AnalyseFormattedString(new StringStream(str), FormStrEndWith.EoL, false);
						StrForm strForm = StrForm.FromWordToken(wt);
						str = strForm.GetString(exm);
					}
				}
				if (func.Function.IsPrintKFunction())
					str = exm.ConvertStringType(str);
				if (isC)
					exm.Console.PrintC(str, true);
				else if (isLC)
					exm.Console.PrintC(str, false);
				else
					exm.OutputToConsole(str, func.Function);
				exm.Console.UseUserStyle = true;
			}
		}


		private sealed class PRINT_DATA_Instruction : AbstractInstruction
		{
			public PRINT_DATA_Instruction(string name)
			{
				//PRINTDATA(|K)(|D)(|L|W)
				flag = EXTENDED | IS_PRINT | PARTIAL;
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.VAR_INT);
				StringStream st = new StringStream(name);
				st.Jump(9);//PRINTDATA
				if (st.CurrentEqualTo("K"))
				{
					flag |= ISPRINTKFUNC | EXTENDED;
					st.Jump(1);
				}
				if (st.CurrentEqualTo("D"))
				{
					flag |= ISPRINTDFUNC | EXTENDED;
					st.Jump(1);
				}
				if (st.CurrentEqualTo("L"))
				{
					flag |= PRINT_NEWLINE;
					flag |= METHOD_SAFE;
					st.Jump(1);
				}
				else if (st.CurrentEqualTo("W"))
				{
					flag |= PRINT_NEWLINE | PRINT_WAITINPUT;
					st.Jump(1);
				}
				else
				{
					flag |= METHOD_SAFE;
				}
				if ((ArgBuilder == null) || (!st.EOS))
					throw new ExeEE("PRINTDATA異常");
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				exm.Console.UseUserStyle = !func.Function.IsPrintDFunction();
                //表示データが空なら何もしないで飛ぶ
                if (func.dataList.Count == 0)
                {
                    state.JumpTo(func.JumpTo);
                    return;
                }
				int count = func.dataList.Count;
				int choice = (int)exm.VEvaluator.GetNextRand(count);
				VariableTerm iTerm = ((PrintDataArgument)func.Argument).Var;
				if (iTerm != null)
				{
					iTerm.SetValue(choice, exm);
				}
				List<InstructionLine> iList = func.dataList[choice];
				int i = 0;
				IOperandTerm term = null;
				string str = null;
				foreach (InstructionLine selectedLine in iList)
				{
                    state.CurrentLine = selectedLine;
                    if (selectedLine.Argument == null)
						ArgumentParser.SetArgumentTo(selectedLine);
					term = ((ExpressionArgument)selectedLine.Argument).Term;
					str = term.GetStrValue(exm);
					if (func.Function.IsPrintKFunction())
						str = exm.ConvertStringType(str);
					exm.Console.Print(str);
					if (++i < (int)iList.Count)
						exm.Console.NewLine();
				}
                if (func.Function.IsNewLine() || func.Function.IsWaitInput())
                {
                    exm.Console.NewLine();
                    if (func.Function.IsWaitInput())
                        exm.Console.ReadAnyKey();
                }
				exm.Console.UseUserStyle = true;
				//ジャンプするが、流れが連続であることを保証。
				state.JumpTo(func.JumpTo);
                //state.RunningLine = null;
            }
		}

		private sealed class DEBUGPRINT_Instruction : AbstractInstruction
		{
			public DEBUGPRINT_Instruction(bool form, bool newline)
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.STR_NULLABLE);
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.FORM_STR_NULLABLE);
				flag = METHOD_SAFE | EXTENDED | DEBUG_FUNC;
				if (newline)
					flag |= PRINT_NEWLINE;
			}
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				string str = null;
				if (func.Argument.IsConst)
					str = func.Argument.ConstStr;
				else
					str = ((ExpressionArgument)func.Argument).Term.GetStrValue(exm);
				exm.Console.DebugPrint(str);
				if (func.Function.IsNewLine())
					exm.Console.DebugNewLine();
			}
		}

		private sealed class METHOD_Instruction : AbstractInstruction
		{
			public METHOD_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.METHOD);
				flag = METHOD_SAFE | EXTENDED;
			}
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				IOperandTerm term = ((MethodArgument)func.Argument).MethodTerm;
				Type type = term.GetOperandType();
				if (term.GetOperandType() == typeof(Int64))
					exm.VEvaluator.RESULT = term.GetIntValue(exm);
				else// if (func.Argument.MethodTerm.GetOperandType() == typeof(string))
					exm.VEvaluator.RESULTS = term.GetStrValue(exm);
				//これら以外の型は現状ない
				//else
				//	throw new ExeEE(func.Function.Name + "命令の型が不明");
			}
		}

		private sealed class SET_Instruction : AbstractInstruction
		{
			public SET_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_SET);
				flag = METHOD_SAFE;
			}
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				if (func.Argument is SpSetArrayArgument)
				{
					SpSetArrayArgument arg = (SpSetArrayArgument)func.Argument;
					if (arg.VariableDest.IsInteger)
					{
						if (arg.IsConst)
							arg.VariableDest.SetValue(arg.ConstTermList, exm);
						else
						{
							Int64[] values = new Int64[arg.TermList.Length];
							for(int i = 0;i < values.Length; i++)
							{
								values[i] = arg.TermList[i].GetIntValue(exm);
							}
							arg.VariableDest.SetValue(values, exm);
						}
					}
					else
					{
						throw new ExeEE("未実装");
					}
					return;
				}
				SpSetArgument spsetarg = (SpSetArgument)func.Argument;
				if (spsetarg.VariableDest.IsInteger)
				{
					Int64 src = spsetarg.IsConst ? spsetarg.ConstInt : spsetarg.Term.GetIntValue(exm);
					if (spsetarg.AddConst)
						spsetarg.VariableDest.PlusValue(src, exm);
					else
						spsetarg.VariableDest.SetValue(src, exm);
				}
				else
				{
					string src = spsetarg.IsConst ? spsetarg.ConstStr : spsetarg.Term.GetStrValue(exm);
					spsetarg.VariableDest.SetValue(src, exm);
				}
			}
		}

		private sealed class REUSELASTLINE_Instruction : AbstractInstruction
		{
			public REUSELASTLINE_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.FORM_STR_NULLABLE);
				flag = METHOD_SAFE | EXTENDED | IS_PRINT;
			}
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				IOperandTerm term = ((ExpressionArgument)func.Argument).Term;
				string str = term.GetStrValue(exm);
				exm.Console.PrintTemporaryLine(str);
			}
		}

		private sealed class CLEARLINE_Instruction : AbstractInstruction
		{
			public CLEARLINE_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.INT_EXPRESSION);
				flag = METHOD_SAFE | EXTENDED | IS_PRINT;
			}
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				ExpressionArgument intExpArg = (ExpressionArgument)func.Argument;
				Int32 delNum = (Int32)intExpArg.Term.GetIntValue(exm);
				exm.Console.deleteLine(delNum);
				exm.Console.RefreshStrings(false);
			}
		}

		private sealed class STRLEN_Instruction : AbstractInstruction
		{
			public STRLEN_Instruction(bool argisform, bool unicode)
			{
				if (argisform)
					ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.FORM_STR_NULLABLE);
				else
					ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.STR_NULLABLE);
				flag = METHOD_SAFE | EXTENDED;
				this.unicode = unicode;
			}
			bool unicode;
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				string str = null;
				if (func.Argument.IsConst)
					str = func.Argument.ConstStr;
				else
					str = ((ExpressionArgument)func.Argument).Term.GetStrValue(exm);
				if (unicode)
					exm.VEvaluator.RESULT = str.Length;
				else
					exm.VEvaluator.RESULT = LangManager.GetStrlenLang(str);
			}
		}
		
		private sealed class SETBIT_Instruction : AbstractInstruction
		{
			public SETBIT_Instruction(int op)
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.BIT_ARG);
				flag = METHOD_SAFE | EXTENDED;
				this.op = op;
			}
			int op;
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				BitArgument spsetarg = (BitArgument)func.Argument;
				VariableTerm varTerm = spsetarg.VariableDest;
                IOperandTerm[] terms = spsetarg.Term;
                for (int i = 0; i < terms.Length; i++)
                {
                    Int64 x = terms[i].GetIntValue(exm);
                    if ((x < 0) || (x > 63))
                        throw new CodeEE("第2引数がビットのレンジ(0から63)を超えています");
                    Int64 baseValue = varTerm.GetIntValue(exm);
                    Int64 shift = 1L << (int)x;
                    if (op == 1)
                        baseValue |= shift;
                    else if (op == 0)
                        baseValue &= ~shift;
                    else
                        baseValue ^= shift;
                    varTerm.SetValue(baseValue, exm);
                }
			}
		}

		private sealed class WAIT_Instruction : AbstractInstruction
		{
			public WAIT_Instruction(bool force)
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.VOID);
				flag = IS_PRINT;
                isForce = force;
			}
            bool isForce;
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
                if (isForce)
                    exm.Console.ReadAnyKeyForce();
                else
                    exm.Console.ReadAnyKey();
			}
		}

        private sealed class WAITANYKEY_Instruction : AbstractInstruction
        {
            public WAITANYKEY_Instruction()
            {
                ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.VOID);
                flag = IS_PRINT;
            }
            public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
            {
                exm.Console.ReadAnyKeyAndGo();
            }
        }

		private sealed class TWAIT_Instruction : AbstractInstruction
		{
			public TWAIT_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_SWAP);
				flag = IS_PRINT | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				exm.Console.ReadAnyKey();
				SpSwapCharaArgument arg = (SpSwapCharaArgument)func.Argument;
				Int64 time = arg.X.GetIntValue(exm);
				Int64 flag = arg.Y.GetIntValue(exm);
				exm.Console.waitInputWithTimer(time, flag);
			}
		}

		private sealed class INPUT_Instruction : AbstractInstruction
		{
			public INPUT_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_INPUT);
				flag = IS_PRINT | IS_INPUT;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
                ExpressionArgument arg = (ExpressionArgument)func.Argument;
                if (arg.Term != null)
                {
                    Int64 def;
                    if (arg.IsConst)
                        def = arg.ConstInt;
                    else
                        def = arg.Term.GetIntValue(exm);
                    exm.Console.ReadInteger(def);
                }
                else
                    exm.Console.ReadInteger();
                exm.Console.UpdateMousePosition();
            }
		}
		private sealed class INPUTS_Instruction : AbstractInstruction
		{
			public INPUTS_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_INPUTS);
				flag = IS_PRINT | IS_INPUT;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
                ExpressionArgument arg = (ExpressionArgument)func.Argument;
                if (arg.Term != null)
                {
                    string def;
                    if (arg.IsConst)
                        def = arg.ConstStr;
                    else
                        def = arg.Term.GetStrValue(exm);
                    exm.Console.ReadString(def);
                }
                else
                    exm.Console.ReadString();
                exm.Console.UpdateMousePosition();
            }
		}

		private sealed class ONEINPUT_Instruction : AbstractInstruction
		{
			public ONEINPUT_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_INPUT);
				flag = IS_PRINT | IS_INPUT | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
                ExpressionArgument arg = (ExpressionArgument)func.Argument;
                if (arg.Term != null)
                {
                    Int64 def;
                    if (arg.IsConst)
                        def = arg.ConstInt;
                    else
                        def = arg.Term.GetIntValue(exm);
                    if (def > 9)
                        def = Int64.Parse(def.ToString().Remove(1));
                    exm.Console.ReadOneInteger(def);
                    if (def < 0)
                        exm.Console.ResetDefValue();
                }
                else
                    exm.Console.ReadOneInteger();
                exm.Console.UpdateMousePosition();
            }
		}

		private sealed class ONEINPUTS_Instruction : AbstractInstruction
		{
			public ONEINPUTS_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_INPUTS);
				flag = IS_PRINT | IS_INPUT | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
                ExpressionArgument arg = (ExpressionArgument)func.Argument;
                if (arg.Term != null)
                {
                    string def;
                    if (arg.IsConst)
                        def = arg.ConstStr;
                    else
                        def = arg.Term.GetStrValue(exm);
                    if (def.Length > 1)
                        def = def.Remove(1);
                    exm.Console.ReadOneString(def);
                    if (def.Length == 0)
                        exm.Console.ResetDefValue();
                }
                else
                    exm.Console.ReadOneString();
                exm.Console.UpdateMousePosition();
            }
		}

		private sealed class TINPUT_Instruction : AbstractInstruction
		{
			public TINPUT_Instruction(bool oneInput)
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_TINPUT);
				flag = IS_PRINT | IS_INPUT | EXTENDED;
                this.isOne = oneInput;
			}
            bool isOne;
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
                SpTInputsArgument tinputarg = (SpTInputsArgument)func.Argument;
                Int64 x = tinputarg.Time.GetIntValue(exm);
                Int64 y = tinputarg.Def.GetIntValue(exm);
                if (isOne)
                {
                    if (y < 0)
                        y = Math.Abs(y);
                    if (y >= 10)
                        y = y / (long)(Math.Pow(10.0, Math.Log10((double)y)));
                }
                Int64 z = (tinputarg.Disp != null) ? tinputarg.Disp.GetIntValue(exm) : 1;
                string defTimeout = (tinputarg.Timeout != null) ? tinputarg.Timeout.GetStrValue(exm) : Config.TimeupLabel;
				exm.Console.ReadIntegerWithTimer(x, y, z, isOne, defTimeout);
                exm.Console.UpdateMousePosition();
            }
		}

		private sealed class TINPUTS_Instruction : AbstractInstruction
		{
            public TINPUTS_Instruction(bool oneInput)
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_TINPUTS);
				flag = IS_PRINT | IS_INPUT | EXTENDED;
                this.isOne = oneInput;
            }
            bool isOne;
            public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				SpTInputsArgument tinputarg = (SpTInputsArgument)func.Argument;
				Int64 x = tinputarg.Time.GetIntValue(exm);
				string strs = tinputarg.Def.GetStrValue(exm);
                if (isOne && strs.Length > 1)
                    strs = strs.Remove(1);
                Int64 z = (tinputarg.Disp != null) ? tinputarg.Disp.GetIntValue(exm) : 1;
                string defTimeout = (tinputarg.Timeout != null) ? tinputarg.Timeout.GetStrValue(exm) : Config.TimeupLabel;
                exm.Console.ReadStringWithTimer(x, strs, z, isOne, defTimeout);
                exm.Console.UpdateMousePosition();
            }
		}

		private sealed class CALLF_Instruction : AbstractInstruction
		{
			public CALLF_Instruction(bool form)
			{
				if (form)
					ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_CALLFORMF);
				else
					ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_CALLF);
				flag = EXTENDED | METHOD_SAFE | FORCE_SETARG;
			}

			public override void SetJumpTo(ref bool useCallForm, InstructionLine func, int currentDepth, ref string FunctionoNotFoundName)
			{
				if (!func.Argument.IsConst)
				{
					useCallForm = true;
					return;
				}
				SpCallFArgment callfArg = (SpCallFArgment)func.Argument;
				if (Config.ICFunction)
					callfArg.ConstStr = callfArg.ConstStr.ToUpper();
				string labelName = callfArg.ConstStr;
				FunctionLabelLine targetLabel = GlobalStatic.LabelDictionary.GetNonEventLabel(labelName);
				if (targetLabel == null)
				{
					if (!Program.AnalysisMode)
						ParserMediator.Warn("指定された関数名\"@" + labelName + "\"は存在しません", func, 2, true, false);
					else
						ParserMediator.Warn(labelName, func, 2, true, false);
					return;
				}
				if (targetLabel.Depth < 0)
					targetLabel.Depth = currentDepth + 1;
				if (!targetLabel.IsMethod)
				{
					ParserMediator.Warn("#FUNCTIONが指定されていない関数\"@" + labelName + "\"をCALLF系命令で呼び出そうとしました", func, 2, true, false);
					return;
				}
				CalledFunction call = CalledFunction.CreateCalledFunctionMethod(targetLabel, targetLabel.LabelName);
				string errMes;
				callfArg.FuncTerm = UserDefinedMethodTerm.Create(callfArg.RowArgs, call, out errMes);
				if (callfArg.FuncTerm == null)
					ParserMediator.Warn(errMes, targetLabel, 2, true, false);
				else
					callfArg.FuncTerm.Restructure(GlobalStatic.EMediator);
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				IOperandTerm mToken = null;
				string labelName = null;
				if ((!func.Argument.IsConst) || (exm.Console.RunERBFromMemory))
				{
					SpCallFArgment spCallformArg = (SpCallFArgment)func.Argument;
					labelName = spCallformArg.FuncnameTerm.GetStrValue(exm);
                    if (Config.ICFunction)
                        labelName = labelName.ToUpper();
					FunctionLabelLine targetLabel = GlobalStatic.LabelDictionary.GetNonEventLabel(labelName);
					if (targetLabel != null)
					{
						if (!targetLabel.IsMethod)
							throw new CodeEE("#FUNCTIONが指定されていない関数\"@" + labelName + "\"をCALLF系命令で呼び出そうとしました");
						CalledFunction call = CalledFunction.CreateCalledFunctionMethod(targetLabel, targetLabel.LabelName);
						string errMes;
						mToken = UserDefinedMethodTerm.Create(spCallformArg.RowArgs, call, out errMes);
						if (mToken == null)
							throw new CodeEE(errMes);
					}
				}
				else
				{
					labelName = func.Argument.ConstStr;
					mToken = ((SpCallFArgment)func.Argument).FuncTerm;
				}
                if (mToken == null)
                    throw new CodeEE("式中関数\"@" + labelName + "\"が見つかりません");
				mToken.GetValue(exm);
			}
		}

		private sealed class BAR_Instruction : AbstractInstruction
		{
			public BAR_Instruction(bool newline)
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_BAR);
				flag = IS_PRINT | METHOD_SAFE | EXTENDED;
				this.newline = newline;
			}
			bool newline;

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				SpBarArgument barArg = (SpBarArgument)func.Argument;
				Int64 var = barArg.Terms[0].GetIntValue(exm);
				Int64 max = barArg.Terms[1].GetIntValue(exm);
				Int64 length = barArg.Terms[2].GetIntValue(exm);
				exm.Console.Print(exm.CreateBar(var, max, length));
				if (newline)
					exm.Console.NewLine();
			}
		}

		private sealed class TIMES_Instruction : AbstractInstruction
		{
			public TIMES_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_TIMES);
				flag = METHOD_SAFE;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				SpTimesArgument timesArg = (SpTimesArgument)func.Argument;
				VariableTerm var = timesArg.VariableDest;
				double d = (double)var.GetIntValue(exm) * timesArg.DoubleValue;
				unchecked
				{
					var.SetValue((Int64)d, exm);
				}
			}
		}

		private sealed class ADDCHARA_Instruction : AbstractInstruction
		{
			public ADDCHARA_Instruction(bool flagSp, bool flagDel)
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.INT_ANY);
				flag = METHOD_SAFE;
				isDel = flagDel;
				isSp = flagSp;
			}
			bool isDel;
			bool isSp;

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				ExpressionArrayArgument intExpArg = (ExpressionArrayArgument)func.Argument;
				Int64 integer = -1;
				foreach (IOperandTerm int64Term in intExpArg.TermList)
				{
					integer = int64Term.GetIntValue(exm);
					if(isDel)
						exm.VEvaluator.DelCharacter(integer);
					else
						exm.VEvaluator.AddCharacter(integer, isSp);
				}
			}
		}


		private sealed class ADDVOIDCHARA_Instruction : AbstractInstruction
		{
			public ADDVOIDCHARA_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.VOID);
				flag = METHOD_SAFE | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				exm.VEvaluator.AddPseudoCharacter();
			}
		}

		private sealed class SWAPCHARA_Instruction : AbstractInstruction
		{
			public SWAPCHARA_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_SWAP);
				flag = METHOD_SAFE | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				SpSwapCharaArgument arg = (SpSwapCharaArgument)func.Argument;
				long x = arg.X.GetIntValue(exm);
				long y = arg.Y.GetIntValue(exm);
				exm.VEvaluator.SwapChara(x, y);
			}
		}
		private sealed class COPYCHARA_Instruction : AbstractInstruction
		{
			public COPYCHARA_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_SWAP);
				flag = METHOD_SAFE | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				SpSwapCharaArgument arg = (SpSwapCharaArgument)func.Argument;
				long x = arg.X.GetIntValue(exm);
				long y = arg.Y.GetIntValue(exm);
				exm.VEvaluator.CopyChara(x, y);
			}
		}

		private sealed class ADDCOPYCHARA_Instruction : AbstractInstruction
		{
			public ADDCOPYCHARA_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.INT_ANY);
				flag = METHOD_SAFE;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				ExpressionArrayArgument intExpArg = (ExpressionArrayArgument)func.Argument;
				foreach (IOperandTerm int64Term in intExpArg.TermList)
                    exm.VEvaluator.AddCopyChara(int64Term.GetIntValue(exm));
			}
		}

		private sealed class SORTCHARA_Instruction : AbstractInstruction
		{
			public SORTCHARA_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_SORTCHARA);
				flag = METHOD_SAFE | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				SpSortcharaArgument spSortArg = (SpSortcharaArgument)func.Argument;
				Int64 elem = 0;
				VariableTerm sortKey = spSortArg.SortKey;
				if (sortKey.Identifier.IsArray1D)
					elem = sortKey.GetElementInt(1, exm);
				else if (sortKey.Identifier.IsArray2D)
				{
					elem = sortKey.GetElementInt(1, exm) << 32;
					elem += sortKey.GetElementInt(2, exm);
				}

				exm.VEvaluator.SortChara(sortKey.Identifier, elem, spSortArg.SortOrder, true);
			}
		}

		private sealed class RESETCOLOR_Instruction : AbstractInstruction
		{
			public RESETCOLOR_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.VOID);
				flag = METHOD_SAFE | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				exm.Console.SetStringStyle(Config.ForeColor);
			}
		}

        private sealed class RESETBGCOLOR_Instruction : AbstractInstruction
        {
            public RESETBGCOLOR_Instruction()
            {
                ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.VOID);
                flag = METHOD_SAFE | EXTENDED;
            }

            public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
            {
                exm.Console.SetBgColor(Config.BackColor);
            }
        }

		private sealed class FONTBOLD_Instruction : AbstractInstruction
		{
			public FONTBOLD_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.VOID);
				flag = METHOD_SAFE | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				exm.Console.SetStringStyle(exm.Console.StringStyle.FontStyle | FontStyle.Bold);
			}
		}
		private sealed class FONTITALIC_Instruction : AbstractInstruction
		{
			public FONTITALIC_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.VOID);
				flag = METHOD_SAFE | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				exm.Console.SetStringStyle(exm.Console.StringStyle.FontStyle | FontStyle.Italic);
			}
		}
		private sealed class FONTREGULAR_Instruction : AbstractInstruction
		{
			public FONTREGULAR_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.VOID);
				flag = METHOD_SAFE | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
					exm.Console.SetStringStyle(FontStyle.Regular);
			}
		}
		
		private sealed class VARSET_Instruction : AbstractInstruction
		{
			public VARSET_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_VAR_SET);
				flag = METHOD_SAFE | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{

				SpVarSetArgument spvarsetarg = (SpVarSetArgument)func.Argument;
				VariableTerm var = spvarsetarg.VariableDest;
				FixedVariableTerm p = var.GetFixedVariableTerm(exm);
				int start = 0;
				int end = 0;
				//endを先に取って判定の処理変更
				if (spvarsetarg.End != null)
					end = (int)spvarsetarg.End.GetIntValue(exm);
				else if (var.Identifier.IsArray1D)
					end = (int)var.GetLength();
				if (spvarsetarg.Start != null)
				{
					start = (int)spvarsetarg.Start.GetIntValue(exm);
					if (start > end)
					{
						int temp = start;
						start = end;
						end = start;
					}
				}
				if (var.IsString)
				{
					string src = spvarsetarg.Term.GetStrValue(exm);
					exm.VEvaluator.SetValueAll(p, src, start, end);
				}
				else
				{
					long src = spvarsetarg.Term.GetIntValue(exm);
					exm.VEvaluator.SetValueAll(p, src, start, end);
				}
			}
		}

		private sealed class CVARSET_Instruction : AbstractInstruction
		{
			public CVARSET_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_CVAR_SET);
				flag = METHOD_SAFE | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				SpCVarSetArgument spvarsetarg = (SpCVarSetArgument)func.Argument;
				FixedVariableTerm p = spvarsetarg.VariableDest.GetFixedVariableTerm(exm);
				SingleTerm index = spvarsetarg.Index.GetValue(exm);
				int charaNum = (int)exm.VEvaluator.CHARANUM;
				int start = 0;
				if (spvarsetarg.Start != null)
				{
					start = (int)spvarsetarg.Start.GetIntValue(exm);
					if (start < 0 || start >= charaNum)
						throw new CodeEE("命令CVARSETの第４引数(" + start.ToString() + ")がキャラクタの範囲外です");
				}
				int end = 0;
				if (spvarsetarg.End != null)
				{
					end = (int)spvarsetarg.End.GetIntValue(exm);
					if (end < 0 || end > charaNum)
						throw new CodeEE("命令CVARSETの第５引数(" + end.ToString() + ")がキャラクタの範囲外です");
				}
				else
					end = charaNum;
				if (start > end)
				{
					int temp = start;
					start = end;
					end = start;
				}
				if (!p.Identifier.IsCharacterData)
					throw new CodeEE("命令CVARSETにキャラクタ変数でない変数" + p.Identifier.Name + "が渡されました");
				if (index.GetOperandType() == typeof(string) && p.Identifier.IsArray1D)
				{
					if (!GlobalStatic.ConstantData.isDefined(p.Identifier.Code, index.Str))
						throw new CodeEE("文字列" + index.Str + "は配列変数" + p.Identifier.Name + "の要素ではありません");
				}
				if (p.Identifier.IsString)
				{
					string src = spvarsetarg.Term.GetStrValue(exm);
					exm.VEvaluator.SetValueAllEachChara(p, index, src, start, end);
				}
				else
				{
					long src = spvarsetarg.Term.GetIntValue(exm);
					exm.VEvaluator.SetValueAllEachChara(p, index, src, start, end);
				}
			}
		}

		private sealed class RANDOMIZE_Instruction : AbstractInstruction
		{
			public RANDOMIZE_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.INT_EXPRESSION_NULLABLE);
				flag = METHOD_SAFE | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				Int64 iValue;
				if (func.Argument.IsConst)
					iValue = func.Argument.ConstInt;
				else
					iValue = ((ExpressionArgument)func.Argument).Term.GetIntValue(exm);
				exm.VEvaluator.Randomize(iValue);
			}
		}
		private sealed class INITRAND_Instruction : AbstractInstruction
		{
			public INITRAND_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.VOID);
				flag = METHOD_SAFE | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				exm.VEvaluator.InitRanddata();
			}
		}

		private sealed class DUMPRAND_Instruction : AbstractInstruction
		{
			public DUMPRAND_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.VOID);
				flag = METHOD_SAFE | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				exm.VEvaluator.DumpRanddata();
			}
		}


		private sealed class SAVEGLOBAL_Instruction : AbstractInstruction
		{
			public SAVEGLOBAL_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.VOID);
				flag = METHOD_SAFE | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				exm.VEvaluator.SaveGlobal();
			}
		}

		private sealed class LOADGLOBAL_Instruction : AbstractInstruction
		{
			public LOADGLOBAL_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.VOID);
				flag = METHOD_SAFE | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				if (exm.VEvaluator.LoadGlobal())
					exm.VEvaluator.RESULT = 1;
				else
					exm.VEvaluator.RESULT = 0;
			}
		}

		private sealed class RESETDATA_Instruction : AbstractInstruction
		{
			public RESETDATA_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.VOID);
				flag = METHOD_SAFE | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				exm.VEvaluator.ResetData();
				exm.Console.ResetStyle();
			}
		}

		private sealed class RESETGLOBAL_Instruction : AbstractInstruction
		{
			public RESETGLOBAL_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.VOID);
				flag = METHOD_SAFE | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				exm.VEvaluator.ResetGlobalData();
			}
		}

		private static int toUInt32inArg(Int64 value, string funcName, int argnum)
		{
			if (value < 0)
				throw new CodeEE(funcName + "の第" + argnum.ToString() + "引数に負の値(" + value.ToString() + ")が指定されました");
			else if (value > Int32.MaxValue)
				throw new CodeEE(funcName + "の第" + argnum.ToString() + "引数の値(" + value.ToString() + ")が大きすぎます");

			return (int)value;
		}

		private sealed class SAVECHARA_Instruction : AbstractInstruction
		{
			public SAVECHARA_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_SAVECHARA);
				flag = METHOD_SAFE | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				//TODO
				throw new CodeEE("未実装の命令です");
				ExpressionArrayArgument arg = (ExpressionArrayArgument)func.Argument;
				IOperandTerm[] terms = arg.TermList;
				int target = FunctionIdentifier.toUInt32inArg(terms[0].GetIntValue(exm), "SAVECHARA", 1);
				string savMes = terms[1].GetStrValue(exm);
				int[] savCharaList = new int[terms.Length - 2];
				int charanum = (int)exm.VEvaluator.CHARANUM;
				for (int i = 0; i < savCharaList.Length; i++)
				{
					Int64 v = terms[i + 2].GetIntValue(exm);
					savCharaList[i] = FunctionIdentifier.toUInt32inArg(v, "SAVECHARA", i + 3);
					if (savCharaList[i] >= charanum)
						throw new CodeEE("SAVECHARAの第" +(i+3).ToString() + "引数の値がキャラ登録番号の範囲を超えています");
					for (int j = 0; j < i; j++)
					{
						if (savCharaList[i] == savCharaList[j])
							throw new CodeEE("同一のキャラ登録番号(" +(savCharaList[i]).ToString() + ")が複数回指定されました");
					}
				}
				exm.VEvaluator.SaveChara(target, savMes, savCharaList);
			}
		}

		private sealed class LOADCHARA_Instruction : AbstractInstruction
		{
			public LOADCHARA_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.INT_EXPRESSION);
				flag = METHOD_SAFE | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				//TODO
				throw new CodeEE("未実装の命令です");
				ExpressionArgument arg = (ExpressionArgument)func.Argument;
				Int64 target;
				if (arg.IsConst)
					target = arg.ConstInt;
				else
					target = arg.Term.GetIntValue(exm);
				int target32 = FunctionIdentifier.toUInt32inArg(target, "LOADCHARA", 1);
				exm.VEvaluator.LoadChara(target32);
			}
		}


		private sealed class SAVEVAR_Instruction : AbstractInstruction
		{
			public SAVEVAR_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_SAVEVAR);
				flag = METHOD_SAFE | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				//TODO
				throw new CodeEE("未実装の命令です");
				SpSaveVarArgument arg = (SpSaveVarArgument)func.Argument;
				VariableToken[] vars = arg.VarTokens;
				int target = FunctionIdentifier.toUInt32inArg(arg.Term.GetIntValue(exm), "SAVECHARA", 1);
				string savMes = arg.SavMes.GetStrValue(exm);
				exm.VEvaluator.SaveVariable(target,savMes, vars);
			}
		}
		private sealed class LOADVAR_Instruction : AbstractInstruction
		{
			public LOADVAR_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.INT_EXPRESSION);
				flag = METHOD_SAFE | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				//TODO
				throw new CodeEE("未実装の命令です");
				ExpressionArgument arg = (ExpressionArgument)func.Argument;
				Int64 target;
				if (arg.IsConst)
					target = arg.ConstInt;
				else
					target = arg.Term.GetIntValue(exm);
				int target32 = FunctionIdentifier.toUInt32inArg(target, "LOADVAR", 1);
				exm.VEvaluator.LoadVariable(target32);

			}
		}

		private sealed class DELDATA_Instruction : AbstractInstruction
		{
			public DELDATA_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.INT_EXPRESSION);
				flag = METHOD_SAFE | EXTENDED;
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				Int64 target;
				if (func.Argument.IsConst)
					target = func.Argument.ConstInt;
				else
					target = ((ExpressionArgument)func.Argument).Term.GetIntValue(exm);

				int target32 = FunctionIdentifier.toUInt32inArg(target, "DELDATA", 1);
				exm.VEvaluator.DelData(target32);
			}
		}
		
		#endregion


		#region flowControlFunction

		private sealed class BEGIN_Instruction : AbstractInstruction
		{
			public BEGIN_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.STR);
				flag = FLOW_CONTROL;
			}
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				string keyword = func.Argument.ConstStr;
				if (Config.ICFunction)//1756 BEGINのキーワードは関数扱いらしい
					keyword = keyword.ToUpper();
				state.SetBegin(keyword);
				state.Return(0);
				exm.Console.ResetStyle();
			}
		}

		private sealed class SAVELOADGAME_Instruction : AbstractInstruction
		{
			public SAVELOADGAME_Instruction(bool isSave)
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.VOID);
				flag = FLOW_CONTROL;
				this.isSave = isSave;
			}
			readonly bool isSave;
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				if ((state.SystemState & SystemStateCode.__CAN_SAVE__) != SystemStateCode.__CAN_SAVE__)
				{
					string funcName = state.Scope;
					if (funcName == null)
						funcName = "";
					throw new CodeEE("@" + funcName + "中でSAVEGAME/LOADGAME命令を実行することはできません");
				}
				GlobalStatic.Process.saveCurrentState(true);
                //バックアップに入れた旧ProcessStateの方を参照するため、ここでstateは使えない
                GlobalStatic.Process.getCurrentState.SaveLoadData(isSave);
			}
		}

		private sealed class REPEAT_Instruction : AbstractInstruction
		{
			public REPEAT_Instruction(bool fornext)
			{
				flag = METHOD_SAFE | FLOW_CONTROL | PARTIAL;
				if (fornext)
				{
					ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_FOR_NEXT);
					flag |= EXTENDED;
				}
				else
				{
					ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.INT_EXPRESSION);
				}
			}
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				SpForNextArgment forArg = (SpForNextArgment)func.Argument;
				func.LoopCounter = forArg.Cnt;
				//1.725 順序変更。REPEATにならう。
				func.LoopCounter.SetValue(forArg.Start.GetIntValue(exm), exm);
				func.LoopEnd = forArg.End.GetIntValue(exm);
				func.LoopStep = forArg.Step.GetIntValue(exm);
				if ((func.LoopStep > 0) && (func.LoopEnd > func.LoopCounter.GetIntValue(exm)))//まだ回数が残っているなら、
					return;//そのまま次の行へ
				else if ((func.LoopStep < 0) && (func.LoopEnd < func.LoopCounter.GetIntValue(exm)))//まだ回数が残っているなら、
					return;//そのまま次の行へ
				state.JumpTo(func.JumpTo);
			}
		}

		private sealed class WHILE_Instruction : AbstractInstruction
		{
			public WHILE_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.INT_EXPRESSION);
				flag = METHOD_SAFE | EXTENDED | FLOW_CONTROL | PARTIAL;
			}
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				ExpressionArgument expArg = (ExpressionArgument)func.Argument;
				if (expArg.Term.GetIntValue(exm) != 0)//式が真
					return;//そのまま中の処理へ
				state.JumpTo(func.JumpTo);
			}
		}

		private sealed class SIF_Instruction : AbstractInstruction
		{
			public SIF_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.INT_EXPRESSION);
				flag = METHOD_SAFE | FLOW_CONTROL | PARTIAL | FORCE_SETARG;
			}

			public override void SetJumpTo(ref bool useCallForm, InstructionLine func, int currentDepth, ref string FunctionoNotFoundName)
			{
				LogicalLine jumpto = func.NextLine;
				if ((jumpto == null) || (jumpto.NextLine == null) ||
					(jumpto is FunctionLabelLine) || (jumpto is NullLine))
				{
					ParserMediator.Warn("SIF文の次の行がありません", func, 2, true, false);
					return;
				}
				else if (jumpto is InstructionLine)
				{
					InstructionLine sifFunc = (InstructionLine)jumpto;
					if (sifFunc.Function.IsPartial())
						ParserMediator.Warn("SIF文の次の行を" + sifFunc.Function.Name + "文にすることはできません", func, 2, true, false);
					else
						func.JumpTo = func.NextLine.NextLine;
				}
				else if (jumpto is GotoLabelLine)
					ParserMediator.Warn("SIF文の次の行をラベル行にすることはできません", func, 2, true, false);
				else
					func.JumpTo = func.NextLine.NextLine;

				if ((func.JumpTo != null) && (func.Position.LineNo + 1 != func.NextLine.Position.LineNo))
					ParserMediator.Warn("SIF文の次の行が空行またはコメント行です(eramaker:SIF文は意味を失います)", func, 0, false, true);
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				ExpressionArgument expArg = (ExpressionArgument)func.Argument;
				if (expArg.Term.GetIntValue(exm) == 0)//評価式が真ならそのまま流れ落ちる
					state.ShiftNextLine();//偽なら一行とばす。順に来たときと同じ扱いにする
			}
		}

		private sealed class ELSEIF_Instruction : AbstractInstruction
		{
			public ELSEIF_Instruction(FunctionArgType argtype)
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(argtype);
				flag = METHOD_SAFE | FLOW_CONTROL | PARTIAL | FORCE_SETARG;
			}
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				//if (iFuncCode == FunctionCode.ELSE || iFuncCode == FunctionCode.ELSEIF
				//	|| iFuncCode == FunctionCode.CASE || iFuncCode == FunctionCode.CASEELSE)
				//チェック済み
				//if (func.JumpTo == null)
				//	throw new ExeEE(func.Function.Name + "のジャンプ先が設定されていない");
				state.JumpTo(func.JumpTo);
			}
		}
		private sealed class ENDIF_Instruction : AbstractInstruction
		{
			public ENDIF_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.VOID);
				flag = FLOW_CONTROL | PARTIAL | FORCE_SETARG;
			}
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
			}
		}

		private sealed class IF_Instruction : AbstractInstruction
		{
			public IF_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.INT_EXPRESSION);
				flag = METHOD_SAFE | FLOW_CONTROL | PARTIAL | FORCE_SETARG;
			}
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				LogicalLine ifJumpto = func.JumpTo;//ENDIF
				//チェック済み
				//if (func.IfCaseList == null)
				//	throw new ExeEE("IFのIF-ELSEIFリストが適正に作成されていない");
				//if (func.JumpTo == null)
				//	throw new ExeEE("IFに対応するENDIFが設定されていない");

                InstructionLine line = null;
				for (int i = 0; i < func.IfCaseList.Count; i++)
				{
                    line = func.IfCaseList[i];
					if (line.IsError)
						continue;
					if (line.FunctionCode == FunctionCode.ELSE)
					{
						ifJumpto = line;
						break;
					}

					//ExpressionArgument expArg = (ExpressionArgument)(line.Argument);
					//チェック済み
					//if (expArg == null)
					//	throw new ExeEE("IFチェック中。引数が解析されていない。", func.IfCaseList[i].Position);

					//1730 ELSEIFが出したエラーがIFのエラーとして検出されていた
                    state.CurrentLine = line;
                    Int64 value = ((ExpressionArgument)(line.Argument)).Term.GetIntValue(exm);
                    if (value != 0)//式が真
					{
						ifJumpto = line;
						break;
					}
				}
				if (ifJumpto != func)//自分自身がジャンプ先ならそのまま
					state.JumpTo(ifJumpto);
                //state.RunningLine = null;
            }
		}


		private sealed class SELECTCASE_Instruction : AbstractInstruction
		{
			public SELECTCASE_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.EXPRESSION);
				flag = METHOD_SAFE | EXTENDED | FLOW_CONTROL | PARTIAL | FORCE_SETARG;
			}
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
                LogicalLine caseJumpto = func.JumpTo;//ENDSELECT
				IOperandTerm selectValue = ((ExpressionArgument)func.Argument).Term;
				string sValue = null;
				Int64 iValue = 0;
				if (selectValue.IsInteger)
					iValue = selectValue.GetIntValue(exm);
				else
					sValue = selectValue.GetStrValue(exm);
				//チェック済み
				//if (func.IfCaseList == null)
				//	throw new ExeEE("SELECTCASEのCASEリストが適正に作成されていない");
				//if (func.JumpTo == null)
				//	throw new ExeEE("SELECTCASEに対応するENDSELECTが設定されていない");
                InstructionLine line = null;
                for (int i = 0; i < func.IfCaseList.Count; i++)
				{
                    line = func.IfCaseList[i];
					if (line.IsError)
						continue;
                    if (line.FunctionCode == FunctionCode.CASEELSE)
					{
                        caseJumpto = line;
                        break;
					}
                    CaseArgument caseArg = (CaseArgument)(line.Argument);
					//チェック済み
					//if (caseArg == null)
					//	throw new ExeEE("CASEチェック中。引数が解析されていない。", func.IfCaseList[i].Position);

                    state.CurrentLine = line;
                    if (selectValue.IsInteger)
					{
						Int64 Is = iValue;
						foreach (CaseExpression caseExp in caseArg.CaseExps)
						{
							if (caseExp.GetBool(Is, exm))
							{
                                caseJumpto = line;
								goto casefound;
							}
						}
					}
					else
					{
						string Is = sValue;
						foreach (CaseExpression caseExp in caseArg.CaseExps)
						{
							if (caseExp.GetBool(Is, exm))
							{
								caseJumpto = line;
								goto casefound;
							}
						}
					}

				}
			casefound:
				state.JumpTo(caseJumpto);
                //state.RunningLine = null;
            }
		}

		private sealed class RETURNFORM_Instruction : AbstractInstruction
		{
			public RETURNFORM_Instruction()
			{
                //ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.FORM_STR_ANY);
                ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.FORM_STR);
                flag = EXTENDED | FLOW_CONTROL;
            }
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
                //int termnum = 0;
                //foreach (IOperandTerm term in ((ExpressionArrayArgument)func.Argument).TermList)
                //{
                //    string arg = term.GetStrValue(exm);
                //    StringStream aSt = new StringStream(arg);
                //    WordCollection wc = LexicalAnalyzer.Analyse(aSt, LexEndWith.EoL, false, false);
                //    exm.VEvaluator.SetResultX((ExpressionParser.ReduceIntegerTerm(wc, TermEndWith.EoL).GetIntValue(exm)), termnum);
                //    termnum++;
                //}
                //state.Return(exm.VEvaluator.RESULT);
                //if (state.ScriptEnd)
                //    return;
                //int termnum = 0;
                StringStream aSt = new StringStream(((ExpressionArgument)func.Argument).Term.GetStrValue(exm));
                List<long> termList = new List<long>();
                while (!aSt.EOS)
                {
                    WordCollection wc = LexicalAnalyzer.Analyse(aSt, LexEndWith.Comma, false, false);
                    //exm.VEvaluator.SetResultX(ExpressionParser.ReduceIntegerTerm(wc, TermEndWith.EoL).GetIntValue(exm), termnum++);
                    termList.Add(ExpressionParser.ReduceIntegerTerm(wc, TermEndWith.EoL).GetIntValue(exm));
                    aSt.ShiftNext();
                    LexicalAnalyzer.SkipHalfSpace(aSt);
                    //termnum++;
                }
                if (termList.Count == 0)
                    termList.Add(0);
                exm.VEvaluator.SetResultX(termList);
                state.Return(exm.VEvaluator.RESULT);
                if (state.ScriptEnd)
                    return;
            }
		}

		private sealed class RETURN_Instruction : AbstractInstruction
		{
			public RETURN_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.INT_ANY);
				flag = FLOW_CONTROL;
			}
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				//int termnum = 0;
                ExpressionArrayArgument expArrayArg = (ExpressionArrayArgument)func.Argument;
                if (expArrayArg.TermList.Length == 0)
                {
                    exm.VEvaluator.RESULT = 0;
                    state.Return(0);
                    return;
                }
                List<long> termList = new List<long>();
                foreach (IOperandTerm term in expArrayArg.TermList)
				{
                    termList.Add(term.GetIntValue(exm));
					//exm.VEvaluator.SetResultX(term.GetIntValue(exm), termnum++);
				}
                if (termList.Count == 0)
                    termList.Add(0);
                exm.VEvaluator.SetResultX(termList);
				state.Return(exm.VEvaluator.RESULT);
			}
		}

		private sealed class CATCH_Instruction : AbstractInstruction
		{
			public CATCH_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.VOID);
				flag = METHOD_SAFE | EXTENDED | FLOW_CONTROL | PARTIAL;
			}
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				//if (sequential)//上から流れてきたなら何もしないでENDCATCHに飛ぶ
				state.JumpTo(func.JumpToEndCatch);
			}
		}

		private sealed class RESTART_Instruction : AbstractInstruction
		{
			public RESTART_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.VOID);
				flag = METHOD_SAFE | EXTENDED;
			}
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				state.JumpTo(func.ParentLabelLine);
			}
		}

		private sealed class BREAK_Instruction : AbstractInstruction
		{
			public BREAK_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.VOID);
				flag = METHOD_SAFE | FLOW_CONTROL;
			}
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				////BREAKのJUMP先はRENDまたはNEXT。そのジャンプ先であるREPEATかFORをiLineに代入。
				//1.723 仕様変更。BREAKのJUMP先にはREPEAT、FOR、WHILEを記憶する。そのJUMP先が本当のJUMP先。
				InstructionLine jumpTo = (InstructionLine)func.JumpTo;
				InstructionLine iLine = (InstructionLine)jumpTo.JumpTo;
				//WHILEとDOはカウンタがないので、即ジャンプ
				if (jumpTo.FunctionCode != FunctionCode.WHILE && jumpTo.FunctionCode != FunctionCode.DO)
				{
					unchecked
					{//本家ではBREAK時にCOUNTが回る
						jumpTo.LoopCounter.PlusValue(jumpTo.LoopStep, exm);
					}
				}
				state.JumpTo(iLine);
			}
		}

		private sealed class CONTINUE_Instruction : AbstractInstruction
		{
			public CONTINUE_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.VOID);
				flag = METHOD_SAFE | FLOW_CONTROL;
			}
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				InstructionLine jumpTo = (InstructionLine)func.JumpTo;
				if ((jumpTo.FunctionCode == FunctionCode.REPEAT) || (jumpTo.FunctionCode == FunctionCode.FOR))
				{
					//ループ変数が不明(REPEAT、FORを経由せずにループしようとした場合は無視してループを抜ける(eramakerがこういう仕様だったりする))
					if (jumpTo.LoopCounter == null)
					{
						state.JumpTo(jumpTo.JumpTo);
						return;
					}
					unchecked
					{
						jumpTo.LoopCounter.PlusValue(jumpTo.LoopStep, exm);
					}
					Int64 counter = jumpTo.LoopCounter.GetIntValue(exm);
					//まだ回数が残っているなら、
					if (((jumpTo.LoopStep > 0) && (jumpTo.LoopEnd > counter))
						|| ((jumpTo.LoopStep < 0) && (jumpTo.LoopEnd < counter)))
						state.JumpTo(func.JumpTo);
					else
						state.JumpTo(jumpTo.JumpTo);
					return;
				}
				if (jumpTo.FunctionCode == FunctionCode.WHILE)
				{
					if (((ExpressionArgument)jumpTo.Argument).Term.GetIntValue(exm) != 0)
						state.JumpTo(func.JumpTo);
					else
						state.JumpTo(jumpTo.JumpTo);
					return;
				}
				if (jumpTo.FunctionCode == FunctionCode.DO)
				{
                    //こいつだけはCONTINUEよりも後ろに判定行があるため、判定行にエラーがあった場合に問題がある
					InstructionLine tFunc = (InstructionLine)((InstructionLine)func.JumpTo).JumpTo;//LOOP
                    if (tFunc.IsError)
                        throw new CodeEE(tFunc.ErrMes, tFunc.Position);
					ExpressionArgument expArg = (ExpressionArgument)tFunc.Argument;
					if (expArg.Term.GetIntValue(exm) != 0)//式が真
						state.JumpTo(jumpTo);//DO
					else
						state.JumpTo(tFunc);//LOOP
					return;
				}
				throw new ExeEE("異常なCONTINUE");
			}
		}

		private sealed class REND_Instruction : AbstractInstruction
		{
			public REND_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.VOID);
				flag = METHOD_SAFE | FLOW_CONTROL | PARTIAL;
			}
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				InstructionLine jumpTo = (InstructionLine)func.JumpTo;
				//ループ変数が不明(REPEAT、FORを経由せずにループしようとした場合は無視してループを抜ける(eramakerがこういう仕様だったりする))
				if (jumpTo.LoopCounter == null)
				{
					state.JumpTo(jumpTo.JumpTo);
					return;
				}
				unchecked
				{
					jumpTo.LoopCounter.PlusValue(jumpTo.LoopStep, exm);
				}
				Int64 counter = jumpTo.LoopCounter.GetIntValue(exm);
				//まだ回数が残っているなら、
				if (((jumpTo.LoopStep > 0) && (jumpTo.LoopEnd > counter))
					|| ((jumpTo.LoopStep < 0) && (jumpTo.LoopEnd < counter)))
					state.JumpTo(func.JumpTo);
			}
		}

		private sealed class WEND_Instruction : AbstractInstruction
		{
			public WEND_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.VOID);
				flag = METHOD_SAFE | EXTENDED | FLOW_CONTROL | PARTIAL;
			}
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				InstructionLine jumpTo = (InstructionLine)func.JumpTo;
				if (((ExpressionArgument)jumpTo.Argument).Term.GetIntValue(exm) != 0)
					state.JumpTo(func.JumpTo);
			}
		}

		private sealed class LOOP_Instruction : AbstractInstruction
		{
			public LOOP_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.INT_EXPRESSION);
				flag = METHOD_SAFE | EXTENDED | FLOW_CONTROL | PARTIAL | FORCE_SETARG;
			}
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				ExpressionArgument expArg = (ExpressionArgument)func.Argument;
				if (expArg.Term.GetIntValue(exm) != 0)//式が真
					state.JumpTo(func.JumpTo);
			}
		}


		private sealed class RETURNF_Instruction : AbstractInstruction
		{
			public RETURNF_Instruction()
			{
				ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.EXPRESSION_NULLABLE);
				flag = METHOD_SAFE | EXTENDED | FLOW_CONTROL;
			}

			public override void SetJumpTo(ref bool useCallForm, InstructionLine func, int currentDepth, ref string FunctionoNotFoundName)
			{
				FunctionLabelLine label = func.ParentLabelLine;
				if (!label.IsMethod)
				{
					ParserMediator.Warn("RETURNFは#FUNCTION以外では使用できません", func, 2, true, false);
				}
				if (func.Argument != null)
				{
					IOperandTerm term = ((ExpressionArgument)func.Argument).Term;
					if (term != null)
					{
						if (label.MethodType != term.GetOperandType())
						{
							if (label.MethodType == typeof(Int64))
								ParserMediator.Warn("#FUNCTIONで始まる関数の戻り値に文字列型が指定されました", func, 2, true, false);
							else if (label.MethodType == typeof(string))
								ParserMediator.Warn("#FUCNTIONSで始まる関数の戻り値に数値型が指定されました", func, 2, true, false);
						}
					}
				}
			}

			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				IOperandTerm term = ((ExpressionArgument)func.Argument).Term;
				SingleTerm ret = null;
				if (term != null)
				{
					ret = term.GetValue(exm);
				}
				state.ReturnF(ret);
			}
		}

		private sealed class CALL_Instruction : AbstractInstruction
		{
			public CALL_Instruction(bool form, bool isJump, bool isTry, bool isTryCatch)
			{
				if (form)
					ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_CALLFORM);
				else
					ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_CALL);
				flag = FLOW_CONTROL | FORCE_SETARG;
				if (isJump)
					flag |= IS_JUMP;
				if (isTry)
					flag |= IS_TRY;
				if (isTryCatch)
					flag |= IS_TRYC | PARTIAL;
				this.isJump = isJump;
				this.isTry = isTry;
			}
			readonly bool isJump;
			readonly bool isTry;

			public override void SetJumpTo(ref bool useCallForm, InstructionLine func, int currentDepth, ref string FunctionoNotFoundName)
			{
				if (!func.Argument.IsConst)
				{
					useCallForm = true;
					return;
				}
				SpCallArgment callArg = (SpCallArgment)func.Argument;
				string labelName = callArg.ConstStr;
				if (Config.ICFunction)
					labelName = labelName.ToUpper();
				CalledFunction call = CalledFunction.CallFunction(GlobalStatic.Process, labelName, func);
				if ((call == null) && (!func.Function.IsTry()))
				{
					FunctionoNotFoundName = labelName;
					return;
				}
				if (call != null)
				{
					func.JumpTo = call.TopLabel;
					if (call.TopLabel.Depth < 0)
						call.TopLabel.Depth = currentDepth + 1;
					if (call.TopLabel.IsError)
					{
						func.IsError = true;
						func.ErrMes = call.TopLabel.ErrMes;
						return;
					}
					string errMes;
					callArg.UDFArgument = call.ConvertArg(callArg.RowArgs, out errMes);
					if (callArg.UDFArgument == null)
					{
						ParserMediator.Warn(errMes, func, 2, true, false);
						return;
					}
				}
				callArg.CallFunc = call;
			}
			
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				SpCallArgment spCallArg = (SpCallArgment)func.Argument;
				CalledFunction call = null;
				string labelName = null;
				UserDefinedFunctionArgument arg = null;
				if (spCallArg.IsConst)
				{
					call = spCallArg.CallFunc;
					labelName = spCallArg.ConstStr;
					arg = spCallArg.UDFArgument;
				}
				else
				{
					labelName = spCallArg.FuncnameTerm.GetStrValue(exm);
					if (Config.ICFunction)
						labelName = labelName.ToUpper();
					call = CalledFunction.CallFunction(GlobalStatic.Process, labelName, func);
				}
				if (call == null)
				{
					if (!isTry)
						throw new CodeEE("関数\"@" + labelName + "\"が見つかりません");
					if (func.JumpToEndCatch != null)
						state.JumpTo(func.JumpToEndCatch);
					return;
				}
				call.IsJump = isJump;
				if (arg == null)
				{
					string errMes;
					arg = call.ConvertArg(spCallArg.RowArgs, out errMes);
					if (arg == null)
						throw new CodeEE(errMes);
				}
				state.IntoFunction(call, arg, exm);
			}
		}

		private sealed class GOTO_Instruction : AbstractInstruction
		{
			public GOTO_Instruction(bool form, bool isTry, bool isTryCatch)
			{
				if (form)
					ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_CALLFORM);
				else
					ArgBuilder = ArgumentParser.GetArgumentBuilder(FunctionArgType.SP_CALL);
				this.isTry = isTry;
				flag = METHOD_SAFE | FLOW_CONTROL | FORCE_SETARG;
				if (isTry)
					flag |= IS_TRY;
				if (isTryCatch)
					flag |= IS_TRYC | PARTIAL;
			}
			readonly bool isTry;

			public override void SetJumpTo(ref bool useCallForm, InstructionLine func, int currentDepth, ref string FunctionoNotFoundName)
			{
				GotoLabelLine jumpto = null;
				func.JumpTo = null;
				if (func.Argument.IsConst)
				{
					string labelName = func.Argument.ConstStr;
					if (Config.ICVariable)//eramakerではGOTO文は大文字小文字を区別しない
						labelName = labelName.ToUpper();
					jumpto = GlobalStatic.LabelDictionary.GetLabelDollar(labelName, func.ParentLabelLine);
                    if ((jumpto == null) && (!func.Function.IsTry()))
                        ParserMediator.Warn("指定されたラベル名\"$" + labelName + "\"は現在の関数内に存在しません", func, 2, true, false);
                    else if (jumpto.IsError)
                        ParserMediator.Warn("指定されたラベル名\"$" + labelName + "\"は無効な$ラベル行です", func, 2, true, false);
                    else if (jumpto != null)
                    {
                        func.JumpTo = jumpto;
                    }
				}
			}
			public override void DoInstruction(ExpressionMediator exm, InstructionLine func, ProcessState state)
			{
				string label = null;
				LogicalLine jumpto = null;
				if (func.Argument.IsConst)
				{
					label = func.Argument.ConstStr;
					jumpto = func.JumpTo;
				}
				else
				{
					label = ((SpCallArgment)func.Argument).FuncnameTerm.GetStrValue(exm);
					if (Config.ICVariable)
						label = label.ToUpper();
					jumpto = state.CurrentCalled.CallLabel(GlobalStatic.Process, label);
				}
                if (jumpto == null)
                {
                    if (!func.Function.IsTry())
                        throw new CodeEE("指定されたラベル名\"$" + label + "\"は現在の関数内に存在しません");
                    if (func.JumpToEndCatch != null)
                        state.JumpTo(func.JumpToEndCatch);
                    return;
                }
                else if (jumpto.IsError)
                    throw new CodeEE("指定されたラベル名\"$" + label + "\"は無効な$ラベル行です");
				state.JumpTo(jumpto);
			}
		}
		#endregion
	}
}