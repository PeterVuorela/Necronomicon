using UnityEngine;
using System.Collections;

public class RotateConterClockwise : MonoBehaviour
{
	void Update()
	{
		transform.Rotate(LevelBase.GetCurrentRotation(false));
	}
}
