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
    private Animator anim = null;
    private float strafe = 0f;
    private float forward = 0f;
    private Vector3 moveDirection = Vector3.zero;
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

        //move
        forward = Input.GetAxis("Vertical");
        strafe = Input.GetAxis("Horizontal");
        anim.SetFloat("forward", forward);
        anim.SetFloat("strafe", strafe);

        if (forward != 0 || strafe != 0)
        {
            anim.SetBool("moving", true);
        }
        else
        {
            anim.SetBool("moving", false);
        }

        moveDirection = new Vector3(strafe, 0, forward);
        moveDirection = transform.TransformDirection(moveDirection);
        characterController.Move(moveDirection * movementSpeed * Time.deltaTime);

        //rotate
        if (myPrioRig.GetJoyStickAxis(YOST_SKELETON_JOYSTICK_AXIS.YOST_SKELETON_RIGHT_X_AXIS) != 0)
        {
            transform.Rotate(new Vector3(0, myPrioRig.GetJoyStickAxis(YOST_SKELETON_JOYSTICK_AXIS.YOST_SKELETON_RIGHT_X_AXIS), 0) * rotateSpeed * Time.deltaTime);
        }
    }

    private void OnApplicationQuit()
    {
        myPrioRig.StopStreaming();
    }
}