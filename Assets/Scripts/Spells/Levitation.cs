using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Levitation : MonoBehaviour {

    private YostSkeletonRig myPrioRig = null;
    private bool isLeft = false;
    private float maxDistance = 50;
    private int maxHits = 3;
    private GameObject levitatingObject = null;
    private bool isLevitating = false;
    private bool isLifted = false;
    Rigidbody rb = null;
    Vector3 handPositionAtPickUp;
    GameObject levitationObject;
    Transform levitationObjectParent;
    GameObject[] hits;

    // Use this for initialization
    void Start () {
        myPrioRig = gameObject.transform.root.gameObject.GetComponent<YostSkeletonRig>();
        isLeft = gameObject.name == "LeftHand";
        hits = new GameObject[maxHits];
    }
	
	// Update is called once per frame
	void Update () {

        RayCast();

        CheckNoLevitationObject();

        LevitateObject();
    }

    private void RayCast()
    {
        //for debugging
        //for (int i = 0; i < maxHits; i++)
        //{
        //    Debug.DrawRay(gameObject.transform.position + new Vector3((i - 1) / 1.5f, 0, 0), isLeft ? -transform.right * maxDistance : transform.right * maxDistance);
        //}

        for (int i = 0; i < maxHits; i++)
        {
            Ray ray = new Ray(gameObject.transform.position + new Vector3((i - 1) / 1.5f, 0, 0), isLeft ? -transform.right : transform.right);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                if (hit.transform.gameObject != null)
                {
                    hits[i] = hit.transform.gameObject;
                }
                
                if (hit.transform.gameObject.GetComponent<LevitationObject>() != null)
                {
                    levitationObject = hit.transform.gameObject;
                    levitationObject.GetComponent<LevitationObject>().IsAimed = true;
                }
            }
        }
    }

    private void CheckNoLevitationObject()
    {
        if (hits.All(hit => hit.gameObject.GetComponent<LevitationObject>() == null))
        {
            if (levitationObject != null && !isLevitating)
            {
                levitationObject.GetComponent<LevitationObject>().IsAimed = false;
                levitationObject = null;
            }
        }
    }

    private void LevitateObject()
    {
        if (levitationObject == null) return;

        if (!isLevitating)
        {
            if ((myPrioRig.GetJoyStickButtonDown(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_JOYSTICK_BUTTON) && isLeft) ||
            (myPrioRig.GetJoyStickButtonDown(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_JOYSTICK_BUTTON) && !isLeft))
            {
                if (gameObject.transform.rotation.eulerAngles.z < 200 && gameObject.transform.rotation.eulerAngles.z > 160)
                {
                    isLevitating = true;
                    handPositionAtPickUp = gameObject.transform.position;
                    rb = levitationObject.GetComponent<Rigidbody>();
                    //rb.freezeRotation = true;
                    rb.useGravity = false;
                    levitationObjectParent = levitationObject.transform.parent;
                    levitationObject.transform.parent = gameObject.transform;
                }
            }
        }
        
        else if (isLevitating)
        {
            levitationObject.transform.rotation = Quaternion.identity;

            if ((myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_JOYSTICK_BUTTON) && isLeft) ||
            (myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_JOYSTICK_BUTTON) && !isLeft))
            {
                isLevitating = false;
                //rb.freezeRotation = false;
                rb.useGravity = true;
                rb = null;
                levitationObject.transform.parent = levitationObjectParent;
            }
        } 
    }
}
