using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ParasiteEnemy : Enemy {

    private NavMeshAgent navMeshAgent;
    private Transform target;
    [SerializeField] private float targetDistance;
    [SerializeField] private float chaseDistance;
    private float distance;
    [SerializeField] private float disappearTime;
    private float disappearTimer = 0.0f;

    private enum States
    {
        Run,
        Attack,
        Dead
    }

    private States currentState = States.Run;

	// Use this for initialization
	protected override void Start ()
    {
        base.Start();

        navMeshAgent = GetComponent<NavMeshAgent>();
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
                navMeshAgent.SetDestination(target.position);

                if (distance <= targetDistance)
                {
                    navMeshAgent.SetDestination(transform.position);
                    currentState = States.Attack;
                }
                break;
            case States.Attack:
                animator.SetBool("IsInRange", true);

                if (distance > chaseDistance)
                {
                    navMeshAgent.SetDestination(target.position);
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

    private float GetDistance()
    {
        Vector3 distanceFromTarget = target.position - transform.position;
        return distanceFromTarget.magnitude;
    }
}
