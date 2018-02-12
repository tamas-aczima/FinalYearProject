using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithObjects : MonoBehaviour
{

    private bool isLeft = false;
    private YostSkeletonRig myPrioRig = null;
    private Interactable interactable = null;
    private GameObject heldObject = null;
    private Vector3 handLocationAtGrab = Vector3.zero;
    private bool isGrabbed = false;
    private float grabbedAngle = 0.0f;

    void Start()
    {
        isLeft = gameObject.name == "LeftHand";
        myPrioRig = gameObject.transform.root.gameObject.GetComponent<YostSkeletonRig>();
    }

    void Update()
    {
        HoldObjects();

        UpdateHeldObject();

        TurnLever();

        ReleaseObjects();
    }

    private void HoldObjects()
    {
        //if top button pressed and collided with interactable object, then interact with object
        if ((myPrioRig.GetJoyStickButtonDown(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_TOP_BUTTON) && isLeft && interactable != null) ||
            (myPrioRig.GetJoyStickButtonDown(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_TOP_BUTTON) && !isLeft && interactable != null))
        {
            if (interactable.PickupAble)
            {
                heldObject = interactable.gameObject;
                heldObject.transform.parent = gameObject.transform;
                heldObject.GetComponent<Rigidbody>().useGravity = false;
                Debug.Log(heldObject);
            }
            else if (interactable.IsLever)
            {
                handLocationAtGrab = gameObject.transform.position;
                isGrabbed = true;
                grabbedAngle = interactable.gameObject.transform.parent.eulerAngles.x;
            }
        }
    }

    private void ReleaseObjects()
    {
        //if holding object and button released, then drop object
        if ((heldObject != null && isLeft && myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_TOP_BUTTON)) ||
            (heldObject != null && !isLeft && myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_TOP_BUTTON)))
        {
            heldObject.transform.parent = null;
            heldObject.GetComponent<Rigidbody>().useGravity = true;
            heldObject = null;
        }

        if ((isGrabbed && isLeft && myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_TOP_BUTTON)) ||
            (isGrabbed && !isLeft && myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_TOP_BUTTON)))
        {
            isGrabbed = false;
        }
    }

    private void UpdateHeldObject()
    {
        //if holding object, move it with parent
        if (heldObject != null)
        {
            heldObject.transform.position = gameObject.transform.position;
        }
    }

    private void TurnLever()
    {
        if (!isGrabbed) return;
        Debug.Log("turning");

        Vector3 moveDirection = gameObject.transform.position - handLocationAtGrab;
        if (Mathf.Abs(moveDirection.z) > 0.05)
        {
            float moveAngle = moveDirection.z * 50;
            Debug.Log("angle: " + interactable.gameObject.transform.parent.rotation.eulerAngles.x);
            float angle = interactable.gameObject.transform.parent.rotation.eulerAngles.x;
            angle = (angle > 180) ? angle - 360 : angle;
            if (angle > -35)
            {
                interactable.gameObject.transform.parent.rotation = Quaternion.Euler(new Vector3(grabbedAngle + moveAngle, 0, 0));
            }
            else
            {
                interactable.IsTurned = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Interactable>() != null)
        {
            interactable = collision.gameObject.GetComponent<Interactable>();
            Debug.Log(interactable.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Interactable>() != null && !isGrabbed)
        {
            interactable = null;
        }
    }

}