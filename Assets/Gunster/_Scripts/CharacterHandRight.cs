using UnityEngine;
using System.Collections;

public enum CharacterHandRightSprite
{
	BEND_0 = 0,
	BEND_1 = 1,
	BEND_2 = 2,
	BEND_3 = 3,
	BEND_4 = 4,
	BEND_5 = 5,
}

public class CharacterHandRight : CharacterParts 
{
	[SerializeField] Sprite[] _sprites;

	SpriteRenderer _spriteRenderer;


	// unity functions ---------------------------------------------------
	void Start()
	{
		defaultSpriteAngle = -90.0f;

		_spriteRenderer = GetComponent<SpriteRenderer> ();
	}


	// public functions ---------------------------------------------------
	public void ChangeSprite (CharacterHandRightSprite index)
	{
		if ((int)index < _sprites.Length)
		{
			_spriteRenderer.sprite = _sprites [(int)index];
		}
	}


	// animation event ---------------------------------------------------
	public void OnAnimation_DeathFront (int keyFrame)
	{
		transform.rotation = Quaternion.Euler(new Vector3(0,0,-45));

		ChangeSprite (CharacterHandRightSprite.BEND_0);
	}

	public void OnAnimation_DeathBack (int keyFrame)
	{
		transform.rotation = Quaternion.Euler(new Vector3(0,0,0));

		ChangeSprite (CharacterHandRightSprite.BEND_1);
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
