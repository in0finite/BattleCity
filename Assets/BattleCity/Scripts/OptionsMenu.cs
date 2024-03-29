﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{

	public class OptionsMenu : Menu
	{
		public static OptionsMenu Instance { get; private set; }

		public Button saveButton, backButton;
		public Slider soundEffectVolumeSlider, musicVolumeSlider;

	//	public float soundEffectVolume, musicVolume;


		void Awake()
		{
			Instance = this;

			ReadPrefs();
		}

	    void Start()
	    {
			saveButton.onClick.AddListener(() => Save());
			backButton.onClick.AddListener(() => GoBack());
	    }

		void ReadPrefs()
		{
			AudioManager.Instance.EffectsVolume = PlayerPrefs.GetFloat("soundEffectVolume", 1.0f);
			AudioManager.Instance.MusicVolume = PlayerPrefs.GetFloat("musicVolume", 1.0f);
		}

		void Save()
		{
			AudioManager.Instance.EffectsVolume = soundEffectVolumeSlider.value;
			AudioManager.Instance.MusicVolume = musicVolumeSlider.value;

			PlayerPrefs.SetFloat("soundEffectVolume", AudioManager.Instance.EffectsVolume);
			PlayerPrefs.SetFloat("musicVolume", AudioManager.Instance.MusicVolume);

			PlayerPrefs.Save();

			GoBack();
		}

		void GoBack()
		{
			if (MapManager.IsMapOpened)
				MenuManager.ActiveMenu = PauseMenu.Instance;
			else
				MenuManager.ActiveMenu = MainMenu.Instance;
		}

		public override void OnBecameActive()
	    {
			ReadPrefs();

	        soundEffectVolumeSlider.value = AudioManager.Instance.EffectsVolume;
			musicVolumeSlider.value = AudioManager.Instance.MusicVolume;

			Time.timeScale = 0f;
			
	    }

		public override void OnBecameInactive()
		{
			Time.timeScale = 1f;
		}

	    void Update()
	    {
	        
	    }
	    
	}

}
