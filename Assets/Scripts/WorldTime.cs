using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldTime : MonoBehaviour, IDataPersistence
{
    public int dayCount;
    public event EventHandler<TimeSpan> WorldTimeChanged;
    [SerializeField]
    private float _dayLength = 300f;
    private TimeSpan _currentTime;
    private float _minuteLength => _dayLength / WorldTimeConstants.MinutesInDay;

    public TextMeshProUGUI dayCounterText;

    private void Start()
    {
        StartCoroutine(AddMinute());
    }
    public float GetDayLength(){
        return _dayLength;
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
        dayCounterText.text = "Day " + dayCount.ToString();

        if(dayCount % 10 == 0){
            Debug.Log("Red Night");
        }
    }

    public void LoadData(GameData gameData)
    {
        dayCount = gameData.DayCount;
        dayCounterText.text = "Day " + dayCount.ToString();
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
