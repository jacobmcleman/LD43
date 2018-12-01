using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Pace : MonoBehaviour {
    public float paceDist = 4;
    public float paceTime = 2;
    public float idleTime = 1;
    float timer;

    bool idle = true;

    Vector3 toPoint;
    Vector3 fromPoint;

	// Use this for initialization
	void Start () {
        toPoint = transform.position + new Vector3(-paceDist / 2, 0, 0);
        fromPoint = transform.position + new Vector3(paceDist / 2, 0, 0);

        transform.position = fromPoint;

        timer = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        
        if (idle)
        {
            GetComponent<Animator>().SetFloat("velocity", 0);

            if (timer > idleTime)
            {
                idle = false;
                timer = 0;
            }
        }
        else
        {
            float t = timer / paceTime;

            if(t >= 1)
            {
                transform.position = toPoint;
                toPoint = fromPoint;
                fromPoint = transform.position;
                timer = 0;
                idle = true;
            }
            else
            {
                transform.position = Vector3.Lerp(fromPoint, toPoint, t);
            }

            GetComponent<Animator>().SetFloat("velocity", (toPoint.x - fromPoint.x) / paceTime);
        }
	}
}
