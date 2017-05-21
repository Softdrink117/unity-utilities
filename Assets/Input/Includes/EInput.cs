using UnityEngine;
using System.Collections;

namespace Softdrink{

	// Class to wrap both Key and Axis input in one 
	// "Extended Input" class
	[System.Serializable]
	public class EInput{
		//[ReadOnlyAttribute]
		[TooltipAttribute("The Keyboard Key bound to this action, if any.")]
		public KeyCode key = KeyCode.None;
		//[ReadOnlyAttribute]
		[TooltipAttribute("The String name of the Input Axis bound to this action, if any.")]
		public string axis = "";
		//[ReadOnlyAttribute]
		[TooltipAttribute("If this action binding uses an Input Axis, is that axis Positive or Negative?")]
		public bool axisPositive = true;
		[ReadOnlyAttribute]
		[TooltipAttribute("Is this action binding using a Keyboard Key or an Input Axis?")]
		public bool isKey = true;
		[HideInInspector]
		public bool isDefined = false;

		// Overloaded constructor variants

		public EInput(string axisIn, bool axisPositiveIn){
			key = KeyCode.None;
			axis = axisIn;
			axisPositive = axisPositiveIn;
			isKey = false;

			isDefined = true;
		}

		public EInput(KeyCode keyIn){
			key = keyIn;
			axis = "";
			isKey = true;

			if(key == KeyCode.None) isDefined = false;

			isDefined = true;
		}

		public EInput(EInput input){
			key = input.key;
			axis = input.axis;
			isKey = input.isKey;
			isDefined = input.isDefined;
		}

		// Validate settings ------

		public void Validate(){
			if(key != KeyCode.None){
				isKey = true;
				isDefined = true;
				axis = "";
				return;
			}
			if(!axis.Equals("")){
				isKey = false;
				isDefined = true;
				key = KeyCode.None;
				return;
			}
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
		// CURRENTLY NOT QUITE WORKING FOR AXIS
		public bool GetInputDown(){
			
			if(isKey){
				if(Input.GetKeyDown(key)) return true;
			}else{
				pOutTemp = outTemp;
				outTemp = Input.GetAxis(axis);
				if(outTemp == pOutTemp) return false;
				if(pOutTemp != 0f) return false;

				if(axisPositive){
					if(outTemp > 0.0f) return true;
				}else{
					if(outTemp < -0.0f) return true;
				}
			}

			return false;
		}

		// Similar to GetKeyUp
		// CURRENTLY NOT QUITE WORKING FOR AXIS
		public bool GetInputUp(){
			
			if(isKey){
				if(Input.GetKeyUp(key)) return true;
			}else{
				pOutTemp = outTemp;
				outTemp = Input.GetAxis(axis);
				if(outTemp == pOutTemp) return false;
				if(axisPositive){
					if(pOutTemp != 1f) return false;
				}else{
					if(pOutTemp != -1f) return false;
				}

				if(axisPositive){
					if(outTemp > 0.0f) return true;
				}else{
					if(outTemp < -0.0f) return true;
				}
			}

			return false;
		}

		// Evaluate and return a float
		public float GetInputValue(){
			
			if(isKey){
				if(Input.GetKey(key)) return 1f;
				else return 0f;
			}else{
				pOutTemp = outTemp;
				outTemp = Input.GetAxis(axis);
				if(axisPositive){
					if(outTemp > 0f) return outTemp;
					else{
						outTemp = 0f;
						return 0f;
					}
				}else{
					if(outTemp < 0f) return outTemp;
					else{
						outTemp = 0f;
						return 0f;
					}
				}
			}

			// return 0f;
		}

		// Print information about this EInput
		public void Print(){
			if(isKey) Debug.Log(key);
			else Debug.Log(axis + " positive: " + axisPositive);
		}

	}

}
