using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyDifficulty
{
    Easy,
    Medium,
    Hard
}

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    [Header("Stats")]
    public float MaxHealth = 100f;
    public float MoveSpeed = 5f;
    public float AttackDamage = 10f;
    public EnemyDifficulty Difficulty = EnemyDifficulty.Easy;
}
