using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    // Health Bar referans�
    public healthBar healthBarUI;

    void Start()
    {
        currentHealth = maxHealth;

        // Health Bar referans�n� bul - Daha kapsaml� bir arama yapal�m
        FindHealthBar();

        // Debug i�in bir kontrol ekleyelim
        if (healthBarUI == null)
        {
            Debug.LogError("PlayerHealth: Health Bar referans� bulunamad�! Player UI olu�turdu�unuzdan emin olun.");
        }
        else
        {
            // Health Bar'� ba�lang�� de�erine ayarla
            healthBarUI.ownerType = healthBar.OwnerType.Player;
            healthBarUI.maxHealth = maxHealth;
            healthBarUI.health = maxHealth;

            // Slider de�erlerini manuel olarak ayarlayal�m
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

            Debug.Log("Player Health Bar ba�ar�yla ba�lat�ld�.");
        }
    }

    void FindHealthBar()
    {
        // 1. �nce PlayerUI tag'ine sahip olanlar aras�nda ara
        GameObject[] bars = GameObject.FindGameObjectsWithTag("PlayerUI");
        foreach (GameObject bar in bars)
        {
            healthBar hb = bar.GetComponentInChildren<healthBar>(true); // inactive objeleri de i�erecek �ekilde
            if (hb != null)
            {
                healthBarUI = hb;
                Debug.Log("Player Health Bar 'PlayerUI' tag ile bulundu!");
                return; // Bulunca ��k
            }
        }

        // 2. Canvas i�inde ara
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas c in canvases)
        {
            healthBar hb = c.GetComponentInChildren<healthBar>(true);
            if (hb != null && hb.ownerType == healthBar.OwnerType.Player)
            {
                healthBarUI = hb;
                Debug.Log("Player Health Bar canvas i�inde bulundu!");
                return;
            }
        }

        // 3. T�m sahnede ara
        healthBar[] allHealthBars = FindObjectsOfType<healthBar>();
        foreach (healthBar hb in allHealthBars)
        {
            if (hb.ownerType == healthBar.OwnerType.Player)
            {
                healthBarUI = hb;
                Debug.Log("Player Health Bar t�m sahnede aranarak bulundu!");
                return;
            }
        }

        Debug.LogError("Hi�bir Player Health Bar bulunamad�! Health Bar olu�turdu�unuzdan emin olun.");
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("PLAYER DAMAGED! Amount: " + amount + ", Current health: " + currentHealth);

        // Health Bar'� g�ncelle
        if (healthBarUI != null)
        {
            healthBarUI.takeDamage(amount);
        }
        else
        {
            Debug.LogError("Player Health Bar bulunamad�, hasar g�sterimi yap�lamad�!");
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