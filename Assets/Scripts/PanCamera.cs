using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanCamera : MonoBehaviour {
    public float moveMult = 1.0f;
    public float edgeScrollSpeed = 1.0f;

    private float baseZ;
    private Vector3 MouseStart, MouseMove;

    void Start()
    {
        baseZ = transform.position.z;  // Distance camera is above map
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseStart = new Vector3(Input.mousePosition.x, Input.mousePosition.y, baseZ);
        }
        else if (Input.GetMouseButton(0))
        {
            MouseMove = -moveMult * new Vector3(Input.mousePosition.x - MouseStart.x, Input.mousePosition.y - MouseStart.y, baseZ);
            MouseStart = new Vector3(Input.mousePosition.x, Input.mousePosition.y, baseZ);
            transform.position = new Vector3(transform.position.x + MouseMove.x * Time.deltaTime, transform.position.y + MouseMove.y * Time.deltaTime, baseZ);
        }
    }
}
