using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Equipment : MonoBehaviour
{
    [Header("Other scripts References")]

    [SerializeField]
    private ItemActionsSystem itemActionsSystem;

    [SerializeField]
    private PlayerStats playerStats;

    [Header("Equipment Panel References")]

    [SerializeField]
    private EquipmentLibrary equipmentLibrary;

    [SerializeField]
    private Image headerSlotImage;

    [SerializeField]
    private Image chestSlotImage;

    [SerializeField]
    private Image gloveSlotImage;

    [SerializeField]
    private Image pantsSlotImage;

    [SerializeField]
    private Image bootsSlotImage;

    private ItemData equipedHeadItem;

    private ItemData equipedChestItem;

    private ItemData equipedHandsItem;

    private ItemData equipedLegsItem;

    private ItemData equipedFeetItem;

    [SerializeField]
    private Button headSlotDesequipButton;

    [SerializeField]
    private Button chestSlotDesequipButton;

    [SerializeField]
    private Button handsSlotDesequipButton;

    [SerializeField]
    private Button legsSlotDesequipButton;

    [SerializeField]
    private Button feetSlotDesequipButton;

    public void EquipAction()
    {
        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem => elem.itemData == itemActionsSystem.itemCurrentlySelected).First();
        
        if (equipmentLibraryItem != null)
        {         
            switch(itemActionsSystem.itemCurrentlySelected.equipmentType)
            {
                case EquipmentType.Head:
                    DisabledPreviousEquipedEquipment(equipedHeadItem);
                    headerSlotImage.sprite = itemActionsSystem.itemCurrentlySelected.visual;
                    equipedHeadItem = itemActionsSystem.itemCurrentlySelected;
                    break;

                case EquipmentType.Chest:
                    DisabledPreviousEquipedEquipment(equipedChestItem);
                    chestSlotImage.sprite = itemActionsSystem.itemCurrentlySelected.visual;
                    equipedChestItem = itemActionsSystem.itemCurrentlySelected;
                    break;

                case EquipmentType.Hands:
                    DisabledPreviousEquipedEquipment(equipedHandsItem);
                    gloveSlotImage.sprite = itemActionsSystem.itemCurrentlySelected.visual;
                    equipedHandsItem = itemActionsSystem.itemCurrentlySelected;
                    break;

                case EquipmentType.Legs:
                    DisabledPreviousEquipedEquipment(equipedLegsItem);
                    pantsSlotImage.sprite = itemActionsSystem.itemCurrentlySelected.visual;
                    equipedLegsItem = itemActionsSystem.itemCurrentlySelected;
                    break;

                case EquipmentType.Feet:
                    DisabledPreviousEquipedEquipment(equipedFeetItem);
                    bootsSlotImage.sprite = itemActionsSystem.itemCurrentlySelected.visual;
                    equipedFeetItem = itemActionsSystem.itemCurrentlySelected;
                    break;
            }

            for (int i = 0; i < equipmentLibraryItem.elementsToDisable.Length; i++)
            {
                equipmentLibraryItem.elementsToDisable[i].SetActive(false);
            }
            equipmentLibraryItem.itemPrefab.SetActive(true);

            playerStats.currentArmorPoints += itemActionsSystem.itemCurrentlySelected.armorPoints;

            Inventory.instance.RemoveItem(itemActionsSystem.itemCurrentlySelected);
        }
        else
        {
            Debug.LogError("Equipment : " + itemActionsSystem.itemCurrentlySelected.name + " is not present");
        }
        itemActionsSystem.CloseActionPanel();
    }

    public void UpdateEquipmentsDesequipButtons()
    {
        headSlotDesequipButton.onClick.RemoveAllListeners();
        headSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Head); });
        headSlotDesequipButton.gameObject.SetActive(equipedHeadItem);
        
        chestSlotDesequipButton.onClick.RemoveAllListeners();
        chestSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Chest); });
        chestSlotDesequipButton.gameObject.SetActive(equipedChestItem);

        handsSlotDesequipButton.onClick.RemoveAllListeners();
        handsSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Hands); });
        handsSlotDesequipButton.gameObject.SetActive(equipedHandsItem);

        legsSlotDesequipButton.onClick.RemoveAllListeners();
        legsSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Legs); });
        legsSlotDesequipButton.gameObject.SetActive(equipedLegsItem);

        feetSlotDesequipButton.onClick.RemoveAllListeners();
        feetSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Feet); });
        feetSlotDesequipButton.gameObject.SetActive(equipedFeetItem);
    }

    public void DesequipEquipment(EquipmentType equipmentType)
    {
        if (Inventory.instance.IsFull())
        {
            return;
        }

        ItemData currentItem = null;

        switch(equipmentType)
        {
            case EquipmentType.Head:
                currentItem = equipedHeadItem;
                equipedHeadItem = null;
                headerSlotImage.sprite = Inventory.instance.emptySlotVisual;
                break;
            
            case EquipmentType.Chest:
                currentItem = equipedChestItem;
                equipedChestItem = null;
                chestSlotImage.sprite = Inventory.instance.emptySlotVisual;
                break;

            case EquipmentType.Hands:
                currentItem = equipedHandsItem;
                equipedHandsItem = null;
                headerSlotImage.sprite = Inventory.instance.emptySlotVisual;
                break;

            case EquipmentType.Legs:
                currentItem = equipedLegsItem;
                equipedLegsItem = null;
                pantsSlotImage.sprite = Inventory.instance.emptySlotVisual;
                break;

            case EquipmentType.Feet:
                currentItem = equipedFeetItem;
                equipedFeetItem = null;
                bootsSlotImage.sprite = Inventory.instance.emptySlotVisual;
                break;
        }

        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem => elem.itemData == currentItem).First();
        
        if (equipmentLibraryItem != null)
        {
            for (int i = 0; i < equipmentLibraryItem.elementsToDisable.Length; i++)
            {
                equipmentLibraryItem.elementsToDisable[i].SetActive(true);
            }
            equipmentLibraryItem.itemPrefab.SetActive(false);
        }

        playerStats.currentArmorPoints -= currentItem.armorPoints;

        Inventory.instance.AddItem(currentItem);

        Inventory.instance.RefreshContent();
    }

    private void DisabledPreviousEquipedEquipment(ItemData itemToDisable)
    {
        if (itemToDisable == null)
        {
            return;
        }

        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem => elem.itemData == itemToDisable).First();
        
        if (equipmentLibraryItem != null)
        {
            for (int i = 0; i < equipmentLibraryItem.elementsToDisable.Length; i++)
            {
                equipmentLibraryItem.elementsToDisable[i].SetActive(true);
            }
            equipmentLibraryItem.itemPrefab.SetActive(false);
        }

        playerStats.currentArmorPoints -= itemToDisable.armorPoints;

        Inventory.instance.AddItem(itemToDisable);
    }

}
