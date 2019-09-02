using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
	
	public class PlayerTank : MonoBehaviour
	{
		public float moveSpeed = 1.0f;


		void Awake()
		{
			
		}
		
		void Start()
		{
			
		}

		void Update()
		{
			Vector3 inputMove = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
			inputMove.Normalize();
			if (inputMove.sqrMagnitude > 0f)
			{
				this.transform.position += inputMove * Time.deltaTime * this.moveSpeed;
				this.transform.rotation = Quaternion.LookRotation(inputMove);
			}
		}
		
	}

}
