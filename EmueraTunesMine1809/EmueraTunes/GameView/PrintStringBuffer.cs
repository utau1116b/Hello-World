using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using MinorShift.Emuera.Sub;

namespace MinorShift.Emuera.GameView
{
	internal sealed class PrintStringBuffer
	{
		public PrintStringBuffer(EmueraConsole parent)
		{
			this.parent = parent;
            makeButtonWrap = Config.ButtonWrap;
		}
		readonly EmueraConsole parent;
        readonly bool makeButtonWrap;
		StringBuilder builder = new StringBuilder();
		List<ConsoleStyledString> stringList = new List<ConsoleStyledString>();
		StringStyle lastStringStyle = new StringStyle();
		List<ConsoleButtonString> buttonList = new List<ConsoleButtonString>();

		public int BufferStrLength
		{
			get
			{
				int length = 0;
				foreach (ConsoleStyledString css in stringList)
					length += css.Length;
				return length;
			}
		}

		public void Append(string str, StringStyle style)
		{
			Append(str, style, false);
		}

		public void Append(string str, StringStyle style, bool force_button)
		{
			if (BufferStrLength > 2000)
				return;
			if (force_button)
				fromCssToButton();
			if ((builder.Length == 0) || (lastStringStyle == style))
			{
				if (builder.Length > 2000)
					return;
				if (builder.Length + str.Length > 2000)
					str = str.Substring(0, 2000 - builder.Length) + "※※※バッファーの文字数が2000字(全角1000字)を超えています。これ以降は表示できません※※※";
				builder.Append(str);
				lastStringStyle = style;
			}
			else
			{
				stringList.Add(new ConsoleStyledString(builder.ToString(), lastStringStyle));
				builder.Remove(0, builder.Length);
				builder.Append(str);
				lastStringStyle = style;
			}
			if (force_button)
				fromCssToButton();
		}

		public void AppendButton(string str, StringStyle style, string input)
		{
			fromCssToButton();
			stringList.Add(new ConsoleStyledString(str, style));
			if (stringList.Count == 0)
				return;
			buttonList.Add(createButton(stringList,input));
			stringList.Clear();
		}

        public ConsoleDisplayLine AppendErrButton(string str, StringStyle style, string input, ScriptPosition pos, StringMeasure sm)
        {
            fromCssToButton();
            stringList.Add(new ConsoleStyledString(str, style));
            if (stringList.Count == 0)
                return null;
            buttonList.Add(createButton(stringList, input, pos));
            stringList.Clear();
            setLengthToButtonList(sm);
            ConsoleButtonString[] dispLineButtonArray = new ConsoleButtonString[buttonList.Count];
            buttonList.CopyTo(dispLineButtonArray);
            ConsoleDisplayLine line = new ConsoleDisplayLine(parent, dispLineButtonArray, true, false);
            this.Clear();
            return line;
        }


		public void AppendButton(string str, StringStyle style, long input)
		{
			fromCssToButton();
			stringList.Add(new ConsoleStyledString(str, style));
			if (stringList.Count == 0)
				return;
			buttonList.Add(createButton(stringList, input));
			stringList.Clear();
		}

        public void AppendPlainText(string str, StringStyle style)
        {
            fromCssToButton();
            stringList.Add(new ConsoleStyledString(str, style));
            if (stringList.Count == 0)
                return;
            buttonList.Add(createPlainButton(stringList));
            stringList.Clear();
        }

		public bool IsEmpty
		{ 
			get
			{
				return ((buttonList.Count == 0) && (builder.Length == 0) && (stringList.Count == 0)); 
			} 
		}

		public void Clear()
		{
			builder.Remove(0, builder.Length);
			stringList.Clear();
			buttonList.Clear();
		}

		public override string ToString()
		{
			StringBuilder buf = new StringBuilder();
			foreach (ConsoleButtonString button in buttonList)
				buf.Append(button.ToString());
			foreach (ConsoleStyledString css in stringList)
				buf.Append(css.Str);
			buf.Append(builder);
			return buf.ToString();
		}

		public ConsoleDisplayLine FlushSingleLine(StringMeasure stringMeasure, bool temporary)
		{
			fromCssToButton();
			setLengthToButtonList(stringMeasure);
			ConsoleButtonString[] dispLineButtonArray = new ConsoleButtonString[buttonList.Count];
			buttonList.CopyTo(dispLineButtonArray);
			ConsoleDisplayLine line = new ConsoleDisplayLine(parent, dispLineButtonArray, true, temporary);
			this.Clear();
			return line;
		}

		public ConsoleDisplayLine[] Flush(StringMeasure stringMeasure, bool temporary)
		{
			fromCssToButton();
			setLengthToButtonList(stringMeasure);
			if (buttonList.Count == 0)
				return new ConsoleDisplayLine[0];
			List<ConsoleDisplayLine> lineList = new List<ConsoleDisplayLine>();
			List<ConsoleButtonString> lineButtonList = new List<ConsoleButtonString>();
			ConsoleButtonString[] dispLineButtonArray = null;
			int shiftX = 0;
            int windowWidth = Config.WindowX;
			bool firstLine = true;
			bool newLine = false;
			for (int i = 0; i < buttonList.Count; i++)
			{
				if (newLine)
				{
					shiftX = buttonList[i].PointX;
					for (int j = i; j < buttonList.Count; j++)
						buttonList[j].ShiftPositionX(-shiftX);
					newLine = false;
				}
				if (buttonList[i].PointX + buttonList[i].Width > windowWidth)
				{//ここから新しい表示行
					if ((!makeButtonWrap) || (lineButtonList.Count == 0) || (!buttonList[i].IsButton && !Config.CompatiLinefeedAs1739))
					{//ボタン分割
						int divIndex =getDivideIndex(buttonList[i], stringMeasure);
						if (divIndex > 0)
						{
							ConsoleButtonString newButton = buttonList[i].DivideAt(divIndex, stringMeasure);
							newButton.SetPointX(buttonList[i].PointX + buttonList[i].Width);
							buttonList.Insert(i + 1, newButton);
							lineButtonList.Add(buttonList[i]);
							i++;
						}
					}
					dispLineButtonArray = new ConsoleButtonString[lineButtonList.Count];
					lineButtonList.CopyTo(dispLineButtonArray);
					lineList.Add(new ConsoleDisplayLine(parent, dispLineButtonArray, firstLine, temporary));
					firstLine = false;
					lineButtonList.Clear();
					newLine = true;
					i--;
					continue;
				}
				lineButtonList.Add(buttonList[i]);
				continue;
			}
			if (lineButtonList.Count > 0)
			{
				dispLineButtonArray = new ConsoleButtonString[lineButtonList.Count];
				lineButtonList.CopyTo(dispLineButtonArray);
				lineList.Add(new ConsoleDisplayLine(parent, dispLineButtonArray, firstLine, temporary));
			}

			//if (StaticConfig.ButtonWrap)
			//{//ボタン処理優先
			//    setLengthToButtonList(stringMeasure, false);
			//    ConsoleButtonString[] buttons = createButtons(stringMeasure, stringList);
			//    List<ConsoleButtonString> buttonList = new List<ConsoleButtonString>();
			//    int shiftX = 0;
			//    int windowWidth = StaticConfig.WindowX;
			//    bool firstLine = true;
			//    bool newLine = false;
			//    for (int i = 0; i < buttons.Length; i++)
			//    {
			//        if (newLine)
			//        {
			//            shiftX = buttons[i].PointX;
			//            for (int j = i; j < buttons.Length; j++)
			//                buttons[j].ShiftPositionX(-shiftX);
			//            newLine = false;
			//        }
			//        if ((buttons[i].PointX + buttons[i].Width > windowWidth) && (buttonList.Count > 0))
			//        {//ここから新しい表示行
			//            dispLineButtonArray = new ConsoleButtonString[buttonList.Count];
			//            buttonList.CopyTo(dispLineButtonArray);
			//            lineList.Add(new ConsoleDisplayLine(parent, dispLineButtonArray, firstLine, temporary));
			//            firstLine = false;
			//            buttonList.Clear();
			//            newLine = true;
			//            i--;
			//            continue;
			//            //buttonList.Add(buttons[i]);
			//            //continue;
			//        }
			//        buttonList.Add(buttons[i]);
			//        continue;
			//    }
			//    if (buttonList.Count > 0)
			//    {
			//        dispLineButtonArray = new ConsoleButtonString[buttonList.Count];
			//        buttonList.CopyTo(dispLineButtonArray);
			//        lineList.Add(new ConsoleDisplayLine(parent, dispLineButtonArray, firstLine, temporary));
			//    }
			//}
			//else
			//{//折り返し処理優先
			//    setLengthToButtonList(stringMeasure, true);
			//    List<ConsoleStyledString> buttonCssList = new List<ConsoleStyledString>();
			//    int pointX = -1;
			//    bool firstLine = true;
			//    for (int i = 0; i < stringList.Count; i++)
			//    {
			//        if ((buttonCssList.Count > 0) && (stringList[i].PointX == 0))
			//        {//ここから新しい表示行
			//            dispLineButtonArray = createButtons(stringMeasure, buttonCssList);
			//            lineList.Add(new ConsoleDisplayLine(parent, dispLineButtonArray, firstLine, temporary));
			//            firstLine = false;
			//            buttonCssList.Clear();
			//            buttonCssList.Add(stringList[i]);
			//            pointX = stringList[i].PointX;
			//            continue;
			//        }
			//        buttonCssList.Add(stringList[i]);
			//        pointX = stringList[i].PointX;
			//        continue;
			//    }
			//    if (buttonCssList.Count > 0)
			//    {
			//        dispLineButtonArray = createButtons(stringMeasure, buttonCssList);
			//        lineList.Add(new ConsoleDisplayLine(parent, dispLineButtonArray, firstLine, temporary));
			//        firstLine = false;
			//    }
			//}
			ConsoleDisplayLine[] ret = new ConsoleDisplayLine[lineList.Count];
			lineList.CopyTo(ret);
			this.Clear();
			return ret;
		}

		#region Flush用privateメソッド

		/// <summary>
		/// cssListをbuttonに変換し、buttonListに追加。
		/// この時点ではWidthなどは考えない。
		/// </summary>
		private void fromCssToButton()
		{
			if (builder.Length != 0)
			{
				stringList.Add(new ConsoleStyledString(builder.ToString(), lastStringStyle));
				builder.Remove(0, builder.Length);
			}
			if (stringList.Count == 0)
				return;
			buttonList.AddRange(createButtons(stringList));
			stringList.Clear();
		}

		/// <summary>
		/// 物理行を１つのボタンへ。
		/// </summary>
		/// <returns></returns>
		private ConsoleButtonString createButton(List<ConsoleStyledString> cssList, string input)
		{
			ConsoleStyledString[] cssArray = new ConsoleStyledString[cssList.Count];
			cssList.CopyTo(cssArray);
			cssList.Clear();
			return new ConsoleButtonString(parent, cssArray, input);
		}
        private ConsoleButtonString createButton(List<ConsoleStyledString> cssList, string input, ScriptPosition pos)
        {
            ConsoleStyledString[] cssArray = new ConsoleStyledString[cssList.Count];
            cssList.CopyTo(cssArray);
            cssList.Clear();
            return new ConsoleButtonString(parent, cssArray, input, pos);
        }
        private ConsoleButtonString createButton(List<ConsoleStyledString> cssList, long input)
		{
			ConsoleStyledString[]  cssArray = new ConsoleStyledString[cssList.Count];
			cssList.CopyTo(cssArray);
			cssList.Clear();
			return new ConsoleButtonString(parent, cssArray, input);
		}
        private ConsoleButtonString createPlainButton(List<ConsoleStyledString> cssList)
        {
            ConsoleStyledString[] cssArray = new ConsoleStyledString[cssList.Count];
            cssList.CopyTo(cssArray);
            cssList.Clear();
            return new ConsoleButtonString(parent, cssArray);
        }

		/// <summary>
		/// 物理行をボタン単位に分割。引数のcssListの内容は変更される場合がある。
		/// </summary>
		/// <returns></returns>
		private ConsoleButtonString[] createButtons(List<ConsoleStyledString> cssList)
		{
			StringBuilder buf = new StringBuilder();
			for (int i = 0; i < cssList.Count; i++)
			{
				buf.Append(cssList[i].Str);
			}
			List<ButtonPrimitive> bpList = ButtonStringCreator.SplitButton(buf.ToString());
			ConsoleButtonString[] ret = new ConsoleButtonString[bpList.Count];
			ConsoleStyledString[] cssArray = null;
			if (ret.Length == 1)
			{
				cssArray = new ConsoleStyledString[cssList.Count];
				cssList.CopyTo(cssArray);
				if (bpList[0].CanSelect)
					ret[0] = new ConsoleButtonString(parent, cssArray, bpList[0].Input);
				else
					ret[0] = new ConsoleButtonString(parent, cssArray);
				return ret;
			}
			int cssStartCharIndex = 0;
			int buttonEndCharIndex = 0;
			int cssIndex = 0;
			List<ConsoleStyledString> buttonCssList = new List<ConsoleStyledString>();
			for (int i = 0; i < ret.Length; i++)
			{
				ButtonPrimitive bp = bpList[i];
				buttonEndCharIndex += bp.Str.Length;
				while (true)
				{
					if (cssIndex >= cssList.Count)
						break;
					ConsoleStyledString css = cssList[cssIndex];
					if (cssStartCharIndex + css.Str.Length >= buttonEndCharIndex)
					{//ボタンの終端を発見
						int used = buttonEndCharIndex - cssStartCharIndex;
						if (used > 0)
						{//cssの区切りの途中でボタンの区切りがある。
							ConsoleStyledString newCss = css.DivideAt(used);
							if (newCss != null)
							{
								cssList.Insert(cssIndex + 1, newCss);
								newCss.PointX = css.PointX + css.Width;
							}
						}
						buttonCssList.Add(css);
						cssStartCharIndex += css.Str.Length;
						cssIndex++;
						break;
					}
					//ボタンの終端はまだ先。
					buttonCssList.Add(css);
					cssStartCharIndex += css.Str.Length;
					cssIndex++;
				}
				cssArray = new ConsoleStyledString[buttonCssList.Count];
				buttonCssList.CopyTo(cssArray);
				if (bp.CanSelect)
					ret[i] = new ConsoleButtonString(parent, cssArray, bp.Input);
				else
					ret[i] = new ConsoleButtonString(parent, cssArray);
				buttonCssList.Clear();
			}
			return ret;

		}


		//stringListにPointX、Widthを追加
		private void setLengthToButtonList(StringMeasure stringMeasure)
		{
			int pointX = 0;
			int count = buttonList.Count;
			for (int i = 0; i < buttonList.Count; i++)
			{
				ConsoleButtonString button = buttonList[i];
				button.SetWidth(stringMeasure);
				button.SetPointX(pointX);
				pointX += button.Width;
				//if (!devide)//分割を行わないならどんどん追加していく
				//    continue;
				//while (pointX > StaticConfig.WindowX)
				//{//はみ出た
				//    int divideIndex = getDivideIndex(css, stringMeasure);
				//    if (divideIndex <= 0)
				//    {//分割不要。全部次の行に移動。
				//        css.PointX = 0;
				//        pointX = css.Width;
				//    }
				//    else if (divideIndex >= css.Str.Length)
				//    {//分割不要。全部その行に収まる
				//        pointX = 0;
				//        break;
				//    }
				//    else
				//    {//分割必要。divideIndex以降を次の行に移動。
				//        ConsoleStyledString newCss = css.DivideAt(divideIndex, stringMeasure);
				//        newCss.PointX = 0;
				//        pointX = newCss.Width;
				//        stringList.Insert(i + dividedCount + 1, newCss);
				//        dividedCount++;
				//    }
				//    css = stringList[i + dividedCount];
				//}
			}
		}

		private int getDivideIndex(ConsoleButtonString button, StringMeasure sm)
		{
			ConsoleStyledString divCss = null;
			int pointX = button.PointX;
			int strLength = 0;
			foreach (ConsoleStyledString css in button.StrArray)
			{
                if (pointX + css.Width > Config.WindowX)
				{
					divCss = css;
					break;
				}
				strLength += css.Str.Length;
				pointX += css.Width;
			}
			if (divCss != null)
				strLength += getDivideIndex(divCss, sm);
			return strLength;
		}

		private int getDivideIndex(ConsoleStyledString css, StringMeasure sm)
		{
            int widthLimit = Config.WindowX - css.PointX;
			string str = css.Str;
			Font font = css.Font;
			int point = 0;
			int highLength = str.Length - 1;//widthLimitを超える最低の文字index(文字数-1)。
			int lowLength = 0;//超えない最大の文字index。
			//int i = (int)(widthLimit / fontDisplaySize);//およその文字数を推定
			int i = 0;//およその文字数を推定
			if (i > str.Length - 1)//配列の外を参照しないように。
				i = str.Length - 1;
			string test = null;
			while ((highLength - lowLength) > 1)//差が一文字以下になるまで繰り返す。
			{
				test = str.Substring(0, i);
				point = sm.GetDisplayLength(test, font);
				if (point <= widthLimit)//サイズ内ならlowLengthを更新。文字数を増やす。
				{
					lowLength = i;
					i++;
				}
				else//サイズ外ならhighLengthを更新。文字数を減らす。
				{
					highLength = i;
					i--;
				}
			}
			return lowLength;
		}
		#endregion

	}
}
