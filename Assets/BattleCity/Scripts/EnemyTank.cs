using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BattleCity
{
	
	public class EnemyTank : Tank
	{
		Vector2 m_currentPos;
		Vector2 m_currentDir;
		public Vector2 TargetPos => m_currentPos + m_currentDir;
		int m_numBlocksUntilRandomTurn = int.MaxValue;

		public int minNumBlocksUntilRandomTurn = 5;
		public int maxNumBlocksUntilRandomTurn = 15;

		static readonly Vector2[] s_positionsAroundFlag = new Vector2[]{new Vector2(-1, 0), new Vector2(-1, 1),
			new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0)};

		public LayerMask targetRaycastMask = Physics.AllLayers;
		public Transform raycastPosition = null;

		public bool fireRandomly = true;
		public float minIntervalToFireRandomly = 3;
		public float maxIntervalToFireRandomly = 7;
		float m_intervalToFireRandomly = 100;

		static List<EnemyTank> s_allTanks = new List<EnemyTank>();
		public static List<EnemyTank> AllTanks => s_allTanks;

		public static event System.Action<EnemyTank> onDestroyed = delegate {};

		public static bool AreAllEnemyTanksFrozen { get; set; } = false;



		protected override void OnEnable()
		{
			base.OnEnable();
			s_allTanks.Add(this);
		}

		protected override void OnDisable()
		{
			s_allTanks.Remove(this);
			base.OnDisable();
			onDestroyed(this);
		}

		protected override void OnKilled(Bullet bullet)
		{
			MapManager.CurrentScore ++;
		}

		protected override void Start()
		{
			base.Start();

			// set material
			foreach(var mr in this.GetComponentsInChildren<MeshRenderer>())
			{
				if (this.healthBar != null && mr.transform.parent.gameObject == this.healthBar.gameObject)
					continue;
				mr.sharedMaterial = MapManager.Instance.enemyTankMaterial;
			}

			Debug.LogFormat("EnemyTank.Start()");

			// set random rotation
			m_currentDir = GetRandomDir();
			this.SetRotationBasedOnDir();

			// set position
			m_currentPos = new Vector2(Mathf.Round(this.GetReal2DPos().x), Mathf.Round(this.GetReal2DPos().y));
			this.SetReal2DPos(m_currentPos);	// just in case

			// set target position
		//	m_targetPos = m_currentPos + m_currentDir;

			m_numBlocksUntilRandomTurn = Random.Range(this.minNumBlocksUntilRandomTurn, this.maxNumBlocksUntilRandomTurn + 1);

			m_intervalToFireRandomly = Random.Range(this.minIntervalToFireRandomly, this.maxIntervalToFireRandomly);

		}

		public static Vector2 GetRandomDir()
		{
			Vector2 dir = Random.insideUnitCircle;

			if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
				dir.y = 0f;
			else
				dir.x = 0f;

			dir.Normalize();

			return dir;
		}

		public static Vector2[] GetAllDirs()
		{
			return new Vector2[]{new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1)};
		}

		public Vector2[] GetAllDirsExceptCurrent()
		{
			return GetAllDirs().Except(new Vector2[]{m_currentDir}).ToArray();
		}

		public Vector2[] GetSideDirs()
		{
			if (m_currentDir.x != 0)	// moving along x axis
			{
				return new Vector2[]{new Vector2(0, -1), new Vector2(0, 1)};
			}
			else
			{
				return new Vector2[]{new Vector2(-1, 0), new Vector2(1, 0)};
			}
		}

		public Vector2[] GetAvailableDirs(Vector2[] dirs)
		{
			return dirs.Where(dir => this.CanWalkToBlock(m_currentPos + dir)).ToArray();
		}

		public void SetRotationBasedOnDir()
		{
			Vector3 dir = new Vector3(m_currentDir.x, 0f, m_currentDir.y);
			this.transform.forward = dir;
		}

		public Vector3 ConvertTo3D(Vector2 pos)
		{
			return new Vector3(pos.x, this.transform.position.y, pos.y);
		}

		public Vector2 GetReal2DPos()
		{
			return new Vector2(this.transform.position.x, this.transform.position.z);
		}

		public void SetReal2DPos(Vector2 pos)
		{
			this.transform.position = new Vector3(pos.x, this.transform.position.y, pos.y);
		}

		public bool CanWalkToBlock(Vector2 blockPos)
		{
			if (! MapManager.IsInsideMap(blockPos))
				return false;
			
			var mapObj = MapManager.GetMapObjectAt(blockPos);
			if (mapObj != null && !mapObj.IsPassable)
				return false;
			
			return ! IsAnyTankAtBlock(blockPos, this);
			//return true;
		}

		public static bool IsAnyTankAtBlock(Vector2 blockPos, Tank tankToIgnore)
		{
			foreach (var tank in EnemyTank.AllTanks)
			{
				if (tank == tankToIgnore)
					continue;
				if (tank.GetApproximatePos() == blockPos)
					return true;
			}

			// check player's tank
			var playerTank = PlayerTank.Instance;
			if (playerTank != null && playerTank != tankToIgnore)
			{
				if (playerTank.GetApproximatePos() == blockPos)
					return true;
			}

			return false;
		}

		public IEnumerable<GameObject> GetTargetsForShooting()
		{
			if (PlayerTank.Instance != null)
			{
				// Vector3 pos = PlayerTank.Instance.transform.position;
				// yield return new Vector2(pos.x, pos.z);
				yield return PlayerTank.Instance.gameObject;
			}

			if (Flag.Instance != null)
			{
				yield return Flag.Instance.gameObject;

				// objects around flag
				Vector2 flagPos = Flag.Instance.Position;
				foreach (Vector2 posOffset in s_positionsAroundFlag)
				{
					Vector2 pos = flagPos + posOffset;
					if (MapManager.IsInsideMap(pos))
					{
						var mapObj = MapManager.GetMapObjectAt(pos);
						if (mapObj != null)
							yield return mapObj.gameObject;
					}
				}
			}
			
			// player tank's bullets
			// foreach (Bullet bullet in Bullet.AllBullets.Where(b => b.TankShooter != null && b.TankShooter == PlayerTank.Instance))
			// {
			// 	yield return bullet.gameObject;
			// }

		}

		public GameObject GetVisibleObject()
		{
			// if (m_currentDir.x != 0)
			// {
			// 	// looking right/left

			// 	// position.y must be the same
			// 	if (pos.y != m_currentPos.y)
			// 		return false;
				
			// 	// go through map
			// 	for (float x = m_currentPos.x + Mathf.Sign(m_currentDir.x); x < MapManager.MapWidth && x >= 0 ; x += Mathf.Sign(m_currentDir.x))
			// 	{
			// 		var mapObj = MapManager.GetMapObjectAt(x, m_currentPos.y);

			// 	}

			// }

			
			RaycastHit hit;
		//	float distance = Vector3.Distance(this.transform.position, new Vector3(pos.x, this.transform.position.y, pos.y));
			if (Physics.Raycast(this.raycastPosition.position, this.raycastPosition.forward, out hit, 1000f, this.targetRaycastMask))
			{
				return hit.transform.gameObject;
			}

			return null;
		}

		public GameObject GetVisibleTarget()
		{
			var go = this.GetVisibleObject();
			if (go != null && this.GetTargetsForShooting().Contains(go))
				return go;
			return null;
		}

		public bool IsAnyTargetVisible()
		{
			return this.GetVisibleTarget() != null;
		}


		void Update()
		{

			if (MapManager.IsGameOver)
				return;

			// fire bullet at target
			if (this.CanFire && ! AreAllEnemyTanksFrozen)
			{
				if (this.IsAnyTargetVisible())
					this.TryFire();
			}

			// fire bullets randomly
			if (this.fireRandomly && this.CanFire && ! AreAllEnemyTanksFrozen)
			{
				if (this.TimeSinceFired >= m_intervalToFireRandomly)
				{
					// we should fire
					
					GameObject visibleObject = this.GetVisibleObject();
					if (visibleObject != null && (this.GetTargetsForShooting().Contains(visibleObject) || visibleObject.GetComponent<Brick>() != null))
					{
						this.TryFire();

						// assign new interval
						m_intervalToFireRandomly = Random.Range(this.minIntervalToFireRandomly, this.maxIntervalToFireRandomly);

					}
					
				}
			}

		}

		void FixedUpdate()
		{
			if (AreAllEnemyTanksFrozen)
				return;

			if (MapManager.IsGameOver)
				return;

			// check if block in front of the tank is taken
			Vector2 nextBlockPos = m_currentPos + m_currentDir;
			bool isNextBlockAvailable = this.CanWalkToBlock(nextBlockPos);

			// if it is, get a new random direction which will unblock the tank
			if (!isNextBlockAvailable)
			{
				Vector2[] dirs = this.GetAllDirsExceptCurrent();
				// find available directions - those are direction whose first block is available
				Vector2[] availableDirs = dirs.Where(dir => this.CanWalkToBlock(m_currentPos + dir)).ToArray();
				if (availableDirs.Length > 0)
				{
					if (! this.IsAnyTargetVisible())	// only change direction if no target is visible
					{
						// pick a random available direction
						Vector2 randomAvailableDir = availableDirs[Random.Range(0, availableDirs.Length)];
						// assign this direction to tank
						m_currentDir = randomAvailableDir;
						this.SetRotationBasedOnDir();
						// update target position
					//	m_targetPos = m_currentPos + m_currentDir;
					}
					else
					{
					//	Debug.LogWarningFormat("skipped direction changing");
					}
				}
				else
				{
				//	Debug.LogErrorFormat("No available dirs");
				}
			}

			// move toward target position

			if (this.CanWalkToBlock(this.TargetPos))
			{
				Vector2 moveDelta = (this.TargetPos - this.GetReal2DPos()).normalized * this.moveSpeed * Time.deltaTime;

				// check if tank will miss the target position
				float distanceToTargetPos = Vector2.Distance(this.GetReal2DPos(), this.TargetPos);

				if (moveDelta.sqrMagnitude < float.Epsilon || distanceToTargetPos <= moveDelta.magnitude)
				{
					// tank will miss target position
					// this means that tank arrived at target position and changed it's current position

					this.OnTankChangedBlock(moveDelta);

				}
				else
				{
					// just update tank position
					this.SetReal2DPos( this.GetReal2DPos() + moveDelta );
				}

			}

		}

		void OnTankChangedBlock(Vector2 moveDelta)
		{

			m_currentPos = this.TargetPos;
		//	m_targetPos = m_currentPos + m_currentDir;

			m_numBlocksUntilRandomTurn --;

			if (m_numBlocksUntilRandomTurn <= 0)
			{
				// try to make a random turn to side
				Vector2[] availableDirs = this.GetAvailableDirs(this.GetSideDirs());
				if (availableDirs.Length > 0 && ! this.IsAnyTargetVisible())
				{
					// we can turn

					// pick a new direction
					Vector2 dir = availableDirs[Random.Range(0, availableDirs.Length)];
					m_currentDir = dir;
					this.SetRotationBasedOnDir();

					// set position at center of block
					this.SetReal2DPos(m_currentPos);

					// generate new number of blocks to wait until a new try
					m_numBlocksUntilRandomTurn = Random.Range(this.minNumBlocksUntilRandomTurn, this.maxNumBlocksUntilRandomTurn + 1);

					// return from function, so that we don't move the tank below
					return;
				}
			}

			// check if tank can continue moving in the same direction
			bool canContinueMoving = this.CanWalkToBlock(this.TargetPos);

			if (canContinueMoving)
			{
				// just update real position according to movement
				this.SetReal2DPos( this.GetReal2DPos() + moveDelta );
			}
			else
			{
				// we will decide where to go in next frame

				// update real position to be at the center of block
				this.SetReal2DPos(m_currentPos);
			}
			
		}


		void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			var go = this.GetVisibleObject();
			if (go != null)
				Gizmos.DrawCube(go.transform.position, Vector3.one);
		}

		void OnDrawGizmos()
		{

			// draw current pos
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(ConvertTo3D(m_currentPos), 0.45f);

			// draw occupying block
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(new Vector3(this.GetApproximatePos().x, this.transform.position.y, this.GetApproximatePos().y), Vector3.one);

			// draw target position
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(new Vector3(this.TargetPos.x, this.transform.position.y, this.TargetPos.y), 0.3f);

			// draw available nearby positions
			Gizmos.color = Color.blue;
			foreach(Vector2 dir in this.GetAvailableDirs(GetAllDirs()))
			{
				Gizmos.DrawWireCube(ConvertTo3D(m_currentPos + dir), Vector3.one * 0.9f);
			}

		}
		
	}

}
