using UnityEngine;
using UnityEngine.UI;

public class Canvas_Ready : MonoBehaviour
{
    private void Start()
    {
        Button button = GetComponentInChildren<Button>();
        button.onClick.AddListener(OnGameStart);
    }

    private void OnGameStart()
    {
        GameManager.Inst.StateChange(GameState.ShipDeployment);
    }
}
