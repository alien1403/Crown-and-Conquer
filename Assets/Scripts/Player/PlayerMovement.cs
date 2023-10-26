using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public struct Sprint
{
    internal bool isSprinting;
    internal KeyCode lastPressed;
    internal float keyCooldown;
    internal const float keyCooldownDuration = 0.5f;
    internal float sprintCooldown;
    internal bool elapsedSprintCooldown;

    void StartSprint(KeyCode code, float sprintDuration)
    {
        isSprinting = true;
        lastPressed = code;
        sprintCooldown = sprintDuration;
    }
    internal void CheckSprint(float sprintDuration, KeyCode code)
    {
        if (lastPressed == code)
        {
            if (keyCooldown > 0)
            {
                if (!elapsedSprintCooldown)
                {
                    if (sprintCooldown >= 0)
                    {
                        StartSprint(KeyCode.P, sprintDuration);
                    }
                }
                else
                    if (sprintCooldown >= sprintDuration)
                {
                    StartSprint(KeyCode.P, sprintDuration);
                }
            }
            else
            {
                keyCooldown = keyCooldownDuration;
                lastPressed = code;
            }
        }
        else
        {
            isSprinting = false;
            lastPressed = code;
        }
    }
}

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float sprintSpeed;
    Rigidbody2D rigidBody;
    [HideInInspector]
    public Vector2 moveDirection;
    public Sprint sprint;
    public float sprintDuration;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        sprint.sprintCooldown = sprintDuration;
        sprint.elapsedSprintCooldown = false;
    }

    void Update()
    {
        InputManagement();
        Move();
    }
    void InputManagement()
    {
        float timePassed = Time.deltaTime;
        float moveX = 0;
        sprint.keyCooldown -= timePassed;
        if (sprint.isSprinting)
        {
            if (sprint.sprintCooldown >= 0)
            { 
                sprint.sprintCooldown -= timePassed; 
            }
            else
            {
                sprint.isSprinting = false;
                sprint.elapsedSprintCooldown = true;
            }
        }
        else
        {
            sprint.sprintCooldown += timePassed;
            if (sprint.sprintCooldown > sprintDuration)
            {
                sprint.elapsedSprintCooldown = false;
            }
        }
        Debug.Log(sprint.sprintCooldown);
        if (Input.GetKeyDown(KeyCode.D))
        {
            sprint.CheckSprint(sprintDuration, KeyCode.D);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            sprint.CheckSprint(sprintDuration, KeyCode.A);
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveX = -1;
        }
        moveDirection = new Vector2(moveX, 0).normalized;

    }
    void Move()
    {
        if (!sprint.isSprinting)
            rigidBody.velocity = new Vector2(moveDirection.x * moveSpeed, 0);
        else
            rigidBody.velocity = new Vector2(moveDirection.x * sprintSpeed, 0);
    }
}
