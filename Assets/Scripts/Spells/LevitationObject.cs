using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitationObject : MonoBehaviour {

    Renderer render;
    private bool isAimed = false;

	void Start () {
        render = GetComponent<Renderer>();
	}
	
	void Update () {
		if (isAimed)
        {
            render.material.SetFloat("_Outline", 0.06f);
        }
        else
        {
            render.material.SetFloat("_Outline", 0);
        }
	}

    public bool IsAimed
    {
        get { return isAimed; }
        set { isAimed = value; }
    }
}
