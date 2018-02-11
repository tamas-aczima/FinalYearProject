using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParasiteAttack : MonoBehaviour {

    private Animator animator;
    private SphereCollider sphereCollider;
    [SerializeField] private float damage;
    private bool hasDamaged = false;

	// Use this for initialization
	void Start () {
        animator = transform.root.GetComponent<Animator>();
        sphereCollider = GetComponent<SphereCollider>();
	}
	
	// Update is called once per frame
	void Update () {
        ToggleCollider();
        ResetHasDamaged();
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
        if (collision.gameObject.GetComponent<Health>() != null && !hasDamaged)
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
            hasDamaged = true;
        }
    }

    private void ResetHasDamaged()
    {
        if (HasAnimationFinished("Attack"))
        {
            hasDamaged = false;
        }
    }

    private bool HasAnimationFinished(string name)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && !animator.IsInTransition(0);
    }
}
