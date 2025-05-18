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

    // Farkl� renkler i�in
    public Color playerHealthColor = Color.green;
    public Color enemyHealthColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        // Slider referanslar�n� otomatik olarak bulal�m (e�er atanmam��sa)
        if (healthSlider == null)
            healthSlider = GetComponent<Slider>();

        if (easeHealthSlider == null && transform.childCount > 0)
            easeHealthSlider = transform.GetChild(0).GetComponent<Slider>();

        health = maxHealth;

        // Slider'lar�n max de�erlerini ayarlayal�m
        if (healthSlider != null)
            healthSlider.maxValue = maxHealth;
        else
            Debug.LogError("Health Slider bulunamad�!");

        if (easeHealthSlider != null)
            easeHealthSlider.maxValue = maxHealth;

        // Renk ayarla
        UpdateHealthBarColor();

        // Debug i�in owner tipini log'a yazd�r
        Debug.Log(gameObject.name + " health bar ba�lat�ld� - Owner tipi: " + ownerType.ToString());
    }

    void UpdateHealthBarColor()
    {
        if (healthSlider != null && healthSlider.fillRect != null)
        {
            Image fillImage = healthSlider.fillRect.GetComponent<Image>();
            if (fillImage != null)
            {
                // Owner tipine g�re renk ayarla
                fillImage.color = (ownerType == OwnerType.Player) ? playerHealthColor : enemyHealthColor;
                Debug.Log(gameObject.name + " rengi ayarland�: " + ((ownerType == OwnerType.Player) ? "Ye�il (Player)" : "K�rm�z� (Enemy)"));
            }
            else
            {
                Debug.LogError("Fill Image bulunamad�!");
            }
        }
        else
        {
            Debug.LogError("Health Slider veya Fill Rect bulunamad�!");
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
            // Yumu�ak ge�i� i�in Lerp kullan�yoruz
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
        }
    }

    // Public metod ekleyelim ki d��ar�dan �a�r�labilsin
    public void takeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth); // Sa�l�k de�erini 0 ile maxHealth aras�nda tut

        // Owner tipine g�re farkl� davran��
        if (ownerType == OwnerType.Player)
        {
            // Ekrana flash effect eklenebilir
            // Oyuncu hasar al�nca ses �alabilir
            Debug.Log("Player took damage: " + damage + ", Kalan sa�l�k: " + health);
        }
        else // Enemy
        {
            // D��man hasar al�nca farkl� bir efekt
            Debug.Log("Enemy took damage: " + damage + ", Kalan sa�l�k: " + health);
        }

        // Slider de�erlerini hemen g�ncelle
        if (healthSlider != null)
            healthSlider.value = health;
    }

    // Sa�l�k de�erini s�f�rlama metodu
    public void ResetHealth()
    {
        health = maxHealth;

        // Slider de�erlerini de s�f�rla
        if (healthSlider != null)
            healthSlider.value = maxHealth;

        if (easeHealthSlider != null)
            easeHealthSlider.value = maxHealth;

        Debug.Log(gameObject.name + " sa�l��� s�f�rland�.");
    }

    // Sa�l�k y�zdesini d�nd�ren yard�mc� metod
    public float GetHealthPercent()
    {
        return health / maxHealth;
    }
}