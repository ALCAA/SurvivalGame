using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [Header("Other scripts References")]

    [SerializeField]
    private Equipment equipment;

    [SerializeField]
    private ItemActionsSystem itemActionsSystem;

    [Header("Inventory system variables")]

    [SerializeField]
    private List<ItemData> content = new List<ItemData>();

    [SerializeField]
    private GameObject inventoryPanel;

    [SerializeField]
    private Transform inventorySlotsParent;

    public Sprite emptySlotVisual;

    public static Inventory instance;

    const int InventorySize = 20;
    private bool isOpen = false;

    private void Awake() 
    {
        instance = this;
    }
    private void Start()
    {
        CloseInventory();
        RefreshContent();
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (isOpen)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
    }

    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        isOpen = true;
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        itemActionsSystem.actionPanel.SetActive(false);
        TooltipSystem.instance.Hide();
        isOpen = false;
    }
    public void AddItem(ItemData item)
    {
        content.Add(item);
        RefreshContent();
    }

    public void RemoveItem(ItemData item)
    {
        content.Remove(item);
        RefreshContent();
    }

    public void RefreshContent()
    {
        for (int i = 0; i < inventorySlotsParent.childCount; i++)
        {
            Slot currentSlot = inventorySlotsParent.GetChild(i).GetComponent<Slot>();
            currentSlot.item = null;
            currentSlot.itemVisual.sprite = emptySlotVisual;
        }
        for (int i = 0; i < content.Count; i++)
        {
            Slot currentSlot = inventorySlotsParent.GetChild(i).GetComponent<Slot>();
            currentSlot.item = content[i];
            currentSlot.itemVisual.sprite = content[i].visual;
       }

       equipment.UpdateEquipmentsDesequipButtons();
    }

    public bool IsFull()
    {
        return InventorySize == content.Count;
    }
}
