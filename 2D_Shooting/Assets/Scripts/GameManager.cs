using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MemoryPool shootMemoryPool = null;    

    private static GameManager instance = null;  // 싱글톤은 나중에 제거할 부분(GameManager만들어진 후)
    public static GameManager Inst
    {
        get
        {
            return instance;
        }
    }

    //생성 직후 한번만 호출
    private void Awake()
    {
        if (instance == null)   // 제일 처음 만들어진 인스턴스다.
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // 다른 씬이 로드되더라도 삭제되지 않는다.
        }
        else
        {
            // 이미 인스턴스가 만들어진게 있다.
            if (instance != this)           // 이미 만들어진 것이 나와 다르다.
            {
                Destroy(this.gameObject);   //나는 죽는다.
            }
        }
    }

    public GameObject GetShootObject()
    {
        return shootMemoryPool.GetObject();
    }
}
