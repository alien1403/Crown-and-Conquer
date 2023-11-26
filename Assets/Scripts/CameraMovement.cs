using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    private Vector3 targetPosition;
    [HideInInspector]
    public float leftBound = int.MinValue;
    [HideInInspector]
    public float rightBound = int.MaxValue;
    void Update()
    {
        
        targetPosition = target.position + offset;
        targetPosition.x = CheckBounds(targetPosition.x);
        transform.position = new Vector3(targetPosition.x, targetPosition.y, -10);
    }
    float CheckBounds(float x)
    {
        if (leftBound >= x)
            return leftBound;
        if (rightBound <= x)
            return rightBound;
        return x;
    }
}
