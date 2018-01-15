using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private GameObject activeCamera;
    [SerializeField] private GameObject firstPersonCamera;
    [SerializeField] private GameObject thirdPersonCamera;
    private bool cameraChanged = false;

	// Use this for initialization
	void Start () {
        activeCamera = thirdPersonCamera;
	}
	
	// Update is called once per frame
	void Update () {
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

        if (!gameObject.GetComponent<YostSkeletonRig>().GetJoyStickButtonDown(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_X_BUTTON))
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
