/*
    The following license supersedes all notices in the source code.
*/

/*
    Copyright (c) 2017 Kurt Dekker/PLBM Games All rights reserved.

    http://www.twitter.com/kurtdekker
    
    Redistribution and use in source and binary forms, with or without
    modification, are permitted provided that the following conditions are
    met:
    
    Redistributions of source code must retain the above copyright notice,
    this list of conditions and the following disclaimer.
    
    Redistributions in binary form must reproduce the above copyright
    notice, this list of conditions and the following disclaimer in the
    documentation and/or other materials provided with the distribution.
    
    Neither the name of the Kurt Dekker/PLBM Games nor the names of its
    contributors may be used to endorse or promote products derived from
    this software without specific prior written permission.
    
    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS
    IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
    TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
    PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
    HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
    SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
    TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
    PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
    LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
    NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
    SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

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

	// for synthesizing engine noise instead of using the WAV file
	public bool useSynthesizedEngineNoise = true;

	public AnimationCurve EngineVolume;
	public AnimationCurve EnginePitch;

	Noise1[] engineNoises;

	Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody> ();

		engineNoises = new Noise1[ engines.Length];
		for (int i = 0; i < engineNoises.Length; i++)
		{
			engineNoises[i] = Noise1.Create ();
			engineNoises[i].volume = 0;
		}
	}

	void Update()
	{
		for (int i = 0; i < engines.Length; i++)
		{
			Engine e = engines[i];
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

			if (useSynthesizedEngineNoise)
			{
				engineNoises[i].volume = EngineVolume.Evaluate( e.currentFraction);
				engineNoises[i].frequency = EnginePitch.Evaluate( e.currentFraction);
			}
			else
			{
				AudioSource[] azes = e.tr.GetComponentsInChildren<AudioSource>();
				foreach( var az in azes)
				{
					az.volume = e.currentFraction;
				}
			}

			rb.AddForceAtPosition( transform.up * e.nominalPower * e.currentFraction, e.tr.position);
		}
	}
}
