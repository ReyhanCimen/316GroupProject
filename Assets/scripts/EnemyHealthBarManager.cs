using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarManager : MonoBehaviour
{
    public GameObject healthBarPrefab;  // Enemy saðlýk çubuðu prefab'ý
    public Vector3 offset = new Vector3(0, 1.5f, 0);  // Düþmanýn tepesinde konumlandýrmak için offset

    private Canvas healthBarCanvas;
    private GameObject healthBarObj;
    private Enemy enemyScript;

    void Start()
    {
        // Düþmanýn Enemy script'ini al
        enemyScript = GetComponent<Enemy>();

        if (enemyScript == null)
        {
            Debug.LogError("Enemy script not found on " + gameObject.name);
            return;
        }

        // Health Bar prefab'ýný kontrol et
        if (healthBarPrefab == null)
        {
            Debug.LogError("Health Bar prefab atanmamýþ!");
            return;
        }

        // Health Bar prefab'ýný instantiate et
        healthBarObj = Instantiate(healthBarPrefab, transform);
        healthBarObj.transform.localPosition = offset;

        // Tag ata
        healthBarObj.tag = "EnemyUI";

        // Canvas'ý World Space olarak ayarla
        healthBarCanvas = healthBarObj.GetComponent<Canvas>();
        if (healthBarCanvas != null)
        {
            healthBarCanvas.renderMode = RenderMode.WorldSpace;
            healthBarCanvas.worldCamera = Camera.main;

            // Scale deðerini ayarla
            healthBarObj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }

        // Health Bar bileþenlerini bul
        healthBar healthBarScript = healthBarObj.GetComponentInChildren<healthBar>();
        if (healthBarScript != null)
        {
            // Owner tipini düþman olarak ayarla
            healthBarScript.ownerType = healthBar.OwnerType.Enemy;

            // Health Bar deðerlerini ayarla
            healthBarScript.maxHealth = enemyScript.maxHealth;
            healthBarScript.health = enemyScript.maxHealth;

            // Slider deðerlerini manuel olarak baþlangýçta ayarla
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

            // Enemy scriptine Health Bar referansýný ata
            enemyScript.healthBarUI = healthBarScript;

            Debug.Log("Health Bar initialized for " + gameObject.name + " with max health: " + enemyScript.maxHealth);
        }
        else
        {
            Debug.LogError("Health Bar script bulunamadý!");
        }

        // Billboard script'ini ekle
        if (!healthBarObj.GetComponent<BillboardHealthBar>())
        {
            healthBarObj.AddComponent<BillboardHealthBar>();
        }
    }
}