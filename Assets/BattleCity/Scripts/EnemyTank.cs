using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BattleCity
{
	
	public class EnemyTank : Tank
	{
		Vector2 m_currentPos;
		Vector2 m_currentDir;
		Vector2 m_targetPos;

		
		protected override void Start()
		{
			base.Start();

			// set material
			foreach(var mr in this.GetComponentsInChildren<MeshRenderer>())
			{
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
			m_targetPos = m_currentPos + m_currentDir;

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

		public void SetRotationBasedOnDir()
		{
			Vector3 dir = new Vector3(m_currentDir.x, 0f, m_currentDir.y);
			this.transform.forward = dir;
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
			return MapManager.IsInsideMap(blockPos) && null == MapManager.GetMapObjectAt(blockPos);
		}


		void FixedUpdate()
		{

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
					// pick a random available direction
					Vector2 randomAvailableDir = availableDirs[Random.Range(0, availableDirs.Length)];
					// assign this direction to tank
					m_currentDir = randomAvailableDir;
					this.SetRotationBasedOnDir();
					// update target position
					m_targetPos = m_currentPos + m_currentDir;
				}
				else
				{
					Debug.LogErrorFormat("No available dirs");
				}
			}

			// move toward target position

			if (m_targetPos != m_currentPos)
			{
				Vector2 moveDelta = m_currentDir * this.moveSpeed * Time.deltaTime;

				// check if tank will miss the target position
				float distanceToTargetPos = Vector2.Distance(this.GetReal2DPos(), m_targetPos);

				if (distanceToTargetPos <= moveDelta.magnitude)
				{
					// tank will miss target position
					// this means that tank arrived at target position and changed it's current position

					m_currentPos = m_targetPos;
					m_targetPos = m_currentPos + m_currentDir;

					// check if tank can continue moving in the same direction
					bool canContinueMoving = this.CanWalkToBlock(m_targetPos);

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
				else
				{
					// just update tank position
					this.SetReal2DPos( this.GetReal2DPos() + moveDelta );
				}

			}

		}
		
	}

}
