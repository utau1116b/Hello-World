using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;


namespace WindowsGame.PlaySound
{
    //曲のボリュームは疑似グローバル変数を作って、そこから読み込めるようにする予定
    public class SoundVolume
    {
        public static float soundVolume = 1.0f;
    }

    public class SoundVolumeSE
    {
        public static float soundVolumeSE = 1.0f;
    }

    public partial class GameMain : Microsoft.Xna.Framework.Game
    {
        public bool checkObj()
        {
            if (audioEngine == null) return false;
            else return true;
        }
        private AudioEngine audioEngine = null;
        private WaveBank waveBank = null;
        private WaveBank waveBank2 = null;
        private SoundBank soundBank = null;
        private SoundBank soundBank2 = null;
        private Cue engineSound = null;
        private Cue engineSound2 = null;
        //ボリューム調整
        private AudioCategory soundCategory;
        private AudioCategory soundCategory2;
        public float soundVolume = SoundVolume.soundVolume;
        public float soundVolume2 = SoundVolumeSE.soundVolumeSE;

        private string bgmname = null;

        public GameMain()
        {

        }

        ~GameMain()
        {
            StopSoundMain();
        }

        public void PlaySound(string bgm) { this.PlaySoundMain(bgm); }
        public void StopSound() { this.StopSoundMain(); }
        public void CloseSound() { this.CloseSoundMain(); }
        public void SetSound() { this.SetSoundMain(); }
        public void PlaySoundSE(string se) { this.PlaySoundMainSE(se); }
        public void SetSoundSE() { this.SetSoundMainSE(); }
        public void StopSoundSE(string se) { this.StopSoundMainSE(se); }

        public void SetVolume(float f) { soundVolume = f; this.SetVolumeMainBGM(); }
        public void SetVolumeSE(float f) { soundVolume2 = f; this.SetVolumeMainSE(); }

        private void SetSoundMain()
        {
            
            audioEngine = new AudioEngine("Music\\MySound.xgs");
            waveBank = new WaveBank(audioEngine, "Music\\Wave Bank.xwb", 0, 1024);
            soundBank = new SoundBank(audioEngine, "Music\\Sound Bank.xsb");
            soundCategory = audioEngine.GetCategory("Music");
            audioEngine.Update();

        }
        private void SetSoundMainSE()
        {
            audioEngine = new AudioEngine("Music\\MySound.xgs");
            waveBank2 = new WaveBank(audioEngine, "Music\\Wave Bank 2.xwb");
            soundBank2 = new SoundBank(audioEngine, "Music\\Sound Bank 2.xsb");
            soundCategory2 = audioEngine.GetCategory("Default");
            audioEngine.Update();
        }
        private void PlaySoundMain(string bgm)
        {
            bgmname = bgm;
            engineSound = soundBank.GetCue(bgm);

            SetVolumeMainBGM();
            audioEngine.Update();
            engineSound.Play();
            //audioEngine.Update();
        }
        private void StopSoundMain()
        {
            if (bgmname != null)
            {
                CloseSound();
                engineSound = null;
            }
            bgmname = null;
        }
        private void CloseSoundMain()
        {
            if (engineSound != null)
            {
                engineSound = soundBank.GetCue(bgmname);
                engineSound.Stop(AudioStopOptions.AsAuthored);
                waveBank.Dispose();
                soundBank.Dispose();
                engineSound.Dispose();
                audioEngine.Update();
            }
        }
        private void PlaySoundMainSE(string se)
        {
            engineSound2 = soundBank2.GetCue(se);
            SetVolumeMainSE();
            audioEngine.Update();
            engineSound2.Play();
            if (engineSound2.IsStopped == true)
            { engineSound2.Dispose(); }
            else if (engineSound2.IsStopping == true)
            { engineSound2.Dispose(); }
        }

        private void StopSoundMainSE(string se)
        {
            if (engineSound2 != null)
            {
                engineSound2 = soundBank2.GetCue(se);
                engineSound2.Stop(AudioStopOptions.AsAuthored);
                waveBank2.Dispose();
                soundBank2.Dispose();
                engineSound2.Dispose();
                audioEngine.Update();
                engineSound2 = null;
            }
        }

        private void SetVolumeMainBGM()
        {
            soundCategory = audioEngine.GetCategory("Music");
            soundCategory.SetVolume(soundVolume);
            audioEngine.Update();
            SoundVolume.soundVolume = soundVolume;
        }
        private void SetVolumeMainSE()
        {
            soundCategory2 = audioEngine.GetCategory("Default");
            soundCategory2.SetVolume(soundVolume2);
            audioEngine.Update();
            SoundVolumeSE.soundVolumeSE = soundVolume2;
        }


    }
}
