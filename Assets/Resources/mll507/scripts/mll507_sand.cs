using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mll507_sand : Tile
{

	public int move_speed_cut;
	private float original_speed;

	private bool been_added;

	void Update()
	{
		if (!been_added)
		{
			transform.parent.gameObject.GetComponent<mll507_room>().sands.Add(this);
			been_added = true;
		}
	}
	
	protected override void die()
	{
		transform.parent.gameObject.GetComponent<mll507_room>().sands.Remove(this);
		
		_alive = false;

		if (tileWereHolding != null) {
			tileWereHolding.dropped(this);
		}
		if (deathEffect != null) {
			Instantiate(deathEffect, transform.position, Quaternion.identity);
		}
		if (deathSFX != null) {
			AudioManager.playAudio(deathSFX);
		}

		Destroy(gameObject);
	}
	
	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		if (!otherCollider.gameObject.CompareTag("Respawn"))
		{
			Tile other_tile = otherCollider.GetComponent<Tile>();

			if (other_tile != null && other_tile.hasTag(TileTags.Player))
			{
				Player play = otherCollider.GetComponent<Player>();
				original_speed = play.moveSpeed;
				play.moveSpeed *= (1f/move_speed_cut);
			}
		}
		
	}

	void OnTriggerExit2D(Collider2D otherCollider)
	{

		if (!otherCollider.gameObject.CompareTag("Respawn"))
		{
			Tile other_tile = otherCollider.GetComponent<Tile>();

			if (other_tile != null && other_tile.hasTag(TileTags.Player))
			{
				Player play = otherCollider.GetComponent<Player>();
				play.moveSpeed = original_speed;
				takeDamage(other_tile, 1);
			} else if (other_tile != null && other_tile.hasTag(TileTags.Creature))
			{
				takeDamage(other_tile, 1);
			}
		}
		
	}
}
