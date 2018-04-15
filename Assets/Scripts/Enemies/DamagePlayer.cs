using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour {

    [SerializeField] private float damage;
    private bool damagedPlayer;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.root.GetComponent<Health>() != null && !damagedPlayer)
        {
            collision.gameObject.transform.root.GetComponent<Health>().TakeDamage(damage);
            damagedPlayer = true;
            Destroy(transform.root.gameObject, 10);
        }
    }
}
