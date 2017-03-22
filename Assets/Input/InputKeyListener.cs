using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Softdrink{
	[AddComponentMenu("Scripts/Input/Key Listener")]
	public class InputKeyListener : MonoBehaviour {

		// Enum for determining how keys are checked during listening
		private enum KeyListeningMode{
			KeyDown,
			KeyUp,
			KeyHeld,
		}

		[SerializeField]
		[TooltipAttribute("Is this KeyListener actively listening for inputs? \nIt is expensive to poll for any input, so only enable this when needed (EG: when rebinding controls)?")]
		private bool listening = true;

		[SerializeField]
		[TooltipAttribute("The current Mode of the Listener. \nKeyDown only checks for Key Down events; KeyUp only checks for Key Up events; KeyHeld returns every frame a Key is pressed.")]
		private KeyListeningMode mode = KeyListeningMode.KeyDown;


		private KeyCode[] validKeyCodes;
		void Awake(){
			Init();
			//Debug.Log(DetectConnectedControllers(), this);
		}

		public void Init(){
			validKeyCodes=(KeyCode[])System.Enum.GetValues(typeof(KeyCode));
		}

		private KeyCode keyOut = KeyCode.None;
		void Update () {
			if(!listening) return;

			keyOut = DetectKey();
			
			if(keyOut != KeyCode.None) Debug.Log( keyOut, this);
		}

		public KeyCode DetectKey(){
			switch(mode){
				case KeyListeningMode.KeyDown:
					keyOut = DetectKeyDown();
					break;
				case KeyListeningMode.KeyUp:
					keyOut = DetectKeyUp();
					break;
				case KeyListeningMode.KeyHeld:
					keyOut = DetectKeyHeld();
					break;
				default:
					keyOut = DetectKeyDown();
					break;
			}

			return keyOut;
		}

		
		// Functions for detecting keys pressed, held, or released -----------

		private KeyCode keyTemp = KeyCode.None;
		private KeyCode keyOutTemp = KeyCode.None;
		public KeyCode DetectKeyDown(){
			if(validKeyCodes == null) Init();

			keyTemp = KeyCode.None;
			keyOutTemp = KeyCode.None;

			// Get the LAST key if there is more than one result -
			// do not prematurely return! This avoids returning
			// a generic 'JoystickButtonX' instead of a specific
			// 'JoystickXButtonY'
			for(int i = 0; i < validKeyCodes.Length; i++){
				keyTemp = validKeyCodes[i];
				if(Input.GetKeyDown(keyTemp)){
					keyOutTemp = keyTemp;
				}
			}

			return keyOutTemp;
		}

		public KeyCode DetectKeyUp(){
			if(validKeyCodes == null) Init();

			keyTemp = KeyCode.None;
			keyOutTemp = KeyCode.None;

			// Get the LAST key if there is more than one result -
			// do not prematurely return! This avoids returning
			// a generic 'JoystickButtonX' instead of a specific
			// 'JoystickXButtonY'
			for(int i = 0; i < validKeyCodes.Length; i++){
				keyTemp = validKeyCodes[i];
				if(Input.GetKeyUp(keyTemp)){
					keyOutTemp = keyTemp;
				}
			}

			return keyOutTemp;
		}

		public KeyCode DetectKeyHeld(){
			if(validKeyCodes == null) Init();

			keyTemp = KeyCode.None;
			keyOutTemp = KeyCode.None;

			// Get the LAST key if there is more than one result -
			// do not prematurely return! This avoids returning
			// a generic 'JoystickButtonX' instead of a specific
			// 'JoystickXButtonY'
			for(int i = 0; i < validKeyCodes.Length; i++){
				keyTemp = validKeyCodes[i];
				if(Input.GetKey(keyTemp)){
					keyOutTemp = keyTemp;
				}
			}

			return keyOutTemp;
		}

		// Functions for returning information about connected controllers ----------------
		private StringBuilder _sb;
		private string[] conNames;
		string DetectConnectedControllers(){
			_sb = new StringBuilder(1024);

			conNames = Input.GetJoystickNames();

			_sb.Append("Detected a total of ");
			_sb.Append(conNames.Length);
			_sb.Append(" controllers: \n");

			for(int i = 0; i < conNames.Length; i++){
				_sb.Append("  Controller ");
				_sb.Append(i+1);
				_sb.Append(": ");
				_sb.Append(conNames[i]);
				_sb.Append("\n");
			}

			return _sb.ToString();
		}

		// SETTERS -----
		public void setListening(bool input){
			listening = input;
		}
	}
}
