using UnityEngine;
using System.Collections;

namespace Softdrink{
	// Pooled Particle System
	// 'Recycles' a single particle system to produce all effects of a given type
	// Moves the particle system emitter each time Fire() is called, and fires a burst
	// The particle system must be in World Space mode to use this properly!
	// [ExecuteInEditMode]
	[AddComponentMenu("Scripts/Effects/Pooled Particle System")]
	public class PooledParticleSystem : MonoBehaviour {

		[SerializeField]
		[TooltipAttribute("Should the PooledParticleSystem ignore timescale-based updates?")]
		private bool ignoreTimescale = true;

		[SerializeField]
		private int effectLength = 60;

		// Internal reference to the Transform
		private Transform self;

		// Internal reference to the Particle System in question
		private ParticleSystem[] _ps;
		

		// Internal reference to the emission range
		private int minCount = 0;
		private int maxCount = 0;

		void Awake () {
			// Assign self Transform
			self = gameObject.GetComponent<Transform>() as Transform;
			if(self == null) Debug.LogError("ERROR! The PooledParticleSystem was unable to associate a reference to the attached Transform!", this);

			// Get referneces to the particle systems
			_ps = gameObject.GetComponentsInChildren<ParticleSystem>() as ParticleSystem[];
			if(_ps == null || _ps.Length == 0) Debug.LogError("The PooledParticleSystem could not associate a reference to the ParticleSystems in the children.", this);

			ParticleSystem.EmissionModule[] _em;
			//ParticleSystem.MainModule[] _main;
			ParticleSystem.MinMaxCurve[] _time;
			_em = new ParticleSystem.EmissionModule[_ps.Length];
			//_main = new ParticleSystem.EmissionModule[_ps.Length];
			_time = new ParticleSystem.MinMaxCurve[_ps.Length];
			float effectLengthTemp = 0f;
			

			// Ge the emission modules from the particle systems
			for(int i = 0; i < _ps.Length; i++){
				//_ps[i].emission.enabled = false;
				//if(_ps[i] == null) Debug.LogError("Particle System could not be associated.", this);
				_em[i] = _ps[i].emission;
				//_main[i] = _ps[i].main;
				_time[i] = _ps[i].startLifetime;
				if(_time[i].constantMax > effectLengthTemp) effectLengthTemp = _time[i].constantMax;
				if(_time[i].constant > effectLengthTemp) effectLengthTemp = _time[i].constant;
				//_em[i].enabled = false;

				if(i == 0){
					ParticleSystem.Burst[] _bursts = new ParticleSystem.Burst[_em[i].burstCount];
					_em[i].GetBursts(_bursts);
					minCount = _bursts[0].minCount;
					maxCount = _bursts[0].maxCount;
				}
			}

			effectLength = (int)(effectLengthTemp * Application.targetFrameRate);
		}

		public void UnscaledUpdate(){
			// Debug.Log("Updating particle system");
			for(int i = 0; i < _ps.Length; i++){
				_ps[i].Simulate(Time.unscaledDeltaTime, false, false, false);
			}
		}

		public bool GetIgnoreTimescale(){
			return ignoreTimescale;
		}

		public int GetEffectLength(){
			return effectLength;
		}
		
		//private float progress = 0f;

		// void Update () {
		// 	progress += Time.deltaTime;

		// 	if(progress >= 3.0f){
		// 		Fire();
		// 		progress = 0f;
		// 	}
		// }

		private int emitCount = 0;

		// Fire the particle system
		void Fire(){
			for(int i = 0; i < _ps.Length; i++){
				emitCount = (int)(Random.Range(minCount, maxCount));
				_ps[i].Emit(emitCount);
			}

		}

		// Reposition the system and rotate it to match a specified direction, then fire the system
		public void Fire(Vector3 position, Vector3 dir){
			self.position = new Vector3(position.x, position.y, position.z);

			self.LookAt(new Vector3(position.x + dir.x, position.y + dir.y, position.z + dir.z));

			for(int i = 0; i < _ps.Length; i++){
				emitCount = (int)(Random.Range(minCount, maxCount));
				_ps[i].Emit(emitCount);
				_ps[i].Play();
			}

		}

	}

}
