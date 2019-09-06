using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
	
	public class Bullet : MonoBehaviour
	{
		public Tank TankShooter { get; set; }
		public float damage = 50f;
		bool m_collided = false;



		void Awake()
		{
			Debug.LogFormat("Bullet.Awake()");
		}
		
		void Start()
		{
			// just in case
			Destroy(this.gameObject, 20f);
		}

		void OnCollisionEnter(Collision collision)
		{
			
			if (m_collided)
				return;
			
			m_collided = true;
			
			Debug.LogFormat("Bullet collision with: {0}", collision.gameObject.name);

			var targetTank = collision.gameObject.GetComponent<Tank>();
			var targetMapObject = collision.gameObject.GetComponent<MapObject>();
			if (targetTank != null)
			{
				targetTank.OnCollidedWithBullet(this);
			}
			else if (targetMapObject != null)
			{
				targetMapObject.OnCollidedWithBullet(this);
			}

			Destroy(this.gameObject);

		}

		void Update()
		{
			
		}
		
	}

}
