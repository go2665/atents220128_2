using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.Inst.OnStageStart();
    }

    void Update()
    {
        GameManager.Inst.Update(Time.deltaTime);
    }
}
