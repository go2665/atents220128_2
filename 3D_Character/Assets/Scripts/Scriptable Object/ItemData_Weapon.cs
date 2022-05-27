using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Scriptable Object/Item Data - Weapon", order = 3)]
public class ItemData_Weapon : ItemData, IEquipableItem
{
    [Header("무기 정보")]
    public float attackPower = 10.0f;

    public void EquipItem(ItemData_Weapon weapon)
    {
        if (weapon != this)
        {
            GameManager.Inst.MainPlayer.EquipWeapon(weapon);
        }
    }

    public bool ToggleEquipItem()
    {
        bool result = false;
        if (GameManager.Inst.MainPlayer.IsEquipWeapon())
        {
            GameManager.Inst.MainPlayer.UnEquipWeapon();
        }
        else
        {
            GameManager.Inst.MainPlayer.EquipWeapon(this);
            result = true;
        }

        return result;
    }

    public void UnEquipItem()
    {
        GameManager.Inst.MainPlayer.UnEquipWeapon();
    }
}
