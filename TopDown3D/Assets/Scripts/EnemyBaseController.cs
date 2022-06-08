using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseController : MonoBehaviour
{
    public float waveInterval = 10.0f;      // 각 웨이브간의 간격
    public int waveSpawnCount = 3;          // 웨이브마다 생성할 적의 수

    private int waveBaseIndex = 0;                  // 현재 웨이브에서 적을 생성할 기지의 인덱스
    private WaitForSeconds waveIntervalTime = null; // 코루틴에서 재활용 용으로 사용

    private EnemyBase[] enemyBases = null;          // 적 기지

    private void Awake()
    {
        enemyBases = GetComponentsInChildren<EnemyBase>();
        waveIntervalTime = new WaitForSeconds(waveInterval);
    }

    /// <summary>
    /// 다음 웨이브 시작
    /// </summary>
    void NextWaveStart()
    {
        enemyBases[waveBaseIndex].SpawnStart(waveSpawnCount);       // 적 기지에 웨이브 시작 신호 보냄
        waveBaseIndex = (waveBaseIndex + 1) % enemyBases.Length;    // waveBaseIndex를 순환시키기
        waveSpawnCount++;   // 다음 웨이브에서 생성될 적 수 추가
    }

    IEnumerator StartWave()
    {
        while(true)
        {
            NextWaveStart();                // 정해진 시간간격으로 계속 웨이브 수행
            yield return waveIntervalTime;
        }
    }

    private void Start()
    {
        StartCoroutine(StartWave());
    }

}
