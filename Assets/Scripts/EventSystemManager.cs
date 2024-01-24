using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;









public class EventSystemManager : MonoBehaviour
{
    private static EventSystemManager _instance;
    public static EventSystemManager Instance { get { return _instance; } }

    private EventSystem eventSystem;







    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        eventSystem = FindAnyObjectByType<EventSystem>();






    }

    public void SetEventSystemActive(bool active)
    {
        if (eventSystem != null)
        {
            eventSystem.enabled = active;
        }
    }
}
