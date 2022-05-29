public interface IEquippableCharacter
{
    ItemData_Weapon EquipItem { get; }
    public void EquipWeapon(ItemData_Weapon weapon);

    public void UnEquipWeapon();

    public bool IsEquipWeapon();
}