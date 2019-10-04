using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
	
	public class ShieldEffect : PickupEffect
	{
		
		public override void ApplyEffect()
		{
			if (PlayerTank.Instance != null)
			{
				PlayerTank.Instance.HasShield = true;
			}
		}
		
		public override void CancelEffect()
		{
			if (PlayerTank.Instance != null)
				PlayerTank.Instance.HasShield = false;
		}

	}

}
