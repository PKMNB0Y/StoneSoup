using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dt1305_shotgun : Tile {

	public GameObject bulletPrefab;
	public GameObject muzzleFlashObj;
	public GameObject smokeParticles;

	public float recoilForce;
	public float shootForce;



	public float cooldownTime;

	protected float _cooldownTimer;

	protected void aim() {
		_sprite.transform.localPosition = new Vector3(1f, 0, 0);
		float aimAngle = Mathf.Atan2(_tileHoldingUs.aimDirection.y, _tileHoldingUs.aimDirection.x)*Mathf.Rad2Deg;
		transform.localRotation = Quaternion.Euler(0, 0, aimAngle);
		if (_tileHoldingUs.aimDirection.x < 0) {
			_sprite.flipY = true;
			muzzleFlashObj.transform.localPosition = new Vector3(muzzleFlashObj.transform.localPosition.x, -Mathf.Abs(muzzleFlashObj.transform.localPosition.y), muzzleFlashObj.transform.localPosition.z);
		}
		else {
			_sprite.flipY = false;
			muzzleFlashObj.transform.localPosition = new Vector3(muzzleFlashObj.transform.localPosition.x, Mathf.Abs(muzzleFlashObj.transform.localPosition.y), muzzleFlashObj.transform.localPosition.z);
		}
	}

	protected virtual void Update() {
		if (_cooldownTimer > 0) {
			_cooldownTimer -= Time.deltaTime;
		}

		if (_tileHoldingUs != null) {
			// If we're held, rotate and aim the gun.
			aim();
		}
		else {
			// Otherwise, move the gun back to the normal position. 
			_sprite.transform.localPosition = Vector3.zero;
			transform.rotation = Quaternion.identity;
		}
		updateSpriteSorting();
	}

	public override void useAsItem(Tile tileUsingUs) {
		if (_cooldownTimer > 0) {
			return;
		}

		// First, make sure we're aimed properly (to avoid shooting ourselves by accident)
		aim();

		if (muzzleFlashObj != null) {
			muzzleFlashObj.SetActive (true);
			Invoke ("deactivateFlash", 0.1f);
		}
		tileUsingUs.addForce(-recoilForce*tileUsingUs.aimDirection.normalized);

		GetComponent<AudioSource> ().Play ();

		// Let's spawn the bullet. The bullet will probably need to be a child of the room. 
		for (int i = 0; i < 12; i++) {

			Vector3 bulletRot = new Vector3(0, 0, (Mathf.Atan2(tileUsingUs.aimDirection.y, tileUsingUs.aimDirection.x) * Mathf.Rad2Deg) + Random.Range(-i * 2f, i * 2f));
			Vector2 bulletDir = new Vector2 (Mathf.Cos (bulletRot.z * Mathf.Deg2Rad), Mathf.Sin (bulletRot.z * Mathf.Deg2Rad));

			GameObject newBullet = Instantiate(bulletPrefab);
			newBullet.transform.parent = tileUsingUs.transform.parent;
			newBullet.transform.position = muzzleFlashObj.transform.position;
			newBullet.transform.rotation = transform.rotation;


			newBullet.GetComponent<Tile>().init();
			newBullet.GetComponent<Tile>().addForce(bulletDir.normalized * shootForce);
		}

		_cooldownTimer = cooldownTime;

		smokeParticles.GetComponent<ParticleSystem> ().Play ();
	}

	public void deactivateFlash() {
		muzzleFlashObj.SetActive(false);
	}
}
