using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Scriptable Object/Item Data - Weapon", order = 3)]
public class ItemData_Weapon : ItemData, IUseableItem, IEquipableItem
{
    [Header("무기 정보")]
    public float attackPower = 10.0f;

    public void Use(GameObject target = null)
    {
        Debug.Log($"프레이어에게 {this.itemName} 장비");
    }
}
