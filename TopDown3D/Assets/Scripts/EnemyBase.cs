using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour, IHealth
{
    public GameObject enemyTank = null;     // 생성할 탱크
    public float spawnInterval = 0.5f;      // 한 웨이브 안에서 연속적으로 적을 생성할 때의 간격
    public Transform spawnTransform = null; // 시작 집결지
    public float restartTime = 10.0f;

    private CountDown countDownUI = null;

    WaitForSeconds waitSpawnInteval = null; // WaitForSeconds 재활용 용도
    WaitForSeconds waitRestartInteval = null;

    bool isDead = false;

    private float hp = 100.0f;
    public float HP 
    { 
        get => hp;
        set
        {
            hp = value;                        
            if (hp<0.0f && !isDead)
            {
                hp = 0.0f;
                Dead();
            }

            hp = Mathf.Clamp(hp, 0, MaxHP);
            onHealthChange?.Invoke();
        }
    }

    public float MaxHP { get => 100.0f; }

    private Vector3 hitPoint = Vector3.zero;
    public Vector3 HitPoint { set => hitPoint = value; }
    public IHealth.HealthDelegate onHealthChange { get; set; }
    public IHealth.HealthDelegate onDead { get; set; }
    public IHealth.HealthDelegate onResurrection { get; set; }

    public CountDown CountDownUI { get => countDownUI; }

    void Awake()
    {
        countDownUI = GetComponentInChildren<CountDown>();
    }

    private void Start()
    {
        HP = MaxHP;
        waitSpawnInteval = new WaitForSeconds(spawnInterval);
        waitRestartInteval = new WaitForSeconds(restartTime);
        countDownUI.gameObject.SetActive(false);
    }

    void Update()
    {
        if( isDead )
        {
            HP = HP + 1 / restartTime * Time.deltaTime * MaxHP;
            //Debug.Log($"Base HP : {hp}");
        }
    }

    /// <summary>
    /// EnemyBaseController에서 웨이브 시작할 때 실행시킬 함수
    /// </summary>
    /// <param name="spawnCount">한 웨이브에서 생성할 개수</param>
    public void SpawnStart(int spawnCount)
    {
        countDownUI.gameObject.SetActive(false);
        StartCoroutine(SpawnEnemy(spawnCount));
    }

    /// <summary>
    /// 적 연속 생성용 코루틴
    /// </summary>
    /// <param name="spawnCount">생성할 수</param>
    /// <returns></returns>
    IEnumerator SpawnEnemy(int spawnCount)
    {
        if (!isDead)
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

    public void Dead()
    {
        StopAllCoroutines();
        isDead = true;
        onDead?.Invoke();
        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        //Debug.Log("Base die");
        yield return waitRestartInteval;
        isDead = false;
        onResurrection?.Invoke();
        //Debug.Log("Base restart");

        //Camera.main.WorldToScreenPoint()
    }

    public void SpawnReady(float waitTime)
    {
        countDownUI.gameObject.SetActive(true);
        countDownUI.CountTime = waitTime;
    }
}
