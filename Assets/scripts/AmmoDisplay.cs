using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoDisplay : MonoBehaviour
{
    public TMP_Text ammoText; // Normal Text yerine TMP_Text kullanýn

    [Header("Gun Reference")]
    public Gun gunScript; // Silah script'i referansý

    private void Start()
    {
        // Eðer referans atanmamýþsa, kendini referans olarak ata
        if (ammoText == null)
        {
            ammoText = GetComponent<TMP_Text>();
            Debug.Log("AmmoText auto-assigned: " + (ammoText != null));
        }

        // Gun script'ini bul
        if (gunScript == null)
        {
            gunScript = FindObjectOfType<Gun>();
            Debug.Log("Gun script found: " + (gunScript != null));
        }

        // Baþlangýçta mermi sayýsýný güncelle
        UpdateAmmoText();
    }

    private void Update()
    {
        // Her frame mermi sayýsýný güncelle
        UpdateAmmoText();
    }

    void UpdateAmmoText()
    {
        if (gunScript != null && ammoText != null)
        {
            ammoText.text = gunScript.currentAmmo + "/" + gunScript.magazineSize;
            Debug.Log("Ammo text updated: " + ammoText.text);
        }
    }


    private void OnDestroy()
    {
        // Event aboneliðini kaldýr (hafýza sýzýntýsýný önlemek için)
        if (gunScript != null)
        {
            gunScript.onAmmoChanged -= UpdateAmmoUI;
        }
    }

    // Event handler metodu
    void UpdateAmmoUI(int currentAmmo, int totalAmmo)
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo + "/" + totalAmmo;
        }
    }
}