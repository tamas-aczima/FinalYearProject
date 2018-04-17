using UnityEngine;
using UnityEngine.UI;

public class Chest : Interactable {

    [SerializeField] private GameObject particles;
    [SerializeField] private int index;
    [SerializeField] private GameObject scrollPrefab;
    private GameObject scroll;
    private bool isOpen = false;
    private bool hasPlayedSound = false;

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
                }
            }    
        }
    }
}
