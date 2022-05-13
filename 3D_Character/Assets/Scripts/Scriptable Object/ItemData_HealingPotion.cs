using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 힐링 포션 아이템용 ItemData. 
/// </summary>
[CreateAssetMenu(fileName = "New Healing Potion", menuName = "Scriptable Object/Item Data - Healing Potion", order = 2)]
public class ItemData_HealingPotion : ItemData, IUseableItem
{
    /// <summary>
    /// 이 아이템을 사용할 경우 대상의 피가 회복된다.
    /// </summary>
    /// <param name="target">아이템의 효과를 받을 대상</param>
    public void Use(GameObject target = null)
    {
        Debug.Log($"{this.itemName}을(를) 사용했습니다");
    }
}
