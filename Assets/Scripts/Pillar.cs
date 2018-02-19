using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour {

    private int id;
    private Vector3 startPos;

	// Use this for initialization
	void Start () {
		switch (id)
        {
            case 0:
                GetComponent<Renderer>().material.color = Color.yellow;
                break;
            case 1:
                GetComponent<Renderer>().material.color = Color.red;
                break;
            case 2:
                GetComponent<Renderer>().material.color = Color.blue;
                break;
            case 3:
                GetComponent<Renderer>().material.color = Color.green;
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    public Vector3 StartPos
    {
        get { return startPos; }
        set { startPos = value; }
    } 
}
