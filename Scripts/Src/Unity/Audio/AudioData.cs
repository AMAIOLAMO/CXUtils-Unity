using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CXUtils.Components
{
	[Serializable]
	[CreateAssetMenu(fileName = "audioData", menuName = "CXUtils/Audio/AudioData")]
	public class AudioData : ScriptableObject
	{
		/// <summary>
		///     Gets a audio clip from the audio clips
		/// </summary>
		public AudioClip GetClip(int index = 0) => _audioClips[index];

		/// <summary>
		///     Gets a random audio clip from the audio clips
		/// </summary>
		public AudioClip GetRandomClip() => !HasAudioClip ? null : GetClip(Random.Range(0, _audioClips.Length));

		public AudioClip[] AudioClips => _audioClips;

		/// <summary>
		///     Does this audio data have no audio clips
		/// </summary>
		public bool HasAudioClip => _audioClips.Length > 0;
		
		[SerializeField] AudioClip[] _audioClips;
	}
}
