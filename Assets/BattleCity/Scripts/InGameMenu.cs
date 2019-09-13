using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{
	
	public class InGameMenu : Menu
	{
		public static InGameMenu Instance { get; private set; }

		public Text hudText;
		public Text gameOverText;


		void Awake()
		{
			Instance = this;
		}

		public override void OnBecameInactive()
		{
			this.gameOverText.gameObject.SetActive(false);
		}

		void Update()
		{
			if (MenuManager.ActiveMenu == this && MapManager.IsMapOpened)
			{
				// update text

				PlayerTank tank = PlayerTank.Instance;
				string str = string.Concat(
					"Score ", MapManager.CurrentScore,
					"\nLifes ", MapManager.NumLifes,
					"\nHealth ", 
					tank != null ? tank.health.ToString() : "0",
					"\nDamage ",
					tank != null ? tank.bulletDamage.ToString() : "0",
					"\nLevel ", MapManager.CurrentLevel,
					"\nEnemies left ", EnemyTankSpawner.Instance.NumTanksLeftToSpawn + EnemyTank.AllTanks.Count);
				
				if (this.hudText.text != str)
					this.hudText.text = str;
				
				// update game-over text
				this.gameOverText.gameObject.SetActive(MapManager.IsGameOver);

			}

			if (Input.GetKeyDown(KeyCode.Escape) && MapManager.IsMapOpened)
			{
				if (MenuManager.ActiveMenu == this)
					MenuManager.ActiveMenu = PauseMenu.Instance;
			}

		}
		
	}

}
