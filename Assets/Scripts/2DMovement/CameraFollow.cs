using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        if(player != null)
        {
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }
    }
}
