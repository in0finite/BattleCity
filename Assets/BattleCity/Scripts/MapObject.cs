using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
	
	public class MapObject : MonoBehaviour
	{
		public Vector2 Position { get; set; }

		public virtual void OnCollidedWithBullet(Bullet bullet)
		{

		}
		
	}

}
