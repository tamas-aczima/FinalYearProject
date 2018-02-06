using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParasiteAttack : MonoBehaviour {

    private Animator animator;
    private SphereCollider sphereCollider;
    [SerializeField] private float damage;

	// Use this for initialization
	void Start () {
        animator = transform.root.GetComponent<Animator>();
        sphereCollider = GetComponent<SphereCollider>();
	}
	
	// Update is called once per frame
	void Update () {
        ToggleCollider();
	}

    private void ToggleCollider()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            sphereCollider.enabled = true;
        }
        else
        {
            sphereCollider.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Health>() != null)
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
