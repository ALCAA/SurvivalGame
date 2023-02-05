using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField]
    private List<RecipeData> availableRecipes;
    
    [SerializeField]
    private GameObject recipeUiPrefab;

    [SerializeField]
    private Transform recipesParent;

    [SerializeField]
    private KeyCode openCraftPanelInput;

    [SerializeField]
    private GameObject craftingPanel;

    void Start() 
    {
        UpdateDisplayedRecipes();
    }

    private void Update() 
    {
        if (Input.GetKeyDown(openCraftPanelInput))
        {
            craftingPanel.SetActive(!craftingPanel.activeSelf);
            TooltipSystem.instance.Hide();
            UpdateDisplayedRecipes();
        }
    }

    public void UpdateDisplayedRecipes()
    {
        foreach(Transform child in recipesParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < availableRecipes.Count; i++)
        {
            GameObject currentrecipe = Instantiate(recipeUiPrefab, recipesParent);
            currentrecipe.GetComponent<Recipe>().Configure(availableRecipes[i]);
        }
    }
}
