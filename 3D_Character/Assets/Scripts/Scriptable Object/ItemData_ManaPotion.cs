using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 마나 포션 아이템용 ItemData. 
/// </summary>
[CreateAssetMenu(fileName = "New Mana Potion", menuName = "Scriptable Object/Item Data - Mana Potion", order = 3)]
public class ItemData_ManaPotion : ItemData, IUseableItem
{
    public float manaRecoveryPoint = 15.0f;
    /// <summary>
    /// 이 아이템을 사용할 경우 대상의 마나가 회복된다.
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

        IMana mana = target.GetComponent<IMana>();
        if (mana != null)
        {
            mana.MP += manaRecoveryPoint;
            Debug.Log($"{this.itemName}을(를) {name}에게 사용해서 MP를 {manaRecoveryPoint}만큼 회복을 시도합니다.");
        }
        else
        {
            Debug.Log($"{this.itemName}은(는) {name}에게 효과가 없습니다.");
        }
    }
}
