using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryWizardEnemy : Enemy
{

    [SerializeField] private Transform[] teleportLocations;
    [SerializeField] private Transform projectileLocation;
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private float minAttackTime;
    [SerializeField] private float maxAttackTime;
    [SerializeField] private int minAttacksToTeleport;
    [SerializeField] private int maxAttacksToTeleport;
    [SerializeField] private float projectileSpeed;
    private float attackTimer = 0.0f;
    private float attackTime;
    private int attackCount = 0;
    private bool attackCounted = false;
    private int attacksToTeleport;
    private bool hasShot = false;
    [SerializeField] private float shotDelay;
    private float shotDelayTimer = 0.0f;
    private bool hasTeleported = false;
    private bool isFadingIn = false;
    private bool isFadingOut = false;
    private bool isVisible = true;
    private Transform target;
    private SkinnedMeshRenderer render;
    private CapsuleCollider capsuleCollider;
    [SerializeField] private float disappearTime;
    private float disappearTimer = 0.0f;

    private enum States
    {
        Idle,
        Attack,
        Teleport,
        Dead
    }

    private States currentState = States.Idle;

    protected override void Start()
    {
        base.Start();

        render = GetComponentInChildren<SkinnedMeshRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        transform.position = GetRandomPosition();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        attacksToTeleport = GetAttacksToTeleport();
    }

    protected override void Update()
    {
        base.Update();

        RotateTowardsPlayer();
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
            case States.Idle:
                capsuleCollider.enabled = true;
                animator.SetBool("IsAttacking", false);
                animator.SetBool("IsTeleporting", false);
                attackCounted = false;
                hasShot = false;
                hasTeleported = false;
                isFadingIn = false;
                isFadingOut = false;

                if (attackCount >= attacksToTeleport)
                {
                    attackCount = 0;
                    attacksToTeleport = GetAttacksToTeleport();
                    currentState = States.Teleport;
                }

                attackTime = GetAttackTime();
                attackTimer += Time.deltaTime;

                if (attackTimer >= attackTime)
                {
                    attackTimer = 0.0f;
                    currentState = States.Attack;
                }
                break;
            case States.Attack:
                animator.SetBool("IsAttacking", true);
                if (!attackCounted)
                {
                    attackCount++;
                    attackCounted = true;
                }

                if (!hasShot)
                {
                    shotDelayTimer += Time.deltaTime;
                    if (shotDelayTimer >= shotDelay)
                    {
                        GameObject projectile = Instantiate(spellPrefab, projectileLocation.position, Quaternion.identity);
                        projectile.GetComponent<Rigidbody>().velocity = transform.forward * projectileSpeed;
                        hasShot = true;
                        shotDelayTimer = 0.0f;
                    }
                }

                if (HasAnimationFinished("Attack"))
                {
                    currentState = States.Idle;
                }
                break;
            case States.Teleport:
                capsuleCollider.enabled = false;
                animator.SetBool("IsTeleporting", true);
                if (!isFadingOut && isVisible)
                {
                    StartCoroutine(Fade(0.0f, 4f));
                    isFadingOut = true;
                }

                if (!hasTeleported && !isVisible)
                {
                    transform.position = GetRandomPosition();
                    hasTeleported = true;
                }

                if (!isFadingIn && !isVisible)
                {
                    StartCoroutine(Fade(1.0f, 4f));
                    isFadingIn = true;
                }
                
                if (HasAnimationFinished("Teleport"))
                {
                    currentState = States.Idle;
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

    private float GetAttackTime()
    {
        return Random.Range(minAttackTime, maxAttackTime);
    }

    private int GetAttacksToTeleport()
    {
        return Random.Range(minAttacksToTeleport, maxAttacksToTeleport + 1);
    }

    private Vector3 GetRandomPosition()
    {
        return teleportLocations[Random.Range(0, teleportLocations.Length)].position;
    }

    private bool HasAnimationFinished(string name)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && !animator.IsInTransition(0);
    }

    private void RotateTowardsPlayer()
    {
        transform.LookAt(target);
    }

    private IEnumerator Fade(float value, float time)
    {
        float alpha = render.material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
        {
            Color color = render.material.color;
            color.a = Mathf.Lerp(alpha, value, t);
            render.material.color = color;

            if (color.a <= 0.1f)
            {
                isVisible = false;
            }
            else if (color.a >= 0.9)
            {
                isVisible = true;
            }

            yield return null;
        }
    }
}
