using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mll507_small_wall : Tile {

	// Walls only take explosive damage.
	public override void takeDamage(Tile tileDamagingUs, int amount, DamageType damageType) {
		if (damageType == DamageType.Normal) {
			base.takeDamage(tileDamagingUs, amount, damageType);
		}
	}

	void Update()
	{
		foreach (Tile tile in transform.parent.GetComponentsInChildren<Tile>())
		{
			if (tile.hasTag(TileTags.CanBeHeld))
			{
				Physics2D.IgnoreCollision(tile.GetComponent<BoxCollider2D>(), _collider, true);
			}
		}
	}
}
