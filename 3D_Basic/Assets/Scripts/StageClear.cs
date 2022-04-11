using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageClear : MonoBehaviour
{
    public void StageClearButton()
    {
        SceneManager.LoadScene(0);        
    }
}
