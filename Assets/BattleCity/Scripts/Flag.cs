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
			
			Destroy(this.gameObject);

			MapManager.OnFlagDestroyed();

		}
		
	}

}
