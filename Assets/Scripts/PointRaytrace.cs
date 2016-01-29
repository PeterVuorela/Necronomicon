using UnityEngine;
using System.Collections;

public class PointRaytrace : MonoBehaviour
{
	[SerializeField]
	public GameObject renderPrefab;
	private static TrailRenderer renderTrail;
	
	public const float Force = 170f;

	private bool touching = false;
	private RaycastHit hit;
	private Ray ray;
	private static float nextTouchTime = 0f;
	
	void Awake()
	{
		if (renderTrail == null && renderPrefab != null)
		{
			GameObject newObj = Instantiate(renderPrefab) as GameObject;
			newObj.transform.localPosition = Vector3.zero;
			newObj.transform.localScale = Vector3.one;
			
			if(newObj != null && newObj.GetComponent<TrailRenderer>() != null)
			{
				renderTrail = newObj.GetComponent<TrailRenderer>();
			}
		}
	}

	void Update ()
	{
		if( (ifEditor() && Input.GetMouseButton(0)) || Input.touchCount > 0 )
		{
			touching = true;
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			//nextTouchTime <= Time.timeSinceLevelLoad
			Debug.Log(nextTouchTime + " : " + Time.timeSinceLevelLoad);
			
			if (renderTrail != null)
			{
				renderTrail.transform.position = ray.origin;
			}
			
			nextTouchTime = Time.timeSinceLevelLoad;
			
			if (Physics.Raycast(ray, out hit, 100f))
			{
				// HIT
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
			Debug.DrawRay(ray.origin - (Vector3.forward*0.5f), ray.direction*10, Color.yellow);
		}
	}
}
