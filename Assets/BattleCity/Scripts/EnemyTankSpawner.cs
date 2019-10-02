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
		public int NumTanksLeftToSpawn => this.numTanksPerLevel - m_numTanksSpawned;



		void Awake()
		{
			Instance = this;
		}
		
		void OnEnable()
		{
			MapManager.onLevelLoaded += OnLevelLoaded;
		}

		void OnDisable()
		{
			MapManager.onLevelLoaded -= OnLevelLoaded;
		}

		void OnLevelLoaded()
		{
			m_numTanksSpawned = 0;
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

				if (MapManager.IsGameOver)
					continue;

				var enemyTanks = FindObjectsOfType<EnemyTank>();
				if (enemyTanks.Length >= this.maxNumTanksAtATime)
					continue;
				
				// find position for spawning
				// use only spawns which are not occupied
				EnemySpawn[] spawns = MapManager.MapObjects.OfType<EnemySpawn>().Where(s => ! EnemyTank.IsAnyTankAtBlock(s.Position, null) ).ToArray();
				if (spawns.Length < 1)
					continue;

				EnemySpawn spawn = spawns[ Random.Range(0, spawns.Length) ];

				// spawn new tank

				m_numTanksSpawned ++;

				EnemyTank enemyTank = Instantiate(MapManager.Instance.enemyTankPrefab, new Vector3(spawn.transform.position.x, MapManager.Instance.enemyTankPrefab.transform.position.y, spawn.transform.position.z), 
					Quaternion.LookRotation(-Vector3.forward)).GetComponent<EnemyTank>();
				
				enemyTank.SetParamsBasedOnCurrentLevel();

			}
		}

		void Update()
		{
			
		}
		
	}

}
