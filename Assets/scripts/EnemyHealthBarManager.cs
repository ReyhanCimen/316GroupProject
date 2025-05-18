using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarManager : MonoBehaviour
{
    public GameObject healthBarPrefab;  // Enemy sa�l�k �ubu�u prefab'�
    public Vector3 offset = new Vector3(0, 1.5f, 0);  // D��man�n tepesinde konumland�rmak i�in offset

    private Canvas healthBarCanvas;
    private GameObject healthBarObj;
    private Enemy enemyScript;

    void Start()
    {
        // D��man�n Enemy script'ini al
        enemyScript = GetComponent<Enemy>();

        if (enemyScript == null)
        {
            Debug.LogError("Enemy script not found on " + gameObject.name);
            return;
        }

        // Health Bar prefab'�n� kontrol et
        if (healthBarPrefab == null)
        {
            Debug.LogError("Health Bar prefab atanmam��!");
            return;
        }

        // Health Bar prefab'�n� instantiate et
        healthBarObj = Instantiate(healthBarPrefab, transform);
        healthBarObj.transform.localPosition = offset;

        // Tag ata
        healthBarObj.tag = "EnemyUI";

        // Canvas'� World Space olarak ayarla
        healthBarCanvas = healthBarObj.GetComponent<Canvas>();
        if (healthBarCanvas != null)
        {
            healthBarCanvas.renderMode = RenderMode.WorldSpace;
            healthBarCanvas.worldCamera = Camera.main;

            // Scale de�erini ayarla
            healthBarObj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }

        // Health Bar bile�enlerini bul
        healthBar healthBarScript = healthBarObj.GetComponentInChildren<healthBar>();
        if (healthBarScript != null)
        {
            // Owner tipini d��man olarak ayarla
            healthBarScript.ownerType = healthBar.OwnerType.Enemy;

            // Health Bar de�erlerini ayarla
            healthBarScript.maxHealth = enemyScript.maxHealth;
            healthBarScript.health = enemyScript.maxHealth;

            // Slider de�erlerini manuel olarak ba�lang��ta ayarla
            if (healthBarScript.healthSlider != null)
            {
                healthBarScript.healthSlider.maxValue = enemyScript.maxHealth;
                healthBarScript.healthSlider.value = enemyScript.maxHealth;
            }

            if (healthBarScript.easeHealthSlider != null)
            {
                healthBarScript.easeHealthSlider.maxValue = enemyScript.maxHealth;
                healthBarScript.easeHealthSlider.value = enemyScript.maxHealth;
            }

            // Enemy scriptine Health Bar referans�n� ata
            enemyScript.healthBarUI = healthBarScript;

            Debug.Log("Health Bar initialized for " + gameObject.name + " with max health: " + enemyScript.maxHealth);
        }
        else
        {
            Debug.LogError("Health Bar script bulunamad�!");
        }

        // Billboard script'ini ekle
        if (!healthBarObj.GetComponent<BillboardHealthBar>())
        {
            healthBarObj.AddComponent<BillboardHealthBar>();
        }
    }
}