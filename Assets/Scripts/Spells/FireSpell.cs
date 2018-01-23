using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpell : MonoBehaviour {

    private bool isLeft = false;
    private bool isCasting = false;
    private bool isFirstTriggerTouched = false;
    private bool isSecondTriggerTouched = false;
    private YostSkeletonRig myPrioRig = null;
    [SerializeField] private GameObject firstTrigger;
    [SerializeField] private GameObject secondTrigger;
    [SerializeField] private GameObject fireBallPrefab;
    [SerializeField] private Transform fireBallLocation;
    private GameObject fireBall = null;
    private Vector3 lastHandPosition = Vector3.zero;
    private Vector3 velocity = Vector3.zero;

    // Use this for initialization
    void Start () {
        myPrioRig = gameObject.transform.root.gameObject.GetComponent<YostSkeletonRig>();
        isLeft = gameObject.name == "LeftHand";
    }
	
	// Update is called once per frame
	void Update () {
        //if button pressed, then start casting
        if ((myPrioRig.GetJoyStickButtonDown(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_JOYSTICK_BUTTON) && isLeft) ||
            (myPrioRig.GetJoyStickButtonDown(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_JOYSTICK_BUTTON) && !isLeft))
        {
            isCasting = true;
            //Debug.Log("Start casting");
        }

        //if button released, then stop casting
        if ((myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_JOYSTICK_BUTTON) && isLeft && isCasting) ||
            (myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_JOYSTICK_BUTTON) && !isLeft && isCasting))
        {
            isCasting = false;
            //Debug.Log("Stop casting");
        }

        //if casting and triggers touched, then create fire ball 
        if (isCasting && isFirstTriggerTouched && isSecondTriggerTouched && fireBall == null)
        {
            fireBall = Instantiate(fireBallPrefab, fireBallLocation.position, Quaternion.identity, fireBallLocation);
        }

        //if there is a fireball, check hand velocity for shooting
        if (fireBall != null)
        {
            Vector3 currentHandPosition = gameObject.transform.position;
            if (currentHandPosition.z < lastHandPosition.z)
            {
                velocity = Vector3.zero;
                //Debug.Log("back");
            }
            else if (currentHandPosition.z > lastHandPosition.z)
            {
                //Debug.Log("forward");
                Vector3 direction = currentHandPosition - lastHandPosition;
                velocity += direction;
                //Debug.Log(velocity);
            }
            lastHandPosition = currentHandPosition;
        }

        //if fireball is created and button released, then shoot fire ball
        if ((myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_JOYSTICK_BUTTON) && isLeft && fireBall != null) ||
            (myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_JOYSTICK_BUTTON) && !isLeft && fireBall != null))
        {
            //Debug.Log("shoot fireball");
            //Debug.Log(myPrioRig.GetBoneVelocity("RightHand"));
            //Vector3 velocity = myPrioRig.GetBoneVelocity("RightHand");
            velocity.Normalize();
            fireBall.GetComponent<Rigidbody>().velocity = velocity * 10;
            fireBall.GetComponent<Rigidbody>().isKinematic = true;
            fireBall.transform.parent = null;
            fireBall = null;
            isFirstTriggerTouched = false;
            isSecondTriggerTouched = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCasting)
        {
            if (other.gameObject == firstTrigger)
            {
                isFirstTriggerTouched = true;
                //Debug.Log("first touched");
            }
            if (other.gameObject == secondTrigger)
            {
                isSecondTriggerTouched = true;
                //Debug.Log("second touched");
            }
        }
    }
}
