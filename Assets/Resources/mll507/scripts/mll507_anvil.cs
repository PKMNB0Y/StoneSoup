using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mll507_anvil : Tile {

	private float standTime;
	private bool triggered = false;
	private int healIndex = 0;
	private int offset;

	public int amountCanHeal;

	private Tile tileOnMe;

	void FixedUpdate()
	{
		if (triggered)
		{
			if ((Time.time + offset) - standTime >= healIndex + 1)
			{
				if (tileOnMe.tileWereHolding != null)
				{
					tileOnMe.tileWereHolding.health++;
					healIndex++;
				}
			}

			if (healIndex >= amountCanHeal)
			{
				die();
			}
		}
		
		
	}

	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		tileOnMe = otherCollider.GetComponent<Tile>();
		if (tileOnMe != null && tileOnMe.hasTag(TileTags.Player))
		{
			triggered = true;
			standTime = Time.time;
			offset = amountCanHeal - (amountCanHeal - healIndex);
		}
	}

	void OnTriggerExit2D(Collider2D otherCollider)
	{
		triggered = false;
		standTime = 0;
	}
}
