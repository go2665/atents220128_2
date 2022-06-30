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
/// 현재 게임 상태
/// </summary>
public enum GameState
{
    Ready = 0,
    ShipDeployment,
    Battle,
    GameOver
}

// 게임시작 -> Ready상태 -> 다음 버튼 누르기 -> 함선배치 상태 -> 내 배치가 끝나면 자동으로 적 함선배치 -> 전투 시작(턴 진행) -> 한쪽이 죽으면 GameOver 상태