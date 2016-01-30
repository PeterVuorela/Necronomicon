using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private GameObject DragPrefabPlayer;
	
	[SerializeField]
	private GameObject DragPrefabAI;
	
	[SerializeField]
	private LevelBase[] LevelArray;
	
	public static LevelBase CurrentLevel;
	private static GameManager _Instance;
	
	void Awake()
	{
		_Instance = this;
		
		Application.targetFrameRate = 60;
		StarFirstLevel();
	}
	
	void Start()
	{
		//
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
	
	public GameObject InstantiateDragObjectTo(GameObject obj, bool player = true)
	{
		GameObject prefab = null;
		if (player)
		{
			prefab = DragPrefabPlayer;
		}
		else
		{
			prefab = DragPrefabAI;
		}
		
		GameObject newObj = null;
		if (prefab != null)
		{
			newObj = GameObject.Instantiate(prefab) as GameObject;
			
			if (obj != null)
			{
				newObj.transform.parent = obj.transform;
			}
			
			newObj.transform.localPosition = Vector3.zero;
			newObj.transform.localScale = prefab.transform.localScale;
		}
		
		return newObj;
	}
}
