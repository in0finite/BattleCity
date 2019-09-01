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
		}
		
		void Start()
		{
			
		}

		void Update()
		{
			
		}

		public static void StartFirstLevel()
		{
			CurrentLevel = 1;
			CurrentScore = 0;
			SceneManager.LoadScene("Map");
			LoadLevel();
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
						Vector2 pos = new Vector2(j, i);
						GameObject go = Instantiate( dict[c], new Vector3(j, 0f, i), Quaternion.identity);
						var mapObject = go.GetComponent<MapObject>();
						mapObject.Position = pos;

					}
				}
			}
			
		}
		
	}

}
