using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
	
	public class MapObject : MonoBehaviour
	{
		public Vector2 Position { get; set; }

		public virtual bool IsPassable { get => false; }
	//	public bool CollidesWithBullet { get => ! Physics.GetIgnoreLayerCollision(this.gameObject.layer, LayerMask.NameToLayer("Bullet")); }

		public virtual void OnCollidedWithBullet(Bullet bullet)
		{

		}
		
	}

}
