using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{

	public class OptionsMenu : Menu
	{
		public static OptionsMenu Instance { get; private set; }

		public Button saveButton, backButton;
		public Slider soundEffectVolumeSlider, musicVolumeSlider;

		public float soundEffectVolume, musicVolume;


		void Awake()
		{
			Instance = this;
		}

	    void Start()
	    {
			ReadPrefs();
			
	        saveButton.onClick.AddListener(() => Save());
			backButton.onClick.AddListener(() => GoBack());
	    }

		void ReadPrefs()
		{
			soundEffectVolume = PlayerPrefs.GetFloat("soundEffectVolume");
			musicVolume = PlayerPrefs.GetFloat("musicVolume");
		}

		void Save()
		{
			soundEffectVolume = soundEffectVolumeSlider.value;
			musicVolume = musicVolumeSlider.value;

			PlayerPrefs.SetFloat("soundEffectVolume", soundEffectVolume);
			PlayerPrefs.SetFloat("musicVolume", musicVolume);

			PlayerPrefs.Save();

			MenuManager.ActiveMenu = MainMenu.Instance;
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

	        soundEffectVolumeSlider.value = soundEffectVolume;
			musicVolumeSlider.value = musicVolume;

	    }

	    void Update()
	    {
	        
	    }
	    
	}

}
