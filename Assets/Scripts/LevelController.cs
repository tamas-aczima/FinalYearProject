using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        transform.position = GameObject.FindGameObjectWithTag("EndPoint").transform.position;
    }
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    void ChangeScene(int buildNumber)
    {
        Debug.Log("called changescene");
        StartCoroutine(LoadNextLevel(buildNumber));
    }

    IEnumerator LoadNextLevel(int buildNumber)
    {
        Debug.Log("called loadnext");
        AsyncOperation loadNext = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        yield return loadNext;
        OnNextLevelLoaded();
    }

    void OnNextLevelLoaded()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
        transform.position = GameObject.FindGameObjectWithTag("EndPoint").transform.position;
        Debug.Log("OnNextLevelLoaded called");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ChangeScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
