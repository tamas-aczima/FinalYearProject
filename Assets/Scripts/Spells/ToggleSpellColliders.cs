using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSpellColliders : MonoBehaviour {

    SpellController[] spellControllers;
    [SerializeField] SphereCollider[] spellColliders;

    private void Start()
    {
        spellControllers = FindObjectsOfType<SpellController>();
        if (!spellControllers[0].IsLeft)
        {
            SpellController temp = spellControllers[0];
            spellControllers[0] = spellControllers[1];
            spellControllers[1] = temp;
        }
    }

    private void Update()
    {
        ToggleColliders();
    }

    private void ToggleColliders()
    {
        if (spellControllers[0].IsLeftCasting || spellControllers[1].IsRightCasting)
        {
            foreach (SphereCollider collider in spellColliders)
            {
                collider.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (SphereCollider collider in spellColliders)
            {
                collider.gameObject.SetActive(false);
            }
        }
    }
}
