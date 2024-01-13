using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    [Header("Stats")]
    public float MaxHealth = 100f;
    public float MoveSpeed = 5f;
    public float AttackDamage = 10f;
}
