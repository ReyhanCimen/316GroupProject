using UnityEngine;

public class EnemySkelly : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    [HideInInspector]
    public int currentHealth;

    public healthBar healthBarUI;

    [Header("Movement Settings")]
    public Transform player; // FPS oyuncusunun transformu
    public float moveSpeed = 2f;

    [Header("Wobble Animation Settings")]
    public float wobbleFrequency = 2f;
    public float wobbleAmount = 0.15f;

    private Vector3 originalScale;

    void Start()
    {
        currentHealth = maxHealth;

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
        }

        originalScale = transform.localScale;
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position);
            direction.y = 0; // Düþman sadece yatay düzlemde hareket etsin
            Vector3 moveDir = direction.normalized;

            // Oyuncuya doðru hareket
            transform.position += moveDir * moveSpeed * Time.deltaTime;

            // Düþman oyuncuya baksýn (saða mý sola mý)
            if (moveDir.x > 0)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            else if (moveDir.x < 0)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            // Sallanma efekti
            float time = Time.time * wobbleFrequency * Mathf.PI * 2;
            float xScale = 1 + Mathf.Sin(time) * wobbleAmount;
            float yScale = 1 - Mathf.Sin(time) * wobbleAmount;

            float baseX = Mathf.Sign(transform.localScale.x) * Mathf.Abs(originalScale.x);
            transform.localScale = new Vector3(baseX * xScale, originalScale.y * yScale, originalScale.z);
        }
    }

    void FindHealthBar()
    {
        healthBar hb = GetComponentInChildren<healthBar>();
        if (hb != null)
        {
            healthBarUI = hb;
            return;
        }

        GameObject[] bars = GameObject.FindGameObjectsWithTag("EnemyUI");
        foreach (GameObject bar in bars)
        {
            hb = bar.GetComponentInChildren<healthBar>();
            if (hb != null && hb.ownerType == healthBar.OwnerType.Enemy)
            {
                healthBarUI = hb;
                break;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (healthBarUI != null)
        {
            healthBarUI.takeDamage(damage);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
