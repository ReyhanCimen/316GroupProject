using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    // Health Bar referansý
    public healthBar healthBarUI;

    void Start()
    {
        currentHealth = maxHealth;

        // Health Bar referansýný bul - Daha kapsamlý bir arama yapalým
        FindHealthBar();

        // Debug için bir kontrol ekleyelim
        if (healthBarUI == null)
        {
            Debug.LogError("PlayerHealth: Health Bar referansý bulunamadý! Player UI oluþturduðunuzdan emin olun.");
        }
        else
        {
            // Health Bar'ý baþlangýç deðerine ayarla
            healthBarUI.ownerType = healthBar.OwnerType.Player;
            healthBarUI.maxHealth = maxHealth;
            healthBarUI.health = maxHealth;

            // Slider deðerlerini manuel olarak ayarlayalým
            if (healthBarUI.healthSlider != null)
            {
                healthBarUI.healthSlider.maxValue = maxHealth;
                healthBarUI.healthSlider.value = maxHealth;
            }

            if (healthBarUI.easeHealthSlider != null)
            {
                healthBarUI.easeHealthSlider.maxValue = maxHealth;
                healthBarUI.easeHealthSlider.value = maxHealth;
            }

            Debug.Log("Player Health Bar baþarýyla baþlatýldý.");
        }
    }

    void FindHealthBar()
    {
        // 1. Önce PlayerUI tag'ine sahip olanlar arasýnda ara
        GameObject[] bars = GameObject.FindGameObjectsWithTag("PlayerUI");
        foreach (GameObject bar in bars)
        {
            healthBar hb = bar.GetComponentInChildren<healthBar>(true); // inactive objeleri de içerecek þekilde
            if (hb != null)
            {
                healthBarUI = hb;
                Debug.Log("Player Health Bar 'PlayerUI' tag ile bulundu!");
                return; // Bulunca çýk
            }
        }

        // 2. Canvas içinde ara
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas c in canvases)
        {
            healthBar hb = c.GetComponentInChildren<healthBar>(true);
            if (hb != null && hb.ownerType == healthBar.OwnerType.Player)
            {
                healthBarUI = hb;
                Debug.Log("Player Health Bar canvas içinde bulundu!");
                return;
            }
        }

        // 3. Tüm sahnede ara
        healthBar[] allHealthBars = FindObjectsOfType<healthBar>();
        foreach (healthBar hb in allHealthBars)
        {
            if (hb.ownerType == healthBar.OwnerType.Player)
            {
                healthBarUI = hb;
                Debug.Log("Player Health Bar tüm sahnede aranarak bulundu!");
                return;
            }
        }

        Debug.LogError("Hiçbir Player Health Bar bulunamadý! Health Bar oluþturduðunuzdan emin olun.");
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("PLAYER DAMAGED! Amount: " + amount + ", Current health: " + currentHealth);

        // Health Bar'ý güncelle
        if (healthBarUI != null)
        {
            healthBarUI.takeDamage(amount);
        }
        else
        {
            Debug.LogError("Player Health Bar bulunamadý, hasar gösterimi yapýlamadý!");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");
        // TODO: Disable controls, show game over screen, etc.
    }
}