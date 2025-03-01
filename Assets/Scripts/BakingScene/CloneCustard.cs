using UnityEngine;

public class CloneCustard : MonoBehaviour
{
    // Code based off of: https://docs.unity3d.com/530/Documentation/ScriptReference/Object.Instantiate.html
    [SerializeField] ProgressBarCustard progressBar;
    public GameObject spritePrefab;
    private Collider2D objectCollider;
    void Start()
    {
        objectCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (progressBar.GetValue() >= progressBar.GetMaxValue()) {
                return;   
            }

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; 

             if (objectCollider.OverlapPoint(mousePos)) {
                Instantiate(spritePrefab, mousePos, Quaternion.identity);
                progressBar.addProgress(1);
            }
        }
    }

}
