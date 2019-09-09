using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
	
	public class PlayerTank : Tank
	{
		
		float m_originalYPos;

		CharacterController m_cc;


		protected override void Awake()
		{
			base.Awake();
			m_cc = this.GetComponent<CharacterController>();
			m_originalYPos = this.transform.position.y;
		}
		
		protected override void Start()
		{
			base.Start();

			// set material
			foreach(var mr in this.GetComponentsInChildren<MeshRenderer>())
			{
				mr.sharedMaterial = MapManager.Instance.playerTankMaterial;
			}

		}

		void FixedUpdate()
		{

			Vector3 inputMove = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
			inputMove.Normalize();
			if (inputMove.sqrMagnitude > 0f)
			{
				Vector3 moveDelta = inputMove * Time.deltaTime * this.moveSpeed;
				//this.transform.position += moveDelta;
				m_cc.Move(moveDelta);
				this.transform.forward = inputMove;
			}

			Vector3 pos = this.transform.position;
			pos.y = m_originalYPos;
			this.transform.position = pos;

		}

		void Update()
		{

			if (Input.GetButton("Submit"))
			{
				this.TryFire();
			}

		}
		
	}

}
