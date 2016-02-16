using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (Character))]
public class CharacterControl : MonoBehaviour 
{
	private Character userCharacter;


	private void Awake()
	{
		userCharacter = GetComponent<Character>();
	}


	private void Update()
	{

	}


	private void FixedUpdate()
	{
		float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
		float vertical = CrossPlatformInputManager.GetAxis("Vertical");
		float horizontalRaw = CrossPlatformInputManager.GetAxisRaw("Horizontal");
		float verticalRaw = CrossPlatformInputManager.GetAxisRaw("Vertical");

		userCharacter.Move(horizontal, vertical, horizontalRaw, verticalRaw);



		bool shoot = CrossPlatformInputManager.GetButton ("Shoot");
		if (shoot) 
		{
			Vector3 mousePosition = CrossPlatformInputManager.mousePosition;

			userCharacter.Shoot (mousePosition);
		}
	}
}
