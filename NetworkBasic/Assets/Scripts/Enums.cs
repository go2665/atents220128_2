using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 가위 바위 보 용
/// </summary>
public enum HandSelection : byte
{
    None = 0,
    Rock,
    Paper,
    Scissors
}

/// <summary>
/// 승리, 패배, 무승부 용
/// </summary>
public enum BattleResult : byte
{
    Draw = 0,
    PlayerWin,
    EnemyWin
}