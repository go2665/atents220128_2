using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BattleField : MonoBehaviour
{
    // 상수들 --------------------------------------------------------------------------------------
    /// <summary>
    /// 필드 한변의 크기.(필드는 항상 정사각형)
    /// </summary>
    public const int FieldSize = 10;

    /// <summary>
    /// 한 필드에 있을 수 있는 배의 수
    /// </summary>
    const int ShipCount = 5;

    // enum ---------------------------------------------------------------------------------------
    /// <summary>
    /// 필드 한칸에 배가 있는지 포탄이 있는지에 대한 정보를 표시할 enum
    /// </summary>    
    public enum FieldExists : byte
    {
        None = 0,
        Ship = 1,
        CannonBall = 2
    }    

    public struct FieldCell
    {
        /// <summary>
        /// 필드 한칸에 배가 있는지 포탄이 있는지에 대한 정보를 가짐
        /// </summary>
        public FieldExists exists;

        /// <summary>
        /// 필드 한칸에 어떤 배가 있는지에 대한 정보
        /// </summary>
        public Ship ship;
    }

    // 주요 변수 -----------------------------------------------------------------------------------
    /// <summary>
    /// 필드. 특정 좌표에 배치된 오브젝트 표시(확인용)
    /// </summary>
    FieldCell[,] field = new FieldCell[FieldSize, FieldSize];

    /// <summary>
    /// 필드에 배치된 가라앉지 않은 배의 수
    /// </summary>
    int fieldHP = 0;

    /// <summary>
    /// 그래픽 출력용 배와 포탄
    /// </summary>
    Ship[] ships = new Ship[ShipCount];
    List<Vector2Int> cannonballPosition = new List<Vector2Int>(FieldSize* FieldSize);


    // 프로퍼티 ------------------------------------------------------------------------------------
    /// <summary>
    /// 필드의 배가 모두 침몰되었는지 여부. true면 모든 배가 침몰되었다.
    /// </summary>
    public bool IsDepeat
    {
        get => (fieldHP <= 0);
    }
    
    

    // 함수들 --------------------------------------------------------------------------------------
    /// <summary>
    /// 게임이 시작될때 실행될 초기화 함수
    /// </summary>
    void Initialize()
    {
        fieldHP = ShipCount;
    }

    /// <summary>
    /// 해당 위치에 배가 있는지 확인
    /// </summary>
    /// <param name="pos">확인할 위치</param>
    /// <returns>배가 있으면 true, 없으면 false</returns>
    public bool IsShipThere(Vector2Int pos)
    {
        return ((field[pos.x, pos.y].exists & FieldExists.Ship) != 0);
    }

    /// <summary>
    /// 배를 배치 가능한지 확인하는 함수
    /// </summary>
    /// <param name="pos">배치할 위치</param>
    /// <param name="ship">배치할 배</param>
    /// <param name="positions">배치 가능할 경우 배치되는 좌표들을 기록해 놓을 곳.(out)</param>
    /// <returns>true면 배치가능, false면 배치불가</returns>
    bool IsShipDeployment(Vector2Int pos, Ship ship, out Vector2Int[] positions)
    {
        positions = new Vector2Int[ship.size];      // 배 크기에 맞게 할당

        int index = (int)ship.Direction;
        Vector2Int[] temp = { new Vector2Int(-1, 0), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(0, 1) };
        for ( int i=0 ; i<ship.size ; i++ )
        {
            positions[i] = pos + temp[index] * i;   // 배가 배치될 좌표들 기록
        }

        bool result = true;
        for (int i = 0; i < ship.size; i++)
        {
            // 밖으로 벗어나는 위치가 있는지 확인
            // 같은 위치에 배가 있는지 확인
            if( !IsValidPosition(positions[i]) || IsShipThere(positions[i]))
            {
                //한 칸이라도 밖으로 벗어나거나 다른배와 겹칠 경우 안됨.
                Debug.Log($"{ship.gameObject.name}을 ({pos.x}, {pos.y})에 배치할 수 없습니다.");
                result = false;
                break;
            }
        }        
        return result;
    }

    /// <summary>
    /// 함선 배치. 배치할 수 없을 경우 실행 안함.
    /// </summary>
    /// <param name="pos">배치할 위치</param>
    /// <param name="ship">배치할 배</param>
    public void ShipDeployment(Vector2Int pos, Ship ship)
    {
        Vector2Int[] positions = null;
        if ( IsShipDeployment(pos, ship, out positions) )
        {
            foreach(Vector2Int position in positions)
            {
                field[position.x, position.y].exists = FieldExists.Ship;
                field[position.x, position.y].ship = ship;
            }
            ship.Position = pos;
            Debug.Log($"{ship.gameObject.name}을 ({pos.x}, {pos.y})에 배치했습니다.");
        }
    }

    /// <summary>
    /// 위치가 필드 범위 안인지 체크하는 함수
    /// </summary>
    /// <param name="pos">체크할 위치</param>
    /// <returns>true면 필드 안의 위치, false면 필드 밖의 위치</returns>
    public bool IsValidPosition(Vector2Int pos)
    {
        return (-1 < pos.x && pos.x < FieldSize && -1 < pos.y && pos.y < FieldSize);
    }

    /// <summary>
    /// 공격 가능한 위치인지 확인
    /// </summary>
    /// <param name="pos">공격할 위치</param>
    /// <returns>공격가능하면 true, 아니면 false</returns>
    public bool IsAttackable(Vector2Int pos)
    {
        return (field[pos.x, pos.y].exists != FieldExists.CannonBall);
    }

    /// <summary>
    /// 이 필드가 공격을 받음처리
    /// </summary>
    /// <param name="pos">공격받은 위치</param>
    public void Attacked(Vector2Int pos)
    {
        if (IsValidPosition(pos))
        {
            if (IsAttackable(pos))
            {
                if(field[pos.x, pos.y].exists == FieldExists.Ship)
                {
                    Debug.Log($"{gameObject.name} : 배({pos.x},{pos.y})가 공격받았습니다.");
                    field[pos.x, pos.y].ship.Hit();
                    if (field[pos.x, pos.y].ship.IsSinking)
                    {
                        fieldHP--;
                    }
                }
                else
                {
                    Debug.Log($"{gameObject.name} : 바다({pos.x},{pos.y})가 공격받았습니다.");
                }
                field[pos.x, pos.y].exists = FieldExists.CannonBall;                
            }
        }
    }


    // 유니티 이벤트 함수 --------------------------------------------------------------------------
    private void Start()
    {
        Initialize();
    }

}