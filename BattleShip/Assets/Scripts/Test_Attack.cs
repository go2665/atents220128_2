using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Attack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Inst.FieldLeft.RandomDeployment();
        GameManager.Inst.FieldRight.RandomDeployment();
        GameManager.Inst.StateChange(GameState.Battle);
    }

}
