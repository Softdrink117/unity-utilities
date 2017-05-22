using UnityEngine;
using System.Collections;
using SharpConfig;
using System.IO;

namespace Softdrink{

	//TODO: Create new InputMaps intelligently based on
	// number of players and connected devices. EG:
	// - 1 Player, 1 Joystick - bindings on J1A1, J1A2, etc
	// - 2 Players, 2 Joysticks - P1 bindings defaulted to J1A1, etc
	// and P2 bindings defaulted to J2A1, etc
	//TODO: InputBinder has a generic modal dialog or method
	// that basically asks "press a key to bind to X action"
	// This can be either standalone for modifying one key
	// or used in succession for a 'quick bind'
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

		// GETTERS 

		// Used when reverting to a known safe configuration during failed Input Bind operations
		public KeyMap getKeyMap(){
			return targetKeymap;
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

		[HeaderAttribute("Configuration Files")]

		[TooltipAttribute("The file that will be loaded/saved for user-defined configurations.")]
		public string userConfigFile = "inputConfig";

		[TooltipAttribute("The file that will be loaded as the default configuration.")]
		public string defaultConfigFile = "defaultInputConfig";


		[HeaderAttribute("Miscellaneous")]

		[SerializeField]
		[TooltipAttribute("Enables debug print statements to console.")]
		private bool debugPrint = false;


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

			hasTriedDefaultLoad = false;

			LoadInputIni();

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

			RebuildOutputsLocal();
		}
		#endif

		void RebuildOutputsLocal(){
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

		public static void RebuildOutputs(){
			Instance.RebuildOutputsLocal();
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

		public static int GetMapsCount(){
			return Instance.maps.Length;
		}

		public static string[] GetMapNames(){
			string[] output = new string[Instance.maps.Length];
			for(int i = 0; i < Instance.maps.Length; i++){
				output[i] = Instance.maps[i].getName();
			}
			return output;
		}
		

		// SERIALIZATION / DESERIALIZATION -------

		[ContextMenu("SaveCurrentConfig")]
		void SaveCurrentConfig(){
			SaveInputConfig("Assets/Input/Resources/" + userConfigFile + ".ini", false);
		}

		// Externally accesible call to save the current configuration to file
		public static void SaveConfig(){
			Instance.SaveCurrentConfig();
		}

		[ContextMenu("SaveDefaultConfig")]
		void SaveDefaultConfig(){
			SaveInputConfig("Assets/Input/Resources/" + defaultConfigFile + ".cfg", true);
		}

		void SaveInputConfig(string filename, bool useBinary){
			Configuration cfg = new Configuration();

			// Indicate the number of defined maps in the output file
			cfg["General"]["Defined Keymaps"].IntValue = maps.Length;

			// Iterate through all defined maps and add to configuration
			for(int i = 0; i < maps.Length; i++){
				cfg["KeyMap " + i]["Keymap Name"].StringValue = maps[i].getName();
				cfg["KeyMap " + i]["Associated Player ID"].StringValue = maps[i].getAssociatedPlayer().ToString();
				
				// Get the labels and EInput strings for this Keymap
				string[] labels = maps[i].getMapLabels();
				string[] bindings = maps[i].getEInputStrings();

				// Iterate through the maps and add to the config
				for(int j = 0; j < labels.Length; j++){
					cfg["KeyMap " + i][labels[j]].StringValue = bindings[j];
				}
			}

			// Finish and save the config to text file
			#if UNITY_EDITOR
				if(debugPrint) Debug.Log("Saving input configuration...");
			#endif
			if(useBinary) cfg.SaveToBinaryFile(filename);
			else cfg.SaveToFile(filename);
		}

		[ContextMenu("LoadInputIni")]
		void LoadInputIni(){
			hasTriedDefaultLoad = false;
			if(File.Exists("Assets/Input/Resources/" + userConfigFile + ".ini")){
				LoadInputConfig("Assets/Input/Resources/" + userConfigFile + ".ini", false);
			}else{
				LoadDefaultConfig();
			}
			
		}

		[ContextMenu("LoadDefaultConfig")]
		void LoadDefaultConfig(){
			if(!File.Exists("Assets/Input/Resources/" + defaultConfigFile + ".cfg")){
				Debug.LogError("ERROR: Input_Manager failed when trying to load default config file... Does the file exist?", this);
				return;
			}
			// Used to prevent infinite recursion
			if(!hasTriedDefaultLoad){
				LoadInputConfig("Assets/Input/Resources/" + defaultConfigFile + ".cfg", true);
				hasTriedDefaultLoad = true;
			}else{
				Debug.LogError("ERROR: Input_Manager has attempted to load a default Input Config from " + defaultConfigFile + " as a fallback, and failed. Please verify integrity of default config file!", this);
			}
		}

		private bool hasTriedDefaultLoad = false;

		void LoadInputConfig(string filename, bool useBinary){
			try{
				Configuration cfg;
				if(useBinary) cfg = Configuration.LoadFromBinaryFile(filename);
				else cfg = Configuration.LoadFromFile(filename);

				// Grab the map labels from the existing map
				string[] labels = maps[0].getMapLabels();
				string[] values = new string[labels.Length + 2];

				// Get the number of defined maps from the loaded file
				int mapCount = cfg["General"]["Defined Keymaps"].IntValue;
				maps = new KeyMap[mapCount];

				for(int i = 0; i < mapCount; i++){
					values[0] = cfg["KeyMap " + i]["KeyMap Name"].StringValue;
					values[1] = cfg["KeyMap " + i]["Associated Player ID"].StringValue;

					// Iterate through the defined binding labels
					for(int j = 0; j < labels.Length; j++){
						values[2 + j] = cfg["KeyMap " + i][labels[j]].StringValue;
					}

					maps[i] = new KeyMap(values);
				}

				// Then rebuild the Action Outputs to match the new KeyMaps
				RebuildOutputsLocal();

				//hasTriedDefaultLoad = false;

			}catch{
				Debug.LogError("ERROR: Input_Manager encountered an error trying to load an input configuration file from " + filename + "\nPlease verify integrity and formatting of target file! \nReverting to default config...", this);
				LoadDefaultConfig();
			}

		}
	}
}