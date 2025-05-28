using System.Collections.Generic;
using UnityEngine;

public class BoosterSpawner : MonoBehaviour
{
    [Header("Booster Prefabs")]
    public GameObject healthBoostPrefab;
    public GameObject ammoBoostPrefab;

    [Header("Spawn Points")]
    public Transform[] spawnPoints; // Assign istediğin kadar konum

    // Internal state
    private List<Transform> availableSpawnPoints = new List<Transform>();

    private void Start()
    {
        // Başta tüm spawn noktalarını müsait olarak işaretle
        availableSpawnPoints.AddRange(spawnPoints);
    }

    public void SpawnBoosters()
    {
        List<Transform> freeSpots = GetFreeSpawnPoints();

        int healthToSpawn = 5;
        int ammoToSpawn = 5;

        int totalNeeded = healthToSpawn + ammoToSpawn;
        if (freeSpots.Count < totalNeeded)
        {
            Debug.LogWarning("Yeterli boş spawn noktası yok. Boost sayısı azaltılacak.");
            totalNeeded = freeSpots.Count;
            healthToSpawn = totalNeeded / 2;
            ammoToSpawn = totalNeeded - healthToSpawn;
        }

        // Spawn Health Boosts
        for (int i = 0; i < healthToSpawn; i++)
        {
            Transform spot = GetRandomFreeSpot(freeSpots);
            Instantiate(healthBoostPrefab, spot.position, Quaternion.identity);
              Debug.Log("SpawnBoosters called");
        }

        // Spawn Ammo Boosts
        for (int i = 0; i < ammoToSpawn; i++)
        {
            Transform spot = GetRandomFreeSpot(freeSpots);
            Instantiate(ammoBoostPrefab, spot.position, Quaternion.identity);
              Debug.Log("SpawnBoosters called");
        }
    }

    private List<Transform> GetFreeSpawnPoints()
    {
        List<Transform> freeSpots = new List<Transform>();

        foreach (Transform spot in spawnPoints)
        {
            Collider[] colliders = Physics.OverlapSphere(spot.position, 0.5f);
            bool hasBooster = false;

            foreach (var col in colliders)
            {
                if (col.CompareTag("HealthBoost") || col.CompareTag("AmmoBoost"))
                {
                    hasBooster = true;
                    break;
                }
            }

            if (!hasBooster)
                freeSpots.Add(spot);
        }

        return freeSpots;
    }

    private Transform GetRandomFreeSpot(List<Transform> freeSpots)
    {
        int index = Random.Range(0, freeSpots.Count);
        Transform selected = freeSpots[index];
        freeSpots.RemoveAt(index);
        return selected;
    }
}
