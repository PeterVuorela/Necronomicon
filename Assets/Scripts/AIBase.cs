using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIBase : MonoBehaviour
{
	private bool AIEnabled = false;
	private Tweener tweener = new Tweener();
	private TargetBase CurrentTarget = null;
	private TargetBase LastTarget = null;
	
	void Update()
	{
		if (AIEnabled && tweener != null && tweener.animating)
		{
			tweener.update();
			transform.localPosition = tweener.progression;
		}
	}
	
	public void StartRandomAI()
	{
		AIEnabled = true;
		SetTarget(GameManager.CurrentLevel.GiveRandomTarget());
	}
	
	public void StopAll()
	{
		AIEnabled = false;
	}
	
	private void SetTarget(TargetBase target)
	{
		LastTarget = CurrentTarget;
		CurrentTarget = target;
		
		Vector3 from = transform.localPosition;
		Vector3 to = CurrentTarget.transform.localPosition;
		
		tweener.easeFromTo(from, to, 1f, Easing.Linear, targetReached);
		Debug.Log("AI CurrentTarget: " + CurrentTarget.id);
	}
	
	private void targetReached()
	{
		SetTarget(GameManager.CurrentLevel.GiveRandomTarget());
	}
}
