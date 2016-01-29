using UnityEngine;
using System.Collections;

public class AlignToCameraView : MonoBehaviour
{
	public enum DIRECTION
	{
		right,
		left,
		bottom
	}

	public DIRECTION direction = DIRECTION.left;

	void Start ()
	{
		Vector3 newPosition;

		newPosition = new Vector3((Camera.main.transform.position.x - Camera.main.orthographicSize*0.5f)-0.5f, transform.position.y, transform.position.z);

 		if(direction == DIRECTION.right)
		{
			newPosition.x*=-1;
		}


		if(direction == DIRECTION.bottom)
		{
			newPosition = new Vector3( Camera.main.transform.position.x, Camera.main.transform.position.y - Camera.main.orthographicSize  , transform.position.z);
		}

		transform.position	= newPosition;
	}
}
