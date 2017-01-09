using UnityEngine;
using System.Collections;

public class SkidVisualizer : MonoBehaviour
{
	public Transform LanderRoot;
	public Transform SkidRoot;

	void LateUpdate ()
	{
		transform.position = SkidRoot.position;
		transform.rotation = LanderRoot.rotation * Quaternion.Euler( 90, 0, 0);
	}
}
