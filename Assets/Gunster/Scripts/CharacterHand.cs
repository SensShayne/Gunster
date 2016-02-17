using UnityEngine;
using System.Collections;

public class CharacterHand : MonoBehaviour 
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
