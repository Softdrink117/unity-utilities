using UnityEngine;
using System.Collections;

namespace Softdrink{
	[AddComponentMenu("Scripts/Input/Input Binder")]
	public class InputBinder : MonoBehaviour {

		[System.Flags]
		public enum BindOperation{
			Up = 1 >> 0,
			Down = 1 >> 1,
			Left = 1 >> 2,
			Right = 1 >> 3,
			A_Button = 1 >> 4,
			B_Button = 1 >> 5,
			X_Button = 1 >> 6,
			Y_Button = 1 >> 7,
			Start_Button = 1 >> 8,
		};

		[EnumFlag("Current Bind Operation")]
		public BindOperation bindOperation = 0 >> 0;

		[ReadOnlyAttribute]
		public bool isBinding = false;

		[HideInInspector]
		public EInput eOut;

		[HideInInspector]
		public KeyMap mapOut;

		// Initialization -------

		void Awake(){

		}

		void Start(){

		}

		// Update -------

		void Update(){

		}

		// Binding functions

		[ContextMenu("BeginQuickBind")]
		public void BeginQuickBind(){

		}

		[ContextMenu("RebindUp")]
		public void RebindUp(){

		}

		[ContextMenu("RebindDown")]
		public void RebindDown(){

		}

		[ContextMenu("RebindLeft")]
		public void RebindLeft(){

		}

		[ContextMenu("RebindRight")]
		public void RebindRight(){

		}

		[ContextMenu("RebindA")]
		public void RebindA(){

		}

		[ContextMenu("RebindB")]
		public void RebindB(){

		}

		[ContextMenu("RebindX")]
		public void RebindX(){

		}

		[ContextMenu("RebindY")]
		public void RebindY(){

		}

		[ContextMenu("RebindStart")]
		public void RebindStart(){

		}

		
	}
}