using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehavior : MonoBehaviour, IEntity
{
    private WallController wallController;
    private WallAnimator wallAnimator;
    private bool thresholdReached1 = false;
    private bool thresholdReached2 = false;
    private void Start()
    {
        wallController = transform.parent.GetComponent<WallController>();
        wallAnimator = GetComponent<WallAnimator>();
    }
    public void TakeDamage(float damageTaken)
    {
        wallController.currentHealth -= damageTaken;
        if (wallController.currentHealth / wallController.maxHealth < wallController.firstHitThreshold && thresholdReached1 == false)
        {
            thresholdReached1 = true;
            wallAnimator.TriggerHit1();
        }
        if(wallController.currentHealth / wallController.maxHealth < wallController.secondHitThreshold && thresholdReached2 == false && thresholdReached1 == true)
        {
            thresholdReached2 = true;
            wallAnimator.TriggerHit2();
        }
        if (wallController.currentHealth < 0)
        {
            wallAnimator.TriggerDeath();
            wallController.SwitchToDestroyed();
        }
    }
}
