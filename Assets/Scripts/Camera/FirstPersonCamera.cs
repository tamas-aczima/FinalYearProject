using UnityEngine;

public class FirstPersonCamera : MonoBehaviour { 

    private void LateUpdate() {
        transform.rotation = transform.root.GetComponent<PrioController>().ThirdPersonCameraTarget.rotation;
    }
}
