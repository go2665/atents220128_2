using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform defaultSpawnPosition = null;
    public Transform defaultTargetPosition = null;
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
        // 랜덤으로 시작위치와 도착위치 결정
        Vector2 startPos = new Vector2(defaultSpawnPosition.position.x, defaultSpawnPosition.position.y + Random.Range(0.0f, randomRangeY));
        Vector2 targetPos = new Vector2(defaultTargetPosition.position.x, defaultTargetPosition.position.y + Random.Range(0.0f, randomRangeY));
        
        // 시작위치와 도착위치를 기반으로 회전값 계산
        Vector2 dir = targetPos - startPos;
        float angle = Vector2.SignedAngle(Vector2.left, dir);
        Quaternion quat = Quaternion.Euler(0, 0, angle);
                
        // 메모리풀에서 적을 하나 가져와서 위치와 회전과 벨로시티 갱신
        GameObject spawnEnemy = GameManager.Inst.GetEnemyObject();
        spawnEnemy.transform.position = startPos;
        spawnEnemy.transform.rotation = quat;
        Enemy enemyComp = spawnEnemy.GetComponent<Enemy>();
        enemyComp?.ResetVelocity();        
    }
}
