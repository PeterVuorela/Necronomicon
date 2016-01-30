using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIBase : MonoBehaviour
{
	public enum State
	{
		Moving,
		Waiting,
		Done
	}
	
	public static bool Init = false;
	public State CurrentState = State.Waiting;
	
	private int DoneWaitTime = 4;
	
	private bool AIEnabled = false;
	private Tweener tweener = new Tweener();
	
	private List<TargetBase> CurrentTargetList = new List<TargetBase>(); 
	private int CurrentTargetIndex = -1;
	
	public static AIBase CreateAIToGameobject(GameObject obj)
	{
		GameObject newObj = GameManager.instance.InstantiateDragObjectTo(obj, false);
			
		// Add AI
		AIBase ai = newObj.AddComponent<AIBase>();
		
		return ai;
	}
	
	void Update()
	{
		if (AIEnabled && tweener != null && tweener.animating)
		{
			tweener.update();
			transform.localPosition = tweener.progression;
		}
		
		if (CurrentState == State.Done)
		{
			gameObject.SetActive(false);
		}
	}
	
	public void StartShowHint()
	{	
		GameManager.CurrentLevel.PopulateWithXAmountOfRandomTargets(4, ref CurrentTargetList);
		
		AIEnabled = true;
		TargetNextTarget();
	}
	
	public void RepeatShowHint()
	{
		//
	}
	
	public void StopAndClear()
	{
		AIEnabled = false;
		CurrentTargetIndex = -1;
		tweener = null;
		gameObject.SetActive(false);
	}
	
	private void TargetNextTarget()
	{
		SetTarget(GetNextTarget());
	}
	
	private void SetTarget(TargetBase target)
	{
		if (target != null)
		{
			CurrentState = State.Moving;
			
			Vector3 from = transform.localPosition;
			Vector3 to = target.transform.localPosition;
			
			Tweener.TweenDelegate[] easings = new Tweener.TweenDelegate[]{ Easing.Linear };
			Tweener.TweenDelegate randomEasing = easings[Random.Range(0, easings.Length-1)];
			
			tweener.easeFromTo(from, to, 0.5f, randomEasing, TargetNextTarget);
			Debug.Log("AI CurrentTarget: " + target.id);
		}
		else
		{
			CurrentState = State.Waiting;
			Invoke("SetAIDoneState", DoneWaitTime);
		}
	}
	
	private void SetAIDoneState()
	{
		CurrentState = State.Done;
		GameManager.ChangeState(GameManager.GameState.Player);
	}
	
	private TargetBase GetNextTarget()
	{
		CurrentTargetIndex++;
		if (CurrentTargetIndex >= CurrentTargetList.Count )
		{
			return null;
		}
		return CurrentTargetList[CurrentTargetIndex];
	}
}
