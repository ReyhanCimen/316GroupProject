using UnityEngine;

public class BoosterManager : MonoBehaviour
{
    [Header("Player References")]
    public PlayerHealth playerHealth; // PlayerHealth scriptine referans
    public Gun gun;                 // Gun scriptine referans

    [Header("Ammo Boost Settings")]
    public float fireRateDecreaseMin = 0.02f;   // Ate� h�z�ndaki minimum azalma (daha h�zl� ate�)
    public float fireRateDecreaseMax = 0.05f;   // Ate� h�z�ndaki maksimum azalma
    public int magazineIncreaseMin = 3;         // �arj�r kapasitesindeki minimum art��
    public int magazineIncreaseMax = 7;         // �arj�r kapasitesindeki maksimum art��
    public int totalAmmoIncreaseMin = 15;       // Toplam mermi say�s�ndaki minimum art��
    public int totalAmmoIncreaseMax = 30;       // Toplam mermi say�s�ndaki maksimum art��

    void Start()
    {
        // Referanslar� otomatik olarak almaya �al��al�m (e�er atanmam��sa)
        if (playerHealth == null)
        {
            // BoosterManager genellikle Player objesine eklenir.
            playerHealth = GetComponent<PlayerHealth>();
            if (playerHealth == null)
            {
                Debug.LogError("BoosterManager: PlayerHealth script'i bu obje �zerinde bulunamad�!");
            }
        }

        if (gun == null)
        {
            // Gun scripti genellikle Player objesinin bir child'�nda (silah objesi) olur.
            // Veya Player objesinin kendisinde de olabilir.
            gun = GetComponentInChildren<Gun>();
            if (gun == null)
            {
                gun = GetComponent<Gun>();
            }
            if (gun == null)
            {
                // E�er Player'�n kendisinde veya child'�nda de�ilse, sahnede bulmay� deneyebiliriz.
                // Ancak bu genellikle iyi bir pratik de�ildir, do�rudan referans daha iyidir.
                GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
                if (playerObject != null)
                {
                    gun = playerObject.GetComponentInChildren<Gun>();
                    if (gun == null) gun = playerObject.GetComponent<Gun>();
                }

                if (gun == null)
                {
                    Debug.LogError("BoosterManager: Gun script'i Player objesinde veya child'lar�nda bulunamad�!");
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HealthBoost"))
        {
            ApplyHealthBoost(other.gameObject);
        }
        else if (other.CompareTag("AmmoBoost"))
        {
            ApplyAmmoBoost(other.gameObject);
        }
    }

    void ApplyHealthBoost(GameObject boosterObject)
    {
        if (playerHealth != null)
        {
            playerHealth.HealToFull(); // PlayerHealth'teki yeni metodu �a��r
            Debug.Log("Health Boost al�nd�! Can tamamen dolduruldu.");
            Destroy(boosterObject); // Boost objesini yok et
        }
        else
        {
            Debug.LogWarning("Health Boost al�nd� ama PlayerHealth referans� yok!");
        }
    }

    void ApplyAmmoBoost(GameObject boosterObject)
    {
        if (gun != null)
        {
            int boostType = Random.Range(0, 3); // 0: Fire Rate, 1: Magazine Size, 2: Total Ammo

            switch (boostType)
            {
                case 0: // Fire Rate Art��� (fireRate de�erini d���rerek)
                    float fireRateDecrease = Random.Range(fireRateDecreaseMin, fireRateDecreaseMax);
                    gun.fireRate = Mathf.Max(0.05f, gun.fireRate - fireRateDecrease); // Minimum 0.05f olsun
                    Debug.Log($"Ammo Boost: Ate� h�z� artt�! Yeni fire rate: {gun.fireRate}");
                    break;
                case 1: // �arj�r Kapasitesi Art���
                    int magIncrease = Random.Range(magazineIncreaseMin, magazineIncreaseMax);
                    gun.magazineSize += magIncrease;
                    // �ste�e ba�l�: Mevcut �arj�re de mermi eklenebilir
                    // gun.AddAmmoToMagazine(magIncrease); // E�er �arj�re de eklemek istersen
                    Debug.Log($"Ammo Boost: �arj�r kapasitesi artt�! Yeni kapasite: {gun.magazineSize}");
                    break;
                case 2: // Toplam Mermi Art���
                    int ammoIncrease = Random.Range(totalAmmoIncreaseMin, totalAmmoIncreaseMax);
                    gun.totalAmmo += ammoIncrease;
                    Debug.Log($"Ammo Boost: Toplam mermi artt�! Yeni toplam mermi: {gun.totalAmmo}");
                    break;
            }

            // Mermi UI'�n� g�ncellemek i�in Gun scriptindeki metodu �a��r
            gun.NotifyAmmoStatsChanged();

            Destroy(boosterObject); // Boost objesini yok et
        }
        else
        {
            Debug.LogWarning("Ammo Boost al�nd� ama Gun referans� yok!");
        }
    }
}