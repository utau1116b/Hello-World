using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace MinorShift.Emuera.GameView
{
	/// <summary>
	/// 装飾付文字列(ConsoleStyledString)用のスタイル構造体
	/// </summary>
	internal struct StringStyle
	{
		public StringStyle(Color color, FontStyle fontStyle, string fontname)
		{
			this.Color = color;
			this.FontStyle = fontStyle;
			if (string.IsNullOrEmpty(fontname))
				Fontname = Config.FontName;
			else
				this.Fontname = fontname;
		}

		public Color Color;
		public FontStyle FontStyle;
		public string Fontname;
		public override bool Equals(object obj)
		{
			if ((obj == null) || (!(obj is StringStyle)))
				return false;
			StringStyle ss = (StringStyle)obj;
			return ((this.Color == ss.Color) && (this.FontStyle == ss.FontStyle) && (this.Fontname.Equals(ss.Fontname,  Config.SCIgnoreCase)));
		}
		public override int GetHashCode()
		{
			return Color.GetHashCode() ^ FontStyle.GetHashCode() ^ Fontname.GetHashCode();
		}
		public static bool operator ==(StringStyle x, StringStyle y)
		{
			return ((x.Color == y.Color) && (x.FontStyle == y.FontStyle) && (x.Fontname.Equals(y.Fontname, Config.SCIgnoreCase)));
		}
		public static bool operator !=(StringStyle x, StringStyle y)
		{
			return !(x == y);
		}
	}
}
