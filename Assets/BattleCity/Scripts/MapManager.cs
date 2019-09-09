using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleCity
{
	
	public class MapManager : MonoBehaviour
	{
		public static MapManager Instance { get; private set; }

		public static int CurrentLevel { get; private set; }
		public static int CurrentScore { get; private set; }

		public GameObject brickPrefab, waterPrefab, wallPrefab, flagPrefab, playerTankPrefab, enemyTankPrefab, playerSpawnPrefab,
			enemySpawnPrefab;
		public GameObject bulletPrefab;

		public Material playerTankMaterial, enemyTankMaterial;

		static int m_mapWidth, m_mapHeight;
		public static int MapWidth => m_mapWidth;
		public static int MapHeight => m_mapHeight;
		static MapObject[,] m_mapObjects;
		public static IEnumerable<MapObject> MapObjects {
			get {
				foreach (var obj in m_mapObjects)
					if (obj != null)
						yield return obj;
			}
		}
		public static MapObject GetMapObjectAt(Vector2 pos) => m_mapObjects[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)];
		public static MapObject GetMapObjectAt(float x, float y) => GetMapObjectAt(new Vector2(x, y));

		public static bool IsInsideMap(Vector2 pos) => IsInsideMap(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
		public static bool IsInsideMap(int x, int y) => x >= 0 && y >= 0 && x < m_mapWidth && y < m_mapHeight;

		public static bool IsMapOpened => SceneManager.GetActiveScene().name == "Map";



		void Awake()
		{
			Instance = this;
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
				LoadLevel();
			}
		}

		void Update()
		{
			
		}

		public static void StartFirstLevel()
		{
			MenuManager.ActiveMenu = null;
			SceneManager.LoadScene("Map");
		}

		static void LoadLevel()
		{
			string path = System.IO.Path.Combine(Application.streamingAssetsPath, "level" + CurrentLevel + ".txt");
			LoadLevel(System.IO.File.ReadAllLines(path));
		}

		static void LoadLevel(string[] lines)
		{
			var dict = new Dictionary<char, GameObject>(){
				{'b', Instance.brickPrefab}, {'w', Instance.wallPrefab}, {'~', Instance.waterPrefab}, 
				{'f', Instance.flagPrefab},
				{'s', Instance.playerSpawnPrefab},
				{'e', Instance.enemySpawnPrefab},
			};

			int width = m_mapWidth = lines[0].Length;
			int height = m_mapHeight = lines.Length;

			m_mapObjects = new MapObject[width, height];

			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					char c = lines[i][j];
					if (dict.ContainsKey(c))
					{
						Vector2 pos = new Vector2(j, height - i - 1);
						GameObject go = Instantiate( dict[c], new Vector3(pos.x, 0f, pos.y), Quaternion.identity);
						var mapObject = go.GetComponent<MapObject>();
						mapObject.Position = pos;

						m_mapObjects[j, height - i - 1] = mapObject;
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

			Vector3 camPos = Camera.main.transform.position;
			camPos.x = width / 2f;
			Camera.main.transform.position = camPos;

			// spawn player tank
			SpawnPlayerTank();

			Debug.LogFormat("LoadLevel() finished");

		}

		static void SpawnPlayerTank()
		{
			Transform spawn = FindObjectOfType<PlayerSpawn>().transform;

			var tankGo = Instantiate(Instance.playerTankPrefab);
			tankGo.transform.position = new Vector3(spawn.position.x, tankGo.transform.position.y, spawn.position.z);
			tankGo.transform.forward = Vector3.forward;

		}

		public static void OnPlayerTankDestroyed()
		{
			// spawn it later
			Instance.Invoke(nameof(SpawnPlayerTankLater), 3f);
		}

		void SpawnPlayerTankLater()
		{
			if (PlayerTank.Instance != null)
				return;
			SpawnPlayerTank();
		}
		
	}

}
