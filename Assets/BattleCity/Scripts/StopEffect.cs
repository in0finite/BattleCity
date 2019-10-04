using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
	
	public class StopEffect : PickupEffect
	{
		
		public override void ApplyEffect()
		{
			EnemyTank.AreAllEnemyTanksFrozen = true;
		}
		
		public override void CancelEffect()
		{
			EnemyTank.AreAllEnemyTanksFrozen = false;
		}
		
	}

}
