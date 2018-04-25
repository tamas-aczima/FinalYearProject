using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Chest : Interactable {

    [SerializeField] private GameObject particles;
    [SerializeField] private int index;
    [SerializeField] private GameObject scrollPrefab;
    private GameObject scroll;
    private bool isOpen = false;
    private bool hasPlayedSound = false;
    [SerializeField] private GameObject chestMesh;
    private Material[] materials;
    [SerializeField] private Material[] opaqueMaterials;

    public bool IsOpen
    {
        get { return isOpen; }
        set { isOpen = value; }
    }

    public bool HasPlayedSound
    {
        get { return hasPlayedSound; }
        set { hasPlayedSound = value; }
    }

    private void Start()
    {
        materials = chestMesh.GetComponent<SkinnedMeshRenderer>().materials;
        StartCoroutine(FadeIn(1f));
    }

    private IEnumerator FadeIn(float time)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            float alpha = materials[i].color.a;
            float t;
            for (t = 0f; t < 1.0f; t += Time.deltaTime / time)
            {
                Debug.Log(t);
            }

            Color col = materials[i].color;
            col.a = Mathf.Lerp(alpha, 1, t);
            materials[i].color = col;

            if (t > 0.9f)
            {
                materials[i] = opaqueMaterials[i];
                Debug.Log("mat: " + materials[i] + " add mat: " + opaqueMaterials[i]);
                chestMesh.GetComponent<SkinnedMeshRenderer>().materials = materials;
            }

            yield return null;
        }
    }

    private void Update()
    {
        if (isOpen)
        {
            if (particles != null)
            {
                particles.SetActive(true);
            }

            if (scroll == null)
            {
                scroll = Instantiate(scrollPrefab, transform.position + new Vector3(-1f, 1, -1f), Quaternion.Euler(0, 45, 0));

                switch (index)
                {
                    case 1:
                        scroll.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().text = "You just learned levitation! Some objects can be levitated which is shown by the outline. Point at them with your palm facing up and press the top button.";
                        Levitation[] levitations = FindObjectsOfType<Levitation>();
                        foreach (Levitation levitation in levitations)
                        {
                            levitation.IsEnabled = true;
                        }
                        break;
                    case 2:
                        scroll.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().text = "You just learned healing! When you are low on health put your hands to the side with your palm facing up to regenerate health.";
                        Health health = FindObjectOfType<Health>();
                        health.IsEnabled = true;
                        break;
                }
            }    
        }
    }
}
