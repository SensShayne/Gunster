using UnityEngine;
using System.Collections;

public enum CharacterPose
{
	STAND = 0,
	CROUCH = 1,
	CROWL = 2,
	SKY = 3
}

public enum DeathDirection
{
	BACK = -1,
	FRONT = 1,
}

public enum MoveDirection
{
	BACKWARD = -1,
	STOP = 0,
	FORWARD = 1
}

[RequireComponent(typeof (Animator))]
[RequireComponent(typeof (Rigidbody2D))]
public class Character : MonoBehaviour 
{
	[SerializeField] float standSpeed;
	[SerializeField] float crouchSpeed;
	[SerializeField] float crowlSpeed;
	[SerializeField] float skyMoveSpeed;
	[SerializeField] float boosterSpeed;

	[SerializeField] LayerMask _whatIsGround;

	[SerializeField] ParticleEffect _damageEffect;

	[SerializeField] CharacterHead _head;
	[SerializeField] CharacterHand _handLeft;
	[SerializeField] CharacterHand _handRight;
	[SerializeField] CharacterGun _gun;
	[SerializeField] GameObject _bulletSpawn;

	Transform _skyCheck;    
	const float _skyCheckRadius = 0.05f;

	Animator _animator;
	Rigidbody2D _rigidbody;

	MoveDirection _moveDirection = MoveDirection.STOP;
	CharacterPose _pose = CharacterPose.STAND;

	bool _flip = false;

	bool _death = false;


	// unity functions ---------------------------------------------------
	void Start () 
	{
		_skyCheck = transform.Find("Sky Check");
		_animator = GetComponent<Animator> ();
		_rigidbody = GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate () 
	{
		if (_death)
		{
			if (IsSkying ())
			{
				SetPose (CharacterPose.SKY);
			}
			else
			{
				SetPose (CharacterPose.STAND);
			}
		}
		// test
		else
		{
			if (Input.GetKeyDown (KeyCode.Q))
			{
				Death (DeathDirection.BACK);
			}
			else if (Input.GetKeyDown (KeyCode.E))
			{
				Death (DeathDirection.FRONT);
			}
		}
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		// test
		//if (collision.gameObject.tag == "Terrain")
		{
			Damage (0, collision.rigidbody.position);
		}
	}


	// public functions ---------------------------------------------------
	public void Move (float horizontal, float vertical, float horizontalRaw, float verticalRaw)
	{
		if (_death)
		{
			return;
		}

		// move
		Vector2 position = _rigidbody.position;
		if (_pose == CharacterPose.SKY) 
		{
			position.x += horizontal * MoveSpeed();
		} 
		else 
		{
			position.x += horizontalRaw * MoveSpeed();
		}
		_rigidbody.position = position;

		Vector2 velocity = _rigidbody.velocity;
		if (vertical > 0)
		{
			velocity.y = boosterSpeed;
		}
		_rigidbody.velocity = velocity;


		// pose
		if (IsSkying ())
		{
			SetPose (CharacterPose.SKY);
		}
		else
		{
			if (-0.5f <= vertical && vertical < 0.0f)
			{
				SetPose (CharacterPose.CROUCH);
			}
			else if (-1.0f <= vertical && vertical < -0.5f)
			{
				SetPose (CharacterPose.CROWL);
			}
			else if (vertical == 0.0f)
			{
				SetPose (CharacterPose.STAND);
			}
		}


		// direction
		if (horizontalRaw == 0)
		{
			SetMoveDirection (MoveDirection.STOP);
		}
		else 
		{
			SetMoveDirection (MoveDirection.FORWARD);

			if ((horizontal > 0 && !_flip) || 
				(horizontal < 0 && _flip))
			{
				Flip ();
			}
		}                                                                                             
	}

	public void Shoot (Vector3 aimPosition)
	{
		if (_death)
		{
			return;
		}

		//Debug.Log (aimPosition);
		const float partsAngleOffset = -90.0f;
		const float precisionAngle = 10.0f;

		float shootAngle = partsAngleOffset + precisionAngle - Utility.GetAngle (transform.position, aimPosition);
		//float shootAngle = partsAngleOffset - Utility.GetAngle (_bulletSpawn.transform.position, aimPosition);
		Debug.Log (shootAngle);
		         
		_head.SetShootAngle (shootAngle);
		_handLeft.SetShootAngle (shootAngle);
		_handRight.SetShootAngle (shootAngle);
		_gun.SetShootAngle (shootAngle);

	}


	// private functions --------------------------------------------------
	float MoveSpeed()
	{
		switch (_pose) 
		{
			case CharacterPose.STAND:
				{
					return standSpeed;
				}
			case CharacterPose.CROUCH:
				{
					return crouchSpeed;
				}
			case CharacterPose.CROWL:
				{
					return crowlSpeed;
				}
			case CharacterPose.SKY:
				{
					return skyMoveSpeed;
				}
			default:
				{
					return 0;
				}
		}
	}

	void SetPose (CharacterPose pose)
	{
		_pose = pose;

		_animator.SetInteger ("pose", (int)pose);
	}

	void SetMoveDirection (MoveDirection moveDirection)
	{
		_moveDirection = moveDirection;

		_animator.SetInteger ("move", (int)moveDirection);
	}

	void Death (DeathDirection deathDirection)
	{
		_animator.SetInteger ("death", (int)deathDirection);

		_animator.SetTrigger ("deathTrigger");

		_death = true;
	}

	void Flip()
	{
		_flip = !_flip;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	bool IsSkying()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(_skyCheck.position, _skyCheckRadius, _whatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject && 
				(colliders[i].gameObject.tag == "Field" || colliders[i].gameObject.tag == "Field Object"))
			{
				return false;
			}
		}

		return true;
	}

	void Damage (int damage, Vector2 damagePosition)
	{
		// damage effect
		var damageEffect = Instantiate(_damageEffect, transform.position, transform.rotation) as ParticleEffect;

		float angle = -Utility.GetAngle (_rigidbody.position, damagePosition);
		damageEffect.RotateParticle (angle);
		damageEffect.FollowTarget (gameObject);


		// HP
	}

	// coroutine ----------------------------------------------------------
}
