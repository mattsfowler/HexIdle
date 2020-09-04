using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public float maxZoomIn = 5;
    public float maxZoomOut = 20;
    public float zoomSpeed = 500;
    public float maxWidth = -1; //-1 means allow camera wrapping
    public float maxHeight = 20;
    public float sensitivity = 10;

    // Update is called once per frame
    void Update()
    {
        // Zoom in or out (calculation):
        float scrollAmount = 0.0f - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        float currentInOut = transform.position.y;
        float trueScrollAmount = Mathf.Clamp(scrollAmount, maxZoomIn - currentInOut, maxZoomOut - currentInOut);

        // Move left / right (calculation):
        float horizontal = Input.GetAxis("Horizontal") * sensitivity * Time.deltaTime;
        float currentLeftRight = transform.position.x;
        if (maxWidth >= 0.0f) horizontal = Mathf.Clamp(horizontal, 0 - currentLeftRight, maxWidth - currentLeftRight);

        // Move up / down (calculation):
        float vertical = Input.GetAxis("Vertical") * sensitivity * Time.deltaTime;
        float currentUpDown = transform.position.z;
        if (maxHeight >= 0.0f) vertical = Mathf.Clamp(vertical, 0 - currentUpDown, maxHeight - currentUpDown);

        // Perform translation
        transform.Translate(horizontal, vertical, -trueScrollAmount);
    }
}
