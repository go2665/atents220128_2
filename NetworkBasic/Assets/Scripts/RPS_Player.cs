using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RPS_Player : NetworkBehaviour
{
    /// <summary>
    /// 네트워크에서 서로 공유하기 위한 변수
    /// </summary>
    NetworkVariable<HandSelection> netSelect = new NetworkVariable<HandSelection>(HandSelection.None);

    /// <summary>
    /// 내가 선택한 가위바위보 중 하나
    /// </summary>
    HandSelection selection = HandSelection.None;
    public HandSelection Selection
    {
        get => selection;
        set
        {
            selection = value;
            SubmitSelectRequestServerRpc(selection);    // selection에 값을 설정하면 서버 RPC 요청
        }
    }

    /// <summary>
    /// 네트워크에 연결되었을 때 실행이 되는 함수
    /// </summary>
    public override void OnNetworkSpawn()
    {
        if ( IsOwner )  // 소유주인지 아닌지 확인
        {
            RPS_GameManager.Inst.Player = this; // 소유주면 player
        }
        else
        {
            RPS_GameManager.Inst.Enemy = this;  // 아니면 적(1:1 게임이니까)
        }

        netSelect.OnValueChanged += OnSelectChange; // netSelect값이 변경될 때 실행될 함수 연결
    }

    /// <summary>
    /// netSelect값이 변경될 때 실행될 함수
    /// </summary>
    /// <param name="previousValue">이전값</param>
    /// <param name="newValue">변경되는 값</param>
    private void OnSelectChange(HandSelection previousValue, HandSelection newValue)
    {
        if (!IsOwner)   // 오너가 아니다 => 적
        {
            selection = newValue;                           // enemy의 selection 변경
            RPS_GameManager.Inst.SetOpponentText(newValue); // 찍히는 글자도 변경
        }

        if (RPS_GameManager.Inst.IsBothComplete())          // 둘 다 선택을 했는지 확인
        {
            RPS_GameManager.Inst.SetBattleResultText();     // 플레이어의 결과를 받아서 승패무 표시
        }
    }

    /// <summary>
    /// 서버에서 실행하는 코드. 해당 플레이어의 손 모양을 변경
    /// </summary>
    /// <param name="hand">선택한 손모양</param>
    [ServerRpc]
    void SubmitSelectRequestServerRpc(HandSelection hand)
    {
        netSelect.Value = hand;
        selection = hand;
    }
}
