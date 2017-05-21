using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Softdrink{
	

	[AddComponentMenu("Scripts/Input/Axis Listener")]
	public class InputAxisListener : MonoBehaviour {

		

		[SerializeField]
		[TooltipAttribute("Is this AxisListener actively listening for inputs? \nIt is expensive to poll for any input, so only enable this when needed (EG: when rebinding controls).")]
		private bool listening = true;

		[SerializeField]
		[TooltipAttribute("The settings file controlling this Listener.")]
		private InputAxisListenerSettings settings = null;

		[SerializeField]
		[TooltipAttribute("Should try/catch error messages be reported to console?")]
		private bool logExceptions = false;
		
		void Awake(){
			Init();

		}

		void Init(){
			
		}

		private string axisNameOut = "";
		private string pAxisName = "";
		private string debugOut = "";
		void Update () {
			if(!listening) return;

			debugOut = DetectAxis();

			if(debugOut == "") return;

			Debug.Log(debugOut, this);
			
			
		}

		// Return the string Name of the axis that provided input,
		// and the direction +/- of input, delimited by a space
		public string DetectAxis(){
			pAxisName = axisNameOut;

			switch(settings.mode){
				case AxisListeningMode.Interpolated:
					axisNameOut = DetectAxisInterpolated();
					break;
				case AxisListeningMode.Raw:
					axisNameOut = DetectAxisRaw();
					break;
				case AxisListeningMode.InterpolatedDelta:
					axisNameOut = DetectAxisInterpolated();
					break;
				case AxisListeningMode.RawDelta:
					axisNameOut = DetectAxisRaw();
					break;
				default:
					axisNameOut = DetectAxisInterpolated();
					break;
			}

			if(settings.mode == AxisListeningMode.InterpolatedDelta || settings.mode == AxisListeningMode.RawDelta){
				if(axisNameOut == pAxisName) return "";
			}

			return axisNameOut;
		}

		
		// Functions for detecting axis movement -----------

		private string tempAxisName = "J0A0";
		private float valueTemp = 0f;
		
		public string DetectAxisInterpolated(){
			valueTemp = 0f;

			// Iterate through the number of defined virtual sticks
			for(int i = 1; i < settings.definedVirtualControllers + 1; i++){
				for(int j = 1; j < settings.axesPerController + 1; j++){
					tempAxisName = settings.controllerNamePrepend + i + settings.controllerNameIntersperse + j;
					try{
						valueTemp = Input.GetAxis(tempAxisName);
						if(Mathf.Abs(valueTemp) >= settings.deadzone){
							if(valueTemp < 0f) tempAxisName += " -";
							else tempAxisName += " +";
							return tempAxisName;
						}
					}catch(UnityException e){
						if(logExceptions) Debug.LogError("ERROR! Trying to access Input Axis " + tempAxisName + " failed.\n" + e, this);
						continue;
					}
				}
			}

			return "";
		}

		public string DetectAxisRaw(){
			valueTemp = 0f;

			// Iterate through the number of defined virtual sticks
			for(int i = 1; i < settings.definedVirtualControllers + 1; i++){
				for(int j = 1; j < settings.axesPerController + 1; j++){
					tempAxisName = settings.controllerNamePrepend + i + settings.controllerNameIntersperse + j;
					try{
						valueTemp = Input.GetAxisRaw(tempAxisName);
						if(Mathf.Abs(valueTemp) >= settings.deadzone){
							if(valueTemp < 0f) tempAxisName += " -";
							else tempAxisName += " +";
							return tempAxisName;
						}
					}catch(UnityException e){
						if(logExceptions) Debug.LogError("ERROR! Trying to access Input Axis " + tempAxisName + " failed.\n" + e, this);
						continue;
					}
				}
			}

			return "";
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

		public void setDefinedVirtualControllers(int input){
			settings.definedVirtualControllers = input;
		}

		public void setAxesPerController(int input){
			settings.axesPerController = input;
		}

		public void setListening(bool input){
			listening = input;
		}
	}
}
