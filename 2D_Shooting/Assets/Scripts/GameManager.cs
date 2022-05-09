using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MemoryPool shootMemoryPool = null;
    public MemoryPool enemyMemoryPool = null;
    public MemoryPool explosionMemoryPool = null;
    public MemoryPool asteroidBigMemoryPool = null;
    public MemoryPool asteroidSmallMemoryPool = null;

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
            instance.Initialize();
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

    private void Initialize()
    {
        shootMemoryPool = transform.Find("ShootMemoryPool").gameObject.GetComponent<MemoryPool>();
        enemyMemoryPool = transform.Find("EnemyMemoryPool").gameObject.GetComponent<MemoryPool>();
        explosionMemoryPool = transform.Find("ExplosionMemoryPool").gameObject.GetComponent<MemoryPool>();
        asteroidBigMemoryPool = transform.Find("AsteroidBigMP").gameObject.GetComponent<MemoryPool>();
        asteroidSmallMemoryPool = transform.Find("AsteroidSmallMP").gameObject.GetComponent<MemoryPool>();
    }

    public GameObject GetShootObject()
    {
        return shootMemoryPool.GetObject();
    }

    public GameObject GetEnemyObject()
    {
        return enemyMemoryPool.GetObject();
    }

    public GameObject GetEnemyObject(Vector2 pos, Quaternion rot)
    {
        return enemyMemoryPool.GetObject(pos, rot);
    }

    public GameObject GetExplosionObject()
    {
        return explosionMemoryPool.GetObject();
    }

    public GameObject GetAsteroidBigObject()
    {
        return asteroidBigMemoryPool.GetObject();
    }

    public GameObject GetAsteroidBigObject(Vector2 pos, Quaternion rot)
    {
        return asteroidBigMemoryPool.GetObject(pos, rot);
    }

    public GameObject GetAsteroidSmallObject()
    {
        return asteroidSmallMemoryPool.GetObject();
    }

    public GameObject GetAsteroidSmallObject(Vector2 pos, Quaternion rot)
    {
        return asteroidSmallMemoryPool.GetObject(pos, rot);
    }
}
