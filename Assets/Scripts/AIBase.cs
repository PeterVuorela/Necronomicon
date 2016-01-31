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
	public State CurrentState = State.Done;
	public string CurrentTargetsID = "";
	
	private int DoneWaitTime = 2;
	
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
		GameManager.CurrentLevel.PopulateWithXAmountOfRandomTargets(GameManager.CurrentLevel.GetSettings().NumberTargets, ref CurrentTargetList);
		
		AIEnabled = true;
		TargetNextTarget();
	}
	
	public void RepeatShowHint()
	{
		//
	}
	
	public void Stop()
	{
		AIEnabled = false;
		CurrentTargetIndex = -1;
	}
	
	private void TargetNextTarget()
	{
		if (CurrentState == State.Done)
		{
			// reset position to fist
			TargetBase target = GetNextTarget();
			transform.localPosition = target.transform.localPosition;
			gameObject.SetActive(true);
			target.Activate();
			CurrentTargetsID = "";
			GameManager.HandleHitID(target, ref CurrentTargetsID);
			
			Debug.Log("AI Reset position");
		}
	
		// Then start tween to next in list
		SetTarget(GetNextTarget());
	}
	
	private void SetTarget(TargetBase target)
	{
		if (target != null)
		{
			GameManager.HandleHitID(target, ref CurrentTargetsID);
			
			CurrentState = State.Moving;
			
			Vector3 from = transform.localPosition;
			Vector3 to = target.transform.localPosition;
			
			Tweener.TweenDelegate[] easings = new Tweener.TweenDelegate[]{ Easing.Linear };
			Tweener.TweenDelegate randomEasing = easings[Random.Range(0, easings.Length-1)];
			
			tweener.easeFromTo(from, to, 0.5f, randomEasing, TargetNextTarget);
			Debug.Log("AI CurrentTarget: " + target.id);
			
			target.Activate();
		}
		else
		{
			CurrentState = State.Waiting;
			Debug.Log("AI Waits for: " + CurrentTargetsID);
			Invoke("SetAIDoneState", DoneWaitTime);
		}
	}
	
	private void SetAIDoneState()
	{
		CurrentState = State.Done;
		GameManager.ChangeState(GameManager.GameState.GamePlayer);
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
