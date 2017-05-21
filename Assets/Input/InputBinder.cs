using UnityEngine;
using System.Collections;

namespace Softdrink{
	[System.Flags]
	public enum BindOperation{
		Up = 1 << 0,
		Down = 1 << 1,
		Left = 1 << 2,
		Right = 1 << 3,
		A_Button = 1 << 4,
		B_Button = 1 << 5,
		X_Button = 1 << 6,
		Y_Button = 1 << 7,
		Start_Button = 1 << 8,
	};

	[AddComponentMenu("Scripts/Input/Input Binder")]
	public class InputBinder : MonoBehaviour {

		// Stores what, if any, bind operations are currently ongoing
		// If this is not 0 << 0, some operation is happening
		// Multiple values can be queued simultaneously - this is what allows for
		// the Quick Bind to occur!
		[EnumFlag("Current Bind Operation")]
		public BindOperation bindOperation = 0 << 0;

		[ReadOnlyAttribute]
		public bool isBinding = false;

		[HeaderAttribute("Output Text")]

		[SerializeField]
		[TooltipAttribute("Use Simple Input Binder Text")]
		private bool useBinderText = false;

		[SerializeField]
		[TooltipAttribute("The Simple Input Binder Text to use, if enabled.")]
		private SimpleInputBinderText simpleText = null;

		[HeaderAttribute("Miscellaneous")]

		[SerializeField]
		[TooltipAttribute("Enables debug print statements to console.")]
		private bool debugPrint = false;

		// Use references only! This allows for instant change of the existing map

		private string mapName = "Sup Bro";

		// Initialization -------

		void Awake(){
			bindOperation = 0 << 0;
		}

		void Start(){

		}

		// Update -------

		void Update(){
			if(bindOperation != 0 << 0){
				if(hasFlag(bindOperation, BindOperation.Up)){
					SetBinderText();
					bindOperation = unsetFlag(bindOperation, BindOperation.Up);
					return;
				}
				if(hasFlag(bindOperation, BindOperation.Down)){
					SetBinderText();
					bindOperation = unsetFlag(bindOperation, BindOperation.Down);
					return;
				}
				if(hasFlag(bindOperation, BindOperation.Left)){
					SetBinderText();
					bindOperation = unsetFlag(bindOperation, BindOperation.Left);
					return;
				}
				if(hasFlag(bindOperation, BindOperation.Right)){
					SetBinderText();
					bindOperation = unsetFlag(bindOperation, BindOperation.Right);
					return;
				}

				if(hasFlag(bindOperation, BindOperation.A_Button)){
					SetBinderText();
					bindOperation = unsetFlag(bindOperation, BindOperation.A_Button);
					return;
				}
				if(hasFlag(bindOperation, BindOperation.B_Button)){
					SetBinderText();
					bindOperation = unsetFlag(bindOperation, BindOperation.B_Button);
					return;
				}
				if(hasFlag(bindOperation, BindOperation.X_Button)){
					SetBinderText();
					bindOperation = unsetFlag(bindOperation, BindOperation.X_Button);
					return;
				}
				if(hasFlag(bindOperation, BindOperation.Y_Button)){
					SetBinderText();
					bindOperation = unsetFlag(bindOperation, BindOperation.Y_Button);
					return;
				}

				if(hasFlag(bindOperation, BindOperation.Start_Button)){
					SetBinderText();
					bindOperation = unsetFlag(bindOperation, BindOperation.Start_Button);
					return;
				}

			}else{
				if(useBinderText) simpleText.Unset();
			}
		}

		void SetBinderText(){
			if(useBinderText){
				simpleText.Set(bindOperation, mapName);
			}
		}

		// Binding functions -------

		[ContextMenu("BeginQuickBind")]
		public void BeginQuickBind(){
			isBinding = true;
			bindOperation = BindOperation.Up | BindOperation.Down | BindOperation.Left | BindOperation.Right;
			bindOperation = bindOperation | BindOperation.A_Button | BindOperation.B_Button | BindOperation.X_Button | BindOperation.Y_Button;
			bindOperation = bindOperation | BindOperation.Start_Button;
		}

		[ContextMenu("RebindUp")]
		public void RebindUp(){
			isBinding = true;
			bindOperation = BindOperation.Up;
		}

		[ContextMenu("RebindDown")]
		public void RebindDown(){
			isBinding = true;
			bindOperation = BindOperation.Down;
		}

		[ContextMenu("RebindLeft")]
		public void RebindLeft(){
			isBinding = true;
			bindOperation = BindOperation.Left;
		}

		[ContextMenu("RebindRight")]
		public void RebindRight(){
			isBinding = true;
			bindOperation = BindOperation.Right;
		}

		[ContextMenu("RebindA")]
		public void RebindA(){
			isBinding = true;
			bindOperation = BindOperation.A_Button;
		}

		[ContextMenu("RebindB")]
		public void RebindB(){
			isBinding = true;
			bindOperation = BindOperation.B_Button;
		}

		[ContextMenu("RebindX")]
		public void RebindX(){
			isBinding = true;
			bindOperation = BindOperation.X_Button;
		}

		[ContextMenu("RebindY")]
		public void RebindY(){
			isBinding = true;
			bindOperation = BindOperation.Y_Button;
		}

		[ContextMenu("RebindStart")]
		public void RebindStart(){
			isBinding = true;
			bindOperation = BindOperation.Start_Button;
		}

		// Misc / Testing functions -------

		[ContextMenu("UnsetText")]
		public void UnsetText(){
			if(useBinderText) simpleText.Unset();
		}

		// Helper methods -------
		
		public bool hasFlag(BindOperation a, BindOperation b){
			return (a & b) != 0;
		}

		public BindOperation unsetFlag(BindOperation a, BindOperation b){
			return a & ~b;
		}
	}
}