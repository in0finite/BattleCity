using System.Collections.Generic;
using UnityEngine;

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
			
	    }

		static void SetMaxFps(int maxFps)
		{
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = maxFps;
		}

	    void Update()
	    {
	        
	    }
	    
	}

}
