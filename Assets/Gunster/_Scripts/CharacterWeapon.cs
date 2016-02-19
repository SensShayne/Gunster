using UnityEngine;
using UnityEditor;
using System.Collections;

public class CharacterWeapon : CharacterParts 
{
	CharacterGun _gun;

	// unity functions ---------------------------------------------------
	void Start()
	{
		defaultSpriteAngle = -90.0f;

		//SetWeapon ("AK-47");
		SetWeapon ("Rocket Launcher");
	}

	// public functions --------------------------------------------------
	public override void LookAt (Vector3 sourcePostion, Vector3 targetPosition, CharacterPose characterPose)
	{
		base.LookAt (sourcePostion, targetPosition, characterPose);

		_gun.LookAt (sourcePostion, targetPosition, characterPose);
	}

	public void Shoot ()
	{
		_gun.Shoot ();
	}

	// private functions -------------------------------------------------
	void SetWeapon (string weapon)
	{
		_gun = Instantiate (Resources.Load ("_Prefabs/Weapon/" + weapon, typeof(CharacterGun))) as CharacterGun;
		_gun.transform.SetParent (transform, false);
	}
}
