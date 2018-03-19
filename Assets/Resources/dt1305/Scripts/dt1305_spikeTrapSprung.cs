using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dt1305_spikeTrapSprung : Tile {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnCollisionEnter2D(Collision2D otherObject){
		if (otherObject.gameObject.GetComponent<Tile> ().hasTag(TileTags.Creature) || otherObject.gameObject.GetComponent<Tile> ().hasTag(TileTags.Wall)) {
			if (otherObject.gameObject.name.Contains ("wall")) {
				return;
			} else {
				otherObject.gameObject.GetComponent<Tile> ().takeDamage (this, 5);
				die ();
			}
		}
	}
}
