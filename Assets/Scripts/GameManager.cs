using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public enum GameState
	{
		None,
		GameStart,
		Intro,
		LoadLevel,
		GameAI,
		GamePlayer,
		LevelLost,
		LevelWin,
		LevelWinOutro,
		GameEndWin
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
	
	[SerializeField]
	private GameObject ReadyMenu;
	
	[SerializeField]
	private GameObject StartMenu;
	
	[SerializeField]
	private GameObject SuccessMenu;
	
	[SerializeField]
	private GameObject GameOverMenu;
	
	private GameObject CurrentUIMenu = null;
	
	public static int CurrentLevelIndex = 0;
	public static LevelBase CurrentLevel;
	public static GameState CurrentState = GameState.None;
	public static GameState OnClickGotoState = GameState.None;
	public static AIBase CurrentAI;
	
	private static int LevelComboWins = 0;
	private static int LevelCombosPlayed = 0;
	
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
		if (CurrentState == GameState.LevelWin || CurrentState == GameState.LevelLost)
		{
			// Clear Game
			if (GameManager.CurrentLevel != null)
			{
				GameManager.CurrentLevel.ClearAndDestroy();
				GameManager.CurrentLevel = null;
			}
		}
		
		if (CurrentState == GameState.GameStart)
		{
			ShowUIMenu(TitleMenu);
			OnClickGotoState = GameState.Intro;
			CurrentLevelIndex = 0;
		}
		else if(CurrentState == GameState.Intro)
		{	
			ShowUIMenu(InfoMenu);
			OnClickGotoState = GameState.LoadLevel;
		}
		else if(CurrentState == GameState.LoadLevel)
		{
			LevelComboWins = 0;
			LevelCombosPlayed = 0;
			
			if(LoadCurrentLevel())
			{
				ShowUIMenu(null);
				ChangeState(GameState.GameAI);
			}
		}
		else if(CurrentState == GameState.GameAI)
		{
			if (GameManager.CurrentLevel != null)
			{
				ShowUIMenu(ReadyMenu);
				
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
			else
			{
				Debug.LogError("Cant Play NULL Level!");
			}			
		}
		else if(CurrentState == GameState.GamePlayer)
		{
			ShowUIMenu(StartMenu);
			Invoke("HideUIMenus", 1f);
			
			LevelCombosPlayed++;
			
			// Players turn
			CurrentAI.Stop();
		}
		else if (CurrentState == GameState.LevelWinOutro)
		{
			ShowUIMenu(SuccessMenu);
			Invoke("LevelWinOutroComplete", 2f);
		}
		else if (CurrentState == GameState.LevelWin)
		{
			ShowUIMenu(WinMenu);
			OnClickGotoState = GameState.LoadLevel;
			CurrentLevelIndex++;
		}
		else if (CurrentState == GameState.LevelLost)
		{
			ShowUIMenu(LostMenu);
			OnClickGotoState = GameState.GameStart;
		}
		else if (CurrentState == GameState.GameEndWin)
		{
			ShowUIMenu(GameOverMenu);
			Invoke("ReplayFromStart", 10f);
		}
	}
	
	private void LevelWinOutroComplete()
	{
		ChangeState(GameState.LevelWin);
	}
	
	private void ReplayFromStart()
	{
		ChangeState(GameState.GameStart);
	}
	
	private void HideUIMenus()
	{
		if (CurrentState == GameState.GamePlayer)
		{
			ShowUIMenu(null);
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
	
	public static  void EvaluatePlayerState()
	{
		Debug.Log("EvaluatePlayer-> " + PointRaytrace.CurrentPlayerHitsID + " : " + CurrentAI.CurrentTargetsID);
		if (CurrentState == GameState.GamePlayer && PointRaytrace.CurrentPlayerHitsID.Length > 0)
		{
			if (PointRaytrace.CurrentPlayerHitsID == CurrentAI.CurrentTargetsID)
			{
				Debug.Log("-- COMBO SUCCESS --");
				LevelComboWins++;
				
				if ((LevelCombosPlayed - LevelComboWins) > CurrentLevel.GetRules().AcceptedFailures)
				{
					Debug.Log("-- LEVEL FAILED --");
					ChangeState(GameState.LevelLost);
				}
				else
				if (LevelComboWins >= CurrentLevel.GetRules().SuccessesNeeded)
				{
					Debug.Log("-- LEVEL WIN --");
					ChangeState(GameState.LevelWinOutro);
				}
			}
			else if (!CurrentAI.CurrentTargetsID.Contains(PointRaytrace.CurrentPlayerHitsID))
			{
				Debug.Log("-- COMBO FAIL --");
				ChangeState(GameState.LevelLost);
			}
			
//			 
			
		}
		
	}
	
	private void NextTurn()
	{
		ChangeState(GameState.GameAI);
	}

	public bool LoadCurrentLevel()
	{
		if (CurrentLevel == null)
		{
			// Load level
			if (CurrentLevelIndex < LevelArray.Length && LevelArray[CurrentLevelIndex] != null)
			{
				GameObject newLevelObj = GameObject.Instantiate(LevelArray[CurrentLevelIndex].gameObject) as GameObject;
				
				GameManager.CurrentLevel = newLevelObj.GetComponent<LevelBase>();
				
				return true;
			}
			else
			{
				Debug.Log("-- RUN OUT OF LEVELS, I GUESS YOU WON --- CurrentLevelIndex: " + CurrentLevelIndex);
				ChangeState(GameState.GameEndWin);
			}
		}
		
		return false;
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
			list+= target.id;
			return true;
		}
		return false;
	}
}
