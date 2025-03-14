using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBased : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask obstacleLayers;
    public Transform movePoint;
    public Transform frontCollider;
    public Vector2 lastDir;

    public AudioClip blockedSound;
    public float blockedSoundVolume = 1f;
    public float blockedSoundCadence = 0.5f;
    private float lastBlockedSoundTime = -Mathf.Infinity;
    
    public Animator anim;
    // If you have an obstacle layer, it might be declared like this:
    // public LayerMask obstacleLayer;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        lastDir = new Vector2(0f, -1f);
    }

    // Update is called once per frame
    void Update()
    {
        frontCollider.position = transform.position + new Vector3(lastDir.x, lastDir.y, 0);
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            lastDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if (!Physics2D.OverlapCircle(
                        movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f),
                        0.2f,
                        obstacleLayers
                    ))
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f);
                } else {
                    if (blockedSound != null && Time.time - lastBlockedSoundTime >= blockedSoundCadence)
                    {
                        AudioSource.PlayClipAtPoint(blockedSound, transform.position, blockedSoundVolume);
                        lastBlockedSoundTime = Time.time;
                    }
                }
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if (!Physics2D.OverlapCircle(
                        movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical")),
                        0.2f,
                        obstacleLayers
                    ))
                {
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                } else {
                    if (blockedSound != null && Time.time - lastBlockedSoundTime >= blockedSoundCadence)
                    {
                        AudioSource.PlayClipAtPoint(blockedSound, transform.position, blockedSoundVolume);
                        lastBlockedSoundTime = Time.time;
                    }
                }
            }
        }
    }
}
