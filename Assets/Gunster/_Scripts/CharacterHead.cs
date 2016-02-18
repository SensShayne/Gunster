using UnityEngine;
using System.Collections;

public class CharacterHead : MonoBehaviour 
{

	// unity functions ---------------------------------------------------



	// public functions --------------------------------------------------
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
					revisionAngle = Mathf.Clamp (shootAngle, -90.0f, 30.0f);
				}
				break;
		}
				
		transform.rotation = Quaternion.Euler(new Vector3(0,0,revisionAngle));
	}


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
