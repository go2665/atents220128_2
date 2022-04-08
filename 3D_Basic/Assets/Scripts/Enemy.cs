using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 적 상태 표시용 enum
public enum EnemyState
{
    PATROL = 0,
    CHASE,
    ATTACK
}

public class Enemy : MonoBehaviour
{
    public Transform[] waypoints = null;    // 순찰할 지점
    public float attackDelay = 1.0f;        // 공격 가능할 때 공격 시작할 때까지의 딜레이
    public float sightRange = 5.0f;
    public float attackRange = 1.0f;

    NavMeshAgent agent = null;              // 길찾기용 NavMesh 에이전트
    Transform target = null;                // 추적할 대상(플레이어만 대상이 된다)
    int waypointIndex = 0;                  // 현재 내가 가야할 웨이포인트의 인덱스
    int enterCounter = 0;                   // 겹친 트리거에 들어간 정도
    int EnterCounter
    {
        get
        {
            return enterCounter;
        }
        set
        {
            enterCounter = Mathf.Clamp(value, 0, 2);    // enterCounter의 범위 지정
        }
    }
    float timeCount = 0.0f;                 // 공격 가능한 범위에 들어갔을 때부터 지난 시간
    EnemyState state = EnemyState.PATROL;   // 현재 enemy의 상태


    // 게임 오브젝트가 생성된 직후
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();   // agent 초기화
        //agent.remainingDistance
        SphereCollider[] colliders = GetComponents<SphereCollider>();
        colliders[0].radius = sightRange;
        colliders[0].center = new Vector3(0, 1.8f, 0);
        colliders[1].radius = attackRange;
        colliders[1].center = new Vector3(0, 1, 0);

    }

    // 첫번째 update가 실행되기 직전
    private void Start()
    {
        state = EnemyState.PATROL;          // 시작은 PATROL 상태로
        agent.SetDestination(waypoints[waypointIndex].position);    // 첫번째 웨이포인트로 이동
    }

    // 목표지점에 도착했는지 체크하는 함수
    bool CheckArrive()
    {
        //agent.pathPending : 에이전트가 경로를 계산 중이면 true 아니면 false
        //agent.remainingDistance : 도착지점까지 남아있는 거리
        //agent.stoppingDistance : 도착했다고 판단해도 되는 거리

        return !agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance);
    }

    // 다음 웨이포인트 지점으로 이동
    void GoNextWaypoint()
    {
        waypointIndex++;
        waypointIndex = waypointIndex % waypoints.Length;
        agent.SetDestination(waypoints[waypointIndex].position);    // 다음 웨이포인트로 이동
    }

    // 트리거로 설정된 내 컬라이더 안에 다른 컬라이더가 들어왔을 때 실행
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))  // 플레이어가 들어왔다면
        {
            EnterCounter++;             // 상태를 결정하는 EnterCounter를 증가시키고
            StateTransition(EnterCounter, other.gameObject);    // 상태를 변화시킨다.
        }
    }

    // 트리거로 설정된 내 컬라이더 안에서 다른 컬라이더가 나갔을 때 실행
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))  // 플레이어가 나갔다면
        {
            EnterCounter--;             // 상태 변경시키기
            StateTransition(EnterCounter, other.gameObject);
        }
    }

    // 상태를 변경시키는 함수
    void StateTransition(int counter, GameObject _target)
    {
        state = (EnemyState)counter;        // counter값으로 상태 지정

        switch (state)                      // 지정된 상태에 따라 알맞은 작업 수행
        {
            case EnemyState.PATROL:         // 순찰 상태일 때 
                target = null;              // 대상 없음
                agent.isStopped = false;    // 길찾기 사용
                agent.SetDestination(waypoints[waypointIndex].position); // 원래 순찰 경로로 돌아가기
                break;
            case EnemyState.CHASE:          // 추적 상태일 때
                target = _target.transform; // 추적 대상 설정
                agent.isStopped = false;    // 길찾기 사용
                break;
            case EnemyState.ATTACK:         // 공격 상태일 때
                target = _target.transform; // 공격 대상 설정
                agent.isStopped = true;     // 기찾기 정지
                timeCount = 0;              // 공격용 카운터 초기화
                break;
            default:                        // 절대로 들어오면 안된다.
                target = null;
                agent.isStopped = true;
                break;
        }
    }

    // 고정된 시간 간격으로 호출
    private void FixedUpdate()
    {
        switch (state)                  // 상태에 따라 다른 진행
        {
            case EnemyState.PATROL:     // 순찰 상태일 때                
                if (CheckArrive())      // 목적지에 도착했는지 확인
                {
                    GoNextWaypoint();   // 도착했으면 다음 지점으로 이동
                }
                break;
            case EnemyState.CHASE:      // 추적 상태일 때
                if (CheckObstacle(target))
                {
                    // 장애물이 있다 => 패트롤 계속 진행
                    if (CheckArrive())      // 목적지에 도착했는지 확인
                    {
                        GoNextWaypoint();   // 도착했으면 다음 지점으로 이동
                    }
                }
                else
                {
                    agent.SetDestination(target.position);  // 추적 대상 위치로 이동
                }
                break;
            case EnemyState.ATTACK:     // 공격 상태일 때
                timeCount += Time.fixedDeltaTime;       // 시간 누적시켜서
                if (timeCount > attackDelay)            // 누적된 시간이 딜레이보다 커지면
                {
                    Debug.Log($"공격 : {Time.realtimeSinceStartup}"); // 공격
                    timeCount = 0;                      // 누적시킨 시간 초기화
                }
                break;
            default:
                break;
        }
    }

    bool CheckObstacle(Transform _target)
    {
        bool result = true;

        Vector3 origin = transform.position + Vector3.up * 1.8f;

        Ray ray = new Ray(origin, _target.position - transform.position );// 시작점과 방향 필요
        RaycastHit hit = new RaycastHit();      // 레이캐스트의 결과를 담을 구조체
        Physics.Raycast(ray, out hit, sightRange);    // 레이캐스트 실행
        if( hit.collider != null )
        {
            if( hit.collider.CompareTag("Player") ) // 플레이어와 적 사이에 가리는 물체가 없다.
            {
                result = false;
                //Debug.Log("See!");
            }
        }
        
        return result;
    }

    
    

    //void Test()
    //{
    //    agent.Warp(new Vector3(1,2,3)); // 순간이동 시킬때
    //    agent.speed;
    //    agent.acceleration;
    //    agent.radius;
    //    agent.avoidancePriority;
    //    agent.remainingDistance;
    //    agent.stoppingDistance;
    //    agent.autoRepath;
    //    agent.pathPending;              // 경로 계산중인지 아닌지 확인할 때
    //}
}
