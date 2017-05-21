using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Softdrink{
	public class SimpleInputBinderText : MonoBehaviour {

		private string displayText = "";

		[SerializeField]
		[TooltipAttribute("The default text to show when binding a new control.")]
		private string defaultBindOpText = "Please press the input to use for ";

		private string keyMapName = "";

		private Text _text;

		private bool active = false;

		// Init ------

		void Awake(){
			GetReferences();
		}

		void GetReferences(){
			_text = gameObject.GetComponent<Text>() as Text;
		}

		// Update Text -------

		void SetText(){
			_text.text = displayText;
		}

		public void Set(BindOperation input, string mapName){
			active = true;
			keyMapName = mapName;
			UpdateCurrentBind(input);

		}

		public void Unset(){
			displayText = "";
			if(active) SetText();
			active = false;
		}

		public void UpdateCurrentBind(BindOperation input){
			displayText = keyMapName + "\n" + defaultBindOpText;

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