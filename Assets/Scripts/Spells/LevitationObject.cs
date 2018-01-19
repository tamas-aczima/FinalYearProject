using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitationObject : MonoBehaviour {

    Renderer render;
    private bool isAimed = false;

	// Use this for initialization
	void Start () {
        render = GetComponent<Renderer>();
        render.material.SetFloat("Outline", 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (isAimed)
        {
            render.material.SetFloat("Outline", 0.2f);
        }
	}

    public bool IsAimed
    {
        get { return isAimed; }
        set { isAimed = value; }
    }
}
