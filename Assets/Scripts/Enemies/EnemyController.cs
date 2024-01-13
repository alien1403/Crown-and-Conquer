using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour, IEntity
{
    EnemyAnimator enemyAnimator;
    private float currentHealth;
    private IEntity enemyInContact = null;
    private int spawnDay;
    private WorldTime worldTime;
    private Vector3 spawnLocation;
    private bool isPushedBack = false;
    private Vector2 pushbackPosition;


    [HideInInspector]
    public Vector2 moveDirection;
    public int SpawnDay { get => spawnDay; set => spawnDay = value; }
    public EnemyScriptableObject enemyScriptableObject;

    protected void Start()
    {
        enemyAnimator = GetComponent<EnemyAnimator>();
        currentHealth = enemyScriptableObject.MaxHealth;
        worldTime = FindObjectOfType<WorldTime>();
    }
    public void Update()
    {
        // for now we make it move to the center of the map, if it encounters a wall or the player it will attack it
        if(isPushedBack)
        {
            if (Vector2.Distance(transform.position, pushbackPosition) < 0.01f)
            {
                isPushedBack = false; // Set back to false when the pushback is completed
                return;
            }
            transform.position = Vector2.MoveTowards(transform.position, pushbackPosition, (enemyScriptableObject.MoveSpeed + 2) * Time.deltaTime);
            return;
        }   
        if (worldTime._currentTime.Days == spawnDay)
        {
            if (enemyInContact != null)
            {
                return;
            }
            Vector2 targetPosition = new Vector2(0f, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, enemyScriptableObject.MoveSpeed * Time.deltaTime);
            moveDirection = targetPosition - (Vector2)transform.position;
            return;
        }
        moveDirection = spawnLocation - transform.position;
        transform.position = Vector2.MoveTowards(transform.position, spawnLocation, enemyScriptableObject.MoveSpeed * Time.deltaTime);
    }
    public void TakeDamage(float damageTaken)
    {
        currentHealth -= damageTaken;
        if(currentHealth > 0)
        {
            enemyAnimator.TriggerHit();
            return;
        }
        enemyAnimator.TriggerDeath();
    }
    public void Kill()
    {
        Destroy(gameObject);
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enemy in contact with " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Enemy in contact with a wall");
            enemyAnimator.TriggerAttack();
            enemyInContact = collision.GetComponent<IEntity>();
            return;
        }
        if(collision.gameObject.CompareTag("Cliff"))
        {
            enemyAnimator.TriggerDeath();
            return;
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            enemyInContact = null;
        }
    }
    public void Attack()
    {
        if (enemyInContact != null)
        {
            enemyInContact.TakeDamage(enemyScriptableObject.AttackDamage);
        }
    }
    private void PushBack()
    {
        isPushedBack = true;
        int sign = moveDirection.x >= 0 ? -1 : 1;
        pushbackPosition = new Vector2(transform.position.x + sign * 0.5f, transform.position.y);
    }
}
