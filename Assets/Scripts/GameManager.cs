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
			Debug.Log("CurrentState: " + CurrentState);
			GameManager.instance.UpdateGame();
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
			Invoke("StarFirstLevel", 2f);
		}
		else if(CurrentState == GameState.AI)
		{
			// Create AI if null
			if (CurrentAI == null)
			{
				CurrentAI = AIBase.CreateAIToGameobject(GameManager.CurrentLevel.gameObject);
			}
			
			GameManager.CurrentLevel.DeactivateAll();
			
			// Showing to player what to do
			CurrentAI.StartShowHint();
		}
		else if(CurrentState == GameState.Player)
		{
			// Clear player last combo
			PointRaytrace.CurrentPlayerHitsID = "";
			
			// Players turn
			CurrentAI.Stop();
			
			Invoke("RunAIAgain", 5f);
		}
	}
	
	private void RunAIAgain()
	{
		ChangeState(GameState.AI);
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
	
	public static bool HandleHitID(TargetBase target, ref string list)
	{
		if (!list.Contains(target.id.ToString()))
		{
			list+="/" + target.id;
			return true;
		}
		return false;
	}
}
