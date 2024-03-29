using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 종류.
/// </summary>
public enum PlayerType
{
    Bank = 0,
    Human,
    CPU1,
    CPU2,
    CPU3
}

public enum PlayerState
{
    TurnStart = 0,  // 턴 시작
    DiceRoll,       // 주사위 굴리기
    RollResult,     // 주사위 굴린 결과 처리
    TurnEnd,        // 턴 종료
    WaitStart,      // 시작대기(플레이어 큐에 들어가 있는 상황)
    WaitPanelResponse   // 패널이 열렸을 때 들어가는 상태
}