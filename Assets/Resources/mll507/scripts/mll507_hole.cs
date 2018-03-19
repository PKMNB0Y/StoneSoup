using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class mll507_hole : Tile
{
	private bool been_added;
	private float jump_time;
	private int idx;

	private bool layer_set;
	
	
	public override void takeDamage(Tile tileDamagingUs, int damageAmount, DamageType damageType) {
		// We're indestructible, ignore all damage.
	}

	void Update()
	{
		if (!layer_set)
		{
			_sprite.sortingLayerID = SortingLayer.NameToID("Below Floor");
			layer_set = true;
		}
	}
	
	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		Tile other_tile = otherCollider.GetComponent<Tile>();
		if (!other_tile.hasTag(TileTags.Wall) && other_tile.tileName != "sand grenade")
		{

			
			if (!(Time.time - jump_time >= 1.5f))
			{
				print("too soon to jump again");
				return;
			}



			jump_time = Time.time;
			print("new jump time is ::  " + jump_time);
			
			int num_sands = GameManager.instance.sands.Count;
			
			if (num_sands == 0)
			{
				Debug.Log("no holes");
				return;
			}
			
			Debug.Log("teleporting zoom !!!");
			
			int sand_idx = Random.Range(0, num_sands);
			mll507_sand other_sand = GameManager.instance.sands[sand_idx];
			//other_hole._collider.enabled = false;
			
			Vector3 to_move_to = other_sand.transform.position;
			other_tile.transform.SetPositionAndRotation(to_move_to, Quaternion.identity);
			
			//other_tile.addForce(expel_force);
			
		}
	}
}
