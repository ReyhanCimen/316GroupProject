using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [Header("Zombie Settings")]
    public GameObject zombiePrefab;
    public Transform[] spawnPoints;

    [Header("Wave Settings")]
    public int[] zombiesPerWave = new int[] { 10, 15, 20, 25, 50 };
    private int currentWave = 0;
    private List<GameObject> aliveZombies = new List<GameObject>();
    private bool isWaveInProgress = false;

    [Header("UI Elements")]
    public Text waveText;

    private void Start()
    {
        StartCoroutine(StartNextWave());
    }

    private void Update()
    {
        if (!isWaveInProgress && currentWave < zombiesPerWave.Length)
        {
            aliveZombies.RemoveAll(z => z == null);

            if (aliveZombies.Count == 0)
            {
                StartCoroutine(StartNextWave());
            }
        }
    }

    IEnumerator StartNextWave()
    {
        isWaveInProgress = true;

        if (currentWave >= zombiesPerWave.Length)
        {
            waveText.text = "All waves completed!";
            yield break;
        }

        // 1. Duyuru: Wave başlıyor
        waveText.text = $"Wave {currentWave + 1} is about to start!";
        yield return new WaitForSeconds(2f);
        waveText.text = "";

        // 2. Geri sayım
        for (int i = 5; i > 0; i--)
        {
            waveText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        // 3. Savaş başlasın
        waveText.text = "FIGHT!";
        yield return new WaitForSeconds(1f);
        waveText.text = "";

        // 4. Zombileri spawnla
        int zombieCount = zombiesPerWave[currentWave];
        for (int i = 0; i < zombieCount; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject zombie = Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
            aliveZombies.Add(zombie);
        }

        currentWave++;
        isWaveInProgress = false;
    }
}
