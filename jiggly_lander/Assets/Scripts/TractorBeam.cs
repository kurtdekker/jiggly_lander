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
