using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
	
	public class PlayerTank : Tank
	{
		
		public static PlayerTank Instance { get; private set; }

		float m_originalYPos;

		CharacterController m_cc;

		public bool HasShield { get; set; } = false;
		public GameObject shieldGameObject;
		public float shieldRotationSpeed = 60f;



		protected override void Awake()
		{
			base.Awake();
			Instance = this;
			m_cc = this.GetComponent<CharacterController>();
			m_originalYPos = this.transform.position.y;
		}
		
		protected override void Start()
		{
			base.Start();

			// save shield material
			Material savedShieldMaterial = this.shieldGameObject.GetComponent<Renderer>().sharedMaterial;

			// set material
			foreach(var mr in this.GetComponentsInChildren<MeshRenderer>())
			{
				mr.sharedMaterial = MapManager.Instance.playerTankMaterial;
			}

			// restore shield material
			this.shieldGameObject.GetComponent<Renderer>().sharedMaterial = savedShieldMaterial;

		}

		public override void OnCollidedWithBullet(Bullet bullet)
		{
			if (this.HasShield)
				return;
			base.OnCollidedWithBullet(bullet);
		}

		protected override void OnKilled(Bullet bullet)
		{
			MapManager.OnPlayerTankDestroyed();
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

			this.shieldGameObject.SetActive(this.HasShield);
			if (this.HasShield)
			{
				// rotate shield
				float rotateAmount = this.shieldRotationSpeed * Time.deltaTime;
				this.shieldGameObject.transform.Rotate(new Vector3(rotateAmount, rotateAmount, 0f));
			}

		}
		
	}

}
