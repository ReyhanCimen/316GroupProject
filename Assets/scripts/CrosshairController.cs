using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    public Image crosshairImage;
    public Color normalColor = Color.white;
    public Color enemyTargetColor = Color.red;
    public float maxRange = 100f;

    private Camera playerCamera;

    void Start()
    {
        // Kameray� referans olarak alal�m
        if (Camera.main != null)
        {
            playerCamera = Camera.main;
        }
        else
        {
            Debug.LogError("Ana kamera bulunamad�!");
        }

        // E�er crosshairImage atanmam��sa otomatik olarak bulal�m
        if (crosshairImage == null)
        {
            crosshairImage = GetComponent<Image>();
        }
    }

    void Update()
    {
        if (playerCamera == null || crosshairImage == null) return;

        // Kameradan ileri do�ru ���n g�nderelim
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // I��n bir nesneye �arparsa
        if (Physics.Raycast(ray, out hit, maxRange))
        {
            // E�er d��man etiketine sahipse ni�angah�n rengini de�i�tirelim
            if (hit.collider.CompareTag("Enemy"))
            {
                crosshairImage.color = enemyTargetColor;
            }
            else
            {
                crosshairImage.color = normalColor;
            }
        }
        else
        {
            // Hi�bir �eye �arpm�yorsa
            crosshairImage.color = normalColor;
        }
    }
}