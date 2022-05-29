using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 힐링 포션 아이템용 ItemData. 
/// </summary>
[CreateAssetMenu(fileName = "New Healing Potion", menuName = "Scriptable Object/Item Data - Healing Potion", order = 2)]
public class ItemData_HealingPotion : ItemData, IUseableItem
{
    float healRecoveryPoint = 20.0f;

    /// <summary>
    /// 이 아이템을 사용할 경우 대상의 피가 회복된다.
    /// </summary>
    /// <param name="target">아이템의 효과를 받을 대상</param>
    public void Use(GameObject target = null)
    {
        string name;
        if (target != null)
        {
            name = target.name;
        }
        else
        {
            name = "Player";
            target = GameManager.Inst.MainPlayer.gameObject;
        }

        IHealth health = target.GetComponent<IHealth>();
        if (health != null)
        {
            health.HP += healRecoveryPoint;
            Debug.Log($"{this.itemName}을(를) {name}에게 사용해서 HP를 {healRecoveryPoint}만큼 회복을 시도합니다.");
        }
        else
        {
            Debug.Log($"{this.itemName}은(는) {name}에게 효과가 없습니다.");
        }
    }
}
