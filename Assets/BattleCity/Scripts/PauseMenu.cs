using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{

	public class PauseMenu : Menu
	{

		public static PauseMenu Instance { get; private set; }

		public Button continueButton, optionsButton, quitButton;



		void Awake()
		{
			Instance = this;

			continueButton.onClick.AddListener(() => MenuManager.ActiveMenu = InGameMenu.Instance);
			optionsButton.onClick.AddListener(() => MenuManager.ActiveMenu = OptionsMenu.Instance);
			quitButton.onClick.AddListener(() => OnQuitPressed());

		}

	    void Start()
	    {
	        
	    }

		public override void OnBecameActive()
		{
			Time.timeScale = 0f;
		}

		public override void OnBecameInactive()
		{
			Time.timeScale = 1f;
		}

	    void Update()
	    {
	        if (Input.GetKeyDown(KeyCode.Escape) && MapManager.IsMapOpened)
			{
				if (MenuManager.ActiveMenu == this)
				{
					// unpause the game
					MenuManager.ActiveMenu = InGameMenu.Instance;
				}
			}
	    }

		void OnQuitPressed()
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
			
			if (MapManager.CurrentScore > 0)
			{
				// open score menu
				ScoreMenu.Instance.ParentMenu = MainMenu.Instance;
				ScoreMenu.Instance.CurrentScore = MapManager.CurrentScore;
				MenuManager.ActiveMenu = ScoreMenu.Instance;
			}
			else
			{
				// open main menu
				MenuManager.ActiveMenu = MainMenu.Instance;
			}
		}
	    
	}

}
