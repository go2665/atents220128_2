using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadEffect : MonoBehaviour
{
    // 일정 시간 후에 사라지게 만들기
    public float lifeTime = 3.0f;

    private void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }
}
