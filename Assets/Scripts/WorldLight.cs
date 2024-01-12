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

    private void Awake()
    {
        _light = GetComponent<Light2D>();
        _worldTime.WorldTimeChanged += OnWorldTimeChanged;
    }
    private void OnWorldTimeChanged(object sender, TimeSpan newTime)
    {
        if(worldTime.dayCount % 10 != 0)
        {
            _light.color = _gradient.Evaluate(PercentOfDay(newTime));
        }
        else
        {
            _light.color = _gradientRedNight.Evaluate(PercentOfDay(newTime));
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
