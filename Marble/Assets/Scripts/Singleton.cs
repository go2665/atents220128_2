using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inherit from this base class to create a singleton.
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // Check to see if we're about to be destroyed
    private static bool shuttingDown = false;       // 이미 삭제되는 중인데 사용하려고 할 경우 null을 리턴하기 위해 사용하는 변수
    //private static object locking = new object();
    private static T instance;

    /// <summary>
    /// 싱글톤 인스턴스에 접근하기 위한 프로퍼티(읽기전용)
    /// </summary>
    public static T Inst
    {
        get
        {
            if (shuttingDown)
            {
                Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed. Returning null.");
                return null;
            }

            //lock (locking)  // 락이 걸린동안 여기에 다른 쓰레드는 접근 못함(문제는 많이 느림. 그래서 일단 제거. 멀티 쓰레딩으로 구현할 경우에는 꼭 있어야 함)
            //{

            if (instance == null)
            {
                // 해당 컴포넌트가 씬에 있는지 찾기
                instance = (T)FindObjectOfType(typeof(T));

                // 찾았는데 없는 경우
                if (instance == null)
                {
                    MakeSingletonObject();                    
                }
            }

            return instance;
            //}
        }
    }

    /// <summary>
    /// 싱글톤 오브젝트 생성
    /// </summary>
    private static void MakeSingletonObject()
    {
        // 새 오브젝트 만들고 컴포넌트도 새로 만들어서 추가해 줌
        var singletonObject = new GameObject();
        instance = singletonObject.AddComponent<T>();
        singletonObject.name = $"{typeof(T).ToString()} (Singleton)";   // 이름도 변경

        // 오브젝트가 삭제되지 않도록 처리
        DontDestroyOnLoad(singletonObject);     
    }

    /// <summary>
    /// 프로그램이 종료될 때 실행되는 함수
    /// </summary>
    private void OnApplicationQuit()
    {
        shuttingDown = true;
    }

    /// <summary>
    /// Destroy로 이 게임오브젝트를 삭제할 때 실행되는 함수
    /// </summary>
    private void OnDestroy()
    {
        shuttingDown = true;
    }
}