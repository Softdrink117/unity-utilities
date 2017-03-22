using UnityEngine;
using System.Collections;
using System.Text;

namespace Softdrink{

	// Class to wrap both Key and Axis input in one
	[System.Serializable]
	public class StandardInput{
		private KeyCode key = KeyCode.None;
		private string axis = "";
		private bool axisPositive = true;
		private bool isKey = true;
		[HideInInspector]
		public bool isDefined = false;

		// Overloaded constructor variants
		public StandardInput(){
			key = KeyCode.None;
			axis = "";
			isKey = true;
			isDefined = false;
		}

		public StandardInput(string axisIn, bool axisPositiveIn){
			key = KeyCode.None;
			axis = axisIn;
			axisPositive = axisPositiveIn;
			isKey = false;

			isDefined = true;
		}

		public StandardInput(KeyCode keyIn){
			key = keyIn;
			axis = "";
			isKey = true;

			isDefined = true;
		}

		// EVALUTE INPUT STATUS
		private float outTemp = 0f;
		private float pOutTemp = 0f;

		// Evaluate and return a bool
		public bool GetInput(){
			pOutTemp = outTemp;
			if(isKey){
				if(Input.GetKey(key)) return true;
			}else{
				outTemp = Input.GetAxis(axis);
				if(axisPositive){
					if(outTemp > 0.25f) return true;
				}else{
					if(outTemp < -0.25f) return true;
				}
			}

			return false;
		}

		// Similar to GetKeyDown
		public bool GetInputDown(){
			pOutTemp = outTemp;
			if(isKey){
				if(Input.GetKeyDown(key)) return true;
			}else{
				outTemp = Input.GetAxis(axis);
				if(outTemp == pOutTemp) return false;

				if(axisPositive){
					if(outTemp > 0.25f) return true;
				}else{
					if(outTemp < -0.25f) return true;
				}
			}

			return false;
		}

		// Similar to GetKeyUp
		public bool GetInputUp(){
			pOutTemp = outTemp;
			if(isKey){
				if(Input.GetKeyUp(key)) return true;
			}else{
				outTemp = Input.GetAxis(axis);
				if(outTemp == pOutTemp) return false;

				if(axisPositive){
					if(outTemp > 0.25f) return true;
				}else{
					if(outTemp < -0.25f) return true;
				}
			}

			return false;
		}

		// Evaluate and return a float
		public float GetInputValue(){
			pOutTemp = outTemp;
			if(isKey){
				if(Input.GetKey(key)) return 1f;
				else return 0f;
			}else{
				outTemp = Input.GetAxis(axis);
				if(axisPositive){
					if(outTemp > 0f) return outTemp;
					else return 0f;
				}else{
					if(outTemp < 0f) return outTemp;
					else return 0f;
				}
			}

			// return 0f;
		}

		// Print information about this StandardInput
		public void Print(){
			if(isKey) Debug.Log(key);
			else Debug.Log(axis + " positive: " + axisPositive);
		}

	}

	[RequireComponent(typeof(InputAxisListener))]
	[RequireComponent(typeof(InputKeyListener))]
	[AddComponentMenu("Scripts/Input/Input Listener")]
	public class InputListener : MonoBehaviour {

		// Singelton Instance
		public static InputListener Instance;

		[SerializeField]
		[TooltipAttribute("Is this Listener actively listening for inputs? \nIt is expensive to poll for any input, so only enable this when needed (EG: when rebinding controls).")]
		private bool listening = true;

		// Internal references to attached Key and Axis listeners
		private InputKeyListener _ikl;
		private InputAxisListener _ial;

		void Awake() {
			// If the Instance doesn't already exist
			if(Instance == null){
				// If the instance doesn't already exist, set it to this
				Instance = this;
			}else if(Instance != this){
				// If an instance already exists that isn't this, destroy this instance and log what happened
				Destroy(gameObject);
				Debug.LogError("ERROR! The InputListener encountered another instance of InputListener; it destroyed itself rather than overwrite the existing instance.", this);
			}

			GetReferences();
			_ikl.setListening(false);
			_ial.setListening(false);

			Debug.Log(DetectConnectedControllers(), this);
		}

		void GetReferences(){
			_ikl = gameObject.GetComponent<InputKeyListener>() as InputKeyListener;
			_ial = gameObject.GetComponent<InputAxisListener>() as InputAxisListener;

			if(_ikl == null || _ial == null) Debug.LogError("ERROR: The Input Listener could not associate all references to attached components!", this);
		
		}

		private KeyCode keyOut = KeyCode.None;
		private string axisOut = "";
		private string[] axisOutTemp = new string[2];
		private StandardInput inputOut = new StandardInput();
		
		void Update() {
			if(!listening) return;

			inputOut = GetInput();

			if(inputOut.isDefined) inputOut.Print();
		}

		void OnValidate(){
			if(_ikl == null || _ial == null) GetReferences();
			_ikl.setListening(false);
			_ial.setListening(false);

		}

		public static StandardInput DetectInput(){
			return Instance.GetInput();
		}

		public StandardInput GetInput(){
			keyOut = _ikl.DetectKey();
			axisOut = _ial.DetectAxis();

			//if(keyOut != KeyCode.None) Debug.Log(keyOut, this);
			//else if(axisOut != "") Debug.Log(axisOut, this);
			inputOut = new StandardInput();

			if(keyOut != KeyCode.None) inputOut = new StandardInput(keyOut);
			else if(axisOut != ""){
				// Parse for +/-
				axisOutTemp = axisOut.Split(' ');
				if(axisOutTemp.Length >= 2){
					if(axisOutTemp[1] == "+") inputOut = new StandardInput(axisOutTemp[0], true);
					else inputOut = new StandardInput(axisOutTemp[0], false);
				}
			}

			return inputOut;
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

		public static void setListening(bool input){
			Instance.listening = input;
		}
	}
}
