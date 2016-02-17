using UnityEngine;
using System.Collections;

public class CharacterHead : MonoBehaviour 
{

	// unity functions ---------------------------------------------------
	void Start()
	{

	}


	// public functions ---------------------------------------------------
	public void SetShootAngle (float shootAngle)
	{
		transform.rotation = Quaternion.Euler(new Vector3(0,0,shootAngle));
	}
}
