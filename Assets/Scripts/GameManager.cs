using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	private static GameManager _Instance;
	
	public virtual void startGame()
	{
		//
	}

	public virtual void restartTheGame()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	public static GameManager instance
	{
		get
		{
			return _Instance;
		}
	}
}
