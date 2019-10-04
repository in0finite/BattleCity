using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BattleCity
{
	
	public class PickupManager : MonoBehaviour
	{
		public static PickupManager Instance { get; private set; }

		int m_numEnemyTanksDestroyed = 0;
		public int numPickupsPerLevel = 2;

		public GameObject pickupPrefab;
		public Material[] pickupMaterials = new Material[0];

		System.Action[] m_pickupActions = new System.Action[]{
			OnStarPickedUp,
			OnFreezePickedUp,
			OnShieldPickedUp,
		};

		static PickupEffect[] s_pickupEffects = new PickupEffect[] {
			new StrengthEffect(),
			new StopEffect(),
			new ShieldEffect(),
		};

		public float pickupLifeTime = 10f;

		public float freezePickupDuration = 8f;
		public float shieldPickupDuration = 10f;

		bool m_shouldSpawnPickup = false;



		void Awake()
		{
			Instance = this;
		}

		void OnEnable()
		{
			EnemyTank.onDestroyed += OnEnemyTankDestroyed;
			MapManager.onLevelLoaded += OnLevelLoaded;
		}

		void OnDisable()
		{
			EnemyTank.onDestroyed -= OnEnemyTankDestroyed;
			MapManager.onLevelLoaded -= OnLevelLoaded;
		}

		void OnLevelLoaded()
		{
			m_numEnemyTanksDestroyed = 0;
			EnemyTank.AreAllEnemyTanksFrozen = false;
		}

		void OnEnemyTankDestroyed(EnemyTank enemyTank)
		{
			m_numEnemyTanksDestroyed ++;
			int numTanksToBeDestroyedForSpawn = Mathf.CeilToInt( EnemyTankSpawner.Instance.numTanksPerLevel / (float) (this.numPickupsPerLevel + 1) );
			Debug.LogFormat("numTanksToBeDestroyedForSpawn {0}, m_numEnemyTanksDestroyed {1}", numTanksToBeDestroyedForSpawn, m_numEnemyTanksDestroyed);
			if (m_numEnemyTanksDestroyed >= numTanksToBeDestroyedForSpawn)
			{
				m_numEnemyTanksDestroyed = 0;
				//SpawnPickup();
				m_shouldSpawnPickup = true;
			}
		}

		void SpawnPickup()
		{
			// find a place for spawning

			List<Vector2Int> freePlaces = new List<Vector2Int>();
			for (int i=0; i < MapManager.MapWidth; i++)
			{
				for (int j=0; j < MapManager.MapHeight; j++)
				{
					if (null == MapManager.GetMapObjectAt(i, j))
						freePlaces.Add(new Vector2Int(i, j));
				}
			}
			
			if (freePlaces.Count < 1)
				return;

			// pick a random place
			Vector2Int pos = freePlaces[Random.Range(0, freePlaces.Count)];

			// spawn it
			var pickup = MapManager.SpawnMapObject(this.pickupPrefab, pos).GetComponent<Pickup>();

			// choose type of pickup
			int index = Random.Range(0, m_pickupActions.Length);

			// assign material
			pickup.GetComponent<MeshRenderer>().sharedMaterial = this.pickupMaterials[index];

			// assign action on collision
			pickup.onCollided += m_pickupActions[index];

			// destroy pickup when it's lifetime expires
			Destroy(pickup.gameObject, this.pickupLifeTime);

			Debug.LogFormat("Spawned pickup {0} at {1}", index, pos);
		}
		
		public float GetMaxPlayerTankHealth()
		{
			return MapManager.Instance.playerTankPrefab.GetComponent<Tank>().health * 2.0f;
		}

		public float GetMaxPlayerTankBulletDamage()
		{
			return MapManager.Instance.playerTankPrefab.GetComponent<Tank>().bulletDamage * 2.0f;
		}

		static void OnStarPickedUp()
		{
			s_pickupEffects[0].ApplyEffect();
		}

		static void OnFreezePickedUp()
		{
			s_pickupEffects[1].ApplyEffect();

			Instance.CancelInvoke(nameof(CancelFreeze));
			Instance.Invoke(nameof(CancelFreeze), Instance.freezePickupDuration);
		}

		void CancelFreeze()
		{
			s_pickupEffects[1].CancelEffect();
		}

		static void OnShieldPickedUp()
		{
			if (PlayerTank.Instance != null)
			{
				s_pickupEffects[2].ApplyEffect();

				Instance.CancelInvoke(nameof(CancelShield));
				Instance.Invoke(nameof(CancelShield), Instance.shieldPickupDuration);
			}
		}

		void CancelShield()
		{
			s_pickupEffects[2].CancelEffect();
		}


		void Update()
		{

			if (m_shouldSpawnPickup)
			{
				m_shouldSpawnPickup = false;
				SpawnPickup();
			}

		}

	}

}
