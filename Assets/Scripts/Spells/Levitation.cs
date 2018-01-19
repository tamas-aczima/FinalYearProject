using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitation : MonoBehaviour {

    private YostSkeletonRig myPrioRig = null;
    private bool isLeft = false;
    private float maxDistance = 100;
    private GameObject levitatingObject = null;
    private bool isLevitating = false;
    private bool isLifted = false;
    Rigidbody rb = null;
    Vector3 handPositionAtPickUp;

    // Use this for initialization
    void Start () {
        myPrioRig = gameObject.transform.root.gameObject.GetComponent<YostSkeletonRig>();
        isLeft = gameObject.name == "LeftHand";
    }
	
	// Update is called once per frame
	void Update () {
        //for outline
        RaycastHit hit;
        Debug.DrawRay(gameObject.transform.position, isLeft ? -transform.right * 50 : transform.right * 50);
        if (Physics.Raycast(gameObject.transform.position, isLeft ? -transform.right : transform.right, out hit, maxDistance))
        {
            if (hit.transform.gameObject.GetComponent<LevitationObject>() != null)
            {
                Debug.Log("levitation object");
                hit.transform.gameObject.GetComponent<LevitationObject>().IsAimed = true;
            }
        }


        if ((myPrioRig.GetJoyStickButtonDown(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_JOYSTICK_BUTTON) && isLeft) ||
            (myPrioRig.GetJoyStickButtonDown(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_JOYSTICK_BUTTON) && !isLeft))
        {
            if (gameObject.transform.rotation.eulerAngles.z < 220 && gameObject.transform.rotation.eulerAngles.z > 100)
            {
                if (Physics.Raycast(transform.position, isLeft ? -transform.right : transform.right, out hit, maxDistance))
                {
                    if (hit.transform.gameObject.GetComponent<LevitationObject>() != null)
                    {
                        //Debug.Log("hit");
                        levitatingObject = hit.transform.gameObject;
                        isLevitating = true;
                        handPositionAtPickUp = gameObject.transform.position;
                        //Debug.Log(handPositionAtPickUp);
                    }
                }
            }
        }
            

        if ((myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_JOYSTICK_BUTTON) && isLeft) ||
            (myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_JOYSTICK_BUTTON) && !isLeft))
        {
            isLevitating = false;
        }

        if (!isLevitating)
        {
            if (levitatingObject != null)
            {
                rb.freezeRotation = false;
                rb.useGravity = true;
                rb = null;
                levitatingObject.transform.parent = null;
                levitatingObject = null;
                isLifted = false;
            }
        }

        if (levitatingObject != null)
        {
            if (!isLifted)
            {
                isLifted = true;
                rb = levitatingObject.GetComponent<Rigidbody>();
                rb.freezeRotation = true;
                rb.useGravity = false;
                levitatingObject.transform.parent = gameObject.transform;
                
            }
            levitatingObject.transform.rotation = Quaternion.identity;
        }
    }
}
