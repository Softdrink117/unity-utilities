using UnityEngine;
using System.Collections;

namespace Softdrink{
	// Enum for determinind axis listening mode
	public enum AxisListeningMode{
		Interpolated,
		Raw,
		InterpolatedDelta,
		RawDelta,
	}

	[CreateAssetMenu(menuName = "Input/InputAxisListener Settings")]
	public class InputAxisListenerSettings : ScriptableObject {

		[HeaderAttribute("Virtual Controller Settings")]

		[TooltipAttribute("The prepend for virtual Controller names in the Unity Project Settings > Input tab, where the format is 'prepend # intersperse #'.")]
		public string controllerNamePrepend = "J";

		[TooltipAttribute("The intersperse for virtual Controller names in the Unity Project Settings > Input tab, where the format is 'prepend # intersperse #'.")]
		public string controllerNameIntersperse = "A";

		[TooltipAttribute("How many virtual Controllers have been defined? \nThis number MUST be less than or equal to the number of defined virtual Controllers in the Unity Project Settings > Input tab.")]
		public int definedVirtualControllers = 2;

		[TooltipAttribute("How many virtual Axes have been defined per virtual Controller? \nThis MUST be less than or equal to the number of Axes defined per Controller in the Unity Project Settings > Input tab. \nNote that a typical controller will use 2 axes per analog thumbstick or POV hat, and they are usually counted in X1Y1X2Y2... order. \nFor left and right thumbstick input per controller, use a value of 4, and define 4 virtual axes per Controller in the Input Settings tab.")]
		public int axesPerController = 2;

		[HeaderAttribute("Listener Settings")]

		[Range(0f, 1f)]
		[TooltipAttribute("Deadzone for Axis movement when Listening. \nThis should be large, to avoid any accidental movement bindings.")]
		public float deadzone = 0.5f;

		[TooltipAttribute("Whether the Axis Listener should check interpolated or raw Axis data.\nInterpolatedDelta and RawDelta are similar to KeyDown / KeyUp events - they only return when there has been a change since the last frame.")]
		public AxisListeningMode mode = AxisListeningMode.InterpolatedDelta;


		public void setDefinedVirtualControllers(int input){
			definedVirtualControllers = input;
		}

		public void setAxesPerController(int input){
			axesPerController = input;
		}
				
	}
}