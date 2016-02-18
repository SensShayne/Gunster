using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
	[SerializeField] float _speed;
	[SerializeField] float _lifeTime;


	// unity functions ---------------------------------------------------
	void Start()
	{
		Rigidbody2D rigidbody = GetComponent<Rigidbody2D> ();
		rigidbody.AddForce(transform.up * _speed);

		Destroy (gameObject, _lifeTime);
	}


	// public functions ---------------------------------------------------

}
