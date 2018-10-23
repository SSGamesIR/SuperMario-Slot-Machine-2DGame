using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheerController : MonoBehaviour {

	public float cheerBoxDelay;

	void OnEnable()
	{
		StartCoroutine ("DisableSelf");
	}

		

	IEnumerator DisableSelf()
	{
		yield return new WaitForSeconds (cheerBoxDelay);

		this.gameObject.SetActive (false);

	}

}
