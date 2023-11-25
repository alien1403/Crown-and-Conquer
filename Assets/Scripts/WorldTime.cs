using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTime : MonoBehaviour, IDataPersistence
{
    public int dayCount;
    public event EventHandler<TimeSpan> WorldTimeChanged;
    [SerializeField]
    private float _dayLength;
    private TimeSpan _currentTime;
    private float _minuteLength => _dayLength / WorldTimeConstants.MinutesInDay;

    private void Start()
    {
        StartCoroutine(AddMinute());
    }

    private IEnumerator AddMinute()
    {
        _currentTime += TimeSpan.FromMinutes(1);
        WorldTimeChanged?.Invoke(this, _currentTime);
        yield return new WaitForSeconds(_minuteLength);
        StartCoroutine(AddMinute());
    }
    public void IncrementDayCount()
    {
        dayCount++;
    }

    public void LoadData(GameData gameData)
    {
        dayCount = gameData.DayCount;
        _currentTime = new TimeSpan(gameData.Hour, gameData.Minute, 0);
        WorldTimeChanged?.Invoke(this, _currentTime);
    }

    public void SaveData(GameData gameData)
    {
        gameData.DayCount = dayCount;
        gameData.Hour = _currentTime.Hours;
        gameData.Minute = _currentTime.Minutes;
    }
}
