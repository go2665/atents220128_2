using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ship : MonoBehaviour
{
    // 주요 데이터 ---------------------------------------------------------------------------------
    /// <summary>
    /// 배의 크기 무조건 2~5의 크기여야 함
    /// </summary>
    const int MinSize = 2;
    const int MaxSize = 5;
    [Range(MinSize, MaxSize)]
    public int size = 2;

    /// <summary>
    /// 배의 종류. 크기에 맞게 설정되어야 한다.
    /// </summary>
    public ShipType shipType = ShipType.Ship2;

    /// <summary>
    /// 일반적인 상황에 사용할 머티리얼
    /// </summary>
    public Material normal = null;

    /// <summary>
    /// 배치 불가능한 상황에서 사용할 머티리얼
    /// </summary>
    public Material error = null;

    /// <summary>
    /// 머티리얼 변경 때 사용하기 위해 캐싱해 놓은 것
    /// </summary>
    MeshRenderer meshRenderer = null;

    /// <summary>
    /// 배가 바라보는 방향. 북쪽을 바라보는 것이 디폴트
    /// </summary>
    ShipDirection direction = ShipDirection.NORTH;

    /// <summary>
    /// 피격당한 위치(맞았으면 true, 아니면 false). 현재 사용하지 않음
    /// </summary>
    bool[] hittedPoint = null;

    /// <summary>
    /// 배의 HP.
    /// </summary>
    int hp = 0;

    /// <summary>
    /// 배의 필드 상 위치. 
    /// </summary>
    Vector2Int position = Vector2Int.zero;

    /// <summary>
    /// 배가 배치되었는지 여부. true면 배치된 배라는 의미
    /// </summary>
    bool isDeployed = false;


    // 읽기 전용 -----------------------------------------------------------------------------------
    /// <summary>
    /// 회전 중심축 위치(뱃머리 위치)
    /// </summary>
    //readonly int pivotIndex = 0;       

    // enum ---------------------------------------------------------------------------------------
    /// <summary>
    /// 배의 방향을 나타낼 enum
    /// </summary>
    public enum ShipDirection : byte   // 4개만 필요해서 최소 단위인 byte로
    {
        EAST = 0,
        SOUTH = 1,
        WEST = 2,
        NORTH = 3
    }

    /// <summary>
    /// enum Direction의 개수 
    /// </summary>
    int dirCount = 4;

    // 프로퍼티 ------------------------------------------------------------------------------------
    /// <summary>
    /// 필드 위에서의 위치(해드부분)
    /// </summary>
    public Vector2Int Position
    {
        get => position;
        set
        {
            position = value;
        }
    }
    
    /// <summary>
    /// 배가 배치되었는지에 대한 프로퍼티. true면 배치되었음
    /// </summary>
    public bool IsDeployed
    {        
        get => isDeployed;
        set => isDeployed = value;
    }

    /// <summary>
    /// 침몰 중인지 아닌지 확인용. true면 가라앉아있음. false면 아직 떠 있음.
    /// </summary>
    public bool IsSinking
    {
        get
        {
            //bool sinking = true;
            //foreach(var point in hittedPoint)
            //{
            //    if (point == false) // 하나라도 안맞은 위치가 있으면 가라앉는 상태가 아님
            //    {
            //        sinking = false;
            //        break;
            //    }
            //}
            //return sinking;
            if(hp < 1)
            {
                Debug.Log($"{gameObject.name} 배가 가라앉았습니다.");
            }
            return (hp < 1);    // 배 HP가 0 이하면 침몰
        }
    }

    /// <summary>
    /// 배가 바라보는 방향 표시.
    /// </summary>
    public ShipDirection Direction
    {
        get => direction;   //set은 Rotate로 실행
    }


    // 주요 함수 -----------------------------------------------------------------------------------
    /// <summary>
    /// 초기화 함수. start 시점에서 실행
    /// </summary>
    void Initialize(int newSize = 2)
    {
        size = Mathf.Clamp(newSize, MinSize, MaxSize);
        hp = size;
        hittedPoint = new bool[size];
        dirCount = System.Enum.GetValues(typeof(ShipDirection)).Length;        
    }

    /// <summary>
    /// 함선을 회전시키는 함수
    /// </summary>
    /// <param name="clockwise">회전방향. true면 시계방향</param>
    public void Rotate(bool clockwise = true)
    {
        float angle = 0.0f; // 매시 회전용 각도
        if (clockwise)
        {
            direction = (ShipDirection)(((int)direction + 1) % dirCount);
            angle = 90.0f;
        }
        else
        {
            int decrease = (int)direction - 1;
            if (decrease < 0)
            {
                direction = (ShipDirection)(dirCount - 1);
            }
            else
            {
                direction = (ShipDirection)decrease;
            }
            angle = -90.0f;
        }
        this.transform.Rotate(0, angle, 0); // 방향에 맞춰 회전
        //Debug.Log($"{gameObject.name} 함선은 {System.Enum.GetValues(typeof(ShipDirection)).GetValue((int)direction)}쪽을 바라봅니다.");
    }
        
    /// <summary>
    /// 매시 머티리얼 변경 함수
    /// </summary>
    /// <param name="isNormal">true면 일반 상황, false면 배치 불가능 상황</param>
    public void SetMaterial(bool isNormal)
    {
        if (isNormal)
        {
            // 일반 상황
            if (meshRenderer.material != normal)    // 변경할 필요가 있을 때만 변경
            {
                meshRenderer.material = normal;
            }
        }
        else
        {
            // 배치 불가 상황
            if (meshRenderer.material != error)     // 변경할 필요가 있을 때만 변경
            {
                meshRenderer.material = error;
            }
        }
    }

    /// <summary>
    /// 함선이 맞은 위치를 표시
    /// </summary>
    /// <param name="index">함선이 맞은 위치</param>
    public void Hit(/*int index*/)
    {
        hp -= 1;
        //if ( -1 < index && index < size)
        //{
        //    hittedPoint[index] = true;
        //    Debug.Log($"{gameObject.name} 함선은 {index}번 칸을 맞았습니다.");
        //}
    }

    // 유니티 이벤트 함수 --------------------------------------------------------------------------
    private void Start()
    {
        Initialize(size);
    }

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }
}
