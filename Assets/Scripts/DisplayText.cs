using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.UI;

public class DisplayText : MonoBehaviour {

    [SerializeField] private float displayRadius;
    [SerializeField] private Text text;
    private bool isVR;
    private List<GameObject> scrolls = new List<GameObject>();

    private void Start() {
        isVR = VRDevice.isPresent;
    }

    private void Update() {
        Display();
    }

    private void Display() {
        if (isVR) return;

        scrolls = GameObject.FindGameObjectsWithTag("Scroll").ToList();

        if (scrolls.Count > 0) {
            var closestScroll = scrolls.Find(scroll => Vector3.Distance(transform.position, scroll.transform.position) <= displayRadius);

            if (closestScroll != null && Vector3.Distance(transform.position, closestScroll.transform.position) <= displayRadius) {
                text.text = text.text = closestScroll.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().text;
            }
            else {
                text.text = "";
            }
        }
    }
}
