using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PayloadGrabber : MonoBehaviour
{
	List<Transform> Payloads;

	const float LockonDistance = 6.0f;

	void Start()
	{
		Payloads = new List<Transform> ();
		Transform[] tmp = FindObjectsOfType<Transform> ();
		foreach( var tr in tmp)
		{
			if (tr.name.ToLower().StartsWith( "payload"))
			{
				Payloads.Add( tr);
			}
		}
	}

	void Update ()
	{
		foreach( var payload in Payloads)
		{
			var delta = payload.position - transform.position;
			if (delta.sqrMagnitude <= LockonDistance * LockonDistance)
			{
				HingeJoint hj = gameObject.AddComponent<HingeJoint>();
				hj.connectedBody = payload.GetComponent<Rigidbody>();
				hj.axis = Vector3.forward;
				hj.anchor = Vector3.zero;

				TractorBeam.Create ( transform, payload);

				// <WIP> maybe we want to "correct" the hang distance
				// in case you came in so hot that you are unusually
				// close (or far) from the payload? Meh...

				Payloads.Remove ( payload);
				return;
			}
		}
	}
}
