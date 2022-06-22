using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 배의 종류를 나타낼 enum
/// </summary>
public enum ShipType : byte
{
    Ship5 = 0,      // 사이즈5
    Ship4 = 1,      // 사이즈4
    Ship3_1 = 2,    // 사이즈3-1
    Ship3_2 = 3,    // 사이즈3-2
    Ship2 = 4,      // 사이즈2    
    SizeOfShipType
}

/// <summary>
/// BattleField의 상태를 나타낼 enum
/// </summary>
public enum FieldState
{
    Ready = 0,                  // 대기 모드(처음 시작했을 때)
    ShipDeployment,             // 함선 배치모드
    ShipDeployment_HoldShip,    // 함선 배치모드(배를 들고 있는 상태)
    GamePlay                    // 게임 플레이 모드
}