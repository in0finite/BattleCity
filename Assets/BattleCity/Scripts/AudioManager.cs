using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{

	public class AudioManager : MonoBehaviour
	{
		public static AudioManager Instance { get; private set; }

		public const int kPickupSoundIndex = 0, kTankDestroySoundIndex = 1, kBulletSoundIndex = 2,
			kGameOverSoundIndex = 3, kPickupPickedUpSoundIndex = 4, kLevelLoadSoundIndex = 5,
			kBulletHitSoundIndex = 6;

		public AudioSource[] AudioSources { get; private set; }
		public AudioClip[] audioClips = new AudioClip[0];

		public float EffectsVolume { get; set; } = 1.0f;
		public float MusicVolume { get; set; } = 1.0f;



		void Awake()
		{
			
			Instance = this;
			
			this.AudioSources = this.GetComponents<AudioSource>();

		}

		void OnEnable()
		{
			UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChanged;
		}

		void OnDisable()
		{
			UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= OnSceneChanged;
		}

		void OnSceneChanged(UnityEngine.SceneManagement.Scene s1, UnityEngine.SceneManagement.Scene s2)
		{

			// stop all sounds if we are in main menu
			if (! MapManager.IsMapOpened)
			{
				foreach (AudioSource audioSource in this.AudioSources)
				{
					if (audioSource.isPlaying)
						audioSource.Stop();
				}
			}

		}

	    void Start()
	    {
			
	        
	    }

		public void PlaySoundEffect(int soundIndex)
		{
			this.PlaySound(soundIndex, this.EffectsVolume);
		}

		public void PlayMusic(int soundIndex)
		{
			this.PlaySound(soundIndex, this.MusicVolume);
		}

		void PlaySound(int soundIndex, float volume)
		{
			AudioSource audioSource = this.AudioSources[soundIndex];
			AudioClip audioClip = this.audioClips[soundIndex];
			if (audioSource.isPlaying)
				audioSource.Stop();
			audioSource.clip = audioClip;
			audioSource.time = 0f;
			audioSource.volume = Mathf.Clamp01(volume);
			audioSource.Play();
		}
	    
	}

}
