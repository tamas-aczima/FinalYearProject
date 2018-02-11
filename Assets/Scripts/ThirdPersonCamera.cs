using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    [SerializeField] private GameObject target;
    [SerializeField] private float damping;
    private Vector3 offset = Vector3.zero;

    void Start()
    {
        offset = target.transform.position - transform.position;
    }

    void LateUpdate()
    {
        //x component
        float currentAngleX = transform.eulerAngles.x;
        float desiredAngleX = target.transform.eulerAngles.x;
        float angleX = Mathf.LerpAngle(currentAngleX, desiredAngleX, damping);

        //y component
        float currentAngleY = transform.eulerAngles.y;
        float desiredAngleY = target.transform.eulerAngles.y;
        float angleY = Mathf.LerpAngle(currentAngleY, desiredAngleY, damping);

        Quaternion rotation = Quaternion.Euler(angleX, angleY, 0);
        transform.position = target.transform.position - (rotation * offset);

        transform.LookAt(target.transform);
    }
}
