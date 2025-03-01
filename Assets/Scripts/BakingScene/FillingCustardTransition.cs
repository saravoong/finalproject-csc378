using UnityEngine;
using System.Collections;

public class FillingCustardTransition : MonoBehaviour
{
    // Code based off of: https://discussions.unity.com/t/how-do-i-have-a-2d-sprite-fade-in-and-out-c/238127/2
    private SpriteRenderer yourSpriteRenderer;
    private bool stopFading = false; 

    void Start()
    {
        yourSpriteRenderer = GetComponent<SpriteRenderer>();
        Color tmp = yourSpriteRenderer.color;
        tmp.a = 0f; 
        yourSpriteRenderer.color = tmp;
    }

    public IEnumerator FadeIn()
    {
        if (stopFading) yield break;
        stopFading = true;
        float alphaVal = yourSpriteRenderer.color.a;
        Color tmp = yourSpriteRenderer.color;

        while (alphaVal < 1.0f)
        {
            alphaVal += 0.035f;
            tmp.a = alphaVal;
            yourSpriteRenderer.color = tmp;
            yield return new WaitForSeconds(0.05f); 
        }
    }
}
