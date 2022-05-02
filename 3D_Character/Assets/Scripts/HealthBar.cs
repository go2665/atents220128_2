using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject fill = null;
    IHealth health = null;
    
    private void Start()
    {
        health = GetComponentInParent<IHealth>();
        //health.onHealthChange = HealthChange;     // 기존에 있던 함수들을 무시하고 지금 대입한 함수만 들어감
        health.onHealthChange += HealthChange;      // 기존에 들어간 함수들이 수행되고 난 후 지금 대입한 함수가 실행됨
    }

    // 체력 증감을 표시
    private void HealthChange()
    {
        fill.transform.localScale = new Vector3(health.HP / health.MaxHP, 0.15f, 1.0f);
    }
}
