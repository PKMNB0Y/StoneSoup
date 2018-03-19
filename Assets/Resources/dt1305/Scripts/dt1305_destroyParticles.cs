using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dt1305_destroyParticles : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<ParticleSystem> ().isPlaying == false) {
			Destroy (gameObject);
		}
	}
}
