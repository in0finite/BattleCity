using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{

	public class MenuManager : MonoBehaviour
	{

		static Menu m_activeMenu;
		public static Menu ActiveMenu
		{
			get => m_activeMenu;
			set
			{
				if (m_activeMenu != null)
					m_activeMenu.gameObject.SetActive(false);
				m_activeMenu = value;
				m_activeMenu.gameObject.SetActive(true);
				m_activeMenu.OnBecameActive();
			}
		}


	    void Start()
	    {
	        
	    }

	    void Update()
	    {
	        
	    }
	    
	}

}
