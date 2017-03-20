using UnityEngine;
using System.Collections;

namespace Softdrink{
	[ExecuteInEditMode]
	public class DampedLookAt : MonoBehaviour {

		[SerializeField]
		[TooltipAttribute("The damping time, in Seconds, over which rotation is interpolated.")]
		private float damping = 1f;

		[SerializeField]
		[TooltipAttribute("The Transform of the Target Object.")]
		private Transform target;

		private Transform self;

		private Quaternion targetRotation;

		void Awake(){
			self = gameObject.transform;
		}
		
		// Update is called once per frame
		void Update () {
			if(self == null) self = gameObject.transform;
			if(target == null) return;
			// self.LookAt(target, Vector3.up);
			targetRotation = Quaternion.LookRotation(target.position - self.position);
			self.rotation = Quaternion.Slerp(self.rotation, targetRotation, Time.unscaledDeltaTime / damping);
		}

		public float GetTargetDistance(){
			return Vector3.Distance(self.position, target.position);
		}
	}

}