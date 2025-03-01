using UnityEngine;

public class DragDrop : MonoBehaviour
{
    // Code based off of: https://www.youtube.com/watch?v=CKQY8MdtBLI&ab_channel=Unity3DSchool
    Vector3 offset;
    Collider2D objectCollider;
    public string destinationTag = "DropArea";
    private bool isDropped = false;
    [SerializeField] ProgressBarStrawberry progressBar;

    void Awake() {
        objectCollider = GetComponent<Collider2D>();
    }

    void OnMouseDown()
    {
        if (isDropped) return;
        offset = transform.position - MouseWorldPosition();
    }

    void OnMouseDrag()
    {
        if (isDropped) return;
        transform.position = MouseWorldPosition() + offset;
    }

    void OnMouseUp() {
        if (progressBar.GetValue() >= progressBar.GetMaxValue()) {
                return;   
        }

        if (isDropped) return;
        objectCollider.enabled = false;
        var rayOrigin = Camera.main.transform.position;
        var rayDirection = MouseWorldPosition() - Camera.main.transform.position;
        RaycastHit2D hitInfo;
        if (hitInfo = Physics2D.Raycast(rayOrigin, rayDirection)) {
            if (hitInfo.transform.tag == destinationTag) {
                transform.position = hitInfo.transform.position + new Vector3(0, 0.5f, -0.1f);
                isDropped = true;
                this.enabled = false;

                FadingTransparency fadingScript = hitInfo.transform.GetComponent<FadingTransparency>();
                if (fadingScript != null) {
                    fadingScript.StopFading();
                }

                progressBar.addProgress(1);
            }
        }
        objectCollider.enabled = true;
    }

    Vector3 MouseWorldPosition() {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }

}
