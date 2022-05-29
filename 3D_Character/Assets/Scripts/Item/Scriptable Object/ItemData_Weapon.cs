using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Scriptable Object/Item Data - Weapon", order = 3)]
public class ItemData_Weapon : ItemData, IEquippableItem
{
    [Header("무기 정보")]
    public float attackPower = 10.0f;

    public void EquipItem(IEquippableCharacter target)
    {
        target.EquipWeapon(this);
    }

    public bool ToggleEquipItem(IEquippableCharacter target)
    {
        bool result = false;
        if (target.IsEquipWeapon())    
        {
            target.UnEquipWeapon();        // 뭐든지 장착하고 있으면 벗고
        }
        else
        {
            target.EquipWeapon(this);      // 장착하고 있지 않으면 장착
            result = true;
        }

        return result;
    }

    public void UnEquipItem(IEquippableCharacter target)
    {
        target.UnEquipWeapon();
    }
}
