using System.Linq;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class GameRules
{
	public int SuccessesNeeded;
	public int AcceptedFailures;
}

public class LevelBase : MonoBehaviour
{
	[SerializeField]
	private GameObject HitEffectPrefab;
	
	[SerializeField]
	private bool RotateEnable = true;
	
	[SerializeField]
	private GameRules LevelRules;
	
	public const string TargetTag = "Target";
	public static List<TargetBase> TargetsList = new List<TargetBase>();
	
	void Awake () 
	{
		TargetBase[] targetsArray = transform.gameObject.GetComponentsInChildren<TargetBase>();
		for (int i = 0; i < targetsArray.Length; i++)
		{
			targetsArray[i].id = i;
			if (HitEffectPrefab != null)
			{
				// Add hit effect
				GameObject obj = GameObject.Instantiate(HitEffectPrefab) as GameObject;
				obj.transform.parent = targetsArray[i].transform;
				obj.transform.localScale = Vector3.one;
				obj.transform.localPosition = Vector3.zero;
				obj.SetActive(false);
				targetsArray[i].GetComponent<TargetBase>().hitEffectParent = obj;
			}
			TargetsList.Add(targetsArray[i].GetComponent<TargetBase>());
		}
		
		GameManager.CurrentLevel = this;
	}
	
	void Update()
	{
		transform.Rotate(GetCurrentRotation());
	}
	
	public void DeactivateAll()
	{
		for (int i = 0; i < TargetsList.Count; i++ )
		{
			TargetsList[i].Deactivate();
		}
	}
	
	public void PopulateWithXAmountOfRandomTargets( int targetAmount, ref List<TargetBase> resultList)
	{
		// Randomize List
		var rnd = new System.Random(); 
		TargetsList = TargetsList.OrderBy(x => rnd.Next()).ToList();
		
		//Clear
		resultList.Clear();
		
		// Populate
		for (int i = 0; i < targetAmount; i++)
		{
			resultList.Add(TargetsList[i]);
		}
	}
	
	public TargetBase GiveRandomTarget()
	{
		return TargetsList[UnityEngine.Random.Range(0, TargetsList.Count-1)];
	}
	
	public static Vector3 GetCurrentRotation(bool clockwise = true, float speed = 10f)
	{
		if (!GameManager.CurrentLevel.RotateEnable || GameManager.CurrentState == GameManager.GameState.AI)
		{
			return Vector3.zero;
		}
		
		if (clockwise)
		{
			return Vector3.back * Time.deltaTime * speed;
		}
		
		return -Vector3.back * Time.deltaTime * speed;
	}
	
	public GameRules GetRules()
	{
		return LevelRules;
	}
}
