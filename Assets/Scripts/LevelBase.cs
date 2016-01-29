using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelBase : MonoBehaviour
{
	[SerializeField]
	private GameObject TargetPrefab;
	
	public const string TargetTag = "Target";
	public static List<TargetBase> TargetsList = new List<TargetBase>();
	
	public static AIBase AI;
	
	void Start () 
	{
		GameObject[] targetsArray = GameObject.FindGameObjectsWithTag(TargetTag);
		for (int i = 0; i < targetsArray.Length; i++)
		{
			targetsArray[i].GetComponent<TargetBase>().id = i;
			GameObject obj = GameObject.Instantiate(TargetPrefab) as GameObject;
			obj.transform.parent = targetsArray[i].transform;
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = Vector3.zero;
			
			TargetsList.Add(targetsArray[i].GetComponent<TargetBase>());
		}
		
		GameManager.CurrentLevel = this;
		
		TestAI();
	}
	
	public TargetBase GiveRandomTarget()
	{
		return TargetsList[Random.Range(0, TargetsList.Count-1)];
	}
	
	private void TestAI()
	{
		GameObject obj = GameManager.instance.InstantiateDragObjectTo(null);
		obj.transform.parent = transform;
		obj.transform.localScale = Vector3.one;
		
		AIBase ai = obj.AddComponent<AIBase>();
		ai.StartRandomAI();
	}
}
