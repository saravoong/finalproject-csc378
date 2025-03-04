using UnityEngine;
using UnityEngine.SceneManagement;

public class MixingManager : MonoBehaviour
{
    [SerializeField] private GameObject[] itemsInBowl; 
    [SerializeField] private GameObject placeholder; 
    [SerializeField] private GameObject progressBarObject;
    [SerializeField] private ProgressBarMixing progressBar; 
    [SerializeField] private GameObject mixIngredientsInstructions; 

    void Update()
    {
        if (AllItemsInactive()) 
        {
            //placeholder.SetActive(false);
            //progressBarObject.SetActive(true);
            //mixIngredientsInstructions.SetActive(true);
            SceneManager.LoadScene("MixingIngredients");
        }
    }

    private bool AllItemsInactive()
    {
        foreach (GameObject item in itemsInBowl)
        {
            if (item.activeSelf) return false;
        }
        return true;
    }
}
