using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dt1305_bushMonster : Tile {
	public Rigidbody2D rb2d;
	public float sightRange;
	public float criticalRange;
	public float movSp;
	public float closingSp;
	public GameObject player;
	public bool monster;
	public bool vulnerable;
	RaycastHit2D ray;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		player = GameObject.Find ("player_tile(Clone)");
		vulnerable = false;
		float monsterVal = Random.value;
		if (monsterVal > 0.6f) {
			monster = true;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (monster == true) {
			Vector2 direction = ((Vector2)player.GetComponent<Transform> ().position - (Vector2)transform.position).normalized;
			Vector2 oppositeDirection = ((Vector2)transform.position - (Vector2)player.GetComponent<Transform> ().position).normalized;
			ray = Physics2D.Raycast (transform.position, direction);
			Debug.DrawRay (transform.position, direction * sightRange, Color.yellow);
//			if (ray.collider.gameObject != null) {
//				Debug.Log (ray.collider.gameObject.name);
//			}
			if (ray.collider.gameObject.name == "player_tile(Clone)") {
				if (ray.distance <= sightRange) {
					if (ray.distance <= criticalRange) {
						vulnerable = true;
						rb2d.MovePosition (rb2d.position + new Vector2 (direction.x * closingSp, direction.y * closingSp) * Time.fixedDeltaTime);
					} else {
						vulnerable = false;
						rb2d.MovePosition (rb2d.position + new Vector2 (direction.x * movSp, direction.y * movSp) * Time.fixedDeltaTime);
					}
				}
			}
		}
	}
	void OnTriggerEnter2D(Collider2D otherObject){
		if (otherObject.gameObject.name == "player_tile(Clone)" && monster == true) {
			otherObject.gameObject.GetComponent<Tile> ().takeDamage (this, 5);
		}
	}
}
