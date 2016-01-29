using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private GameObject DragPrefab;
	
	private static GameManager _Instance;
	
	void Start()
	{
		Application.targetFrameRate = 60;
	}
	
	public virtual void startGame()
	{
		//
	}

	public virtual void restartTheGame()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	public static GameManager instance
	{
		get
		{
			return _Instance;
		}
	}
	
	public void InstantiateDragObjectTo(GameObject obj)
	{
		if (DragPrefab != null && obj != null)
		{
			GameObject newObj = obj.Instantiate(DragPrefab) as GameObject;
			newObj.transform.localPosition = Vector3.zero;
			newObj.transform.localScale = Vector3.one;
			
			return newObj;
		}
	}
}
