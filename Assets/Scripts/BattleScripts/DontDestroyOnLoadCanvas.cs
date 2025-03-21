using UnityEngine;

public class DontDestroyOnLoadCanvas : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}