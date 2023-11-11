using System.Collections;
using UnityEngine;


public delegate IEnumerator FadeDelegate();

public class Fade : MonoBehaviour
{
    public float fadeTime = 1.0f;
    SpriteRenderer[] childRenderers;

    void Start()
    {
        childRenderers = GetComponentsInChildren<SpriteRenderer>();
    }
    public IEnumerator DoFadeIn()
    {
        Color temporaryColor = childRenderers[0].color;
        while (temporaryColor.a < 1f)
        {

            temporaryColor.a += Time.deltaTime / fadeTime;
            foreach (SpriteRenderer childRenderer in childRenderers)
                childRenderer.color = temporaryColor;
            yield return null;
        }
    }
    public IEnumerator DoFadeOut()
    {
        Color temporaryColor = childRenderers[0].color;
        while (temporaryColor.a > 0f)
        {
            temporaryColor.a -= Time.deltaTime / fadeTime;
            foreach (SpriteRenderer childRenderer in childRenderers)
                childRenderer.color = temporaryColor;
            yield return null;
        }
    }
    public IEnumerator StartFadeWithDelay(FadeDelegate fadeAction, float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(fadeAction());
    }
}
