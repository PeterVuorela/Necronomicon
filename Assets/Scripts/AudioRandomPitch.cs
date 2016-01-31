using UnityEngine;
using System.Collections;

public class AudioRandomPitch : MonoBehaviour {

	[SerializeField]
	private float minPitch = 0.8f;

	[SerializeField]
	private float maxPitch = 1.2f;



	// Use this for initialization
	void Start () {

		AudioSource source = GetComponent<AudioSource>();

		if (source != null)
		{
			source.pitch = Random.Range (minPitch, maxPitch);
		}
	}
}
