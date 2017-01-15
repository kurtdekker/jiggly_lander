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

public class SparseWallOfCubes : MonoBehaviour
{
	public float width = 40;
	public float height = 40;

	public int NumCubes = 40;

	public Material material;

	IEnumerator Start()
	{
		for ( int i = 0; i < NumCubes; i++)
		{
			float x = Random.Range ( -width, width);
			float y = (i * height) / NumCubes;
			float z = Random.Range ( 0.0f, 5.0f);

			GameObject cube = GameObject.CreatePrimitive( PrimitiveType.Cube);
			cube.transform.SetParent ( transform);
			cube.transform.position = transform.position + new Vector3( x, y, z);
			cube.transform.localScale = new Vector3(
				Random.Range ( 3.0f, 6.0f),
				Random.Range ( 3.0f, 6.0f),
				0.4f);
			cube.GetComponent<Renderer>().material = material;

			yield return null;
		}

		Destroy (this);
	}

	void OnDrawGizmos()
	{
		int count = 20;
		for (int i = -count; i <= count; i++)
		{
			float x = (width * i) / count;
			Gizmos.DrawLine( transform.position + Vector3.right * x,
			                transform.position + Vector3.right * x + Vector3.up * height);
			if (i >= 0)
			{
				float y = (height * i) / count;
				Gizmos.DrawLine( transform.position + Vector3.up * y + Vector3.right * width,
				                transform.position + Vector3.up * y + Vector3.left * width);
			}
		}
	}
}
