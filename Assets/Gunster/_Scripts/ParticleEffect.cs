using UnityEngine;
using System.Collections;

public class ParticleEffect : MonoBehaviour 
{
	ParticleSystem _particleSystem;
	GameObject _object;

	// unity functions ---------------------------------------------------
	void Awake()
	{
		_particleSystem = GetComponent<ParticleSystem>();
	}

	void FixedUpdate()
	{
		if (_object)
		{
			transform.position = _object.transform.position;
		}
	}


	// public functions ---------------------------------------------------
	public void RotateParticle (float angle)
	{
		_particleSystem.startRotation += angle * Mathf.Deg2Rad;
	}

	public void FollowTarget (GameObject ob)
	{
		_object = ob;
	}
}
