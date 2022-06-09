using UnityEngine;

public interface IHealth
{
    /// <summary>
    /// 현재 HP
    /// </summary>
    float HP { get; set; }

    /// <summary>
    /// 최대 HP
    /// </summary>
    float MaxHP { get; }

    /// <summary>
    /// 피격 위치 기록용
    /// </summary>
    Vector3 HitPoint { set; }

    delegate void HealthDelegate();
    HealthDelegate onHealthChange { get; set; }
    HealthDelegate onDead { get; set; }
    HealthDelegate onResurrection { get; set; }

    /// <summary>
    /// 사망 처리
    /// </summary>
    void Dead();
}
