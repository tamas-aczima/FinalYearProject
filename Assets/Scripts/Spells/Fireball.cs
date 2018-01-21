using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    private Rigidbody rb;
    private ParticleSystem[] particleSystems;
    [SerializeField] private float maxLifeTime;
    private float lifeTimer = 0.0f;
    [SerializeField] private float damage;

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
        if (collision.gameObject.GetComponent<Health>() != null && gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
        }

        if (collision.gameObject.GetComponent<Enemy>() != null && gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
