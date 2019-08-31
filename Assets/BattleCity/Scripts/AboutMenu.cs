using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{

	public class AboutMenu : Menu
	{
		public static AboutMenu Instance { get; private set; }

		public Button okButton;


		void Awake()
		{
			Instance = this;
		}

	    void Start()
	    {
	        okButton.onClick.AddListener(() => MenuManager.ActiveMenu = MainMenu.Instance);
	    }

	    void Update()
	    {
	        
	    }
	    
	}

}
