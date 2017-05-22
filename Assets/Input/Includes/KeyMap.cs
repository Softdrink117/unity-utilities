using UnityEngine;
using System.Collections;

namespace Softdrink{

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

		[ReadOnlyAttribute]
		[TooltipAttribute("Is this KeyMap fully bound?")]
		public bool fullyBound = false;

		[HeaderAttribute("Directions")]

		[TooltipAttribute("What input is bound to UP?")]
		public EInput up;
		[TooltipAttribute("What input is bound to DOWN?")]
		public EInput down;
		[TooltipAttribute("What input is bound to LEFT?")]
		public EInput left;
		[TooltipAttribute("What input is bound to RIGHT?")]
		public EInput right;

		[HeaderAttribute("Actions")]

		[TooltipAttribute("What input is bound to A?")]
		public EInput a;
		[TooltipAttribute("What input is bound to B?")]
		public EInput b;
		[TooltipAttribute("What input is bound to X?")]
		public EInput x;
		[TooltipAttribute("What input is bound to Y?")]
		public EInput y;

		[TooltipAttribute("What input is bound to Start?")]
		public EInput start;

		// Constructors -------

		public KeyMap(KeyMap input){
			Name = input.getName();
			associatedPlayer = input.getAssociatedPlayer();

			up = new EInput(input.up);
			down = new EInput(input.down);
			left = new EInput(input.left);
			right = new EInput(input.right);

			a = new EInput(input.a);
			b = new EInput(input.b);
			x = new EInput(input.x);
			y = new EInput(input.y);

			start = new EInput(input.start);
		}

		// Special constructor to build a new KeyMap when loading from a String
		public KeyMap(string[] values){
			Name = values[0];
			//Debug.Log(values[1]);
			associatedPlayer = System.Convert.ToInt32(values[1]);

			up = new EInput(values[2]);
			down = new EInput(values[3]);
			left = new EInput(values[4]);
			right = new EInput(values[5]);

			a = new EInput(values[6]);
			b = new EInput(values[7]);
			x = new EInput(values[8]);
			y = new EInput(values[9]);

			start = new EInput(values[10]);

		}

		// Set -------

		// Set the keymap from an existing one WITHOUT building a new one
		public void SetKeyMap(KeyMap input){
			Name = input.getName();
			associatedPlayer = input.getAssociatedPlayer();

			up = new EInput(input.up);
			down = new EInput(input.down);
			left = new EInput(input.left);
			right = new EInput(input.right);

			a = new EInput(input.a);
			b = new EInput(input.b);
			x = new EInput(input.x);
			y = new EInput(input.y);

			start = new EInput(input.start);
		}

		// Validate -------

		public void Validate(){
			up.Validate();
			down.Validate();
			left.Validate();
			right.Validate();

			a.Validate();
			b.Validate();
			x.Validate();
			y.Validate();

			start.Validate();

			fullyBound = CheckFullyBound();
		}

		public bool CheckFullyBound(){
			if(!up.isDefined) return false;
			if(!down.isDefined) return false;
			if(!right.isDefined) return false;
			if(!left.isDefined) return false;

			if(!a.isDefined) return false;
			if(!b.isDefined) return false;
			if(!x.isDefined) return false;
			if(!y.isDefined) return false;

			if(!start.isDefined) return false;

			return true;
		}

		// Evaluate a given Key

		public bool EvaluateUp(){
			return up.GetInput();
		}

		public bool EvaluateDown(){
			return down.GetInput();
		}

		public bool EvaluateLeft(){
			return left.GetInput();
		}

		public bool EvaluateRight(){
			return right.GetInput();
		}

		public bool EvaluateA(){
			return a.GetInput();
		}

		public bool EvaluateB(){
			return b.GetInput();
		}

		public bool EvaluateX(){
			return x.GetInput();
		}

		public bool EvaluateY(){
			return y.GetInput();
		}

		public bool EvaluateStart(){
			return start.GetInput();
		}

		// GETTERS

		public string getName(){
			return Name;
		}

		public int getAssociatedPlayer(){
			return associatedPlayer;
		}

		// Serialization -------
		// Format the KeyMap as a series of strings for saving to a SharpConfig file
		
		// List of the names of each key being mapped (these are config items labels)
		public string[] getMapLabels(){
			string[] output = new string[9];

			output[0] = "Up";
			output[1] = "Down";
			output[2] = "Left";
			output[3] = "Right";

			output[4] = "A Button";
			output[5] = "B Button";
			output[6] = "X Button";
			output[7] = "Y Button";

			output[8] = "Start Button";

			return output;
		}

		// List of the serializable formatted EInputs (the actual content of each config item)
		public string[] getEInputStrings(){
			string[] output = new string[9];

			output[0] = up.ConvertToString();
			output[1] = down.ConvertToString();
			output[2] = left.ConvertToString();
			output[3] = right.ConvertToString();

			output[4] = a.ConvertToString();
			output[5] = b.ConvertToString();
			output[6] = x.ConvertToString();
			output[7] = y.ConvertToString();

			output[8] = start.ConvertToString();

			return output;
		}

	}

}
