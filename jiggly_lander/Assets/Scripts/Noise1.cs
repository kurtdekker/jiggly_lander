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

public class Noise1 : MonoBehaviour
{
	public static Noise1 Create( float initialFrequency = 5000, float initialVolume = 0)
	{
		Noise1 nz1 = new GameObject( "Noise1.Create();").
			AddComponent<Noise1>();
		nz1.sampleRate = AudioSettings.outputSampleRate;
		if (nz1.sampleRate < 100) nz1.sampleRate = 22050.0f;
		nz1.gameObject.AddComponent<AudioSource> ();
		nz1.frequency = initialFrequency;
		nz1.volume = initialVolume;
		return nz1;
	}

	float sampleRate;

	float _volume;
	public float volume
	{
		get
		{
			return _volume;
		}
		set
		{
			_volume = value;
		}
	}

	float _frequency;
	public float frequency
	{
		get
		{
			return _frequency;
		}
		set
		{
			_frequency = value;
			noisePhaseMotion = _frequency / sampleRate;
		}
	}

	// where in a single "wavecycle" of noise are we (0 to 1)
	float noisePhase;
	// how fast we progress through the phase
	float noisePhaseMotion;

	float lastNoiseSample;

	const int maxRands = 10000;
	// audio callback "Consumes" our pre-made random numbers with this
	int noiseRover;
	float[] someRands;

	void Update()
	{
		if (someRands == null)
		{
			someRands = new float[maxRands];
			
			lock(someRands)
			{
				// From docs: "Also note, that OnAudioFilterRead is called on a
				// different thread from the main thread (namely the audio thread) 
				for (int i = 0; i < maxRands; i++)
				{
					someRands[i] = Random.Range ( -1.0f, 1.0f);
				}
			}
		}
	}

	void OnAudioFilterRead(float[] __data, int __channels)
	{
		if (someRands == null)
		{
			return;
		}

		int count = __data.Length;
		int samples = count / __channels;

		int j = 0;
		for( int i = 0; i < samples; i++)
		{
			if (noisePhase < 1.0f)
			{
				noisePhase += noisePhaseMotion;
			}
			else
			{
				noisePhase -= 1.0f;
				lastNoiseSample = someRands[noiseRover];
				noiseRover++;
				if (noiseRover >= maxRands) noiseRover = 0;
			}

			for (int channel = 0; channel < __channels; channel++)
			{
				__data[i * __channels + channel] += lastNoiseSample * _volume;
			}
		}
	}
}
