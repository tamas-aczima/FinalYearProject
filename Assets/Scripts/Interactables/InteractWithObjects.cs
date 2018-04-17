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
    [SerializeField] private float chestOpenAngle;
    [SerializeField] private float leverTurnAngle;

    void Start()
    {
        isLeft = gameObject.name == "LeftHand";
        myPrioRig = gameObject.transform.root.gameObject.GetComponent<YostSkeletonRig>();
    }

    void Update()
    {
        HoldObjects();
        UpdateHeldObject();
        ReleaseObjects();
        TurnLever();
        OpenChest();
    }

    private void HoldObjects() {
        //if top button pressed and collided with interact able object, then interact with object
        if ((myPrioRig.GetJoyStickButtonDown(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_TOP_BUTTON) && isLeft && interactable != null) ||
            (myPrioRig.GetJoyStickButtonDown(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_TOP_BUTTON) && !isLeft && interactable != null)) {
            //check object type and set some variables
            if (interactable.PickupAble) {
                heldObject = interactable.gameObject;
                heldObject.transform.parent = gameObject.transform;
                heldObject.GetComponent<Rigidbody>().useGravity = false;
            }
            else if (interactable.IsLever) {
                handLocationAtGrab = gameObject.transform.position;
                isGrabbed = true;
                grabbedAngle = interactable.gameObject.transform.parent.eulerAngles.x;
            }
            else if (interactable.IsChest) {
                handLocationAtGrab = gameObject.transform.position;
                isGrabbed = true;
                grabbedAngle = interactable.gameObject.transform.eulerAngles.x;
            }
        }
    }

    private void ReleaseObjects() {
        //if holding object and button released, then drop object
        if ((heldObject != null && isLeft && myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_TOP_BUTTON)) ||
            (heldObject != null && !isLeft && myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_TOP_BUTTON))) {
            heldObject.transform.parent = null;
            heldObject.GetComponent<Rigidbody>().useGravity = true;
            heldObject = null;
        }

        if ((isGrabbed && isLeft && myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_TOP_BUTTON)) ||
            (isGrabbed && !isLeft && myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_TOP_BUTTON))) {
            isGrabbed = false;
        }
    }

    private void UpdateHeldObject() {
        //if holding object, move it with parent
        if (heldObject != null) {
            heldObject.transform.position = gameObject.transform.position;
        }
    }

    private void TurnLever() {
        if (interactable == null || !isGrabbed || !interactable.IsLever) return;

        Lever lever = interactable as Lever;

        Vector3 moveDirection = gameObject.transform.position - handLocationAtGrab;
        float moveAngle = moveDirection.z * 50;
        float angle = lever.gameObject.transform.parent.rotation.eulerAngles.x;
        angle = (angle > 180) ? angle - 360 : angle;
        if (angle > leverTurnAngle) {
            lever.gameObject.transform.parent.rotation = Quaternion.Euler(new Vector3(grabbedAngle + moveAngle, 0, 0));
        }
        else {
            lever.IsTurned = true;
        }
    }

    private void OpenChest() {
        if (interactable == null || !isGrabbed || !interactable.IsChest) return;

        //cast to child 
        Chest chest = interactable as Chest;

        //get movement from hand
        Vector3 moveDirection = gameObject.transform.position - handLocationAtGrab;
        float moveAngle = moveDirection.y * 100;

        //angle of lid
        float angle = chest.gameObject.transform.localEulerAngles.x;
        angle = (angle > 180) ? angle - 360 : angle;

        //check angle of lid, if angle is greater than the required angle rotate and play sound
        if (angle > chestOpenAngle) {
            Vector3 chestAngles = chest.gameObject.transform.eulerAngles;
            if (!chest.HasPlayedSound) {
                AudioSource chestSource = chest.gameObject.GetComponent<AudioSource>();
                chestSource.PlayOneShot(chestSource.clip);
                chest.HasPlayedSound = true;
            }
            chest.gameObject.transform.rotation = Quaternion.Euler(new Vector3(-moveAngle, chestAngles.y, chestAngles.z));

            //limit rotation to 90 degrees so lid does not move down
            if (angle > 90) {
                chest.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(90, chestAngles.y, chestAngles.z));
            }
        }
        //if reached requred angle set chest to be opened
        else {
            chest.IsOpen = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<Interactable>() != null) {
            interactable = other.gameObject.GetComponent<Interactable>();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.GetComponent<Interactable>() != null && !isGrabbed) {
            interactable = null;
        }
    }
}