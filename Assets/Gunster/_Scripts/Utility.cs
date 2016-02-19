using UnityEngine;
using System.Collections;

public static class Utility 
{
	public static float GetAngle(Vector2 source, Vector2 target)
	{
		float dx = target.x - source.x;
		float dy = target.y - source.y;

		float rad = Mathf.Atan2(dx, dy);
		float degree = rad * Mathf.Rad2Deg;

		return degree;
	} 

	public static float GetAngle(Vector2 source, Vector2 target, bool absX, bool absY)
	{
		float dx = absX ? Mathf.Abs(target.x - source.x) : target.x - source.x;
		float dy = absY ? Mathf.Abs(target.y - source.y) : target.y - source.y;

		float rad = Mathf.Atan2(dx, dy);
		float degree = rad * Mathf.Rad2Deg;

		return degree;
	} 

	public static float CalcDefaultUpAngle (float criteriaAngle)
	{
		return 0.0f + criteriaAngle;
	}

	public static float CalcDefaultDownAngle (float criteriaAngle)
	{
		return 180.0f + criteriaAngle;
	}

	public static float CalcDefaultLeftAngle (float criteriaAngle)
	{
		return 90.0f + criteriaAngle;
	}

	public static float CalcDefaultRightAngle (float criteriaAngle)
	{
		return -90.0f + criteriaAngle;
	}

	// unity functions ---------------------------------------------------
	// public functions --------------------------------------------------
	// private functions -------------------------------------------------
	// property ----------------------------------------------------------
	// animation event ---------------------------------------------------
	// coroutine ---------------------------------------------------------
}
