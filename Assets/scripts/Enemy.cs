using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    [HideInInspector]
    public int currentHealth;

    public healthBar healthBarUI;
    public bool isEnemyHit = false;

    private EnemySoundManager soundManager;

    void Start()
    {
        currentHealth = maxHealth;

        soundManager = GetComponentInChildren<EnemySoundManager>();

        if (healthBarUI == null)
        {
            FindHealthBar();
        }

        if (healthBarUI != null)
        {
            healthBarUI.ownerType = healthBar.OwnerType.Enemy;
            healthBarUI.maxHealth = maxHealth;
            healthBarUI.health = maxHealth;

            if (healthBarUI.healthSlider != null)
                healthBarUI.healthSlider.value = maxHealth;

            if (healthBarUI.easeHealthSlider != null)
                healthBarUI.easeHealthSlider.value = maxHealth;

            Debug.Log(gameObject.name + " için Health Bar ayarlandı.");
        }
        else
        {
            Debug.LogError(gameObject.name + " için Health Bar bulunamadı!");
        }
    }

    void FindHealthBar()
    {
        healthBar hb = GetComponentInChildren<healthBar>();
        if (hb != null)
        {
            healthBarUI = hb;
            Debug.Log("Health Bar çocuk objede bulundu.");
            return;
        }

        GameObject[] bars = GameObject.FindGameObjectsWithTag("EnemyUI");
        foreach (GameObject bar in bars)
        {
            hb = bar.GetComponentInChildren<healthBar>();
            if (hb != null && hb.ownerType == healthBar.OwnerType.Enemy)
            {
                healthBarUI = hb;
                Debug.Log("Health Bar EnemyUI tag'i ile bulundu.");
                break;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        isEnemyHit = true;

        Debug.Log(gameObject.name + " hasar aldı! Kalan can: " + currentHealth);

        if (healthBarUI != null)
        {
            healthBarUI.takeDamage(damage);
        }

        if (soundManager != null)
        {
            soundManager.PlayHitSound();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " öldü!");
        if (soundManager != null)
        {
            soundManager.PlayDeathSound();
        }

        Destroy(gameObject, 1.0f);
    }
}
