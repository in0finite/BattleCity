using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
	
	public class Tank : MonoBehaviour
	{
		
		public float moveSpeed = 1.0f;
		public float fireInterval = 0.5f;
		public Transform firePosition;
		public float bulletVelocity = 4f;
		public float bulletDamage = 50f;
		public string bulletLayerName = "";
		public float health = 100f;

		float m_timeWhenFired = 0f;
		public float TimeSinceFired => Time.time - m_timeWhenFired;

		public bool CanFire => this.TimeSinceFired >= this.fireInterval;

		float m_originalHealth, m_originalMoveSpeed, m_originalBulletVelocity, m_originalBulletDamage;



		protected virtual void Awake()
		{
			m_originalHealth = this.health;
			m_originalMoveSpeed = this.moveSpeed;
			m_originalBulletVelocity = this.bulletVelocity;
			m_originalBulletDamage = this.bulletDamage;
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
			if (this.health <= 0f)
				return;
			
			this.health -= bullet.damage;
			
			if (this.health <= 0f)
			{
				Destroy(this.gameObject);
				AudioManager.Instance.PlaySoundEffect(AudioManager.kTankDestroySoundIndex);
				this.OnKilled(bullet);
			}
		}

		protected virtual void OnKilled(Bullet bullet)
		{

		}

		public virtual void TryFire()
		{

			if (this.CanFire)
			{
				
				m_timeWhenFired = Time.time;
				GameObject bulletGo = Instantiate(MapManager.Instance.bulletPrefab, this.firePosition.position, this.firePosition.rotation);
				bulletGo.layer = LayerMask.NameToLayer(this.bulletLayerName);
				Bullet bullet = bulletGo.GetComponent<Bullet>();
				bullet.TankShooter = this;
				bullet.damage = this.bulletDamage;
				bulletGo.GetComponent<Rigidbody>().velocity = bulletGo.transform.forward * this.bulletVelocity;

				// play sound
				AudioManager.Instance.PlaySoundEffect(AudioManager.kBulletSoundIndex);
				
			}

		}

		public void SetParamsBasedOnCurrentLevel()
		{
			int currentLevel = MapManager.CurrentLevel;
			int levelMul = currentLevel - 1;

			this.health = m_originalHealth * (1 + levelMul * 0.0f);
			this.moveSpeed = m_originalMoveSpeed * (1 + levelMul * 0.25f);
			this.bulletVelocity = m_originalBulletVelocity * (1 + levelMul * 0.1f);
			this.bulletDamage = m_originalBulletDamage * (1 + levelMul * 0.25f);

		}

		public void IncreaseHealth(float perc)
		{
			this.health += m_originalHealth * perc;
		}

		public void IncreaseBulletDamage(float perc)
		{
			this.bulletDamage += m_originalBulletDamage * perc;
		}

		public float GetHealthPerc()
		{
			return this.health / m_originalHealth;
		}

		public void SetHealthPerc(float perc)
		{
			this.health = m_originalHealth * perc;
		}

		public float GetBulletDamagePerc()
		{
			return this.bulletDamage / m_originalBulletDamage;
		}

		public void SetBulletDamagePerc(float perc)
		{
			this.bulletDamage = m_originalBulletDamage * perc;
		}
		
	}

}
