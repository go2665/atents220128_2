using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class NetworkPlayer : NetworkBehaviour
{    
    NetworkVariable<int> number = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    string playerName;

    Text debugText = null;
    InputField inputField = null;
    Button confirm = null;

    public override void OnNetworkSpawn()
    {
        Debug.Log("Spawn!");
        playerName = $"Player{Random.Range(0, 10000)}";
        this.gameObject.name = playerName;

        debugText = GameObject.Find("DebugText").GetComponent<Text>();
        inputField = GameObject.Find("InputField").GetComponent<InputField>();
        confirm = GameObject.Find("Confirm").GetComponent<Button>();

        if (IsOwner)
        {
            confirm.onClick.AddListener(OnConfirm);
        }
        else
        {
            number.OnValueChanged += OnNumberChange;
        }
    }

    private void OnNumberChange(int previousValue, int newValue)
    {
        debugText.text += $"상대방이 수를 보냈다 : {newValue}\n";
    }

    private void OnConfirm()
    {
        //debugText.text += $"내 이름은 {playerName}\n";
        number.Value = int.Parse(inputField.text);
        debugText.text += $"{inputField.text}을(를) 보냈다.\n";
    }
}
