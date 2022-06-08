using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    public GameObject enemyTank = null;     // 생성할 탱크
    public float spawnInterval = 0.5f;      // 한 웨이브 안에서 연속적으로 적을 생성할 때의 간격
    public Transform spawnTransform = null; // 시작 집결지

    WaitForSeconds waitSpawnInteval = null; // WaitForSeconds 재활용 용도

    private void Start()
    {
        waitSpawnInteval = new WaitForSeconds(spawnInterval);
    }

    /// <summary>
    /// EnemyBaseController에서 웨이브 시작할 때 실행시킬 함수
    /// </summary>
    /// <param name="spawnCount">한 웨이브에서 생성할 개수</param>
    public void SpawnStart(int spawnCount)
    {
        StartCoroutine(SpawnEnemy(spawnCount));
    }

    /// <summary>
    /// 적 연속 생성용 코루틴
    /// </summary>
    /// <param name="spawnCount">생성할 수</param>
    /// <returns></returns>
    IEnumerator SpawnEnemy(int spawnCount)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject obj = Instantiate(enemyTank);
            Vector2 randomCircle = Random.insideUnitCircle * 5.0f;  // 시작 집결지에 랜덤성 추가
            Vector3 randPosition = spawnTransform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
            NavMeshAgent nav = obj.GetComponent<NavMeshAgent>();
            nav.Warp(transform.position);       // 해리패드 위에 탱크 배치시키기 위해 사용.
            nav.SetDestination(randPosition);   // 시작 집결지로 이동

            yield return waitSpawnInteval;
        }
    }
}
