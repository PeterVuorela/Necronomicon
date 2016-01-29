using UnityEngine;
using System.Collections;

public class DontDestroyOnLoad : MonoBehaviour
{
	public bool destroyable = true;

	void Start()
	{
		if(destroyable)
		{
			DontDestroyOnLoad(this);
		}
	}

	void onLevelLoad()
	{
		// On Load
	}
}
