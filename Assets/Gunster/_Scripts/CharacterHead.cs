using UnityEngine;
using System.Collections;

public class CharacterHead : CharacterParts 
{


	// unity functions ---------------------------------------------------
	void Start()
	{
		defaultSpriteAngle = -90.0f;
	}


	// public functions --------------------------------------------------
	public override void LookAt (Vector3 sourcePostion, Vector3 targetPosition, CharacterPose characterPose)
	{
		float lookAngle = Mathf.Abs(Utility.GetAngle (sourcePostion, targetPosition, true, false)) + defaultSpriteAngle;

		float revisionAngle;

		switch (characterPose)
		{
			case CharacterPose.CROWL:
				{
					revisionAngle = Mathf.Clamp (lookAngle, 
						Utility.CalcDefaultLeftAngle (defaultSpriteAngle)-Character.CROWL_POSE_ANGLE_RANGE, 
						Utility.CalcDefaultLeftAngle (defaultSpriteAngle)+Character.CROWL_POSE_ANGLE_RANGE);
				}
				break;
			default:
				{
					revisionAngle = Mathf.Clamp (lookAngle, 
						Utility.CalcDefaultUpAngle (defaultSpriteAngle), 
						Utility.CalcDefaultDownAngle (defaultSpriteAngle) -60.0f);
				}
				break;
		}
				
		transform.rotation = Quaternion.Euler(new Vector3(0,0,revisionAngle));
	}


	// private functions -------------------------------------------------



	// animation event ---------------------------------------------------
	public void OnAnimation_DeathFront (int keyFrame)
	{
		transform.rotation = Quaternion.Euler(new Vector3(0,0,-90));
	}

	public void OnAnimation_DeathBack (int keyFrame)
	{
		transform.rotation = Quaternion.Euler(new Vector3(0,0,90));
	}

	public void OnAnimation_SkyDeathFront (int keyFrame)
	{
		switch (keyFrame)
		{
			case 3:
				{
					transform.rotation = Quaternion.Euler(new Vector3(0,0,-30));
				}
				break;
			case 4:
				{
					transform.rotation = Quaternion.Euler(new Vector3(0,0,-90));
				}
				break;
			case 5:
				{
					transform.rotation = Quaternion.Euler(new Vector3(0,0,-90));
				}
				break;
			case 6:
				{
					transform.rotation = Quaternion.Euler(new Vector3(0,0,-120));
				}
				break;
		}
	}

	public void OnAnimation_SkyDeathBack (int keyFrame)
	{
		transform.rotation = Quaternion.Euler(new Vector3(0,0,65));
	}
}
