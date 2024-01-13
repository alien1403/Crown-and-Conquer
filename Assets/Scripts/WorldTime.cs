using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class WorldTime : MonoBehaviour, IDataPersistence
{
    public int dayCount;
    public event EventHandler<TimeSpan> WorldTimeChanged;
    [SerializeField]
    private float _dayLength = 300f;
    public TimeSpan _currentTime;
    private float _minuteLength => _dayLength / WorldTimeConstants.MinutesInDay;
    [SerializeField]
    public int _portalActivationHour;

    public TextMeshProUGUI dayCounterText;

    private void Start()
    {
        ChangeDayCounterText();
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
        ChangeDayCounterText();
    }

    public void ChangeDayCounterText()
    {
        if (dayCount % 10 == 0)
        {
            dayCounterText.text = "Day " + dayCount.ToString();
            dayCounterText.color = Color.red;
            Debug.Log("Red Night");
        }
        else if (dayCount % 10 == 1 && dayCount != 1)
        {
            dayCounterText.text = "Day " + dayCount.ToString();
            dayCounterText.color = Color.white;
            Debug.Log("White Night");
        }
        else
        {
            dayCounterText.text = "";
        }
    }
    public void TriggerPortalActivation()
    {
        IEnumerable<PortalAnimator> portalAnimators = FindObjectsOfType<PortalAnimator>();
        foreach (PortalAnimator portalAnimator in portalAnimators)
        {
            portalAnimator.TriggerStart();
        }
    }

    public void TriggerPortalDeactivation()
    {
        IEnumerable<PortalAnimator> portalAnimators = FindObjectsOfType<PortalAnimator>();
        foreach (PortalAnimator portalAnimator in portalAnimators)
        {
            portalAnimator.TriggerEnd();
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
