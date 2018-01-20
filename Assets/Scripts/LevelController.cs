using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

    // Use this for initialization
    void Start () {
        GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
        transform.position = GameObject.FindGameObjectWithTag("EndPoint").transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    //void ChangeScene(int buildNumber)
    //{
    //    Debug.Log("called changescene");
    //    SceneManager.LoadScene(buildNumber);
    //    OnNextLevelLoaded();
    //    //StartCoroutine(LoadNextLevel(buildNumber));
    //}

    //IEnumerator LoadNextLevel(int buildNumber)
    //{
    //    Debug.Log("called loadnext");
    //    AsyncOperation loadNext = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    //    yield return loadNext;
    //    OnNextLevelLoaded();
    //}

    //void OnNextLevelLoaded()
    //{
    //    GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
    //    transform.position = GameObject.FindGameObjectWithTag("EndPoint").transform.position;
    //    Debug.Log(transform.position);
    //    Debug.Log(GameObject.FindGameObjectWithTag("EndPoint").transform.position);
    //    Debug.Log("OnNextLevelLoaded called");
    //}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
