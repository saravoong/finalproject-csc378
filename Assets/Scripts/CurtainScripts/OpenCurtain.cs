using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OpenCurtain : MonoBehaviour
{
    // Help from: https://gamedevbeginner.com/the-right-way-to-lerp-in-unity-with-examples/
    [SerializeField] private Image leftCurtain;
    [SerializeField] private Image rightCurtain;
    private Canvas canvas;
    private float moveSpeed = 1f;

    void Start()
    {
        Invoke("openCurtains", 2f); 
        canvas = GetComponent<Canvas>();
    }

    private void openCurtains()
    {
        StartCoroutine(MoveCurtains());
    }

    private IEnumerator MoveCurtains()
    {
        Vector3 leftStartPos = leftCurtain.rectTransform.anchoredPosition;
        Vector3 rightStartPos = rightCurtain.rectTransform.anchoredPosition;

        Vector3 leftTargetPos = new Vector3(-2443f, 470.4186f, 0f);
        Vector3 rightTargetPos = new Vector3(628f, 470.4186f, 0f);

        float duration = 0f;
        while (duration < 1f)
        {
            leftCurtain.rectTransform.anchoredPosition = Vector3.Lerp(leftStartPos, leftTargetPos, duration);
            rightCurtain.rectTransform.anchoredPosition = Vector3.Lerp(rightStartPos, rightTargetPos, duration);

            duration += Time.deltaTime * moveSpeed;
            yield return null; 
        }

        leftCurtain.rectTransform.anchoredPosition = leftTargetPos;
        rightCurtain.rectTransform.anchoredPosition = rightTargetPos;
        canvas.gameObject.SetActive(false);
    }
}
