using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Utau.Eramakerview.Library;

namespace Utau.Eramakerview
{
    static class Program
    {
        /*
         * START
         * ・CSV読み込み
         * 読み込み処理(ReadData)
         * 書き込みに必要なデータを保存（ConstantData）
         * ・HTML書き出し
         */

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            ExeDir=Library.Library.ExeDir;

            CsvDir = ExeDir + "csv\\";
            ExeName = Library.Library.ExeName;
           
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public static string ExeDir { get; private set; }
        public static string CsvDir { get; private set; }
        public static string ExeName { get; private set; }

    }

}
