using System;
using System.Collections;
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

        //readonly List<AudioSource> multiUseAudioSources = new List<AudioSource>();
        readonly Queue<AudioSource> freeAudioSources = new Queue<AudioSource>();
        readonly List<AudioSource> occupiedAudioSources = new List<AudioSource>();

        Coroutine _audioSourceCheckerCoroutine;

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

        public void ExpandBufferCount( int addCount )
        {
            audioSourceAmount += addCount;

            //then generate more
            InitializeAudioSources( addCount );
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
            //if no free audio sources
            if ( freeAudioSources.Count == 0 )
                return null;

            AudioSource audioSource;

            MakeOccupied( audioSource = freeAudioSources.Dequeue() );

            return audioSource;
        }

        void MakeOccupied( AudioSource source )
        {
            occupiedAudioSources.Add( source );

            //if this is the first occupied audio source
            if ( occupiedAudioSources.Count == 1 )
                _audioSourceCheckerCoroutine = StartCoroutine( AudioSourceChecker() );
        }

        IEnumerator AudioSourceChecker()
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
