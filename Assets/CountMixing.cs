using UnityEngine;

public class CountMixing : MonoBehaviour
{
    [SerializeField] private ProgressBarMixing progressBar;
    private Collider2D objectCollider;
    private Vector2 lastMousePosition;
    private bool isSwirling = false;

    void Start()
    {
        objectCollider = GetComponent<Collider2D>();
    }

        void Update()
    {
        if (progressBar.GetValue() >= progressBar.GetMaxValue()) 
        {
            return; 
        }

        if (Input.GetMouseButtonDown(0)) 
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            if (objectCollider.OverlapPoint(mousePos)) 
            {
                lastMousePosition = mousePos;
                isSwirling = true;
            }
        }

        if (Input.GetMouseButton(0) && isSwirling) 
        {
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentMousePosition.z = 0;

            if (objectCollider.OverlapPoint(currentMousePosition)) 
            {
                float movementDistance = Vector2.Distance(currentMousePosition, lastMousePosition);

                if (movementDistance >= 4f) 
                {
                    progressBar.addProgress(1);
                    lastMousePosition = currentMousePosition; 
                }
            }
            else
            {
                isSwirling = false; 
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isSwirling = false;
        }
    }

}
