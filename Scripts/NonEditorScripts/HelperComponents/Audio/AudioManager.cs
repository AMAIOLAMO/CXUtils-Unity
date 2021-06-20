using System;
using System.Collections.Generic;
using UnityEngine;

namespace CXUtils.HelperComponents
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] int audioSourceAmount = 10;
        [Range( 0f, 1f )]
        [SerializeField]
        float mainVolume = 1f;

        readonly object _threadLock = new object();

        public readonly List<AudioSource> multiUseAudioSources = new List<AudioSource>();

        int currentAudioIndex;

        public float MainVolume
        {
            get => mainVolume;
            set
            {
                mainVolume = value;
                AudioListener.volume = value;

                OnMainVolumeChanged?.Invoke( value );
            }
        }

        void Awake()
        {
            AudioListener.volume = mainVolume;

            //initialize audio sources
            InstantiateAudioSources( audioSourceAmount );
        }

        void OnValidate()
        {
            audioSourceAmount = Mathf.Max( audioSourceAmount, 1 );
        }

        void InstantiateAudioSources( int amount )
        {
            for ( int i = 0; i < amount; i++ )
            {
                var source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                multiUseAudioSources.Add( source );
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
        ///     Request an audio source from the list
        /// </summary>
        public AudioSource RequestSource()
        {
            lock ( _threadLock )
            {
                if ( currentAudioIndex >= multiUseAudioSources.Count )
                    currentAudioIndex = 0;

                var receivedAudioSource = multiUseAudioSources[currentAudioIndex];

                // we lock this because we don't want to get different indexes at the same time

                currentAudioIndex++;

                return receivedAudioSource;
            }
        }
    }
}
