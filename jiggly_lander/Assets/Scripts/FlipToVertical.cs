using UnityEngine;
using System.Collections;

public class FlipToVertical : MonoBehaviour
{
	void Update ()
	{
		if (Input.GetKeyDown( KeyCode.Tab))
		{
			transform.rotation = Quaternion.identity;
		}
	}
}
