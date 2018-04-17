using UnityEngine;
using UnityEngine.VR;

public class CameraController : MonoBehaviour {

    [SerializeField] private GameObject firstPersonCamera;
    [SerializeField] private GameObject thirdPersonCamera;
    [SerializeField] private GameObject vrCamera;
    private GameObject activeCamera;
    private bool cameraChanged = false;
    private bool isVR = false;

    void Start () {
        isVR = VRDevice.isPresent;

        if (isVR) {
            activeCamera = vrCamera;
        }
        else {
            activeCamera = thirdPersonCamera;
        }

        activeCamera.SetActive(true);
	}
	
	void Update () {
        ChangeCamera();
    }

    private void ChangeCamera() {
        if (isVR) return;

        if (!cameraChanged && gameObject.GetComponent<YostSkeletonRig>().
            GetJoyStickButtonDown(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.
            YOST_SKELETON_LEFT_X_BUTTON)) {
            cameraChanged = true;
            ToggleCamera();
        }

        if (gameObject.GetComponent<YostSkeletonRig>().
            GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.
            YOST_SKELETON_LEFT_X_BUTTON)) {
            cameraChanged = false;
        }
    }

    private void ToggleCamera() {
        if (activeCamera == firstPersonCamera) {
            activeCamera = thirdPersonCamera;
            thirdPersonCamera.SetActive(true);
            firstPersonCamera.SetActive(false);
        }
        else {
            activeCamera = firstPersonCamera;
            firstPersonCamera.SetActive(true);
            thirdPersonCamera.SetActive(false);
        }
    }
}
