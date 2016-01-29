using UnityEngine;
using System.Collections;

public class PointRaytraceForce : MonoBehaviour
{
	public const float Force = 170f;

	bool touching = false;
	RaycastHit hit;
	Ray ray;

	float sideways = 0f;
	float nextTouchTime = 0f;

	void Start ()
	{
		//
	}

	void FixedUpdate ()
	{
		if( ifEditor() || Input.touchCount>0 )
		{
			touching = true;
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			if (Physics.Raycast(ray, out hit, 100f))
			{
				if(hit.rigidbody && nextTouchTime<=Time.timeSinceLevelLoad)
				{
					sideways 		= hit.transform.position.x - hit.point.x;

					if(transform.localScale.x<1f)
					{
						sideways*=1.5f;
					}

					nextTouchTime 	= Time.timeSinceLevelLoad+0.01f;
					hit.rigidbody.AddForce( new Vector3(sideways*Force, Force, 0f) );
					hit.rigidbody.AddTorque(new Vector3( Mathf.Abs(sideways*Force), Random.Range(0, 10f), Random.Range(0, 10f)), ForceMode.Impulse);
				}
			}
		}
		else
		{
			touching = false;
		}
	}

	private bool ifEditor()
	{
#if UNITY_EDITOR
		return true;
#endif
		return false;
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
