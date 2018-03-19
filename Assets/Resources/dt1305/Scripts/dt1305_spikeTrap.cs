using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dt1305_spikeTrap : Tile {

	public GameObject spikeTrapSprung;
	public float startUpTime;
	public float timeStamp;
	public bool sprung;
	public bool spawned;

	// Use this for initialization
	void Start () {
		sprung = false;
		spawned = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (sprung == true && spawned == false && timeStamp < Time.timeSinceLevelLoad) {
			Instantiate (spikeTrapSprung, transform.position, Quaternion.identity);
			spawned = true;
			die ();
		}
	}
	void OnTriggerEnter2D(Collider2D otherObject){
		if (otherObject.gameObject.name.Contains ("player") && sprung == false) {
			GetComponent<AudioSource> ().Play ();
			timeStamp = Time.timeSinceLevelLoad + startUpTime;
			sprung = true;
		}
	}
}
