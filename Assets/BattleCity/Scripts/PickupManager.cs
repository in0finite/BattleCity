﻿using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BattleCity
{
	
	public class PickupManager : MonoBehaviour
	{
		
		int m_numEnemyTanksDestroyed = 0;
		public int numPickupsPerLevel = 2;

		public GameObject pickupPrefab;
		public Material[] pickupMaterials = new Material[0];

		System.Action[] m_pickupActions = new System.Action[]{
			OnStarPickedUp,
			OnFreezePickedUp,
		};



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
		}

		void OnEnemyTankDestroyed(EnemyTank enemyTank)
		{
			m_numEnemyTanksDestroyed ++;
			int numTanksToBeDestroyedForSpawn = Mathf.CeilToInt( EnemyTankSpawner.Instance.numTanksPerLevel / (float) (this.numPickupsPerLevel + 1) );
			Debug.LogFormat("numTanksToBeDestroyedForSpawn {0}, m_numEnemyTanksDestroyed {1}", numTanksToBeDestroyedForSpawn, m_numEnemyTanksDestroyed);
			if (m_numEnemyTanksDestroyed >= numTanksToBeDestroyedForSpawn)
			{
				m_numEnemyTanksDestroyed = 0;
				SpawnPickup();
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
			int index = Random.Range(0, 2);

			// assign material
			pickup.GetComponent<MeshRenderer>().sharedMaterial = this.pickupMaterials[index];

			// assign action on collision
			pickup.onCollided += m_pickupActions[index];

			Debug.LogFormat("Spawned pickup {0} at {1}", index, pos);
		}
		
		static void OnStarPickedUp()
		{

		}

		static void OnFreezePickedUp()
		{

		}

	}

}
