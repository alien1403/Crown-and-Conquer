using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.Build.Player;
using UnityEngine.Playables;

namespace Tests
{
    [TestFixture]
    public class PlayerMovementTests
    {
        [Test]
        public void MoveMethodShouldUpdateVelocity()
        {
            GameObject playerGameObject = new GameObject();
            PlayerMovement playerMovement = playerGameObject.AddComponent<PlayerMovement>();
            Rigidbody2D rigidbody = playerGameObject.AddComponent<Rigidbody2D>();

            playerMovement.Start();
            playerMovement.InputManagement();

            playerMovement.Move();

            Assert.AreEqual(playerMovement.moveSpeed, rigidbody.velocity.x, 0.01f, "Velocity should be set to moveSpeed");
        }   
    }

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
            Debug.Log("Before CheckSprint - isSprinting: " + isSprinting + ", keyCooldown: " + keyCooldown + ", sprintCooldown: " + sprintCooldown);

            if (lastPressed == code)
            {
                if (keyCooldown > 0)
                {
                    if (!elapsedSprintCooldown)
                    {
                        if (sprintCooldown >= 0)
                        {
                            StartSprint(KeyCode.P, sprintDuration);
                            Debug.Log("Activated Sprint");
                        }
                    }
                    else if (sprintCooldown >= sprintDuration)
                    {
                        StartSprint(KeyCode.P, sprintDuration);
                        Debug.Log("Activated Sprint");
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

            Debug.Log("After CheckSprint - isSprinting: " + isSprinting + ", keyCooldown: " + keyCooldown + ", sprintCooldown: " + sprintCooldown);
        }
    }

    internal class PlayerMovement : MonoBehaviour
    {
        public float moveSpeed;
        public float sprintSpeed;
        Rigidbody2D rigidBody;
        [HideInInspector]
        public Vector2 moveDirection;
        public Sprint sprint;
        public float sprintDuration;
        public void Start()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            Debug.Log("Start() called!");
            sprint.sprintCooldown = sprintDuration;
            sprint.elapsedSprintCooldown = false;
        }

        void Update()
        {
            InputManagement();
            Move();
        }
        public void InputManagement()
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
        public void Move()
        {
            Debug.Log("Before Move - Velocity: " + rigidBody.velocity);

            if (!sprint.isSprinting)
                rigidBody.velocity = new Vector2(moveDirection.x * moveSpeed, 0);
            else
                rigidBody.velocity = new Vector2(moveDirection.x * sprintSpeed, 0);

            Debug.Log("After Move - Velocity: " + rigidBody.velocity);
        }
    }
}
