using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RPS_Player : NetworkBehaviour
{
    NetworkVariable<HandSelection> netSelect = new NetworkVariable<HandSelection>(HandSelection.None);

    HandSelection selection = HandSelection.None;
    public HandSelection Selection
    {
        get => selection;
        set
        {
            selection = value;
            SubmitSelectRequestServerRpc(selection);
        }
    }

    public override void OnNetworkSpawn()
    {
        if ( IsOwner )
        {
            RPS_GameManager.Inst.Player = this;
        }
        else
        {
            RPS_GameManager.Inst.Enemy = this;  // ??? 애매. 확인 필요
        }

        netSelect.OnValueChanged += OnSelectChange;
    }

    private void OnSelectChange(HandSelection previousValue, HandSelection newValue)
    {
        if (!IsOwner)
        {
            string select = "";
            switch (newValue)
            {
                case HandSelection.Rock:
                    select = "바위";
                    break;
                case HandSelection.Paper:
                    select = "보";
                    break;
                case HandSelection.Scissors:
                    select = "가위";
                    break;
                case HandSelection.None:
                default:
                    break;
            }
            selection = newValue;
            RPS_GameManager.Inst.SetOpponentText(select);
        }

        if (RPS_GameManager.Inst.IsBothComplete())
        {
            BattleResult result = RPS_GameManager.Inst.IsPlayerWin();

            switch (result)
            {
                case BattleResult.Draw:
                    RPS_GameManager.Inst.SetResultText("무승부");
                    break;
                case BattleResult.PlayerWin:
                    RPS_GameManager.Inst.SetResultText("승리");
                    break;
                case BattleResult.EnemyWin:
                    RPS_GameManager.Inst.SetResultText("패배");
                    break;
                default:
                    break;
            }
        }
    }

    [ServerRpc]
    void SubmitSelectRequestServerRpc(HandSelection hand)
    {
        netSelect.Value = hand;
        selection = hand;
    }
}
