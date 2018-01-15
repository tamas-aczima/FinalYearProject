using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    private Rigidbody rb;
    private ParticleSystem[] particleSystems;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        rb.MovePosition(transform.position + rb.velocity * Time.deltaTime);
    }
}
