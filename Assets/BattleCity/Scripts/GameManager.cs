using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleCity
{

	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance { get; private set; }


		void Awake()
		{
			if (Instance != null)
			{
				Destroy(this.gameObject);
				return;
			}

			Instance = this;
			
		}

	    void Start()
	    {
			
	        if (Application.isEditor)
				SetMaxFps(15);
			else
				SetMaxFps(30);
			
			if (0 == SceneManager.GetActiveScene().buildIndex)
				StartCoroutine(this.ChangeScene());

	    }

		static void SetMaxFps(int maxFps)
		{
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = maxFps;
		}
		
		IEnumerator ChangeScene()
		{
			yield return null;
			yield return null;
			SceneManager.LoadScene("MainMenu");
		}

	    void Update()
	    {
	        
	    }
	    
	}

}
