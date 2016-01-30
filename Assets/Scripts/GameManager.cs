using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public enum GameState
	{
		None,
		GameStart,
		LevelSelect,
		GameAI,
		GamePlayer,
		GameLost,
		GameWin
	}

	[SerializeField]
	private GameObject DragPrefabPlayer;
	
	[SerializeField]
	private GameObject DragPrefabAI;
	
	[SerializeField]
	private LevelBase[] LevelArray;
	
	[Header("UI Stuff")]
	[SerializeField]
	private GameObject TitleMenu;
	
	[SerializeField]
	private GameObject InfoMenu;
	
	[SerializeField]
	private GameObject WinMenu;
	
	[SerializeField]
	private GameObject LostMenu;
	
	private GameObject CurrentUIMenu = null;
	
	public static LevelBase CurrentLevel;
	public static GameState CurrentState = GameState.None;
	public static GameState OnClickGotoState = GameState.None;
	public static AIBase CurrentAI;
	
	private static int SessionWins = 0;
	private static int SessionPlays = 0;
	
	private static GameManager _Instance;
	
	void Awake()
	{
		_Instance = this;
		
		Application.targetFrameRate = 60;
	}
	
	void Update()
	{
		if (Input.GetMouseButtonDown(0) && OnClickGotoState != GameState.None)
		{
			ChangeState(OnClickGotoState);
		}
	}
	
	void Start()
	{
		ChangeState(GameState.GameStart);
	}
	
	public static void ChangeState(GameState state)
	{
		if (CurrentState != state)
		{
			OnClickGotoState = GameState.None;
			CurrentState = state;
			Debug.Log("CurrentState: " + CurrentState);
			GameManager.instance.UpdateGameState();
		}
	}
	
	public void UpdateGameState()
	{
		if (CurrentState == GameState.GameStart)
		{
			ShowUIMenu(TitleMenu);
			OnClickGotoState = GameState.LevelSelect;
		}
		else if(CurrentState == GameState.LevelSelect)
		{
			SessionWins = 0;
			SessionPlays = 0;
			
			ShowUIMenu(InfoMenu);
			OnClickGotoState = GameState.GameAI;
		}
		else if(CurrentState == GameState.GameAI)
		{
			LoadFirstLevelIfNull();
		
			ShowUIMenu(null);
			
			PointRaytrace.CurrentPlayerHitsID = "";
			
			// Create AI if null
			if (CurrentAI == null)
			{
				CurrentAI = AIBase.CreateAIToGameobject(GameManager.CurrentLevel.gameObject);
			}
			
			GameManager.CurrentLevel.DeactivateAll();
			
			// Showing to player what to do
			CurrentAI.StartShowHint();
		}
		else if(CurrentState == GameState.GamePlayer)
		{
			SessionPlays++;
			
			// Players turn
			CurrentAI.Stop();
			
			Invoke("EvaluatePlayer", 5f);
		}
		else if (CurrentState == GameState.GameWin)
		{
			ShowUIMenu(WinMenu);
			OnClickGotoState = GameState.GameStart;
		}
		else if (CurrentState == GameState.GameLost)
		{
			ShowUIMenu(LostMenu);
			OnClickGotoState = GameState.GameStart;
		}
		
		if (CurrentState == GameState.GameWin || CurrentState == GameState.GameLost)
		{
			// Clear Game
			if (GameManager.CurrentLevel != null)
			{
				GameManager.CurrentLevel.ClearAndDestroy();
			}
		}
	}
	
	public void ShowUIMenu(GameObject obj)
	{
		if (CurrentUIMenu != null)
		{
			CurrentUIMenu.SetActive(false);
		}
		if (obj != null)
		{
			CurrentUIMenu = obj;
			CurrentUIMenu.SetActive(true);
		}
	}
	
	private void EvaluatePlayer()
	{
		Debug.Log("EvaluatePlayer-> " + PointRaytrace.CurrentPlayerHitsID + " : " + CurrentAI.CurrentTargetsID);
		if (PointRaytrace.CurrentPlayerHitsID == CurrentAI.CurrentTargetsID)
		{
			Debug.Log("-- COMBO SUCCESS --");
			SessionWins++;
		}
		else
		{
			Debug.Log("-- COMBO FAIL --");
		}
		
		if ((SessionPlays - SessionWins) > CurrentLevel.GetRules().AcceptedFailures)
		{
			Debug.Log("-- LEVEL FAILED --");
			ChangeState(GameState.GameLost);
		}
		else if (SessionWins >= CurrentLevel.GetRules().SuccessesNeeded)
		{
			Debug.Log("-- LEVEL WIN --");
			ChangeState(GameState.GameWin);
		}
		else
		{
			Invoke("NextTurn", 3f);
		}
	}
	
	private void NextTurn()
	{
		ChangeState(GameState.GameAI);
	}

	public void LoadFirstLevelIfNull()
	{
		if (CurrentLevel == null)
		{
			// load level
			GameObject.Instantiate(LevelArray[0].gameObject);
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
