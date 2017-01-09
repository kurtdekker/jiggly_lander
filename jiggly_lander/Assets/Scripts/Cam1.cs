using UnityEngine;
using System.Collections;

public class Cam1 : MonoBehaviour
{
	public Transform target;

	public float Snappiness = 6.0f;

	Vector3 offset;

	void Start()
	{
		offset = transform.position - target.position;
	}

	void FixedUpdate ()
	{
		Vector3 pos = target.position + offset;

		transform.position = Vector3.Lerp (transform.position, pos, Snappiness * Time.deltaTime);
	}
}
