using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAnimator : MonoBehaviour
{
    Animator animator;
    PlayerMovement playerMovement;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(playerMovement.moveDirection.x != 0)
        {
            if(playerMovement.sprint.isSprinting)
            {
                animator.SetBool("Move", false);
                animator.SetBool("Sprint", true);
            }
            else
            {
                animator.SetBool("Sprint", false);
                animator.SetBool("Move", true);
            }
            SpriteDirectionChecker();
        }
        else
        {
            animator.SetBool("Move", false);
            animator.SetBool("Sprint", false);
        }
    }
    void SpriteDirectionChecker()
    {
        if(playerMovement.moveDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            if(playerMovement.moveDirection.x > 0) 
            {
                spriteRenderer.flipX = false;
            }
        }
    }
    public void SetSprint(bool value)
    {
        animator.SetBool("Sprint", value);
    }
}
