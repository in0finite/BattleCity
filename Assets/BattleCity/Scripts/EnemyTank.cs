using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
	
	public class EnemyTank : MonoBehaviour
	{
		
		void Awake()
		{
			
		}
		
		void Start()
		{
			// set material
			foreach(var mr in this.GetComponentsInChildren<MeshRenderer>())
			{
				mr.sharedMaterial = MapManager.Instance.enemyTankMaterial;
			}

		}

		void Update()
		{
			
		}
		
	}

}
