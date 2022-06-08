using UnityEngine;

public interface IHealth
{
    float HP { get; set; }
    float MaxHP { get; }

    Vector3 HitPoint { set; }

    delegate void HealthDelegate();
    HealthDelegate onHealthChange { get; set; }

    void Dead();
}
