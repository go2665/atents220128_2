using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BattleField : MonoBehaviour
{
    const int FieldSize = 10;
    const int ShipCount = 5;

    [Flags]
    enum FieldSpace : short
    {
        None = 0,
        Ship = 1,
        CannonBall = 2
        //ShipHead_1 = 1,
        //ShipBody_1 = 2,
        //ShipHead_2 = 4,
        //ShipBody_2 = 8,
        //ShipHead_3 = 16,
        //ShipBody_3 = 32,
        //ShipHead_4 = 64,
        //ShipBody_4 = 128,
        //CannonBall = 256
    }
    //const FieldSpace ShipSpace = FieldSpace.ShipHead_1 | FieldSpace.ShipHead_2 | FieldSpace.ShipHead_3 | FieldSpace.ShipHead_4
    //    | FieldSpace.ShipBody_1 | FieldSpace.ShipBody_2 | FieldSpace.ShipBody_3 | FieldSpace.ShipBody_4;
    const FieldSpace ShipSpace = FieldSpace.Ship;
    FieldSpace[,] field = new FieldSpace[FieldSize, FieldSize];     // 확인용 데이터

    Ship[] ships = new Ship[ShipCount];
    List<Vector2Int> cannonballPosition = new List<Vector2Int>(FieldSize* FieldSize);
    
    
    /// <summary>
    /// 해당 위치에 배가 있는지 확인
    /// </summary>
    /// <param name="pos">확인할 위치</param>
    /// <returns>배가 있으면 true, 없으면 false</returns>
    public bool IsShipThere(Vector2Int pos)
    {
        return ((field[pos.x, pos.y] & ShipSpace) != 0);
    }

    //- 전함 배치 가능 여부
    bool IsShipDeployment(Vector2Int pos, Ship ship, out Vector2Int[] positions)
    {
        // 배치할 배가 차지하는 위치
        positions = new Vector2Int[ship.size];
        //for (int i = 0; i < ship.size; i++)
        //{
        //    switch (ship.Direction)
        //    {
        //        case Ship.ShipDirection.EAST:
        //            positions[i].Set(pos.x - i, pos.y);
        //            break;
        //        case Ship.ShipDirection.SOUTH:
        //            positions[i].Set(pos.x, pos.y - i);
        //            break;
        //        case Ship.ShipDirection.WEST:
        //            positions[i].Set(pos.x + i, pos.y);
        //            break;
        //        case Ship.ShipDirection.NORTH:
        //            positions[i].Set(pos.x, pos.y + i);
        //            break;
        //        default:
        //            break;
        //    }
        //}

        //int[] tempX = { -1, 0, 1, 0 };
        //int[] tempY = { 0, -1, 0, 1 };
        //int index = (int)ship.Direction;
        Vector2Int[] temp = { new Vector2Int(-1, 0), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(0, 1) };
        for ( int i=0 ; i<ship.size ; i++ )
        {
            //positions[i].Set(pos.x + tempX[index] * i, pos.y + tempY[index] * i);
            positions[i] = pos + temp[i] * i;
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
                result = false;
                break;
            }
        }

        return result;
    }

    //- 전함 배치
    public void ShipDeployment(Vector2Int pos, Ship ship)
    {
        Vector2Int[] positions = new Vector2Int[ship.size];
        if ( IsShipDeployment(pos, ship, out positions) )
        {
            foreach(Vector2Int position in positions)
            {
                field[position.x, position.y] = FieldSpace.Ship;
            }
            ship.Position = pos;
        }
    }


    //- 게임 종료 여부

    public void Test()
    {

    }
}
