using System;

namespace WindowsGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリー ポイントです。
        /// </summary>
        static void Main(string[] args)
        {
            using (WinGame game = new WinGame())
            {
                game.Run();
            }
        }
    }
#endif
}

