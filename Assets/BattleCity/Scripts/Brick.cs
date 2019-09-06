using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
	
	public class Brick : MapObject
	{
		
		public float health = 150f;
		
		public override void OnCollidedWithBullet(Bullet bullet)
		{
			this.health -= bullet.damage;
			if (this.health <= 0f)
				Destroy(this.gameObject);
		}

	}

}
