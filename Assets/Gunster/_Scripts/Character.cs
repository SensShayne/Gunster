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



// Character Parts ------------------------------------------------------------------
public class CharacterParts : MonoBehaviour 
{
	private float _defaultSpriteAngle;

	public float defaultSpriteAngle
	{
		get { return _defaultSpriteAngle; }
		set { _defaultSpriteAngle = value; }
	}
		
	public virtual void LookAt (Vector3 sourcePostion, Vector3 targetPosition, CharacterPose characterPose)
	{
		float lookAngle = Mathf.Abs(Utility.GetAngle (sourcePostion, targetPosition, true, false)) + defaultSpriteAngle;

		float revisionAngle;

		switch (characterPose)
		{
			case CharacterPose.CROWL:
				{
					revisionAngle = Mathf.Clamp (lookAngle, 
						Utility.CalcDefaultLeftAngle (defaultSpriteAngle)-Character.CROWL_POSE_ANGLE_RANGE, 
						Utility.CalcDefaultLeftAngle (defaultSpriteAngle)+Character.CROWL_POSE_ANGLE_RANGE);
				}
				break;
			default:
				{
					revisionAngle = Mathf.Clamp (lookAngle, 
						Utility.CalcDefaultUpAngle (defaultSpriteAngle), 
						Utility.CalcDefaultDownAngle (defaultSpriteAngle));
				}
				break;
		}

		transform.rotation = Quaternion.Euler(new Vector3(0,0,revisionAngle));
	}

	public void ResetRotation ()
	{
		transform.rotation = Quaternion.identity;
	}
}

// Character ------------------------------------------------------------------
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
	[SerializeField] CharacterWeapon _weapon;
	[SerializeField] Transform _boosterOutput;

	public const float CROWL_POSE_ANGLE_RANGE = 20.0f;



	Transform _skyCheck;    
	const float _skyCheckRadius = 0.05f;

	Animator _animator;
	Rigidbody2D _rigidbody;



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
				pose = CharacterPose.SKY;
			}
			else
			{
				pose = CharacterPose.STAND;
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
		if (pose == CharacterPose.SKY) 
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
			pose = CharacterPose.SKY;
		}
		else
		{
			if (-0.5f <= vertical && vertical < 0.0f)
			{
				pose = CharacterPose.CROUCH;
			}
			else if (-1.0f <= vertical && vertical < -0.5f)
			{
				pose = CharacterPose.CROWL;
			}
			else if (vertical == 0.0f)
			{
				pose = CharacterPose.STAND;
			}
		}


		// direction
		if (horizontalRaw == 0)
		{
			moveDirection = MoveDirection.STOP;
		}
		else 
		{
			moveDirection = MoveDirection.FORWARD;

			if ((horizontal > 0 && !_flip) || 
				(horizontal < 0 && _flip))
			{
				flip = !flip;
			}
		}                                                                                             
	}

	public void Shoot (Vector3 mousePosition)
	{
		if (_death)
		{
			return;
		}



		_head.LookAt (_weapon.transform.position, mousePosition, pose);
		_handLeft.LookAt (_weapon.transform.position, mousePosition, pose);
		_handRight.LookAt (_weapon.transform.position, mousePosition, pose);
		_weapon.LookAt (_weapon.transform.position, mousePosition, pose);


		// flip
		bool checkThatMousePositionIsRight = mousePosition.x > transform.position.x; 
		if ((checkThatMousePositionIsRight && !_flip) || 
			(!checkThatMousePositionIsRight && _flip))
		{
			flip = !flip;
		}


		// shoot
		_weapon.Shoot();
	}


	// private functions --------------------------------------------------
	float MoveSpeed()
	{
		switch (pose) 
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

	void Death (DeathDirection deathDirection)
	{
		_animator.SetInteger ("death", (int)deathDirection);

		_animator.SetTrigger ("deathTrigger");

		_death = true;
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


	// property ----------------------------------------------------------
	CharacterPose _pose = CharacterPose.STAND;
	public CharacterPose pose
	{
		get
		{
			return _pose;
		}
		set
		{
			if (_pose != value)
			{
				_head.ResetRotation ();
				_handLeft.ResetRotation ();
				_handRight.ResetRotation ();
				_weapon.ResetRotation ();
			}

			_pose = value;

			_animator.SetInteger ("pose", (int)value);
		}
	}

	MoveDirection _moveDirection = MoveDirection.STOP;
	public MoveDirection moveDirection
	{
		get
		{
			return _moveDirection;
		}
		set
		{
			_moveDirection = value;

			_animator.SetInteger ("move", (int)value);
		}
	}

	bool _flip = false;
	public bool flip
	{
		get
		{
			return _flip;
		}
		set
		{
			_flip = value;

			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}
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
