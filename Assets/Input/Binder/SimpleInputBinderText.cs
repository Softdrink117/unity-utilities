using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Softdrink{
	[AddComponentMenu("Scripts/Input/Simple Input Binder Text")]
	public class SimpleInputBinderText : MonoBehaviour {

		private string displayText = "";

		private KeyMap map;		// The actual map that will be sent to the InputBinder for rebinding
		private KeyCode[] buttons;
		private string[]  mapNames;

		[HeaderAttribute("Display Text")]

		[SerializeField]
		[MultilineAttribute(3)]
		[TooltipAttribute("The default text to show when Idle")]
		private string idleText = "Choose a KeyMap to rebind: ";

		[SerializeField]
		[TooltipAttribute("The KeyCode at which to start the buttons list.")]
		private KeyCode buttonStart = KeyCode.Alpha0;

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

		private bool idle = true;

		// Init ------

		void Start(){
			GetReferences();
			SetupKeyCodes();
			active = true;
			Unset();
			active = true;
			idle = true;
		}

		void GetReferences(){
			_text = gameObject.GetComponent<Text>() as Text;
		}

		void SetupKeyCodes(){
			int mapsCount = Input_Manager.GetMapsCount();
			buttons = new KeyCode[mapsCount];
			mapNames = Input_Manager.GetMapNames();
			for(int i = 0; i < mapsCount; i++){
				buttons[i] = (KeyCode)(buttonStart + i);
			}
		}

		// Update -------

		void Update(){
			if(idle){
				for(int i = 0; i < buttons.Length; i++){
					if(Input.GetKeyUp(buttons[i])){
						map = Input_Manager.GetMapFromPlayerID(i+1);
						InputBinder.BeginQuickBind(map);
					}
				}
			}
		}

		// Update Text -------

		void SetText(){
			_text.text = displayText;
		}

		string SetupIdleText(){
			string output = "\n";
			for(int i = 0; i < mapNames.Length; i++){
				output += mapNames[i] + "\t\t\t" + buttons[i].ToString() + "\n";
			}
			//Debug.Log(output);
			return output;
		}

		public void Set(BindOperation input, string mapName){
			active = true;
			keyMapName = mapName;
			if(showConnectedControllers) controllerText = InputListener.DetectControllers() + "\n\n";
			else controllerText = "";
			UpdateCurrentBind(input);
			idle = false;
		}

		public void Set(float timeIn){
			displayText = confirmationText + timeIn.ToString("F0") + confirmationAppend;
			SetText();
			idle = false;
		}

		public void SetSuccess(){
			displayText = successText;
			SetText();
			idle = false;
		}

		public void SetFailure(){
			displayText = failureText;
			SetText();
			idle = false;
		}

		public void Unset(){
			//displayText = "";
			displayText = idleText + SetupIdleText();
			if(active) SetText();
			active = false;
			idle = true;
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