using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TriggerAttack();
        }
    }

    void TriggerAttack()
    {
        Vector2 lastDir = playerMovement.lastMoveDirection;
        if (lastDir == Vector2.zero)
            lastDir = Vector2.down;
        if (lastDir.y > 0)
        {
            animator.SetTrigger("AttackUp");
        }
        else if (lastDir.y < 0)
        {
            animator.SetTrigger("AttackDown");
        }
        else if (lastDir.x < 0)
        {
            animator.SetTrigger("AttackLeft");
            spriteRenderer.flipX = false;
        }
        else if (lastDir.x > 0)
        {
            animator.SetTrigger("AttackLeft");
            spriteRenderer.flipX = true;
        }
    }
}
