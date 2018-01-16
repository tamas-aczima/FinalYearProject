using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour {

    private int id;
    private bool match = false;

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
        //Debug.Log(match);
	}

    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    public bool Match
    {
        get { return match; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Pillar>() != null)
        {
            if (id == other.gameObject.GetComponent<Pillar>().ID)
            {
                match = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Pillar>() != null && match)
        {
            match = false;
        }
    }
}
