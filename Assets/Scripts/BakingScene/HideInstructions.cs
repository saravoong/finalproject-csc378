using UnityEngine;
using System.Collections;

public class HideInstructions : MonoBehaviour
{
    private SpriteRenderer yourSpriteRenderer;
    private bool stopFading = false; 

    void Start()
    {
        yourSpriteRenderer = GetComponent<SpriteRenderer>();
        Color tmp = yourSpriteRenderer.color;
        tmp.a = 1f; 
        yourSpriteRenderer.color = tmp;

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(2f);

        if (stopFading) yield break;
        stopFading = true;
        float alphaVal = yourSpriteRenderer.color.a;
        Color tmp = yourSpriteRenderer.color;

        while (alphaVal > 0.0f)
        {
            alphaVal -= 0.035f;
            tmp.a = alphaVal;
            yourSpriteRenderer.color = tmp;
            yield return new WaitForSeconds(0.05f); 
        }
    }
}
