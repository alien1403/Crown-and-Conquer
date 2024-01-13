using UnityEngine;

public class WallAnimator : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }
    public void TriggerHit1()
    {
        animator.SetTrigger("Hit1");
    }
    public void TriggerHit2()
    {
        animator.SetTrigger("Hit2");
    }
    public void TriggerDeath()
    {
        animator.SetTrigger("Die");
    }
}
