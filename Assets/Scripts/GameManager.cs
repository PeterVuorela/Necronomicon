using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private GameObject DragPrefab;
	
	[SerializeField]
	private LevelBase[] LevelArray;
	
	public static LevelBase CurrentLevel;
	private static GameManager _Instance;
	
	void Awake()
	{
		_Instance = this;
	}
	
	void Start()
	{
		Application.targetFrameRate = 60;
		
		StarFirstLevel();
	}

	public void StarFirstLevel()
	{
		if (CurrentLevel == null)
		{
			GameObject.Instantiate(LevelArray[0].gameObject);
		}
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
		if (DragPrefab != null)
		{
			newObj = GameObject.Instantiate(DragPrefab) as GameObject;
			
			if (obj != null)
			{
				newObj.transform.parent = obj.transform;
			}
			
			newObj.transform.localPosition = Vector3.zero;
			newObj.transform.localScale = DragPrefab.transform.localScale;
		}
		
		return newObj;
	}
}
