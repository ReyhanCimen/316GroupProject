using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    [HideInInspector] // Inspector'da gösterme ama diğer scriptlerden erişilebilir
    public int currentHealth;

    // Health Bar UI referansı
    public healthBar healthBarUI;

    void Start()
    {
        currentHealth = maxHealth;

        // Health Bar referansı yoksa bul
        if (healthBarUI == null)
        {
            FindHealthBar();
        }

        // Health Bar'ı başlangıçta ayarlayın
        if (healthBarUI != null)
        {
            healthBarUI.ownerType = healthBar.OwnerType.Enemy;
            healthBarUI.maxHealth = maxHealth;
            healthBarUI.health = maxHealth;

            // Health Bar slider değerlerinin doğru ayarlandığından emin olalım
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
        // Önce çocuk objeler içinde ara
        healthBar hb = GetComponentInChildren<healthBar>();
        if (hb != null)
        {
            healthBarUI = hb;
            Debug.Log("Health Bar çocuk objede bulundu.");
            return;
        }

        // Sadece EnemyUI tag'ine sahip health bar'ı bulma
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

    // Silah bu fonksiyonu çağıracak
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " hasar aldı! Kalan can: " + currentHealth);

        // Health Bar'ı güncelleyin
        if (healthBarUI != null)
        {
            // Doğrudan sağlık barının takeDamage metodunu çağıralım
            healthBarUI.takeDamage(damage);
        }
        else
        {
            Debug.LogError(gameObject.name + " için Health Bar bulunamadı, hasar gösterimi yapılamadı!");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " öldü!");
        Destroy(gameObject);
    }
}