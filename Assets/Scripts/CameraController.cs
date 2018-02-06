using UnityEngine;
using UnityEngine.VR;

public class CameraController : MonoBehaviour {

    private GameObject activeCamera;
    [SerializeField] private GameObject firstPersonCamera;
    [SerializeField] private GameObject thirdPersonCamera;
    [SerializeField] private GameObject vrCamera;
    private bool cameraChanged = false;
    private bool isVR;

    void Start () {
        isVR = VRDevice.isPresent;

        if (isVR)
        {
            activeCamera = vrCamera;
        }
        else
        {
            activeCamera = thirdPersonCamera;
        }

        activeCamera.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
        ChangeCamera();
    }

    private void ChangeCamera()
    {
        if (isVR) return;

        if (activeCamera == firstPersonCamera)
        {
            firstPersonCamera.SetActive(true);
            thirdPersonCamera.SetActive(false);
        }
        else
        {
            thirdPersonCamera.SetActive(true);
            firstPersonCamera.SetActive(false);
        }

        if (gameObject.GetComponent<YostSkeletonRig>().GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_X_BUTTON))
        {
            cameraChanged = false;
        }

        if (!cameraChanged && gameObject.GetComponent<YostSkeletonRig>().GetJoyStickButtonDown(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_X_BUTTON))
        {
            cameraChanged = true;

            if (activeCamera == firstPersonCamera)
            {
                activeCamera = thirdPersonCamera;
            }
            else
            {
                activeCamera = firstPersonCamera;
            }
        }
    }
}
