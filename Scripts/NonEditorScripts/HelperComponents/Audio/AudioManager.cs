using System;
using System.Collections.Generic;
using UnityEngine;

namespace CXUtils.HelperComponents
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private int audioSourceAmount = 10;
        [Range( 0f, 1f )]
        [SerializeField] private float mainVolume = 1;

        public readonly List<AudioSource> AudioSources = new List<AudioSource>();

        private readonly object threadLock = new object();

        private int currentAudioIndex;

        public float MainVolume
        {
            get => mainVolume;
            set
            {
                mainVolume = value;
                OnMainVolumeChanged?.Invoke( value );
            }
        }

        private void Awake()
        {
            OnMainVolumeChanged += newValue => AudioListener.volume = newValue;

            AudioListener.volume = mainVolume;

            //initialize audio sources
            for (var i = 0; i < audioSourceAmount; i++)
            {
                var source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                AudioSources.Add( source );
            }
        }

        private void OnValidate()
        {
            audioSourceAmount = Mathf.Max( audioSourceAmount, 1 );
        }

        public event Action<float> OnMainVolumeChanged;

        public AudioSource PlayAudioClip(AudioClip audioClip)
        {
            var receivedAudioSource = GetAudioSource();

            receivedAudioSource.clip = audioClip;
            receivedAudioSource.Play();

            return receivedAudioSource;
        }

        public AudioSource GetAudioSource()
        {
            lock ( threadLock )
            {
                if ( currentAudioIndex >= AudioSources.Count )
                    currentAudioIndex = 0;

                var receivedAudioSource = AudioSources[currentAudioIndex];

                // we lock this because we don't want to get different indexes at the same time

                currentAudioIndex++;

                return receivedAudioSource;
            }
        }
    }
}
