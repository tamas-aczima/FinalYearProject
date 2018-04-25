using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParasiteEnemy : Enemy {

    private Transform target;
    [SerializeField] private float targetDistance;
    [SerializeField] private float chaseDistance;
    private float distance;
    [SerializeField] private float disappearTime;
    private float disappearTimer = 0.0f;
    [SerializeField] private GameObject fire;

    private enum States
    {
        Run,
        Attack,
        Burn,
        Dead
    }

    private States currentState = States.Run;

	// Use this for initialization
	protected override void Start ()
    {
        base.Start();

        target = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	protected override void Update ()
    {
        base.Update();

        distance = GetDistance();
        FSM();
        
        if (isDead)
        {
            currentState = States.Dead;
            animator.SetBool("IsDead", true);
        }
	}

    private void FSM()
    {
        switch (currentState)
        {
            case States.Run:
                animator.SetBool("IsInRange", false);
                PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
                RotateTowardsTarget(target.transform.position);

                if (distance <= targetDistance)
                {
                    currentState = States.Attack;
                }
                break;
            case States.Attack:
                animator.SetBool("IsInRange", true);

                if (distance > chaseDistance)
                {
                    currentState = States.Run;
                }
                break;
            case States.Burn:
                animator.SetBool("IsBurning", true);
                fire.SetActive(true);
                if (HasAnimationFinished("Burn"))
                {
                    animator.SetBool("IsBurning", false);
                    fire.SetActive(false);
                    currentState = States.Run;
                }
                break;
            case States.Dead:
                if (HasAnimationFinished("Dead"))
                {
                    disappearTimer += Time.deltaTime;
                    if (disappearTimer >= disappearTime)
                    {
                        Destroy(gameObject);
                    }
                }
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Fireball")
        {
            currentState = States.Burn;
        }
    }

    private float GetDistance()
    {
        Vector3 distanceFromTarget = target.position - transform.position;
        return distanceFromTarget.magnitude;
    }
}
