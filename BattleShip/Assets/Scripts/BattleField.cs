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
    const int FieldSize = 10;

    /// <summary>
    /// 한 필드에 있을 수 있는 배의 수
    /// </summary>
    const int ShipCount = 5;

    // enum ---------------------------------------------------------------------------------------
    /// <summary>
    /// 필드 한칸에 들어있는 것을 표시할 enum
    /// </summary>
    [Flags]
    enum FieldSpace : byte
    {
        None = 0,
        Ship = 1,
        CannonBall = 2
    }

    // 주요 변수 -----------------------------------------------------------------------------------
    /// <summary>
    /// 필드. 특정 좌표에 배치된 오브젝트 표시(확인용)
    /// </summary>
    FieldSpace[,] field = new FieldSpace[FieldSize, FieldSize];

    /// <summary>
    /// 그래픽 출력용 배와 포탄
    /// </summary>
    Ship[] ships = new Ship[ShipCount];
    List<Vector2Int> cannonballPosition = new List<Vector2Int>(FieldSize* FieldSize);
    
    

    // 함수들 --------------------------------------------------------------------------------------
    /// <summary>
    /// 해당 위치에 배가 있는지 확인
    /// </summary>
    /// <param name="pos">확인할 위치</param>
    /// <returns>배가 있으면 true, 없으면 false</returns>
    public bool IsShipThere(Vector2Int pos)
    {
        return ((field[pos.x, pos.y] & FieldSpace.Ship) != 0);
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
            if( 0 > positions[i].x || positions[i].x >= FieldSize
                || 0 > positions[i].y || positions[i].y >= FieldSize 
                || IsShipThere(positions[i]))
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
                field[position.x, position.y] = FieldSpace.Ship;
            }
            ship.Position = pos;
            Debug.Log($"{ship.gameObject.name}을 ({pos.x}, {pos.y})에 배치했습니다.");
        }
    }
}
