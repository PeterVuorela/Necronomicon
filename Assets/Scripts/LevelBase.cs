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
	private GameObject TargetPrefab;
	
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
			if (TargetPrefab != null)
			{
				GameObject obj = GameObject.Instantiate(TargetPrefab) as GameObject;
				obj.transform.parent = targetsArray[i].transform;
				obj.transform.localScale = Vector3.one;
				obj.transform.localPosition = Vector3.zero;
			}
			TargetsList.Add(targetsArray[i].GetComponent<TargetBase>());
		}
		
		GameManager.CurrentLevel = this;
	}
	
	void Update()
	{
		transform.Rotate(GetCurrentRotation());
	}
	
	public void GiveXAmountOfRandomTargetsVector( int targetAmount, ref Vector3[] resultArray)
	{
		for (int i = 0; i < resultArray.Length; i++)
		{
			resultArray[i] = GiveRandomTarget().transform.localPosition;
		}
	}
	
	public TargetBase GiveRandomTarget()
	{
		return TargetsList[UnityEngine.Random.Range(0, TargetsList.Count-1)];
	}
	
	public static Vector3 GetCurrentRotation(bool clockwise = true, float speed = 5f)
	{
		if (!GameManager.CurrentLevel.RotateEnable)
		{
			return Vector3.zero;
		}
	
		if (clockwise)
		{
			return Vector3.back * Time.deltaTime * speed;
		}
		
		return -Vector3.back * Time.deltaTime * speed;
	}
}
