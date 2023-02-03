using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField]
    private float interactRange = 2.6f;

    public InteractBehavior playerInteractBehavior;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private GameObject interactText;
    
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit, interactRange, layerMask))
        {
            if(hit.transform.CompareTag("Item"))
            {
                interactText.SetActive(true);
                if(Input.GetKeyDown(KeyCode.E))
                {
                    playerInteractBehavior.DoPickUp(hit.transform.gameObject.GetComponent<Item>());
                }
            }
            if (hit.transform.CompareTag("Harvestable"))
            {
                interactText.SetActive(true);
                if(Input.GetKeyDown(KeyCode.E))
                {
                    playerInteractBehavior.DoHarvest(hit.transform.gameObject.GetComponent<Harvestable>());
                }
            }
        }
        else
        {
            interactText.SetActive(false);
        }
    }
}
