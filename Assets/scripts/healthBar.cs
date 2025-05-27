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

    void Awake() // Start yerine Awake kullanmak, di�er scriptlerin Start'�ndan �nce �al��mas�n� sa�layabilir.
    {
        // Slider referanslar�n� otomatik olarak bulal�m (e�er atanmam��sa)
        if (healthSlider == null)
            healthSlider = GetComponent<Slider>();

        // easeHealthSlider genellikle healthSlider'�n bir child'� olur.
        if (easeHealthSlider == null && transform.childCount > 0)
        {
            // Do�rudan ilk child'� almak yerine, Slider bile�enine sahip bir child arayabiliriz.
            // Veya Inspector'dan atanmas� daha g�venlidir. �imdilik basit tutal�m:
            Slider[] childSliders = GetComponentsInChildren<Slider>();
            foreach (Slider s in childSliders)
            {
                if (s != healthSlider) // Kendisi olmayan ilk slider'� ease olarak kabul et
                {
                    easeHealthSlider = s;
                    break;
                }
            }
        }


        // health = maxHealth; // Bu sat�r PlayerHealth veya EnemyHealth taraf�ndan ayarlanmal�.
        // healthBar sadece g�rselle�tirmeden sorumlu olmal�.

        // Slider'lar�n max de�erlerini ayarlayal�m
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health; // Ba�lang�� de�erini health'e e�itle
        }
        else
        {
            Debug.LogError(gameObject.name + ": Health Slider bulunamad�!");
        }

        if (easeHealthSlider != null)
        {
            easeHealthSlider.maxValue = maxHealth;
            easeHealthSlider.value = health; // Ba�lang�� de�erini health'e e�itle
        }

        // Renk ayarla
        UpdateHealthBarColor();

        Debug.Log(gameObject.name + " health bar ba�lat�ld� - Owner tipi: " + ownerType.ToString() + ", MaxHealth: " + maxHealth + ", CurrentHealth: " + health);
    }

    // DE����KL�K: Bu metodu public yapt�k
    public void UpdateHealthBarColor()
    {
        if (healthSlider != null && healthSlider.fillRect != null)
        {
            Image fillImage = healthSlider.fillRect.GetComponent<Image>();
            if (fillImage != null)
            {
                fillImage.color = (ownerType == OwnerType.Player) ? playerHealthColor : enemyHealthColor;
                // Debug.Log(gameObject.name + " rengi ayarland�: " + ((ownerType == OwnerType.Player) ? "Ye�il (Player)" : "K�rm�z� (Enemy)"));
            }
            else
            {
                Debug.LogError(gameObject.name + ": Fill Image bulunamad�!");
            }
        }
        else
        {
            // Debug.LogError(gameObject.name + ": Health Slider ("+ (healthSlider != null) +") veya Fill Rect ("+ (healthSlider?.fillRect != null) +") bulunamad�!");
        }
    }

    void Update()
    {
        // health de�eri d��ar�dan (PlayerHealth, EnemyHealth) y�netildi�i i�in
        // healthSlider.value = health; sat�r� takeDamage veya ResetHealth gibi metodlarda g�ncellenmeli.
        // Update'te s�rekli e�itlemek yerine, de�i�iklik oldu�unda e�itlemek daha performansl� olabilir.
        // Ancak yumu�ak ge�i� i�in easeHealthSlider'�n Update'te kalmas� mant�kl�.

        if (healthSlider != null && healthSlider.value != health) // Sadece de�i�iklik varsa g�ncelle
        {
            healthSlider.value = health;
        }

        if (easeHealthSlider != null)
        {
            if (easeHealthSlider.value != healthSlider.value) // Sadece fark varsa lerp yap
            {
                easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed * Time.deltaTime * 60); // Frame rate ba��ms�z lerp
            }
        }
    }

    public void takeDamage(int damage) // Hasar miktar� int ise parametre int olmal�
    {
        float oldHealth = health;
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        // Debug.Log(gameObject.name + " health bar damage: " + damage +
        //           ", Old: " + oldHealth + ", New: " + health);

        if (healthSlider != null)
        {
            healthSlider.value = health;
            // Debug.Log("Health slider de�eri g�ncellendi: " + health);
        }
        // easeHealthSlider Update'te zaten yumu�ak ge�i� yapacak
    }

    public void ResetHealth()
    {
        health = maxHealth;

        if (healthSlider != null)
            healthSlider.value = maxHealth;

        if (easeHealthSlider != null)
            easeHealthSlider.value = maxHealth; // Ease slider'� da hemen max yapabiliriz

        UpdateHealthBarColor(); // Tam can oldu�unda rengi tekrar kontrol et/ayarla
        // Debug.Log(gameObject.name + " sa�l��� s�f�rland�.");
    }

    public float GetHealthPercent()
    {
        if (maxHealth == 0) return 0; // S�f�ra b�lme hatas�n� engelle
        return health / maxHealth;
    }
}