using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LookAt : MonoBehaviour {

	public Transform target;
	private Transform self;

	void Awake(){
		self = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		self.LookAt(target, Vector3.up);
	}
}
