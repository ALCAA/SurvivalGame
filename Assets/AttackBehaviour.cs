using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Equipment equipmentSystem;

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private InteractBehavior interactBehavior;

    private bool isAttacking;

    [SerializeField]
    private float attackRange;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Vector3 attackOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.DrawRay(transform.position + attackOffset, transform.forward * attackRange, Color.red);

        if (Input.GetMouseButtonDown(0) && CanAttack())
        {
            isAttacking = true;
            SendAttack();
            animator.SetTrigger("Attack");
        }    
    }

    void SendAttack()
    {
        Debug.Log("attack sent");

        RaycastHit hit;
        
        if (Physics.Raycast(transform.position + attackOffset, transform.forward, out hit, attackRange, layerMask))
        {
            if (hit.transform.CompareTag("AI"))
            {
                EnemyAI enemy = hit.transform.GetComponent<EnemyAI>();
                enemy.TakeDamage(equipmentSystem.equipedWeaponItem.attackPoints);
            }
        }
    }

    bool CanAttack()
    {
        /* Pour attaquer on doit :
        - Avoir une arme dans la main
        - Ne pas être en train d'attaquer
        - Ne pas être dans l'inventaire
        - Ne pas être en train de récolter un harvestable
        */
        return (equipmentSystem.equipedWeaponItem != null && !isAttacking && !uiManager.atLeastOnePanelOpened && !interactBehavior.isBusy); 
    }

    public void AttackFinished()
    {
        isAttacking = false;
    }
}
