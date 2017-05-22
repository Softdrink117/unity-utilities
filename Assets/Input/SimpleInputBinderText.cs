using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Softdrink{
	[AddComponentMenu("Scripts/Input/Simple Input Binder Text")]
	public class SimpleInputBinderText : MonoBehaviour {

		private string displayText = "";

		[HeaderAttribute("Display Text")]

		[SerializeField]
		[MultilineAttribute(3)]
		[TooltipAttribute("The default text to show when binding a new control.")]
		private string defaultBindOpText = "Please press the input to use for ";

		[SerializeField]
		[MultilineAttribute(3)]
		[TooltipAttribute("The text to show when in the confirmation dialog.")]
		private string confirmationText = "Please press and hold START to confirm,\nor wait ";

		[SerializeField]
		[TooltipAttribute("The text to show in the confirmation dialog after the timer.")]
		private string confirmationAppend = " seconds to revert control bindings.";

		[HeaderAttribute("Success / Failure Text")]

		[SerializeField]
		[MultilineAttribute(3)]
		[TooltipAttribute("The text to show if the Binding was successful.")]
		private string successText = "Controls successfully rebound!";
		[SerializeField]
		[MultilineAttribute(3)]
		[TooltipAttribute("The text to show if the Binding was not successful.")]
		private string failureText = "Controls were not rebound; reverted to last working configuration.";

		[HeaderAttribute("Miscellaneous")]

		[SerializeField]
		[TooltipAttribute("Show a prepend listing all currently detected controllers?")]
		private bool showConnectedControllers = true;

		private string keyMapName = "";

		private string controllerText = "";

		private Text _text;

		private bool active = false;

		// Init ------

		void Awake(){
			GetReferences();
		}

		void GetReferences(){
			_text = gameObject.GetComponent<Text>() as Text;
		}

		// Update Text -------

		void SetText(){
			_text.text = displayText;
		}

		public void Set(BindOperation input, string mapName){
			active = true;
			keyMapName = mapName;
			if(showConnectedControllers) controllerText = InputListener.DetectControllers() + "\n\n";
			else controllerText = "";
			UpdateCurrentBind(input);

		}

		public void Set(float timeIn){
			displayText = confirmationText + timeIn.ToString("F0") + confirmationAppend;
			SetText();
		}

		public void SetSuccess(){
			displayText = successText;
			SetText();
		}

		public void SetFailure(){
			displayText = failureText;
			SetText();
		}

		public void Unset(){
			displayText = "";
			if(active) SetText();
			active = false;
		}

		public void UpdateCurrentBind(BindOperation input){
			displayText = controllerText + keyMapName + "\n" + defaultBindOpText;

			if(hasFlag(input, BindOperation.Up)){
				displayText += "UP";
				SetText();
				return;
			}
			if(hasFlag(input, BindOperation.Down)){
				displayText += "DOWN";
				SetText();
				return;
			}
			if(hasFlag(input, BindOperation.Left)){
				displayText += "LEFT";
				SetText();
				return;
			}
			if(hasFlag(input, BindOperation.Right)){
				displayText += "RIGHT";
				SetText();
				return;
			}

			if(hasFlag(input, BindOperation.A_Button)){
				displayText += "A BUTTON";
				SetText();
				return;
			}
			if(hasFlag(input, BindOperation.B_Button)){
				displayText += "B BUTTON";
				SetText();
				return;
			}
			if(hasFlag(input, BindOperation.X_Button)){
				displayText += "X BUTTON";
				SetText();
				return;
			}
			if(hasFlag(input, BindOperation.Y_Button)){
				displayText += "Y BUTTON";
				SetText();
				return;
			}

			if(hasFlag(input, BindOperation.Start_Button)){
				displayText += "START";
				SetText();
				return;
			}
		}

		// Helper methods -------

		public bool hasFlag(BindOperation a, BindOperation b){
			return (a & b) != 0;
		}
		
	}
}