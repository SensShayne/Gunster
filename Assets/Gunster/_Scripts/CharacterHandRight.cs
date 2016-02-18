using UnityEngine;
using System.Collections;

public class CharacterHandRight : MonoBehaviour 
{



	// unity functions ---------------------------------------------------



	// public functions ---------------------------------------------------
	public void SetShootAngle (float shootAngle, CharacterPose characterPose)
	{
		float revisionAngle;
		float revisionOffset = 0.3f * shootAngle;

		switch (characterPose)
		{
			case CharacterPose.CROWL:
				{
					revisionAngle = Mathf.Clamp (shootAngle+revisionOffset, 0.0f-Character.CROWL_POSE_ANGLE_RANGE, 0.0f+Character.CROWL_POSE_ANGLE_RANGE);
				}
				break;
			default:
				{
					revisionAngle = Mathf.Clamp (shootAngle+revisionOffset, -90.0f, 90.0f);
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
		transform.rotation = Quaternion.Euler(new Vector3(0,0,-55));
	}

	public void OnAnimation_SkyDeathFront (int keyFrame)
	{
		switch (keyFrame)
		{
			case 3:
				{
					transform.rotation = Quaternion.Euler(new Vector3(0,0,-25));
				}
				break;
			case 4:
				{
					transform.rotation = Quaternion.Euler(new Vector3(0,0,-25));
				}
				break;
			case 5:
				{
					transform.rotation = Quaternion.Euler(new Vector3(0,0,-25));
				}
				break;
			case 6:
				{
					transform.rotation = Quaternion.Euler(new Vector3(0,0,-114));
				}
				break;
		}
	}

	public void OnAnimation_SkyDeathBack (int keyFrame)
	{
		transform.rotation = Quaternion.Euler(new Vector3(0,0,-40));
	}
}
