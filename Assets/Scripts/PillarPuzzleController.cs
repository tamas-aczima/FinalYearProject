﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PillarPuzzleController : MonoBehaviour {

    [SerializeField] private GameObject pillarPrefab;
    [SerializeField] private GameObject pedestalPrefab;
    [SerializeField] private Transform[] pillarLocations;
    [SerializeField] private Transform[] pedestalLocations;
    [SerializeField] private GameObject lever;
    private List<GameObject> pedestals = new List<GameObject>();
    private List<GameObject> pillars = new List<GameObject>();
    private List<int> assignedPillarLocations = new List<int>();
    private List<int> assignedPedestalLocations = new List<int>();

    // Use this for initialization
    void Start () {
        //place pillars
		for (int i = 0; i < pillarLocations.Length; i++)
        {
            //instantiate game object
            GameObject pillar = Instantiate(pillarPrefab, gameObject.transform);
            //set id for game object
            pillar.GetComponent<Pillar>().ID = i;
            //add to list
            pillars.Add(pillar);

            //check for available location
            int arrayLoc;
            bool newLoc = false;
            while (!newLoc)
            {
                arrayLoc = Random.Range(0, pillarLocations.Length);
                //if the location has not been used, then use it
                if (assignedPillarLocations.IndexOf(arrayLoc) == -1)
                {
                    assignedPillarLocations.Add(arrayLoc);
                    pillar.transform.position = pillarLocations[arrayLoc].position;
                    pillar.GetComponent<Pillar>().StartPos = pillar.transform.position;
                    newLoc = true;
                }
            }
        }

        //place pedestals
        for (int i = 0; i < pedestalLocations.Length; i++)
        {
            GameObject pedestal = Instantiate(pedestalPrefab);
            pedestal.GetComponent<Pedestal>().ID = i;
            pedestals.Add(pedestal);

            int arrayLoc;
            bool newLoc = false;
            while (!newLoc)
            {
                arrayLoc = Random.Range(0, pedestalLocations.Length);
                if (assignedPedestalLocations.IndexOf(arrayLoc) == -1)
                {
                    assignedPedestalLocations.Add(arrayLoc);
                    pedestal.transform.position = pedestalLocations[arrayLoc].position;
                    newLoc = true;
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        CheckMatches();
	}

    private void CheckMatches()
    {
        for (int i = 0; i < pedestals.Count; i++)
        {
            if (!pedestals.ElementAt(i).GetComponent<Pedestal>().Match)
            {
                return;
            }
        }

        lever.SetActive(true);
        Debug.Log("Puzzle complete");
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (GameObject pillar in pillars)
        {
            if (pillar.gameObject == other.gameObject)
            {
                pillar.transform.position = pillar.GetComponent<Pillar>().StartPos;
            }
        }
    }
}
