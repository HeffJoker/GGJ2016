using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Foundation.Messenger;
using SF_Tools.Messages;

using System.ComponentModel;

namespace SF_Tools.Managers
{
    public class SoundManager : SingletonBase<SoundManager>
    {
        #region Editor Properties

        public AudioSource SoundSource;
        public AudioSource MusicSource;
        public AudioClip ButtonAudio;
        public AudioClip GameOverClip;
        public AudioClip MainMusic;

        //public ToggleActions MusicToggle;
        //public ToggleActions SoundToggle;
        public Image MusicImage;
        public Image SoundImage;

        #endregion

        #region Private Members

        private bool allowSound = true;
        private bool allowMusic = true;
        private float prevVol = 1;

        #endregion

        #region Public Properties

        public bool AllowSound
        {
            get { return allowSound; }
        }

        public bool AllowMusic
        {
            get { return allowMusic; }
        }

        public bool IsMusicPlaying
        {
            get { return MusicSource.isPlaying; }
        }

        public float MusicVolume
        {
            get { return MusicSource.volume; }
            set { MusicSource.volume = value; }
        }

        public float SoundVolume
        {
            get { return SoundSource.volume; }
            set { SoundSource.volume = value; }
        }

        #endregion

        #region Public Interface

        public void ToggleSound()
        {
            allowSound = !allowSound;

            if (!AllowSound)
                SoundSource.volume = 0;
            else
                SoundSource.volume = 1f;
        }

        public void ToggleMusic()
        {
            allowMusic = !allowMusic;

            if (!AllowMusic)
                MusicSource.volume = 0;
            else
                MusicSource.volume = prevVol;
        }

        public void StopMusic()
        {
            MusicSource.Stop();
        }

        public void PlayButtonClick()
        {        
            if(SoundSource != null && AllowSound)
                SoundSource.PlayOneShot(ButtonAudio);
        }

        public void PlaySound(AudioClip clip)
        {
            if(clip != null && AllowSound)
                SoundSource.PlayOneShot(clip);
        }

        public void PlaySound(AudioClip clip, float volume)
        {
            if (clip != null && AllowSound)
                SoundSource.PlayOneShot(clip, volume);
        }

        public void PlayMusic(AudioClip clip, bool loop, float delay)
        {
            PlayMusic(clip, loop, delay, MusicSource.volume);
        }

        public void PlayMusic(AudioClip clip, bool loop, float delay, float volume)
        {
            if (clip != null && allowMusic)
            {
                prevVol = volume;

                MusicSource.loop = loop;
                MusicSource.clip = clip;
                MusicSource.volume = volume;
                MusicSource.PlayDelayed(delay);
            }
        }

        [Subscribe]
        public void HandleGameOver(Message_GameOver message)
        {
            MusicSource.Stop();
            PlaySound(GameOverClip);
        }

        [Subscribe]
        public void HandleGameStart(Message_GameStart message)
        {
            PlayMusic(MainMusic, true, 0, 0.5f);
        }
       
        #endregion

        #region Private Routines

        protected override void OnWake()
        {
            Messenger.Subscribe(this);

            if (PlayerPrefs.HasKey("Music"))
                allowMusic = (PlayerPrefs.GetInt("Music", 1) > 0);
            else
                PlayerPrefs.SetInt("Music", 1);

            if (PlayerPrefs.HasKey("Sound"))
                allowSound = (PlayerPrefs.GetInt("Sound", 1) > 0);
            else
                PlayerPrefs.SetInt("Sound", 1);

            /*
            if (!allowMusic && MusicToggle != null)
                MusicToggle.SpriteSwap(MusicImage);

            if (!allowSound && SoundToggle != null)
                SoundToggle.SpriteSwap(SoundImage);*/
        }

        protected override void OnDestroy_Sub()
        {
            Messenger.Unsubscribe(this);
        }

        private IEnumerator WaitAndPlay()
        {
            MusicSource.loop = false;
            while (MusicSource.isPlaying)
                yield return null;

            PlayMusic(MainMusic, true, 0, prevVol);
        }

        #endregion
    }

    public static class SoundExtensionMethods
    {
        public static void PlaySound(this AudioSource source)
        {
            if (SoundManager.Instance.AllowSound)
                source.Play();
        }

        public static void PlaySound(this AudioSource source, AudioClip clip)
        {
            if (SoundManager.Instance.AllowSound)
                source.PlayOneShot(clip);
        }
    }
}
