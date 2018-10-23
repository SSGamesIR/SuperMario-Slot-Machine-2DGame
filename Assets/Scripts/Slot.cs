#pragma warning disable 0414

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class Slot : MonoBehaviour {

	private const float CENTER = 3.0f;

    private int slotNumber;

    private bool isSpinning = false;

    private GameObject[] faces;

    public void Init(int slotNumber)
    {
        this.slotNumber = slotNumber;

        faces = new GameObject[Machine.instance.GetNumFaces()];

        for (int i = 0; i < Machine.instance.GetNumFaces(); i++)
        {

            faces[i] = Instantiate(Machine.instance.GetFace(i).facePrefab) as GameObject;

			faces [i].transform.position += new Vector3 ((float)slotNumber, i, 0);


			faces [i].transform.parent = this.gameObject.transform;

			FaceController faceScript = faces [i].GetComponent<FaceController> ();
			faceScript.SetSlotRef (this);
        }
    }

	public void StartSpinning()
	{
		isSpinning = true;
//		Debug.Log ("Slot received message to spin!");
	}
	public void StoppedSpinning()
	{
		if (isSpinning)
		{
			isSpinning = false;

			Machine.instance.SlotStopped ();
		}
	}


	//return face type of center image in slot
	public FACE_TYPE GetFaceType()
	{
		FACE_TYPE faceType = 0;

		for (int i = 0; i < Machine.instance.GetNumFaces (); i++) 
		{
			if (Mathf.Round(faces [i].transform.position.y) == CENTER)
			{
				faceType = faces [i].GetComponent<FaceController> ().GetFaceType ();
				return faceType;
			}

		}
		Debug.Log ("Error! Returned default facetype. This shuoldn't happen!");

		return faceType;
	}		

	public Vector3 GetPosition()
	{

		Vector3 position = new Vector3 ();

		for (int i = 0; i < Machine.instance.GetNumFaces (); i++) 
		{
			if (Mathf.Round(faces [i].transform.position.y) == CENTER)
			{
				position=faces[i].GetComponent<FaceController>().GetPosition();

			}

		}

		return position;
	}
}