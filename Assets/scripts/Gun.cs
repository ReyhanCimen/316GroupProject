using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    public int magazineSize = 10;
    public int totalAmmo = 50;
    public float fireRate = 0.2f;
    public float range = 100f;

    [Header("References")]
    public Transform muzzlePoint; // Ucu
    public Camera playerCamera;

    // Mermi değişikliği için event
    public delegate void AmmoChangedHandler(int currentAmmo, int totalAmmo);
    public event AmmoChangedHandler onAmmoChanged;

    // Public değişkene çevirdik (UI erişimi için)
    public int currentAmmo { get; private set; }
    private float nextTimeToFire = 0f;

    void Start()
    {
        currentAmmo = magazineSize;

        // UI güncellemesi için event'i tetikle
        if (onAmmoChanged != null)
            onAmmoChanged(currentAmmo, totalAmmo);
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            if (currentAmmo > 0)
            {
                nextTimeToFire = Time.time + fireRate;
                Shoot();
            }
            else
            {
                Debug.Log("Şarjör boş! R ile doldur.");
                // Silah boş sesini çalabilirsiniz
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    void Shoot()
    {
        currentAmmo--;
        Debug.Log("Ateş! Kalan mermi: " + currentAmmo);

        // Event'i tetikle (UI güncellemesi için)
        if (onAmmoChanged != null)
            onAmmoChanged(currentAmmo, totalAmmo);

        // Kameradan ileri doğru ışın gönder (nişangah ile aynı yöne)
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log("Vurulan nesne: " + hit.collider.name);

            if (hit.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(25); // Her atışta 25 hasar
                }
            }
        }
    }

    void Reload()
    {
        if (currentAmmo == magazineSize || totalAmmo <= 0)
        {
            Debug.Log("Dolu ya da mermi yok.");
            return;
        }

        int neededAmmo = magazineSize - currentAmmo;
        int ammoToReload = Mathf.Min(neededAmmo, totalAmmo);

        totalAmmo -= ammoToReload;
        currentAmmo += ammoToReload;

        Debug.Log("Şarjör değiştirildi. Kalan toplam mermi: " + totalAmmo);

        // Event'i tetikle (UI güncellemesi için)
        if (onAmmoChanged != null)
            onAmmoChanged(currentAmmo, totalAmmo);
    }
}