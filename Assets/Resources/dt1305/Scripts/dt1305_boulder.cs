﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dt1305_boulder : BasicAICreature {

	// How much force we inflict if something collides with us.
	public float damageForce = 1000;
	public int damageAmount = 1;


	// If we're chasing a friendly object, it'll be stored here.
	protected Tile _tileWereChasing = null;

	// How far the object we're chasing has to be before we stop chasing it. 
	public float maxDistanceToContinueChase = 12f;

	// When chasing we either
	// a. Choose our next target position once we reach our current one
	// or
	// b. Choose our next target position if we've gone too long without reaching our target position.
	// or
	// c. Recalculate our target position when we collide with something (not the target tile).
	protected float _timeSinceLastStep = 0f;


	void Update() {
		if (_tileWereChasing != null) {
			_timeSinceLastStep += Time.deltaTime;
			Vector2 targetGlobalPos = Tile.toWorldCoord(_targetGridPos.x, _targetGridPos.y);
			float distanceToTarget = Vector2.Distance(transform.position, targetGlobalPos);
			if (distanceToTarget <= GRID_SNAP_THRESHOLD || _timeSinceLastStep >= 2f) {
				takeStep();
			}
		}
		else {
			// Always lock to the position we're in when we're not chasing anything. 
			_targetGridPos = toGridCoord(globalX, globalY);
		}
		updateSpriteSorting();
	}

	protected override void takeStep() {
		_timeSinceLastStep = 0f;

		// First, figure out if the target is too far away
		float distanceToTile = Vector2.Distance(transform.position, _tileWereChasing.transform.position);
		if (distanceToTile > maxDistanceToContinueChase) {
			_tileWereChasing = null;
			return;
		}

		// We do this to re-calculate exactly where we are right now. 
		_targetGridPos = Tile.toGridCoord(globalX, globalY);

		_neighborPositions.Clear();

		// Otherwise, we're going to look at all potential neighbors and then figure out the best one to go to.
		Vector2 upGridNeighbor = new Vector2(_targetGridPos.x, _targetGridPos.y+1);
		if (pathIsClear(toWorldCoord(upGridNeighbor), CanOverlapIgnoreTargetTile)) {
			_neighborPositions.Add(upGridNeighbor);
		}
		Vector2 upRightGridNeighbor = new Vector2(_targetGridPos.x+1, _targetGridPos.y+1);
		if (pathIsClear(toWorldCoord(upRightGridNeighbor), CanOverlapIgnoreTargetTile)) {
			_neighborPositions.Add(upRightGridNeighbor);
		}
		Vector2 rightGridNeighbor = new Vector2(_targetGridPos.x+1, _targetGridPos.y);
		if (pathIsClear(toWorldCoord(rightGridNeighbor), CanOverlapIgnoreTargetTile)) {
			_neighborPositions.Add(rightGridNeighbor);
		}
		Vector2 downRightGridNeighbor= new Vector2(_targetGridPos.x+1, _targetGridPos.y-1);
		if (pathIsClear(toWorldCoord(downRightGridNeighbor), CanOverlapIgnoreTargetTile)) {
			_neighborPositions.Add(downRightGridNeighbor);
		}
		Vector2 downGridNeighbor = new Vector2(_targetGridPos.x, _targetGridPos.y-1);
		if (pathIsClear(toWorldCoord(downGridNeighbor), CanOverlapIgnoreTargetTile)) {
			_neighborPositions.Add(downGridNeighbor);
		}
		Vector2 downLeftGridNeighbor= new Vector2(_targetGridPos.x-1, _targetGridPos.y-1);
		if (pathIsClear(toWorldCoord(downLeftGridNeighbor), CanOverlapIgnoreTargetTile)) {
			_neighborPositions.Add(downLeftGridNeighbor);
		}
		Vector2 leftGridNeighbor = new Vector2(_targetGridPos.x-1, _targetGridPos.y);
		if (pathIsClear(toWorldCoord(leftGridNeighbor), CanOverlapIgnoreTargetTile)) {
			_neighborPositions.Add(leftGridNeighbor);
		}
		Vector2 upLeftGridNeighbor= new Vector2(_targetGridPos.x-1, _targetGridPos.y+1);
		if (pathIsClear(toWorldCoord(upLeftGridNeighbor), CanOverlapIgnoreTargetTile)) {
			_neighborPositions.Add(upLeftGridNeighbor);
		}

		// Now, of the neighbor positions, pick the one that's closest. 
		float minDistance = distanceToTile;
		Vector2 minNeighbor = _targetGridPos;
		GlobalFuncs.shuffle(_neighborPositions);
		foreach (Vector2 neighborPos in _neighborPositions) {
			float distanceFromTarget = Vector2.Distance(Tile.toWorldCoord(neighborPos.x, neighborPos.y), _tileWereChasing.transform.position);
			if (distanceFromTarget < minDistance) {
				minNeighbor = neighborPos;
				minDistance = distanceFromTarget;
			}
		}

		_targetGridPos = minNeighbor;
	}


	// Colliding with a friendly should hurt it.
	void OnCollisionEnter2D(Collision2D collision) {
		Tile otherTile = collision.gameObject.GetComponent<Tile>();

		// If we're chasing something, then take a step probably
		if (otherTile != _tileWereChasing && _tileWereChasing != null
			&& otherTile != null && otherTile.hasTag(TileTags.Creature)) {
			//_moveCooldownTimer = 0.5f;
			takeStep();
		}

		if (otherTile != null && otherTile.hasTag(tagsWeChase)) {
			otherTile.takeDamage(this, damageAmount);
			Vector2 toOtherTile = (Vector2)otherTile.transform.position - (Vector2)transform.position;
			toOtherTile.Normalize();
			otherTile.addForce(damageForce*toOtherTile);
		}

	}

	void OnCollisionStay2D(Collision2D collision) {
		OnCollisionEnter2D(collision);
	}

//	public override void tileDetected(Tile otherTile) {
//		if (_tileWereChasing == null && otherTile.hasTag(tagsWeChase)) {
//			_tileWereChasing = otherTile;
//			takeStep();
//		}
//	}

	// For the purposes of chasing an object, we make a special CanOverlapFunc that ignores the tile we're chasing
	protected bool CanOverlapIgnoreTargetTile(RaycastHit2D hitResult) {
		Tile maybeResultTile = hitResult.transform.GetComponent<Tile>();
		if (maybeResultTile == _tileWereChasing) {
			return true;
		}
		return DefaultCanOverlapFunc(hitResult);
	}
	public override void takeDamage(Tile tileDamagingUs, int amount, DamageType damageType) {
		if (damageType == DamageType.Explosive) {
			base.takeDamage(tileDamagingUs, amount, damageType);
		}
	}
}
