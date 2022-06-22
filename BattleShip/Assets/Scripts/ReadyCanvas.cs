using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReadyCanvas : MonoBehaviour
{
    private void Start()
    {
        Button button = GetComponentInChildren<Button>();
        button.onClick.AddListener(OnGameStart);
    }

    private void OnGameStart()
    {
        SceneManager.LoadScene("Scene_Main");
    }
}
