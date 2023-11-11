using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    private Camera camera;
    private float leftBound;
    private float rightBound;
    private float width;
    private float height;
    private Vector3 targetPosition;
    void Start()
    {
        MapController mapController = FindObjectOfType<MapController>();
        leftBound = mapController.leftMostX;
        rightBound = mapController.rightMostX;
        camera = GetComponent<Camera>();
        height = camera.orthographicSize;
        width = height * camera.aspect;
        leftBound = leftBound + width / 2;
        rightBound = rightBound - width / 2;
    }

    void Update()
    {
        
        targetPosition = target.position + offset;
        //targetPosition.x = CheckBounds(targetPosition.x);
        //transform.position = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);
        transform.position = targetPosition;
    }
    float CheckBounds(float x)
    {
        //Debug.Log($"Leftbound = {leftBound}, x = {x}, Rightbound = {rightBound}");
        if (leftBound >= x)
            return leftBound;
        if (rightBound <= x)
            return rightBound;
        return x;
    }
}
