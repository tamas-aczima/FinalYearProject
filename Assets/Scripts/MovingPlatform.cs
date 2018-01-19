using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    private Vector3 startPos;
    private Vector3 endPos;
    private enum Direction { x, y, z};
    [SerializeField] Direction direction;
    [SerializeField] private float distance;
    [SerializeField] private float timeToReachEnd;
    private float currentTime = 0.0f;
    private bool reachedEnd;
    private bool reachedStart;

	// Use this for initialization
	void Start () {
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
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    private void Move()
    {
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
        }
        else if (transform.position == startPos)
        {
            reachedEnd = false;
            reachedStart = true;
            currentTime = 0;
        }
        
    }
}
