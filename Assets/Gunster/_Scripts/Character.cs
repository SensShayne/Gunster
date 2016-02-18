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
	[SerializeField] CharacterHandLeft _handLeft;
	[SerializeField] CharacterHandRight _handRight;
	[SerializeField] CharacterGun _gun;
	[SerializeField] BulletSpawn _bulletSpawn;
	[SerializeField] Transform _boosterOutput;

	public const float CROWL_POSE_ANGLE_RANGE = 20.0f;

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
			//Damage (0, collision.rigidbody.position);
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
		Vector2 position = transform.position;
		if (_pose == CharacterPose.SKY) 
		{
			position.x += horizontal * MoveSpeed();
		} 
		else 
		{ 
			position.x += horizontalRaw * MoveSpeed();
		}
		transform.position = position;
		

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

	public void Shoot (Vector3 mousePosition)
	{
		if (_death)
		{
			return;
		}

		// angle for sprite rotation range is -90 ~ 90
		const float offsetAngleForPartsRotation = -90.0f;

		float angleForSpriteRotation = Mathf.Abs(Utility.GetAngle (_gun.transform.position, mousePosition, true, false)) + offsetAngleForPartsRotation;

		_head.SetShootAngle (angleForSpriteRotation, _pose);
		_handLeft.SetShootAngle (angleForSpriteRotation, _pose);
		_handRight.SetShootAngle (angleForSpriteRotation, _pose);
		_gun.SetShootAngle (angleForSpriteRotation, _pose);

		// flip
		bool checkThatAimPositionIsRight = mousePosition.x > transform.position.x; 
		if ((checkThatAimPositionIsRight && !_flip) || 
			(!checkThatAimPositionIsRight && _flip))
		{
			Flip ();
		}

		// gun shoot
		float angleForBulletDirection = -Utility.GetAngle (_gun.transform.position, mousePosition);
		_bulletSpawn.SetShootAngle (angleForBulletDirection, _pose);
		_gun.Shoot();
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

	void Booster()
	{
	}


	// coroutine ----------------------------------------------------------


	// animation event ----------------------------------------------------
	void OnAnimation_DeathFront (int keyFrame)
	{
		_head.OnAnimation_DeathFront (keyFrame);
		_handLeft.OnAnimation_DeathFront (keyFrame);
		_handRight.OnAnimation_DeathFront (keyFrame);
	}

	void OnAnimation_DeathBack (int keyFrame)
	{
		_head.OnAnimation_DeathBack (keyFrame);
		_handLeft.OnAnimation_DeathBack (keyFrame);
		_handRight.OnAnimation_DeathBack (keyFrame);
	}

	void OnAnimation_SkyDeathFront (int keyFrame)
	{
		_head.OnAnimation_SkyDeathFront (keyFrame);
		_handLeft.OnAnimation_SkyDeathFront (keyFrame);
		_handRight.OnAnimation_SkyDeathFront (keyFrame);
	}

	void OnAnimation_SkyDeathBack (int keyFrame)
	{
		_head.OnAnimation_SkyDeathBack (keyFrame);
		_handLeft.OnAnimation_SkyDeathBack (keyFrame);
		_handRight.OnAnimation_SkyDeathBack (keyFrame);
	}
}
