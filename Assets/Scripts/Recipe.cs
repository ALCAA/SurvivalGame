using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Recipe : MonoBehaviour
{
    private RecipeData currentRecipe;

    [SerializeField]
    private Image craftableItemImage;

    [SerializeField]
    private GameObject elementRequiredPrefab;

    [SerializeField]
    private Transform elementsRequiredParent;

    [SerializeField]
    private Button craftButton;

    [SerializeField]
    private Sprite canbuildIcon;

    [SerializeField]
    private Sprite cantBuildIcon;

    [SerializeField]
    private Color missingColor;

    [SerializeField]
    private Color availableColor;

    public void Configure(RecipeData recipe)
    {
        currentRecipe = recipe;

        craftableItemImage.sprite = recipe.craftableItem.visual;

        // Affichage du Tooltip
        craftableItemImage.transform.GetComponent<Slot>().item = recipe.craftableItem;

        bool canCraft = true;

        for (int i = 0; i < recipe.requiredItems.Length; i++)
        {
            // Récupère tous les éléments nécessaires pour la recette
            GameObject requiredItemGO = Instantiate(elementRequiredPrefab, elementsRequiredParent);
            Image requireItemGoImage = requiredItemGO.GetComponent<Image>();
            ItemData requiredItem = recipe.requiredItems[i].itemData;
            ElementsRequired elementsRequired = requiredItemGO.GetComponent<ElementsRequired>();
            
            requiredItemGO.GetComponent<Slot>().item = requiredItem;
            

            ItemInInventory[] itemInInventory = Inventory.instance.GetContent().Where(elem => elem.itemData == requiredItem).ToArray();
            
            int totalRequiredItemQuantityInInventory = 0;

            for (int j = 0; j < itemInInventory.Length; j++)
            {
                totalRequiredItemQuantityInInventory += itemInInventory[j].count;
            }
            if (totalRequiredItemQuantityInInventory >= recipe.requiredItems[i].count)
            {
                requireItemGoImage.color = availableColor;
            }
            else
            {
                requireItemGoImage.color = missingColor;
                canCraft = false;
            }

            // Configure le visuel de l'élement requis
            elementsRequired.elementImage.sprite = recipe.requiredItems[i].itemData.visual;
            elementsRequired.elementCountText.text = recipe.requiredItems[i].count.ToString();
        }

        // Gestion de l'affichage du button
        craftButton.image.sprite = canCraft ? canbuildIcon : cantBuildIcon;
        craftButton.enabled = canCraft;

        ResizeElementsRequiredParent();
    }

    private void ResizeElementsRequiredParent()
    {
        Canvas.ForceUpdateCanvases();
        elementsRequiredParent.GetComponent<ContentSizeFitter>().enabled = false;
        elementsRequiredParent.GetComponent<ContentSizeFitter>().enabled = true;
    }

    public void CraftItem()
    {
        for (int i = 0; i < currentRecipe.requiredItems.Length; i++)
        {
            for (int j = 0; j < currentRecipe.requiredItems[i].count; j++)
            {
                Inventory.instance.RemoveItem(currentRecipe.requiredItems[i].itemData);
            }
        }
        Inventory.instance.AddItem(currentRecipe.craftableItem);
    }
}
