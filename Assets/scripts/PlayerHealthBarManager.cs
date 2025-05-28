using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBarManager : MonoBehaviour
{
    public GameObject healthBarPrefab;  // Player sa�l�k �ubu�u prefab'�
    public Transform uiContainer;      // Canvas referans�

    private GameObject healthBarObj;
    private PlayerHealth playerHealthScript;

    void Start()
    {
        // Player health script'ini al
        playerHealthScript = GetComponent<PlayerHealth>();

        if (playerHealthScript == null)
        {
            Debug.LogError("PlayerHealth script bulunamad�!");
            return;
        }

        // Health Bar prefab'�n� kontrol et
        if (healthBarPrefab == null)
        {
            Debug.LogError("Health Bar prefab atanmam��!");
            return;
        }

        // UI Container yoksa, ana Canvas'� bul
        if (uiContainer == null)
        {
            Canvas mainCanvas = FindObjectOfType<Canvas>();
            if (mainCanvas != null)
            {
                uiContainer = mainCanvas.transform;
            }
            else
            {
                Debug.LogError("UI Container veya Canvas bulunamad�!");
                return;
            }
        }

        // Health Bar prefab'�n� instantiate et
        healthBarObj = Instantiate(healthBarPrefab, uiContainer);
        healthBarObj.tag = "PlayerUI";

        // Health Bar bile�enlerini bul
        healthBar healthBarScript = healthBarObj.GetComponent<healthBar>();
        if (healthBarScript != null)
        {
            // Owner tipini oyuncu olarak ayarla
            healthBarScript.ownerType = healthBar.OwnerType.Player;

            // Health Bar de�erlerini ayarla
            healthBarScript.maxHealth = playerHealthScript.maxHealth;
            healthBarScript.health = playerHealthScript.maxHealth;

            // Slider de�erlerini manuel olarak ba�lang��ta ayarla
            if (healthBarScript.healthSlider != null)
            {
                healthBarScript.healthSlider.maxValue = playerHealthScript.maxHealth;
                healthBarScript.healthSlider.value = playerHealthScript.maxHealth;
            }

            if (healthBarScript.easeHealthSlider != null)
            {
                healthBarScript.easeHealthSlider.maxValue = playerHealthScript.maxHealth;
                healthBarScript.easeHealthSlider.value = playerHealthScript.maxHealth;
            }

            // PlayerHealth scriptine Health Bar referans�n� ata
            playerHealthScript.healthBarUI = healthBarScript;

            Debug.Log("Player Health Bar ba�ar�yla olu�turuldu ve ayarland�!");
        }
        else
        {
            Debug.LogError("Health Bar script bulunamad�!");
        }
    }
}
