using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
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

    private void Awake()
    {
        currentHealth = maxHealth;
        currentHunger = maxHunger;   
        currentThrist = maxThirst;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHungerBarFill();
        UpdateThristBarFill();
    }

    void TakeDamage(float damage, bool overTime = false)
    {
        if (overTime)
        {
            currentHealth -= damage * Time.deltaTime;
        }
        else
        {
            currentHealth -= damage;
        }

        if (currentHealth <= 0)
        {
            Debug.Log("You died :(");
        }
        UpdateHealthBarFill();
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
