using UnityEngine;
using TMPro;
using System.Collections;

public class settingText : MonoBehaviour
{
    public float time = 3f;
    //public TextMeshBroUGUI text;
    public GameObject text;

    IEnumerator Start ()
	{
		yield return new WaitForSeconds(time);
		text.SetActive(false);
	}
}
