using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
	
	public class Bullet : MonoBehaviour
	{
		
		void Awake()
		{
			Debug.LogFormat("Bullet.Awake()");
		}
		
		void Start()
		{
			Destroy(this.gameObject, 20f);
		}

		void OnCollisionEnter(Collision collision)
		{
			//collision.gameObject;
			
			Debug.LogFormat("Bullet collision with: {0}", collision.gameObject.name);

		}

		void Update()
		{
			
		}
		
	}

}
