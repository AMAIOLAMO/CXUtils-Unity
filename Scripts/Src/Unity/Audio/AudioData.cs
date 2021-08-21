using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace CXUtils.Components
{
    [Serializable]
    [CreateAssetMenu( fileName = "audioData", menuName = "CXUtils/Audio/AudioData" )]
    public class AudioData : ScriptableObject
    {
        [FormerlySerializedAs("audioClips")] [SerializeField] AudioClip[] _audioClips;

        public AudioClip[] AudioClips => _audioClips;

        /// <summary>
        ///     Does this audio data have no audio clips
        /// </summary>
        public bool HasAudioClip => _audioClips.Length > 0;

        /// <summary>
        ///     Gets a audio clip from the audio clips
        /// </summary>
        public AudioClip GetAudioClip( int index = 0 ) => _audioClips[index];

        /// <summary>
        ///     Get's a random audio clip from the audio clips
        /// </summary>
        public AudioClip GetRandomAudioClip() => !HasAudioClip ? null : _audioClips[Random.Range( 0, _audioClips.Length )];
    }
}
