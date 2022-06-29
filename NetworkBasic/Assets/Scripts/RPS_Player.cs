using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

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

    //public void HandSelect(HandSelection hand)
    //{
    //    selection = hand;
    //}

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
        RPS_GameManager.Inst.SetOpponentText(select);
    }

    [ServerRpc]
    void SubmitSelectRequestServerRpc(HandSelection hand)
    {
        netSelect.Value = hand;
    }
}
