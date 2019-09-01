using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
	
	public class DontDestroyOnLoad : MonoBehaviour
	{
		
		void Awake()
		{
			DontDestroyOnLoad(this.gameObject);
		}
		
	}

}
