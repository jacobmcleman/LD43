using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Activatable
{
    public float openTime = 1.0f;
    public float closeTime = 0.5f;
    public bool isOpen = false;
    bool moveFinished = false;
    float stateTime = 0;

    public Vector3 moveVector;

    private Vector3 openPoint;
    private Vector3 closedPoint;

    private Vector3 moveStart;
    private Vector3 moveEnd;
    private float moveTime;

    AudioSource doorScreamer;

    public override void OnActivate(GameObject activatedBy)
    {
        float t = stateTime / (isOpen ? openTime : closeTime);

        isOpen = !isOpen;
        stateTime = 0;

        moveTime = 1 * (isOpen ? openTime : closeTime);
        moveStart = transform.position;
        moveEnd = (isOpen ? openPoint : closedPoint);

        moveFinished = false;

        if(doorScreamer)
            doorScreamer.Play();
    }

    private void Start()
    {
        if (isOpen)
        {
            openPoint = transform.position;
            closedPoint = transform.position + moveVector;

            moveStart = closedPoint;
            moveEnd = openPoint;
        }
        else
        {
            closedPoint = transform.position;
            openPoint = transform.position + moveVector;

            moveStart = openPoint;
            moveEnd = closedPoint;
        }

        moveTime = (isOpen ? openTime : closeTime);
        stateTime = moveTime + 1;
        moveFinished = true;
        transform.position = moveEnd;

        doorScreamer = GetComponentInParent<AudioSource>();
    }

    private void Update()
    {
        stateTime += Time.deltaTime;

        if(stateTime < moveTime)
        {
            transform.position = Vector3.Lerp(moveStart, moveEnd, stateTime / moveTime);
        }
        else if(!moveFinished)
        {
            transform.position = moveEnd;
            moveFinished = true;
            //Play appropriate slamming sound here
        }
    }
}
