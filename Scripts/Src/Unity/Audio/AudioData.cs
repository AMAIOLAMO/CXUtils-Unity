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
		public AudioClip GetClip(int index = 0) => audioClips[index];

		/// <summary>
		///     Gets a random audio clip from the audio clips
		/// </summary>
		public AudioClip GetRandomClip() => IsEmpty ? null : GetClip(Random.Range(0, audioClips.Length));

		public AudioClip[] AudioClips => audioClips;

		/// <summary>
		///     Does this audio data have no audio clips
		/// </summary>
		public bool IsEmpty => audioClips.Length == 0;

		[SerializeField] AudioClip[] audioClips;
	}
}
