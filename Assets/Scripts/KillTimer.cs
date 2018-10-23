using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTimer : MonoBehaviour {

	public float seconds;


	// Use this for initialization
	void Start () {

		StartCoroutine (killAfterTime(seconds));
		
	}

	IEnumerator killAfterTime(float seconds)
	{
		yield return new WaitForSeconds (seconds);

		Destroy (gameObject);
	
	}



}
