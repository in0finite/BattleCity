using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{

	public class MainMenu : Menu
	{
		public static MainMenu Instance { get; private set; }

		public Button newGameButton, optionsButton, scoreButton, helpButton, exitButton;


		void Awake()
		{
			Instance = this;
		}

	    void Start()
	    {
	        optionsButton.onClick.AddListener(() => MenuManager.ActiveMenu = OptionsMenu.Instance);
			helpButton.onClick.AddListener(() => Application.OpenURL("manual.pdf"));
			exitButton.onClick.AddListener(() => Application.Quit());
	    }

	    void Update()
	    {
	        
	    }
	    
	}

}
