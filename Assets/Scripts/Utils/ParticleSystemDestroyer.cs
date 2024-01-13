using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParicleSystemDestroyer : MonoBehaviour
{
    private ParticleSystem particleSystem;

    void Start()
    {
        // Assuming your ParticleSystem component is attached to the same GameObject
        particleSystem = GetComponent<ParticleSystem>();

        // Start the coroutine to destroy the GameObject after the particle system animation is done
        StartCoroutine(DestroyAfterAnimation());
    }

    IEnumerator DestroyAfterAnimation()
    {
        // Wait for the duration of the particle system animation
        yield return new WaitForSeconds(particleSystem.main.duration);

        // Destroy the GameObject after the animation is complete
        Destroy(gameObject);
    }
}

