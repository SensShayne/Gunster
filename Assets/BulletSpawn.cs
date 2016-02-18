using UnityEngine;
using System.Collections;

public class BulletSpawn : MonoBehaviour 
{
	// unity functions ---------------------------------------------------



	// public functions ---------------------------------------------------
	public void SetShootAngle (float shootAngle, CharacterPose characterPose)
	{
		float revisionAngle = 0.0f;

		switch (characterPose)
		{
			case CharacterPose.CROWL:
				{
					if (shootAngle > 0)
					{
						revisionAngle = Mathf.Clamp (shootAngle, 90.0f-Character.CROWL_POSE_ANGLE_RANGE, 90.0f+Character.CROWL_POSE_ANGLE_RANGE);
					}
					else if (shootAngle < 0)
					{
						revisionAngle = Mathf.Clamp (shootAngle, -90.0f-Character.CROWL_POSE_ANGLE_RANGE, -90.0f+Character.CROWL_POSE_ANGLE_RANGE);
					}
				}
				break;
			default:
				{
					revisionAngle = shootAngle;
				}
				break;
		}
				
		transform.rotation = Quaternion.Euler(new Vector3(0,0,revisionAngle));
	}
}
