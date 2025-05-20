using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBarManager : MonoBehaviour
{
    public GameObject healthBarPrefab;  // Player saðlýk çubuðu prefab'ý
    public Transform uiContainer;      // Canvas referansý

    private GameObject healthBarObj;
    private PlayerHealth playerHealthScript;

    void Start()
    {
        // Player health script'ini al
        playerHealthScript = GetComponent<PlayerHealth>();

        if (playerHealthScript == null)
        {
            Debug.LogError("PlayerHealth script bulunamadý!");
            return;
        }

        // Health Bar prefab'ýný kontrol et
        if (healthBarPrefab == null)
        {
            Debug.LogError("Health Bar prefab atanmamýþ!");
            return;
        }

        // UI Container yoksa, ana Canvas'ý bul
        if (uiContainer == null)
        {
            Canvas mainCanvas = FindObjectOfType<Canvas>();
            if (mainCanvas != null)
            {
                uiContainer = mainCanvas.transform;
            }
            else
            {
                Debug.LogError("UI Container veya Canvas bulunamadý!");
                return;
            }
        }

        // Health Bar prefab'ýný instantiate et
        healthBarObj = Instantiate(healthBarPrefab, uiContainer);
        healthBarObj.tag = "PlayerUI";

        // Health Bar bileþenlerini bul
        healthBar healthBarScript = healthBarObj.GetComponent<healthBar>();
        if (healthBarScript != null)
        {
            // Owner tipini oyuncu olarak ayarla
            healthBarScript.ownerType = healthBar.OwnerType.Player;

            // Health Bar deðerlerini ayarla
            healthBarScript.maxHealth = playerHealthScript.maxHealth;
            healthBarScript.health = playerHealthScript.maxHealth;

            // Slider deðerlerini manuel olarak baþlangýçta ayarla
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

            // PlayerHealth scriptine Health Bar referansýný ata
            playerHealthScript.healthBarUI = healthBarScript;

            Debug.Log("Player Health Bar baþarýyla oluþturuldu ve ayarlandý!");
        }
        else
        {
            Debug.LogError("Health Bar script bulunamadý!");
        }
    }
}
