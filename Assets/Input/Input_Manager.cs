using UnityEngine;
using System.Collections;

namespace Softdrink{

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
	[AddComponentMenu("Scripts/Input/Input Manager")]
	public class Input_Manager : MonoBehaviour {

		// Singleton instance
		public static Input_Manager Instance = null;

		[SerializeField]
		[TooltipAttribute("The KeyMaps used by the Input_Manager to bind key input into game actions.")]
		public KeyMap[] maps;

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

		void Update(){
			for(int i = 0; i < output.Length; i++){
				output[i].CheckActions();
			}
		}

		#if UNITY_EDITOR
		// Make sure the input and output are logically similar
		public void OnValidate(){
			if(maps.Length <= 0){
				maps = new KeyMap[1];
			}

			// Validate the KeyMaps that already exist
			for(int i = 0; i < maps.Length; i++){
				maps[i].Validate();
			}

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
				//output[i] .targetKeymap = maps[i];
				output[i].targetKeymap = new KeyMap(maps[i]);
			}
		}
		#endif

		public static void RebuildOutputs(){
			// Make sure there are the same number of input and output maps
			if(Instance.output.Length != Instance.maps.Length){
				Instance.output = new ActionOutput[Instance.maps.Length];
				for(int i = 0; i < Instance.output.Length; i++){
					Instance.output[i] = new ActionOutput();
				}
			}

			// Set names of Outputs to match input maps
			for(int i = 0; i < Instance.output.Length; i++){
				Instance.output[i].setName(Instance.maps[i].getName());
				Instance.output[i].setAssociatedPlayer(Instance.maps[i].getAssociatedPlayer());
				//output[i] .targetKeymap = maps[i];
				Instance.output[i].targetKeymap = new KeyMap(Instance.maps[i]);
			}
		}

		// GETTERS
		public static ActionOutput GetOutputFromPlayerID(int IDin){
			for(int i = 0; i < Instance.output.Length; i++){
				if(Instance.output[i].associatedPlayer == IDin) return Instance.output[i];
			}
			return Instance.output[0];
		}

		public static KeyMap GetMapFromPlayerID(int IDin){
			for(int i = 0; i < Instance.maps.Length; i++){
				if(Instance.maps[i].getAssociatedPlayer() == IDin) return Instance.maps[i];
			}
			return Instance.maps[0];
		}
		
	}
}