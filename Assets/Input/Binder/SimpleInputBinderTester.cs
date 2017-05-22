using UnityEngine;
using System.Collections;

namespace Softdrink{
	[AddComponentMenu("Scripts/Input/Testers/Simple Input Binder Tester")]
	public class SimpleInputBinderTester : MonoBehaviour {

		[TooltipAttribute("Which Player Keymap should be rebound?")]
		public int playerKeymapID = 1;

		private KeyMap map;

		void GetMap(){
			map = Input_Manager.GetMapFromPlayerID(playerKeymapID);
		}

		[ContextMenu("TestQuickBind")]
		public void TestQuickBind(){
			if(map == null) GetMap();
			InputBinder.BeginQuickBind(map);
		}


	}
}
