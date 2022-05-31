using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerTank mainPlayer = null;
    public PlayerTank MainPlayer
    {
        get
        {
            return mainPlayer;
        }
    }

    private static GameManager instance = null;
    public static GameManager Inst
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            instance.Initialize();
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Initialize()
    {
        mainPlayer = FindObjectOfType<PlayerTank>(); 
    }
}
