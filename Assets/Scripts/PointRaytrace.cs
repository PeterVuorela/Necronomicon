using UnityEngine;
using System.Collections;

public class PointRaytrace : MonoBehaviour
{	
	private static GameObject playerTrail;

	private bool touching = false;
	private RaycastHit hit;
	private Ray ray;
	private static float nextTouchTime = 0f;
	
	void Start()
	{
		if (playerTrail == null)
		{
			playerTrail = GameManager.instance.InstantiateDragObjectTo(null);
		}
	}

	void Update ()
	{
		if( GameManager.CurrentState == GameManager.GameState.Player && (ifEditor() && Input.GetMouseButton(0)) || Input.touchCount > 0 )
		{
			touching = true;
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			//nextTouchTime <= Time.timeSinceLevelLoad
			//Debug.Log(nextTouchTime + " : " + Time.timeSinceLevelLoad);
			
			if (playerTrail != null)
			{
				playerTrail.transform.position = ray.origin;
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
		
		if (playerTrail != null)
		{
			playerTrail.SetActive(touching);
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
