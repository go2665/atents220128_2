using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageClear : MonoBehaviour
{
    private void Start()
    {
        GameManager.Inst.OnStageStart();
    }

    public void StageClearButton()
    {
        SceneManager.LoadScene(0);        
    }
}
