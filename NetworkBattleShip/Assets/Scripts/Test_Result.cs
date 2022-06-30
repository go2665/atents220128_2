using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Result : MonoBehaviour
{
    private void Start()
    {
        GameManager.Inst.StateChange(GameState.GameOver);
    }
}
