using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public static SoundManager instance;

	public AudioSource gameMusic;

	public AudioSource starsMusic;

	public AudioSource winMoneySound;

	public AudioSource loseMoneySound;

	public AudioSource StarFireworksSound;

	public AudioClip clickSound;


	void Awake()
	{
		if (!instance) {
			instance = this;
		} else
		{
			Debug.Log("Sound manager instance already initialized");
		}
	}

	public void PlayerLoseMoney()
	{
		loseMoneySound.Play ();
	}

	public void PlayerWinMoney()
	{
		winMoneySound.Play ();
	}

	public void PlayerStars()
	{
		starsMusic.Play ();
	}

	public void PlayGameMusic()
	{
		gameMusic.Play ();
	}

	public void StopGameMusic()
	{
		gameMusic.Pause ();
	}

	public void PlayStarFireworks()
	{
		StarFireworksSound.Play ();
	}

	public bool MusicPlaying()
	{
		return gameMusic.isPlaying;
	}

	public void PlaySlotClick(Vector3 positionToPlayForm)
	{
		AudioSource.PlayClipAtPoint (clickSound, positionToPlayForm);
	}

}