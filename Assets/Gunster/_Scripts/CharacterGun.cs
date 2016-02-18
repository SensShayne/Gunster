using UnityEngine;
using System.Collections;

public class CharacterGun : MonoBehaviour 
{
	[SerializeField] Bullet _bullet;
	[SerializeField] Transform _bulletSpawn;
	[SerializeField] float _timeBetweenShoot;
	[SerializeField] float _reloadTime;
	[SerializeField] uint _initialAmmo;

	float _shootTimer = 0.0f;
	uint _remainAmmo;


	// unity functions ---------------------------------------------------
	void Start()
	{
		_remainAmmo = _initialAmmo;
	}


	// public functions ---------------------------------------------------
	public void SetShootAngle (float shootAngle, CharacterPose characterPose)
	{
		float revisionAngle;

		switch (characterPose)
		{
			case CharacterPose.CROWL:
				{
					revisionAngle = Mathf.Clamp (shootAngle, 0.0f-Character.CROWL_POSE_ANGLE_RANGE, 0.0f+Character.CROWL_POSE_ANGLE_RANGE);
				}
				break;
			default:
				{
					revisionAngle = Mathf.Clamp (shootAngle, -90.0f, 90.0f);
				}
				break;
		}
				
		transform.rotation = Quaternion.Euler(new Vector3(0,0,revisionAngle));
	}

	public void Shoot()
	{
		if (Time.time > _shootTimer)
		{
			_shootTimer = Time.time + _timeBetweenShoot;

			Instantiate (_bullet, _bulletSpawn.position, _bulletSpawn.rotation);
		}
	}


	// animation event ---------------------------------------------------

}
