using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dt1305_chest : Tile {
	public GameObject[] spawnableObjects;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnCollisionEnter2D(Collision2D otherObject){
		if (otherObject.gameObject.name.Contains ("player")) {
			int randomIndex = Random.Range (0, spawnableObjects.Length);
			GameObject newObject = Instantiate(spawnableObjects[randomIndex], transform.position, Quaternion.identity);
			newObject.GetComponent<Tile> ().init ();
			Debug.Log (newObject.gameObject.name);
			Debug.Log (spawnableObjects [randomIndex].name);
			die ();
		}
	}
	public override void takeDamage(Tile tileDamagingUs, int amount, DamageType damageType) {
		if (damageType == DamageType.Explosive) {
			base.takeDamage(tileDamagingUs, amount, damageType);
		}
	}
}
