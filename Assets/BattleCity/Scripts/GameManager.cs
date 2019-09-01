using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleCity
{

	public class GameManager : MonoBehaviour
	{

	    void Start()
	    {
			
	        if (Application.isEditor)
				SetMaxFps(15);
			else
				SetMaxFps(30);
			
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
