using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBehavior : MonoBehaviour
{
    [SerializeField]
    private MoveBehaviour PlayerMoveBehaviour;

    [SerializeField]
    private Animator PlayerAnimator;

    [SerializeField]
    private Inventory inventory;

    private Item currentItem;
    private Harvestable currentHarvestable;
    private Tool currentTool;

    [Header("Tools visual")]
    [SerializeField]
    private GameObject pickaxeVisual;

    [SerializeField]
    private GameObject axeVisual;

    private Vector3 spawnItemOffset = new Vector3(0, 0.5f, 0);
    
    public void DoPickUp(Item item)
    {
        if (!inventory.IsFull())
        {
            currentItem = item;

            PlayerAnimator.SetTrigger("PickUp");
            PlayerMoveBehaviour.canMove = false;
        }
    }

    public void DoHarvest(Harvestable harvestable)
    {
        currentTool = harvestable.tool;
        EnableToolGameObjectFromEnum(currentTool);

        currentHarvestable = harvestable;
        PlayerAnimator.SetTrigger("Harvest");
        PlayerMoveBehaviour.canMove = false;        
    }

    IEnumerator BreakHarvestable()
    {
        if (currentHarvestable.disableKinematicOnHarvest)
        {
            Rigidbody rigidbody = currentHarvestable.gameObject.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.AddForce(new Vector3(750, 750, 0), ForceMode.Impulse);
        }

        yield return new WaitForSeconds(currentHarvestable.destroyDelay);

        for (int i = 0; i < currentHarvestable.harvestableItems.Length; i++)
        {
            Ressource ressource = currentHarvestable.harvestableItems[i];

            if (Random.Range(1,101) <= ressource.dropChance)
            {
                GameObject instanciatedRessource = Instantiate(ressource.itemData.prefab);
                instanciatedRessource.transform.position = currentHarvestable.transform.position + spawnItemOffset;
            }
        }
        Destroy(currentHarvestable.gameObject);
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
    }

    private void EnableToolGameObjectFromEnum(Tool toolType, bool enabled = true)
    {
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
