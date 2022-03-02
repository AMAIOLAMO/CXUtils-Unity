using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CXUtils.Components
{
	[Serializable]
	[CreateAssetMenu(fileName = "advancedAudioData", menuName = "CXUtils/Audio/AdvancedAudioData")]
	public class AdvancedAudioData : AudioData
	{
		public float CalculatePitch() =>
			Random.Range(basePitch - pitchFluctuate, basePitch + pitchFluctuate);
		
		public float CalculateVolume() =>
			Random.Range(baseVolume - volumeFluctuate, baseVolume + volumeFluctuate);

		public float BasePitch  => basePitch;
		public float BaseVolume => baseVolume;

		public float PitchFluctuate  => pitchFluctuate;
		public float VolumeFluctuate => volumeFluctuate;

		[Header("Base Values")]
		[SerializeField] float basePitch;
		[SerializeField] float baseVolume;

		[Header("Fluctuations")]
		[SerializeField] float pitchFluctuate;
		[SerializeField] float volumeFluctuate;
	}
}
