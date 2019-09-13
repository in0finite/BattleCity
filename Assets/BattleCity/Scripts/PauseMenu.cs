using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{

	public class PauseMenu : Menu
	{

		public static PauseMenu Instance { get; private set; }



		void Awake()
		{
			Instance = this;
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
	    
	}

}
