using System.Collections;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public float fadeTime = 1.0f;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    IEnumerator DoFadeIn()
    {
        Color temporaryColor = spriteRenderer.color;
        while(temporaryColor.a < 1f)
        {
            temporaryColor.a += Time.deltaTime / fadeTime;
            if(temporaryColor.a >= 1f)
            {
                temporaryColor.a = 1.0f;
            }
            yield return null;
        }
        spriteRenderer.color = temporaryColor;
    }
    IEnumerator DoFadeOut()
    {
        Color temporaryColor = spriteRenderer.color;
        while (temporaryColor.a < 1f)
        {
            temporaryColor.a -= Time.deltaTime / fadeTime;
            if (temporaryColor.a >= 1f)
            {
                temporaryColor.a = 1.0f;
            }
            yield return null;
        }
        spriteRenderer.color = temporaryColor;
    }
}
