using UnityEngine;
using System.Collections;

public class TargetBase : MonoBehaviour
{
	public int id = -1;
	public GameObject hitEffectParent;
	
	private BoxCollider boxCollider;
	
	void Awake()
	{
		boxCollider = GetComponent<BoxCollider>();
		Deactivate();
	}
	
	public void ShowHit()
	{
		hitEffectParent.SetActive(true);
	}
	
	public void HideHit()
	{
		hitEffectParent.SetActive(false);
	}
	
	public void Activate()
	{
		boxCollider.enabled = true;
	}
	
	public void Deactivate()
	{
		boxCollider.enabled = false;
		HideHit();
	}
}
