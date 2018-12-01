using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanCamera : MonoBehaviour {
    public float moveMult = 1.0f;
    public float edgeScrollSpeed = 1.0f;

    public float scrollSpeed = 2.5f;

    public float minSize = 1;
    public float maxSize = 5;

    private float baseZ;
    private Vector3 MouseStart, MouseMove;

    private float zoomAmount;

    Camera camera;

    void Start()
    {
        baseZ = transform.position.z;  // Distance camera is above map

        camera = GetComponent<Camera>();
        zoomAmount = (maxSize - camera.orthographicSize) / (maxSize - minSize);
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

        Vector2 inputMotion = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * scrollSpeed;
        if(inputMotion.sqrMagnitude > 0.01)
        {
            transform.position = new Vector3(transform.position.x + inputMotion.x * Time.deltaTime, transform.position.y + inputMotion.y * Time.deltaTime, baseZ);
        }

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0.0f)
        {
            zoomAmount -= scrollInput;

            zoomAmount = Mathf.Clamp01(zoomAmount);

            //Use unclamped cus i already clamped
            camera.orthographicSize = Mathf.LerpUnclamped(minSize, maxSize, zoomAmount);
        }
    }
}
