using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using UnityEngine.UI;

public class NetworkPlayer : NetworkBehaviour
{
    // NetCode에서 데이터를 넘겨주는 방법
    // 1. NetworkVariable을 직접 변경한다. -> 중요하고 오래가는 값들 넘겨주기 용
    // 2. ServerRPC -> 일시적인 값을 넘겨주기 용

    NetworkVariable<int> number = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    NetworkVariable<int> numberRPC = new NetworkVariable<int>(0);
    NetworkVariable<FixedString32Bytes> playerName = 
        new(new("Player"), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    Text debugText = null;
    InputField inputField = null;
    Button confirm = null;

    public override void OnNetworkSpawn()
    {
        Debug.Log("Spawn!");

        debugText = GameObject.Find("DebugText").GetComponent<Text>();
        inputField = GameObject.Find("InputField").GetComponent<InputField>();
        confirm = GameObject.Find("Confirm").GetComponent<Button>();

        if (IsOwner)    // 소유자가 자신인지 확인
        {
            playerName.Value = $"Player{Random.Range(0, 10000)}";
            this.gameObject.name = playerName.Value.ToString();
            confirm.onClick.AddListener(OnConfirm);
        }
        else
        {
            number.OnValueChanged += OnNumberChange;
        }
    }

    private void OnNumberChange(int previousValue, int newValue)
    {
        debugText.text += $"{playerName.Value}이 수를 보냈다 : {newValue}, {numberRPC.Value}\n";
    }

    private void OnConfirm()
    {
        //debugText.text += $"내 이름은 {playerName}\n";
        number.Value = int.Parse(inputField.text);
        SubmitNumberRPC_RequestServerRpc(int.Parse(inputField.text));

        debugText.text += $"{inputField.text}을(를) 보냈다.\n";
    }

    /// <summary>
    /// 서버RPC. 함수 앞에 [ServerRpc] 속성 붙여야 함. 함수이름의 뒤부분은 ServerRpc로 끝나야 한다.
    /// </summary>
    /// <param name="num"></param>
    [ServerRpc]
    void SubmitNumberRPC_RequestServerRpc(int num)
    {
        numberRPC.Value = num;
    }
}
