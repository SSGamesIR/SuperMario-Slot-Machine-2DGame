using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TitleManager : MonoBehaviour {


	public AudioSource startSound;

	public GameObject fader;

	public void PlayButtonPressed()
	{
		startSound.Play();
		fader.SetActive (true);
	}


	public void StartGame()
	{

		SceneManager.LoadScene (1);  

	}

	void Update(){
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.Quit(); 
	}


}