using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Jam
{
    public class AudioManager : Singleton<AudioManager>
    {
        public AudioMixer audioMixer;

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
    }
}
