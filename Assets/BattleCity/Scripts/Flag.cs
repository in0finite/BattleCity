using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
	
	public class Flag : MapObject
	{
		public static Flag Instance { get; private set; }


		void Awake()
		{
			Instance = this;
		}

		public override void OnCollidedWithBullet(Bullet bullet)
		{
			// game over
			Debug.LogFormat("Bullet collided with flag - game over");

			Destroy(this.gameObject);
		}
		
	}

}
