using UnityEngine;

public class ItemActionsSystem : MonoBehaviour
{
    [Header("Other scripts References")]

    [SerializeField]
    private Equipment equipment;

    [Header("Action Panel References")]

    public GameObject actionPanel;

    [SerializeField]
    private GameObject useButton;

    [SerializeField]
    private GameObject equipButton;

    [SerializeField]
    private GameObject dropButton;

    [SerializeField]
    private GameObject destroyButton;

    [HideInInspector]
    public ItemData itemCurrentlySelected;

    [SerializeField]
    private Transform dropPoint;

    public void OpenActionPanel(ItemData item, Vector3 slotPosition)
    {
        itemCurrentlySelected = item;
        if (item == null)
        {
            actionPanel.SetActive(false);
            return;
        }
        switch(item.itemType)
        {
            case ItemType.Ressource:
                useButton.SetActive(false);
                equipButton.SetActive(false);
                break;

            case ItemType.Equipment:
                useButton.SetActive(false);
                equipButton.SetActive(true);
                break;

            case ItemType.Consumable:
                useButton.SetActive(true);
                equipButton.SetActive(false);   
                break;    
        }
        actionPanel.transform.position = slotPosition;
        actionPanel.SetActive(true);
    }

    public void CloseActionPanel()
    {
        actionPanel.SetActive(false);
        itemCurrentlySelected = null;
    }

    public void UseActionButton()
    {
        CloseActionPanel();
    }

    public void EquipActionButton()
    {
        equipment.EquipAction();
    }

    public void DropActionButton()
    {
        GameObject instantiatedItem = Instantiate(itemCurrentlySelected.prefab);
        instantiatedItem.transform.position = dropPoint.position;
       Inventory.instance.RemoveItem(itemCurrentlySelected);
        Inventory.instance.RefreshContent();
        CloseActionPanel();
    }

    public void DestroyActionButton()
    {
        Inventory.instance.RemoveItem(itemCurrentlySelected);
        Inventory.instance.RefreshContent();
        CloseActionPanel();
    }
}
