using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private GameObject DragPrefab;
	
	private static GameManager _Instance;
	
	void Awake()
	{
		_Instance = this;
	}
	
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
	
	public GameObject InstantiateDragObjectTo(GameObject obj)
	{
		GameObject newObj = null;
		if (DragPrefab != null && obj != null)
		{
			newObj = Instantiate(DragPrefab) as GameObject;
			newObj.transform.parent = obj.transform;
			newObj.transform.localPosition = Vector3.zero;
			newObj.transform.localScale = DragPrefab.transform.localScale;
		}
		
		return newObj;
	}
}
