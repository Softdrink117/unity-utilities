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

		[HeaderAttribute("Directions")]

		public EInput up = new EInput();
		public EInput down = new EInput();
		public EInput left = new EInput();
		public EInput right = new EInput();

		[TooltipAttribute("What Key(s) are bound to the logical Up action?")]
		public KeyCode[] upKeys = new KeyCode[1];

		[TooltipAttribute("What Key(s) are bound to the logical Down action?")]
		public KeyCode[] downKeys = new KeyCode[1];

		[TooltipAttribute("What Key(s) are bound to the logical Left action?")]
		public KeyCode[] leftKeys = new KeyCode[1];

		[TooltipAttribute("What Key(s) are bound to the logical Right action?")]
		public KeyCode[] rightKeys = new KeyCode[1];

		[HeaderAttribute("Actions")]

		public EInput a = new EInput();
		public EInput b = new EInput();
		public EInput x = new EInput();
		public EInput y = new EInput();

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

			up = input.up;
			down = input.down;
			left = input.left;
			right = input.right;

			upKeys = input.getKeys(input.upKeys);
			downKeys = input.getKeys(input.downKeys);
			leftKeys = input.getKeys(input.leftKeys);
			rightKeys = input.getKeys(input.rightKeys);

			a = input.a;
			b = input.b;
			x = input.x;
			y = input.y;

			aButtonKeys = input.getKeys(input.aButtonKeys);
			bButtonKeys = input.getKeys(input.bButtonKeys);
			xButtonKeys = input.getKeys(input.xButtonKeys);
			yButtonKeys = input.getKeys(input.yButtonKeys);
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
		}

		// Evaluate a given Key

		public bool EvaluateUp(){
			// for(int i = 0; i < upKeys.Length; i++){
			// 	if(Input.GetKey(upKeys[i])) return true;
			// }
			// return false;
			return up.GetInput();
		}

		public bool EvaluateDown(){
			// for(int i = 0; i < downKeys.Length; i++){
			// 	if(Input.GetKey(downKeys[i])) return true;
			// }
			// return false;
			return down.GetInput();
		}

		public bool EvaluateLeft(){
			// for(int i = 0; i < leftKeys.Length; i++){
			// 	if(Input.GetKey(leftKeys[i])) return true;
			// }
			// return false;
			return left.GetInput();
		}

		public bool EvaluateRight(){
			// for(int i = 0; i < rightKeys.Length; i++){
			// 	if(Input.GetKey(rightKeys[i])) return true;
			// }
			// return false;
			return right.GetInput();
		}

		public bool EvaluateA(){
			// for(int i = 0; i < aButtonKeys.Length; i++){
			// 	if(Input.GetKey(aButtonKeys[i])) return true;
			// }
			// return false;
			return a.GetInput();
		}

		public bool EvaluateB(){
			// for(int i = 0; i < bButtonKeys.Length; i++){
			// 	if(Input.GetKey(bButtonKeys[i])) return true;
			// }
			// return false;
			return b.GetInput();
		}

		public bool EvaluateX(){
			// for(int i = 0; i < xButtonKeys.Length; i++){
			// 	if(Input.GetKey(xButtonKeys[i])) return true;
			// }
			// return false;
			return x.GetInput();
		}

		public bool EvaluateY(){
			// for(int i = 0; i < yButtonKeys.Length; i++){
			// 	if(Input.GetKey(yButtonKeys[i])) return true;
			// }
			// return false;
			return y.GetInput();
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

}
