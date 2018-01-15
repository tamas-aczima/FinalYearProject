using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    [SerializeField] private bool pickupAble;
    [SerializeField] private bool isLever;
    [SerializeField] private GameObject doorToOpen;
    private bool isTurned = false;

    public bool PickupAble
    {
        get { return pickupAble; }
    }

    public bool IsLever
    {
        get { return isLever; }
    }

    public bool IsTurned
    {
        get { return isTurned; }
        set { isTurned = value; }
    }

    private void Update()
    {
        if (isLever && isTurned && doorToOpen != null)
        {
            Destroy(doorToOpen);
        }
    }
}
