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

        // Health Bar referans�n� bul
        FindHealthBar();

        // Health Bar'� ba�lang�� de�erine ayarla
        if (healthBarUI != null)
        {
            healthBarUI.ownerType = healthBar.OwnerType.Player;
            healthBarUI.maxHealth = maxHealth;
            healthBarUI.health = maxHealth;
        }
        else
        {
            Debug.LogError("Player Health Bar bulunamad�!");
        }
    }

    void FindHealthBar()
    {
        // Sadece PlayerUI tag'ine sahip health bar'� bulma
        GameObject[] bars = GameObject.FindGameObjectsWithTag("PlayerUI");
        foreach (GameObject bar in bars)
        {
            healthBar hb = bar.GetComponentInChildren<healthBar>();
            if (hb != null && hb.ownerType == healthBar.OwnerType.Player)
            {
                healthBarUI = hb;
                Debug.Log("Player Health Bar bulundu!");
                break;
            }
        }

        // E�er tag ile bulunamad�ysa, genel arama yap
        if (healthBarUI == null)
        {
            healthBarUI = FindObjectOfType<healthBar>();
            if (healthBarUI != null)
            {
                healthBarUI.ownerType = healthBar.OwnerType.Player;
                Debug.Log("Player Health Bar genel arama ile bulundu!");
            }
        }
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