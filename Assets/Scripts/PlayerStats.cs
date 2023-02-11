using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Other elements references")]
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private MoveBehaviour playerMovementScript;

    [SerializeField]
    private AimBehaviourBasic playerAimScript;

    [Header("Health")]
    [SerializeField]
    private float maxHealth = 100f;
    private float currentHealth;

    [SerializeField]
    private Image healthBarFill;
    // Start is called before the first frame update

    [Header("Hunger")]
    [SerializeField]
    private float maxHunger = 100f;
    private float currentHunger;

    [SerializeField]
    private Image hungerBarFill;

    [SerializeField]
    private float hungerDecreaseRate;

    [Header("Thrist")]
    [SerializeField]
    private float maxThirst = 100f;
    private float currentThrist;

    [SerializeField]
    private Image thirstBarFill;

    [SerializeField]
    private float thirstDecreaseRate;

    public float currentArmorPoints;

    [HideInInspector]
    public bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
        currentHunger = maxHunger;   
        currentThrist = maxThirst;
    }
    void Start()
    {
        
    }

    public void ConsumeItem(float health, float hunger, float thirst)
    {
        currentHealth += health;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        currentHunger += hunger;
        if (currentHunger > maxHunger)
        {
            currentHunger = maxHunger;
        }

        currentThrist += thirst;
        if (currentThrist > maxThirst)
        {
            currentThrist = maxThirst;
        }

        UpdateHealthBarFill();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHungerBarFill();
        UpdateThristBarFill();
    }

    public void TakeDamage(float damage, bool overTime = false)
    {
        if (overTime)
        {
            currentHealth -= damage * Time.deltaTime;
        }
        else
        {
            currentHealth -= damage * (1 - (currentArmorPoints / 100));
        }

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
        UpdateHealthBarFill();
    }

    private void Die()
    {
        isDead = true;
        playerMovementScript.canMove = false;
        playerAimScript.enabled = false;
        hungerDecreaseRate = 0;
        thirstDecreaseRate = 0;
        animator.SetTrigger("Die");
    }

    void UpdateHealthBarFill()
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
    }

    void UpdateHungerBarFill()
    {
        currentHunger -= hungerDecreaseRate * Time.deltaTime;
        hungerBarFill.fillAmount = currentHunger / maxHunger;

        currentHunger = currentHunger < 0 ? 0 : currentHunger;

        if (currentHunger <= 0)
        {
            TakeDamage(0.1f, true);
        }
    }

    void UpdateThristBarFill()
    {
        currentThrist -= thirstDecreaseRate * Time.deltaTime;
        thirstBarFill.fillAmount = currentThrist / maxThirst;

        currentThrist = currentThrist < 0 ? 0 : currentThrist;

        if (currentThrist <= 0)
        {
            TakeDamage(0.1f, true);
        }
    }
}
