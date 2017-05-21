using UnityEngine;
using System.Collections;

namespace Softdrink{

	//TODO: Modify to take StandardInputs
	//TODO: Create new InputMaps intelligently based on
	// number of players and connected devices. EG:
	// - 1 Player, 1 Joystick - bindings on J1A1, J1A2, etc
	// - 2 Players, 2 Joysticks - P1 bindings defaulted to J1A1, etc
	// and P2 bindings defaulted to J2A1, etc
	//TODO: Integration with InputListener and InputBinder
	//TODO: InputBinder has a generic modal dialog or method
	// that basically asks "press a key to bind to X action"
	// This can be either standalone for modifying one key
	// or used in succession for a 'quick bind'
	//TODO: InputBinder has two main methods:
	// One takes an InputMap as a parameter and returns a modified
	// version of that InputMap. This is useful for rebinding one
	// key at a time.
	// The other takes no parameters and returns a new InputMap - 
	// this is a Quick Bind operation.
	//TODO: Generic standardized Inputs for Title screen, and other
	// situations where the Player may not have bound all his inputs

	// Class for storing key->action mappings		// HEAVILY DEBUG RN
	[System.Serializable]
	public class KeyMap{

		[SerializeField]
		[TooltipAttribute("The display name for this Keymap")]
		private string Name = "Player";

		[SerializeField]
		//[ReadOnlyAttribute]
		[TooltipAttribute("Which Player is this KeyMap associated with? \nA value of 0 or less means that it is common to all Players, as a system-wide mapping. This is desirable in certain cases, such as common menus.")]
		private int associatedPlayer = 0;

		[HeaderAttribute("Directions")]

		[TooltipAttribute("What Key(s) are bound to the logical Up action?")]
		public KeyCode[] upKeys = new KeyCode[1];

		[TooltipAttribute("What Key(s) are bound to the logical Down action?")]
		public KeyCode[] downKeys = new KeyCode[1];

		[TooltipAttribute("What Key(s) are bound to the logical Left action?")]
		public KeyCode[] leftKeys = new KeyCode[1];

		[TooltipAttribute("What Key(s) are bound to the logical Right action?")]
		public KeyCode[] rightKeys = new KeyCode[1];

		[HeaderAttribute("Actions")]

		[TooltipAttribute("What Key(s) are bound to the logical 'A Button'?")]
		public KeyCode[] aButtonKeys = new KeyCode[1];

		[TooltipAttribute("What Key(s) are bound to the logical 'B Button'?")]
		public KeyCode[] bButtonKeys = new KeyCode[1];

		[TooltipAttribute("What Key(s) are bound to the logical 'X Button'?")]
		public KeyCode[] xButtonKeys = new KeyCode[1];

		[TooltipAttribute("What Key(s) are bound to the logical 'Y Button'?")]
		public KeyCode[] yButtonKeys = new KeyCode[1];

		

		// Constructor to set some defaults
		KeyMap(){
			// Directions
			upKeys[0] = KeyCode.W;
			downKeys[0] = KeyCode.S;
			leftKeys[0] = KeyCode.A;
			rightKeys[0] = KeyCode.D;

			// Actions
			aButtonKeys[0] = KeyCode.J;
			bButtonKeys[0] = KeyCode.K;
			xButtonKeys[0] = KeyCode.I;
			yButtonKeys[0] = KeyCode.L;

		}

		public KeyMap(KeyMap input){
			Name = input.getName();
			associatedPlayer = input.getAssociatedPlayer();

			upKeys = input.getKeys(input.upKeys);
			downKeys = input.getKeys(input.downKeys);
			leftKeys = input.getKeys(input.leftKeys);
			rightKeys = input.getKeys(input.rightKeys);

			aButtonKeys = input.getKeys(input.aButtonKeys);
			bButtonKeys = input.getKeys(input.bButtonKeys);
			xButtonKeys = input.getKeys(input.xButtonKeys);
			yButtonKeys = input.getKeys(input.yButtonKeys);
		}

		// Evaluate a given Key

		public bool EvaluateUp(){
			for(int i = 0; i < upKeys.Length; i++){
				if(Input.GetKey(upKeys[i])) return true;
			}
			return false;
		}

		public bool EvaluateDown(){
			for(int i = 0; i < downKeys.Length; i++){
				if(Input.GetKey(downKeys[i])) return true;
			}
			return false;
		}

		public bool EvaluateLeft(){
			for(int i = 0; i < leftKeys.Length; i++){
				if(Input.GetKey(leftKeys[i])) return true;
			}
			return false;
		}

		public bool EvaluateRight(){
			for(int i = 0; i < rightKeys.Length; i++){
				if(Input.GetKey(rightKeys[i])) return true;
			}
			return false;
		}

		public bool EvaluateA(){
			for(int i = 0; i < aButtonKeys.Length; i++){
				if(Input.GetKey(aButtonKeys[i])) return true;
			}
			return false;
		}

		public bool EvaluateB(){
			for(int i = 0; i < bButtonKeys.Length; i++){
				if(Input.GetKey(bButtonKeys[i])) return true;
			}
			return false;
		}

		public bool EvaluateX(){
			for(int i = 0; i < xButtonKeys.Length; i++){
				if(Input.GetKey(xButtonKeys[i])) return true;
			}
			return false;
		}

		public bool EvaluateY(){
			for(int i = 0; i < yButtonKeys.Length; i++){
				if(Input.GetKey(yButtonKeys[i])) return true;
			}
			return false;
		}

		// GETTERS

		public string getName(){
			return Name;
		}

		public KeyCode[] getKeys(KeyCode[] input){
			KeyCode[] output = new KeyCode[input.Length];
			for(int i = 0; i < output.Length; i++){
				output[i] = input[i];
			}
			return output;
		}

		public int getAssociatedPlayer(){
			return associatedPlayer;
		}

	}

	[System.Serializable]
	public class ActionOutput{
		[SerializeField]
		[ReadOnlyAttribute]
		private string Name = "";

		[ReadOnlyAttribute]
		public int associatedPlayer = 0;

		[HideInInspector]
		public KeyMap targetKeymap = null;

		[ReadOnlyAttribute]
		public bool up = false;
		[ReadOnlyAttribute]
		public bool down = false;
		[ReadOnlyAttribute]
		public bool left = false;
		[ReadOnlyAttribute]
		public bool right = false;

		[ReadOnlyAttribute]
		public bool a = false;
		[ReadOnlyAttribute]
		public bool b = false;
		[ReadOnlyAttribute]
		public bool x = false;
		[ReadOnlyAttribute]
		public bool y = false; 

		// CHECK ACTIONS
		public void CheckActions(){
			up = targetKeymap.EvaluateUp();
			down = targetKeymap.EvaluateDown();
			left = targetKeymap.EvaluateLeft();
			right = targetKeymap.EvaluateRight();

			a = targetKeymap.EvaluateA();
			b = targetKeymap.EvaluateB();
			x = targetKeymap.EvaluateX();
			y = targetKeymap.EvaluateY();
		}

		// SETTERS

		public void setName(string nameIn){
			Name = nameIn + " Action Output";
			nameIn = Name;		// Get rid of annoying editor popups about "Name" being unused
		}

		public void setAssociatedPlayer(int input){
			associatedPlayer = input;
		}
	}

	// Input_Manager is a Singleton that handles all input for all Players in the game
	// It polls for inputs and uses KeyMaps to bind them to actions
	[AddComponentMenu("Scripts/Global Managers/Input Manager")]
	public class Input_Manager : MonoBehaviour {

		// Singleton instance
		public static Input_Manager Instance = null;

		[SerializeField]
		[TooltipAttribute("The KeyMaps used by the Input_Manager to bind key input into game actions.")]
		public KeyMap[] maps = new KeyMap[1];

		[SerializeField]
		//[ReadOnlyAttribute]
		[TooltipAttribute("The output Actions procced for a given Player")]
		public ActionOutput[] output = new ActionOutput[1];


		// Assign Singleton instance and initialize input stuff
		void Awake(){

			// If the Instance doesn't already exist
			if(Instance == null){
				// If the instance doesn't already exist, set it to this
				Instance = this;
			}else if(Instance != this){
				// If an instance already exists that isn't this, destroy this instance and log what happened
				Destroy(gameObject);
				Debug.LogError("ERROR! The Input Manager encountered another instance of Input_Manager; it destroyed itself rather than overwrite the existing instance.", this);
			}

		}

		//void Update(){}

		// Make sure the input and output are logically similar
		public void OnValidate(){

			// Make sure there are the same number of input and output maps
			if(output.Length != maps.Length){
				output = new ActionOutput[maps.Length];
				for(int i = 0; i < output.Length; i++){
					output[i] = new ActionOutput();
					
				}
			}

			// Set names of Outputs to match input maps
			for(int i = 0; i < output.Length; i++){
				output[i].setName(maps[i].getName());
				output[i].setAssociatedPlayer(maps[i].getAssociatedPlayer());
				output[i].targetKeymap = new KeyMap(maps[i]);
			}
		}

		// GETTERS
		public static ActionOutput GetOutputFromPlayerID(int IDin){
			for(int i = 0; i < Instance.output.Length; i++){
				if(Instance.output[i].associatedPlayer == IDin) return Instance.output[i];
			}
			return Instance.output[0];
		}

		
	}
}