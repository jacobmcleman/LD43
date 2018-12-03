using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairToggle : Activatable
{
    private Transform[] steps;

    public float animateUpTime = 2.0f;
    public float animateDownTime = 2.0f;

    public float slope = 0.33f;

    public float spacing = 0.218f;

    public float waveStrength = 1.0f;
    public bool rightSideHigh = true;

    public bool isOpen = false;
    bool isMoving = false;

    public Collider2D rampCollider;
    public Collider2D topCollider;

    private GameObject stairScreamerUpObj;
    private GameObject stairScreamerDownObj;
    AudioSource stairScreamerUp;
    AudioSource stairScreamerDown;

	// Use this for initialization
	void Start () {
        steps = new Transform[transform.childCount];

        for(int i = 0; i < steps.Length; ++i)
        {
            steps[i] = transform.Find("SingleStep (" + i + ")");
        }

        for (int i = 0; i < steps.Length; ++i)
        {
            int index = i;
            if (!rightSideHigh) index = steps.Length - i;

            if (isOpen)
            {
                steps[i].localPosition = new Vector3(i * spacing, index * spacing * slope, i * 0.05f);
                rampCollider.enabled = true;
                topCollider.enabled = true;
            }
            else
            {
                steps[i].localPosition = new Vector3(i * spacing, 0, i * 0.05f);
                rampCollider.enabled = false;
                if (topCollider) topCollider.enabled = false;
            }
        }

        try
        {
        stairScreamerUpObj = GameObject.Find("stairsUp");
        stairScreamerDownObj = GameObject.Find("stairDown");

        stairScreamerUp = stairScreamerUpObj.GetComponent<AudioSource>();
        stairScreamerDown = stairScreamerDownObj.GetComponent<AudioSource>();
        }
        catch (Exception e)
        {}
    }
	
    public override void OnActivate(GameObject a)
    {
        isOpen = !isOpen;

        if (!isMoving)
        {
            if(isOpen) StartCoroutine(AnimateUp());
            else StartCoroutine(AnimateDown());
        }
    }

	IEnumerator AnimateUp()
    {
        float elapsed = 0;
        isMoving = true;
        try
        {
        stairScreamerUp.Play();
        }
        catch (Exception e)
        {}
        while (elapsed < animateUpTime)
        {
            float mainT = elapsed / animateDownTime;
            for (int i = 0; i < steps.Length; ++i)
            {
                int index = i;
                if (!rightSideHigh) index = steps.Length - i;

                float t = mainT * (1 - ((1.0f * i) / steps.Length) + 1);
                t = Mathf.Clamp01(t);
                float overshoot = 1.70158f;

                t -= 1;
                t = t * t * ((overshoot + 1) * t + overshoot) + 1;

                steps[i].localPosition = new Vector3(i * spacing, Mathf.LerpUnclamped(0, index * spacing * slope, t), i * 0.05f);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        isMoving = false;

        rampCollider.enabled = true;
        if(topCollider) topCollider.enabled = true;

        if (!isOpen) StartCoroutine(AnimateDown());
    }

    IEnumerator AnimateDown()
    {
        float elapsed = 0;
        try
        {
        stairScreamerDown.Play();
        }
        catch (Exception e)
        {}
        isMoving = true;
        rampCollider.enabled = false;
        if (topCollider) topCollider.enabled = false;

        while (elapsed < animateUpTime)
        {
            float mainT = elapsed / animateDownTime;
            for (int i = 0; i < steps.Length; ++i)
            {
                int index = i;
                if (!rightSideHigh) index = steps.Length - i;

                float t = mainT * (((1.0f * i) / steps.Length) + 1);

                steps[i].localPosition = new Vector3(i * spacing, Mathf.Lerp(index * spacing * slope, 0, t), i * 0.05f);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        isMoving = false;

        if (isOpen) StartCoroutine(AnimateUp());
    }
}
