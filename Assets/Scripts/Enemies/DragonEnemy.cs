using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonEnemy : Enemy {

    private Vector3 target = Vector3.zero;
    private Vector3 targetDirection = Vector3.zero;
    [SerializeField] private float flyUpDistance;
    [SerializeField] private float flySpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private Transform[] flyTargets;
    [SerializeField] private MeshCollider ground;
    [SerializeField] private float landingProbability;
    [SerializeField] private float flyingProbability;

    private Quaternion rotation = Quaternion.identity;
    private int lastRandom = 0;
    private int random = 0;
    private float rand = 0.0f;

    private enum States
    {
        Idle,
        FlyUp,
        Fly,
        FlyBreatheFire,
        Land,
        Walk,
        Attack,
        Dead
    }

    private States currentState = States.Idle;

	// Use this for initialization
	protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();

        FSM();
	}

    private void FSM()
    {
        switch (currentState)
        {
            case States.Idle:
                if (HasAnimationFinished("Idle"))
                {
                    currentState = States.FlyUp;
                }
                break;
            case States.FlyUp:
                animator.SetBool("IsFlying", true);
                animator.SetBool("HasLanded", false);

                if (target == Vector3.zero)
                {
                    target = transform.position + new Vector3(0, flyUpDistance, 0);
                }
                FlyUp(States.FlyBreatheFire);
                break;
            case States.Fly:
                animator.SetBool("ShouldBreatheFire", false);

                if (target == Vector3.zero)
                {
                    while (lastRandom == random)
                    {
                        random = Random.Range(0, flyTargets.Length);
                    }
                    lastRandom = random;
                    target = flyTargets[random].position;
                    GetRotationToTarget();
                }
                if (Mathf.Abs(transform.rotation.eulerAngles.y - rotation.eulerAngles.y) > 0.1f)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);
                }
                else
                {
                    FlyForward(States.FlyBreatheFire);
                }
                break;
            case States.FlyBreatheFire:
                if (target == Vector3.zero)
                {
                    target = GameObject.FindGameObjectWithTag("Player").transform.position;
                    GetRotationToTarget();
                }
                if (Mathf.Abs(transform.rotation.eulerAngles.y - rotation.eulerAngles.y) > 0.1f)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);
                }
                else
                {
                    animator.SetBool("ShouldBreatheFire", true);
                }
                if (HasAnimationFinished("FlyBreatheFire"))
                {
                    target = Vector3.zero;
                    rand = Random.Range(0f, 1f);
                    if (rand < landingProbability)
                    {
                        currentState = States.Land;
                    }
                    else
                    {
                        currentState = States.Fly;
                    }
                }
                break;
            case States.Land:
                animator.SetBool("ShouldLand", true);
                animator.SetBool("ShouldBreatheFire", false);

                if (target == Vector3.zero)
                {
                    target = new Vector3(Random.Range(ground.bounds.min.x, ground.bounds.max.x), 0, Random.Range(ground.bounds.min.z, ground.bounds.max.z));
                    GetRotationToTarget();
                }
                if (Mathf.Abs(transform.rotation.eulerAngles.y - rotation.eulerAngles.y) > 0.1f)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);
                }
                else
                {
                    FlyDown(States.Walk);
                }
                 break;
            case States.Walk:
                animator.SetBool("HasLanded", true);
                animator.SetBool("IsAttacking", false);
                animator.SetBool("IsFlying", false);
                animator.SetBool("ShouldLand", false);

                target = GameObject.FindGameObjectWithTag("Player").transform.position;
                GetRotationToTarget();
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);

                Walk(States.Attack);
                break;
            case States.Attack:
                animator.SetBool("IsAttacking", true);
                int attackNumber = Random.Range(0, 3);
                animator.SetInteger("AttackNumber", attackNumber);

                if (HasAnimationFinished("Attack " + (attackNumber + 1)))
                {
                    rand = Random.Range(0f, 1f);
                    if (rand < flyingProbability)
                    {
                        currentState = States.FlyUp;
                    }
                    else
                    {
                        currentState = States.Walk;
                    }
                }
                break;
            case States.Dead:

                break;
        }
    }

    private void GetRotationToTarget()
    {
        targetDirection = target - transform.position;
        targetDirection.y = 0;
        rotation = Quaternion.LookRotation(targetDirection);
    }

    private void FlyUp(States switchToState)
    {
        if (Vector3.Distance(target, transform.position) > 1.0f)
        {
            transform.Translate(Vector3.up * flySpeed * Time.deltaTime);
        }
        else
        {
            target = Vector3.zero;
            currentState = switchToState;
        }
    }

    private void FlyForward(States switchToState)
    {
        if (Vector3.Distance(target, transform.position) > 2.0f)
        {
            transform.Translate(Vector3.forward * flySpeed * Time.deltaTime);
        }
        else
        {
            target = Vector3.zero;
            currentState = switchToState;
        }
    }

    private void FlyDown(States switchToState)
    {
        if (transform.position.y > 0.05f)
        {
            transform.Translate(new Vector3(0, -0.5f, 1) * flySpeed * Time.deltaTime);
        }
        else
        {
            target = Vector3.zero;
            currentState = switchToState;
        }
    }

    private void Walk(States switchToState)
    {
        if (Vector3.Distance(target, transform.position) > 5.0f)
        {
            transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
        }
        else
        {
            target = Vector3.zero;
            currentState = switchToState;
        }
    }
}
