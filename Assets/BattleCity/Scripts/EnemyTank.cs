using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
	
	public class EnemyTank : Tank
	{
		
		
		protected override void Start()
		{
			base.Start();

			// set material
			foreach(var mr in this.GetComponentsInChildren<MeshRenderer>())
			{
				mr.sharedMaterial = MapManager.Instance.enemyTankMaterial;
			}

		}
		
	}

}
