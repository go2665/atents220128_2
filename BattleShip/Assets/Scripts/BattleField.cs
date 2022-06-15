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
        ShipHead_1 = 1,
        ShipBody_1 = 2,
        ShipHead_2 = 4,
        ShipBody_2 = 8,
        ShipHead_3 = 16,
        ShipBody_3 = 32,
        ShipHead_4 = 64,
        ShipBody_4 = 128,
        CannonBall = 256
    }
    const FieldSpace ShipSpace = FieldSpace.ShipHead_1 | FieldSpace.ShipHead_2 | FieldSpace.ShipHead_3 | FieldSpace.ShipHead_4
        | FieldSpace.ShipBody_1 | FieldSpace.ShipBody_2 | FieldSpace.ShipBody_3 | FieldSpace.ShipBody_4;
    FieldSpace[,] field = new FieldSpace[FieldSize, FieldSize];     // 그리기 및 확인용 데이터

    Ship[] ships = new Ship[ShipCount];
    List<Vector2Int> cannonballPosition = new List<Vector2Int>(FieldSize* FieldSize);

    /// <summary>
    /// 공격했을 때 결과 확인
    /// </summary>
    /// <param name="pos">true면 배에 맞았다. false면 다른 곳이다.</param>
    /// <returns></returns>
    public bool ShotResult(Vector2Int pos)
    {
        bool result = false;
        
        if( (field[pos.x, pos.y] & ShipSpace) != 0 )
        {
            result = true;
        }
        return result;
    }

    //- 전함 배치 가능 여부
    bool ShipDeployment(Vector2Int pos, Ship ship)
    {
        // 배치할 배가 차지하는 위치
        Vector2Int[] positions = new Vector2Int[ship.size];
        for (int i = 0; i < ship.size; i++)
        {
            switch (ship.Direction)
            {
                case Ship.ShipDirection.EAST:
                    positions[i].Set(pos.x - i, pos.y);
                    break;
                case Ship.ShipDirection.SOUTH:
                    positions[i].Set(pos.x, pos.y - i);
                    break;
                case Ship.ShipDirection.WEST:
                    positions[i].Set(pos.x + i, pos.y);
                    break;
                case Ship.ShipDirection.NORTH:
                    positions[i].Set(pos.x, pos.y + i);
                    break;
                default:
                    break;
            }
        }

        bool result = false;
        // 밖으로 벗어나는지 아닌지
        // 같은 위치에 배가 있는지

        return result;
    }

    //- 전함 배치(가능 여부 확인을 여기서 할 것인가)
    //- 게임 종료 여부
}
