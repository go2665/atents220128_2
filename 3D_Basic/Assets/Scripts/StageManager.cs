using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void StageClearButton()
    {
        SceneManager.LoadScene(0);
    }
}
