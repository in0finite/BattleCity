using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace BattleCity
{

	public class ScoreMenu : Menu
	{
		public static ScoreMenu Instance { get; private set; }

		public Button saveButton, cancelButton, okButton;
		public Text nameText;
		public InputField nameInputField;
		public GameObject scorePanel;

		public GameObject scoreEntryPrefab;

		public Menu ParentMenu { get; set; }
		public int CurrentScore { get; set; }

		public class ScoreInfo
		{
			public string playerName;
			public int score;
			public System.DateTime date;
		}

		public const int kMaxNumScores = 10;

		List<ScoreInfo> m_currentScoreList = new List<ScoreInfo>();
		ScoreInfo m_newScoreInfo;



		void Awake()
		{
			Instance = this;
		}

	    void Start()
	    {
	        saveButton.onClick.AddListener(() => SaveClicked());
			cancelButton.onClick.AddListener(() => MenuManager.ActiveMenu = this.ParentMenu);
			okButton.onClick.AddListener(() => MenuManager.ActiveMenu = this.ParentMenu);
	    }

		public override void OnBecameActive()
		{

			m_newScoreInfo = null;
			var scoreList = m_currentScoreList = LoadScore();

			bool bEnableInput = this.CurrentScore > 0 && (scoreList.Count < kMaxNumScores || scoreList.Any(item => item.score < this.CurrentScore));
			
			this.saveButton.gameObject.SetActive(bEnableInput);
			this.cancelButton.gameObject.SetActive(bEnableInput);
			this.nameText.gameObject.SetActive(bEnableInput);
			this.nameInputField.gameObject.SetActive(bEnableInput);

			this.okButton.gameObject.SetActive(! bEnableInput);


			// insert new score into the list
			if (bEnableInput)
			{
				var newScoreInfo = m_newScoreInfo = new ScoreInfo(){playerName = "", score = this.CurrentScore, date = System.DateTime.Now};

				scoreList.Add(newScoreInfo);
				SortScoreList(scoreList);

				if (scoreList.Count > kMaxNumScores)
				{
					scoreList.RemoveAt(scoreList.Count - 1);
				}
			}

			// populate UI

			// delete all children
			for (int i=0; i < this.scorePanel.transform.childCount; i++)
			{
				Destroy(this.scorePanel.transform.GetChild(i));
			}

			// create children
			foreach (var scoreInfo in scoreList)
			{
				var scoreEntryGo = Instantiate(this.scoreEntryPrefab, this.scorePanel.transform, false);
				scoreEntryGo.GetComponent<Text>().text = scoreInfo.playerName + " - " + scoreInfo.score;
			}


		}

		static void SortScoreList(List<ScoreInfo> list)
		{
			list.Sort((a, b) => a.score.CompareTo(b.score));
		}

		public static List<ScoreInfo> LoadScore()
		{
			var list = new List<ScoreInfo>();

			bool hasScoreList = PlayerPrefs.HasKey("hasScoreList");
			if (!hasScoreList)
				return list;
			
			int numScores = PlayerPrefs.GetInt("numScores");

			for (int i = 0; i < numScores && i < kMaxNumScores; i++)
			{
				list.Add(new ScoreInfo() {playerName = PlayerPrefs.GetString("playerName" + i), 
					score = PlayerPrefs.GetInt("score" + i),
					date = System.DateTime.Parse(PlayerPrefs.GetString("date" + i))});
				
			}

			SortScoreList(list);

			return list;
		}

		static void SaveScoreList(List<ScoreInfo> list)
		{

			SortScoreList(list);

			for (int i = 0; i < list.Count; i++)
			{
				PlayerPrefs.SetString("playerName" + i, list[i].playerName);
				PlayerPrefs.SetInt("score" + i, list[i].score);
				PlayerPrefs.SetString("date" + i, list[i].date.ToShortDateString());
			}

			PlayerPrefs.SetInt("numScores", list.Count);
			PlayerPrefs.SetInt("hasScoreList", 1);

			PlayerPrefs.Save();

		}

		void SaveClicked()
		{
			if (null == m_newScoreInfo)
				return;
			
			string playerName = this.nameInputField.text.Trim();
			if (playerName.Length < 1)
				return;
			
			m_newScoreInfo.playerName = playerName;

			SaveScoreList(m_currentScoreList);

			MenuManager.ActiveMenu = this.ParentMenu;

		}

	    void Update()
	    {
	        
	    }
	    
	}

}
