using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CXUtils.HelperComponents
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] int audioSourceAmount = 10;
        [Range( 0f, 1f )]
        [SerializeField] float mainVolume = 1f;

        readonly Queue<AudioSource> freeAudioSources = new Queue<AudioSource>();
        readonly List<AudioSource> occupiedAudioSources = new List<AudioSource>();

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
            InitializeAudioSources( audioSourceAmount );
        }

        void OnValidate()
        {
            audioSourceAmount = Mathf.Max( audioSourceAmount, 1 );
        }

        void InitializeAudioSources( int amount )
        {
            for ( int i = 0; i < amount; i++ )
            {
                var source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;

                freeAudioSources.Enqueue( source );
            }
        }

        public event Action<float> OnMainVolumeChanged;

        /// <summary>
        ///     Expands the audio buffers with extra <paramref name="addCount" />
        /// </summary>
        public void Expand( int addCount )
        {
            audioSourceAmount += addCount;

            //then generate more
            InitializeAudioSources( addCount );
        }

        public AudioSource PlayClip( AudioClip audioClip )
        {
            var receivedAudioSource = RequestSource();

            receivedAudioSource.clip = audioClip;
            receivedAudioSource.Play();

            return receivedAudioSource;
        }

        /// <summary>
        /// Tries to request a, <see cref="AudioSource"/>
        /// </summary>
        /// <param name="audioSource"></param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public bool TryRequestSource( out AudioSource audioSource ) =>
            ( audioSource = RequestSource() ) != null;

        /// <summary>
        ///     Request an audio source from the free queue
        /// </summary>
        public AudioSource RequestSource()
        {
            //if no free audio sources
            if ( freeAudioSources.Count == 0 )
                return null;

            AudioSource audioSource;

            MakeOccupied( audioSource = freeAudioSources.Dequeue() );

            return audioSource;
        }

        // == Helper ==

        void MakeOccupied( AudioSource source )
        {
            occupiedAudioSources.Add( source );

            //if this is the first occupied audio source
            if ( occupiedAudioSources.Count == 1 )
                StartCoroutine( AudioCheck() );
        }

        IEnumerator AudioCheck()
        {
            while ( occupiedAudioSources.Count > 0 )
            {
                //check
                for ( int i = 0; i < occupiedAudioSources.Count; i++ )
                {
                    if ( occupiedAudioSources[i].isPlaying ) continue;

                    //else finished playing
                    freeAudioSources.Enqueue( occupiedAudioSources[i] );
                    occupiedAudioSources.RemoveAt( i );
                }

                yield return null;
            }
        }
    }
}
