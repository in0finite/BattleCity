﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

namespace BattleCity
{
	
	public class MapManager : MonoBehaviour
	{
		public static MapManager Instance { get; private set; }

		public static int CurrentLevel { get; private set; }
		public static int CurrentScore { get; set; }
		public static int NumLifes { get; private set; }

		public GameObject brickPrefab, waterPrefab, wallPrefab, flagPrefab, playerTankPrefab, enemyTankPrefab, playerSpawnPrefab,
			enemySpawnPrefab;
		public GameObject bulletPrefab;

		public Material playerTankMaterial, enemyTankMaterial;

		public float timeToWaitBeforeLoadingNextLevel = 4f;

		public int startingNumLifes = 3;

		static bool s_isLoadingLevel = false;
		public static bool IsGameOver { get; private set; } = false;

		static List<string> s_availableLevels = new List<string>();

		static int s_mapWidth, s_mapHeight;
		public static int MapWidth => s_mapWidth;
		public static int MapHeight => s_mapHeight;
		static MapObject[,] s_mapObjects;
		public static IEnumerable<MapObject> MapObjects {
			get {
				foreach (var obj in s_mapObjects)
					if (obj != null)
						yield return obj;
			}
		}
		public static MapObject GetMapObjectAt(Vector2 pos) => s_mapObjects[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)];
		public static MapObject GetMapObjectAt(float x, float y) => GetMapObjectAt(new Vector2(x, y));

		public static bool IsInsideMap(Vector2 pos) => IsInsideMap(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
		public static bool IsInsideMap(int x, int y) => x >= 0 && y >= 0 && x < s_mapWidth && y < s_mapHeight;

		public static bool IsMapOpened => SceneManager.GetActiveScene().name == "Map";

		public static event System.Action onLevelLoaded = delegate {};



		void Awake()
		{
			Instance = this;

			s_isLoadingLevel = false;

			IsGameOver = false;

			CurrentScore = 0;
			CurrentLevel = 1;

			// find available levels
			s_availableLevels = System.IO.Directory.GetFiles(Application.streamingAssetsPath)
				.Where(str => str.Contains("level") && str.EndsWith(".txt"))
				.OrderBy(str => str)
				.ToList();

		}

		void OnEnable()
		{
			SceneManager.activeSceneChanged += this.SceneChanged;
		}

		void OnDisable()
		{
			SceneManager.activeSceneChanged -= this.SceneChanged;
		}

		void SceneChanged(Scene s1, Scene s2)
		{
			LoadLevelIfMapIsOpened();
		}
		
		void Start()
		{
			//LoadLevelIfMapIsOpened();
		}

		void LoadLevelIfMapIsOpened()
		{
			if (IsMapOpened)
			{
				CurrentLevel = 1;
				CurrentScore = 0;
				LoadLevel(0f);
			}
		}

		void Update()
		{

			if (!s_isLoadingLevel && !IsGameOver)
			{
				if (EnemyTank.AllTanks.Count < 1 && EnemyTankSpawner.Instance.NumTanksLeftToSpawn < 1)
				{
					LoadNextLevel();
				}
			}

		}

		public static void StartFirstLevel()
		{
			MenuManager.ActiveMenu = InGameMenu.Instance;
			SceneManager.LoadScene("Map");
		}

		static void LoadNextLevel()
		{
			if (IsGameOver)
				return;
			
			CurrentLevel ++;
			Debug.LogFormat("Starting to load level {0}", CurrentLevel);
			LoadLevel(Instance.timeToWaitBeforeLoadingNextLevel);
		}

		static void LoadLevel(float timeToWaitBeforeLoading)
		{
		//	string path = System.IO.Path.Combine(Application.streamingAssetsPath, "level" + CurrentLevel + ".txt");
			int index = (CurrentLevel - 1) % s_availableLevels.Count;
			string path = s_availableLevels[index];
			LoadLevel(System.IO.File.ReadAllLines(path), timeToWaitBeforeLoading);
		}

		static void LoadLevel(string[] lines, float timeToWaitBeforeLoading)
		{
			Instance.StartCoroutine(LoadLevelCoroutine(lines, timeToWaitBeforeLoading));
		}

		static System.Collections.IEnumerator LoadLevelCoroutine(string[] lines, float timeToWaitBeforeLoading)
		{

			s_isLoadingLevel = true;

			yield return null;
			yield return new WaitForSeconds(timeToWaitBeforeLoading);

			// first destroy all existing objects
			foreach(GameObject go in FindObjectsOfType<MapObject>().Select(obj => obj.gameObject)
				.Concat(FindObjectsOfType<EnemyTank>().Select(obj => obj.gameObject)
				.Concat(FindObjectsOfType<Bullet>().Select(obj => obj.gameObject))))
			{
				Destroy(go);
			}

			yield return null;
			yield return null;


			var dict = new Dictionary<char, GameObject>(){
				{'b', Instance.brickPrefab}, {'w', Instance.wallPrefab}, {'~', Instance.waterPrefab}, 
				{'f', Instance.flagPrefab},
				{'s', Instance.playerSpawnPrefab},
				{'e', Instance.enemySpawnPrefab},
			};

			int width = s_mapWidth = lines[0].Length;
			int height = s_mapHeight = lines.Length;

			s_mapObjects = new MapObject[width, height];

			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					char c = lines[i][j];
					if (dict.ContainsKey(c))
					{
						Vector2Int pos = new Vector2Int(j, height - i - 1);
						SpawnMapObject(dict[c], pos);
					}
				}
			}

			// set ground scale and position

			Transform groundTransform = GameObject.Find("Ground").transform;

			Vector3 groundScale = groundTransform.localScale;
			groundScale.x = width;
			groundScale.z = height;
			groundTransform.localScale = groundScale;

			groundTransform.position = new Vector3(width / 2f - 0.5f, groundTransform.position.y, height / 2f - 0.5f);

			// set camera position
			if (CurrentLevel <= 1)
				CameraManager.Instance.SwitchToSideView();

			if (null == PlayerTank.Instance)
			{
				SpawnPlayerTank();
			}
			else
			{
				// reset position of player tank
				PlayerTank tank = PlayerTank.Instance;

				Transform tr = tank.transform;
				Vector3 pos = FindObjectOfType<PlayerSpawn>().transform.position;
				pos.y = tr.position.y;

				tank.CController.enabled = false;
				tr.position = pos;
				tr.rotation = Quaternion.identity;
				tank.CController.enabled = true;

				// restore health
				if (tank.GetHealthPerc() < 1)
					tank.SetHealthPerc(1);

			}

			// reset num lifes
			NumLifes = Instance.startingNumLifes;

			AudioManager.Instance.PlayMusic(AudioManager.kLevelLoadSoundIndex);

			s_isLoadingLevel = false;

			Debug.LogFormat("LoadLevel() finished");

			onLevelLoaded();

		}

		public static MapObject SpawnMapObject(GameObject prefab, Vector2Int pos)
		{
			GameObject go = Instantiate( prefab, new Vector3(pos.x, 0f, pos.y), Quaternion.identity);
			var mapObject = go.GetComponent<MapObject>();
			mapObject.Position = pos;

			s_mapObjects[pos.x, pos.y] = mapObject;

			return mapObject;
		}

		static void SpawnPlayerTank()
		{
			Transform spawn = FindObjectOfType<PlayerSpawn>().transform;

			Instantiate(Instance.playerTankPrefab, new Vector3(spawn.position.x, Instance.playerTankPrefab.transform.position.y, spawn.position.z),
				Quaternion.identity);
			
		}

		public static void OnPlayerTankDestroyed()
		{
			if (IsGameOver)
				return;
			
			NumLifes --;
			if (NumLifes <= 0)
			{
				// game over
				IsGameOver = true;
				Debug.LogFormat("Game over - out of lifes");
				OnGameOver();
				return;
			}

			// spawn it later
			Instance.Invoke(nameof(SpawnPlayerTankLater), 3f);

		}

		void SpawnPlayerTankLater()
		{
			if (MapManager.IsGameOver)
				return;
			
			if (PlayerTank.Instance != null)
				return;
			
			if (EnemyTankSpawner.CanSpawnTankAt(FindObjectOfType<PlayerSpawn>().Position))
				SpawnPlayerTank();
			else
				this.Invoke(nameof(SpawnPlayerTankLater), 1f);
			
		}

		public static void OnFlagDestroyed()
		{
			if (IsGameOver)
				return;
			
			// game over

			Debug.LogFormat("Flag destroyed - game over");

			OnGameOver();

		}

		static void OnGameOver()
		{
			IsGameOver = true;

			Instance.CancelInvoke(nameof(FinishGameLater));
			Instance.Invoke(nameof(FinishGameLater), 3f);

			AudioManager.Instance.PlaySoundEffect(AudioManager.kGameOverSoundIndex);

			// destroy all bullets
			foreach (Bullet bullet in FindObjectsOfType<Bullet>())
			{
				Destroy(bullet.gameObject);
			}

		}

		void FinishGameLater()
		{
			SceneManager.LoadScene("MainMenu");

			ScoreMenu.Instance.ParentMenu = MainMenu.Instance;
			ScoreMenu.Instance.CurrentScore = MapManager.CurrentScore;
			ScoreMenu.Instance.ExitIfNotEnoughScore = true;
			MenuManager.ActiveMenu = ScoreMenu.Instance;

		}


		public static IEnumerable<Tank> GetAllTanks()
		{
			foreach (var enemyTank in EnemyTank.AllTanks)
				yield return enemyTank;
			
			if (PlayerTank.Instance != null)
				yield return PlayerTank.Instance;
			
		}
		
	}

}
