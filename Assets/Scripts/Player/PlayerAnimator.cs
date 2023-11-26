using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAnimator : MonoBehaviour, IDataPersistence
{
    Animator animator;
    PlayerMovement playerMovement;
    SpriteRenderer spriteRenderer;
    bool flipX;
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = flipX;
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

    public void LoadData(GameData gameData)
    {
        flipX = gameData.playerFlip;
    }

    public void SaveData(GameData gameData)
    {
        gameData.playerFlip = spriteRenderer.flipX;
    }
}
