#pragma warning disable 0414

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour {

    public static Machine instance;

    public int numSlots;
    public float minSlotSpeed;
    public float maxSlotSpeed;

	public float slotSlowdownStartTime;
	public float slotSlowdownStartNext;

	public float slotSlowdownInterval;
	public float slotSlowdownDelta;
	public float slotSlowestSpinSpeed;

    public GameObject slotPrefab;

    public Face[] faces;

    private bool isSpinning = false;

    private GameObject[] slots;

    private int slotsSpinning;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("Machine singleton is already initialized.");
            Destroy(this.gameObject);
        }
        else if (instance != this)
        {
            instance = this;
        }
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Init()
    {
        SpawnSlots();
    }


    public int GetNumFaces()
    {
        return faces.Length;

    }
	public int GetNumSlots()
	{
		return numSlots;
	}

    public Face GetFace(int i)
        
        {
        return faces[i];
        }
        
     public void SpawnSlots()
    {
        slots = new GameObject[numSlots];

        for(int i=0;i<numSlots;i++)
        {
            slots[i] = Instantiate(slotPrefab) as GameObject;

			Slot slotScript = slots [i].GetComponent<Slot> ();

			if (slotScript == null) {
				Debug.Log ("No slot script on object!");
			} else 
			{
				slotScript.Init(i);
			}
        }
    }

	public void StartSpinning()
	{

		for (int i = 0; i < numSlots; i++)
		{
			slots [i].BroadcastMessage ("StartSpinning",Random.Range (minSlotSpeed, maxSlotSpeed));
		}

		slotsSpinning = numSlots;
		isSpinning = true;

		StartCoroutine (SlotSlowdownTimers (slotSlowdownStartTime, slotSlowdownStartNext));
	}

	public void SlotStopped()
	{
		slotsSpinning--;
		if (slotsSpinning == 0) 
		{
			GameManager.instance.ReadyToMatch ();
		}
	}

	public int[] FindMatches()
	{
		int[] faceCountArray;
		faceCountArray = new int[GetNumFaces ()];

		for(int i=0;i<numSlots;i++)
		{
			Slot slotScript = slots [i].GetComponent<Slot> ();

			//faceCountArray [i] = (int)slotScript.GetFaceType ();
			faceCountArray[(int)slotScript.GetFaceType()]++;

		}

		return faceCountArray;

	}

	public List<Vector3>FindStars()
	{
		List<Vector3> locations = new List<Vector3> ();

		for (int i = 0; i < numSlots; i++) 
		{
			Slot slotScript = slots [i].GetComponent<Slot> ();

			if (slotScript.GetFaceType () == FACE_TYPE.Star)
			{
				locations.Add (slotScript.GetPosition ());
			}
		}

		return locations;
	}



	IEnumerator SlotSlowdownTimers(float slotSlowdownStartTime,float slotSlowdownStartNext)
	{
		yield return new WaitForSeconds (slotSlowdownStartTime);

		for (int i = 0; i < numSlots; i++)
		{
			slots [i].BroadcastMessage ("StopSpinning");

			yield return new WaitForSeconds (slotSlowdownStartNext);
		}
		yield break;
	}
}
