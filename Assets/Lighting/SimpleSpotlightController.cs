using UnityEngine;
using System.Collections;

namespace Softdrink{
	[ExecuteInEditMode]
	[RequireComponent(typeof(LineRenderer))]
	[RequireComponent(typeof(Light))]
	[AddComponentMenu("Scripts/Effects/Simple Spot Light")]
	public class SimpleSpotlightController : MonoBehaviour {

		[SerializeField]
		[TooltipAttribute("The damping time, in Seconds, over which rotation is interpolated.")]
		private float damping = 1f;

		[SerializeField]
		[TooltipAttribute("The Transform of the Target Object.")]
		private Transform target;

		[SerializeField]
		[TooltipAttribute("Scalar for the beam end width. This allows for control when using Cookies to define the shape of the spotlight.")]
		private float beamEndScalar = 0.55f;

		[SerializeField]
		[TooltipAttribute("Scalar for the beam falloff distance relative to the lookTarget distance.")]
		private float beamFalloffScalar = 1f;

		private Transform self;

		private Quaternion targetRotation;

		// Internal reference to LineRenderer on
		private LineRenderer _lr;
		private Light _light;

		void Awake(){
			GetReferences();
		}

		void GetReferences(){
			_lr = gameObject.GetComponent<LineRenderer>() as LineRenderer;
			_light = gameObject.GetComponent<Light>() as Light;
			self = gameObject.GetComponent<Transform>() as Transform;

			if(_light == null || _lr == null || self == null) Debug.Log("SimpleSpotlightController could not associate all of its references!", this);

			_lr.SetPosition(0, self.position);

		}
		
		// Update is called once per frame
		void Update () {
			if(_light == null || _lr == null || self == null) GetReferences();
			if(target == null) return;
			// self.LookAt(target, Vector3.up);
			
			RotateLight();
			SetBeam();
		}

		void RotateLight(){
			targetRotation = Quaternion.LookRotation(target.position - self.position);
			self.rotation = Quaternion.Slerp(self.rotation, targetRotation, Time.unscaledDeltaTime / damping);

			#if UNITY_EDITOR
			if(!UnityEditor.EditorApplication.isPlaying) self.LookAt(target, Vector3.up);
			#endif
		}

		private float beamEndWidth = 0f;
		void SetBeam(){
			_lr.SetPosition(0, self.position);

			_lr.SetPosition(1, self.TransformPoint(Vector3.forward * GetTargetDistance() * beamFalloffScalar));


			beamEndWidth =  GetTargetDistance() * Mathf.Tan(_light.spotAngle/2.0f);
			if(beamEndWidth < 0) beamEndWidth *= -1f;
			beamEndWidth *= beamEndScalar * beamFalloffScalar;

			_lr.SetWidth(0f, beamEndWidth);

		}

		public float GetTargetDistance(){
			return Vector3.Distance(self.position, target.position);
		}
	}

}