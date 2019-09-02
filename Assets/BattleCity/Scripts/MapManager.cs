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

		public GameObject brickPrefab, waterPrefab, wallPrefab, flagPrefab;



		void Awake()
		{
			Instance = this;
			SceneManager.activeSceneChanged += this.SceneChanged;
		}

		void SceneChanged(Scene s1, Scene s2)
		{
			if (s2.name == "Map")
			{
				CurrentLevel = 1;
				CurrentScore = 0;
				LoadLevel();
			}
		}
		
		void Start()
		{
			
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
				{'b', Instance.brickPrefab}, {'w', Instance.wallPrefab}, {'~', Instance.waterPrefab}, {'f', Instance.flagPrefab}
			};

			int width = lines[0].Length;
			int height = lines.Length;

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

					}
				}
			}

			Transform groundTransform = GameObject.Find("Ground").transform;

			Vector3 groundScale = groundTransform.localScale;
			groundScale.x = width;
			groundScale.z = height;
			groundTransform.localScale = groundScale;

			groundTransform.position = new Vector3(width / 2f - 0.5f, groundTransform.position.y, height / 2f - 0.5f);

			Vector3 camPos = Camera.main.transform.position;
			camPos.x = width / 2f;
			Camera.main.transform.position = camPos;

		}
		
	}

}
