using UnityEngine;
using System.Collections;

public class RetryScreenUI : MonoBehaviour
{
	[SerializeField]
	private GameObject[] EyesOpen;
	
	[SerializeField]
	private GameObject[] EyesClosed;
	
	[SerializeField]
	private float AnimDelay = 2f;
	
	private GameObject DelayedOpen;
	private GameObject DelayedClose;
	
	void OnEnable()
	{
		DelayedOpen = null;
		DelayedClose = null;
		
		int i = 0;
		for (i = 0; i < EyesOpen.Length; i++)
		{
			if (GameManager.RetriesUsed-1 < i)
			{
				EyesOpen[i].SetActive(true);
			}
			else if (GameManager.RetriesUsed-1 == i)
			{
				DelayedOpen = EyesOpen[i];
				DelayedClose = EyesClosed[i];
				
				DelayedOpen.SetActive(true);
			}
			else
			{
				EyesOpen[i].SetActive(false);
			}
		}
		
		for (i = 0; i < EyesClosed.Length; i++)
		{
			EyesClosed[i].SetActive(false);
		}
		
		Invoke("TriggerAnimation", AnimDelay);
	}
	
	private void TriggerAnimation()
	{
		if (DelayedOpen != null && DelayedClose != null)
		{
			DelayedOpen.SetActive(false);
			DelayedClose.SetActive(true);
		}
	}
}
