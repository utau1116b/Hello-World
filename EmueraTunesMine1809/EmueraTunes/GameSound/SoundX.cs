using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsGame.PlaySound;
using MinorShift.Emuera.GameSound.SoundX;
using MinorShift.Emuera.GameProc.Function;

namespace MinorShift.Emuera.GameSound.SoundX
{
    public partial class VolumeDialog : Form
    {
        public VolumeDialog()
        {
            InitializeComponent();
            float f = SoundVolume.soundVolume * 50;
            float f2 = SoundVolumeSE.soundVolumeSE * 50;
            firstVolume = SoundVolume.soundVolume;
            firstVolumeSE = SoundVolumeSE.soundVolumeSE;
            firstMute = isMute;
            if (trackval1 > 0 || trackval2 > 0)
            {
                trackBar1.Value = trackval1;
                trackBar2.Value = trackval2;
                label3.Text = trackval1.ToString();
                label4.Text = trackval2.ToString();
                savtrackval1 = trackval1;
                savtrackval2 = trackval2;
            }
            else
            {
                trackBar1.Value = (int)f;
                trackBar2.Value = (int)f2;
                label3.Text = f.ToString();
                label4.Text = f2.ToString();
            }
            if (isMute == true) { checkBox1.Checked = true; }
            else { checkBox1.Checked = false; }
        }

        private bool isOb() 
        {
            
            //audioEngine存在チェック
            bool gmc = FunctionIdentifier.gm.checkObj();
            if (gmc == true)
            { return true; }
            else
            { return false; }
        }
        private static int trackval1 = 0;
        private static int trackval2 = 0;
        //Muteキャンセル時に色々おかしくなる対策
        private static int savtrackval1 = 0;
        private static int savtrackval2 = 0;
        private static bool firstMute = false;
        private float firstVolume = 1.0f;
        private float firstVolumeSE = 1.0f;
        private static bool isMute = false;

        private void VolumeDialog_Load(object sender, EventArgs e) 
        {
            //最大値・最小値設定
            trackBar1.Minimum = 0;
            trackBar1.Maximum = 100;
            trackBar2.Minimum = 0;
            trackBar2.Maximum = 100;

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox1.Checked)
            {

                float f = 0.0f;

                isMute = true;

                trackval1 = trackBar1.Value;
                trackval2 = trackBar2.Value;

                if (isOb() == false) { return; }


                FunctionIdentifier.gm.SetVolume(f);
                FunctionIdentifier.gm.SetVolumeSE(f);
                //trackBar1.Value = 0;
                //trackBar2.Value = 0;
                //renewLabel();
            }
            else 
            {
                if (isOb() == false) { return; }

                isMute = false;
                
                //trackBar1.Value = 50;
                //trackBar2.Value = 50;

                float f = ConvertItoF(trackBar1.Value);
                float f2 = ConvertItoF(trackBar2.Value);

                trackval1 = 0;
                trackval2 = 0;

                if (isOb() == false) { return; }

                FunctionIdentifier.gm.SetVolume(f);
                FunctionIdentifier.gm.SetVolumeSE(f2);

                renewLabel();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int _i = trackBar1.Value;
            label3.Text = _i.ToString();


            if (isOb() == false) { return; }

            if (isMute == false)
            {
                FunctionIdentifier.gm.SetVolume(ConvertItoF(_i));
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            int _i = trackBar2.Value;
            label4.Text = _i.ToString();

            if (isOb() == false) { return; }

            if (isMute == false)
            {
                FunctionIdentifier.gm.SetVolumeSE(ConvertItoF(_i));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DefaultVolume();
            renewLabel();
        }

        private void DefaultVolume()
        {
            trackBar1.Value = 50;
            trackBar2.Value = 50;
            //label3.Text = "50";
            //label4.Text = "50";

            if (isOb() == false) { return; }

            FunctionIdentifier.gm.SetVolume(1.0f);
            FunctionIdentifier.gm.SetVolumeSE(1.0f);
            renewLabel();
        }

        private float ConvertItoF(int i)
        {
            float f = i;
            f = f * 0.02f;
            if (f >= 2.0f) f = 2.0f;
            else if (f <= 0.0f) f = 0.0f;
            return f;
        }
        //キャンセル
        private void button3_Click(object sender, EventArgs e)
        {
            if (isOb() == true)
            {
                FunctionIdentifier.gm.SetVolume(this.firstVolume);
                FunctionIdentifier.gm.SetVolumeSE(this.firstVolumeSE);

            }
            //ミュートをキャンセルした時
            if (firstMute == true) { trackval1 = savtrackval1; trackval2 = savtrackval2; isMute = true; }
            else { trackval1 = 0; trackval2 = 0; } 
            this.Close();
        }
        //完了
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void renewLabel() 
        {
            

            float f1 = SoundVolume.soundVolume * 50;
            float f2 = SoundVolumeSE.soundVolumeSE * 50;
            label3.Text = f1.ToString();
            label4.Text = f2.ToString();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void VolumeDialog_Load_1(object sender, EventArgs e)
        {

        }

    }
}
