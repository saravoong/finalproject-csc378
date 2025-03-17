using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UINonFading : MonoBehaviour
{
    private Image UIImage;
    private bool isFading = false; 

    void Start()
    {
        UIImage = GetComponent<Image>();
        StartCoroutine(WaitThenFadeOut(2.5f));
    }
    private IEnumerator WaitThenFadeOut(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); 
        StartCoroutine(FadeOut());
    }
    public IEnumerator FadeOut()
    {
        if (isFading) yield break; 
        isFading = true;
        float alphaVal = UIImage.color.a;
        Color tmp = UIImage.color;

        while (alphaVal < 1.0f)
        {
            alphaVal += 0.02f;
            tmp.a = alphaVal;
            UIImage.color = tmp;

            yield return new WaitForSeconds(0.05f); 
        }

        tmp.a = 1f;
        UIImage.color = tmp;
        isFading = false;
    }

}
