using UnityEngine;
using System.Collections;

public class BulletSpawn : CharacterParts 
{
	// unity functions ---------------------------------------------------
	void Start()
	{
		defaultSpriteAngle = 0.0f;
	}


	// public functions ---------------------------------------------------
//	public override void LookAt (Vector3 sourcePostion, Vector3 targetPosition, CharacterPose characterPose)
//	{
//		float lookAngle = -Utility.GetAngle (sourcePostion, targetPosition) + defaultSpriteAngle;
//
//		float revisionAngle = 0.0f;
//
//		switch (characterPose)
//		{
//			case CharacterPose.CROWL:
//				{
//					if (lookAngle > 0)
//					{
//						revisionAngle = Mathf.Clamp (lookAngle, 
//							Utility.CalcDefaultLeftAngle (defaultSpriteAngle)-Character.CROWL_POSE_ANGLE_RANGE, 
//							Utility.CalcDefaultLeftAngle (defaultSpriteAngle)+Character.CROWL_POSE_ANGLE_RANGE);
//					}
//					else if (lookAngle < 0)
//					{
//						revisionAngle = Mathf.Clamp (lookAngle, 
//							Utility.CalcDefaultRightAngle (defaultSpriteAngle)-Character.CROWL_POSE_ANGLE_RANGE, 
//							Utility.CalcDefaultRightAngle (defaultSpriteAngle)+Character.CROWL_POSE_ANGLE_RANGE);
//					}
//				}
//				break;
//			default:
//				{
//					revisionAngle = lookAngle;
//				}
//				break;
//		}
//		Debug.Log (lookAngle);
//		transform.rotation = Quaternion.Euler(new Vector3(0,0,revisionAngle));
//	}
}
