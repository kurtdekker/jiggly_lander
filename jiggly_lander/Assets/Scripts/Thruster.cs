using UnityEngine;
using System.Collections;

public class Thruster : MonoBehaviour
{
	// how quickly engine spools up
	public float RampUpTime = 0.5f;

	// how quickly engine spools down
	public float RampDownTime = 0.1f;

	public KeyCode KeyForBothEngines;

	// Characteristics of each engine
	[System.Serializable]
	public class Engine
	{
		public KeyCode key;
		public Transform tr;
		public float nominalPower = 120.0f;

		[HideInInspector]
		public float currentFraction;
	}

	public Engine[] engines;

	Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody> ();
	}

	void Update()
	{
		foreach( Engine e in engines)
		{
			if (Input.GetKey ( e.key) || Input.GetKey ( KeyForBothEngines))
			{
				if (RampUpTime > 0)
				{
					e.currentFraction += Time.deltaTime / RampUpTime;
					if (e.currentFraction > 1.0f)
					{
						e.currentFraction = 1.0f;
					}
				}
				else
				{
					e.currentFraction = 1.0f;
				}
			}
			else
			{
				if (RampDownTime > 0)
				{
					e.currentFraction -= Time.deltaTime / RampDownTime;
					if (e.currentFraction < 0)
					{
						e.currentFraction = 0;
					}
				}
				else
				{
					e.currentFraction = 0;
				}
			}

			ParticleSystem[] pses = e.tr.GetComponentsInChildren<ParticleSystem>();
			foreach( var ps in pses)
			{
				ps.emissionRate = 250 * e.currentFraction;
			}

			AudioSource[] azes = e.tr.GetComponentsInChildren<AudioSource>();
			foreach( var az in azes)
			{
				az.volume = e.currentFraction;
			}

			rb.AddForceAtPosition( transform.up * e.nominalPower * e.currentFraction, e.tr.position);
		}
	}
}
