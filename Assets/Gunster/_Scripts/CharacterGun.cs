using UnityEngine;
using System.Collections;

public class CharacterGun : CharacterParts 
{
	[SerializeField] Bullet _bullet;
	//[SerializeField] BulletSpawn _bulletSpawn;
	[SerializeField] Transform _bulletSpawn;

	[SerializeField] float _timeBetweenShoot;
	[SerializeField] float _reloadTime;
	[SerializeField] uint _initialAmmo;

	[SerializeField] uint _leftHandSpriteIndex;
	[SerializeField] uint _rightHandSpriteIndex;

	float _shootTimer = 0.0f;
	uint _remainAmmo;


	// unity functions ---------------------------------------------------
	void Start()
	{
		defaultSpriteAngle = -90.0f;

		_remainAmmo = _initialAmmo;

		// search hand
		transform.parent.parent.Find ("Hand Left").
		gameObject.GetComponent<CharacterHandLeft>().ChangeSprite((CharacterHandLeftSprite)_leftHandSpriteIndex);
		transform.parent.parent.Find ("Hand Right").
		gameObject.GetComponent<CharacterHandRight>().ChangeSprite((CharacterHandRightSprite)_rightHandSpriteIndex);
	}


	// public functions ---------------------------------------------------
	public override void LookAt (Vector3 sourcePostion, Vector3 targetPosition, CharacterPose characterPose)
	{
		//_bulletSpawn.LookAt (transform.position, targetPosition, characterPose);

		const float bulletSpawnDefaultAngle = 0.0f;

		float lookAngle = -Utility.GetAngle (sourcePostion, targetPosition) + bulletSpawnDefaultAngle;

		float revisionAngle = 0.0f;

		switch (characterPose)
		{
			case CharacterPose.CROWL:
				{
					bool isCharacterFlip = targetPosition.x > transform.parent.parent.transform.position.x;
					if (!isCharacterFlip)
					{
						revisionAngle = Mathf.Clamp (lookAngle, 
							Utility.CalcDefaultLeftAngle (bulletSpawnDefaultAngle)-Character.CROWL_POSE_ANGLE_RANGE, 
							Utility.CalcDefaultLeftAngle (bulletSpawnDefaultAngle)+Character.CROWL_POSE_ANGLE_RANGE);
					}
					else if (isCharacterFlip)
					{
						revisionAngle = Mathf.Clamp (lookAngle, 
							Utility.CalcDefaultRightAngle (bulletSpawnDefaultAngle)-Character.CROWL_POSE_ANGLE_RANGE, 
							Utility.CalcDefaultRightAngle (bulletSpawnDefaultAngle)+Character.CROWL_POSE_ANGLE_RANGE);
					}
					Debug.Log (isCharacterFlip);
				}
				break;
			default:
				{
					revisionAngle = lookAngle;
				}
				break;
		}

		//_bulletSpawn.transform.rotation = Quaternion.Euler(new Vector3(0,0,revisionAngle));
		_bulletSpawn.rotation = Quaternion.Euler(new Vector3(0,0,revisionAngle));
	}

	public void Shoot()
	{
		if (Time.time > _shootTimer)
		{
			_shootTimer = Time.time + _timeBetweenShoot;

			//Instantiate (_bullet, _bulletSpawn.transform.position, _bulletSpawn.transform.rotation);
			Instantiate (_bullet, _bulletSpawn.position, _bulletSpawn.rotation);
		}
	}


	// animation event ---------------------------------------------------

}
