using UnityEngine;

public class Flammable : MonoBehaviour
{
    [SerializeField] private GameObject[] fires;
    [SerializeField] private GameObject burnableObject;
    [SerializeField] private float burnTime;
    private float burnTimer = 0.0f;
    private bool isOnFire = false;
    private bool hasStartedBurning = false;
	
	private void Update () {
        BurnDown();
	}

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.GetComponent<Fireball>() != null && !hasStartedBurning) {
            isOnFire = true;
        }
    }

    private void BurnDown() {
        if (!isOnFire) return;

        Debug.Log("burning");
        if (!hasStartedBurning) {
            foreach (GameObject fire in fires) {
                fire.gameObject.SetActive(true);
            }
            hasStartedBurning = true;
        }

        burnTimer += Time.deltaTime;
        if (burnTimer >= burnTime) {
            foreach (GameObject fire in fires) {
                fire.gameObject.SetActive(false);
            }
            if (burnableObject != null) {
                burnableObject.SetActive(false);
            }
            else {
                Destroy(gameObject);
            }
            isOnFire = false;
        }
    }
}
