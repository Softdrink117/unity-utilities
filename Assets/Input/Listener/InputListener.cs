using UnityEngine;
using System.Collections;
using System.Text;

namespace Softdrink{

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
		private EInput inputOut = new EInput(KeyCode.None);
		
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

		public static EInput DetectInput(){
			return Instance.GetInput();
		}

		public EInput GetInput(){
			keyOut = _ikl.DetectKey();
			axisOut = _ial.DetectAxis();

			//if(keyOut != KeyCode.None) Debug.Log(keyOut, this);
			//else if(axisOut != "") Debug.Log(axisOut, this);
			inputOut = new EInput(KeyCode.None);

			if(keyOut != KeyCode.None) inputOut = new EInput(keyOut);
			else if(axisOut != ""){
				// Parse for +/-
				axisOutTemp = axisOut.Split(' ');
				if(axisOutTemp.Length >= 2){
					if(axisOutTemp[1] == "+") inputOut = new EInput(axisOutTemp[0], true);
					else inputOut = new EInput(axisOutTemp[0], false);
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
