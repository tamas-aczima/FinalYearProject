using UnityEngine;

public class Interactable : MonoBehaviour {

    [SerializeField] protected bool pickupAble;
    [SerializeField] protected bool isLever;
    [SerializeField] protected bool isChest;

    public bool PickupAble
    {
        get { return pickupAble; }
    }

    public bool IsLever
    {
        get { return isLever; }
    }

    public bool IsChest
    {
        get { return isChest; }
    }
}
