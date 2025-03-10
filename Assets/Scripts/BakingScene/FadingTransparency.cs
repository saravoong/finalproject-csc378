using UnityEngine;
using System.Collections;

public class FadingTransparency : MonoBehaviour
{
    // Code based off of: https://discussions.unity.com/t/how-do-i-have-a-2d-sprite-fade-in-and-out-c/238127/2
    private SpriteRenderer yourSpriteRenderer;
    private bool stopFading = false; 

    void Start()
    {
        yourSpriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(FadeLoop());
    }

    private IEnumerator FadeLoop()
    {
        while (!stopFading) 
        {
            yield return StartCoroutine(FadeIn());
            yield return StartCoroutine(FadeOut());
        }
    }

    public IEnumerator FadeIn()
    {
        float alphaVal = yourSpriteRenderer.color.a;
        Color tmp = yourSpriteRenderer.color;

        while (yourSpriteRenderer.color.a > 0.25 && !stopFading)
        {
            alphaVal -= 0.035f;
            tmp.a = alphaVal;
            yourSpriteRenderer.color = tmp;

            yield return new WaitForSeconds(0.05f); 
        }
    }

    public IEnumerator FadeOut()
    {
        float alphaVal = yourSpriteRenderer.color.a;
        Color tmp = yourSpriteRenderer.color;

        while (yourSpriteRenderer.color.a < 1 && !stopFading)
        {
            alphaVal += 0.035f;
            tmp.a = alphaVal;
            yourSpriteRenderer.color = tmp;

            yield return new WaitForSeconds(0.05f); 
        }
    }

    public void StopFading()
    {
        stopFading = true; 
        StopAllCoroutines(); 
        Color tmp = yourSpriteRenderer.color;
        tmp.a = 0f; 
        yourSpriteRenderer.color = tmp;
    }
}
