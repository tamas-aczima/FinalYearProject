using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using YostSkeletalAPI;

[RequireComponent(typeof(YostSkeletonRig))]

public class PrioController : MonoBehaviour 
{
    private CharacterController characterController = null;
    [SerializeField] private float movementSpeed = 0f;
    [SerializeField] private float rotateSpeed = 0f;
    [SerializeField] private Transform thirdPersonCameraTarget;
    [SerializeField] private Transform hips;
    [SerializeField] private float xRotationRange;
    [SerializeField] private float yRotationRange;
    private Animator anim = null;
    private float strafe = 0f;
    private float forward = 0f;
    private float gravity = 0.0f;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 moveDirectionRelativeToHips = Vector3.zero;
	//PrioRig Component
    YostSkeletonRig myPrioRig = null;
    bool isCalibrated = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);    
    }

    // Use this for initialization
    void Start () 
	{
		//Get the PrioRig component from the character
        myPrioRig = GetComponent<YostSkeletonRig>();
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
        //Press "t" to start calibration
        if (Input.GetKeyDown("t") && !isCalibrated)
        {
            //Starts calibration after a wait time of 2.0 seconds
            isCalibrated = true;
            StartCoroutine(myPrioRig.CalibrateSens(2.0f));
        }

        // Recenter's the player and allow for recalibrate
        if( Input.GetKeyDown(KeyCode.Space) )
        {
            transform.position = Vector3.zero;
            isCalibrated = false;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            myPrioRig.StopStreaming();
        }

        //reset movement from previous frame
        forward = 0.0f;
        strafe = 0.0f;
        moveDirection = Vector3.zero;
        moveDirectionRelativeToHips = Vector3.zero;

        //move
        forward = Input.GetAxis("Vertical");
        strafe = Input.GetAxis("Horizontal");
        //anim.SetFloat("forward", forward);
        //anim.SetFloat("strafe", strafe);

        //if (forward != 0 || strafe != 0)
        //{
        //    anim.SetBool("moving", true);
        //}
        //else
        //{
        //    anim.SetBool("moving", false);
        //}

        if (characterController.isGrounded)
        {
            gravity = 0.0f;
        }
        gravity -= Mathf.Sqrt(Time.deltaTime);

        

        moveDirection = new Vector3(strafe, gravity, forward);
        if (strafe != 0.0f || forward != 0.0f)
        {
            moveDirectionRelativeToHips = Quaternion.Euler(0, hips.transform.rotation.eulerAngles.y, 0) * moveDirection;
            moveDirectionRelativeToHips = transform.TransformDirection(moveDirectionRelativeToHips);
            characterController.Move(moveDirectionRelativeToHips * movementSpeed * Time.deltaTime);
        }


        //rotate
        if (myPrioRig.GetJoyStickAxis(YOST_SKELETON_JOYSTICK_AXIS.YOST_SKELETON_RIGHT_X_AXIS) != 0)
        {
            float angle = thirdPersonCameraTarget.rotation.eulerAngles.y;
            angle += myPrioRig.GetJoyStickAxis(YOST_SKELETON_JOYSTICK_AXIS.YOST_SKELETON_RIGHT_X_AXIS) * rotateSpeed * Time.deltaTime;

            if (angle > 180)
            {
                angle -= 360;
            }

            angle = Mathf.Clamp(angle, -yRotationRange, yRotationRange);

            thirdPersonCameraTarget.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }

        if (myPrioRig.GetJoyStickAxis(YOST_SKELETON_JOYSTICK_AXIS.YOST_SKELETON_RIGHT_Y_AXIS) != 0)
        {
            float angle = thirdPersonCameraTarget.rotation.eulerAngles.x;
            angle += myPrioRig.GetJoyStickAxis(YOST_SKELETON_JOYSTICK_AXIS.YOST_SKELETON_RIGHT_Y_AXIS) * rotateSpeed * Time.deltaTime;

            if (angle > 180)
            {
                angle -= 360;
            }

            angle = Mathf.Clamp(angle, -xRotationRange, xRotationRange);

            thirdPersonCameraTarget.rotation = Quaternion.Euler(new Vector3(angle, 0, 0));
        }
    }

    private void OnApplicationQuit()
    {
        myPrioRig.StopStreaming();
    }
}