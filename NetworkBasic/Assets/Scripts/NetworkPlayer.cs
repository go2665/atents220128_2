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

        confirm.onClick.AddListener(OnConfirm);
    }

    private void OnConfirm()
    {
        debugText.text += $"내 이름은 {playerName}\n";
    }
}
