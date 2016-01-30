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
		
		TestAI();
	}
	
	void Update()
	{
		transform.Rotate(Vector3.back * Time.deltaTime * 5f);
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
		return TargetsList[Random.Range(0, TargetsList.Count-1)];
	}
	
	private void TestAI()
	{
		GameObject obj = GameManager.instance.InstantiateDragObjectTo(null, false);
		obj.transform.parent = transform;
		obj.transform.localScale = Vector3.one;
		
		AIBase ai = obj.AddComponent<AIBase>();
		ai.StartRandomAI();
	}
}
