using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
	
	public class Flag : MapObject
	{
		
		public override void OnCollidedWithBullet(Bullet bullet)
		{
			// game over
			Debug.LogFormat("Bullet collided with flag - game over");

			Destroy(this.gameObject);
		}
		
	}

}
