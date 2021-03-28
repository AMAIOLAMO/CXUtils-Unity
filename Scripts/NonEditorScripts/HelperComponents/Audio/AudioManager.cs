using System;
using UnityEngine;
using System.Collections.Generic;

namespace CXUtils.HelperComponents
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private int audioSourceAmount = 10;

        public readonly List<AudioSource> AudioSources = new List<AudioSource>();

        private int currentAudioIndex = 0;

        public float MainVolume
        {
            get => mainVolume;
            set
            {
                mainVolume = value;
                OnMainVolumeChanged?.Invoke(value);
            }
        }
        [Range(0f, 1f)]
        [SerializeField] private float mainVolume = 1;

        public event Action<float> OnMainVolumeChanged;
        
        private void Awake()
        {
            OnMainVolumeChanged += newValue => AudioListener.volume = newValue;
            
            AudioListener.volume = mainVolume;

            //initialize audio sources
            for (int i = 0; i < audioSourceAmount; i++)
                AudioSources.Add(gameObject.AddComponent<AudioSource>());
        }
        
        private readonly object threadLock = new object();

        public AudioSource PlayAudioClip(AudioClip audioClip)
        {
            var receivedAudioSource = GetAudioSource();
        
            receivedAudioSource.clip = audioClip;
            receivedAudioSource.Play();
        
            return receivedAudioSource;
        }
    
        public AudioSource GetAudioSource()
        {
            lock (threadLock)
            {
                if (currentAudioIndex >= AudioSources.Count)
                    currentAudioIndex = 0;

                var receivedAudioSource = AudioSources[currentAudioIndex];
            
                // we lock this because we don't want to get different indexes at the same time
        
                currentAudioIndex++;
        
                return receivedAudioSource;
            }
        }
    
        private void OnValidate() =>
            audioSourceAmount = Mathf.Max(audioSourceAmount, 1);
    }
}
