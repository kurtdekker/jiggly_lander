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

public class TractorBeam : MonoBehaviour
{
	Transform t1, t2;
	LineRenderer lr;

	Vector2 uvOffset;
	Vector2 uvOffsetSpeed;

	Vector2 uvScale;

	static Material _ForceBeam1_mtl;
	static Material ForceBeam1_mtl
	{
		get
		{
			if (_ForceBeam1_mtl == null || !_ForceBeam1_mtl)
			{
				_ForceBeam1_mtl = Instantiate<Material>( Resources.Load<Material>(
					"Materials/force_beam_mtl"));
			}
			return _ForceBeam1_mtl;
		}
	}

	public static TractorBeam Create( Transform t1, Transform t2)
	{
		TractorBeam tb = new GameObject ("TractorBeam").
			AddComponent<TractorBeam> ();

		float LineWidth = 0.5f;

		tb.t1 = t1;
		tb.t2 = t2;

		LineRenderer lr = tb.gameObject.AddComponent<LineRenderer> ();
		lr.material = ForceBeam1_mtl;
		lr.SetVertexCount (2);
		lr.SetWidth (LineWidth, LineWidth);

		tb.lr = lr;

		tb.uvOffsetSpeed = Vector2.right * 8.0f;

		// adjust repetition of the texture to match line width and
		// hence preserve aspect ratio
		tb.uvScale = new Vector2 ((t1.position - t2.position).magnitude / LineWidth, 1);

		return tb;
	}

	void Update ()
	{
		uvOffset += uvOffsetSpeed * Time.deltaTime;

		if (uvOffset.x >= 1.0f) uvOffset.x -= 1.0f;

		lr.material.mainTextureOffset = uvOffset;
		lr.material.mainTextureScale = uvScale;

		lr.SetPosition (0, t1.position);
		lr.SetPosition (1, t2.position);
	}
}
