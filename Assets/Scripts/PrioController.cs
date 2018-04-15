using UnityEngine;
using YostSkeletalAPI;

[RequireComponent(typeof(YostSkeletonRig))]
public class PrioController : MonoBehaviour {

    [SerializeField] private float movementSpeed = 0f;
    [SerializeField] private float rotateSpeed = 0f;
    [SerializeField] private Transform thirdPersonCameraTarget;
    [SerializeField] private Transform spine;
    [SerializeField] private float xRotationRange;
    [SerializeField] private float yRotationRange;
    [HideInInspector] public YostSkeletonRig myPrioRig = null;
    private CharacterController characterController = null;
    private Animator anim = null;
    private float strafe = 0f;
    private float forward = 0f;
    private float gravity = 0.0f;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 moveDirectionRelativeToHips = Vector3.zero;
    private float angleX;
    private float angleY;
    private bool isCalibrated = false;

    void Awake() {
        DontDestroyOnLoad(gameObject);    
    }

    void Start () {
		//Get the relevant components from the character
        myPrioRig = GetComponent<YostSkeletonRig>();
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
	}

	void Update () {
        Calibration();
        Move();
        Rotate();
    }

    private void Calibration() {
        //Press "t" to start calibration
        if (Input.GetKeyDown("t") && !isCalibrated) {
            //Starts calibration after a wait time of 2.0 seconds
            isCalibrated = true;
            StartCoroutine(myPrioRig.CalibrateSens(2.0f));
        }

        // Recenter's the player and allow for recalibrate
        if (Input.GetKeyDown(KeyCode.Space)) {
            transform.position = Vector3.zero;
            isCalibrated = false;
        }

        if (Input.GetKeyDown(KeyCode.X)) {
            myPrioRig.StopStreaming();
        }
    }

    private void Move() {
        //reset movement from previous frame
        forward = 0.0f;
        strafe = 0.0f;
        moveDirection = Vector3.zero;
        moveDirectionRelativeToHips = Vector3.zero;

        //get input
        forward = Input.GetAxis("Vertical");
        strafe = Input.GetAxis("Horizontal");

        //set animator values
        anim.SetFloat("forward", forward);
        anim.SetFloat("strafe", strafe);

        if (forward != 0 || strafe != 0) {
            anim.SetBool("moving", true);
        }
        else {
            anim.SetBool("moving", false);
        }

        //gravity
        if (characterController.isGrounded) {
            gravity = 0.0f;
        }
        else {
            gravity -= Mathf.Sqrt(Time.deltaTime);
        }

        //movement
        moveDirection = new Vector3(strafe, 0, forward);
        moveDirection.Normalize();
        moveDirection.y = gravity;

        if (strafe != 0.0f || forward != 0.0f || !characterController.isGrounded) {
            moveDirectionRelativeToHips = Quaternion.Euler(0, spine.transform.rotation.eulerAngles.y, 0) * moveDirection;
            moveDirectionRelativeToHips = transform.TransformDirection(moveDirectionRelativeToHips);
            characterController.Move(moveDirectionRelativeToHips * movementSpeed * Time.deltaTime);
        }
    }

    private void Rotate() {
        //x component
        if (myPrioRig.GetJoyStickAxis(YOST_SKELETON_JOYSTICK_AXIS.YOST_SKELETON_RIGHT_Y_AXIS) != 0) {
            angleX = thirdPersonCameraTarget.rotation.eulerAngles.x;
            angleX -= myPrioRig.GetJoyStickAxis(YOST_SKELETON_JOYSTICK_AXIS.YOST_SKELETON_RIGHT_Y_AXIS) * rotateSpeed * Time.deltaTime;
            angleX = (angleX > 180) ? angleX - 360 : angleX;
            angleX = Mathf.Clamp(angleX, -xRotationRange, xRotationRange);
        }

        //y component
        if (myPrioRig.GetJoyStickAxis(YOST_SKELETON_JOYSTICK_AXIS.YOST_SKELETON_RIGHT_X_AXIS) != 0) {
            angleY = thirdPersonCameraTarget.rotation.eulerAngles.y;
            angleY += myPrioRig.GetJoyStickAxis(YOST_SKELETON_JOYSTICK_AXIS.YOST_SKELETON_RIGHT_X_AXIS) * rotateSpeed * Time.deltaTime;
            angleY = (angleY > 180) ? angleY - 360 : angleY;
            angleY = Mathf.Clamp(angleY, -yRotationRange, yRotationRange);
        }

        //rotate 
        thirdPersonCameraTarget.rotation = Quaternion.Euler(new Vector3(angleX, angleY, 0));
    }

    private void OnApplicationQuit() { 
        myPrioRig.StopStreaming();
    }

    public Transform ThirdPersonCameraTarget
    {
        get { return thirdPersonCameraTarget; }
    }
}