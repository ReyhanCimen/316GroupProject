using UnityEngine;

public class BillboardHealthBar : MonoBehaviour
{
    private Camera mainCamera;
    private Canvas canvas;

    void Start()
    {
        mainCamera = Camera.main;
        canvas = GetComponent<Canvas>();

        // Canvas optimizasyonu
        if (canvas != null)
        {
            canvas.worldCamera = mainCamera;
            canvas.sortingOrder = 10;
            canvas.pixelPerfect = false; // Performans i�in
        }
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            // Optimize edilmi� billboard rotasyonu
            Vector3 directionToCamera = mainCamera.transform.position - transform.position;
            directionToCamera.y = 0; // Y ekseni rotasyonunu engelle (sadece yatay d�ns�n)

            if (directionToCamera.sqrMagnitude > 0.01f) // sqrMagnitude daha h�zl�
            {
                transform.rotation = Quaternion.LookRotation(-directionToCamera);
            }
        }
    }
}