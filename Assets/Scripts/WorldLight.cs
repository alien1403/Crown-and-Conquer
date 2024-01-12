using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class WorldLight : MonoBehaviour
{
    private Light2D _light;
    [SerializeField]
    private WorldTime _worldTime;
    [SerializeField]
    private Gradient _gradient;
    [SerializeField]
    private Gradient _gradientRedNight;
    public WorldTime worldTime;

    public EnemyControllerRazvan enemyController;
    private void Awake()
    {
        _light = GetComponent<Light2D>();
        _worldTime.WorldTimeChanged += OnWorldTimeChanged;
    }
    private void OnWorldTimeChanged(object sender, TimeSpan newTime)
    {
        if(worldTime.dayCount % 10 != 0)
        {
            enemyController.canSpawnEnemies = true;
            _light.color = _gradient.Evaluate(PercentOfDay(newTime));
            enemyController.spawnRate = 5.0f;
        }
        else if(worldTime.dayCount % 10 == 1 && worldTime.dayCount != 1){
           enemyController.canSpawnEnemies = false;
        }
        else
        {
            enemyController.canSpawnEnemies = true;
            _light.color = _gradientRedNight.Evaluate(PercentOfDay(newTime));
            enemyController.spawnRate = 1.0f;
        }
    }
    private float PercentOfDay(TimeSpan timeSpan)
    {
        return (float)timeSpan.TotalMinutes % WorldTimeConstants.MinutesInDay / WorldTimeConstants.MinutesInDay;
    }
    private void OnDestroy()
    {
        _worldTime.WorldTimeChanged -= OnWorldTimeChanged;
    }
}
