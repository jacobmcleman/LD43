using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : Activatable
{

    bool moveFinished = false;
    float stateTime = 0;

    private Vector3 rotateStart;
    private Vector3 rotateEnd;
    private Vector3 moveStart;
    private Vector3 moveEnd;
    private float moveTime;

    public Vector3[] rotationPoints;
    public Vector3[] movePoints;
    public float [] moveTimes;
    public int positionNum = 0;

    AudioSource elevatorScreaming;

    public bool reverseable = true;
    public bool reverse = false;

    public override void OnActivate(GameObject activatedBy)
    {
        stateTime = 0;

        moveFinished = false;

        if (positionNum >= (movePoints.Length - 1))
        {
            if (reverseable)
            {
                positionNum -= 1;
                reverse = true;
            }
            else
                positionNum = 0;
        }
        else if (positionNum <= 0)
        {
            if (reverseable)
            {
                positionNum += 1;
                reverse = false;
            }
            else
                positionNum = movePoints.Length;
        }
        else
        {
            if (reverse)
                positionNum -= 1;
            else
                positionNum += 1;
        }
            

        Debug.Log("Going to Postion:" + positionNum);
        elevatorScreaming.Play();
        rotateStart = transform.eulerAngles;
        rotateEnd = rotationPoints[positionNum];
        moveStart = transform.position;
        moveEnd = movePoints[positionNum];
        moveTime = moveTimes[positionNum];
    }

    private void Start()
    {
        if (movePoints.Length == 0)
            movePoints[0] = transform.position;
        if (moveTimes.Length == 0)
            moveTimes[0] = 1;
        stateTime = moveTime + 1;
        moveFinished = true;
        elevatorScreaming = GetComponent<AudioSource>();
    }

    private void Update()
    {
        stateTime += Time.deltaTime;

        if(stateTime < moveTime)
        {
            transform.position = Vector3.Lerp(moveStart, moveEnd, stateTime / moveTime);
            transform.eulerAngles = Vector3.Lerp(rotateStart, rotateEnd, stateTime / moveTime);
        }
        else if(!moveFinished)
        {
            transform.position = moveEnd;
            transform.eulerAngles = rotateEnd;
            moveFinished = true;
            //Play appropriate slamming sound here
        }
    }
}
