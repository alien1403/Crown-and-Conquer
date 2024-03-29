using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    [HideInInspector]
    public GameObject camera;
    [HideInInspector]
    public float parallaxEffect;
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponentsInChildren<SpriteRenderer>()[0].bounds.size.x;
    }

    void FixedUpdate()
    {
        float temp = (camera.transform.position.x * (1 - parallaxEffect));
        float dist = (camera.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
        if (temp > startpos + length)
            startpos += length;
        else if (temp < startpos - length)
            startpos -= length;
    }
}
