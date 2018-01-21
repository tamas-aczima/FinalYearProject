using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    protected Animator animator;
    [SerializeField] private float maxHealth;
    private float health;
    protected bool isDead;

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

    public bool IsDead
    {
        get { return isDead; }
    }
}
