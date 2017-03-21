using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Softdrink{
	[AddComponentMenu("Scripts/Input/Key Listener")]
	public class InputKeyListener : MonoBehaviour {

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

		}

		void Init(){
			validKeyCodes=(KeyCode[])System.Enum.GetValues(typeof(KeyCode));
		}

		void Update () {
			if(!listening) return;

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
			
			if(keyOut != KeyCode.None) Debug.Log( keyOut, this);
		}

		private KeyCode keyTemp = KeyCode.None;
		private KeyCode keyOut = KeyCode.None;
		// Functions for detecting keys pressed, held, or released -----------
		KeyCode DetectKeyDown(){
			if(validKeyCodes == null) Init();

			for(int i = 0; i < validKeyCodes.Length; i++){
				keyTemp = validKeyCodes[i];
				if(Input.GetKeyDown(keyTemp)){
					return keyTemp;
				}
			}

			return KeyCode.None;
		}

		KeyCode DetectKeyUp(){
			if(validKeyCodes == null) Init();

			for(int i = 0; i < validKeyCodes.Length; i++){
				keyTemp = validKeyCodes[i];
				if(Input.GetKeyUp(keyTemp)){
					return keyTemp;
				}
			}

			return KeyCode.None;
		}

		KeyCode DetectKeyHeld(){
			if(validKeyCodes == null) Init();

			for(int i = 0; i < validKeyCodes.Length; i++){
				keyTemp = validKeyCodes[i];
				if(Input.GetKey(keyTemp)){
					return keyTemp;
				}
			}

			return KeyCode.None;
		}

	}
}
