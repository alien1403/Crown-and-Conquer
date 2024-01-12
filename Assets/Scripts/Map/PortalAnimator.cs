using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalAnimator : MonoBehaviour//, IDataPersistence
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        WorldTime worldTime = FindObjectOfType<WorldTime>();
        if(worldTime._currentTime.Hours > worldTime._portalActivationHour)
        {
            TriggerStart();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TriggerStart()
    {
        animator.SetTrigger("Start");
    }
    public void TriggerEnd()
    {
        animator.SetTrigger("End");
    }
}
