using System.Collections;
using UnityEngine;
using System.Linq;

public class InteractBehavior : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private MoveBehaviour PlayerMoveBehaviour;

    [SerializeField]
    private Animator PlayerAnimator;

    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private Equipment equipmentSystem;

    [SerializeField]
    private EquipmentLibrary equipmentLibrary;

    private Item currentItem;
    private Harvestable currentHarvestable;
    private Tool currentTool;

    public bool isBusy = false;

    [Header("Tools visual")]
    [SerializeField]
    private GameObject pickaxeVisual;

    [SerializeField]
    private GameObject axeVisual;

    private Vector3 spawnItemOffset = new Vector3(0, 0.5f, 0);
    
    public void DoPickUp(Item item)
    {
        if (isBusy)
        {
            return;
        }

        isBusy = true;
        if (!inventory.IsFull())
        {
            currentItem = item;

            PlayerAnimator.SetTrigger("PickUp");
            PlayerMoveBehaviour.canMove = false;
        }
    }

    public void DoHarvest(Harvestable harvestable)
    {
        if (isBusy)
        {
            return;
        }
        isBusy = true;
        currentTool = harvestable.tool;
        EnableToolGameObjectFromEnum(currentTool);

        currentHarvestable = harvestable;
        PlayerAnimator.SetTrigger("Harvest");
        PlayerMoveBehaviour.canMove = false;        
    }

    IEnumerator BreakHarvestable()
    {
        Harvestable currentlyHarveting = currentHarvestable;
        // Fix la possibilité de break un harvestable alors qu'il est déjà break
        currentlyHarveting.gameObject.layer = LayerMask.NameToLayer("Default");

        if (currentlyHarveting.disableKinematicOnHarvest)
        {
            Rigidbody rigidbody = currentlyHarveting.gameObject.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.AddForce(transform.forward * 800, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(currentlyHarveting.destroyDelay);

        for (int i = 0; i < currentlyHarveting.harvestableItems.Length; i++)
        {
            Ressource ressource = currentlyHarveting.harvestableItems[i];

            if (Random.Range(1,101) <= ressource.dropChance)
            {
                GameObject instanciatedRessource = Instantiate(ressource.itemData.prefab);
                instanciatedRessource.transform.position = currentlyHarveting.transform.position + spawnItemOffset;
            }
        }
        Destroy(currentlyHarveting.gameObject);
    }

    public void AddItemToInventory()
    {
        inventory.AddItem(currentItem.itemData);
        Destroy(currentItem.gameObject); 
    }

    public void ReEnablePlayMovement()
    {
        EnableToolGameObjectFromEnum(currentTool, false);
        PlayerMoveBehaviour.canMove = true;
        isBusy = false;   
    }

    private void EnableToolGameObjectFromEnum(Tool toolType, bool enabled = true)
    {
        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem => elem.itemData == equipmentSystem.equipedWeaponItem).FirstOrDefault();
        
        if (equipmentLibraryItem != null)
        {
            for (int i = 0; i < equipmentLibraryItem.elementsToDisable.Length; i++)
            {
                equipmentLibraryItem.elementsToDisable[i].SetActive(enabled);
            }
            equipmentLibraryItem.itemPrefab.SetActive(!enabled);
        }

        switch (toolType)
        {
            case Tool.Pickaxe:
                pickaxeVisual.SetActive(enabled);
                break;
            case Tool.Axe:
                axeVisual.SetActive(enabled);
                break;
        }
    }
}
