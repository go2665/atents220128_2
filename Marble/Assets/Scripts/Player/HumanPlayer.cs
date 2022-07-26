using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어(인간, 컴퓨터, 은행 포함)
/// </summary>
public class HumanPlayer : Player
{
   

    protected override void Update_TurnStart()
    {
        base.Update_TurnStart();
    }

    protected override void Update_DiceRoll()
    {
        base.Update_DiceRoll();
    }

    protected override void Update_RollResult()
    {
        base.Update_RollResult();
    }

    protected override void Update_TurnEnd()
    {
        base.Update_TurnEnd();
    }

    protected override void Update_WaitStart()
    {
        base.Update_WaitStart();
    }

}
