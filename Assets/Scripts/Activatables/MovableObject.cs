using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : Activatable
{

    bool moveFinished = false;
    float stateTime = 0;

    public Vector3 moveVector;

    private Vector3 rotateStart;
    private Vector3 rotateEnd;
    private Vector3 moveStart;
    private Vector3 moveEnd;
    private float moveTime;

    public Vector3[] rotationPoints;
    public Vector3[] movePoints;
    public float [] moveTimes;
    public int positionNum = 0;

    public override void OnActivate(GameObject activatedBy)
    {
        stateTime = 0;

        moveFinished = false;

        if (positionNum >= (movePoints.Length-1))
            positionNum = 0;
        else
            positionNum += 1;

        Debug.Log("Going to Postion:" + positionNum);

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
