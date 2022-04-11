using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    void Update()
    {
        GameManager.Inst.Update(Time.deltaTime);
    }
}
