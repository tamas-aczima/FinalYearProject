using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

    private Image blackImage;
    private bool endLevel = false;

    void Start () {
        GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
        blackImage = GameObject.FindGameObjectWithTag("Player").transform.Find("FadeCanvas").transform.Find("Black").GetComponent<Image>();
        StartCoroutine(Fade(true));
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            PlayerPrefs.SetInt("StartedLevel", SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
	
	void Update () {
        if (endLevel && blackImage.color.a >= 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(Fade(false));
            endLevel = true;
        }
    }

    private IEnumerator Fade(bool fadeAway)
    {
        if (fadeAway)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                blackImage.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
        else
        {
            for (float i = 0; i <= 1.2; i += Time.deltaTime)
            {
                blackImage.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
    }
}
