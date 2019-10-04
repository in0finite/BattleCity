using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
	
	public class StrengthEffect : PickupEffect
	{
		
		public override void ApplyEffect()
		{
			PlayerTank tank = PlayerTank.Instance;
			if (tank != null)
			{
				// 3 levels of strength/damage (0.34 * 3 = 1.02)

				float newPerc = Mathf.Min( tank.GetHealthPerc() + 0.34f, 2.0f );
				tank.SetHealthPerc(newPerc);

				newPerc = Mathf.Min( tank.GetBulletDamagePerc() + 0.34f, 2.0f );
				tank.SetBulletDamagePerc(newPerc);

			}
		}
		
	}

}
