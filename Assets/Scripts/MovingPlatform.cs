using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    private Vector3 startPos;
    private Vector3 endPos;
    private enum Direction { x, y, z};
    [SerializeField] Direction direction;
    [SerializeField] private bool requiresPlayer;
    [SerializeField] private float distance;
    [SerializeField] private float timeToReachEnd;
    [SerializeField] private float delayTime;
    private float delayTimer = 0.0f;
    private float currentTime = 0.0f;
    private bool reachedEnd;
    private bool reachedStart;
    private bool isPlayerOnPlatform;

	private void Start () {
        startPos = transform.position;
        reachedStart = true;
        switch (direction)
        {
            case Direction.x:
                endPos = startPos + new Vector3(distance, 0, 0);
                break;
            case Direction.y:
                endPos = startPos + new Vector3(0, distance, 0);
                break;
            case Direction.z:
                endPos = startPos + new Vector3(0, 0, distance);
                break;
        }
	}
	
	private void Update () {
        if (!requiresPlayer || (requiresPlayer && isPlayerOnPlatform))
        {
            Move();
        }
	}

    public bool IsPlayerOnPlatform
    {
        get { return isPlayerOnPlatform; }
    }

    private void Move()
    {
        delayTimer += Time.deltaTime;
        if (delayTimer < delayTime) return;

        currentTime += Time.deltaTime;

        float percent = currentTime / timeToReachEnd;

        if (reachedStart)
        {
            transform.position = Vector3.Lerp(startPos, endPos, percent);
        }
        else if (reachedEnd)
        {
            transform.position = Vector3.Lerp(endPos, startPos, percent);
        }

        if (transform.position == endPos)
        {
            reachedEnd = true;
            reachedStart = false;
            currentTime = 0;
            delayTimer = 0;
        }
        else if (transform.position == startPos)
        {
            reachedEnd = false;
            reachedStart = true;
            currentTime = 0;
            delayTimer = 0;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PrioController>() != null)
        {
            isPlayerOnPlatform = true;
            other.gameObject.transform.parent = gameObject.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PrioController>() != null)
        {
            isPlayerOnPlatform = false;
            other.gameObject.transform.parent = null;
        }
    }
}
