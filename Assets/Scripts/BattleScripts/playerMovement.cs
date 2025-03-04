using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput*speed;
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position+moveVelocity*Time.fixedDeltaTime);
    }

    // public void setCanMove(bool b) {
    //     canMove = b;
    // }
}
