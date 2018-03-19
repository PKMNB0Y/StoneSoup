using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dt1305_pellet : Tile {
	//public ParticleSystem smokeTrail;

	public float damageThreshold = 14;

	public float onGroundThreshold = 1f;

	protected float _destroyTimer = 0.5f;

	protected ContactPoint2D[] _contacts = null;
	// Use this for initialization
	void Start () {
		GetComponent<TrailRenderer>().Clear();
		//Instantiate (smokeTrail, transform.position, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.GetComponent<Tile> () != null) {
			if (collider.gameObject.name.Contains ("pellet") || collider.gameObject.name.Contains("Trap") || collider.gameObject.GetComponent<Tile> ().hasTag (TileTags.CanBeHeld)) {
				return;
			} else if (collider.gameObject.name.Contains("bush")){
				if (collider.gameObject.GetComponent<dt1305_bushMonster> ().vulnerable == false) {
					return;
				}
			}
//			else if (collider.gameObject.GetComponent<Tile> ().hasTag (TileTags.CanBeHeld) == false) {
//				collider.gameObject.GetComponent<Tile> ().takeDamage (this, 1);
//				die ();
//			}
			Tile otherTile = collider.gameObject.GetComponent<Tile>();
			otherTile.takeDamage(this, 1);
			die ();
		}
	}
}
