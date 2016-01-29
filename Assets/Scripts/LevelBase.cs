using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelBase : MonoBehaviour
{
	public const string TargetTag = "Target";
	private List<TargetBase> TargetsList = new List<TargetBase>();
	
	public static AIBase AI;
	
	void Start () 
	{	
		GameObject[] targetsArray = GameObject.FindGameObjectsWithTag(TargetTag);
		for (int i = 0; i < targetsArray.Length; i++)
		{
			targetsArray[i].GetComponent<TargetBase>().id = i;
			TargetsList.Add(targetsArray[i].GetComponent<TargetBase>());
		}
	}
}
