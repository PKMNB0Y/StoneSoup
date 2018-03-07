using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slippery_tile : Tile {
	
	//plp248

	public float slipForce = 100;
	public Vector2 slipDir;
	
	public float dampingConstant = 10f;

	protected List<Tile> _slipperyObjects = new List<Tile>(10);
	
	void FixedUpdate()
	{	
		foreach (Tile slippingTile in _slipperyObjects)
		{
			slippingTile.addForce(slipDir * slipForce);
			Rigidbody2D tileBody = slippingTile.GetComponent<Rigidbody2D>();
			tileBody.AddForce(dampingConstant * tileBody.velocity);
		}
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Tile slippingTile = other.GetComponent<Tile>();
		slipDir = new Vector2(Random.Range(-1,1), Random.Range(-1,1));

		if (slippingTile != null )
		{
			_slipperyObjects.Add(slippingTile);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		_slipperyObjects.Remove(other.GetComponent<Tile>());
	}
}
