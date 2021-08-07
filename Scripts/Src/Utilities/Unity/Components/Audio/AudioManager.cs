using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CXUtils.Components
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] int _audioSourceAmount = 10;
        [Range( 0f, 1f )]
        [SerializeField] float _mainVolume = 1f;

        readonly Queue<AudioSource> _freeAudioSources = new Queue<AudioSource>();
        readonly List<AudioSource> _occupiedAudioSources = new List<AudioSource>();

        public float MainVolume
        {
            get => _mainVolume;
            set
            {
                _mainVolume = value;
                AudioListener.volume = value;

                OnMainVolumeChanged?.Invoke( value );
            }
        }

        public bool UseAudioCheckDelay { get; set; } = false;

        public float AudioCheckDelay { get; set; }

        void Awake()
        {
            AudioListener.volume = _mainVolume;

            //initialize audio sources
            InitializeAudioSources( _audioSourceAmount );
        }

        void OnValidate()
        {
            _audioSourceAmount = Math.Max( _audioSourceAmount, 1 );
        }

        void InitializeAudioSources( int amount )
        {
            for ( int i = 0; i < amount; i++ )
            {
                var source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;

                _freeAudioSources.Enqueue( source );
            }
        }

        public event Action<float> OnMainVolumeChanged;

        /// <summary>
        ///     Expands the audio buffers with extra <paramref name="addCount" />
        /// </summary>
        public void Expand( int addCount )
        {
            _audioSourceAmount += addCount;

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
        ///     Tries to request a, <see cref="AudioSource" />
        /// </summary>
        public bool TryRequestSource( out AudioSource audioSource ) => ( audioSource = RequestSource() ) != null;

        /// <summary>
        ///     Request an audio source from the free queue
        /// </summary>
        public AudioSource RequestSource()
        {
            //if no free audio sources
            if ( _freeAudioSources.Count == 0 )
                return null;

            AudioSource audioSource;

            MakeOccupied( audioSource = _freeAudioSources.Dequeue() );

            return audioSource;
        }

        // == Helper ==

        void MakeOccupied( AudioSource source )
        {
            _occupiedAudioSources.Add( source );

            //if this is the first occupied audio source
            if ( _occupiedAudioSources.Count == 1 )
                StartCoroutine( AudioCheck() );
        }

        IEnumerator AudioCheck()
        {
            while ( _occupiedAudioSources.Count > 0 )
            {
                //check
                for ( int i = 0; i < _occupiedAudioSources.Count; i++ )
                {
                    if ( _occupiedAudioSources[i].isPlaying ) continue;

                    //else finished playing
                    _freeAudioSources.Enqueue( _occupiedAudioSources[i] );
                    _occupiedAudioSources.RemoveAt( i );
                }
                
                yield return UseAudioCheckDelay ? new WaitForSecondsRealtime( AudioCheckDelay ) : null;
            }
        }
    }
}
