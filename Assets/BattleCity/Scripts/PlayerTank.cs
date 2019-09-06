using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
	
	public class PlayerTank : MonoBehaviour
	{
		public float moveSpeed = 1.0f;
		public float fireInterval = 0.5f;
		public Transform firePosition;
		public float bulletVelocity = 4f;

		float m_timeWhenFired = 0f;

		CharacterController m_cc;


		void Awake()
		{
			m_cc = this.GetComponent<CharacterController>();
		}
		
		void Start()
		{
			// set material
			foreach(var mr in this.GetComponentsInChildren<MeshRenderer>())
			{
				mr.sharedMaterial = MapManager.Instance.playerTankMaterial;
			}

		}

		void Update()
		{

			Vector3 inputMove = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
			inputMove.Normalize();
			if (inputMove.sqrMagnitude > 0f)
			{
				Vector3 moveDelta = inputMove * Time.deltaTime * this.moveSpeed;
				//this.transform.position += moveDelta;
				m_cc.Move(moveDelta);
				this.transform.rotation = Quaternion.LookRotation(inputMove);
			}

			if (Input.GetButton("Submit"))
			{
				if (Time.time - m_timeWhenFired >= this.fireInterval)
				{
					// enough time passed

					m_timeWhenFired = Time.time;
					GameObject bulletGo = Instantiate(MapManager.Instance.bulletPrefab, this.firePosition.position, this.firePosition.rotation);
					bulletGo.GetComponent<Rigidbody>().velocity = bulletGo.transform.forward * this.bulletVelocity;

				}
			}

		}
		
	}

}
