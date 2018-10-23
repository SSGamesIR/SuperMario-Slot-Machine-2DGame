using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FACE_TYPE{Star=0,Mario=1,Luigi=2,Peach=3,Toadstool=4,Bowser=5}

[System.Serializable]
public class Face {

	public FACE_TYPE faceType;

	public GameObject facePrefab;
     
}

