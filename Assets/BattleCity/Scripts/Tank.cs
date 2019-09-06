﻿using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
	
	public class Tank : MonoBehaviour
	{
		
		public float moveSpeed = 1.0f;
		public float fireInterval = 0.5f;
		public Transform firePosition;
		public float bulletVelocity = 4f;
		public float health = 100f;

		float m_timeWhenFired = 0f;



		protected virtual void Awake()
		{
			
		}

		protected virtual void OnEnable()
		{
			
		}

		protected virtual void OnDisable()
		{
			
		}
		
		protected virtual void Start()
		{
			
		}


		public virtual void OnCollidedWithBullet(Bullet bullet)
		{
			this.health -= bullet.damage;
			if (this.health <= 0f)
				Destroy(this.gameObject);
		}

		public virtual void TryFire()
		{

			if (Time.time - m_timeWhenFired >= this.fireInterval)
			{
				// enough time passed

				m_timeWhenFired = Time.time;
				GameObject bulletGo = Instantiate(MapManager.Instance.bulletPrefab, this.firePosition.position, this.firePosition.rotation);
				bulletGo.GetComponent<Rigidbody>().velocity = bulletGo.transform.forward * this.bulletVelocity;

			}

		}
		
	}

}
