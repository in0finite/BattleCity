using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{
	
	public class InGameMenu : Menu
	{
		public static InGameMenu Instance { get; private set; }

		public Text hudText;


		void Awake()
		{
			Instance = this;
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
				
			}
		}
		
	}

}
