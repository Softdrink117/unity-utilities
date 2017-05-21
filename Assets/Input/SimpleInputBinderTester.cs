using UnityEngine;
using System.Collections;

namespace Softdrink{
	[AddComponentMenu("Scripts/Input/Testers/Simple Input Binder Tester")]
	public class SimpleInputBinderTester : MonoBehaviour {

		public InputBinder binder = null;

		public int playerKeymapID = 1;

		private KeyMap map;

		void Awake(){

		}

		void GetMap(){
			map = Input_Manager.GetMapFromPlayerID(playerKeymapID);
		}

		[ContextMenu("TestQuickBind")]
		public void TestQuickBind(){
			if(map == null) GetMap();
			binder.BeginQuickBind(map);
		}


	}
}
