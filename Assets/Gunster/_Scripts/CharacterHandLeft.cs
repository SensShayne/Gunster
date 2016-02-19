using UnityEngine;
using System.Collections;

public enum CharacterHandLeftSprite
{
	BEND_0 = 0,
	BEND_1 = 1,
	BEND_2 = 2,
	BEND_3 = 3,
	BEND_4 = 4,
	BEND_5 = 5,
	BEND_6 = 6,
}

public class CharacterHandLeft : CharacterParts 
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
	public void ChangeSprite (CharacterHandLeftSprite index)
	{
		if ((int)index < _sprites.Length)
		{
			_spriteRenderer.sprite = _sprites [(int)index];
		}
	}


	// animation event ---------------------------------------------------
	public void OnAnimation_DeathFront (int keyFrame)
	{
		switch (keyFrame)
		{
			case 1:
				{
					ChangeSprite (CharacterHandLeftSprite.BEND_3);
				}
				break;
			case 2:
				{
					ChangeSprite (CharacterHandLeftSprite.BEND_4);
				}
				break;
			case 3:
				{
					ChangeSprite (CharacterHandLeftSprite.BEND_5);
				}
				break;
		}

		transform.rotation = Quaternion.Euler(new Vector3(0,0,-70));
	}

	public void OnAnimation_DeathBack (int keyFrame)
	{
		transform.rotation = Quaternion.Euler(new Vector3(0,0,-40));

		ChangeSprite (CharacterHandLeftSprite.BEND_4);
	}

	public void OnAnimation_SkyDeathFront (int keyFrame)
	{
		switch (keyFrame)
		{
			case 3:
				{
					transform.rotation = Quaternion.Euler(new Vector3(0,0,-48));

					ChangeSprite (CharacterHandLeftSprite.BEND_4);
				}
				break;
			case 4:
				{
					transform.rotation = Quaternion.Euler(new Vector3(0,0,-77));
				}
				break;
			case 5:
				{
					transform.rotation = Quaternion.Euler(new Vector3(0,0,-100));
				}
				break;
			case 6:
				{
					transform.rotation = Quaternion.Euler(new Vector3(0,0,-153));

					ChangeSprite (CharacterHandLeftSprite.BEND_5);
				}
				break;
		}
	}

	public void OnAnimation_SkyDeathBack (int keyFrame)
	{
		transform.rotation = Quaternion.Euler(new Vector3(0,0,-90));

		ChangeSprite (CharacterHandLeftSprite.BEND_4);
	}
}
