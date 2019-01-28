using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Jam
{
    public class AudioManager : Singleton<AudioManager>
    {
        public AudioMixer audioMixer;

        public AudioSource fxSource;
        public AudioSource musicSource;
        [Header("AUDIO CLIPS")]
        public AudioClip[] writingSFX;
        public AudioClip flashlightSound; 

        // Start is called before the first frame update
        void Start()
        {
            if (audioMixer == null)
                Debug.LogWarning("No audio mixer attached to audio manager.");
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PlayWriteSound()
        {
            float volume = 1f;
            audioMixer.GetFloat("sfxVol", out volume);
            //Debug.Log("Vol:" + volume); 

            if (fxSource.isPlaying)
                return; 
            else
            {
                AudioClip clip = writingSFX[Random.Range(0, writingSFX.Length)];
                if(clip != null)
                    fxSource.PlayOneShot(clip, volume);
            }
        }

        public void PlayFlashlightSFX()
        {
            float volume = 1f;
            audioMixer.GetFloat("sfxVol", out volume); 
           

            if (fxSource.isPlaying)
                return;
            else
            {
                AudioClip clip = flashlightSound;
                if (clip != null)
                    fxSource.PlayOneShot(clip, volume);
            }
        }

        public void StopWriteSound()
        {
            fxSource.Stop(); 
        }

    }
}
