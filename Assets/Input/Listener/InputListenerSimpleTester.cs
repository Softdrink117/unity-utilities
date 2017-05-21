using UnityEngine;
using System.Collections;

namespace Softdrink{
	[AddComponentMenu("Scripts/Input/Testers/Simple Input Listener Tester")]
	public class InputListenerSimpleTester : MonoBehaviour {

		private enum InputListenerTesterMode{
			InputDown,
			InputHeld,
			InputUp,
			InputValue,
			All,
		}

		[SerializeField]
		[TooltipAttribute("Should the Tester check InputDown, InputHeld, InputUp, InputValue, or all of the above? \nNote that All may not work properly under some conditions.")]
		private InputListenerTesterMode mode = InputListenerTesterMode.InputHeld;

		[TooltipAttribute("The particular input being tested.")]
		public EInput test = new EInput(KeyCode.None);

		void Start () {
		
		}
		
		string msg = "";
		void Update () {
			if(!test.isDefined){
				test = InputListener.DetectInput();
				if(test.isDefined){
					InputListener.setListening(false);
					test.Print();
				}
			}else{
				//test.Print();
				if(mode == InputListenerTesterMode.InputDown){
					msg = "Down: " + test.GetInputDown();
					Debug.Log(msg, this);
				}
				if(mode == InputListenerTesterMode.InputHeld){
					msg = "Held: " + test.GetInput();
					Debug.Log(msg, this);
				}
				if(mode == InputListenerTesterMode.InputUp){
					msg = "Up: " + test.GetInputUp();
					Debug.Log(msg, this);
				}
				if(mode == InputListenerTesterMode.InputValue){
					msg = "Value: " + test.GetInputValue();
					Debug.Log(msg, this);
				}
				if(mode == InputListenerTesterMode.All){
					msg = "Down: " + test.GetInputDown();
					msg += "\t| Held: " + test.GetInput();
					msg += "\t| Up: " + test.GetInputUp();
					msg += "\t| Value: " + test.GetInputValue();
					Debug.Log(msg, this);
				}
			}


		}
	}
}