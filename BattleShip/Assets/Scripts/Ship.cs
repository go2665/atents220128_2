using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    enum Direction : byte   // 4개만 필요해서 최소 단위인 byte로
    {
        EAST = 0,
        SOUTH = 1,
        WEST = 2,
        NORTH = 3
    }

    [Range(2,5)]
    public int size = 2;                    // 배의 크기
    
    readonly int pivotIndex = 0;            // 회전 중심축 위치(뱃머리)

    Direction direction = Direction.NORTH;  // 배가 바라보는 방향
    bool[] hittedPoint = null;              // 피격당한 위치(맞았으면 true, 아니면 false)

    Vector2Int position = Vector2Int.zero;  // 배의 위치

    int dirCount = 4;                       // enum Direction의 개수

    /// <summary>
    /// 필드 위에서의 위치
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
            bool sinking = true;
            foreach(var point in hittedPoint)
            {
                if (point == false) // 하나라도 안맞은 위치가 있으면 가라앉는 상태가 아님
                {
                    sinking = false;
                    break;
                }
            }
            return sinking;
        }
    }

    /// <summary>
    /// 초기화 함수. start 시점에서 실행
    /// </summary>
    void Initialize()
    {
        hittedPoint = new bool[size];
        dirCount = System.Enum.GetValues(typeof(Direction)).Length;
    }

    /// <summary>
    /// 함선을 회전시키는 함수
    /// </summary>
    /// <param name="clockwise">회전방향. true면 시계방향</param>
    public void Rotate(bool clockwise = true)
    {
        if (clockwise)
        {
            direction = (Direction)(((int)direction + 1) % dirCount);
        }
        else
        {
            int decrease = (int)direction - 1;
            if (decrease < 0)
            {
                direction = (Direction)(dirCount - 1);
            }
            else
            {
                direction = (Direction)decrease;
            }
        }
        Debug.Log($"{gameObject.name} 함선은 {System.Enum.GetValues(typeof(Direction)).GetValue((int)direction)}쪽을 바라봅니다.");
    }

    /// <summary>
    /// 함선이 맞은 위치를 표시
    /// </summary>
    /// <param name="index">함선이 맞은 위치</param>
    public void Hit(int index)
    {
        if ( -1 < index && index < size)
        {
            hittedPoint[index] = true;
            Debug.Log($"{gameObject.name} 함선은 {index}번 칸을 맞았습니다.");
        }
    }

    private void Start()
    {
        Initialize();
    }
}
