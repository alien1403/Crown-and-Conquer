using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    private float moveSpeedFactor = 0.2f;
    private float enemyYValue = -0.918f;
    public float spawnRate;
    private Transform leftCliff;
    private Transform rightCliff;

    public Transform enemyPrefab;
    public List<Transform> enemies = new List<Transform>();

    private bool canCollide = true;
    public bool canSpawnEnemies = true;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (enemyPrefab.GetComponent<Collider2D>() == null)
        {
            enemyPrefab.gameObject.AddComponent<BoxCollider2D>();
        }

        spawnRate = 0.0f;
        
        StartCoroutine(SearchForCliffs());
        StartCoroutine(SpawnEnemies());
        StartCoroutine(CheckCollisions());
    }

    void Update()
    {
        foreach (Transform enemy in enemies)
        {
            if (player != null && enemy != null)
            {
                Vector3 direction = player.position - enemy.position;

                if (direction.magnitude >= 0.5f)
                {
                    direction.Normalize();
                    enemy.Translate(direction * moveSpeed * moveSpeedFactor * Time.deltaTime);
                    enemy.position = new Vector3(enemy.position.x, enemyYValue, enemy.position.z);

                    if (direction.x < 0)
                    {
                        enemy.localScale = new Vector3(-2, 2, 2);
                    }
                    else if (direction.x > 0)
                    {
                        enemy.localScale = new Vector3(2, 2, 2);
                    }
                }
                else
                {
                    if (canCollide)
                    {
                        Debug.Log("Enemy near the player");
                        canCollide = false;
                    }
                }
            }
        }
    }

    IEnumerator SearchForCliffs()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject floorsObject = GameObject.Find("Floors");

        if (floorsObject != null)
        {
            foreach (Transform child in floorsObject.transform)
            {
                if (child.name.Contains("Cliff"))
                {
                    Transform backgroundChild = child.Find("Background");
                    if (backgroundChild != null)
                    {
                        float childX = backgroundChild.position.x;

                        if (childX < 0)
                        {
                            if (leftCliff == null || childX > leftCliff.position.x)
                            {
                                leftCliff = backgroundChild;
                            }
                        }
                        else
                        {
                            if (rightCliff == null || childX < rightCliff.position.x)
                            {
                                rightCliff = backgroundChild;
                            }
                        }
                    }
                }
            }
        }
    }

    IEnumerator SpawnEnemies()
    {
        if(canSpawnEnemies){
            while (true)
            {
                yield return new WaitForSeconds(spawnRate);

                if (leftCliff != null && rightCliff != null && enemyPrefab != null)
                {
                    Vector3 spawnPosition = Random.Range(0, 2) == 0 ? leftCliff.position : rightCliff.position;
                    spawnPosition += Vector3.forward * Random.Range(-3f, 3f);

                    Transform newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                    enemies.Add(newEnemy);
                }
            }
        }
    }

    IEnumerator CheckCollisions()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            canCollide = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Enter Detected with: " + other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enemy collided with the player!");
        }
    }
}
