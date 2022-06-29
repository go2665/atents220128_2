using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HandSelection : byte
{
    None = 0,
    Rock,
    Paper,
    Scissors
}

public enum BattleResult : byte
{
    Draw = 0,
    PlayerWin,
    EnemyWin
}