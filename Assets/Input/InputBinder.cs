using UnityEngine;
using System.Collections;

namespace Softdrink{
	[System.Flags]
	public enum BindOperation{
		None = 0,
		Up = 1 << 0,
		Down = 1 << 1,
		Left = 1 << 2,
		Right = 1 << 3,
		A_Button = 1 << 4,
		B_Button = 1 << 5,
		X_Button = 1 << 6,
		Y_Button = 1 << 7,
		Start_Button = 1 << 8,
	};

	public enum InputBindState{
		Idle = 0,
		Binding = 1,
		Confirmation = 2,
		Success = 3,
		Failure = 4,
	};

	// InputBinder is a Singleton - mostly to avoid ANY POSSIBLE ISSUE with
	// more than one existing at once and massively fucking up the Input system
	[AddComponentMenu("Scripts/Input/Input Binder")]
	public class InputBinder : MonoBehaviour {

		// Singleton instance
		public static InputBinder Instance = null;

		// Stores what, if any, bind operations are currently ongoing
		// If this is not 0 << 0, some operation is happening
		// Multiple values can be queued simultaneously - this is what allows for
		// the Quick Bind to occur!
		[EnumFlag("Current Bind Operation")]
		public BindOperation bindOperation = 0;

		[ReadOnlyAttribute]
		public InputBindState state = InputBindState.Idle;

		[ReadOnlyAttribute]
		public bool wasBinding = false;

		[HeaderAttribute("Confirm Dialog and Timer")]

		[TooltipAttribute("Should a confirmaton be requested before assigning the new Map?")]
		public bool useConfirmation = true;

		[TooltipAttribute("How long should the confirmation timeout before falling back to the old Map be?")]
		public float confirmationTimeout = 10f;

		[SerializeField]
		[TooltipAttribute("How long should the START button be held before accepting the current config?")]
		private float startHoldTime = 1.0f;
		private float currentStartHoldTime = 0f;

		[HideInInspector]
		public float currentConfirmationTime = 0f;

		[HideInInspector]
		public bool isConfirmationDialog = false;

		[SerializeField]
		[TooltipAttribute("How long should success/failure messages be shown?")]
		private float resultMessageTimeout = 2.0f;
		private float currentResultTime = 0.0f;

		[HeaderAttribute("Output Text")]

		[SerializeField]
		[TooltipAttribute("Use Simple Input Binder Text")]
		private bool useBinderText = false;

		[SerializeField]
		[TooltipAttribute("The Simple Input Binder Text to use, if enabled.")]
		private SimpleInputBinderText simpleText = null;

		[HeaderAttribute("Miscellaneous")]

		[SerializeField]
		[TooltipAttribute("Should the Input_Manager automatically rebuild all Action Outputs on conclusion of a Bind? \nIf disabled, it will be necessary to manually rebuild the Action Outputs.")]
		private bool autoRebuildOutputs = true;

		[SerializeField]
		[TooltipAttribute("Should the Input_Manager automatically save the current config file on successful Bind? \nIf disabled, it will be necessary to manually save after a Bind operation.")]
		private bool autosaveOnSuccess = true;

		[SerializeField]
		[TooltipAttribute("Enables debug print statements to console.")]
		private bool debugPrint = false;

		// Use references only! This allows for instant change of the existing map

		private string mapName = "Sup Bro";

		private KeyMap map = null;
		private KeyMap backupMap = null;

		// Initialization -------

		void Awake(){
			// If the Instance doesn't already exist
			if(Instance == null){
				// If the instance doesn't already exist, set it to this
				Instance = this;
			}else if(Instance != this){
				// If an instance already exists that isn't this, destroy this instance and log what happened
				Destroy(gameObject);
				Debug.LogError("ERROR! The Input Binder encountered another instance of InputBinder; it destroyed itself rather than overwrite the existing instance.", this);
			}

			bindOperation = 0;
			wasBinding = false;

			currentConfirmationTime = 0.0f;
			currentResultTime = 0.0f;
			currentStartHoldTime = 0.0f;
		}
		// Update -------

		private EInput outTemp = new EInput(KeyCode.None);

		void Update(){
			switch(state){
				case InputBindState.Binding:
					wasBinding = true;
					Bind();
					break;
				case InputBindState.Confirmation:
					Confirmation();
					break;
				case InputBindState.Success:
					Success();
					break;
				case InputBindState.Failure:
					Failure();
					break;
				case InputBindState.Idle:
					if(wasBinding){
						if(useBinderText) simpleText.Unset();
						wasBinding = false;
					}
					break;
			}
			
		}


		void Bind(){
			if(bindOperation != BindOperation.None){
				// Make sure the Listener is listening for inputs
				InputListener.setListening(true);


				// Directions -------
				if(hasFlag(bindOperation, BindOperation.Up)){
					SetBinderText();
					outTemp = InputListener.DetectInput();
					if(outTemp.isDefined && map != null) map.up = new EInput(outTemp);
					if(outTemp.isDefined) bindOperation = unsetFlag(bindOperation, BindOperation.Up);
					return;
				}
				if(hasFlag(bindOperation, BindOperation.Down)){
					SetBinderText();
					outTemp = InputListener.DetectInput();
					if(outTemp.isDefined && map != null) map.down = new EInput(outTemp);
					if(outTemp.isDefined) bindOperation = unsetFlag(bindOperation, BindOperation.Down);
					return;
				}
				if(hasFlag(bindOperation, BindOperation.Left)){
					SetBinderText();
					outTemp = InputListener.DetectInput();
					if(outTemp.isDefined && map != null) map.left = new EInput(outTemp);
					if(outTemp.isDefined) bindOperation = unsetFlag(bindOperation, BindOperation.Left);
					return;
				}
				if(hasFlag(bindOperation, BindOperation.Right)){
					SetBinderText();
					outTemp = InputListener.DetectInput();
					if(outTemp.isDefined && map != null) map.right = new EInput(outTemp);
					if(outTemp.isDefined) bindOperation = unsetFlag(bindOperation, BindOperation.Right);
					return;
				}

				// Face Buttons -------
				if(hasFlag(bindOperation, BindOperation.A_Button)){
					SetBinderText();
					outTemp = InputListener.DetectInput();
					if(outTemp.isDefined && map != null) map.a = new EInput(outTemp);
					if(outTemp.isDefined) bindOperation = unsetFlag(bindOperation, BindOperation.A_Button);
					return;
				}
				if(hasFlag(bindOperation, BindOperation.B_Button)){
					SetBinderText();
					outTemp = InputListener.DetectInput();
					if(outTemp.isDefined && map != null) map.b = new EInput(outTemp);
					if(outTemp.isDefined) bindOperation = unsetFlag(bindOperation, BindOperation.B_Button);
					return;
				}
				if(hasFlag(bindOperation, BindOperation.X_Button)){
					SetBinderText();
					outTemp = InputListener.DetectInput();
					if(outTemp.isDefined && map != null) map.x = new EInput(outTemp);
					if(outTemp.isDefined) bindOperation = unsetFlag(bindOperation, BindOperation.X_Button);
					return;
				}
				if(hasFlag(bindOperation, BindOperation.Y_Button)){
					SetBinderText();
					outTemp = InputListener.DetectInput();
					if(outTemp.isDefined && map != null) map.y = new EInput(outTemp);
					if(outTemp.isDefined) bindOperation = unsetFlag(bindOperation, BindOperation.Y_Button);
					return;
				}
				
				// Start Button -------
				if(hasFlag(bindOperation, BindOperation.Start_Button)){
					SetBinderText();
					outTemp = InputListener.DetectInput();
					if(outTemp.isDefined && map != null) map.start = new EInput(outTemp);
					if(outTemp.isDefined) bindOperation = unsetFlag(bindOperation, BindOperation.Start_Button);
					return;
				}

			}
			if(bindOperation == 0){
				if(useConfirmation){
					state = InputBindState.Confirmation;
					currentConfirmationTime = 0f;
					currentStartHoldTime = 0f;
				}
				else{
					state = InputBindState.Idle;
					if(autoRebuildOutputs) Input_Manager.RebuildOutputs();
				}
				// Deactivate the Listener
				InputListener.setListening(false);
			}
			//if(debugPrint) Debug.Log(System.Convert.ToString((int)bindOperation, 2));
		}

		void SetBinderText(){
			if(useBinderText){
				simpleText.Set(bindOperation, mapName);
			}
		}

		void SetMaps(KeyMap mapIn){
			map = mapIn;
			backupMap = new KeyMap(mapIn);
			mapName = map.getName();
		}

		void Confirmation(){
			if(useBinderText){
				simpleText.Set(confirmationTimeout - currentConfirmationTime);
			}

			if(map.start.GetInput()){
				currentStartHoldTime += Time.unscaledDeltaTime;
			}else{
				currentStartHoldTime = 0f;
			}

			if(currentStartHoldTime >= startHoldTime){
				state = InputBindState.Success;
				currentResultTime = 0.0f;
				Input_Manager.RebuildOutputs();
				if(autosaveOnSuccess) Input_Manager.SaveConfig();
				return;
			}

			// Revert to the previous map if the time is expired
			if(currentConfirmationTime >= confirmationTimeout){
				//map = Input_Manager.GetOutputFromPlayerID(map.getAssociatedPlayer()).getKeyMap();
				map.SetKeyMap(backupMap);
				state = InputBindState.Failure;
				currentResultTime = 0.0f;
				Input_Manager.RebuildOutputs();
			}

			currentConfirmationTime += Time.unscaledDeltaTime;
		}

		void Success(){
			if(useBinderText) simpleText.SetSuccess();
			currentResultTime += Time.unscaledDeltaTime;
			if(currentResultTime >= resultMessageTimeout){
				state = InputBindState.Idle;
				currentResultTime = 0.0f;
			}
		}

		void Failure(){
			if(useBinderText) simpleText.SetFailure();
			currentResultTime += Time.unscaledDeltaTime;
			if(currentResultTime >= resultMessageTimeout){
				state = InputBindState.Idle;
				currentResultTime = 0.0f;
			}
		}

		// Binding functions -------

		public static void BeginQuickBind(KeyMap mapIn){
			Instance.SetMaps(mapIn);
			Instance.BeginQuickBind();
		}

		[ContextMenu("BeginQuickBind")]
		public void BeginQuickBind(){
			state = InputBindState.Binding;
			bindOperation = BindOperation.Up | BindOperation.Down | BindOperation.Left | BindOperation.Right;
			bindOperation |= BindOperation.A_Button | BindOperation.B_Button | BindOperation.X_Button | BindOperation.Y_Button;
			bindOperation |= BindOperation.Start_Button;
			if(debugPrint) Debug.Log(System.Convert.ToString((int)bindOperation, 2));
		}


		public static void RebindUp(KeyMap mapIn){
			Instance.SetMaps(mapIn);
			Instance.RebindUp();
		}

		[ContextMenu("RebindUp")]
		public void RebindUp(){
			state = InputBindState.Binding;
			bindOperation = BindOperation.Up;
		}


		public static void RebindDown(KeyMap mapIn){
			Instance.SetMaps(mapIn);
			Instance.RebindDown();
		}

		[ContextMenu("RebindDown")]
		public void RebindDown(){
			state = InputBindState.Binding;
			bindOperation = BindOperation.Down;
		}


		public static void RebindLeft(KeyMap mapIn){
			Instance.SetMaps(mapIn);
			Instance.RebindLeft();
		}

		[ContextMenu("RebindLeft")]
		public void RebindLeft(){
			state = InputBindState.Binding;
			bindOperation = BindOperation.Left;
		}


		public static void RebindRight(KeyMap mapIn){
			Instance.SetMaps(mapIn);
			Instance.RebindRight();
		}

		[ContextMenu("RebindRight")]
		public void RebindRight(){
			state = InputBindState.Binding;
			bindOperation = BindOperation.Right;
		}


		public static void RebindA(KeyMap mapIn){
			Instance.SetMaps(mapIn);
			Instance.RebindA();
		}

		[ContextMenu("RebindA")]
		public void RebindA(){
			state = InputBindState.Binding;
			bindOperation = BindOperation.A_Button;
		}


		public static void RebindB(KeyMap mapIn){
			Instance.SetMaps(mapIn);
			Instance.RebindB();
		}

		[ContextMenu("RebindB")]
		public void RebindB(){
			state = InputBindState.Binding;
			bindOperation = BindOperation.B_Button;
		}


		public static void RebindX(KeyMap mapIn){
			Instance.SetMaps(mapIn);
			Instance.RebindX();
		}

		[ContextMenu("RebindX")]
		public void RebindX(){
			state = InputBindState.Binding;
			bindOperation = BindOperation.X_Button;
		}


		public static void RebindY(KeyMap mapIn){
			Instance.SetMaps(mapIn);
			Instance.RebindY();
		}

		[ContextMenu("RebindY")]
		public void RebindY(){
			state = InputBindState.Binding;
			bindOperation = BindOperation.Y_Button;
		}


		public static void RebindStart(KeyMap mapIn){
			Instance.SetMaps(mapIn);
			Instance.RebindStart();
		}

		[ContextMenu("RebindStart")]
		public void RebindStart(){
			state = InputBindState.Binding;
			bindOperation = BindOperation.Start_Button;
		}

		// Misc / Testing functions -------

		[ContextMenu("UnsetText")]
		public void UnsetText(){
			if(useBinderText) simpleText.Unset();
		}

		// Helper methods -------
		
		public bool hasFlag(BindOperation a, BindOperation b){
			return (a & b) != 0;
		}

		public BindOperation unsetFlag(BindOperation a, BindOperation b){
			return a & (~b);
		}
	}
}