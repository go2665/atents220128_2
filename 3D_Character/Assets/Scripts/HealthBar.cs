using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // 필요한 데이터
    // 현재HP, 최대HP
    // 변화 속도
    float health = 0.0f;
    float maxHealth = 1.0f;
    public GameObject fill = null;

    // 필요 함수
    // 체력 증감을 표시

    public void SetHeath(float _health)
    {
        health = _health;
        fill.transform.localScale = new Vector3(health/maxHealth, 0.15f, 1.0f);
    }

    public void SetMaxHealth(float _maxHealth)
    {
        maxHealth = _maxHealth;
    }

}
