using UnityEngine;
using System.Collections;

public class PointRaytraceVelocity : MonoBehaviour
{
	bool touching = false;
	RaycastHit hit;
	Ray ray;
	
	Vector3 lastMousePosition;
	Vector3 lastDelta = Vector3.zero;

	void Start ()
	{
		//
	}

	void Update ()
	{
		Vector2 deltaTouch = Vector3.zero;

		if( Input.touchCount>0 || Input.GetMouseButton(0) )
		{
#if UNITY_EDITOR
			deltaTouch 		= Input.mousePosition - lastMousePosition;
#else
			deltaTouch 		= deltaTouch = Input.GetTouch(0).deltaPosition;
#endif
			touching = true;
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			deltaTouch.y	= Mathf.Abs(deltaTouch.y);
			deltaTouch.x	= 0;
			
			if (Physics.Raycast(ray, out hit, 100f))
			{
				if(hit.rigidbody && deltaTouch.magnitude>0.01)
				{
					//lastDelta 	= deltaTouch*Screen.height;
					//lastDelta.y = Mathf.Min(lastDelta.y, 20);

					hit.rigidbody.velocity =  (deltaTouch/Screen.height*1000f)*2f;
					//hit.rigidbody.AddTorque(new Vector3(Random.Range(0f, 100f), Random.Range(0f, 100f), Random.Range(0f, 100f)), ForceMode.Impulse);
				}
			}
		}
		else
		{
			lastDelta = deltaTouch;
			touching = false;
		}

		lastMousePosition = Input.mousePosition;
	}

	void OnDrawGizmos()
	{
		if(touching)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(ray.origin, ray.direction*100f);
		}
	}
}
