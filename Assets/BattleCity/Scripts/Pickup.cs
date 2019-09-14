using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
	
	public class Pickup : MapObject
	{
		public override bool IsPassable => true;

		bool m_collided = false;
		public event System.Action onCollided = delegate {};


		void Start()
		{
			AudioManager.Instance.PlaySoundEffect(AudioManager.kPickupSoundIndex);
		}

		void OnTriggerEnter(Collider collider)
		{
			if (m_collided)
				return;
			
			m_collided = true;

			Destroy(this.gameObject);

		//	Debug.LogWarningFormat("Collision with pickup");

			AudioManager.Instance.PlaySoundEffect(AudioManager.kPickupPickedUpSoundIndex);

			this.onCollided();

		}
		
	}

}
