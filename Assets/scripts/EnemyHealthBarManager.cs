using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarManager : MonoBehaviour
{
    public GameObject healthBarPrefab;
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    private Canvas healthBarCanvas;
    private GameObject healthBarObj;
    private Enemy enemyScript;

    void Start()
    {
        enemyScript = GetComponent<Enemy>();

        if (enemyScript == null)
        {
            Debug.LogError($"Enemy script not found on {gameObject.name}");
            return;
        }

        if (healthBarPrefab == null)
        {
            Debug.LogError("Health Bar prefab atanmam��!");
            return;
        }

        // Health Bar olu�turmay� geciktir (Enemy Start()'�ndan sonra)
        Invoke("CreateHealthBar", 0.05f);
    }

    void CreateHealthBar()
    {
        // Health Bar prefab'�n� instantiate et
        healthBarObj = Instantiate(healthBarPrefab, transform);
        healthBarObj.transform.localPosition = offset;
        healthBarObj.tag = "EnemyUI";
        healthBarObj.name = $"{gameObject.name}_HealthBar";

        // Canvas ayarlar� - PERFORMANS �NEML�
        healthBarCanvas = healthBarObj.GetComponent<Canvas>();
        if (healthBarCanvas != null)
        {
            healthBarCanvas.renderMode = RenderMode.WorldSpace;
            healthBarCanvas.worldCamera = Camera.main;
            
            // Performans optimizasyonlar�
            healthBarCanvas.sortingOrder = 10;
            healthBarCanvas.pixelPerfect = false; // Performans i�in kapal�
            
            // Scale ayarlar�
            healthBarObj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            
            Debug.Log($"{gameObject.name} - Canvas ayarlar� tamamland�");
        }

        // Health Bar bile�enlerini bul ve ayarla
        healthBar healthBarScript = healthBarObj.GetComponentInChildren<healthBar>();
        if (healthBarScript != null)
        {
            // Owner tipini d��man olarak ayarla
            healthBarScript.ownerType = healthBar.OwnerType.Enemy;

            // Health Bar de�erlerini ayarla
            healthBarScript.maxHealth = enemyScript.maxHealth;
            healthBarScript.health = enemyScript.maxHealth;

            // Slider de�erlerini ANINDA ayarla
            if (healthBarScript.healthSlider != null)
            {
                healthBarScript.healthSlider.maxValue = enemyScript.maxHealth;
                healthBarScript.healthSlider.value = enemyScript.maxHealth;
                
                // Slider ayarlar�n� optimize et
                healthBarScript.healthSlider.interactable = false; // Performans i�in
                Debug.Log($"{gameObject.name} - HealthSlider ayarland�: {enemyScript.maxHealth}");
            }

            if (healthBarScript.easeHealthSlider != null)
            {
                healthBarScript.easeHealthSlider.maxValue = enemyScript.maxHealth;
                healthBarScript.easeHealthSlider.value = enemyScript.maxHealth;
                
                // Ease slider ayarlar�n� optimize et
                healthBarScript.easeHealthSlider.interactable = false; // Performans i�in
                Debug.Log($"{gameObject.name} - EaseHealthSlider ayarland�: {enemyScript.maxHealth}");
            }

            // Enemy scriptine Health Bar referans�n� ata
            enemyScript.healthBarUI = healthBarScript;

            Debug.Log($"Health Bar initialized for {gameObject.name} with max health: {enemyScript.maxHealth}");
        }
        else
        {
            Debug.LogError("Health Bar script bulunamad�!");
        }

        // Billboard script'ini ekle
        BillboardHealthBar billboard = healthBarObj.GetComponent<BillboardHealthBar>();
        if (billboard == null)
        {
            billboard = healthBarObj.AddComponent<BillboardHealthBar>();
        }
    }

    void Update()
    {
        // Health bar pozisyonunu s�rekli g�ncelle (Billboard script'i de var ama emin olmak i�in)
        if (healthBarObj != null)
        {
            healthBarObj.transform.position = transform.position + offset;
        }
    }
}