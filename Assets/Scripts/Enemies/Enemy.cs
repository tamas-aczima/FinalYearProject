using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    protected Animator animator;
    [SerializeField] private float maxHealth;
    private float health;
    protected bool isDead;
    private int targetIndex;
    private Vector3[] path;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float rotateSpeed;

	protected virtual void Start () {
        animator = GetComponent<Animator>();
        health = maxHealth;
	}
	
	protected virtual void Update () {
        CheckDeath();
	}

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("enemy damaged: " + damage);
    }
    
    private void CheckDeath()
    {
        if (health <= 0)
        {
            isDead = true;
        }
    }

    protected void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            targetIndex = 0;
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    private IEnumerator FollowPath()
    {
        if (path.Length > 0)
        {
            Vector3 currentWaypoint = path[0];
            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    //Debug.Log("target index: " + targetIndex + " path length: " + path.Length);
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);

                yield return null;
            }
        }
    }

    public bool IsDead
    {
        get { return isDead; }
    }

    protected bool HasAnimationFinished(string name)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && !animator.IsInTransition(0);
    }

    protected void RotateTowardsTarget(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0;
        Quaternion rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
    }
}
