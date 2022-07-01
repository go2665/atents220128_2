using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        Debug.Log("Game Manager Initialize");
    }
}
