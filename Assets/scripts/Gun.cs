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

    private int currentAmmo;
    private float nextTimeToFire = 0f;

    void Start()
    {
        currentAmmo = magazineSize;
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
    }
}
