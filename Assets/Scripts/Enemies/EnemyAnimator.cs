using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator animator;
    private EnemyController enemyController;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        SpriteDirectionChecker();
    }
    void SpriteDirectionChecker()
    {
        if(enemyController.moveDirection.x < 0)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }
    public void TriggerAttack()
    {
        Debug.Log("Trigger Attack");
        animator.SetTrigger("Attack");
        Debug.Log("Attack triggered");
    }
    public void TriggerDeath()
    {
        animator.SetTrigger("Die");
    }
    public void TriggerHit()
    {
        animator.SetTrigger("Hit");
    }
}
