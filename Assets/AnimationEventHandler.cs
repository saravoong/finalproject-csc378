using UnityEngine;

// Create a new script called AnimationEventHandler.cs and attach it to the child with the animator
public class AnimationEventHandler : MonoBehaviour
{
    private PlayerCombat playerCombat; // Or whatever your main script is called


    void Start()
    {
        playerCombat = GetComponentInParent<PlayerCombat>(); // Find parent script
    }

    public void EndAttackAnimation()
    {
        // playerCombat.EndAttackAnimation(); // Forward the call to parent
        playerCombat.animator.SetBool("Attacking", false);
    }
}