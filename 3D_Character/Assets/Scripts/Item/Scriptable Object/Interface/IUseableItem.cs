using UnityEngine;

/// <summary>
/// 이 인터페이스가 있는 아이템은 사용할 수 있는 아이템이다.
/// </summary>
interface IUseableItem
{
    void Use(GameObject target = null);
}

