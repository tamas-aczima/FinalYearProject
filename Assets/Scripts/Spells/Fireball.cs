using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    private Rigidbody rb;
    private ParticleSystem[] particleSystems;
    [SerializeField] private float maxLifeTime;
    private float lifeTimer = 0.0f;

	private void Start () {
        rb = GetComponent<Rigidbody>();
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }
	}

    private void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= maxLifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate () {
        rb.MovePosition(transform.position + rb.velocity * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
