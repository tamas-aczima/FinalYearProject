using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private float healthRegenRate;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject healingAuraPrefab;
    private GameObject healingAura;
    private float minHandDistance = 1f;
    private float minHandRotation = 0.8f;
    private bool isEnabled;

    public bool IsEnabled {
        get { return isEnabled; }
        set { isEnabled = value; }
    }

	void Start () {
        health = maxHealth;
	}
	
	void Update () {
        if (isEnabled) {
            Heal();
        }
	}

    private void Heal() {
        float leftHandAngle = Mathf.Abs(leftHand.transform.rotation.x);
        float rightHandAngle = Mathf.Abs(rightHand.transform.rotation.x);

        if (Vector3.Distance(leftHand.transform.position, rightHand.transform.position) >= minHandDistance && 
            leftHandAngle >= minHandRotation && rightHandAngle >= minHandRotation && health < maxHealth) {
            if (healingAura == null) {
                healingAura = Instantiate(healingAuraPrefab, transform);
            }

            health += healthRegenRate * Time.deltaTime;

            if (health > maxHealth) {
                health = maxHealth;
            }
        }
        else {
            if (healingAura != null) {
                Destroy(healingAura);
            }
        }
    }

    public void TakeDamage(float damage) {
        health -= damage;
        Debug.Log("Player damaged by: " + damage);
    }
}
