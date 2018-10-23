#pragma warning disable 0414
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameObject machinePrefab;

	public Reward[] rewards;

	public int startingCoins;

	public int costPerSpin;

	public GameObject cheerBoxUI;

	public Text cheerTextUI;

	public float cheerDelay;

	public Text coinsTextUI;

	public Text costTextUI;

	public GameObject faderUI;

	public GameObject cheerConfettiPrefab;

	public GameObject mainCam;

	public GameObject starFireworksPrefab;

	private bool canSpin=true;

	private int playerCoins;


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("GameManager singleton is already initialized.");
            Destroy(this.gameObject);
        }
        else if (instance != this)
        {
            instance = this;
		}

        Instantiate(machinePrefab);

        Machine.instance.Init();
    }

    // Use this for initialization
    void Start () {

		playerCoins = startingCoins;

 		coinsTextUI.text = playerCoins.ToString ();

		costTextUI.text = costPerSpin.ToString();

		Debug.Log ("Player given " + startingCoins + " coins to start!");

	}
	
	// Update is called once per frame
	void Update () {


		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			Application.Quit (); 
		
		}

		if (Input.GetKeyDown ("m")) 
		{
			int[] matches;

			matches = Machine.instance.FindMatches ();

			for (int i = 0; i < Machine.instance.GetNumFaces (); i++) 
			{
				Debug.Log (matches [i]);
			}
		}
	}

	public void SpinButtonPressed()
	{
		if (canSpin && playerCoins >= costPerSpin) 
		{
			playerCoins -= costPerSpin;

			coinsTextUI.text = playerCoins.ToString ();

			Debug.Log ("Player spent " + costPerSpin + " coins to spin!");

			canSpin = false;

			Machine.instance.StartSpinning ();
		}

	}


	public void ReadyToMatch()
	{
		Debug.Log ("All slots have stopped spinning. Ready to match!");

		StartCoroutine ("Rewards");
	}

	public void CheckCoins()
	{
		if (playerCoins <= 0) 
		{
			playerCoins = 0;

			faderUI.SetActive (true);
		}

	}

	public void GameOver()
	{
		SceneManager.LoadScene (2);

	}

	public void StarFireworks()
	{
		List<Vector3> positions = new List <Vector3> ();

		positions.AddRange (Machine.instance.FindStars ());

		foreach (Vector3 position in positions)
		{
			Instantiate (starFireworksPrefab, position, starFireworksPrefab.transform.rotation);

		}
	}



	public void ProcessReward(REWARD_TYPE rewardType, int rewardCount,FACE_TYPE faceType,int facecount)
	{
		string strCheer = "";
		switch (rewardType)
		{
		case REWARD_TYPE.WinCoins:
			playerCoins += rewardCount;
			GameObject confetti = Instantiate (cheerConfettiPrefab, mainCam.transform);
			strCheer += "You Win ";

			SoundManager.instance.StopGameMusic ();
			SoundManager.instance.PlayerWinMoney ();

			break;

		case REWARD_TYPE.Multiplier:
			StarFireworks ();
			playerCoins += rewardCount;
			strCheer += "BONUS MULTIPLIER! You Win ";

			SoundManager.instance.PlayerStars ();
			SoundManager.instance.StopGameMusic ();
			SoundManager.instance.PlayStarFireworks ();

			break;

		case REWARD_TYPE.LoseCoins:
			playerCoins -= rewardCount;
			CheckCoins ();
			strCheer += "Oh no! You lost ";

			SoundManager.instance.StopGameMusic ();
			SoundManager.instance.PlayerLoseMoney ();

			break;

		

		default:
			break;
		}

		coinsTextUI.text = playerCoins.ToString ();


		strCheer += rewardCount;

		if (rewardType == REWARD_TYPE.Multiplier) {
			strCheer += " extra coins for having ";
		} else if (rewardType == REWARD_TYPE.LoseCoins) {
			strCheer += " coins for having ";
		} else 
		{
			strCheer += "coins for matching ";
		}

		strCheer += facecount;
		strCheer += " " + faceType.ToString() + "s!";


		Debug.Log (strCheer);

		cheerTextUI.text = strCheer;

		cheerBoxUI.SetActive (true);


	}



	IEnumerator Rewards()
	{
		int[] matches;

		int starCount = 0;

		int multiplier = 0;

		int rewardTotal = 0;

		matches = Machine.instance.FindMatches ();

		yield return new WaitForSeconds (1);

		for (int i = 0; i < Machine.instance.GetNumFaces (); i++ )
		{
			foreach (Reward reward in rewards)
			{
				if (reward.faceType == (FACE_TYPE)i && reward.reqMatches == matches [i]) 
				{
					if (reward.faceType != FACE_TYPE.Star) 
					{
						if (reward.rewardType == REWARD_TYPE.WinCoins) 
						{
							rewardTotal += reward.rewardAmount;
						}

					//	Debug.Log ("Matched " + matches [i] + ((FACE_TYPE)i).ToString () + "." + reward.rewardType.ToString () + " " + reward.rewardAmount);	
					
						ProcessReward (reward.rewardType,reward.rewardAmount,reward.faceType,matches[i]);
					
						yield return new WaitForSeconds (cheerDelay);
					
					} else
					{
						multiplier = reward.rewardAmount;

						starCount = matches [i];
					}
				}
			}

		}

		if (multiplier > 0 && starCount > 0 && rewardTotal > 0) 
		{
			//Debug.Log ("Reward amount " + rewardTotal + " has been multipiled by " + multiplier + " star to equal " + (rewardTotal * multiplier) + ".");	

			ProcessReward (REWARD_TYPE.Multiplier, (rewardTotal * multiplier),FACE_TYPE.Star,starCount);

			yield return new WaitForSeconds (cheerDelay);
		
		}

		canSpin = true;
		
		if (!SoundManager.instance.MusicPlaying ()) 
		{
			SoundManager.instance.PlayGameMusic ();
		}
	}
}