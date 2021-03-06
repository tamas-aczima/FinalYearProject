﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Controller : MonoBehaviour {

    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private GameObject lever;
    [SerializeField] private GameObject movingPlatform;
    private GameObject enemy;
    private bool hasSpawnedEnemy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (movingPlatform.GetComponent<MovingPlatform>().IsPlayerOnPlatform && !hasSpawnedEnemy)
        {
            enemy = Instantiate(enemyToSpawn);
            hasSpawnedEnemy = true;
        }

        if (enemy != null && enemy.GetComponent<Enemy>().IsDead)
        {
            lever.SetActive(true);
        }
	}
}
