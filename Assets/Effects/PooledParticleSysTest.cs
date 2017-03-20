using UnityEngine;
using System.Collections;

namespace Softdrink{
	[AddComponentMenu("Scripts/Effects/Pooled Particle System Test")]
	public class PooledParticleSysTest : MonoBehaviour {

		private Transform self;

		public PooledParticleSystem _pps;

		public Vector3 direction = new Vector3(0f,0f,-1f);

		public float procTime = 3.0f;

		private float progress = 0f;

		void Awake(){
			self = gameObject.transform;
		}

		void Update () {
			progress += Time.deltaTime;

			if(progress >= procTime){
				_pps.Fire(self.position,  direction);
				progress = 0f;
			}
		}

	}
}