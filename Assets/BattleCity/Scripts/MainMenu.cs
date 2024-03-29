﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{

	public class MainMenu : Menu
	{
		public static MainMenu Instance { get; private set; }

		public Button newGameButton, optionsButton, scoreButton, helpButton, aboutButton, exitButton;


		void Awake()
		{
			Instance = this;
		}

	    void Start()
	    {
			newGameButton.onClick.AddListener(() => MapManager.StartFirstLevel());
	        optionsButton.onClick.AddListener(() => MenuManager.ActiveMenu = OptionsMenu.Instance);
			scoreButton.onClick.AddListener(() => { ScoreMenu.Instance.CurrentScore = 0; ScoreMenu.Instance.ParentMenu = this; MenuManager.ActiveMenu = ScoreMenu.Instance; });
			aboutButton.onClick.AddListener(() => MenuManager.ActiveMenu = AboutMenu.Instance);
			helpButton.onClick.AddListener(() => OnHelpClicked());
			exitButton.onClick.AddListener(() => Application.Quit());
	    }

		void OnHelpClicked()
		{
			var dirInfo = System.IO.Directory.GetParent(Application.dataPath);
			string path = System.IO.Path.Combine(dirInfo.FullName, "Prilog A - Uputstvo za korišćenje.pdf");
			if (System.IO.File.Exists(path))
				System.Diagnostics.Process.Start(path);
		}

	    void Update()
	    {
	        
	    }
	    
	}

}
