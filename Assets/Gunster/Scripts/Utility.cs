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

	// unity functions ---------------------------------------------------
	// public functions --------------------------------------------------
	// coroutine ---------------------------------------------------------
	// private functions -------------------------------------------------
}
