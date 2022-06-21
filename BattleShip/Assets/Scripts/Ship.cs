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

    public Material normal = null;
    public Material error = null;

    MeshRenderer meshRenderer = null;

    /// <summary>
    /// 배가 바라보는 방향. 북쪽을 바라보는 것이 디폴트
    /// </summary>
    ShipDirection direction = ShipDirection.NORTH;

    /// <summary>
    /// 피격당한 위치(맞았으면 true, 아니면 false)
    /// </summary>
    bool[] hittedPoint = null;

    int hp = 0;

    /// <summary>
    /// 배의 필드 상 위치. 
    /// </summary>
    Vector2Int position = Vector2Int.zero;

    

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
        set => position = value;
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

    //public bool MouseFollowMode { get; set; }

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

        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    /// <summary>
    /// 함선을 회전시키는 함수
    /// </summary>
    /// <param name="clockwise">회전방향. true면 시계방향</param>
    public void Rotate(bool clockwise = true)
    {
        float angle = 0.0f;
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
        this.transform.Rotate(0, angle, 0);
        Debug.Log($"{gameObject.name} 함선은 {System.Enum.GetValues(typeof(ShipDirection)).GetValue((int)direction)}쪽을 바라봅니다.");
    }
        
    public void SetMaterial(bool isNormal)
    {
        if (meshRenderer != null)
        {
            if (isNormal)
            {
                if (meshRenderer.material != normal)
                {
                    meshRenderer.material = normal;
                }
            }
            else
            {
                if (meshRenderer.material != error)
                {
                    meshRenderer.material = error;
                }
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

    //private void Update()
    //{
    //    if( MouseFollowMode )
    //    {
    //        Vector2 mousePos = Mouse.current.position.ReadValue();
    //        Vector3 newPos = Camera.main.ScreenToWorldPoint(mousePos);
    //        transform.position = new Vector3(newPos.x, 0.0f, newPos.z);
    //    }
    //}
}
