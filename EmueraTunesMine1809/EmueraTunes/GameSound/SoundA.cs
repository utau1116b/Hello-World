using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectSound;
using Microsoft.DirectX.AudioVideoPlayback;



namespace MinorShift.Emuera.GameSound
{
    /// <summary>
    /// メインフォーム
    /// </summary>
    public partial class MainMDXS2 : Form
    {
        private Audio audio = null;

        public MainMDXS2()
        {
            //audio = null;
        }
        public void SetSound2(string uri)
        {
            this.Sound_Load2(uri);
        }
        public void CloseSound2()
        {
            this.Sound_Closed2();
        }
        public void PlaySound2(bool isLoop)
        {
            if (isLoop == true)
                this.Sound_Play_Loop2();
            else
                this.Sound_Play2();
        }
        public void StopSound2()
        {
            this.Sound_Stop2();
        }
        private void Sound_Load2(string uri)
        {
            try
            {
                audio = new Audio(uri);
            }
            catch (Exception ex)
            {
                // 失敗
                MessageBox.Show(ex.ToString(), "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void Sound_Closed2()
        {
            // オーディオの破棄
            if (audio != null)
            {
                audio.Dispose();
                audio = null;
            }
        }
        private void Sound_Play2()
        {
            if (audio != null)
            {
                // 再生位置を一番最初に設定
                audio.CurrentPosition = 0.0;

                // 再生
                audio.Play();
            }
        }
        private void Sound_Stop2()
        {
            if (audio != null)
            {
                //audio = null;
                // 停止
                audio.Stop();
            }
        }
        private void Sound_Play_Loop2()
        {
            if (audio != null)
            {
                // 再生位置を一番最初に設定
                audio.CurrentPosition = 0.0;

                // 再生
                audio.Play();
                audio.Ending += new EventHandler(OnAudioEnding);
            }

        }
        private void OnAudioEnding(object ob, EventArgs e)
        {
            if (audio != null)
            {
                // 再生位置を一番最初に設定
                audio.CurrentPosition = 0.0;
                audio.Play();
            }
        }
    }
}

