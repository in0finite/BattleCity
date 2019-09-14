using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{

	public class AudioManager : MonoBehaviour
	{
		public static AudioManager Instance { get; private set; }

		public const int kPickupSoundIndex = 0, kTankDestroySoundIndex = 1, kBulletSoundIndex = 2,
			kGameOverSoundIndex = 3, kPickupPickedUpSoundIndex = 4, kLevelLoadSoundIndex = 5;

		public AudioSource[] AudioSources { get; private set; }
		public AudioClip[] audioClips = new AudioClip[0];



		void Awake()
		{
			
			Instance = this;
			
			this.AudioSources = this.GetComponents<AudioSource>();

		}

	    void Start()
	    {
			
	        
	    }

		public void PlaySoundEffect(int soundIndex)
		{
			AudioSource audioSource = this.AudioSources[soundIndex];
			AudioClip audioClip = this.audioClips[soundIndex];
			if (audioSource.isPlaying)
				audioSource.Stop();
			audioSource.clip = audioClip;
			audioSource.time = 0f;
			audioSource.Play();
		}
	    
	}

}
