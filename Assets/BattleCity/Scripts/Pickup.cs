using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
	
	public class Pickup : MapObject
	{
		public override bool IsPassable => true;

		bool m_collided = false;
		public event System.Action onCollided = delegate {};


		void OnCollisionEnter(Collision collision)
		{
			if (m_collided)
				return;
			
			m_collided = true;

			Destroy(this.gameObject);

			Debug.LogFormat("Collision with pickup");

			this.onCollided();

		}
		
	}

}
