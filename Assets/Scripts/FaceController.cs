#pragma warning disable 0414

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceController : MonoBehaviour {

	public FACE_TYPE faceType;

	private Slot slotRef;

	private bool isSpinning=false;
	private bool isStopping=false;
	private bool isSlowing=false;

	//stop position on the y axis
	private float stopPoint = 0;

	private float spinSpeed = 0;


	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "ClickTrigger")
		{
			SoundManager.instance.PlaySlotClick (transform.position);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (spinSpeed < Machine.instance.slotSlowestSpinSpeed)
		{
			spinSpeed = Machine.instance.slotSlowestSpinSpeed;
			isSlowing = false;
			isStopping = true;

			stopPoint = Mathf.Floor (transform.position.y);


		}

		if (isSpinning) {
			transform.Translate (Vector3.down * Time.deltaTime * spinSpeed, Space.World);	
			if (!isStopping) {
				if (transform.position.y < 0) {

					transform.position += new Vector3 (0, Machine.instance.GetNumFaces (), 0);
				}
		
			}
		}

		if (isStopping)
		{
			if (transform.position.y <= stopPoint)
			{
				transform.position = new Vector3 (transform.position.x, stopPoint, transform.position.z);
			
				isSpinning = false;
				isStopping = false;
				stopPoint = 0;

				slotRef.StoppedSpinning ();
			}

		}

	}

	public void StartSpinning(float slotSpeed)
	{
		StopAllCoroutines();
	//	Debug.Log ("Face controller received message to spin!");
	
		isSpinning = true;
		isStopping = false;
		isSlowing = false;


		spinSpeed = slotSpeed;
	}

	public void StopSpinning()
	{
		isSlowing = true;

		StartCoroutine (SlowSpinOverTime (Machine.instance.slotSlowdownInterval, Machine.instance.slotSlowdownDelta));
	}

	public FACE_TYPE GetFaceType()
	{
		return(faceType);
	}

	public void SetSlotRef(Slot slot)
	{
		slotRef = slot;

	}

	public Vector3 GetPosition()
	{
		return transform.position;

	}


	IEnumerator SlowSpinOverTime(float interval,float delta)
	{
		while(true)
		{

			yield return new WaitForSeconds (interval);

				if(isStopping || !isSpinning) yield break;

				spinSpeed -= delta;
			
		}
	}
}
