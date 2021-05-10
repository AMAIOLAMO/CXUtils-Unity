using System;
using UnityEngine;
using System.Collections.Generic;

namespace CXUtils.HelperComponents
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private int audioSourceAmount = 10;

        [Range( 0f, 1f )]
        [SerializeField] private float mainVolume = 1;

        public readonly List<AudioSource> MultiUseAudioSources = new List<AudioSource>();

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
            InstantiateAudioSources( audioSourceAmount );
        }

        private void OnValidate()
        {
            audioSourceAmount = Mathf.Max( audioSourceAmount, 1 );
        }

        private void InstantiateAudioSources( int amount )
        {
            for ( var i = 0; i < amount; i++ )
            {
                var source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                MultiUseAudioSources.Add( source );
            }
        }

        public event Action<float> OnMainVolumeChanged;

        public void ExpandBufferCount( int addCount )
        {
            audioSourceAmount += addCount;

            //then generate more
            InstantiateAudioSources( addCount );
        }

        public AudioSource PlayAudioClip( AudioClip audioClip )
        {
            var receivedAudioSource = RequestSource();

            receivedAudioSource.clip = audioClip;
            receivedAudioSource.Play();

            return receivedAudioSource;
        }

        /// <summary>
        /// Request an audio source from the list
        /// </summary>
        public AudioSource RequestSource()
        {
            lock ( threadLock )
            {
                if ( currentAudioIndex >= MultiUseAudioSources.Count )
                    currentAudioIndex = 0;

                var receivedAudioSource = MultiUseAudioSources[currentAudioIndex];

                // we lock this because we don't want to get different indexes at the same time

                currentAudioIndex++;

                return receivedAudioSource;
            }
        }
    }
}
