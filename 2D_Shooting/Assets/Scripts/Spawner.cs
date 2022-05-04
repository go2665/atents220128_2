using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemy = null;
    public Transform defaultSpawnPosition = null;
    public float randomRangeY = 5.0f;
    public float spawnInterval = 1.0f;

    WaitForSeconds intervalWait = null;

    private void Start()
    {
        intervalWait = new WaitForSeconds(spawnInterval);
        StartCoroutine(SpawnCouroutine());
    }

    IEnumerator SpawnCouroutine()
    {
        while(true)
        {
            Spawn();
            yield return intervalWait;
        }
    }

    void Spawn()
    {
        Vector2 randomPos = new Vector2(defaultSpawnPosition.position.x, defaultSpawnPosition.position.y + Random.Range(0.0f, randomRangeY));
        Instantiate(enemy, randomPos, Quaternion.identity);
    }
}
