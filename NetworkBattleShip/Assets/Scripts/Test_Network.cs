using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Test_Network : MonoBehaviour
{
    private void Start()
    {
        GameManager.Inst.StateChange(GameState.Battle);
        GameManager.Inst.FieldLeft.RandomDeployment();
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 150, 150));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            if (GUILayout.Button("Host"))
                NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client"))
                NetworkManager.Singleton.StartClient();
        }
        GUILayout.EndArea();
    }
}
