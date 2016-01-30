using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public enum GameState
	{
		None,
		LevelSelect,
		AI,
		Player
	}

	[SerializeField]
	private GameObject DragPrefabPlayer;
	
	[SerializeField]
	private GameObject DragPrefabAI;
	
	[SerializeField]
	private LevelBase[] LevelArray;
	
	public static LevelBase CurrentLevel;
	public static GameState CurrentState;
	public static AIBase CurrentAI;
	
	private static GameManager _Instance;
	
	void Awake()
	{
		_Instance = this;
		
		Application.targetFrameRate = 60;
	}
	
	void Start()
	{
		ChangeState(GameState.LevelSelect);
	}
	
	public static void ChangeState(GameState state)
	{
		if (CurrentState != state)
		{
			CurrentState = state;
			GameManager.instance.UpdateGame();
			
			Debug.Log("CurrentState: " + CurrentState);
		}
	}
	
	public void UpdateGame()
	{
		if (CurrentState == GameState.None)
		{
			ChangeState(GameState.LevelSelect);
		}
		else if(CurrentState == GameState.LevelSelect)
		{
			StarFirstLevel();
		}
		else if(CurrentState == GameState.AI)
		{
			// Showing to player what to do
			if (CurrentAI == null)
			{
				CurrentAI = AIBase.CreateAIToGameobject(GameManager.CurrentLevel.gameObject);
			}
			
			// Showing to player what to do
			CurrentAI.StartRandomAI();
		}
		else if(CurrentState == GameState.Player)
		{
			// Players turn
			CurrentAI.StopAndClear();
		}
	}

	public void StarFirstLevel()
	{
		if (CurrentLevel == null)
		{
			// load level
			GameObject.Instantiate(LevelArray[0].gameObject);
			
			ChangeState(GameState.AI);
		}
	}

	public virtual void restartTheGame()
	{
		//Application.LoadLevel(Application.loadedLevel);
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
