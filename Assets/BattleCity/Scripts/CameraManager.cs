using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
	
	public class CameraManager : MonoBehaviour
	{
		public static CameraManager Instance { get; private set; }

		Vector3 m_originalCamPos = Vector3.zero;
		Vector3 m_originalCamEulers = Vector3.zero;
		float m_orignalCamFieldOfView = 0f;

		public float ortoZPosition = 0f;
		public Vector3 ortoEulers = Vector3.zero;

		public float birdHeight = 12f;
		public float birdZPosition = 0f;
		public Vector3 birdEulers = Vector3.zero;
		public float birdFieldOfView = 60f;



		void Awake()
		{
			Instance = this;

			var cam = Camera.main;
			m_originalCamPos = cam.transform.position;
			m_originalCamEulers = cam.transform.eulerAngles;
			m_orignalCamFieldOfView = cam.fieldOfView;
		}
		
		void Start()
		{
			
		}

		void Update()
		{

			if (MapManager.IsMapOpened)
			{
				if (Input.GetKeyDown(KeyCode.Alpha1))
					SwitchToBirdView();
				else if (Input.GetKeyDown(KeyCode.Alpha2))
					SwitchTo2DView();
				else if (Input.GetKeyDown(KeyCode.Alpha3))
					SwitchToSideView();
			}

		}

		public void SwitchToSideView()
		{
			var cam = Camera.main;

			cam.orthographic = false;

			Vector3 pos = m_originalCamPos;
			pos.x = MapManager.MapWidth / 2f;
			cam.transform.position = pos;

			cam.transform.eulerAngles = m_originalCamEulers;

			cam.fieldOfView = m_orignalCamFieldOfView;
		}

		public void SwitchToBirdView()
		{
			var cam = Camera.main;

			cam.orthographic = false;

			Vector3 pos = m_originalCamPos;
			pos.x = MapManager.MapWidth / 2f;
			pos.y = this.birdHeight;
			pos.z = this.birdZPosition;
			cam.transform.position = pos;

			cam.transform.eulerAngles = this.birdEulers;

			cam.fieldOfView = this.birdFieldOfView;
		}

		public void SwitchTo2DView()
		{
			var cam = Camera.main;

			cam.orthographic = true;

			Vector3 pos = m_originalCamPos;
			pos.x = MapManager.MapWidth / 2f;
			pos.z = this.ortoZPosition;
			cam.transform.position = pos;

			cam.transform.eulerAngles = this.ortoEulers;

			cam.fieldOfView = m_orignalCamFieldOfView;
		}

	}

}
