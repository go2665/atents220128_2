using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    public GameObject enemyTank = null;
    public float waveInterval = 10.0f;
    public int waveSpawnCount = 3;
    public float spawnInterval = 0.5f;
    public Transform spawnTransform = null;

    WaitForSeconds waitSpawnInteval = null;

    private void Start()
    {
        waitSpawnInteval = new WaitForSeconds(spawnInterval);
        StartCoroutine(WaveSpawn());
    }

    IEnumerator WaveSpawn()
    {
        for( int i=0;i<waveSpawnCount;i++)
        {
            GameObject obj = Instantiate(enemyTank);
            Vector2 randomCircle = Random.insideUnitCircle * 5.0f;
            Vector3 randPosition = spawnTransform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
            NavMeshAgent nav = obj.GetComponent<NavMeshAgent>();
            nav.Warp(transform.position);
            nav.SetDestination(randPosition);

            yield return waitSpawnInteval;
        }

    }
}
