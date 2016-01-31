using System.Linq;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class LeveSettings
{
	[SerializeField]
	public int RotateSpeed = 0;
	[SerializeField]
	public int NumberTargets = 4;
	[SerializeField]
	public int SuccessesNeeded;
	[SerializeField]
	public int AcceptedFailures;
}

public class LevelBase : MonoBehaviour
{
	[SerializeField]
	private GameObject HitEffectPrefab;
	
	[SerializeField]
	private LeveSettings LevelSettings;
	
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
		
		Debug.Log("Level Loaded: Rotate Speed" + GetSettings().RotateSpeed);
		Debug.Log("Level Loaded: Number Targets" + GetSettings().NumberTargets);
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
	
	public static Vector3 GetCurrentRotation(bool forceInvert = false)
	{
		if (GameManager.CurrentLevel.GetSettings().RotateSpeed == 0 || GameManager.CurrentState == GameManager.GameState.GameAI)
		{
			return Vector3.zero;
		}
		float rotateSpeed = GameManager.CurrentLevel.GetSettings().RotateSpeed;
		
		if (forceInvert)
		{
			if(rotateSpeed > 0)
			{
				rotateSpeed=-rotateSpeed;
			}
			else
			{
				rotateSpeed=Math.Abs(rotateSpeed);
			}
		}
		
		if (rotateSpeed > 0)
		{
			return Vector3.back * Time.deltaTime * Math.Abs(rotateSpeed);
		}
		
		return -Vector3.back * Time.deltaTime * Math.Abs(rotateSpeed);
	}
	
	public LeveSettings GetSettings()
	{
		return LevelSettings;
	}

	public void ClearAndDestroy ()
	{
		if (gameObject != null)
		{
			TargetsList.Clear();
			DestroyImmediate(gameObject);
		}
	}
}
