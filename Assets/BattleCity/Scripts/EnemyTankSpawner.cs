using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

namespace BattleCity
{
	
	public class EnemyTankSpawner : MonoBehaviour
	{
		public static EnemyTankSpawner Instance { get; private set; }

		public float spawnInterval = 4f;
		public int maxNumTanksAtATime = 4;
		public int numTanksPerLevel = 20;

		int m_numTanksSpawned = 0;



		void Awake()
		{
			Instance = this;
		}
		
		void Start()
		{
			this.StartCoroutine(this.SpawnCoroutine());
		}

		IEnumerator SpawnCoroutine()
		{
			while (true)
			{
				yield return new WaitForSeconds(this.spawnInterval);

				if (!MapManager.IsMapOpened)
					continue;

				if (m_numTanksSpawned >= this.numTanksPerLevel)
					continue;

				var enemyTanks = FindObjectsOfType<EnemyTank>();
				if (enemyTanks.Length >= this.maxNumTanksAtATime)
					continue;
				
				// spawn new tank

				m_numTanksSpawned ++;

				// find position for spawning
				EnemySpawn[] spawns = MapManager.MapObjects.OfType<EnemySpawn>().ToArray();
				EnemySpawn spawn = spawns[ Random.Range(0, spawns.Length) ];

				Instantiate(MapManager.Instance.enemyTankPrefab, new Vector3(spawn.transform.position.x, MapManager.Instance.enemyTankPrefab.transform.position.y, spawn.transform.position.z), 
					Quaternion.LookRotation(-Vector3.forward));

			}
		}

		void Update()
		{
			
		}
		
	}

}
