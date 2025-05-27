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
        FindHealthBar(); // Health bar'� bul

        if (healthBarUI != null)
        {
            InitializeHealthBar();
            Debug.Log("Player Health Bar ba�ar�yla ba�lat�ld�.");
        }
        else
        {
            Debug.LogError("PlayerHealth: Start i�inde Health Bar referans� bulunamad�!");
        }
    }

    void FindHealthBar()
    {
        // Sahnede "PlayerUI" tag'ine sahip healthBar'� bulmaya �al��
        GameObject[] bars = GameObject.FindGameObjectsWithTag("PlayerUI");
        foreach (GameObject barGO in bars)
        {
            healthBar hb = barGO.GetComponentInChildren<healthBar>(true); // inactive objeleri de i�erecek �ekilde
            if (hb != null && hb.ownerType == healthBar.OwnerType.Player) // Sahip tipini de kontrol et
            {
                healthBarUI = hb;
                Debug.Log("Player Health Bar 'PlayerUI' tag ve ownerType ile bulundu!");
                return;
            }
        }

        // E�er tag ile bulunamazsa, Canvas i�inde ara
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas c in canvases)
        {
            healthBar hb = c.GetComponentInChildren<healthBar>(true);
            if (hb != null && hb.ownerType == healthBar.OwnerType.Player)
            {
                healthBarUI = hb;
                Debug.Log("Player Health Bar canvas i�inde ve ownerType ile bulundu!");
                return;
            }
        }

        // Hala bulunamad�ysa, t�m sahnede ownerType'a g�re ara
        healthBar[] allHealthBars = FindObjectsOfType<healthBar>(true);
        foreach (healthBar hb in allHealthBars)
        {
            if (hb.ownerType == healthBar.OwnerType.Player)
            {
                healthBarUI = hb;
                Debug.Log("Player Health Bar t�m sahnede ownerType ile bulundu!");
                return;
            }
        }
        // Bu noktaya gelinirse, healthBarUI hala null olabilir.
    }

    void InitializeHealthBar()
    {
        if (healthBarUI != null)
        {
            healthBarUI.ownerType = healthBar.OwnerType.Player; // Tekrar emin olal�m
            healthBarUI.maxHealth = maxHealth;
            healthBarUI.health = currentHealth; // Ba�lang��ta currentHealth neyse o

            // Slider de�erlerini manuel olarak ayarlayal�m
            if (healthBarUI.healthSlider != null)
            {
                healthBarUI.healthSlider.maxValue = maxHealth;
                healthBarUI.healthSlider.value = currentHealth;
            }

            if (healthBarUI.easeHealthSlider != null)
            {
                healthBarUI.easeHealthSlider.maxValue = maxHealth;
                healthBarUI.easeHealthSlider.value = currentHealth;
            }
            healthBarUI.UpdateHealthBarColor(); // Rengi de g�ncelleyelim
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Can�n 0'�n alt�na d��mesini veya maxHealth'i a�mas�n� engelle
        Debug.Log("PLAYER DAMAGED! Amount: " + amount + ", Current health: " + currentHealth);

        if (healthBarUI != null)
        {
            healthBarUI.takeDamage(amount); // healthBarUI.health = currentHealth; de olabilir veya direkt takeDamage
        }
        else
        {
            // Health bar bulunamad�ysa bile FindHealthBar'� tekrar �a��rmay� deneyebiliriz.
            FindHealthBar();
            if (healthBarUI != null)
            {
                InitializeHealthBar(); // E�er yeni bulunduysa ba�lat
                healthBarUI.takeDamage(amount);
            }
            else
            {
                Debug.LogError("Player Health Bar bulunamad�, hasar g�sterimi yap�lamad�!");
            }
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // YEN� METOD: Can� tamamen doldurur ve UI'� g�nceller
    public void HealToFull()
    {
        currentHealth = maxHealth;
        Debug.Log("PLAYER HEALED TO FULL! Current health: " + currentHealth);

        if (healthBarUI != null)
        {
            healthBarUI.ResetHealth(); // healthBar scriptindeki ResetHealth'i �a��r�yoruz
            Debug.Log("Player Health Bar s�f�rland� ve tam cana ayarland�.");
        }
        else
        {
            // Health bar bulunamad�ysa bile FindHealthBar'� tekrar �a��rmay� deneyebiliriz.
            FindHealthBar();
            if (healthBarUI != null)
            {
                InitializeHealthBar(); // E�er yeni bulunduysa ba�lat
                healthBarUI.ResetHealth();
            }
            else
            {
                Debug.LogError("Player Health Bar bulunamad�, tam can g�sterimi yap�lamad�!");
            }
        }
    }

    void Die()
    {
        Debug.Log("Player died!");
        // TODO: Disable controls, show game over screen, etc.
    }
}