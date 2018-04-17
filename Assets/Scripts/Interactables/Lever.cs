using System.Collections;
using UnityEngine;

public class Lever : Interactable {

    [SerializeField] private GameObject doorToOpen;
    private bool isTurned = false;
    private bool hasOpened = false;

    public bool IsTurned
    {
        get { return isTurned; }
        set { isTurned = value; }
    }

    private void Update()
    {
        if (isTurned && doorToOpen != null && !hasOpened)
        {
            hasOpened = true;
            doorToOpen.GetComponent<AudioSource>().PlayOneShot(doorToOpen.GetComponent<AudioSource>().clip);
            StartCoroutine(OpenDoor(doorToOpen));
        }
    }

    private IEnumerator OpenDoor(GameObject door)
    {
        Vector3 rot = door.transform.rotation.eulerAngles;
        float desiredRot = rot.y + 90;
        while (rot.y != desiredRot)
        {
            rot.y = Mathf.Lerp(rot.y, desiredRot, Time.deltaTime * 0.7f);
            door.transform.eulerAngles = rot;
            yield return null;
        }
    }
}
