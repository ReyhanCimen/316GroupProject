using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    public enum OwnerType { Player, Enemy }
    public OwnerType ownerType = OwnerType.Player;

    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHealth = 100f;
    public float health;
    private float lerpSpeed = 0.05f;

    // Farklý renkler için
    public Color playerHealthColor = Color.green;
    public Color enemyHealthColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        // Slider referanslarýný otomatik olarak bulalým (eðer atanmamýþsa)
        if (healthSlider == null)
            healthSlider = GetComponent<Slider>();

        if (easeHealthSlider == null && transform.childCount > 0)
            easeHealthSlider = transform.GetChild(0).GetComponent<Slider>();

        health = maxHealth;

        // Slider'larýn max deðerlerini ayarlayalým
        if (healthSlider != null)
            healthSlider.maxValue = maxHealth;
        else
            Debug.LogError("Health Slider bulunamadý!");

        if (easeHealthSlider != null)
            easeHealthSlider.maxValue = maxHealth;

        // Renk ayarla
        UpdateHealthBarColor();

        // Debug için owner tipini log'a yazdýr
        Debug.Log(gameObject.name + " health bar baþlatýldý - Owner tipi: " + ownerType.ToString());
    }

    void UpdateHealthBarColor()
    {
        if (healthSlider != null && healthSlider.fillRect != null)
        {
            Image fillImage = healthSlider.fillRect.GetComponent<Image>();
            if (fillImage != null)
            {
                // Owner tipine göre renk ayarla
                fillImage.color = (ownerType == OwnerType.Player) ? playerHealthColor : enemyHealthColor;
                Debug.Log(gameObject.name + " rengi ayarlandý: " + ((ownerType == OwnerType.Player) ? "Yeþil (Player)" : "Kýrmýzý (Enemy)"));
            }
            else
            {
                Debug.LogError("Fill Image bulunamadý!");
            }
        }
        else
        {
            Debug.LogError("Health Slider veya Fill Rect bulunamadý!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (healthSlider != null)
        {
            healthSlider.value = health;
        }

        if (easeHealthSlider != null)
        {
            // Yumuþak geçiþ için Lerp kullanýyoruz
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
        }
    }

    // Public metod ekleyelim ki dýþarýdan çaðrýlabilsin
    public void takeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth); // Saðlýk deðerini 0 ile maxHealth arasýnda tut

        // Owner tipine göre farklý davranýþ
        if (ownerType == OwnerType.Player)
        {
            // Ekrana flash effect eklenebilir
            // Oyuncu hasar alýnca ses çalabilir
            Debug.Log("Player took damage: " + damage + ", Kalan saðlýk: " + health);
        }
        else // Enemy
        {
            // Düþman hasar alýnca farklý bir efekt
            Debug.Log("Enemy took damage: " + damage + ", Kalan saðlýk: " + health);
        }

        // Slider deðerlerini hemen güncelle
        if (healthSlider != null)
            healthSlider.value = health;
    }

    // Saðlýk deðerini sýfýrlama metodu
    public void ResetHealth()
    {
        health = maxHealth;

        // Slider deðerlerini de sýfýrla
        if (healthSlider != null)
            healthSlider.value = maxHealth;

        if (easeHealthSlider != null)
            easeHealthSlider.value = maxHealth;

        Debug.Log(gameObject.name + " saðlýðý sýfýrlandý.");
    }

    // Saðlýk yüzdesini döndüren yardýmcý metod
    public float GetHealthPercent()
    {
        return health / maxHealth;
    }
}