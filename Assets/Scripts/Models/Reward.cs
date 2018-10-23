using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum REWARD_TYPE{WinCoins=0,LoseCoins=1,Multiplier=2}

[System.Serializable]
public class Reward {

	public FACE_TYPE faceType;

	public int reqMatches;

	public REWARD_TYPE rewardType;

	public int rewardAmount;


}

