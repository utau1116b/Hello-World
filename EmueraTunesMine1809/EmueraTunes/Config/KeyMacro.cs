
using System.IO;
using MinorShift.Emuera.Sub;
using System;
using System.Windows.Forms;
namespace MinorShift.Emuera
{
	internal static class KeyMacro
	{
		readonly static string macroPath = Program.ExeDir + "macro.txt";

		public const int MaxMacro = 12;
		static string[] macro = new string[MaxMacro];
		static string[] macroName = new string[MaxMacro];
		static bool isMacroChanged = false;
		static KeyMacro()
		{
			for(int i = 0;i < MaxMacro; i++)
			{
				macro[i] = "";
				macroName[i] = "マクロキーF" + (i + 1).ToString() + ":";
			}
		}
		
        public static bool SaveMacro()
        {
            if (!isMacroChanged)
                return true;
			StreamWriter writer = null;

			try
			{
				writer = new StreamWriter(macroPath, false, Config.Encode);
				for (int i = 0; i < MaxMacro; i++)
				{
					writer.WriteLine(macroName[i] + macro[i]);
				}
			}
			catch (Exception)
			{
				return false;
			}
			finally
			{
				if (writer != null)
					writer.Close();
			}
			return true;
		}        

        public static void LoadMacroFile(string filename)
        {
            EraStreamReader eReader = new EraStreamReader();
            if (!eReader.Open(filename))
                return;
            try
            {
                string line = null;
                while ((line = eReader.ReadLine()) != null)
                {
                    if ((line.Length == 0) || (line[0] == ';'))
                        continue;
                    for(int i = 0; i < MaxMacro;i++)
                    {
						if (line.StartsWith(macroName[i]))
							macro[i] = line.Substring(macroName[i].Length);
					}
                }
            }
            catch { return; }
            finally { eReader.Dispose(); }
        }


        public static void SetMacro(Keys key, string macroStr)
        {
			int i = 0;
            switch (key)
            {
                case Keys.F1: i = 0; break;
                case Keys.F2: i = 1; break;
                case Keys.F3: i = 2; break;
                case Keys.F4: i = 3; break;
                case Keys.F5: i = 4; break;
                case Keys.F6: i = 5; break;
                case Keys.F7: i = 6; break;
                case Keys.F8: i = 7; break;
                case Keys.F9: i = 8; break;
                case Keys.F10: i = 9; break;
                case Keys.F11: i = 10; break;
                case Keys.F12: i = 11; break;
                default:
                    return;
            }
            isMacroChanged = true;
			macro[i] = macroStr;
        }

        public static string GetMacro(Keys key)
        {
			int i = 0;
            switch (key)
            {
                case Keys.F1: i = 0; break;
                case Keys.F2: i = 1; break;
                case Keys.F3: i = 2; break;
                case Keys.F4: i = 3; break;
                case Keys.F5: i = 4; break;
                case Keys.F6: i = 5; break;
                case Keys.F7: i = 6; break;
                case Keys.F8: i = 7; break;
                case Keys.F9: i = 8; break;
                case Keys.F10: i = 9; break;
                case Keys.F11: i = 10; break;
                case Keys.F12: i = 11; break;
                default:
                    return "";
            }
            return macro[i];
        }


		public static string MacroF1 { get { return macro[0]; } }
		public static string MacroF2 { get { return macro[1]; } }
		public static string MacroF3 { get { return macro[2]; } }
		public static string MacroF4 { get { return macro[3]; } }
		public static string MacroF5 { get { return macro[4]; } }
		public static string MacroF6 { get { return macro[5]; } }
		public static string MacroF7 { get { return macro[6]; } }
		public static string MacroF8 { get { return macro[7]; } }
		public static string MacroF9 { get { return macro[8]; } }
		public static string MacroF10 { get { return macro[9]; } }
		public static string MacroF11 { get { return macro[10]; } }
		public static string MacroF12 { get { return macro[11]; } }
	}
	
}