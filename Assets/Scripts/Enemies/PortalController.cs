using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{

    private void Awake()
    {
        EnemyManager.instance.enemiesPortals.Add(gameObject);
    }
}
