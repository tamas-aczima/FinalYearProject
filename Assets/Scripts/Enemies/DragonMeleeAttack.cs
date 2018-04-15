using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonMeleeAttack : MonoBehaviour {

    private Animator animator;
    private CapsuleCollider capsuleCollider;
    [SerializeField] private float damage;
    private bool hasDamaged = false;

    void Start()
    {
        animator = transform.root.GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        ToggleCollider();
        ResetHasDamaged();
    }

    private void ToggleCollider()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 1") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 2"))
        {
            capsuleCollider.enabled = true;
        }
        else
        {
            capsuleCollider.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.root.GetComponent<Health>() != null && !hasDamaged)
        {
            collision.gameObject.transform.root.GetComponent<Health>().TakeDamage(damage);
            hasDamaged = true;
        }
    }

    private void ResetHasDamaged()
    {
        if (HasAnimationFinished("Attack 1") || HasAnimationFinished("Attack 2"))
        {
            hasDamaged = false;
        }
    }

    private bool HasAnimationFinished(string name)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && !animator.IsInTransition(0);
    }
}
